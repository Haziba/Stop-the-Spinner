using System;
using System.Linq;
using System.Collections.Generic;
using Libraries;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

class WorldPathController : MonoBehaviour
{
  public GameObject[] Arrows;
  public GameObject Background;

  static MapData _mapData;
  static Vector2 _mapLocation = new Vector2(0, 0);
  static IList<Vector2> _pathTaken = new List<Vector2>();

  public void Start()
  {
    Random.InitState(5);
    _mapData = State.WorldPath.MapData;
    
    foreach(var arrow in Arrows)
      arrow.GetComponent<ArrowController>().OnArrowClicked += ArrowClicked;

    RefreshScene();
  }

  MapPoint CurrentMapPoint()
  {
    return _mapData.Points[(int)_mapLocation.y][(int)_mapLocation.x];
  }

  void RefreshScene()
  {
    Background.GetComponent<BackgroundController>().SetImage(CurrentMapPoint().BackgroundName);

    foreach(var arrow in Arrows)
    {
      if(arrow.GetComponent<ArrowController>().PathIndex < 0) {
        arrow.SetActive(_mapLocation.y > 0);
      } else {
        arrow.SetActive(CurrentMapPoint().Children != null);
        switch (CurrentMapPoint().Children.Length)
        {
          case 0:
            arrow.SetActive(false);
            break; 
          case 1:
            arrow.SetActive(arrow.GetComponent<ArrowController>().PathIndex == 1);
            break;
          case 2:
            // 0 or 2
            arrow.SetActive(arrow.GetComponent<ArrowController>().PathIndex % 2 == 0);
            break;
          case 3:
            arrow.SetActive(true);
            break;
        }
      }
    }
  }

  void ArrowClicked(object sender, EventArgs e)
  {
    var nextIndex = (e as PathChosenEventArgs).PathIndex;
    if(nextIndex >= 0)
    {
      _pathTaken.Add(_mapLocation);
      // TODO:: not sure if this works, that might not always be the correct child
      // consider storing correct child or storing proper index number in each arrow
      // TODO:: Also this is a mess
      _mapLocation = new Vector2(Array.IndexOf(_mapData.Points[(int)_mapLocation.y+1], CurrentMapPoint().Children[Math.Min(CurrentMapPoint().Children.Length - 1, nextIndex)]), _mapLocation.y+1);
    } else {
      _mapLocation = _pathTaken.Last();
      _pathTaken.Remove(_mapLocation);
    }
    var newPoint = _mapData.Points[(int)_mapLocation.y][(int)_mapLocation.x];

    if(newPoint.Visited || newPoint.Event == PointEvent.None) {
      RefreshScene();
    } else {
      newPoint.Visited = true;

      switch(newPoint.Event)
      {
        case PointEvent.Battle:
          SceneDataHandler.UpdateData(new Dictionary<SceneDataKey, object> { [SceneDataKey.Enemy] = new EnemyConfig(newPoint.MonsterName, newPoint.BackgroundName) });
          SceneManager.LoadScene("BattleScene");
          break;
        case PointEvent.Event:
          SceneDataHandler.UpdateData(new Dictionary<SceneDataKey, object> { [SceneDataKey.Event] = new EventConfig(EventName.WitchHut, BackgroundName.WitchHut) });
          SceneManager.LoadScene("EventScene");
          break;
      }
    }
  }

  public void ClickDeck()
  {
    SceneManager.LoadScene("DeckScene");
  }

  public void ClickPlayerInfo()
  {
    SceneManager.LoadScene("PlayerInfoScene");
  }
}