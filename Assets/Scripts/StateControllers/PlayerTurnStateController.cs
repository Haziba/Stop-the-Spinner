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
    _context.Get<GameObject>(ContextObjects.EndTurnButton).GetComponent<EndTurnButtonController>().OnClicked +=
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

    _context.Get<GameObject>(ContextObjects.EndTurnButton).GetComponent<EndTurnButtonController>().EnableButton();

    //DebugCard(CardName.BiteThem, AgentStatusEffects.Focused, default(AgentStatusEffects));
  }

  public override void End()
  {
    _context.Get<GameObject>(ContextObjects.EndTurnButton).GetComponent<EndTurnButtonController>().DisableButton();
  }

  public override void Update()
  {
    if(_innerState == InnerState.SpinnerSpinning)
      if(Input.GetKeyDown("space") || Input.touchCount > 0)
        SpacePressed();
    
    if (_cardClickedEventArgs != null)
      HandleCardBeingClicked();
    
    //todo: There should be some sort of event firer when the turn state changes. Maybe an overridable method
    if(_innerState == InnerState.ChoosingCard)
      _context.Get<GameObject>(ContextObjects.EndTurnButton).GetComponent<EndTurnButtonController>().EnableButton();
    else
      _context.Get<GameObject>(ContextObjects.EndTurnButton).GetComponent<EndTurnButtonController>().DisableButton();

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
        cardNewPosition.z = _context.Get<GameObject>(ContextObjects.PlayerPlayedCard).transform.position.z;
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
