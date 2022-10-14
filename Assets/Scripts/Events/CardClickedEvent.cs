using System;

public class CardClickedEventArgs : EventArgs 
{
  private readonly CardName _cardName;
  private readonly HandCard _handCard;

  public CardClickedEventArgs(CardName cardName, HandCard handCard)
  {
    _cardName = cardName;
    _handCard = handCard;
  }

  public CardName CardName()
  {
    return _cardName;
  }
  
  public HandCard HandCard()
  {
    return _handCard;
  }
}

