# Revit MCP Plugin

This project provides a Revit add-in that exposes a **Model Context Protocol (MCP)** server.
It allows remote automation of Revit through simple HTTP requests.
Commands are strongly typed and executed through the Revit API.
The server can optionally synchronize model data to PostgreSQL where it becomes
searchable by an AI agent.  An accompanying **n8n** workflow connects ChatGPT to
this add‑in so natural language requests are converted to the JSON commands
described below. Dynamic scripting was removed for now but may return in the
future.

---

## 🔧 Features

* **HTTP Listener** – serves `http://localhost:5005/mcp/` for JSON-based commands.
* **Command Pattern** – strongly typed command classes implement `ICommand`.
* **External Event Handling** – safely dispatches actions onto Revit’s UI thread.
* **View Filter Automation** – create and apply filters with overrides from HTTP requests.
* **Database Sync & SQL** – export model data to PostgreSQL and run queries via `QuerySql`.
* **Vector Search Tools** – resolve category names or API concepts with `RevitBuiltinCategories` and `RevitApiVectorDB`.
* **Plan Execution** – chain multiple commands in a single request.
* **LLM Integration** – use the provided n8n workflow to translate chat messages into commands.
* **Async Queue** – long-running plans can be queued via `EnqueuePlan` and executed in the background. Results are stored with a job status so n8n can poll for completion.
* **In-memory Caching** – frequently used metadata like views and parameter bindings are cached per model session.

---

## 📁 Project Structure

| Core Files               | Purpose                                                                 |
| ------------------------ | ----------------------------------------------------------------------- |
| `App.cs`                 | Entry point for the Revit add-in. Starts and stops the MCP server.      |
| `McpServer.cs`           | Initializes the HTTP listener and threads.                              |
| `RequestHandler.cs`      | Routes incoming requests to appropriate ICommand implementations.       |
| `ICommand.cs`            | Interface that all typed command classes implement.                     |

| Command Files            | Purpose                                                                 |
|------------------------- |------------------------------------------------------------------------ |
| `AddViewFilter`          | Add a graphical view filter based on a parameter rule.                  |
| `CreateSheet`            | Create a new sheet with a given title block.                            |
| `ExecutePlan`            | Chain multiple commands in a single plan.                               |
| `ExportToJson`           | Export selected categories to JSON.                                     |
| `FilterByParameter`      | Filter element list by a parameter's value.                             |
| `ListCategories`         | List all Revit categories.                                              |
| `ListElements`           | List all elements of a specified category.                              |
| `ListElementParameters`  | Retrieve parameters for specified elements.                             |
| `ListFamiliesAndTypes`   | Retrieve all families and their types in the model.                     |
| `ListModelContext`       | Return active model name, path, and project info.                       |
| `ListSheets`             | List all Revit sheets.                                                  |
| `ListSchedules`          | List all schedules in the model.                                        |
| `ListViews`              | List all Revit views.                                                   |
| `CaptureToolState`       | Inspect active view and selection.                     |
| `ModifyElements`         | Update types and/or parameters for elements.                            |
| `NewSharedParameter`     | Create and bind a shared parameter to categories.                       |
| `PlaceViewsOnSheet`      | Place view(s) on a sheet with layout options.                           |
| `QuerySqlCommand.cs`     | Executes arbitrary SQL queries against the PostgreSQL database.         |
| `SyncModelToSql`         | Save active model data to PostgreSQL for querying.                      |

| Helper Files             | Purpose                                                                 |
| ------------------------ | ----------------------------------------------------------------------- |
| `RevitHelpers.cs`        | Utility functions for element filtering and parameter setting.          |
| `UiHelpers.cs`           | Revit UI utilities (e.g., `TaskDialog`).                                |

| Data Files               | Purpose                                                                 |
| ------------------------ | ----------------------------------------------------------------------- |
| `PostgresDb.cs`          | Utility to access upsert, modify, retrieve data from PostgresDB         |

---

## Command Summary

