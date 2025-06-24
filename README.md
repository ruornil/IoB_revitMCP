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
| `ChangeFamilyAndType.cs`       | Changes the family and type for elements. |
| `CreateSheetCommand.cs`        | Creates a new sheet using a specified title block. |
| `ExportToJsonCommand.cs`       | Exports elements and their parameters to JSON. |
| `FilterByParameterCommand.cs`  | Filters a list of elements based on parameter value. |
| `GetFamiliesAndTypesCommand.cs` | Lists all families and their types in the model. |
| `GetParametersCommand.cs`      | Retrieves all parameters of a selected Revit element. |
| `GetParametersByID.cs`         | Retrieves all parameters of Revit elements selected by ID. |
| `GetProjectInfo.cs`            | Retrieves model-level metadata such as name and save time. |
| `GetProjectParametersCommand.cs` | Retrieves all project parameters and their metadata. |
| `ListElementsCommand.cs`       | Lists Revit elements of a given category. |
| `NewSharedParameter.cs`        | Creates and binds shared parameters from shared parameter file. |
| `PlaceViewsOnSheet.cs`         | Places views on a Revit sheet, stacking them from bottom-right up. |
| `PlanExecutorCommand.cs`       | Executes a stepwise plan, enabling command chaining. |
| `SetParameters.cs`             | Sets multiple parameters on one or more elements by ID or selection. |

| Helper Files                   | Purpose                                                                        |
| ------------------------------ | ------------------------------------------------------------------------------ |
| `RevitHelpers.cs`              | Utility functions for element filtering and parameter setting.                 |
| `UiHelpers.cs`                 | Revit UI utilities (e.g., `TaskDialog`).                                       |

---

## 🚀 Usage Examples

### 🔹 Get Parameters

```json
{
  "action": "GetParameters"
}
```
### 🔹 Get Parameters By ID

```json
{
  "action": "GetParametersById",
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

### 🔹 Set Multiple Parameters on Selected or Targeted Elements

```json
{
  "action": "SetParameters",
  "element_ids": "[12345, 67890]",
  "parameters": "{\"Mark\": \"Wall-A1\", \"Comments\": \"Checked\"}"
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
