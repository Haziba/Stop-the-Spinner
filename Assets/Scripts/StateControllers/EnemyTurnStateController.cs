using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    _playedCardTarget = new Vector2(0, 1f);
    _playedCardOrigin = new Vector2(0, 8f);
    _spinnerTarget = new Vector2(0, 1f);
    _spinnerOrigin = new Vector2(0, 8f);
    _meState = _context.Get<IContextObject>(ContextObjects.EnemyState) as AgentState;
    _themState = _context.Get<IContextObject>(ContextObjects.PlayerState) as AgentState;
  }

  public override void Start()
  {
    _countdowns.Add(new HUtilities.Countdown(1f, () => {
      PlayCard();
      _innerState = InnerState.PlayedCardMovingToPosition;
    }));

    base.Start();
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
    var cardToPlay = handController.CardAt(UnityEngine.Random.Range(0, handController.TotalCards()-1));
    handController.RemoveCard(cardToPlay);

    _context.Get<GameObject>(ContextObjects.EnemyPlayedCard).GetComponent<PlayedCardController>().PlayCard(Agent.Enemy, cardToPlay.CardName(), cardToPlay.Image());
  }

}
