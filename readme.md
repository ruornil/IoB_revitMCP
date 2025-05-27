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

| File                           | Purpose                                                                        |                                          |
| ------------------------------ | ------------------------------------------------------------------------------ | ---------------------------------------- |
| `App.cs`                       | Entry point for the Revit add-in. Starts and stops the MCP server.             |                                          |
| `McpServer.cs`                 | Initializes the HTTP listener and threads.                                     |                                          |
| `RequestHandler.cs`            | Routes incoming requests to appropriate ICommand implementations.              |                                          |
| `ICommand.cs`                  | Interface that all typed command classes implement.                            |                                          |
| `GetParametersCommand.cs`      | Retrieves all parameters of a selected Revit element.                          |                                          |
| `ListElementsCommand.cs`       | Lists Revit elements of a given category.                                      |                                          |
| `FilterByParameterCommand.cs`  | Filters a list of elements based on parameter value.                           |                                          |
| `PlanExecutorCommand.cs`       | Executes a stepwise plan, enabling command chaining.                           |                                          |
| `AddViewFilterCommand.cs`      | Creates view filters with visibility, color, line pattern, and fill overrides. |                                          |
| `NewSharedParameterCommand.cs` | Creates and binds shared parameters from shared parameter file.                |                                          |
| `SetParametersCommand.cs`      | Sets multiple parameters on one or more elements by ID or selection.           |                                          |
| `RevitHelpers.cs`              | Utility functions for element filtering and parameter setting.                 |                                          |
| `UiHelpers.cs`                 | Revit UI utilities (e.g., `TaskDialog`).                                       | Revit UI utilities (e.g., `TaskDialog`). |

---

## 🚀 Usage Examples

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

---

## 📄 License

MIT License.
