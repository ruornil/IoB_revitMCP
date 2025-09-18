async function mcp(endpoint, body) {
  const res = await fetch(endpoint, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(body)
  });
  if (!res.ok) throw new Error(`MCP HTTP ${res.status}`);
  return await res.json();
}

function qs(id) { return document.getElementById(id); }
const palette = ['#4f8cff','#7c5cff','#3ad1bf','#ffa24f','#e95f86','#35b46f','#b480ff','#ffcd56','#66e1ff','#f78c6c'];
function color(idx){ return palette[idx % palette.length]; }
function cssVar(name){ return getComputedStyle(document.documentElement).getPropertyValue(name).trim(); }

let charts = {}; // echarts instances by id
let chatFrame = null; // iframe reference for chat
let sessionId = null; // dashboard session id shared with chat
let lastEventId = 0; // last processed ui_events.id
let modelPollTimer = null; // periodic refresh of model list

function upsertChart(id, option, onEvents){
  const el = qs(id);
  if (!el) return;
  let chart = charts[id];
  if (!chart){ chart = echarts.init(el); charts[id] = chart; }
  chart.setOption(option, true);
  if (onEvents){
    // remove old handlers by disposing and re-init for simplicity
    chart.off('click');
    if (onEvents.click) chart.on('click', onEvents.click);
  }
  window.addEventListener('resize', () => chart.resize());
}

// Global filter state for cross-filtering
// Support multi-category via categories; keep category for backward compat (single)
const filterState = { categories: [], category: null, level: null, family: null, type: null, selectionIds: null };
const pager = { page: 1, pageSize: 25, total: 0 };
const paramsPager = { page: 1, pageSize: 25, total: 0 };
let selectedParamCols = [];
let unitsMode = 'imperial'; // or 'metric'
let miniAbort = null; // AbortController for mini chat

