using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyTurnStateController : TurnStateController
{
  HUtilities.Countdown _playCardCountdown;
  
  public EnemyTurnStateController(ContextManager context) : base(context)
  {
    _meGameState = GameState.EnemyTurn;
    _themGameState = GameState.PlayerTurn;
    _doCardPauseLength = 1f;
    _hand = _context.GO(ContextObjects.EnemyHand);
    _spinners = new Dictionary<SpinnerType, GameObject>
    {
      [SpinnerType.Wheel] = _context.GO(ContextObjects.EnemySpinner),
      [SpinnerType.Music] = _context.GO(ContextObjects.EnemySpinner),
    };
    _playedCard = _context.GO(ContextObjects.EnemyPlayedCard);
    _meHealthBar = _context.GO(ContextObjects.EnemyHealthBar);
    _themHealthBar = _context.GO(ContextObjects.PlayerHealthBar);
    _drawPile = _context.GO(ContextObjects.EnemyDrawPile);
    _discardPile = _context.GO(ContextObjects.EnemyDiscardPile);
    _meManaCounter = _context.GO(ContextObjects.EnemyManaCounter);
    _themDamageAnchor = _context.GO(ContextObjects.PlayerDamageAnchor);
    _playedCardTarget = new Vector3(0, 1f, -5f);
    _spinnerTarget = new Vector3(0, 1f, -6f);
    _spinnerOrigin = new Vector3(0, 8f, -6f);
    _meState = _context.Get<IContextObject>(ContextObjects.EnemyState) as AgentState;
    _themState = _context.Get<IContextObject>(ContextObjects.PlayerState) as AgentState;
  }

  public override void Start()
  {
    base.Start();
    
    if (!AvailableCardIndexes().Any())
    {
      ChangeGameState(_themGameState);
      return;
    }
  }

  protected override void UpdateInnerState(InnerState newState)
  {
    if (newState == InnerState.ChoosingCard)
    {
      _playCardCountdown = new HUtilities.Countdown(1f, PlayCard);
      _countdowns.Add(_playCardCountdown);
    }

    base.UpdateInnerState(newState);
  }

  protected override void OnPlayedCardInPosition()
  {
    _playedCard.GetComponent<PlayedCardController>().RevealCard();
  }

  protected override void OnSpinnerInPosition()
  {
    var stopIn = 1f + (float)(new System.Random().NextDouble()) * 3f;
    _countdowns.Add(new HUtilities.Countdown(stopIn, StopSpinning));
  }

  void PlayCard()
  {
    var handController = _context.GO(ContextObjects.EnemyHand).GetComponent<HandController>();
    var availableCardIndexes = AvailableCardIndexes();

    if (!availableCardIndexes.Any())
    {
      EndTurn();
      return;
    }

    var cardToPlay = handController.CardAt(availableCardIndexes[Random.Range(0, availableCardIndexes.Length-1)]);
    
    handController.RemoveCard(cardToPlay);

    _context.GO(ContextObjects.EnemyPlayedCard).GetComponent<PlayedCardController>().PlayCard(Agent.Enemy, cardToPlay.CardName(), cardToPlay.Image());

    PlayCardFromHand();
  }
}
