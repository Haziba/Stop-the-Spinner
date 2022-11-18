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
    _hand = _context.Get<GameObject>(ContextObjects.EnemyHand);
    _spinner = _context.Get<GameObject>(ContextObjects.EnemySpinner);
    _playedCard = _context.Get<GameObject>(ContextObjects.EnemyPlayedCard);
    _meHealthBar = _context.Get<GameObject>(ContextObjects.EnemyHealthBar);
    _themHealthBar = _context.Get<GameObject>(ContextObjects.PlayerHealthBar);
    _drawPile = _context.Get<GameObject>(ContextObjects.EnemyDrawPile);
    _discardPile = _context.Get<GameObject>(ContextObjects.EnemyDiscardPile);
    _meManaCounter = _context.Get<GameObject>(ContextObjects.EnemyManaCounter);
    _themDamageAnchor = _context.Get<GameObject>(ContextObjects.PlayerDamageAnchor);
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
    
    // todo: I don't like this. Maybe there should be an event on TurnStateController, but that feels
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
    var handController = _context.Get<GameObject>(ContextObjects.EnemyHand).GetComponent<HandController>();
    var availableCardIndexes = AvailableCardIndexes();

    if (!availableCardIndexes.Any())
    {
      EndTurn();
      return;
    }

    var cardToPlay = handController.CardAt(availableCardIndexes[Random.Range(0, availableCardIndexes.Length-1)]);
    
    handController.RemoveCard(cardToPlay);

    _context.Get<GameObject>(ContextObjects.EnemyPlayedCard).GetComponent<PlayedCardController>().PlayCard(Agent.Enemy, cardToPlay.CardName(), cardToPlay.Image());

    PlayCardFromHand();
  }
}
