namespace _00_Scripts.Events
{
  public class LoadSceneEvent
  {
    public string SceneName
    {
      get;
    }

    public LoadSceneEvent(string sceneName) => SceneName = sceneName;
  }
}
