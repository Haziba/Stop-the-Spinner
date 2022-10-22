using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventController : MonoBehaviour
{
  public GameObject Text;
  public GameObject Background;

  EventName _event = EventName.WitchHut;
  int _eventStep;
  EventStep _currentEventStep;

  // Start is called before the first frame update
  void Start()
  {
    _eventStep = -1;

    //todo: Get the _event from the scene change data
    Background.GetComponent<BackgroundController>().SetImage(EventLibrary.Details[_event].Image);

    StartStep();
  }

  // Update is called once per frame
  void Update()
  {
    UpdateStep();
  }

  void StartStep()
  {
    if(_eventStep > 0) {
      EndStep();
    }
    _eventStep++;
    _currentEventStep = EventLibrary.Details[_event].Steps[_eventStep];
    StartEventStep();
  }

  void EndStep()
  {
  }

  void StartEventStep()
  {
    switch(_currentEventStep.Type)
    {
      case EventStepType.Text:
        // todo: Smell
        UpdateText(((_currentEventStep as TextEventStep).Data as TextEventStep.EventData).Text);
        break;
    }
  }

  void UpdateStep()
  {
    var stepComplete = _currentEventStep.Update(Input.GetKeyDown("space") || Input.touchCount > 0);
    if(stepComplete)
      StartStep();
  }

  void UpdateText(string text)
  {
    Text.GetComponent<Text>().text = text;
  }
}
