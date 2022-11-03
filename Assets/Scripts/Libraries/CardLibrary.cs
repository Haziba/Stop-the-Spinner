using System.Collections.Generic;

namespace Libraries
{
  public static class CardLibrary
  {
    public static IDictionary<CardName, Card> Cards = new Dictionary<CardName, Card>
    {
      [CardName.SwordThem] = new Card(2),
      [CardName.AxeThem] = new Card(3),
      [CardName.DaggerThem] = new Card(1), 
      [CardName.BiteThem] = new Card(3),
      [CardName.DistractThem] = new Card(1), 
      [CardName.FocusMe] = new Card(1),
      [CardName.IntoxicateThem] = new Card(2), 
      [CardName.ShieldBashThem] = new Card(3),
      [CardName.RaiseShield] = new Card(1)
    };
  }
}

public class Card
{
  int _manaCost;
  
  public int ManaCost => _manaCost;
  
  public Card(int manaCost)
  {
    _manaCost = manaCost;
  }
}