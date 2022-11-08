using System.Collections.Generic;

public static class Player
{
  static bool _inited;
  
  public static IList<CardName> Deck { get; private set; }
  public static int MaxCardsInHand { get; private set; }
  public static int MaxHealth { get; private set; }
  public static int Health { get; private set; }
  public static int MaxMana { get; private set; }
  public static int ManaRecoveryAmount { get; private set; }
  public static int Armour { get; private set; }


  public static void Init()
  {
    if (_inited)
      return;
    
    // todo: Load this from a file
    Deck = new List<CardName>
    {
      CardName.SwordThem,
      CardName.SwordThem,
      CardName.AxeThem,
      CardName.AxeThem,
      CardName.IntoxicateThem,
      CardName.FocusMe,
      CardName.DistractThem,
      CardName.SwordThem,
      CardName.AxeThem,
      CardName.FocusMe,
      CardName.DistractThem
    };

    MaxCardsInHand = 5;
    MaxHealth = 10;
    Health = 10;
    MaxMana = 3;
    ManaRecoveryAmount = 2;
    Armour = 0;

    _inited = true;
  }
}