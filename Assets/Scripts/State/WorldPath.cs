using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace State
{
  public static class WorldPath
  {
    static MapData _mapData;

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
        Enumerable.Range(0, 7).Select(num => new List<MapPoint>
        {
          new MapPoint()
        }).ToArray();

      var extraPoints = new[] { 1, 2, 2, 3, 3, 3, 4 };
      extraPoints.Shuffle();
      
      // todo: Refactor into nice functions or something
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

      foreach (var mapPoint in newMapPoints.SelectMany(nMP => nMP))
      {
        mapPoint.Visited = true;
        
        if(mapPoint.Children == null)
          Debug.Log(mapPoint);

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

      _mapData = new MapData
      {
        Points = newMapPoints.Select(layer => layer.ToArray()).ToArray()
      };
    }
  }
}