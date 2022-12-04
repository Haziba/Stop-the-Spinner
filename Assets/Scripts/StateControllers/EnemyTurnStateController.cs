using System.Linq;
using UnityEngine;

public class EnemyTurnStateController : TurnStateController
{
  HUtilities.Countdown _playCardCountdown;
  
  public EnemyTurnStateController(ContextManager context) : base(context)
  {
    _meGameState = GameState.EnemyTurn;
    _themGameState = GameState.PlayerTurn;
    _doCardPauseLength = 2f;
    _hand = _context.GO(ContextObjects.EnemyHand);
    _spinner = _context.GO(ContextObjects.EnemySpinner);
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

  public override void Update()
  {
    base.Update();
    
    // TODO:: I don't like this. Maybe there should be an event on TurnStateController, but that feels
    // janky too. Maybe an overridable method? Ah whatever, this will do for now
    if (_innerState != InnerState.ChoosingCard || (_playCardCountdown != null && _playCardCountdown.Running()))
      return;
    
    _playCardCountdown = new HUtilities.Countdown(1f, PlayCard);
    _countdowns.Add(_playCardCountdown);
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
