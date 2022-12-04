using System.Collections.Generic;
using System.Linq;
using Libraries;

public static class Player
{
  static bool _inited;

  static IList<CardName> _baseDeck;
  public static IList<CardName> Deck => _baseDeck.Concat(ItemCards()).ToList();
  public static IDictionary<ItemSlot, Item> Items { get; private set; }
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
    
    // TODO:: Load this from a file
    _baseDeck = new List<CardName>
    {
      CardName.IntoxicateThem,
      CardName.DistractThem,
      CardName.DistractThem
    };

    Items = new Dictionary<ItemSlot, Item>
    {
      [ItemSlot.Head] = ItemLibrary.Items[ItemName.FancyHat],
      [ItemSlot.LeftArm] = ItemLibrary.Items[ItemName.RustySword]
    };

    MaxCardsInHand = 5;
    MaxHealth = 10;
    Health = 10;
    MaxMana = 3;
    ManaRecoveryAmount = 2;
    Armour = 0;

    _inited = true;
  }

  static IEnumerable<CardName> ItemCards()
  {
    return Items.Values.SelectMany(item => item.Cards);
  }
}