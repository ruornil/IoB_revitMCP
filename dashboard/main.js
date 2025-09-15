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
    opt.textContent = row.model_name ? `${row.model_name} — ${row.doc_id}` : row.doc_id;
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

  const qsParams = (obj) => JSON.stringify(obj);
  // Build WHERE fragments based on active filters (support multi-category)
  const catList = (filterState.categories && filterState.categories.length) ? filterState.categories : (filterState.category ? [filterState.category] : []);
  const catFilter = catList.length ? (' AND category IN (' + catList.map(c => "'" + String(c).replace(/'/g, "''") + "'").join(',') + ')') : '';
  const lvlFilter = filterState.level ? ' AND level=@lvl' : '';
  const typeFilter = filterState.type ? ' AND type_name=@typ' : '';
  const familyInFilter = filterState.family ? (' AND type_name IN (SELECT type_name FROM revit_elementTypes WHERE doc_id=@doc' + (catList.length ? (' AND category IN (' + catList.map(c => "'" + String(c).replace(/'/g, "''") + "'").join(',') + ')') : '') + ' AND family=@fam)') : '';

  const queries = {
    byCat: {
      action: 'Db.Query',
      sql: `SELECT COALESCE(category,'(none)') AS category, COUNT(*) AS c FROM revit_elements WHERE doc_id=@doc${lvlFilter}${typeFilter}${familyInFilter}${catFilter} GROUP BY category ORDER BY c DESC LIMIT 20`,
      params: qsParams({ '@doc': doc, ...(filterState.level ? { '@lvl': filterState.level } : {}), ...(filterState.type ? { '@typ': filterState.type } : {}), ...(filterState.family ? { '@fam': filterState.family } : {}) })
    },
    byLevel: {
      action: 'Db.Query',
      sql: "SELECT COALESCE(level, '(none)') AS level, COUNT(*) AS c FROM revit_elements WHERE doc_id=@doc" + catFilter + typeFilter + familyInFilter + ' GROUP BY level ORDER BY c DESC',
      params: qsParams({ '@doc': doc, ...(filterState.type ? { '@typ': filterState.type } : {}), ...(filterState.family ? { '@fam': filterState.family } : {}) })
    },
    typesByCat: {
      action: 'Db.Query',
      sql: `SELECT COALESCE(category,'(none)') AS category, COUNT(DISTINCT type_name) AS c FROM revit_elements WHERE doc_id=@doc${lvlFilter}${typeFilter}${familyInFilter}${catFilter} GROUP BY category ORDER BY c DESC LIMIT 20`,
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

  // Recent Insight (from ui_events)
  try {
    const latest = await mcp(endpoint, { action: 'Db.Query', sql: 'SELECT id, event_type, payload, created_at FROM ui_events WHERE doc_id=@doc ORDER BY id DESC LIMIT 1', params: qsParams({ '@doc': doc }) });
    const evRows = rows(latest);
    if (evRows.length) {
      const ev = evRows[0];
      const payload = typeof ev.payload === 'string' ? JSON.parse(ev.payload) : ev.payload;
      renderInsight(ev.event_type, payload, ev.created_at);
    }
  } catch (e) {
    console.warn('No insight events yet', e);
  }

  // Update details after charts and insight
  await loadDetails();
}

// Families and Types by Category panel
async function loadFamilyTypePanel(){
  const endpoint = qs('endpoint').value.trim();
  const doc = qs('model').value;
  if (!doc) return;
  const qsParams = (obj) => JSON.stringify(obj);

  // Load categories for select (from elementTypes)
  const catsResp = await mcp(endpoint, { action:'Db.Query', sql: "SELECT COALESCE(category,'(none)') AS category, COUNT(DISTINCT family) AS c FROM revit_elementTypes WHERE doc_id=@doc GROUP BY category ORDER BY c DESC", params: qsParams({ '@doc': doc }) });
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
  const famWhere = catList.length ? ('WHERE doc_id=@doc AND category IN (' + catList.map(c=>"'"+String(c).replace(/'/g,"''")+"'").join(',') + ')') : 'WHERE doc_id=@doc';
  const famResp = await mcp(endpoint, { action:'Db.Query', sql: `SELECT family, COUNT(DISTINCT type_name) AS c FROM revit_elementTypes ${famWhere} GROUP BY family ORDER BY c DESC LIMIT 200`, params: qsParams({ '@doc': doc }) });
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
      const typeWhere = 'WHERE doc_id=@doc' + (catList.length ? (' AND category IN (' + catList.map(c=>"'"+String(c).replace(/'/g,"''")+"'").join(',') + ')') : '') + ' AND family=@fam';
      const typeResp = await mcp(endpoint, { action:'Db.Query', sql: `SELECT type_name, COUNT(*) AS c FROM revit_elementTypes ${typeWhere} GROUP BY type_name ORDER BY c DESC LIMIT 400`, params: qsParams({ '@doc': doc, '@fam': filterState.family }) });
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
  // Generate or reuse a session id to correlate chat ↔ dashboard
  sessionId = (localStorage.getItem('mcp_dash_session')) || (self.crypto?.randomUUID ? crypto.randomUUID() : String(Date.now()) + Math.random().toString(16).slice(2));
  localStorage.setItem('mcp_dash_session', sessionId);
  qs('refresh').addEventListener('click', async () => {
    await flushUiEventsSafe();
    await loadModels();
    await loadCharts();
  });
  qs('endpoint').addEventListener('change', loadModels);
  setupChatControls();
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
  startEventPolling();
  startModelPolling();

  // On tab close/refresh, flush events for this doc/session
  window.addEventListener('beforeunload', () => {
    try { flushUiEventsBeacon(); } catch {}
  });
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
function renderInsight(type, payload, createdAt){
  const meta = qs('insightMeta');
  meta.textContent = `Type: ${type} — ${new Date(createdAt).toLocaleString()}`;

  // Reset any previous content
  if (charts['insightChart1']) { charts['insightChart1'].dispose(); delete charts['insightChart1']; }
  if (charts['insightChart2']) { charts['insightChart2'].dispose(); delete charts['insightChart2']; }

  if (type === 'filter_summary' || (payload && payload.kind === 'filter_summary')){
    const s = payload.summary || payload;
    const labels1 = (s.by_type || []).map(x=>x.type);
    const data1 = (s.by_type || []).map(x=>Number(x.count));
    upsertChart('insightChart1', {
      grid:{left:80,right:10,top:10,bottom:30},
      xAxis:{ type:'value', axisLabel:{ color:'#9aa0b4' } },
      yAxis:{ type:'category', data:labels1, axisLabel:{ color:'#9aa0b4' } },
      series:[{ type:'bar', data:data1 }]
    });

    const sums = s.sums || {};
    const labels2 = ['area','volume','length'];
    const data2 = labels2.map(k=>Number(sums[k]||0));
    upsertChart('insightChart2', {
      grid:{left:40,right:10,top:10,bottom:30},
      xAxis:{ type:'category', data:labels2, axisLabel:{ color:'#9aa0b4' } },
      yAxis:{ type:'value', axisLabel:{ color:'#9aa0b4' } },
      series:[{ type:'bar', data:data2 }]
    });
  }
  else if (type === 'model_summary' || (payload && payload.kind === 'model_summary')){
    const byCat = payload.by_category || [];
    const tByCat = payload.types_by_category || [];
    const l1 = byCat.map(x=>x.category || '(none)');
    const d1 = byCat.map(x=>Number(x.c));
    upsertChart('insightChart1', {
      grid:{left:40,right:10,top:10,bottom:30},
      xAxis:{ type:'category', data:l1, axisLabel:{ color:'#9aa0b4', interval:0, rotate:30 } },
      yAxis:{ type:'value', axisLabel:{ color:'#9aa0b4' } },
      series:[{ type:'bar', data:d1.map((v,i)=>({ value:v, itemStyle:{ color:color(i) } })) }]
    });
    const l2 = tByCat.map(x=>x.category || '(none)');
    const d2 = tByCat.map(x=>Number(x.c));
    upsertChart('insightChart2', {
      legend:{ top:0, textStyle:{ color:'#e6e8f2' } },
      series:[{ type:'pie', radius:['40%','70%'], data: l2.map((n,i)=>({ name:n, value:d2[i], itemStyle:{ color:color(i) } })) }]
    });
  }
  else if (type === 'params_upserted' || (payload && payload.kind === 'params_upserted')){
    const updated = Number(payload?.updated || 0);
    upsertChart('insightChart1', {
      title:{ text:`Parameters updated: ${updated}`, left:'center', textStyle:{ color:'#e6e8f2', fontSize:14 } }
    });
    const names = (payload?.names || []).slice(0,12);
    if (names.length){
      upsertChart('insightChart2', {
        grid:{left:40,right:10,top:10,bottom:30},
        xAxis:{ type:'category', data:names, axisLabel:{ color:'#9aa0b4', interval:0, rotate:30 } },
        yAxis:{ show:false },
        series:[{ type:'bar', data:names.map((_,i)=>({ value:1, itemStyle:{ color:color(i) } })) }]
      });
    }
  }
  else {
    // Fallback message in chart area
    upsertChart('insightChart1', {
      title:{ text:'No renderer for this insight type', left:'center', textStyle:{ color:'#9aa0b4', fontSize:12 } }
    });
    upsertChart('insightChart2', { title:{ text:'', left:'center' } });
  }
}

// ---- Event bridge (poll ui_events) ----
function startEventPolling(){
  async function tick(){
    try {
      const endpoint = qs('endpoint').value.trim();
      const doc = qs('model').value || null;
      const sidParam = (sessionId && typeof sessionId === 'string' && sessionId.trim().length) ? sessionId : null;
      const resp = await mcp(endpoint, {
        action:'Db.Query',
        // Also listen to model_summary across docs so we can refresh model list
        sql: "SELECT id, event_type, payload, created_at FROM ui_events WHERE id > @since AND (((@doc IS NULL OR doc_id=@doc)) OR event_type='model_summary') AND (@sid IS NULL OR session_id=@sid OR session_id IS NULL OR session_id='') ORDER BY id ASC LIMIT 50",
        params: JSON.stringify({ '@doc': doc, '@sid': sidParam, '@since': lastEventId })
      });
      const rows = resp.results || resp.rows || resp.data || [];
      for (const r of rows){
        lastEventId = Math.max(lastEventId, Number(r.id||0));
        await handleUiEvent(r);
      }
    } catch(e){
      // silent; will try again
    } finally {
      setTimeout(tick, 1500);
    }
  }
  tick();
}

async function handleUiEvent(ev){
  const { event_type, payload, created_at } = ev;
  const data = typeof payload === 'string' ? safeJson(payload) : payload;
  if (!event_type) return;

  // Show in insight when it looks like a summary/update
  if (event_type === 'model_summary' || (data && (data.kind==='model_summary' || data.kind==='filter_summary' || data.kind==='params_upserted'))){
    renderInsight(event_type, data, created_at || new Date().toISOString());
    // Refresh models list in case a new model_info row was written by sync/summary
    loadModels().catch(()=>{});
  }

  if (event_type === 'set_filter'){
    if (data && typeof data === 'object'){
      if ('category' in data) filterState.category = data.category || null;
      if ('level' in data) filterState.level = data.level || null;
      if ('family' in data) filterState.family = data.family || null;
      if ('type' in data) filterState.type = data.type || null;
      pager.page = 1;
      loadCharts();
    }
  }

  if (event_type === 'element_selection'){
    // Expect { ids:[...], category?, level?, apply_filters?:true }
    const ids = Array.isArray(data?.ids) ? data.ids.map(x=> parseInt(x,10)).filter(n=>Number.isFinite(n)) : [];
    filterState.selectionIds = ids.slice(0, 500); // cap to 500 for SQL safety
    // Derive categories from selected ids to drive family/types & filters
    try {
      const endpoint = qs('endpoint').value.trim();
      const doc = qs('model').value || null;
      if (endpoint && doc && ids.length){
        const inList = ids.join(',');
        const resp = await mcp(endpoint, { action:'Db.Query', sql: `SELECT DISTINCT COALESCE(category,'(none)') AS category FROM revit_elements WHERE doc_id=@doc AND id IN (${inList})`, params: JSON.stringify({ '@doc': doc }) });
        const cats = (resp.results || resp.rows || resp.data || []).map(r=> r.category).filter(Boolean);
        if (cats.length){ filterState.categories = cats; filterState.category = null; }
      }
    } catch {}
    if (data?.apply_filters){ if (data.level) filterState.level = data.level; }
    pager.page = 1;
    await loadCharts();
    await loadDetails();
  }
}

function safeJson(s){ try { return JSON.parse(s); } catch { return null; } }

// ---- Periodic model list refresh (fallback if no events are pushed) ----
function startModelPolling(){
  stopModelPolling();
  const run = () => loadModels().catch(()=>{});
  modelPollTimer = setInterval(run, 15000); // every 15s
  document.addEventListener('visibilitychange', () => {
    if (document.hidden) { stopModelPolling(); }
    else { startModelPolling(); run(); }
  });
}

function stopModelPolling(){ if (modelPollTimer) { clearInterval(modelPollTimer); modelPollTimer = null; } }

// ---- Flush ui_events for current doc/session ----
function buildFlushPayload(){
  const endpoint = qs('endpoint')?.value?.trim();
  const doc = qs('model')?.value || null;
  const sidParam = (sessionId && typeof sessionId === 'string' && sessionId.trim().length) ? sessionId : null;
  const body = {
    action: 'Db.Query',
    sql: "DELETE FROM ui_events WHERE (@doc IS NULL OR doc_id=@doc) AND (@sid IS NULL OR session_id=@sid OR session_id='')",
    params: JSON.stringify({ '@doc': doc, '@sid': sidParam })
  };
  return { endpoint, body };
}

async function flushUiEventsSafe(){
  try {
    const { endpoint, body } = buildFlushPayload();
    if (!endpoint) return;
    await mcp(endpoint, body);
  } catch {}
}

function flushUiEventsBeacon(){
  const { endpoint, body } = buildFlushPayload();
  if (!endpoint) return;
  const json = JSON.stringify(body);
  if (navigator.sendBeacon){
    const blob = new Blob([json], { type: 'application/json' });
    navigator.sendBeacon(endpoint, blob);
  } else {
    // Fallback for older browsers
    try { fetch(endpoint, { method: 'POST', body: json, keepalive: true, headers: { 'Content-Type': 'application/json' } }); } catch {}
  }
}

// ---- Drill-through details ----
async function loadDetails(){
  const endpoint = qs('endpoint').value.trim();
  const doc = qs('model').value;
  if (!doc) return;

  const where = ['doc_id=@doc'];
  const catList = (filterState.categories && filterState.categories.length) ? filterState.categories : (filterState.category ? [filterState.category] : []);
  if (catList.length) where.push('category IN (' + catList.map(c=>"'"+String(c).replace(/'/g,"''")+"'").join(',') + ')');
  if (filterState.level) where.push('level=@lvl');
  if (filterState.type) where.push('type_name=@typ');
  if (filterState.family) {
    const famFilter = 'type_name IN (SELECT type_name FROM revit_elementTypes WHERE doc_id=@doc' + (catList.length ? ' AND category IN (' + catList.map(c=>"'"+String(c).replace(/'/g,"''")+"'").join(',') + ')' : '') + ' AND family=@fam)';
    where.push(famFilter);
  }
  if (Array.isArray(filterState.selectionIds) && filterState.selectionIds.length){
    const ids = filterState.selectionIds.filter(n=>Number.isFinite(n)).slice(0,500);
    if (ids.length){ where.push(`id IN (${ids.join(',')})`); }
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

  // render
  const tbody = qs('detailsTable').querySelector('tbody');
  tbody.innerHTML = '';
  for (const r of rows){
    const tr = document.createElement('tr');
    tr.innerHTML = `
      <td style="padding:6px 8px; border-bottom:1px solid var(--border); color:var(--text)">${r.id ?? ''}</td>
      <td style="padding:6px 8px; border-bottom:1px solid var(--border); color:var(--text)">${escapeHtml(r.name ?? '')}</td>
      <td style="padding:6px 8px; border-bottom:1px solid var(--border); color:var(--muted)">${escapeHtml(r.category ?? '')}</td>
      <td style="padding:6px 8px; border-bottom:1px solid var(--border); color:var(--muted)">${escapeHtml(r.type_name ?? '')}</td>
      <td style="padding:6px 8px; border-bottom:1px solid var(--border); color:var(--muted)">${escapeHtml(r.level ?? '')}</td>`;
    tbody.appendChild(tr);
  }

  const start = total ? offset + 1 : 0;
  const end = Math.min(total, offset + rows.length);
  const maxPage = Math.max(1, Math.ceil(total / Math.max(1,pager.pageSize)));
  qs('detailsInfo').textContent = `Showing ${start}-${end} of ${total} • Page ${pager.page}/${maxPage}`;
}

function escapeHtml(s){
  return String(s).replace(/&/g,'&amp;').replace(/</g,'&lt;').replace(/>/g,'&gt;');
}

async function exportCsvAll(){
  const endpoint = qs('endpoint').value.trim();
  const doc = qs('model').value;
  if (!doc) return;
  const where = ['doc_id=@doc'];
  const catList2 = (filterState.categories && filterState.categories.length) ? filterState.categories : (filterState.category ? [filterState.category] : []);
  if (catList2.length) where.push('category IN (' + catList2.map(c=>"'"+String(c).replace(/'/g,"''")+"'").join(',') + ')');
  if (filterState.level) where.push('level=@lvl');
  if (filterState.type) where.push('type_name=@typ');
  if (filterState.family) {
    const famFilter = 'type_name IN (SELECT type_name FROM revit_elementTypes WHERE doc_id=@doc' + (catList2.length ? ' AND category IN (' + catList2.map(c=>"'"+String(c).replace(/'/g,"''")+"'").join(',') + ')' : '') + ' AND family=@fam)';
    where.push(famFilter);
  }
  if (Array.isArray(filterState.selectionIds) && filterState.selectionIds.length){
    const ids = filterState.selectionIds.filter(n=>Number.isFinite(n)).slice(0,10000);
    if (ids.length){ where.push(`id IN (${ids.join(',')})`); }
  }
  const whereSql = where.join(' AND ');
  const paramsBase = { '@doc': doc, ...(filterState.level ? { '@lvl': filterState.level } : {}), ...(filterState.type ? { '@typ': filterState.type } : {}), ...(filterState.family ? { '@fam': filterState.family } : {}) };

  // Safety cap to avoid exporting an enormous dataset by mistake
  const cap = 10000;
  const sql = `SELECT id, name, COALESCE(category,'(none)') AS category, COALESCE(type_name,'') AS type_name, COALESCE(level,'(none)') AS level
               FROM revit_elements WHERE ${whereSql} ORDER BY category, type_name, id LIMIT ${cap}`;
  const resp = await mcp(endpoint, { action:'Db.Query', sql, params: JSON.stringify(paramsBase) });
  const rows = resp.results || resp.rows || resp.data || [];
  const headers = ['id','name','category','type_name','level'];
  const csv = [headers.join(',')].concat(rows.map(r=> headers.map(h=> csvEscape(r[h])).join(','))).join('\n');

  const blob = new Blob([csv], { type: 'text/csv;charset=utf-8;' });
  const url = URL.createObjectURL(blob);
  const a = document.createElement('a');
  a.href = url;
  a.download = 'revit-elements.csv';
  document.body.appendChild(a);
  a.click();
  document.body.removeChild(a);
  URL.revokeObjectURL(url);
}

function csvEscape(v){
  const s = (v==null? '': String(v));
  if (/[",\n]/.test(s)) return '"' + s.replace(/"/g,'""') + '"';
  return s;
}


