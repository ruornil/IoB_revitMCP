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

---

## 📁 Project Structure

| Core Files                     | Purpose                                                                        |
| ------------------------------ | ------------------------------------------------------------------------------ |
| `App.cs`                       | Entry point for the Revit add-in. Starts and stops the MCP server.             |
| `McpServer.cs`                 | Initializes the HTTP listener and threads.                                     |
| `RequestHandler.cs`            | Routes incoming requests to appropriate ICommand implementations.              |
| `ICommand.cs`                  | Interface that all typed command classes implement.                            |


| Command Files                  | Purpose |
| ------------------------------ | -------------------------------------------------------------------------------- |
| `AddViewFilterCommand.cs`      | Creates view filters with visibility, color, line pattern, and fill overrides. |
| `ModifyElementsCommand.cs`       | Changes element types and sets parameter values. |
| `CreateSheetCommand.cs`        | Creates a new sheet using a specified title block. |
| `ExportToJsonCommand.cs`       | Exports elements and their parameters to JSON. |
| `FilterByParameterCommand.cs`  | Filters a list of elements based on parameter value. |
| `GetFamiliesAndTypesCommand.cs` | Lists all families and their types in the model. |
| `GetElementParametersCommand.cs`      | Retrieves parameters for one or more Revit elements.                          |
| `GetModelContext.cs`            | Retrieves model-level metadata such as name and save time and all project parameters and their metadata. |
| `ListElementsCommand.cs`       | Lists Revit elements of a given category. |
| `NewSharedParameter.cs`        | Creates and binds shared parameters from shared parameter file. |
| `PlaceViewsOnSheet.cs`         | Places views on a Revit sheet, stacking them from bottom-right up. |
| `PlanExecutorCommand.cs`       | Executes a stepwise plan, enabling command chaining. |
| `ListCategoriesCommand.cs`     | Enumerates all categories in the model. |
| `ListViewsCommand.cs`          | Lists views with metadata. |
| `ListSheetsCommand.cs`         | Lists sheets and title blocks. |
| `ListSchedulesCommand.cs`      | Lists schedule views in the model. |
| `SyncModelToSqlCommand.cs`     | Writes model data to PostgreSQL. |
| `QuerySqlCommand.cs`           | Executes arbitrary SQL queries against the PostgreSQL database. |

| Helper Files                   | Purpose                                                                        |
| ------------------------------ | ------------------------------------------------------------------------------ |
| `RevitHelpers.cs`              | Utility functions for element filtering and parameter setting.                 |
| `UiHelpers.cs`                 | Revit UI utilities (e.g., `TaskDialog`).                                       |

---

## 🚀 Usage Examples

### 🔹 Get Element Parameters

```json
{
  "action": "GetElementParameters",
  "element_ids": "123456,789012"
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
### 🔹 Get Model Context

```json
{
  "action": "GetModelContext"
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

### 🔹 Get Families and Types

```json
{
  "action": "GetFamiliesAndTypes"
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

### 🔹 Run an Arbitrary SQL Query

```json
{
  "action": "QuerySql",
  "sql": "SELECT * FROM revit_elements WHERE category = @cat",
  "params": "{ \"cat\": \"Walls\" }"
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
