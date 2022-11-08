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

public class Item
{
  public ItemName Name { get; }
  public ItemSlot ItemSlot { get; }
  public IList<CardName> Cards { get; internal set; }
  
  public Item(ItemName name, ItemSlot itemSlot)
  {
    Name = name;
    ItemSlot = itemSlot;
  }
}