using System;
using UnityEngine;
using System.Collections.Generic;

public static class EventLibrary
{
  static IDictionary<EventName, EventDetails> _details = new Dictionary<EventName, EventDetails> {
    [EventName.WitchHut] = WitchHut()
  };

  public static IDictionary<EventName, EventDetails> Details => _details;

  static EventDetails WitchHut()
  {
    var initialShpiel = new TextEventStep("You come a cross a hut in a spooky swamp.");
    var goodResolution = new ResolutionEventStep("You find a new card", new List<ResolutionEventStep.Resolution> {
      new ResolutionEventStep.Resolution(ResolutionType.GainCard, CardName.IntoxicateThem)
    }, finalStep: true);
    var badResolution = new ResolutionEventStep("It's trapped! You take 3 damage", new List<ResolutionEventStep.Resolution> {
      new ResolutionEventStep.Resolution(ResolutionType.Health, -3)
    }, finalStep: true);

    return new EventDetails(EventName.WitchHut, EventImage.WitchHut, new List<EventStep> {
      initialShpiel,
      new QuestionEventStep("Do you investigate inside, or leave well alone?", new List<QuestionOption> {
        new QuestionOption("Explore inside", () => { if(UnityEngine.Random.Range(0, 10) <= 3) return 3; return 2; }),
        new QuestionOption("Ignore", () => -1)
      }),
      goodResolution,
      badResolution,
    });
  }
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
  protected bool _finalStep;
  public bool FinalStep => _finalStep;
}

public class EventStep<TData> : EventStep { }

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

public class QuestionEventStep : EventStep<QuestionEventStep.EventData>
{
  public QuestionEventStep(string text, IEnumerable<QuestionOption> options)
  {
    _type = EventStepType.Question;
    _data = new EventData(text, options);
  }

  public class EventData
  {
    string _text;
    public string Text => _text;
    IEnumerable<QuestionOption> _options;
    public IEnumerable<QuestionOption> Options => _options;

    public EventData(string text, IEnumerable<QuestionOption> options)
    {
      _text = text;
      _options = options;
    }
  }
}

public class QuestionOption
{
  string _text;
  public string Text => _text;
  Func<int> _resolution;
  public Func<int> Resolution => _resolution;

  public QuestionOption(string text, Func<int> resolution)
  {
    _text = text;
    _resolution = resolution;
  }
}

public class ResolutionEventStep : EventStep<ResolutionEventStep.EventData>
{
  public ResolutionEventStep(string text, IEnumerable<Resolution> resolutions, bool finalStep = false)
  {
    _type = EventStepType.Resolution;
    _data = new EventData(text, resolutions);
    _finalStep = finalStep;
  }

  public class EventData
  {
    string _text;
    public string Text => _text;
    IEnumerable<Resolution> _resolutions;
    public IEnumerable<Resolution> Resolutions => _resolutions;

    public EventData(string text, IEnumerable<Resolution> resolutions)
    {
      _text = text;
      _resolutions = resolutions;
    }
  }

  public class Resolution
  {
    ResolutionType _type;
    public ResolutionType Type => _type;
    object _change;
    public object Change => _change;

    public Resolution(ResolutionType type, object change)
    {
      _type = type;
      _change = change;
    }
  }
}

public enum ResolutionType
{
  Health,
  GainCard,
  Gold
}

public enum EventStepType
{
  Text,
  Question,
  Resolution,
  End,
}

public enum EventImage
{
  WitchHut
}