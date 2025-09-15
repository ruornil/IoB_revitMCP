Revit MCP Dashboard
===================

This dashboard visualizes your synced Revit model with quick, responsive charts and an optional embedded chat. It calls your MCP endpoint (`Db.Query`) directly from the browser and reacts to tool/chat events via the `ui_events` table.

What's new

- ECharts-based charts with click-to-filter across views.
- Filters badge and Clear action.
- Drill-through table (pagination + CSV export).
- Chat ↔ Dashboard event bridge using `Ui.PushEvent` + `ui_events`.

What it provides

- Elements by category and level
- Distinct types per category
- Drill-through elements table
- Optional embedded chat (Flowise or n8n)

How it works

- MCP enables CORS and accepts JSON at `http://localhost:5005/mcp/`.
- `dashboard/index.html` + `dashboard/main.js` render charts and query Postgres through MCP.
- The model dropdown is populated from `model_info`.
- A polling bridge watches `ui_events` for the active `doc_id` and session.

Run it

1) Start the Revit add-in (MCP listening on `http://localhost:5005/mcp/`).
2) Ensure Postgres is configured and synced (`Db.SyncModel`).
3) Open `dashboard/index.html` in a browser and select your model.

Chat integration

- The dashboard creates a `session_id` and passes it into the embedded chat.
  - n8n embed (`dashboard/n8n.html`) includes `metadata.session_id`, `metadata.doc_id`, and `metadata.model_name`.
- Your workflow can push UI updates using MCP `Ui.PushEvent` so the dashboard reacts immediately.

Event contract (Ui.PushEvent)

```html
- Endpoint: `http://localhost:5005/mcp/`
- Body:
  { "action":"Ui.PushEvent", "event_type":"...", "payload":"<json>", "session_id":"<sid>", "doc_id":"<doc>" }
```

Supported events

- `model_summary`: payload `{ kind:'model_summary', by_category:[...], types_by_category:[...] }` → shows in "Recent Insight".
- `filter_summary`: payload `{ kind:'filter_summary', by_type:[{type,count}], sums:{area,volume,length} }` → shows in "Recent Insight".
- `set_filter`: payload `{ category?:string, level?:string }` → updates filters and reloads charts and table.
- `element_selection`: payload `{ ids:number[], category?:string, level?:string, apply_filters?:true }`
  - Populates the drill-through with those element IDs (capped to 500 for performance).
  - If `apply_filters` is true, also sets category/level filters.
- `params_upserted`: payload `{ updated:number, distinct_params?:number, names?:string[] }` → summarized in "Recent Insight".

Notes

- If your DB schema differs, adjust the SQL in `dashboard/main.js`.
- The event bridge polls every ~1.5s; tighten or switch to SSE/WebSocket if needed.
- Flowise embed accepts `sid` in URL but doesn’t set metadata by default; deep integration works best with n8n.
