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
    try
    {
      var tool = (T)tools[name];
      return tool;
    }
    catch (KeyNotFoundException)
    {
      throw new MissingToolException(name);
    }
  }
}