| Command                    | Required JSON fields                                                        | Optional JSON fields                                                      | Resulting keys                                                                                                                            |
| -------------------------- | --------------------------------------------------------------------------- | ------------------------------------------------------------------------- | ----------------------------------------------------------------------------------------------------------------------------------------- |
| **AddViewFilter**          | `action`, `category`, `filter_name`, `parameter`, `value`                   | override options (`visible`, `color`, `line_pattern`, `fill_color`, etc.) | `filterId` on success, `status` and error info if failing\                                                                                |
| **CreateSheet**            | `action`, `title_block_name`                                                | –                                                                         | On success: `sheet_id`, `sheet_name`, `sheet_number`                                                                                      |
| **ExecutePlan**            | `action`, `steps` (array of sub-commands)\                                  | –                                                                         | `results` array containing each sub-command’s output; `status` overall\                                                                   |
| **ExportToJson**           | `action` with optional `categories` list (defaults to Walls)\               | –                                                                         | `elements` (instance and type in their respective tables) array with id, name, category and parameter values\                             |
| **FilterByParameter**      | `action`, `param`, `value`, `input_elements` (JSON list)\                   | –                                                                         | `elements` list containing `{ Id, Name }` of matching elements\                                                                           |
| **ListCategories**         | `action` plus a PostgreSQL connection string (via config/env/`conn_file`)\  | –                                                                         | `categories` list describing each Revit category\                                                                                         |
| **ListElementParameters**  | `action` plus `element_ids` (comma list) or selected elements\              | `param_names` comma list, connection string options via `conn_file` etc.                            | `parameters` keyed by element id and `parameter_names` list\                                                                              |
| **ListElementsByCategory** | `action` (name `ListElementsByCategory`), `category` (defaults to Walls)\   | –                                                                         | `elements` list with id and name\                                                                                                         |
| **ListFamiliesAndTypes**   | `action` (`GetFamilyAndTypes`)                                              | `class_name` to filter, optional database connection                      | Returns `types` list with family, type, id, category, guid, doc_id,info\                                                                  |
| **ListSchedules**          | `action` and DB connection string\                                          | –                                                                         | `schedules` list with id, name and category\                                                                                              |
| **ListModelContext**       | `action` only                                                               | –                                                                         | Returns model metadata including `model_name`, `guid`, `last_saved`, and lists of project parameters\                                     |
| **ListViews**              | `action` and DB connection string\                                          | –                                                                         | `views` list with metadata including scale and associated sheet id\                                                                       |
| **CaptureToolState**       | `action`
                           | –
                       | `tool_state` object describing active view and selected elements\
   |
| **ListSheets**             | `action` and DB connection string\                                          | –                                                                         | `sheets` list with name, number and title block                                                                                           |
| **ModifyElements**         | `action`, `changes` (JSON array)\                                           | –                                                                         | `status`, per-element results with `type_changed`, `updated_parameters`, `skipped_parameters`                                             |
| **NewSharedParameter**     | `action`, `parameter_name`, `parameter_group`, `categories`, `binding_type` | –                                                                         | `parameter`, `categories` on success or error details\                                                                                    |
| **PlaceViewsOnSheet**      | `action`, `sheet_id`, `view_ids`                                            | `offsetRight` (mm)\                                                       | `status`, `placed`, `unplaced`, `remaining_view_ids` and message\                                                                         |
| **QuerySql**               | `action`, `sql` query, and DB connection string\                            | `params` (JSON object) for parameterized SQL queries\                     | `results` from database query or error info\                                                                                              |
| **SyncModelToSql**         | `action` and DB connection string\                                          | –                                                                         | On success: `updated`, `model_name`, `project_info`, `project_parameters`, `guid`, `last_saved`; returns `up_to_date` if nothing changed\ |

## Usage Examples

### 🔹 List Element Parameters

```json
{
  "action": "ListElementParameters",
  "element_ids": "123456,789012",
  "param_names": "Mark,Comments"
}
```

### 🔹 Create a New Sheet

```json
{
  "action": "CreateSheet",
  "title_block_name": "A1 1024 x 768mm"
}
```

### 🔹 Place Views on a Sheet

```json
{
  "action": "PlaceViewsOnSheet",
  "sheet_id": "1108327",
  "view_ids": "744829,744830",
  "offsetRight": "120"
}
```

### 🔹 Create and Bind a Shared Parameter

```json
{
  "action": "NewSharedParameter",
  "parameter_name": "Pset_WallCommon.AcousticRating[Type]",
  "parameter_group": "PG_IFC",
  "categories": "Walls",
  "binding_type": "Type"
}
```

### 🔹 Modify Element Types and Parameters

```json
{
  "action": "ModifyElements",
  "changes": [
    { "element_id": 12345, "new_type_name": "36\" x 84\"" },
    { "element_id": 12345, "parameters": { "Mark": "Wall-A1" } }
  ]
}
```

### 🔹 List Model Context

```json
{
  "action": "ListModelContext"
}
```

### 🔹 Export Elements to JSON

