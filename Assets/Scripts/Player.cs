using System;
using System.Collections.Generic;
using System.Linq;
using Libraries;
using UnityEngine;

public class Player
{
  static Player _instance;

  IList<PlayerBackpackCardDetails> _backpack;
  IList<PlayerCardDetails> _deck;
  
  public IList<PlayerBackpackCardDetails> Backpack => _backpack;
  public IList<PlayerCardDetails> Deck => _deck;
  public IDictionary<ItemSlot, Item> Items { get; }
  public List<Item> Inventory { get;  }
  public int MaxCardsInHand { get; }
  public int MaxHealth { get; }
  public int Health { get; }
  public int MaxMana { get; }
  public int ManaRecoveryAmount { get; }
  public int Armour { get; }

  public static Player Instance => _instance ??= Init();

  Player(SaveData data)
  {
    _backpack = data.backpack.ToList();
    _deck = data.deck.ToList();
    Items = data.items;
    Inventory = data.inventory;
    MaxCardsInHand = data.maxCardsInHand;
    MaxHealth = data.maxHealth;
    Health = data.health;
    MaxMana = data.maxMana;
    ManaRecoveryAmount = data.manaRecoveryAmount;
    Armour = 0;
  }

  static Player Init()
  {
    /*var path = System.IO.Path.Combine(Application.persistentDataPath, "playerData.json");
    if (System.IO.File.Exists(path))
    {
      var json = System.IO.File.ReadAllText(path);
      var data = JsonUtility.FromJson<SaveData>(json);

      return new Player(data);
    }*/

    var newData = new SaveData
    {
      backpack = new List<PlayerBackpackCardDetails>
      {
        new PlayerBackpackCardDetails { Id = Guid.NewGuid(), CardName = CardName.IntoxicateThem },
        new PlayerBackpackCardDetails { Id = Guid.NewGuid(), CardName = CardName.DistractThem },
        new PlayerBackpackCardDetails { Id = Guid.NewGuid(), CardName = CardName.DistractThem }
      },
      items = new Dictionary<ItemSlot, Item>(),
      inventory = new List<Item>
      {
        ItemLibrary.Items[ItemName.RustyAxe],
        ItemLibrary.Items[ItemName.RustyAxe],
        ItemLibrary.Items[ItemName.FancyHat],
      },
      maxCardsInHand = 5,
      maxHealth = 10,
      health = 10,
      maxMana = 3,
      manaRecoveryAmount = 2,
    };

    newData.deck = newData.backpack.ToList<PlayerCardDetails>();

    var player = new Player(newData);
    
    player.EquipItem(ItemSlot.Head, ItemLibrary.Items[ItemName.FancyHat]);
    player.EquipItem(ItemSlot.LeftArm, ItemLibrary.Items[ItemName.RustySword]);
    player.EquipItem(ItemSlot.RightArm, ItemLibrary.Items[ItemName.RustySword]);
    
    return player;
  }

  IEnumerable<CardName> ItemCards()
  {
    return Items?.Values.SelectMany(item => item.Cards) ?? new List<CardName>();
  }

  public void GainCard(CardName cardName)
  {
    _backpack.Add(new PlayerBackpackCardDetails { Id = Guid.NewGuid(), CardName = cardName });
    Save();
  }

  public void EquipItem(ItemSlot slot, Item item)
  {
    // de-quip old item
    if(Items.ContainsKey(slot))
      AddInventoryItem(Items[slot]);
    RemoveInventoryItem(item);
    // equip new item
    Items[slot] = item;
    item.Cards.ForEach(card => _deck.Add(new PlayerItemCardDetails { Id = Guid.NewGuid(), CardName = card, ItemSlot = slot }));
    Save();
  }

  public void AddInventoryItem(Item item)
  {
    Inventory.Add(item);
  }

  public void RemoveInventoryItem(Item item)
  {
    Inventory.Remove(item);
    _deck.ToList().RemoveAll(card => card is PlayerItemCardDetails itemCard && itemCard.ItemSlot == item.ItemSlot);
  }

  void Save()
  {
    var json = JsonUtility.ToJson(new SaveData
    {
      backpack = _backpack.ToList(),
      items = Items,
      inventory = Inventory,
      deck = _deck.ToList(),
      maxCardsInHand = MaxCardsInHand,
      maxHealth = MaxHealth,
      health = Health,
      maxMana = MaxMana,
      manaRecoveryAmount = ManaRecoveryAmount
    });
    var path = System.IO.Path.Combine(Application.persistentDataPath, "playerData.json");
    System.IO.File.WriteAllText(path, json);
  }

  [Serializable]
  public class SaveData
  {
    public List<PlayerBackpackCardDetails> backpack;
    public List<PlayerCardDetails> deck;
    public IDictionary<ItemSlot, Item> items;
    public List<Item> inventory;
    public int maxCardsInHand;
    public int maxHealth;
    public int health;
    public int maxMana;
    public int manaRecoveryAmount;
  }
}

[Serializable]
public class PlayerCardDetails
{
  public Guid Id;
  public CardName CardName;
}

[Serializable]
public class PlayerBackpackCardDetails : PlayerCardDetails {}

[Serializable]
public class PlayerItemCardDetails : PlayerCardDetails
{
  public ItemSlot ItemSlot;
}
