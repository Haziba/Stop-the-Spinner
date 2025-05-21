using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Libraries;
using Random = UnityEngine.Random;

public class TurnStateController : StateController
{
  protected InnerState _innerState;
  protected PerformCardEffect _cardEffect;

  protected IList<HUtilities.Countdown> _countdowns;

  protected GameState _meGameState;
  protected GameState _themGameState;

  protected float _doCardPauseLength;

  protected GameObject _hand;
  protected IDictionary<SpinnerType, GameObject> _spinners;
  protected GameObject _playedCard;
  protected GameObject _meHealthBar;
  protected GameObject _themHealthBar;
  protected GameObject _drawPile;
  protected GameObject _discardPile;
  protected GameObject _meManaCounter;
  protected GameObject _themDamageAnchor;
  protected GameObject _meArmourCounter;
  protected GameObject _themArmourCounter;
  protected GameObject _hitResult;

  protected Vector3 _playedCardTarget;
  protected Vector3 _spinnerTarget;
  protected Vector3 _spinnerOrigin;

  protected AgentState _meState;
  protected AgentState _themState;
  
  private ISpinnerController _spinnerController;

  public TurnStateController(ContextManager context) : base(context)
  {
    _countdowns = new List<HUtilities.Countdown>();
    _hitResult = context.GO(ContextObjects.HitResult);
  }

  public override void Start()
  {
    ChangeGameState(_meGameState);
    UpdateInnerState(InnerState.ChoosingCard);

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
    
    // TODO:: This may not be fit for purpose anymore
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
    UpdateInnerState(InnerState.PlayedCardMovingToPosition);

    _playedCard.transform.DOMove(_playedCardTarget, 0.5f)
      .OnComplete(() => {
        OnPlayedCardInPosition();
        _countdowns.Add(new HUtilities.Countdown(_doCardPauseLength, DoCard));
      });
  }

  protected int[] AvailableCardIndexes()
  {
    var handController = _context.GO(ContextObjects.EnemyHand).GetComponent<HandController>();
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
    // TODO:: Feels janky, maybe the Played Card Name should be held in this class instead idk
    _cardEffect = new PerformCardEffect(cardName, _meHealthBar, _themHealthBar, _meArmourCounter, _themArmourCounter, _meState, _themState);
    var performOutcome = _cardEffect.Perform();

    switch(performOutcome.Outcome())
    {
      case EffectOutcome.Continue:
        EndPlayCard();
        break;
      case EffectOutcome.SpinWheel:
        SetSpinnerController(performOutcome.Data() as ISpinnerConfiguration);
        UpdateInnerState(InnerState.SpinnerComingIn);
        _spinnerController.UpdateConfig(performOutcome.Data() as ISpinnerConfiguration);
        _spinners[_spinnerController.SpinnerType].transform.DOMove(_spinnerTarget, 0.5f)
          .OnComplete(() => {
            UpdateInnerState(InnerState.SpinnerSpinning);
            _spinnerController.StartSpinning(_meState.StatusEffects());
            OnSpinnerInPosition();
          });
        break;
    }
  }

  void EndPlayCard()
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
        UpdateInnerState(InnerState.ChoosingCard);
      });
    discardCardSequence.Play();
  }

  protected void StopSpinning()
  {
    var result = _spinnerController.StopSpinning();
    if (result == SpinnerResult.StillSpinning) return;
    
    _cardEffect.ResolveSpinner(result);

    _hitResult.GetComponent<SpriteFromEnumController>().ShowEnumValue(result.ToString());
    var hitResultRenderer = _hitResult.GetComponent<SpriteFromEnumController>().SpriteRenderer;
    
    _hitResult.transform.localScale = new Vector3(0.9f, 0.9f, 1f);
    var color = hitResultRenderer.color;
    color.a = 0;
    hitResultRenderer.color = color;

    Sequence seq = DOTween.Sequence();

    seq.Append(_hitResult.transform.DOScale(1.2f, 0.2f))
      .Join(hitResultRenderer.DOFade(1f, 0.2f))
      .Append(_hitResult.transform.DOScale(1f, 0.5f))
      .Join(hitResultRenderer.DOFade(0f, 0.5f));

    if (result != SpinnerResult.Miss)
    {
      var damageBall = _context.GO(ContextObjects.Instantiator).GetComponent<InitiatorController>()
        .Instantiate(_context.GO(ContextObjects.DamageBallPrefab));
      // TODO:: Refactor
      damageBall.GetComponent<DamageBallController>().SetImage(_playedCard.GetComponent<PlayedCardController>().CardName());
      damageBall.transform.position = _spinners[_spinnerController.SpinnerType].transform.position + new Vector3(0, 0, -2f);
      damageBall.transform
        .DOMove(new Vector3(_themDamageAnchor.transform.position.x, _themDamageAnchor.transform.position.y, damageBall.transform.position.z), 0.5f)
        .SetEase(Ease.InExpo)
        .OnComplete(() =>
        {
          // TODO:: This really, really doesn't belong. Maybe both characters have a state manager that would handle this, and it handles health / mana / armour as well
          if(this is PlayerTurnStateController)
            _context.GO(ContextObjects.Enemy).GetComponent<MonsterController>().UpdateState(_context.Get<IContextObject>(ContextObjects.EnemyState) as AgentState);
          
          // TODO:: Oh wow even more stuff that doesn't belong here looks like an anti-pattern emerging
          _meHealthBar.GetComponent<HealthBarController>().SetHealthAndArmour(_meState.Health(), _meState.Armour());
          _themHealthBar.GetComponent<HealthBarController>().SetHealthAndArmour(_themState.Health(), _themState.Armour());
          
          _context.Get<Camera>(ContextObjects.Camera).GetComponent<CameraController>().Shake(0.2f, 0.5f, () =>
          {
            _context.GO(ContextObjects.Instantiator).GetComponent<InitiatorController>().Destroy(damageBall);
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
      UpdateInnerState(InnerState.SpinnerGoingOut);

      _spinners[_spinnerController.SpinnerType].transform.DOMove(_spinnerOrigin, 0.5f)
        .OnComplete(EndPlayCard);
    });
    _countdowns.Add(hideSpinnerCountdown);
  }

  public bool SpinnerSpinning()
  {
    return _spinnerController.IsSpinning();
  }

  public void SetSpinnerController(ISpinnerConfiguration outcome)
  {
    switch (outcome.SpinnerType)
    {
      case SpinnerType.Wheel:
        _spinnerController = _spinners[outcome.SpinnerType].GetComponent<SpinnerController>();
        break;
      case SpinnerType.Music:
        _spinnerController = _spinners[outcome.SpinnerType].GetComponent<MusicSpinnerController>();
        break;
      default:
        throw new ArgumentOutOfRangeException();
    }
  }

  public ISpinnerController GetSpinnerController()
  {
    return _spinnerController;
  }

  public override void End()
  {
  }

  protected void EndTurn()
  {
    ChangeGameState(_themGameState);
  }

  protected virtual void UpdateInnerState(InnerState newState)
  {
    _innerState = newState;
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
