using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EventController : MonoBehaviour
{
  public GameObject Text;
  public GameObject Background;
  public GameObject OptionsCanvas;
  public GameObject OptionPrefab;

  EventName _event = EventName.WitchHut;
  int _eventStep;
  EventStep _currentEventStep;

  IList<GameObject> _options = new List<GameObject>();

  // Start is called before the first frame update
  void Start()
  {
    _eventStep = -1;

    //todo: Get the _event from the scene change data
    var sceneData = SceneDataHandler.GetData();
    var eventData = (sceneData[SceneDataKey.Event] as EventConfig);
    _event = eventData.EventName;
    Background.GetComponent<BackgroundController>().SetImage(eventData.BackgroundName);

    StartStep(0);
  }

  // Update is called once per frame
  void Update()
  {
    UpdateStep();
  }

  void StartStep(int newStep)
  {
    if(_eventStep > 0) {
      EndStep();
    }
    if(newStep < 0) {
      SceneManager.LoadScene("WorldPathScene");
      return;
    }

    _eventStep = newStep;
    _currentEventStep = EventLibrary.Details[_event].Steps[_eventStep];
    StartEventStep();
  }

  void EndStep()
  {
    switch(_currentEventStep.Type)
    {
      case EventStepType.Question:
        ClearOptions();
        break;
    }
  }

  void StartEventStep()
  {
    switch(_currentEventStep.Type)
    {
      case EventStepType.Text:
        // todo: Smell
        UpdateText(((_currentEventStep as TextEventStep).Data as TextEventStep.EventData).Text);
        break;
      case EventStepType.Question:
        UpdateText(((_currentEventStep as QuestionEventStep).Data as QuestionEventStep.EventData).Text);
        SetOptions(((_currentEventStep as QuestionEventStep).Data as QuestionEventStep.EventData).Options);
        break;
      case EventStepType.Resolution:
        UpdateText(((_currentEventStep as ResolutionEventStep).Data as ResolutionEventStep.EventData).Text);
        break;
    }
  }

  void SetOptions(IEnumerable<QuestionOption> options)
  {
    var optArray = options.ToArray();
    for(var i = 0; i < optArray.Count(); i++)
      AddOption(optArray[i], i);
  }

  void ClearOptions()
  {
    foreach(var option in _options)
      Destroy(option);
    _options = new List<GameObject>();
  }

  void AddOption(QuestionOption option, int i)
  {
    var opt = Instantiate(OptionPrefab, transform.position, Quaternion.identity);
    opt.transform.SetParent(OptionsCanvas.transform, false);
    opt.transform.position = new Vector3(0, -1.7f + 1f * i, 0);
    opt.transform.Find("Text").gameObject.GetComponent<Text>().text = option.Text;
    opt.GetComponent<EventOptionController>().OnOptionClicked += (object sender, EventArgs e) => {
      var nextStepId = (sender as EventOptionController).NextStepId();
      StartStep(nextStepId);
    };
    // todo: Eesh big smell around here
    opt.GetComponent<EventOptionController>().SetResolution(option.Resolution);
    _options.Add(opt);
  }

  void UpdateStep()
  {
    var goNextPressed = GoNextPressed();

    if(goNextPressed && _currentEventStep.FinalStep) {
      StartStep(-1);
      return;
    }

    var newStep = _currentEventStep.Update(goNextPressed);
    if(newStep != _eventStep)
      StartStep(newStep);
  }

  bool GoNextPressed()
  {
    return Input.GetKeyDown("space") || Input.touchCount > 0;
  }

  void UpdateText(string text)
  {
    Text.GetComponent<Text>().text = text;
  }
}
