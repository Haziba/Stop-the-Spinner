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
    _doCardPauseLength = 0.2f;
    _hand = _context.GO(ContextObjects.PlayerHand);
    _spinner = _context.GO(ContextObjects.PlayerSpinner);
    _playedCard = _context.GO(ContextObjects.PlayerPlayedCard);
    _meHealthBar = _context.GO(ContextObjects.PlayerHealthBar);
    _themHealthBar = _context.GO(ContextObjects.EnemyHealthBar);
    _drawPile = _context.GO(ContextObjects.PlayerDrawPile);
    _discardPile = _context.GO(ContextObjects.PlayerDiscardPile);
    _meManaCounter = _context.GO(ContextObjects.PlayerManaCounter);
    _themDamageAnchor = _context.GO(ContextObjects.Enemy);
    _playedCardTarget = new Vector3(0, -1f, -5f);
    _spinnerTarget = new Vector3(0, -1f, -6f);
    _spinnerOrigin = new Vector3(0, -8f, -6f);
    _meState = _context.Get<IContextObject>(ContextObjects.PlayerState) as AgentState;
    _themState = _context.Get<IContextObject>(ContextObjects.EnemyState) as AgentState;

    _hand.GetComponent<HandController>().OnCardClicked += OnCardClicked;
    _context.GO(ContextObjects.EndTurnButton).GetComponent<EndTurnButtonController>().OnClicked +=
      OnEndTurnClicked;
  }
  public override void Start()
  {
    base.Start();
    
    if (!AvailableCardIndexes().Any())
    {
      ChangeGameState(_themGameState);
      return;
    }

    _context.GO(ContextObjects.EndTurnButton).GetComponent<EndTurnButtonController>().EnableButton();

    //DebugCard(CardName.BiteThem, AgentStatusEffects.Focused, default(AgentStatusEffects));
  }

  public override void End()
  {
    _context.GO(ContextObjects.EndTurnButton).GetComponent<EndTurnButtonController>().DisableButton();
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

  protected override void UpdateInnerState(InnerState newState)
  {
    if(newState == InnerState.ChoosingCard)
      _context.GO(ContextObjects.EndTurnButton).GetComponent<EndTurnButtonController>().EnableButton();
    else
      _context.GO(ContextObjects.EndTurnButton).GetComponent<EndTurnButtonController>().DisableButton();
    
    base.UpdateInnerState(newState);
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
      
      _context.GO(ContextObjects.PlayerPlayedCard).GetComponent<PlayedCardController>().PlayCard(Agent.Player, _cardClickedEventArgs.CardName(), _cardClickedEventArgs.HandCard().Image());
      _context.GO(ContextObjects.PlayerHand).GetComponent<HandController>().RemoveCard(_cardClickedEventArgs.HandCard());
      _cardBeingDragged = true;
    }
    
    if (_cardBeingDragged)
    {
      if (Input.touches.Any())
      {
        var cardNewPosition = _context.Get<Camera>(ContextObjects.Camera).ScreenToWorldPoint(Input.GetTouch(0).position);
        cardNewPosition.z = _context.GO(ContextObjects.PlayerPlayedCard).transform.position.z;
        _context.GO(ContextObjects.PlayerPlayedCard).transform.position = cardNewPosition;
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
    _context.GO(ContextObjects.ToolTip).GetComponent<ToolTipController>().ShowToolTip(cardName);
  }

  public void OnCardClicked(object sender, EventArgs e)
  {
    var cardClickedEvent = e as CardClickedEventArgs;

    if(!IsActiveState() || _innerState != InnerState.ChoosingCard || !_meState.CanPlayCard(cardClickedEvent.CardName()))
      return;

    _cardClickedEventArgs = cardClickedEvent;
    _cardClickTimer = 0.2f;
  }

  void OnEndTurnClicked(object sender, EventArgs e)
  {
    EndTurn();
  }

	public void SpacePressed()
	{
		if(!SpinnerSpinning())
			return;
    
    StopSpinning();
	}
}
