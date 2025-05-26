# Revit MCP Plugin

This project implements a Model Context Protocol (MCP) server and command interface for Autodesk Revit, enabling both typed command execution and dynamic scripting capabilities (e.g. via IronPython). It supports HTTP-based interactions to trigger Revit operations externally.

---

## 🔧 Features

* **HTTP Listener**: Listens on `http://localhost:5005/mcp/` to receive JSON-based commands.
* **Command Pattern**: Typed commands implement the `ICommand` interface.
* **Dynamic Scripting**: Execute IronPython scripts with access to `doc`, `uidoc`, Revit API types, and helper methods.
* **External Event Handling**: Safely dispatches commands into Revit’s UI thread.

---

## 📁 Project Structure

| File                      | Purpose                                                                                         |
| ------------------------- | ----------------------------------------------------------------------------------------------- |
| `App.cs`                  | Entry point for the Revit add-in. Starts and stops the MCP server.                              |
| `McpServer.cs`            | Initializes the HTTP listener and threads.                                                      |
| `RequestHandler.cs`       | Routes incoming requests to appropriate ICommand implementations or IronPython script executor. |
| `ICommand.cs`             | Interface that all typed command classes implement.                                             |
| `GetParametersCommand.cs` | Retrieves all parameters of a selected Revit element.                                           |
| `ListElementsCommand.cs`  | Lists Revit elements of a given category.                                                       |
| `RevitHelpers.cs`         | Utility functions for IronPython scripting (e.g., element filtering, parameter setting).        |
| `UiHelpers.cs`            | Revit UI utilities (e.g., `TaskDialog`).                                                        |

---

## 🚀 Usage

### Trigger a Typed Command

POST to `http://localhost:5005/mcp/`

```json
{
  "action": "GetParameters"
}
```

### Run a Python Script

```json
{
  "action": "RunPython",
  "script": "print(doc.Title)"
}
```

---

## 🛠 Future Development

* [ ] Add more typed Revit commands (e.g., create walls, place families)
* [ ] Add gRPC or WebSocket transport (optional)
* [ ] Unit tests and logging support
* [ ] External DLL loader for fully isolated command execution

---

## 📄 License

MIT License (or project-specific license to be added)
