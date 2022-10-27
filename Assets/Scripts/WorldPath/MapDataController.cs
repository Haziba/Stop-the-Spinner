/*using System;
using System.IO;*/

using System;
using UnityEngine;

public class MapDataController : MonoBehaviour
{
  /*public MapData _data;
  string _savePath;

  public void Start()
  {
    _savePath = Application.persistentDataPath + "/gamedata.json";
    Debug.Log(_savePath);

    if(File.Exists(_savePath)) {
        _data = JsonUtility.FromJson<MapData>(File.ReadAllText(_savePath));
        Debug.Log(JsonUtility.ToJson(_data));
        if(_data == null)
            _data = new MapData();
    } else {
        File.Create(_savePath);
        _data = new MapData();
    }
  }

  public void UpdateMapData(MapData data) {
    var jsonData = JsonUtility.ToJson(data);
    File.WriteAllText(_savePath, jsonData);
    _data = data;
    Debug.Log(jsonData);
    Debug.Log("Update complete");
	}

	public MapData MapData() {
		return _data;
	}*/
}

public class MapData
{
  public MapPoint[][] Points;
}

public class MapPoint
{
  public PointEvent Event;
  public MapPoint[] Children;
  public BackgroundName BackgroundName;
  public bool Visited;

  public MapPoint()
  {
    Children = Array.Empty<MapPoint>();
  }
}

public enum PointEvent
{
  None,
  Battle,
  Event
}