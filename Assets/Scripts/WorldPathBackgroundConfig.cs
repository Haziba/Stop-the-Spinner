public class EventConfig : IContextObject
{
  EventName _eventName;
  public EventName EventName => _eventName;
  WorldPathBackgroundName _backgroundName;
  public WorldPathBackgroundName BackgroundName => _backgroundName;

  public EventConfig(EventName eventName, WorldPathBackgroundName backgroundName)
  {
    _eventName = eventName;
    _backgroundName = backgroundName;
  }
}

//todo: Doesn't belong here
public enum WorldPathBackgroundName
{
  WitchHut,
  TwoWayPath,
  OneWayPath,
}