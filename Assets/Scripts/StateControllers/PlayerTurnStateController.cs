using System;
using System.Linq;
using UnityEngine;

public class PlayerTurnStateController : TurnStateController
{
  public PlayerTurnStateController(ContextManager context) : base(context)
  {
    _meGameState = GameState.PlayerTurn;
    _themGameState = GameState.EnemyTurn;
    _doCardPauseLength = 1f;
    _hand = _context.Get<GameObject>(ContextObjects.PlayerHand);
    _spinner = _context.Get<GameObject>(ContextObjects.PlayerSpinner);
    _playedCard = _context.Get<GameObject>(ContextObjects.PlayerPlayedCard);
    _meHealthBar = _context.Get<GameObject>(ContextObjects.PlayerHealthBar);
    _themHealthBar = _context.Get<GameObject>(ContextObjects.EnemyHealthBar);
    _drawPile = _context.Get<GameObject>(ContextObjects.PlayerDrawPile);
    _discardPile = _context.Get<GameObject>(ContextObjects.PlayerDiscardPile);
    _meManaCounter = _context.Get<GameObject>(ContextObjects.PlayerManaCounter);
    _themDamageAnchor = _context.Get<GameObject>(ContextObjects.Enemy);
    _meArmourCounter = _context.Get<GameObject>(ContextObjects.PlayerArmourCounter);
    _themArmourCounter = _context.Get<GameObject>(ContextObjects.EnemyArmourCounter);
    _playedCardTarget = new Vector3(0, -1f, -5f);
    _spinnerTarget = new Vector3(0, -1f, -6f);
    _spinnerOrigin = new Vector3(0, -8f, -6f);
    _meState = _context.Get<IContextObject>(ContextObjects.PlayerState) as AgentState;
    _themState = _context.Get<IContextObject>(ContextObjects.EnemyState) as AgentState;

    _hand.GetComponent<HandController>().OnCardClicked += OnCardClicked;
  }
  public override void Start()
  {
    base.Start();
    
    if (!AvailableCardIndexes().Any())
    {
      ChangeGameState(_themGameState);
      return;
    }

    //DebugCard(CardName.BiteThem, AgentStatusEffects.Focused, default(AgentStatusEffects));
  }

  public override void Update()
  {
    if(_innerState == InnerState.SpinnerSpinning)
      if(Input.GetKeyDown("space") || Input.touchCount > 0)
        SpacePressed();

    base.Update();
  }

  public void OnCardClicked(object sender, EventArgs e)
  {
    var cardClickedEvent = e as CardClickedEventArgs;

    if(!IsActiveState() || _innerState != InnerState.ChoosingCard || !_meState.CanPlayCard(cardClickedEvent.CardName()))
      return;

    _context.Get<GameObject>(ContextObjects.PlayerPlayedCard).GetComponent<PlayedCardController>().PlayCard(Agent.Player, cardClickedEvent.CardName(), cardClickedEvent.HandCard().Image());
    _context.Get<GameObject>(ContextObjects.PlayerHand).GetComponent<HandController>().RemoveCard(cardClickedEvent.HandCard());

    PlayCardFromHand();
  }

	public void SpacePressed()
	{
		if(!SpinnerSpinning())
			return;
    
    StopSpinning();
	}
}
