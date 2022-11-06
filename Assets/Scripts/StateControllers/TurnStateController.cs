using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;
using Libraries;

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
  protected GameObject _drawPile;
  protected GameObject _discardPile;
  protected GameObject _meManaCounter;
  protected GameObject _themDamageAnchor;
  protected GameObject _meArmourCounter;
  protected GameObject _themArmourCounter;

  protected Vector3 _playedCardTarget;
  protected Vector3 _spinnerTarget;
  protected Vector3 _spinnerOrigin;

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

    _meState.RecoverMana();
    _meManaCounter.GetComponent<ManaCounterController>()
      .UpdateMana(_meState);
    
    _hand.GetComponent<HandController>().DrawCard();
  }

  public override void Update()
  {
    for(var i = _countdowns.Count-1; i >= 0; i--)
      if(!_countdowns[i].Update())
        _countdowns.RemoveAt(i);
    
    // todo: This may not be fit for purpose anymore
    switch(_innerState)
    {
      case InnerState.ChoosingCard:
        break;
      case InnerState.PlayedCardMovingToPosition:
        break;
      case InnerState.SpinnerComingIn:
        break;
      case InnerState.SpinnerSpinning:
        break;
      case InnerState.SpinnerGoingOut:
        break;
    }
  }

  protected void DebugCard(CardName cardName, AgentStatusEffects meState, AgentStatusEffects themState)
  {
    var handController = _hand.GetComponent<HandController>();
    var cardToPlay = handController.CardAt(Random.Range(0, handController.TotalCards()-1));
    handController.RemoveCard(cardToPlay);

    _playedCard.GetComponent<PlayedCardController>().PlayCard(Agent.Player, cardName, cardToPlay.Image());

    PlayCardFromHand();

    _hand.GetComponent<HandController>().DiscardCard(cardName);
  }

  protected void PlayCardFromHand()
  {
    _innerState = InnerState.PlayedCardMovingToPosition;

    _playedCard.transform.DOMove(_playedCardTarget, 0.5f)
      .OnComplete(() => {
        OnPlayedCardInPosition();
        _countdowns.Add(new HUtilities.Countdown(_doCardPauseLength, DoCard));
      });
  }

  protected int[] AvailableCardIndexes()
  {
    var handController = _context.Get<GameObject>(ContextObjects.EnemyHand).GetComponent<HandController>();
    return handController.AvailableCardIndexes(_meState);
  }
  
  void DoCard()
  {
    PerformCard(_playedCard.GetComponent<PlayedCardController>().CardName());
    _meState.SpendMana(CardLibrary.Cards[_playedCard.GetComponent<PlayedCardController>().CardName()].ManaCost);
    _meManaCounter.GetComponent<ManaCounterController>()
      .UpdateMana(_meState);
  }

  void PerformCard(CardName cardName)
  {
    // todo: Feels janky, maybe the Played Card Name should be held in this class instead idk
    _cardEffect = new PerformCardEffect(cardName, _meHealthBar, _themHealthBar, _meArmourCounter, _themArmourCounter, _meState, _themState);
    var performOutcome = _cardEffect.Perform();

    // todo: Now this bit is DEFINITELY janky
    _meArmourCounter.GetComponent<ArmourCounterController>().SetArmour(_meState.Armour());
    _themArmourCounter.GetComponent<ArmourCounterController>().SetArmour(_themState.Armour());

    switch(performOutcome.Outcome())
    {
      case EffectOutcome.Continue:
        EndTurn();
        break;
      case EffectOutcome.SpinWheel:
        _innerState = InnerState.SpinnerComingIn;
        _spinner.GetComponent<SpinnerController>().UpdateConfig(performOutcome.Data() as SpinnerConfiguration);
        _spinner.transform.DOMove(_spinnerTarget, 0.5f)
          .OnComplete(() => {
            _innerState = InnerState.SpinnerSpinning;
            _spinner.GetComponent<SpinnerController>().StartSpinning(_meState.StatusEffects());
            OnSpinnerInPosition();
          });
        break;
    }
  }

  protected void StopSpinning()
  {
    var result = _spinner.GetComponent<SpinnerController>().StopSpinning();

    _cardEffect.ResolveSpinner(result);

    if (result != SpinnerResult.Miss)
    {
      var damageBall = _context.Get<GameObject>(ContextObjects.Instantiator).GetComponent<InitiatorController>()
        .Instantiate(_context.Get<GameObject>(ContextObjects.DamageBallPrefab));
      //todo: Refactor
      damageBall.GetComponent<DamageBallController>().SetImage(_playedCard.GetComponent<PlayedCardController>().CardName());
      damageBall.transform.position = _spinner.transform.position + new Vector3(0, 0, -2f);
      damageBall.transform
        .DOMove(new Vector3(_themDamageAnchor.transform.position.x, _themDamageAnchor.transform.position.y, damageBall.transform.position.z), 0.5f)
        .SetEase(Ease.InExpo)
        .OnComplete(() =>
        {
          _context.Get<Camera>(ContextObjects.Camera).GetComponent<CameraController>().Shake(0.5f, 1f, () =>
          {
            _context.Get<GameObject>(ContextObjects.Instantiator).GetComponent<InitiatorController>().Destroy(damageBall);
            HideSpinner();
          });
      });
    }
    else
      HideSpinner();
  }

  void HideSpinner()
  {
    var hideSpinnerCountdown = new HUtilities.Countdown(1f, () =>
    {
      _innerState = InnerState.SpinnerGoingOut;

      _spinner.transform.DOMove(_spinnerOrigin, 0.5f)
        .OnComplete(EndTurn);
    });
    _countdowns.Add(hideSpinnerCountdown);
  }

  public bool SpinnerSpinning()
  {
    return _spinner.GetComponent<SpinnerController>().IsSpinning();
  }

  public override void End()
  {
  }

  void EndTurn()
  {
    if (!_meState.Alive() || !_themState.Alive())
    {
      ChangeGameState(GameState.EndBattle);
      return;
    }
    
    var discardCardSequence = DOTween.Sequence();
    _playedCard.GetComponent<PlayedCardController>().HideCard();
    discardCardSequence.Append(_playedCard.transform.DOMove(_discardPile.transform.position, 0.5f));
    discardCardSequence.OnComplete(() =>
      {
        _hand.GetComponent<HandController>().DiscardCard(_playedCard.GetComponent<PlayedCardController>().CardName());
        _playedCard.GetComponent<PlayedCardController>().RemoveCard();

        ChangeGameState(_themGameState);
      });
    discardCardSequence.Play();
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
