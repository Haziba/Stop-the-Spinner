using System;
using System.Collections.Generic;

namespace Libraries
{
  public static class ItemLibrary
  {
    public static IDictionary<ItemName, Item> Items = new Dictionary<ItemName, Item>
    {
      [ItemName.FancyHat] = new Item(ItemName.FancyHat, ItemSlot.Head)
      {
        Cards = new List<CardName>
        {
          CardName.FocusMe,
          CardName.FocusMe
        }
      },
      [ItemName.RustySword] = new Item(ItemName.RustySword, ItemSlot.RightArm)
      {
        Cards = new List<CardName>
        {
          CardName.SwordThem,
          CardName.SwordThem,
          CardName.SwordThem,
          CardName.SwordThem
        }
      }
    };
  }
}

public enum ItemName
{
  FancyHat,
  RustySword
}

public enum ItemSlot
{
  Head,
  Chest,
  LeftArm,
  RightArm,
  Legs,
  Feet
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