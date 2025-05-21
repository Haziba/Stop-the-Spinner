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
        }
      },
      [ItemName.RustySword] = new Item(ItemName.RustySword, ItemSlot.RightArm | ItemSlot.LeftArm)
      {
        Cards = new List<CardName>
        {
          CardName.SwordThem,
          CardName.SwordThem,
          CardName.SwordThem,
        }
      },
      [ItemName.RustyAxe] = new Item(ItemName.RustyAxe, ItemSlot.RightArm | ItemSlot.LeftArm)
      {
        Cards = new List<CardName>
        {
          CardName.AxeThem,
          CardName.AxeThem,
          CardName.AxeThem,
        }
      },
      [ItemName.OldLute] = new(ItemName.OldLute, ItemSlot.RightArm)
      {
        Cards = new List<CardName>
        {
          CardName.OldLute,
          CardName.OldLute,
          CardName.OldLute,
        }
      }
    };
  }
}

public enum ItemName
{
  FancyHat,
  RustySword,
  RustyAxe,
  OldLute,
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