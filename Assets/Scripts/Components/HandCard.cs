using System;
using UnityEngine;
using System.Collections;

// TODO:: Delete this whole class, all it really does is hold onto the cardName
public class HandCard
{
  CardName _cardName;

  GameObject _card;
  GameObject _hand;

  public event EventHandler OnCardClicked;

  public HandCard(CardName cardName, GameObject card, GameObject hand) {
    _cardName = cardName;
    _card = card;
    _hand = hand;

    // TODO:: Should this really live here? This whole class is starting to smell
    _card.GetComponent<CardHandler>().OnCardClicked += CardClicked;
    _card.transform.parent = _hand.transform;
    _card.GetComponent<CardHandler>().SetCardImage(_cardName);
  }

  void CardClicked(object sender, EventArgs e)
  {
    OnCardClicked?.Invoke(sender, new CardClickedEventArgs(_cardName, this) as EventArgs);
  }

  // TODO:: This feels a bit rusty, handing the image in to get it back out. Do people use tuples?
  public GameObject Image()
  {
    // TODO:: Also this should be renamed `_cardImage`, or `_image`
    return _card;
  }

  public CardName CardName()
  {
    return _cardName;
  }
}