```json
{
  "action": "ExportToJson",
  "categories": "Walls,Doors"
}
```

### 🔹 Create and Apply a View Filter

```json
{
  "action": "AddViewFilter",
  "category": "Walls",
  "filter_name": "ColoredExternalWalls",
  "parameter": "Top is Attached",
  "value": "No",
  "visible": "true",
  "color": "255,0,0",
  "line_pattern": "Dashed",
  "fill_color": "255,255,0",
  "fill_pattern": "Solid Fill"
}
```

### 🔹 Chain Commands Using Plan Execution

```json
{
  "action": "ExecutePlan",
  "steps": [
    { "action": "ListElementsByCategory", "params": { "category": "Walls" } },
    { "action": "FilterByParameter", "params": { "param": "Top is Attached", "value": "No" } }
  ]
}
```

### 🔹 List Categories

```json
{
  "action": "ListCategories"
}
```

### 🔹 List Elements by Category

```json
{
  "action": "ListElements",
  "category": "Walls"
}
```

### 🔹 List Families and Types

```json
{
  "action": "ListFamiliesAndTypes"
}
```

### 🔹 List Views

```json
{
  "action": "ListViews"
}
```

### 🔹 List Sheets

```json
{
  "action": "ListSheets"
}
```

### 🔹 List Schedules

```json
{
  "action": "ListSchedules"
}
```

### 🔹 Filter Elements by Parameter

```json
{
  "action": "FilterByParameter",
  "param": "Mark",
  "value": "A1",
  "input_elements": "[{\"Id\":12345},{\"Id\":67890}]"
}
```

### 🔹 Sync Model Data to PostgreSQL

```json
{
  "action": "SyncModelToSql"
}
```

Add `"async": "true"` to run the sync in the background. The response includes a `job_id` which can be looked up in `mcp_queue`.

```json
{
  "action": "SyncModelToSql",
  "async": "true",
  "conn_file": "revit-conn.txt"
}
```

### 🔹 Run an Arbitrary SQL Query

```json
{
  "action": "QuerySql",
  "sql": "SELECT * FROM revit_elements WHERE category = @cat",
  "params": "{ \"cat\": \"Walls\" }"
}
```

### 🔹 Queue a Plan for Async Execution

Queued plans run in the background and results are stored in the `mcp_queue` table. Poll the `status` field (e.g. via `QuerySql`) until it becomes `done` to retrieve the `result` JSON.

```json
{
  "action": "EnqueuePlan",
  "plan": "[{ \"action\": \"ListElements\", \"params\":{\"category\":\"Walls\"}}]",
  "conn_file": "revit-conn.txt"
}
```

## 🔌 Configuring the PostgreSQL Connection

The plugin looks for a connection string in several locations:

1. `App.config` under the key `revit`
2. Environment variable `REVIT_DB_CONN`
3. A file path provided via the `conn_file` parameter
4. A `revit-conn.txt` file next to the add-in DLL

Example `App.config` snippet:

```xml
<configuration>
  <connectionStrings>
    <add name="revit" connectionString="Host=localhost;Database=revit;Username=user;Password=pass"/>
  </connectionStrings>
</configuration>
```

---

## 🚀 Standalone Extractor

A lightweight console application `RevitExtractor` is included for batch exporting model data without running the MCP HTTP server. Build the project in `RevitExtractor/RevitExtractor.csproj` and run it with:

```bash
RevitExtractor.exe "path/to/model.rvt" "Host=localhost;Database=revit;Username=user;Password=pass"
```

The tool opens the specified model in Revit automation mode and writes elements, element types and their parameters to PostgreSQL using the same schema as the add‑in.

---

## 🛠 Development Notes

* ✅ C# 7.3 compatibility enforced.
* ✅ Removed IronPython scripting (may return later).
* ✅ `OverrideGraphicSettings` now supports projection color, line pattern, fill color and fill pattern.
* ✅ Sheets arrange views starting bottom-right and fill leftward then upward.
* ✅ Model data syncs to PostgreSQL tables including `model_info` with project metadata.
* ✅ SQL queries can be issued via `QuerySql` for AI-driven analysis.

---

## 📄 License

This project is licensed under the Apache License 2.0.

It includes contributions assisted by OpenAI's ChatGPT and uses Autodesk Revit API (which is proprietary and not included in this repository).

See the NOTICE file for attribution and licensing notes.

You are free to use, modify, and distribute this code commercially or privately, as long as you include proper attribution and comply with the Apache 2.0 terms.
