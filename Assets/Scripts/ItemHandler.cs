using System;
using System.Collections.Generic;
using System.Linq;
using PlayerInfo;
using UnityEngine;

public class ItemHandler : MonoBehaviour
{
  ItemName _itemName;

  public List<ItemGameObject> ItemSprites;
  public GameObject ItemBackground;
  
  public void SetItemImage(ItemName itemName)
  {
    _itemName = itemName;
    HideAllItemImages();
    ShowItemImage(itemName);
  }

  void HideAllItemImages()
  {
    ItemSprites.ForEach(sprite => sprite.GameObject.SetActive(false));
  }

  void ShowItemImage(ItemName itemName)
  {
    ItemSprites.ForEach(sprite => sprite.GameObject.SetActive(false));
    ItemSprites.First(sprite => sprite.Name == itemName).GameObject.SetActive(true);
  }
}