using System.Linq;
using UnityEngine;

public class EnemyTurnStateController : TurnStateController
{
  public EnemyTurnStateController(ContextManager context) : base(context)
  {
    _meGameState = GameState.EnemyTurn;
    _themGameState = GameState.PlayerTurn;
    _doCardPauseLength = 2f;
    _hand = _context.Get<GameObject>(ContextObjects.EnemyHand);
    _spinner = _context.Get<GameObject>(ContextObjects.EnemySpinner);
    _playedCard = _context.Get<GameObject>(ContextObjects.EnemyPlayedCard);
    _meHealthBar = _context.Get<GameObject>(ContextObjects.EnemyHealthBar);
    _themHealthBar = _context.Get<GameObject>(ContextObjects.PlayerHealthBar);
    _drawPile = _context.Get<GameObject>(ContextObjects.EnemyDrawPile);
    _discardPile = _context.Get<GameObject>(ContextObjects.EnemyDiscardPile);
    _meManaCounter = _context.Get<GameObject>(ContextObjects.EnemyManaCounter);
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

    _countdowns.Add(new HUtilities.Countdown(1f, PlayCard));
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
    var handController = _context.Get<GameObject>(ContextObjects.EnemyHand).GetComponent<HandController>();
    var availableCardIndexes = AvailableCardIndexes();
    var cardToPlay = handController.CardAt(availableCardIndexes[Random.Range(0, availableCardIndexes.Length-1)]);
    
    handController.RemoveCard(cardToPlay);

    _context.Get<GameObject>(ContextObjects.EnemyPlayedCard).GetComponent<PlayedCardController>().PlayCard(Agent.Enemy, cardToPlay.CardName(), cardToPlay.Image());

    PlayCardFromHand();
  }
}
