namespace Hot.Core.Tool;

public static class ToolBag
{
  private static readonly Dictionary<string, ITool> tools = new Dictionary<string, ITool>();

  public static void AddTool(ITool tool)
  {
    tools[tool.Name] = tool;
  }

  public static T GetTool<T>(string name) where T : ITool
  {
    // TODO: create custom error class if tool doesn't exist
    return (T)tools[name];
  }
}