public class EventConfig : IContextObject
{
  EventName _eventName;
  public EventName EventName => _eventName;
  BackgroundName _backgroundName;
  public BackgroundName BackgroundName => _backgroundName;

  public EventConfig(EventName eventName, BackgroundName backgroundName)
  {
    _eventName = eventName;
    _backgroundName = backgroundName;
  }
}
