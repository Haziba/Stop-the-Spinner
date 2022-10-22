using System.Collections.Generic;

public static class SceneDataHandler
{
  static IDictionary<SceneDataKey, object> _data;

  public static void UpdateData(IDictionary<SceneDataKey, object> data)
  {
    _data = data;
  }

  public static IDictionary<SceneDataKey, object> GetData()
  {
    var data = _data;
    _data = null;
    return data;
  }
}

public enum SceneDataKey
{
  Enemy
}