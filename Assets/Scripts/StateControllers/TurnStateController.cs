using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnStateController : StateController
{
  protected InnerState _innerState;
  protected PerformCardEffect _cardEffect;

  protected IList<HUtilities.Countdown> _countdowns;

  protected GameState _meGameState;
  protected GameState _themGameState;

  protected float _doCardPauseLength;

  protected GameObject _hand;
  protected GameObject _spinner;
  protected GameObject _playedCard;
  protected GameObject _meHealthBar;
  protected GameObject _themHealthBar;

  protected Vector2 _playedCardTarget;
  protected Vector2 _playedCardOrigin;
  protected Vector2 _spinnerTarget;
  protected Vector2 _spinnerOrigin;

  protected AgentState _meState;
  protected AgentState _themState;

  public TurnStateController(ContextManager context) : base(context)
  {
    _countdowns = new List<HUtilities.Countdown>();
  }

  public override void Start()
  {
    ChangeGameState(_meGameState);
    _innerState = InnerState.ChoosingCard;
  }

  public override void Update()
  {
    for(var i = _countdowns.Count-1; i >= 0; i--)
      if(!_countdowns[i].Update())
        _countdowns.RemoveAt(i);
    
    switch(_innerState)
    {
      case InnerState.ChoosingCard:
        break;
      
      case InnerState.PlayedCardMovingToPosition:
        _playedCard.transform.position = new Vector3(HUtilities.MoveTowards(_playedCard.transform.position.x, _playedCardTarget.x, 2f), HUtilities.MoveTowards(_playedCard.transform.position.y, _playedCardTarget.y, 16f), _playedCard.transform.position.z);

        if(HUtilities.InPosition(_playedCard.transform.position, _playedCardTarget)) {
          OnPlayedCardInPosition();
          _countdowns.Add(new HUtilities.Countdown(_doCardPauseLength, DoCard));
        }
        break;

      case InnerState.SpinnerComingIn:
        _spinner.transform.position = new Vector3(0, HUtilities.MoveTowards(_spinner.transform.position.y, _spinnerTarget.y, 8f), _spinner.transform.position.z);
        if(HUtilities.InPosition(_spinner.transform.position, _spinnerTarget)) {
          _innerState = InnerState.SpinnerSpinning;
          _spinner.GetComponent<SpinnerController>().StartSpinning();
          OnSpinnerInPosition();
        }
        break;
      case InnerState.SpinnerSpinning:
        break;
      case InnerState.SpinnerGoingOut:
        _spinner.transform.position = new Vector3(0, HUtilities.MoveTowards(_spinner.transform.position.y, _spinnerOrigin.y, 8f), _spinner.transform.position.z);
        if(HUtilities.InPosition(_spinner.transform.position, _spinnerOrigin)) {
          EndTurn();
        }
        break;
    }
  }

  void DoCard()
  {
    // todo: Feels janky, maybe the Played Card Name should be held in this class instead idk
    _cardEffect = new PerformCardEffect(_playedCard.GetComponent<PlayedCardController>().CardName(), _meHealthBar, _themHealthBar, _meState, _themState);

    switch(_cardEffect.Perform())
    {
      case EffectOutcome.Continue:
        EndTurn();
        break;
      case EffectOutcome.SpinWheel:
        _innerState = InnerState.SpinnerComingIn;
        break;
    }
  }

  protected void StopSpinning()
  {
    var result = _spinner.GetComponent<SpinnerController>().StopSpinning();

    _cardEffect.ResolveSpinner(result);

    _countdowns.Add(new HUtilities.Countdown(1f, () => {
      _innerState = InnerState.SpinnerGoingOut;
    }));
  }

  public bool SpinnerSpinning()
  {
    return _spinner.GetComponent<SpinnerController>().IsSpinning();
  }

  public override void End()
  {
    _playedCard.GetComponent<PlayedCardController>().RemoveCard();
  }

  void EndTurn()
  {
    ChangeGameState(_themGameState);
  }
  
  protected virtual void OnSpinnerInPosition() { }
  protected virtual void OnPlayedCardInPosition() { }

  protected enum InnerState
  {
    ChoosingCard,
    PlayedCardMovingToPosition,

    SpinnerComingIn,
    SpinnerSpinning,
    DamageBeingDone,
    SpinnerGoingOut
  }
}
