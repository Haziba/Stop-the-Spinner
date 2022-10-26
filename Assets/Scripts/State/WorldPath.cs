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
  }
}