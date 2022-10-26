using System;
using System.Linq;
using System.Collections.Generic;
using Libraries;
using UnityEngine;
using UnityEngine.SceneManagement;

class WorldPathController : MonoBehaviour
{
  public GameObject[] Arrows;
  public GameObject Background;

  static MapData _mapData;
  static Vector2 _mapLocation = new Vector2(0, 0);
  static IList<Vector2> _pathTaken = new List<Vector2>();

  public WorldPathController()
  {
    if(_mapData != null)
      return;
    var childPoints = new[] {
        new MapPoint {
          Event = PointEvent.Battle,
          BackgroundName = WorldPathBackgroundName.OneWayPath
        },
        new MapPoint {
          Event = PointEvent.Event,
          BackgroundName = WorldPathBackgroundName.WitchHut
        },
      };
    _mapData = new MapData
    {
      Points = new [] {
        new [] {
          new MapPoint {
            Event = PointEvent.None,
            Children = childPoints,
            BackgroundName = WorldPathBackgroundName.TwoWayPath,
            Visited = true
          },
        },
        childPoints
      }
    };
  }

  public void Start()
  {
    foreach(var arrow in Arrows)
      arrow.GetComponent<ArrowController>().OnArrowClicked += ArrowClicked;

    RefreshScene();
  }

  MapPoint CurrentMapPoint()
  {
    return _mapData.Points[(int)_mapLocation.x][(int)_mapLocation.y];
  }

  void RefreshScene()
  {
    Background.GetComponent<BackgroundController>().SetImage(CurrentMapPoint().BackgroundName);

    foreach(var arrow in Arrows)
    {
      if(arrow.GetComponent<ArrowController>().PathIndex < 0) {
        arrow.SetActive(_mapLocation.x > 0);
      } else {
        arrow.SetActive(CurrentMapPoint().Children != null);
      }
    }
  }

  void ArrowClicked(object sender, EventArgs e)
  {
    var nextIndex = (e as PathChosenEventArgs).PathIndex;
    if(nextIndex >= 0)
    {
      _pathTaken.Add(_mapLocation);
      _mapLocation = new Vector2(nextIndex, _mapLocation.y+1);
    } else {
      _mapLocation = _pathTaken.Last();
      _pathTaken.Remove(_mapLocation);
    }
    Debug.Log(_mapLocation);
    var newPoint = _mapData.Points[(int)_mapLocation.y][(int)_mapLocation.x];

    if(newPoint.Visited) {
      RefreshScene();
    } else {
      newPoint.Visited = true;

      switch(newPoint.Event)
      {
        case PointEvent.Battle:
          SceneDataHandler.UpdateData(new Dictionary<SceneDataKey, object> { [SceneDataKey.Enemy] = new EnemyConfig(MonsterName.Witch, newPoint.BackgroundName) });
          SceneManager.LoadScene("BattleScene");
          break;
        case PointEvent.Event:
          SceneDataHandler.UpdateData(new Dictionary<SceneDataKey, object> { [SceneDataKey.Event] = new EventConfig(EventName.WitchHut, WorldPathBackgroundName.WitchHut) });
          SceneManager.LoadScene("EventScene");
          break;
      }
    }
  }

  public void Update()
  {
  }
}