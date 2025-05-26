// ICommand.cs - interface for structured MCP command classes
using Autodesk.Revit.UI;
using System.Collections.Generic;

public interface ICommand
{
    Dictionary<string, object> Execute(UIApplication app, Dictionary<string, string> input);
}