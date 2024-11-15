using System;
using System.Collections.Generic;
using System.Linq;
using Libraries;
using UnityEngine;
using Random = UnityEngine.Random;

namespace State
{
  public static class WorldPath
  {
    static MapData _mapData;
    
    static MapPoint _forcedPoint = null; /*new MapPoint
      {
        Event = PointEvent.Event,
        Visited = false,
        BackgroundName = BackgroundName.WitchHut
      };*/

    public static MapData MapData
    {
      get
      {
        if (_mapData == null)
          InitMapData();
        return _mapData;
      }
    }

    static void InitMapData()
    {
      var newMapPoints =
        Enumerable.Range(0, 4/*7*/).Select(num => new List<MapPoint>
        {
          new MapPoint()
        }).ToArray();

      if (_forcedPoint != null)
      {
        _mapData = new MapData
        {
          Points = new[]
          {
            new []
            {
              new MapPoint { BackgroundName = BackgroundName.OneWayPath, Children = new [] { _forcedPoint }, Visited = false }
            }, new [] { _forcedPoint }
          }
        };

        return;
      }

      var extraPoints = new[] { 1, 2, 2, 3, 3, 3, 4 };
      extraPoints.Shuffle();
      
      // TODO:: Refactor into nice functions or something
      for (var i = 0; i < newMapPoints.Count() - 2; i++)
      {
        var children = newMapPoints[i].Select(newMapPoint => new List<MapPoint>()).ToArray();

        children[Random.Range(0, children.Count() - 1)].Add(newMapPoints[i + 1].First());
        
        for (var j = 0; j < extraPoints[i]; j++)
        {
          var childrenWithRoomToSpare = children.Where(childs => childs.Count < 3).ToArray();

          if (!childrenWithRoomToSpare.Any())
            break;
          
          var newMapPoint = new MapPoint();
          newMapPoints[i + 1].Add(newMapPoint);
          childrenWithRoomToSpare[Random.Range(0, childrenWithRoomToSpare.Length - 1)].Add(newMapPoint);
        }
        
        for (var j = 0; j < newMapPoints[i].Count(); j++)
          newMapPoints[i][j].Children = children[j].ToArray();
      }

      var allMapPoints = newMapPoints.SelectMany(nMP => nMP).ToArray();

      foreach (var mapPoint in allMapPoints)
      {
        mapPoint.Visited = true;

        switch (mapPoint.Children.Length)
        {
          case 0:
            mapPoint.BackgroundName = BackgroundName.DeadEnd;
            break;
          case 1:
            mapPoint.BackgroundName = BackgroundName.OneWayPath;
            break;
          case 2:
            mapPoint.BackgroundName = BackgroundName.TwoWayPath;
            break;
          case 3:
            mapPoint.BackgroundName = BackgroundName.ThreeWayPath;
            break;
        }
      }

      // TODO:: Big nice easy refactor jobbie to be done here
      var availableMapPoints = allMapPoints.Where(mP => mP.Event == PointEvent.None && !mP.Children.Any()).ToList();
      availableMapPoints.Shuffle();
      for (var i = 0; i < 3; i++)
      {
        availableMapPoints[i].Visited = false;
        availableMapPoints[i].Event = PointEvent.Event;
        availableMapPoints[i].BackgroundName = BackgroundName.WitchHut;
      }

      availableMapPoints = allMapPoints.Where(mP => mP.Event == PointEvent.None).ToList();
      availableMapPoints.Shuffle();
      var mapPointIndex = 0;
      // TODO:: Could do with shuffling the monster names
      foreach (MonsterName monsterName in Enum.GetValues(typeof(MonsterName)))
      {
        availableMapPoints[mapPointIndex].Visited = false;
        availableMapPoints[mapPointIndex].Event = PointEvent.Battle;
        availableMapPoints[mapPointIndex].MonsterName = monsterName;
        mapPointIndex++;
        
        // TODO:: Eh I don't really like this but maybe all of this functionality needs refactoring
        if(mapPointIndex >= availableMapPoints.Count)
          break;
      }

      // TODO:: Gotta be a better way to make sure the first point doesn't have anything on it
      newMapPoints.First().First().Event = PointEvent.None;
      
      _mapData = new MapData
      {
        Points = newMapPoints.Select(layer => layer.ToArray()).ToArray()
      };
    }
  }
}