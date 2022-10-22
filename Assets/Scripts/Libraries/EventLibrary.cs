using UnityEngine;
using System.Collections.Generic;

public static class EventLibrary
{
  static IDictionary<EventName, EventDetails> _details = new Dictionary<EventName, EventDetails> {
    [EventName.WitchHut] = new EventDetails(EventName.WitchHut, EventImage.WitchHut, new List<EventStep> {
      new TextEventStep("Hey there cowboy"),
      new TextEventStep("Hey multiple lines, who knew")
    })
  };

  public static IDictionary<EventName, EventDetails> Details => _details;
}

public enum EventName
{
  WitchHut
}

public class EventDetails
{
  EventName _name;
  public EventName Name => _name;
  EventImage _image;
  public EventImage Image => _image;
  IList<EventStep> _steps;
  public IList<EventStep> Steps => _steps;

  public EventDetails(EventName name, EventImage image, IList<EventStep> steps)
  {
    _name = name;
    _image = image;
    _steps = steps;
  }
}

public class EventStep
{
  protected EventStepType _type;
  public EventStepType Type => _type;
  protected object _data;
  public object Data => _data;

  //todo: This whole thing stinks
  public bool Update(bool spacePressed)
  {
    switch(_type)
    {
      case EventStepType.Text:
        if(spacePressed)
          return true;
        break;
    }

    return false;
  }
}

public class EventStep<TData> : EventStep
{
}

public class TextEventStep : EventStep<TextEventStep.EventData>
{
  public TextEventStep(string text)
  {
    _type = EventStepType.Text;
    _data = new EventData(text);
  }

  public class EventData
  {
    string _text;
    public string Text => _text;

    public EventData(string text)
    {
      _text = text;
    }
  }
}

public enum EventStepType
{
  Text,
  Reward
}

public enum EventImage
{
  WitchHut
}