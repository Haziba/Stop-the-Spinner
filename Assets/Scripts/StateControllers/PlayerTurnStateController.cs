using System;
using System.Linq;
using UnityEngine;

public class PlayerTurnStateController : TurnStateController
{
  CardClickedEventArgs _cardClickedEventArgs;
  float _cardClickTimer;
  bool _cardBeingDragged;

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
    
    if (_cardClickedEventArgs != null)
      HandleCardBeingClicked();

    base.Update();
  }

  void HandleCardBeingClicked()
  {
    if (_cardClickTimer >= 0)
    {
      if (!Input.touches.Any())
      {
        ShowCardTooltip(_cardClickedEventArgs.CardName());
        _cardClickedEventArgs = null;
        return;
      }
      
      _cardClickTimer -= Time.deltaTime;
      if (_cardClickTimer > 0)
        return;
      
      _context.Get<GameObject>(ContextObjects.PlayerPlayedCard).GetComponent<PlayedCardController>().PlayCard(Agent.Player, _cardClickedEventArgs.CardName(), _cardClickedEventArgs.HandCard().Image());
      _context.Get<GameObject>(ContextObjects.PlayerHand).GetComponent<HandController>().RemoveCard(_cardClickedEventArgs.HandCard());
      _cardBeingDragged = true;
    }
    
    if (_cardBeingDragged)
    {
      if (Input.touches.Any())
      {
        var cardNewPosition = _context.Get<Camera>(ContextObjects.Camera).ScreenToWorldPoint(Input.GetTouch(0).position);
        cardNewPosition.z = 0;
        _context.Get<GameObject>(ContextObjects.PlayerPlayedCard).transform.position = cardNewPosition;
      }
      else
      {
        _cardBeingDragged = false;
        _cardClickedEventArgs = null;
        if(_playedCard.transform.position.y >= -3)
          PlayCardFromHand();
        else
        {
          _playedCard.GetComponent<PlayedCardController>().RemoveCard();
          _hand.GetComponent<HandController>().AddCard(_playedCard.GetComponent<PlayedCardController>().CardName());
        }
      }
    }
  }

  void ShowCardTooltip(CardName cardName)
  {
    Debug.Log("Show tooltip - " + cardName);
    _context.Get<GameObject>(ContextObjects.ToolTip).GetComponent<ToolTipController>().ShowToolTip(cardName);
  }

  public void OnCardClicked(object sender, EventArgs e)
  {
    var cardClickedEvent = e as CardClickedEventArgs;

    if(!IsActiveState() || _innerState != InnerState.ChoosingCard || !_meState.CanPlayCard(cardClickedEvent.CardName()))
      return;

    _cardClickedEventArgs = cardClickedEvent;
    _cardClickTimer = 0.2f;
  }

	public void SpacePressed()
	{
		if(!SpinnerSpinning())
			return;
    
    StopSpinning();
	}
}
