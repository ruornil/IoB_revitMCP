Metabase Embedded Dashboard
===========================

This is a copy of the dashboard, trimmed to host an embedded Metabase dashboard while still using your MCP to pick a model and discover `doc_id`/`model_name`.

What this is
- A simple page with:
  - MCP endpoint and model picker (reads `model_info`).
  - A Metabase iframe that you can point at:
    - A Metabase Public Link URL (least secure, simplest), or
    - A Signed Embed URL from your backend (recommended), or
    - A template URL containing placeholders like `{doc_id}` and `{model_name}` that we replace client‑side (works with Public Links or if you explicitly map URL params in Metabase filters).

What this is not
- This page does NOT mint signed embed tokens in the browser. Metabase signed embeds require server‑side signing.

How to use
1) Start MCP so the model list can load (same as existing dashboard).
2) Open `dashboard_metabase/index.html` in a browser.
3) Choose your model. The page tracks `doc_id` and `model_name`.
4) Provide one of the following and click "Load Metabase":
   - Direct URL: paste a Metabase Public Link or already-signed embed URL.
   - Template URL: paste a URL with placeholders `{doc_id}` and/or `{model_name}` and optional query parameters. Example:
     `https://metabase.your.org/public/dashboard/abcd1234?doc_id={doc_id}&model={model_name}#bordered=false&titled=false`
   - Backend endpoint: provide an HTTP endpoint you own that returns a signed embed URL for the current `doc_id`/`model_name`. We `GET` it with `?doc_id=...&model_name=...` and use the returned string as the iframe `src`.

Suggested backend (Signed Embedding)
- Create a tiny endpoint in your existing app that mints a Metabase signed embed URL.
- Your backend will:
  - Hold the Metabase embedding secret key.
  - Accept `doc_id` and `model_name` from the client.
  - Produce the signed JWT payload with `resource` and `params` (matching your dashboard filter mappings), then respond with the full `https://metabase.host/embed/dashboard/<JWT>#bordered=false&titled=false` URL.
  - Return it as plain text or JSON `{ url: "..." }`.

Notes
- If your Metabase dashboard uses filters, map those to either:
  - Signed embed parameters (in the JWT `params`), or
  - URL parameters (if you configured field filters to read from URL—works with public links; see Metabase docs).
- This page reuses styles from `iofbim-tokens.css` (copied here for convenience).