// Category denylist config (loaded from file or localStorage)
let hiddenCategories = null;
const defaultHiddenCategories = [
  'Levels','Views','Viewports','Sheets','Schedules','Project Information',
  'Grids','Reference Planes','Title Blocks','Revision Clouds','Text Notes',
  'Dimensions','Tags'
];
async function loadDenyList(){
  // 1) Try localStorage override
  try {
    const raw = localStorage.getItem('mcp_hidden_categories');
    if (raw){ const arr = JSON.parse(raw); if (Array.isArray(arr)) { hiddenCategories = arr; return; } }
  } catch {}

  // 2) Try embedded script tag (optional)
  try {
    const el = document.getElementById('category-denylist');
    if (el && el.textContent){ const arr = JSON.parse(el.textContent); if (Array.isArray(arr)) { hiddenCategories = arr; return; } }
  } catch {}

  // 3) If served via http/https, try fetching the JSON file. Skip under file:// to avoid CORS console noise.
  try {
    const proto = (location && location.protocol || '').toLowerCase();
    if (proto === 'http:' || proto === 'https:'){
      const res = await fetch('./category-denylist.json', { cache: 'no-store' });
      if (res.ok){ const arr = await res.json(); if (Array.isArray(arr)) { hiddenCategories = arr; return; } }
    }
  } catch {}

  // 4) Fallback to built-in defaults
  hiddenCategories = defaultHiddenCategories.slice();
}
function sqlHideCategories(columnName){
  const cats = hiddenCategories;
  if (!cats || !cats.length) return '';
  const list = cats.map(c=> "'" + String(c).replace(/'/g, "''") + "'").join(',');
  return ` AND ${columnName} NOT IN (${list})`;
}

function filtersText(){
  const f = [];
  const cats = (filterState.categories && filterState.categories.length) ? filterState.categories : (filterState.category ? [filterState.category] : []);
  if (cats.length) f.push(`Category=${cats.join(', ')}`);
  if (filterState.level) f.push(`Level=${filterState.level}`);
  if (filterState.family) f.push(`Family=${filterState.family}`);
  if (filterState.type) f.push(`Type=${filterState.type}`);
  return f.length ? f.join(' | ') : '(none)';
}

async function loadModels() {
  const endpoint = qs('endpoint').value.trim();
  const resp = await mcp(endpoint, { action: 'Db.Query', sql: 'SELECT doc_id, model_name, last_saved FROM model_info ORDER BY COALESCE(last_saved, CURRENT_TIMESTAMP) DESC' });
  const data = resp.results || resp.rows || resp.result || resp.data || [];
  const sel = qs('model');
  const prev = sel.value;
  sel.innerHTML = '';
  for (const row of data) {
    const opt = document.createElement('option');
    opt.value = row.doc_id;
    opt.textContent = row.model_name ? `${row.model_name} â€” ${row.doc_id}` : row.doc_id;
    sel.appendChild(opt);
  }
  // Preserve previous selection if still available; otherwise select first
  if (prev && Array.from(sel.options).some(o => o.value === prev)) {
    sel.value = prev;
  } else if (sel.options.length) {
    sel.selectedIndex = 0;
  }
}

async function loadCharts() {
  const endpoint = qs('endpoint').value.trim();
  const doc = qs('model').value;
  if (!doc) return;
  qs('filtersText').textContent = filtersText();
  // Reset parameter pager on filter changes
  paramsPager.page = 1;

  const qsParams = (obj) => JSON.stringify(obj);
  // Build WHERE fragments based on active filters (support multi-category)
  const catList = (filterState.categories && filterState.categories.length) ? filterState.categories : (filterState.category ? [filterState.category] : []);
  const catFilter = catList.length ? (' AND category IN (' + catList.map(c => "'" + String(c).replace(/'/g, "''") + "'").join(',') + ')') : '';  const lvlFilter = filterState.level ? ' AND level=@lvl' : '';
  const typeFilter = filterState.type ? ' AND type_name=@typ' : '';
  const familyInFilter = filterState.family ? (' AND type_name IN (SELECT type_name FROM revit_elementTypes WHERE doc_id=@doc' + (catList.length ? (' AND category IN (' + catList.map(c => "'" + String(c).replace(/'/g, "''") + "'").join(',') + ')') : '') + ' AND family=@fam)') : '';

  const hideFilterCharts = (hiddenCategories && hiddenCategories.length) ? (' AND category NOT IN (' + hiddenCategories.map(c=>"'"+String(c).replace(/'/g,"''")+"'").join(',') + ')') : '';
  const queries = {
    byCat: {
      action: 'Db.Query',
      sql: `SELECT COALESCE(category,'(none)') AS category, COUNT(*) AS c FROM revit_elements WHERE doc_id=@doc${lvlFilter}${typeFilter}${familyInFilter}${catFilter}${hideFilterCharts} GROUP BY category ORDER BY c DESC LIMIT 20`,
      params: qsParams({ '@doc': doc, ...(filterState.level ? { '@lvl': filterState.level } : {}), ...(filterState.type ? { '@typ': filterState.type } : {}), ...(filterState.family ? { '@fam': filterState.family } : {}) })
    },
    byLevel: {
      action: 'Db.Query',
      sql: "SELECT COALESCE(level, '(none)') AS level, COUNT(*) AS c FROM revit_elements WHERE doc_id=@doc" + catFilter + hideFilterCharts + typeFilter + familyInFilter + ' GROUP BY level ORDER BY c DESC',
      params: qsParams({ '@doc': doc, ...(filterState.type ? { '@typ': filterState.type } : {}), ...(filterState.family ? { '@fam': filterState.family } : {}) })
    },
    typesByCat: {
      action: 'Db.Query',
      sql: `SELECT COALESCE(category,'(none)') AS category, COUNT(DISTINCT type_name) AS c FROM revit_elements WHERE doc_id=@doc${lvlFilter}${typeFilter}${familyInFilter}${catFilter}${hideFilterCharts} GROUP BY category ORDER BY c DESC LIMIT 20`,
      params: qsParams({ '@doc': doc, ...(filterState.level ? { '@lvl': filterState.level } : {}), ...(filterState.type ? { '@typ': filterState.type } : {}), ...(filterState.family ? { '@fam': filterState.family } : {}) })
    }
  };

  const [byCat, byLevel, typesByCat] = await Promise.all(
    Object.values(queries).map(q => mcp(endpoint, q))
  );

  const rows = r => (r.results || r.rows || r.result || r.data || []);

  // Families/Types panel
  await loadFamilyTypePanel();

  // Elements by Level
  (function() {
    const r = rows(byLevel);
    const labels = r.map(x => x.level || '(none)');
    const data = r.map(x => Number(x.c));
    const option = {
      grid:{left:60,right:10,top:10,bottom:30},
      xAxis:{ type:'value', axisLabel:{ color:'#9aa0b4' } },
      yAxis:{ type:'category', data:labels, axisLabel:{ color:'#9aa0b4' } },
      series:[{ type:'bar', data: data.map((v,i)=>({ value:v, itemStyle:{ color:color(i) } })), animationDuration:300 }],
      tooltip:{ trigger:'item' }
    };
    upsertChart('levelChart', option, {
      click: (params)=>{
        if (!params || params.componentType!=='series') return;
        const val = labels[params.dataIndex];
        filterState.level = (filterState.level===val) ? null : val;
        loadCharts();
      }
    });
  })();

  // Types by Category
  (function() {
    const r = rows(typesByCat);
    const labels = r.map(x => x.category || '(none)');
    const data = r.map(x => Number(x.c));
    const textColor = cssVar('--text') || '#3f3b36';
    const option = {
      tooltip:{ trigger:'item' },
      legend:{ top:0, type:'scroll', textStyle:{ color:textColor } },
      series:[{
        name:'Types', type:'pie', radius:['40%','70%'], center:['50%','62%'],
        avoidLabelOverlap: true,
        label: { color:textColor, overflow:'truncate', width:130 },
        labelLine: { length:10, length2:10 },
        data: labels.map((l,i)=>({ name:l, value:data[i], itemStyle:{ color:color(i) } }))
      }]
    };
    upsertChart('typesChart', option, {
      click: (params)=>{
        const val = params?.name;
        if (!val) return;
        const now = new Set(filterState.categories && filterState.categories.length ? filterState.categories : (filterState.category ? [filterState.category] : []));
        if (now.has(val)) now.delete(val); else { now.clear(); now.add(val); }
        filterState.categories = Array.from(now);
        filterState.category = null;
        const sel = qs('ftCategory'); if (sel){ Array.from(sel.options).forEach(o=> o.selected = filterState.categories.includes(o.value)); }
        loadCharts();
      }
    });
  })();

  // Removed: Top Parameters and Views charts per request
  // Update details and parameters after charts and insight
  await loadDetails();
}

// Families and Types by Category panel
async function loadFamilyTypePanel(){
  const endpoint = qs('endpoint').value.trim();
  const doc = qs('model').value;
  if (!doc) return;
  const qsParams = (obj) => JSON.stringify(obj);

  // Load categories for select (from elementTypes) with denylist + instances only
  const hideFilterFT = (hiddenCategories && hiddenCategories.length)
    ? (' AND t.category NOT IN (' + hiddenCategories.map(c=>"'"+String(c).replace(/'/g,"''")+"'").join(',') + ')') : '';
  const catsSql = "SELECT t.category, COUNT(DISTINCT t.family) AS c FROM revit_elementTypes t WHERE t.doc_id=@doc"
    + hideFilterFT + " AND EXISTS (SELECT 1 FROM revit_elements e WHERE e.doc_id=@doc AND e.category=t.category)"
    + " GROUP BY t.category ORDER BY c DESC";
  const catsResp = await mcp(endpoint, { action:'Db.Query', sql: catsSql, params: qsParams({ '@doc': doc }) });
  const cats = (catsResp.results || catsResp.rows || catsResp.data || []).map(r=>r.category);
  const sel = qs('ftCategory');
  if (sel){
    sel.innerHTML = '';
    // Removed the implicit "(all)" option; Clear Family now resets filters
    for (const c of cats){ const o=document.createElement('option'); o.value=c; o.textContent=c; sel.appendChild(o); }
    const selected = (filterState.categories && filterState.categories.length) ? filterState.categories : (filterState.category ? [filterState.category] : []);
    Array.from(sel.options).forEach(o => o.selected = selected.includes(o.value));
  }

  // Families for current category (or all)
  const catList = (filterState.categories && filterState.categories.length) ? filterState.categories : (filterState.category ? [filterState.category] : []);
  const famWhere = (catList.length ? ('WHERE t.doc_id=@doc AND t.category IN (' + catList.map(c=>"'"+String(c).replace(/'/g,"''")+"'").join(',') + ')') : 'WHERE t.doc_id=@doc')
    + hideFilterFT + ' AND EXISTS (SELECT 1 FROM revit_elements e WHERE e.doc_id=@doc AND e.type_name=t.type_name)';
  const famResp = await mcp(endpoint, { action:'Db.Query', sql: `SELECT t.family, COUNT(DISTINCT t.type_name) AS c FROM revit_elementTypes t ${famWhere} GROUP BY t.family ORDER BY c DESC LIMIT 200`, params: qsParams({ '@doc': doc }) });
  const fams = (famResp.results || famResp.rows || famResp.data || []);
  const famList = qs('familyList');
  if (famList){
    famList.innerHTML='';
    for (const f of fams){
      const div = document.createElement('div');
      div.className = 'list-item' + (filterState.family===f.family ? ' active':'');
      div.setAttribute('data-name', f.family || '');
      div.innerHTML = `<span>${escapeHtml(f.family||'(none)')}</span><span class="badge">${f.c}</span>`;
      famList.appendChild(div);
    }
  }

  // Types for selected family (and category)
  const typeList = qs('typeList');
  if (typeList){
    typeList.innerHTML='';
    if (filterState.family){
      const typeWhere = 'WHERE t.doc_id=@doc' + (catList.length ? (' AND t.category IN (' + catList.map(c=>"'"+String(c).replace(/'/g,"''")+"'").join(',') + ')') : '') + ' AND t.family=@fam'
        + hideFilterFT + ' AND EXISTS (SELECT 1 FROM revit_elements e WHERE e.doc_id=@doc AND e.type_name=t.type_name)';
      const typeResp = await mcp(endpoint, { action:'Db.Query', sql: `SELECT t.type_name, COUNT(*) AS c FROM revit_elementTypes t ${typeWhere} GROUP BY t.type_name ORDER BY c DESC LIMIT 400`, params: qsParams({ '@doc': doc, '@fam': filterState.family }) });
      const types = (typeResp.results || typeResp.rows || typeResp.data || []);
      for (const t of types){
        const div = document.createElement('div');
        div.className='list-item' + (filterState.type===t.type_name ? ' active':'');
        div.setAttribute('data-name', t.type_name || '');
        div.innerHTML = `<span>${escapeHtml(t.type_name||'(none)')}</span><span class="badge">${t.c}</span>`;
        typeList.appendChild(div);
      }
    }
  }
}

async function init() {
  // Generate or reuse a session id to correlate chat â†” dashboard
  sessionId = (localStorage.getItem('mcp_dash_session')) || (self.crypto?.randomUUID ? crypto.randomUUID() : String(Date.now()) + Math.random().toString(16).slice(2));
  localStorage.setItem('mcp_dash_session', sessionId);
  // Load category denylist before initial render
  try { await loadDenyList(); } catch {}
  qs('refresh').addEventListener('click', async () => {
  try { await loadDenyList(); } catch {}
    await flushUiEventsSafe();
    await loadModels();
    await loadCharts();
  });
  qs('endpoint').addEventListener('change', loadModels);
  if (qs('chatProvider')) setupChatControls();
  // filters UI
  qs('clearFilters').addEventListener('click', ()=>{ filterState.category=null; filterState.level=null; filterState.family=null; filterState.type=null; filterState.selectionIds=null; pager.page=1; loadCharts(); });
  qs('model').addEventListener('change', ()=>{ filterState.category=null; filterState.level=null; filterState.family=null; filterState.type=null; filterState.selectionIds=null; pager.page=1; lastEventId=0; loadCharts(); });
  // Families & Types controls
  document.addEventListener('click', (ev)=>{
    // delegate item clicks
    const famItem = ev.target.closest && ev.target.closest('#familyList .list-item');
    if (famItem){
      const name = famItem.getAttribute('data-name');
      filterState.family = (filterState.family===name) ? null : name;
      filterState.type = null;
      pager.page=1; loadCharts();
      return;
    }
    const typItem = ev.target.closest && ev.target.closest('#typeList .list-item');
    if (typItem){
      const name = typItem.getAttribute('data-name');
      filterState.type = (filterState.type===name) ? null : name;
      pager.page=1; loadCharts();
      return;
    }
  });
  // Clear buttons
  const cf = qs('clearFamily'); if (cf) cf.addEventListener('click', ()=>{
    filterState.family = null;
    filterState.type = null;
    // Also clear category filters to emulate previous "(all)" behavior
    filterState.categories = [];
    filterState.category = null;
    // Clear selection UI for category multi-select
    const csel = qs('ftCategory'); if (csel) { Array.from(csel.options).forEach(o => o.selected = false); }
    pager.page = 1;
    loadCharts();
  });
  const ct = qs('clearType'); if (ct) ct.addEventListener('click', ()=>{ filterState.type=null; pager.page=1; loadCharts(); });
  const csel = qs('ftCategory'); if (csel) csel.addEventListener('change', ()=>{
    const selected = Array.from(csel.selectedOptions).map(o=>o.value).filter(v=>v);
    filterState.categories = selected;
    filterState.category = null;
    filterState.family=null; filterState.type=null; pager.page=1; loadCharts();
  });
  qs('pageSize').addEventListener('change', ()=>{ pager.pageSize = parseInt(qs('pageSize').value,10)||25; pager.page=1; loadDetails(); });
  qs('prevPage').addEventListener('click', ()=>{ if (pager.page>1){ pager.page--; loadDetails(); } });
  qs('nextPage').addEventListener('click', ()=>{ const maxPage = Math.max(1, Math.ceil(pager.total / pager.pageSize)); if (pager.page < maxPage){ pager.page++; loadDetails(); } });
  qs('exportCsv').addEventListener('click', exportCsvAll);
  await loadModels();
  await loadCharts();
  
  // Column selector handlers for parameters table
  const pcs = qs('paramColumns');
  if (pcs){
    pcs.addEventListener('change', ()=>{
      selectedParamCols = Array.from(pcs.selectedOptions).map(o=>o.value);
      paramsPager.page = 1;
      loadDetails();
    });
  }
  const ap = qs('applyParamCols'); if (ap) ap.addEventListener('click', ()=>{ const pcs2 = qs('paramColumns'); selectedParamCols = Array.from(pcs2.selectedOptions).map(o=>o.value); paramsPager.page=1; loadDetails(); });
  const cp = qs('clearParamCols'); if (cp) cp.addEventListener('click', ()=>{ selectedParamCols = []; const pcs3 = qs('paramColumns'); if(pcs3){ Array.from(pcs3.options).forEach(o=>o.selected=false); } paramsPager.page=1; loadDetails(); });
  // Units toggle
  const um = qs('unitsMode');
  if (um){
    const saved = localStorage.getItem('mcp_units_mode');
    if (saved === 'metric' || saved === 'imperial') { unitsMode = saved; }
    um.value = unitsMode;
    um.addEventListener('change', ()=>{
      unitsMode = um.value === 'metric' ? 'metric' : 'imperial';
      localStorage.setItem('mcp_units_mode', unitsMode);
      loadDetails();
    });
  }
  startModelPolling();

  // On tab close/refresh, flush events for this doc/session
  window.addEventListener('beforeunload', () => {
    try { flushUiEventsBeacon(); } catch {}
  });

  // Mini drawer chat (n8n)
  initMiniDrawer();
}

init().catch(err => {
  console.error(err);
  alert('Failed to load dashboard. Ensure MCP is running and DB is configured.');
});

// --- Chat embedding ---

function setupChatControls() {
  const providerSel = qs('chatProvider');
  const flowiseInputs = qs('flowiseInputs');
  const n8nInputs = qs('n8nInputs');
  const applyBtn = qs('applyChat');
  const toggleBtn = qs('toggleChat');
  const panel = qs('chatPanel');
  chatFrame = qs('chatFrame');

  providerSel.addEventListener('change', () => {
    const v = providerSel.value;
    flowiseInputs.style.display = v === 'flowise' ? 'flex' : 'none';
    n8nInputs.style.display = v === 'n8n' ? 'flex' : 'none';
  });

  // Set initial visibility based on current selection
  (function initProviderVisibility(){
    const v = providerSel.value;
    flowiseInputs.style.display = v === 'flowise' ? 'flex' : 'none';
    n8nInputs.style.display = v === 'n8n' ? 'flex' : 'none';
  })();

  applyBtn.addEventListener('click', async () => {
    const v = providerSel.value;
    await destroyChat();
    if (v === 'flowise') {
      await initFlowise();
    } else if (v === 'n8n') {
      await initN8n();
    }
    panel.classList.remove('collapsed');
    toggleBtn.textContent = 'Hide Chat';
  });

  toggleBtn.addEventListener('click', () => {
    const collapsed = panel.classList.toggle('collapsed');
    toggleBtn.textContent = collapsed ? 'Show Chat' : 'Hide Chat';
  });
}

async function destroyChat() { if (chatFrame) chatFrame.src = 'about:blank'; }

async function initFlowise() {
  const flowiseId = qs('flowiseId').value.trim();
  const flowiseHost = encodeURIComponent(qs('flowiseHost').value.trim());
  chatFrame.src = `./flowise.html?id=${encodeURIComponent(flowiseId)}&host=${flowiseHost}&sid=${encodeURIComponent(sessionId)}`;
}

async function initN8n() {
  const webhook = qs('n8nWebhook').value.trim();
  const sel = qs('model');
  const doc = sel.value || '';
  const modelName = (sel.selectedOptions && sel.selectedOptions[0] && sel.selectedOptions[0].textContent) || '';
  const params = new URLSearchParams({ webhook, doc, model: modelName, sid: sessionId });
  chatFrame.src = `./n8n.html?${params.toString()}`;
}

// ---- Insight rendering (ECharts) ----

// Unit helpers for client display (Revit internal doubles are imperial)
function inferUnitKind(paramName){
  const n = String(paramName||'').toLowerCase();
  if (/(^|\b)(area|surface|room area)($|\b)/.test(n)) return 'area';
  if (/(^|\b)(volume|room volume)($|\b)/.test(n)) return 'volume';
  if (/(length|width|height|offset|elevation|thickness|radius|diameter|perimeter|depth|top|bottom|sill|head)/.test(n)) return 'length';
  return null;
}
function displayNumber(name, val){
  const num = Number(val);
  if (!isFinite(num)) return null;
  const kind = inferUnitKind(name);
  if (unitsMode !== "metric" || !kind) return num;
  if (kind === "length") return num * 0.3048;
  if (kind === "area") return num * 0.09290304;
  if (kind === "volume") return num * 0.028316846592;
  return num;
}
function convertNumeric(name, val){
  const n = displayNumber(name, val);
  return n == null ? val : n.toFixed(2);
}
function headerWithUnits(name){
  if (unitsMode !== "metric") return name;
  const kind = inferUnitKind(name);
  if (kind === "length") return `${name} (m)`;
  if (kind === "area") return `${name} (mï¿½)`;
  if (kind === "volume") return `${name} (mï¿½)`;
  return name;
}

// --- Mini drawer chat (n8n webhook) --------------------------------------
function initMiniDrawer(){
  const handle = qs('miniDrawerHandle');
  const body = qs('miniDrawerBody');
  const chevron = qs('miniDrawerChevron');
  const hint = qs('miniDrawerHint');
  const input = qs('miniChatInput');
  const sendBtn = qs('miniChatSend');
  const stopBtn = qs('miniChatStop');
  const aiBox = qs('miniChatAI');
  if (!handle || !body) return;

  // Restore open/closed state
  let open = (localStorage.getItem('mini_drawer_open') === '1');
  function setOpen(v){
    open = !!v; localStorage.setItem('mini_drawer_open', open ? '1' : '0');
    body.style.display = open ? 'block' : 'none';
    chevron.textContent = open ? '?' : '?';
  }
  setOpen(open);
  handle.addEventListener('click', ()=> setOpen(!open));

  function appendAI(text){
    const prev = aiBox.textContent || '';
    // Keep last ~4 lines worth of content (approx 600 chars)
    const joined = (prev + (prev?"\n":"") + (text||'')).split('\n');
    const trimmed = joined.slice(-8); // generous, ui clamps height
    aiBox.textContent = trimmed.join('\n');
    aiBox.scrollTop = aiBox.scrollHeight;
    hint.textContent = trimmed.join(' ').slice(-100);
  }

  async function sendMini(e){
    e && e.preventDefault && e.preventDefault();
    const url = (qs('n8nWebhook') && qs('n8nWebhook').value || '').trim();
    const msg = (input && input.value || '').trim();
    if (!url || !msg) return;
    const session = localStorage.getItem('mcp_dash_session') || '';
    appendAI(`You: ${msg}`);
    input.value='';
    try{
      miniAbort?.abort?.();
      miniAbort = new AbortController();
      const res = await fetch(url, {
        method:'POST', headers:{'Content-Type':'application/json'},
        signal: miniAbort.signal,
        body: JSON.stringify({ sessionId: session, action: 'sendMessage', chatInput: msg })
      });
      let txt = '';
      try{
        const ct = res.headers.get('content-type')||'';
        if (ct.includes('application/json')){
          const j = await res.json();
          // Attempt to find a reasonable reply field
          txt = j.reply || j.output || j.result || j.message || j.text || JSON.stringify(j);
        } else {
          txt = await res.text();
        }
      }catch{ txt = '(no response)'; }
      appendAI(`AI: ${txt}`);
      setOpen(true);
    }catch(err){
      if (err?.name === 'AbortError') { appendAI('AI: (stopped)'); return; }
      appendAI('AI: Error: ' + (err?.message || err));
    }
  }

  function stopMini(){ try{ miniAbort?.abort?.(); }catch{} }

  sendBtn && sendBtn.addEventListener('click', sendMini);
  input && input.addEventListener('keydown', (ev)=>{
    if (ev.key === 'Enter' && !ev.shiftKey){ ev.preventDefault(); sendMini(ev); }
  });
  stopBtn && stopBtn.addEventListener('click', stopMini);
}

  // Initialize MCP endpoint from URL or localStorage
  try {
    const epInput = qs('endpoint');
    const params = new URLSearchParams(location.search);
    const urlEp = params.get('mcp') || params.get('endpoint');
    const savedEp = localStorage.getItem('mcp_endpoint');
    if (epInput){
      if (urlEp) epInput.value = urlEp;
      else if (savedEp) epInput.value = savedEp;
      epInput.addEventListener('change', ()=>{
        localStorage.setItem('mcp_endpoint', epInput.value.trim());
      });
    }
  } catch {}


function escapeHtml(s){
  return String(s).replace(/&/g,'&amp;').replace(/</g,'&lt;').replace(/>/g,'&gt;');
}

function csvEscape(v){
  const s = (v==null? '': String(v));
  if (/[",\n]/.test(s)) return '"' + s.replace(/"/g,'""') + '"';
  return s;
}




// ---- Details table rendering ---------------------------------------------
async function loadDetails(){
  const endpoint = qs('endpoint').value.trim();
  const doc = qs('model').value;
  if (!doc) return;
  try {
    const where = ['doc_id=@doc'];
    const catList = (filterState.categories && filterState.categories.length) ? filterState.categories : (filterState.category ? [filterState.category] : []);
    if (catList.length) where.push('category IN (' + catList.map(c=>"'"+String(c).replace(/'/g,"''")+"'" ).join(',') + ')');
    if (filterState.level) where.push('level=@lvl');
    if (filterState.type) where.push('type_name=@typ');
    if (filterState.family) {
      const famFilter = 'type_name IN (SELECT type_name FROM revit_elementTypes WHERE doc_id=@doc' + (catList.length ? ' AND category IN (' + catList.map(c=>"'"+String(c).replace(/'/g,"''")+"'" ).join(',') + ')' : '') + ' AND family=@fam)';
      where.push(famFilter);
    }
    if (Array.isArray(filterState.selectionIds) && filterState.selectionIds.length){
      const ids = filterState.selectionIds.filter(n=>Number.isFinite(n)).slice(0,500);
      if (ids.length){ where.push('id IN (' + ids.join(',') + ')'); }
    }
    // Apply category denylist to details table
    if (hiddenCategories && hiddenCategories.length){
      const list = hiddenCategories.map(c=>"'"+String(c).replace(/'/g,"''")+"'").join(',');
      where.push('category NOT IN (' + list + ')');
    }
    const whereSql = where.join(' AND ');
    const paramsBase = { '@doc': doc, ...(filterState.level ? { '@lvl': filterState.level } : {}), ...(filterState.type ? { '@typ': filterState.type } : {}), ...(filterState.family ? { '@fam': filterState.family } : {}) };

    // total count
    const countResp = await mcp(endpoint, { action:'Db.Query', sql: `SELECT COUNT(*) AS c FROM revit_elements WHERE ${whereSql}`, params: JSON.stringify(paramsBase) });
    const total = Number((countResp.results?.[0]?.c) ?? (countResp.rows?.[0]?.c) ?? (countResp.data?.[0]?.c) ?? 0);
    pager.total = total;

    const offset = (pager.page - 1) * pager.pageSize;
    const rowsResp = await mcp(endpoint, { action:'Db.Query', sql: `SELECT id, name, COALESCE(category,'(none)') AS category, COALESCE(type_name,'') AS type_name, COALESCE(level,'(none)') AS level FROM revit_elements WHERE ${whereSql} ORDER BY category, type_name, id LIMIT @lim OFFSET @off`, params: JSON.stringify({ ...paramsBase, '@lim': pager.pageSize, '@off': offset }) });
    const rows = rowsResp.results || rowsResp.rows || rowsResp.data || [];

    // Column selector population
    const whereSqlE = whereSql
      .replace(/\bdoc_id\b/g, 'e.doc_id')
      .replace(/\bcategory\b/g, 'e.category')
      .replace(/\blevel\b/g, 'e.level')
      .replace(/\btype_name\b/g, 'e.type_name');
    const namesResp = await mcp(endpoint, { action:'Db.Query', sql: `SELECT DISTINCT p.param_name FROM revit_parameters p JOIN revit_elements e ON e.id=p.element_id AND e.doc_id=@doc WHERE ${whereSqlE} ORDER BY p.param_name LIMIT 500`, params: JSON.stringify(paramsBase) });
    const availableNames = (namesResp.results || namesResp.rows || namesResp.data || []).map(r=> r.param_name).filter(Boolean);
    const sel = qs('paramColumns');
    if (sel){
      const prev = new Set(selectedParamCols);
      sel.innerHTML = '';
      for (const n of availableNames){
        const opt = document.createElement('option');
        opt.value = n; opt.textContent = n; if (prev.has(n)) opt.selected = true; sel.appendChild(opt);
      }
      if (!selectedParamCols.length && availableNames.length){
        selectedParamCols = availableNames.slice(0,6); Array.from(sel.options).forEach(o=> o.selected = selectedParamCols.includes(o.value));
      }
    }

    // Fetch parameter values for current page
    let paramMap = new Map();
    if (rows.length && selectedParamCols.length){
      const ids = rows.map(r=> Number(r.id)).filter(n=>Number.isFinite(n));
      const idList = ids.join(',');
      const colList = selectedParamCols.map(n=> "'" + String(n).replace(/'/g,"''") + "'" ).join(',');
      const pvSql = `SELECT p.element_id, p.param_name, p.param_value FROM revit_parameters p JOIN revit_elements e ON e.id=p.element_id AND e.doc_id=@doc WHERE e.id IN (${idList}) AND p.param_name IN (${colList})`;
      const pvResp = await mcp(endpoint, { action:'Db.Query', sql: pvSql, params: JSON.stringify({ '@doc': doc }) });
      const paramRows = (pvResp.results || pvResp.rows || pvResp.data || []);
      paramMap = new Map();
      for (const r of paramRows){ const id = Number(r.element_id); if (!paramMap.has(id)) paramMap.set(id, {}); paramMap.get(id)[r.param_name] = r.param_value; }
    }

    // Header
    const header = qs('detailsHeader');
    if (header){
      const base = ['ID','Name','Category','Type','Level']; header.innerHTML = '';
      for (const h of base){ const th = document.createElement('th'); th.style.cssText = 'text-align:left; padding:8px; border-bottom:1px solid var(--border);'; th.textContent = h; header.appendChild(th); }
      for (const n of selectedParamCols){ const th = document.createElement('th'); th.style.cssText = 'text-align:left; padding:8px; border-bottom:1px solid var(--border);'; th.textContent = headerWithUnits(n); header.appendChild(th); }
    }

    // Body + totals
    const tbody = qs('detailsTable').querySelector('tbody'); tbody.innerHTML = '';
    const totals = new Map(); const hasNumeric = new Map();
    for (const r of rows){
      const tr = document.createElement('tr'); let html = '';
      html += `<td style=\\"padding:6px 8px; border-bottom:1px solid var(--border); color:var(--text)\\">${r.id ?? ''}</td>`;
      html += `<td style=\\"padding:6px 8px; border-bottom:1px solid var(--border); color:var(--text)\\">${escapeHtml(r.name ?? '')}</td>`;
      html += `<td style=\\"padding:6px 8px; border-bottom:1px solid var(--border); color:var(--muted)\\">${escapeHtml(r.category ?? '')}</td>`;
      html += `<td style=\\"padding:6px 8px; border-bottom:1px solid var(--border); color:var(--muted)\\">${escapeHtml(r.type_name ?? '')}</td>`;
      html += `<td style=\\"padding:6px 8px; border-bottom:1px solid var(--border); color:var(--muted)\\">${escapeHtml(r.level ?? '')}</td>`;
      const vals = paramMap.get(Number(r.id)) || {};
      for (const n of selectedParamCols){
        const v = vals[n] ?? ''; const cv = convertNumeric(n, v);
        const num = Number(cv); if (isFinite(num)) { totals.set(n, (totals.get(n)||0)+num); hasNumeric.set(n,true); }
        html += `<td style=\\"padding:6px 8px; border-bottom:1px solid var(--border); color:var(--text)\\">${escapeHtml(cv ?? '')}</td>`;
      }
      tr.innerHTML = html; tbody.appendChild(tr);
    }

    const table = qs('detailsTable'); if (table.tFoot) table.removeChild(table.tFoot);
    const tfoot = document.createElement('tfoot'); const trow = document.createElement('tr');
    for (const txt of ['S','Totals','','','']){ const td=document.createElement('td'); td.style.cssText='padding:6px 8px; border-top:2px solid var(--border); font-weight:600; color:var(--text)'; td.textContent=txt; trow.appendChild(td); }
    for (const n of selectedParamCols){ const td=document.createElement('td'); td.style.cssText='padding:6px 8px; border-top:2px solid var(--border); font-weight:600; color:var(--text)'; td.textContent = hasNumeric.get(n) ? (totals.get(n)||0).toFixed(2) : ''; trow.appendChild(td); }
    tfoot.appendChild(trow); table.appendChild(tfoot);

    const offset2 = (pager.page - 1) * pager.pageSize; const start = total ? offset2 + 1 : 0; const end = Math.min(total, offset2 + rows.length); const maxPage = Math.max(1, Math.ceil(total / Math.max(1,pager.pageSize)));
    const info = qs('detailsInfo'); if (info) info.textContent = total ? `Showing ${start}-${end} of ${total} — Page ${pager.page}/${maxPage}` : 'No rows match the current filters.';
  } catch (e) {
    console.error('loadDetails error', e);
    const info = qs('detailsInfo'); if (info) info.textContent = 'Error loading details: ' + (e?.message || e);
  }
}

// --- Minimal stubs/utilities to keep dashboard responsive ---------------
function startModelPolling(){
  try { if (modelPollTimer) clearInterval(modelPollTimer); } catch {}
  modelPollTimer = setInterval(() => { loadModels().catch(()=>{}); }, 30000);
}
async function flushUiEventsSafe(){ /* no-op stub to avoid errors */ return; }
function flushUiEventsBeacon(){ /* no-op stub */ }


async function exportCsvAll(){
  // Export current filtered elements including currently selected parameter columns (in display units)
  const endpoint = qs('endpoint').value.trim();
  const doc = qs('model').value;
  if (!doc) return;

  // Build WHERE according to current filters
  const where = ['doc_id=@doc'];
  const catList = (filterState.categories && filterState.categories.length) ? filterState.categories : (filterState.category ? [filterState.category] : []);
  if (catList.length) where.push('category IN (' + catList.map(c=>"'"+String(c).replace(/'/g,"''")+"'" ).join(',') + ')');
  if (filterState.level) where.push('level=@lvl');
  if (filterState.type) where.push('type_name=@typ');
  if (filterState.family) {
    const famFilter = 'type_name IN (SELECT type_name FROM revit_elementTypes WHERE doc_id=@doc' + (catList.length ? ' AND category IN (' + catList.map(c=>"'"+String(c).replace(/'/g,"''")+"'" ).join(',') + ')' : '') + ' AND family=@fam)';
    where.push(famFilter);
  }
  if (Array.isArray(filterState.selectionIds) && filterState.selectionIds.length){
    const ids = filterState.selectionIds.filter(n=>Number.isFinite(n)).slice(0,10000);
    if (ids.length){ where.push(`id IN (${ids.join(',')})`); }
  }
  // Apply category denylist to export
  if (hiddenCategories && hiddenCategories.length){
    const list = hiddenCategories.map(c=>"'"+String(c).replace(/'/g,"''")+"'").join(',');
    where.push('category NOT IN (' + list + ')');
  }
  const whereSql = where.join(' AND ');
  const paramsBase = { '@doc': doc, ...(filterState.level ? { '@lvl': filterState.level } : {}), ...(filterState.type ? { '@typ': filterState.type } : {}), ...(filterState.family ? { '@fam': filterState.family } : {}) };

  // Fetch elements (cap to avoid huge CSVs)
  const cap = 10000;
  const elSql = `SELECT id, name, COALESCE(category,'(none)') AS category, COALESCE(type_name,'') AS type_name, COALESCE(level,'(none)') AS level
                 FROM revit_elements WHERE ${whereSql} ORDER BY category, type_name, id LIMIT ${cap}`;
  const elResp = await mcp(endpoint, { action:'Db.Query', sql: elSql, params: JSON.stringify(paramsBase) });
  const elems = elResp.results || elResp.rows || elResp.data || [];

  // Pivot selected parameter columns for these elements
  const paramCols = Array.isArray(selectedParamCols) ? selectedParamCols.slice() : [];
  let paramMap = new Map();
  if (elems.length && paramCols.length){
    const ids = elems.map(r=> Number(r.id)).filter(n=>Number.isFinite(n));
    const idList = ids.join(',');
    const colList = paramCols.map(n=> "'" + String(n).replace(/'/g,"''") + "'" ).join(',');
    const pvSql = `SELECT p.element_id, p.param_name, p.param_value
                   FROM revit_parameters p
                   JOIN revit_elements e ON e.id=p.element_id AND e.doc_id=@doc
                   WHERE e.id IN (${idList}) AND p.param_name IN (${colList})`;
    const pvResp = await mcp(endpoint, { action:'Db.Query', sql: pvSql, params: JSON.stringify({ '@doc': doc }) });
    const rows = pvResp.results || pvResp.rows || pvResp.data || [];
    paramMap = new Map();
    for (const r of rows){ const id = Number(r.element_id); if (!paramMap.has(id)) paramMap.set(id, {}); paramMap.get(id)[r.param_name] = r.param_value; }
  }

  // Compose CSV
  const baseHeaders = ['id','name','category','type_name','level'];
  const headers = baseHeaders.concat(paramCols.map(c => headerWithUnits(c)));
  const lines = [headers.join(',')];
  for (const r of elems){
    const base = [r.id, r.name, r.category, r.type_name, r.level].map(csvEscape);
    const vals = paramMap.get(Number(r.id)) || {};
    const extras = paramCols.map(n => csvEscape(convertNumeric(n, vals[n])));
    lines.push(base.concat(extras).join(','));
  }
  const csv = lines.join('\n');

  const blob = new Blob([csv], { type: 'text/csv;charset=utf-8;' });
  const url = URL.createObjectURL(blob);
  const a = document.createElement('a');
  a.href = url; a.download = 'revit-elements.csv';
  document.body.appendChild(a); a.click(); document.body.removeChild(a);
  URL.revokeObjectURL(url);
}








