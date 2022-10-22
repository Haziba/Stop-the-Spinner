using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

class WorldPathController : MonoBehaviour
{
  public GameObject[] Arrows;

  MapData _mapData;
  Vector2 _mapLocation;

  public void Start()
  {
    _mapLocation = new Vector2(0, 0);

    var childPoints = new[] {
        new MapPoint {
          Event = PointEvent.Battle
        },
        new MapPoint {
          Event = PointEvent.Event
        },
      };
    _mapData = new MapData
    {
      Points = new [] {
        new [] {
          new MapPoint {
            Event = PointEvent.None,
            Children = childPoints,
            Visited = true
          },
        },
        childPoints
      }
    };

    foreach(var arrow in Arrows)
    {
      if(arrow.GetComponent<ArrowController>().PathIndex < 0) {
        arrow.SetActive(_mapLocation.x > 0);
      } else {
        arrow.SetActive(CurrentMapPoint().Children.Length > 0);
      }
      arrow.GetComponent<ArrowController>().OnArrowClicked += ArrowClicked;
    }
  }

  MapPoint CurrentMapPoint()
  {
    return _mapData.Points[(int)_mapLocation.x][(int)_mapLocation.y];
  }

  void ArrowClicked(object sender, EventArgs e)
  {
    var nextIndex = (e as PathChosenEventArgs).PathIndex;
    var newPoint = _mapData.Points[(int)_mapLocation.y + 1][nextIndex];

    switch(newPoint.Event)
    {
      case PointEvent.Battle:
        SceneDataHandler.UpdateData(new Dictionary<SceneDataKey, object> { [SceneDataKey.Enemy] = new EnemyConfig(EnemyName.Flaps) });
        SceneManager.LoadScene("BattleScene");
        break;
      case PointEvent.Event:
        Debug.Log("Events coming soon");
        break;
    }
  }

  public void Update()
  {
  }
}