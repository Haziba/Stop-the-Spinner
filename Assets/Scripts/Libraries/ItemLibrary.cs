using System;
using System.Collections.Generic;

namespace Libraries
{
  public static class ItemLibrary
  {
    public static readonly IDictionary<ItemName, Item> Items = new Dictionary<ItemName, Item>
    {
      [ItemName.FancyHat] = new Item(ItemName.FancyHat, ItemSlot.Head)
      {
        Cards = new List<CardName>
        {
          CardName.FocusMe,
          CardName.FocusMe
        }
      },
      [ItemName.Sword] = new Item(ItemName.Sword, ItemSlot.RightArm | ItemSlot.LeftArm)
      {
        Cards = new List<CardName>
        {
          CardName.SwordThem,
          CardName.SwordThem,
          CardName.SwordThem,
          CardName.SwordThem
        }
      },
      [ItemName.Axe] = new Item(ItemName.Axe, ItemSlot.RightArm | ItemSlot.LeftArm)
      {
        Cards = new List<CardName>
        {
          CardName.AxeThem,
          CardName.AxeThem,
          CardName.AxeThem,
          CardName.AxeThem
        }
      },
      [ItemName.Dagger] = new Item(ItemName.Dagger, ItemSlot.RightArm | ItemSlot.LeftArm)
      {
        Cards = new List<CardName>()
      },
      [ItemName.Bow] = new Item(ItemName.Bow, ItemSlot.RightArm | ItemSlot.LeftArm)
      {
        Cards = new List<CardName>()
      },
      [ItemName.Wand] = new Item(ItemName.Wand, ItemSlot.RightArm | ItemSlot.LeftArm)
      {
        Cards = new List<CardName>()
      },
      [ItemName.BasicHat] = new Item(ItemName.BasicHat, ItemSlot.Head)
      {
        Cards = new List<CardName>()
      },
      [ItemName.PeasantShirt] = new Item(ItemName.PeasantShirt, ItemSlot.Chest)
      {
        Cards = new List<CardName>()
      },
      [ItemName.PeasantJorts] = new Item(ItemName.PeasantJorts, ItemSlot.Legs)
      {
        Cards = new List<CardName>()
      },
      [ItemName.PeasantBoots] = new Item(ItemName.PeasantBoots, ItemSlot.Feet)
      {
        Cards = new List<CardName>()
      },
      [ItemName.ChestArmour] = new Item(ItemName.ChestArmour, ItemSlot.Chest)
      {
        Cards = new List<CardName>()
      },
    };
  }
}

public enum ItemName
{
  FancyHat,
  Sword,
  Axe,
  Dagger,
  Bow,
  Wand,
  
  BasicHat,
  PeasantShirt,
  PeasantJorts,
  PeasantBoots,
  ChestArmour,
}

[Flags]
public enum ItemSlot
{
  Head = 1,
  Chest = 2,
  LeftArm = 4,
  RightArm = 8,
  Legs = 16,
  Feet = 32
}

[Serializable]
public class Item
{
  public ItemName Name { get; set; }
  public ItemSlot ItemSlot { get; set; }
  public List<CardName> Cards { get; set; }
  
  public Item(ItemName name, ItemSlot itemSlot)
  {
    Name = name;
    ItemSlot = itemSlot;
  }
}