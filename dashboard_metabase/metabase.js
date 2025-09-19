async function mcp(endpoint, body) {
  const res = await fetch(endpoint, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(body)
  });
  if (!res.ok) throw new Error(`MCP HTTP ${res.status}`);
  return await res.json();
}

function qs(id){ return document.getElementById(id); }

let currentModel = { doc_id: null, model_name: null };
let miniAbort = null; // AbortController for mini chat

function ensureSessionId(){
  try {
    const existing = localStorage.getItem('mcp_dash_session');
    if (existing) return existing;
    const sid = (self.crypto?.randomUUID ? crypto.randomUUID() : (String(Date.now()) + Math.random().toString(16).slice(2)));
    localStorage.setItem('mcp_dash_session', sid);
    return sid;
  } catch {
    return '';
  }
}

function setMode(mode){
  qs('mbDirectRow').style.display = (mode==='direct') ? '' : 'none';
  qs('mbTplRow').style.display    = (mode==='template') ? '' : 'none';
  qs('mbBackendRow').style.display= (mode==='backend') ? '' : 'none';
}

function templateUrl(tpl, ctx){
  return (tpl||'').replaceAll('{doc_id}', encodeURIComponent(ctx.doc_id||''))
                  .replaceAll('{model_name}', encodeURIComponent(ctx.model_name||''));
}

async function loadModels(){
  const endpoint = qs('endpoint').value.trim();
  const resp = await mcp(endpoint, { action: 'Db.Query', sql: 'SELECT doc_id, model_name, last_saved FROM model_info ORDER BY COALESCE(last_saved, CURRENT_TIMESTAMP) DESC' });
  const data = resp.results || resp.rows || resp.result || resp.data || [];
  const sel = qs('model');
  const prev = sel.value;
  sel.innerHTML='';
  for (const row of data){
    const opt = document.createElement('option');
    opt.value = row.doc_id;
    opt.textContent = row.model_name ? `${row.model_name} — ${row.doc_id}` : row.doc_id;
    opt.setAttribute('data-model-name', row.model_name||'');
    sel.appendChild(opt);
  }
  if (prev && Array.from(sel.options).some(o=>o.value===prev)) sel.value = prev; else if (sel.options.length) sel.selectedIndex = 0;
  updateCurrentModelFromSelect();
}

function updateCurrentModelFromSelect(){
  const sel = qs('model');
  const opt = sel.options[sel.selectedIndex];
  if (!opt){ currentModel = { doc_id:null, model_name:null }; return; }
  currentModel = { doc_id: opt.value, model_name: opt.getAttribute('data-model-name') || '' };
  qs('hint').textContent = `doc_id=${currentModel.doc_id || '(none)'}  model_name=${currentModel.model_name || '(none)'} `;
}

async function resolveEmbedUrl(){
  const mode = qs('mbMode').value;
  const ctx = currentModel;
  if (mode === 'direct'){
    return qs('mbDirect').value.trim();
  } else if (mode === 'template'){
    return templateUrl(qs('mbTpl').value.trim(), ctx);
  } else if (mode === 'backend'){
    const u = templateUrl(qs('mbBackend').value.trim(), ctx);
    if (!u) return '';
    // Backend may return plain text or JSON { url: "..." }
    try {
      const res = await fetch(u, { method:'GET' });
      if (!res.ok) throw new Error(`Backend ${res.status}`);
      const txt = await res.text();
      try { const j = JSON.parse(txt); if (j && j.url) return String(j.url); } catch {}
      return txt.trim();
    } catch (e){
      console.error(e);
      alert('Failed to fetch embed URL from backend: ' + (e?.message||e));
      return '';
    }
  }
  return '';
}

async function loadMetabase(){
  const url = await resolveEmbedUrl();
  if (!url){
    alert('Please provide a valid Metabase URL or endpoint.');
    return;
  }
  const frame = qs('mbFrame');
  frame.src = url;
}

// Wire up events
window.addEventListener('DOMContentLoaded', ()=>{
  qs('refresh').addEventListener('click', loadModels);
  qs('model').addEventListener('change', ()=>{
    updateCurrentModelFromSelect();
  });
  qs('mbMode').addEventListener('change', (e)=> setMode(e.target.value));
  qs('loadMb').addEventListener('click', loadMetabase);
  setMode(qs('mbMode').value);
  loadModels().catch(err=> console.error(err));
  // Init mini drawer chat (n8n)
  try { ensureSessionId(); initMiniDrawer(); } catch {}
});

// --- Mini drawer chat copied from dashboard/main.js (trimmed) ---
function initMiniDrawer(){
  const drawer = qs('miniDrawer');
  const bar = qs('miniDrawerBar');
  const handle = qs('miniDrawerHandle');
  const chevron = qs('miniDrawerChevron');
  const body = qs('miniDrawerBody');
  const aiBox = qs('miniChatAI');
  const input = qs('miniChatInput');
  const sendBtn = qs('miniChatSend');
  const stopBtn = qs('miniChatStop');
  const hint = qs('miniDrawerHint');

  if (!drawer || !bar || !handle || !body || !aiBox || !input || !sendBtn || !stopBtn) return;

  let open = false;
  function setOpen(v){
    open = !!v;
    body.style.display = open ? 'block' : 'none';
    chevron.textContent = open ? 'v' : '^';
  }
  setOpen(false);

  handle.addEventListener('click', ()=> setOpen(!open));

  function appendAI(text){
    const prev = aiBox.textContent || '';
    const joined = (prev + (prev?'\n':'') + (text||'')).split('\n');
    const trimmed = joined.slice(-8);
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
        body: JSON.stringify({ sessionId: session, action: 'sendMessage', chatInput: msg, context: { doc_id: currentModel.doc_id, model_name: currentModel.model_name } })
      });
      let txt = '';
      try{
        const ct = res.headers.get('content-type')||'';
        if (ct.includes('application/json')){
          const j = await res.json();
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
