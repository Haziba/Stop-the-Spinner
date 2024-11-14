using System;
using System.Linq;
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
  int _currentStepNumber;
  EventStep _currentEventStep;
  int _lastTouchCount;
  bool _readyForInput = false;

  IList<GameObject> _options = new List<GameObject>();

  // Start is called before the first frame update
  void Start()
  {
    _currentStepNumber = -1;

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
    if(_currentStepNumber > 0) {
      EndStep();
    }
    if(newStep < 0) {
      SceneManager.LoadScene("WorldPathScene");
      return;
    }

    _currentStepNumber = newStep;
    _currentEventStep = EventLibrary.Events[_event].Steps[_currentStepNumber];
    StartEventStep();
    UpdateLastTouchCount();
    _readyForInput = true;
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
        // TODO:: Smell
        UpdateText(((_currentEventStep as TextEventStep).Data as TextEventStep.EventData).Text);
        break;
      case EventStepType.Question:
        UpdateText(((_currentEventStep as QuestionEventStep).Data as QuestionEventStep.EventData).Text);
        SetOptions(((_currentEventStep as QuestionEventStep).Data as QuestionEventStep.EventData).Options);
        break;
      case EventStepType.Resolution:
        UpdateText(((_currentEventStep as ResolutionEventStep).Data as ResolutionEventStep.EventData).Text);
        foreach (var resolution in
                 ((_currentEventStep as ResolutionEventStep).Data as ResolutionEventStep.EventData).Resolutions)
          HandleResolution(resolution);
        break;
    }
  }

  void HandleResolution(ResolutionEventStep.Resolution resolution)
  {
    switch(resolution.Type)
    {
      case ResolutionType.Health:
        throw new NotImplementedException();
      case ResolutionType.Armour:
        throw new NotImplementedException();
      case ResolutionType.GainCard:
        Player.Instance.GainCard((CardName)resolution.Change);
        break;
      case ResolutionType.Gold:
        throw new NotImplementedException();
      default:
        throw new NotImplementedException();
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
    opt.GetComponent<EventOptionController>().OnOptionClicked += (object sender, EventArgs e) =>
    {
      Debug.Log("Option clicked!");
      var nextStepId = (sender as EventOptionController).NextStepId();
      StartStep(nextStepId);
    };
    opt.GetComponent<EventOptionController>().SetResolution(option.Resolution);
    _options.Add(opt);
  }

  void UpdateStep()
  {
    if (GoNextPressed() && _readyForInput)
    {
      Debug.Log("Ready for input and go next clicked");
      _readyForInput = false;
      
      if (_currentEventStep.FinalStep)
      {
        StartStep(-1);
        return;
      }
      
      switch(_currentEventStep.Type)
      {
        case EventStepType.Text:
          StartStep(_currentStepNumber + 1);
          break;
      }
    }
  }

  void UpdateLastTouchCount()
  {
    _lastTouchCount = Input.touchCount;
  }

  bool GoNextPressed()
  {
    var newTouch = Input.touchCount > _lastTouchCount;
    UpdateLastTouchCount();

    if (!_readyForInput)
      return false;
    
    return Input.GetKeyDown("space") || newTouch;
  }

  void UpdateText(string text)
  {
    Text.GetComponent<Text>().text = text;
  }
}
