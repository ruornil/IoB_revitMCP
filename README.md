# Revit MCP Plugin

This project implements a Model Context Protocol (MCP) server and command interface for Autodesk Revit, enabling typed command execution and (future) dynamic scripting capabilities. It supports HTTP-based interactions to trigger Revit operations externally.

---

## 🔧 Features

* **HTTP Listener**: Listens on `http://localhost:5005/mcp/` to receive JSON-based commands.
* **Command Pattern**: Typed commands implement the `ICommand` interface.
* **External Event Handling**: Safely dispatches commands into Revit’s UI thread.
* **View Filter Automation**: Create and apply filters with overrides directly from HTTP requests.

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

---

## 🛠 Development Notes

* ✅ C# 7.3 compatibility enforced
* ✅ Removed IronPython scripting (future feature)
* ✅ `OverrideGraphicSettings` supports projection color, line pattern, fill color, and fill pattern
* ✅ Views are placed on sheets in columns starting from bottom-right, going top-left.

---

## 📄 License

This project is licensed under the Apache License 2.0.

It includes contributions assisted by OpenAI's ChatGPT and uses Autodesk Revit API (which is proprietary and not included in this repository).

See the NOTICE file for attribution and licensing notes.

You are free to use, modify, and distribute this code commercially or privately, as long as you include proper attribution and comply with the Apache 2.0 terms.
