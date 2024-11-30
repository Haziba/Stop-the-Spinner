using UnityEngine;

public class ItemHandler : MonoBehaviour
{
  ItemName _itemName;
  
  public void SetItemImage(ItemName itemName)
  {
    _itemName = itemName;
    HideAllItemImages();
    ShowItemImage(itemName);
  }

  void HideAllItemImages()
  {
    for(var i = 0; i < transform.childCount; i++)
      transform.GetChild(i).gameObject.SetActive(false);
  }

  void ShowItemImage(ItemName itemName)
  {
    transform.Find(itemName.ToString()).gameObject.SetActive(true);;
  }
}