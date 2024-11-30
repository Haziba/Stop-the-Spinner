using System;
using System.Collections.Generic;
using System.Linq;
using Libraries;
using Newtonsoft.Json;
using UnityEngine;

public class Player
{
  static Player _instance;

  static IList<CardName> _baseDeck;
  public IList<CardName> Deck => _baseDeck.Concat(ItemCards()).ToList();
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
    _baseDeck = data.baseDeck.ToList();
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
    var path = System.IO.Path.Combine(Application.persistentDataPath, "playerData.json");
    if (System.IO.File.Exists(path))
    {
      var json = System.IO.File.ReadAllText(path);
      var data = JsonConvert.DeserializeObject<SaveData>(json);

      return new Player(data);
    }

    var newData = new SaveData
    {
      baseDeck = new List<CardName>
      {
        CardName.IntoxicateThem,
        CardName.DistractThem,
        CardName.DistractThem
      },
      items = new Dictionary<ItemSlot, Item>
      {
        [ItemSlot.Head] = ItemLibrary.Items[ItemName.FancyHat],
        [ItemSlot.LeftArm] = ItemLibrary.Items[ItemName.RustySword],
        [ItemSlot.RightArm] = ItemLibrary.Items[ItemName.RustySword]
      },
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

    return new Player(newData);
  }

  IEnumerable<CardName> ItemCards()
  {
    return Items.Values.SelectMany(item => item.Cards);
  }

  public void GainCard(CardName cardName)
  {
    _baseDeck.Add(cardName);
    Save();
  }

  public void EquipItem(ItemSlot slot, Item item)
  {
    Debug.Log(slot);
    if(Items.ContainsKey(slot))
      AddInventoryItem(Items[slot]);
    RemoveInventoryItem(item);
    Items[slot] = item;
    Save();
  }

  public void AddInventoryItem(Item item)
  {
    Inventory.Add(item);
  }

  public void RemoveInventoryItem(Item item)
  {
    Inventory.Remove(item);
  }

  void Save()
  {
    var json = JsonConvert.SerializeObject(new SaveData
    {
      baseDeck = _baseDeck.ToList(),
      items = Items,
      inventory = Inventory,
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
    public List<CardName> baseDeck;
    public IDictionary<ItemSlot, Item> items;
    public List<Item> inventory;
    public int maxCardsInHand;
    public int maxHealth;
    public int health;
    public int maxMana;
    public int manaRecoveryAmount;
  }
}
