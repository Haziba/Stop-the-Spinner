using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipmentHandler : MonoBehaviour, IPointerClickHandler
{
  public event EventHandler OnItemClicked;
  public Item Item { get; private set; }
  
  public void SetItem(Item item, Sprite sprite)
  {
    Item = item;
    GetComponent<Image>().sprite = sprite;
  }
  
  public void OnPointerClick(PointerEventData eventData)
  {
    //ItemSlots.Where(slot => slot.Slot != _item.ItemSlot).ToList().ForEach(slot => slot.GameObject.GetComponent<Image>().color = Color.red);
    OnItemClicked?.Invoke(this, new EventArgs());
  }

  public void Select()
  {
    GetComponent<Outline>().enabled = true;
  }

  public void Unselect()
  {
    GetComponent<Outline>().enabled = false;
  }
}
