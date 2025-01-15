using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipmentHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
  public event EventHandler OnItemClicked;
  public event EventHandler OnItemLongPress;
  public Item Item { get; private set; }
  public GameObject IconImage;
  public GameObject Foreground;
  float? _clickStart;

  public void Update()
  {
    if (Time.time - _clickStart > 0.5f)
    {
      OnItemLongPress?.Invoke(this, new EventArgs());
      _clickStart = null;
    }
  }
  
  public void SetItem(Item item, Sprite sprite)
  {
    Item = item;
    IconImage.GetComponent<Image>().sprite = sprite;
  }
  
  public void OnPointerDown(PointerEventData eventData)
  {
    _clickStart = Time.time;
  }

  public void OnPointerUp(PointerEventData eventData)
  {
    if (_clickStart == null)
      return;
    OnItemClicked?.Invoke(this, new EventArgs());
    _clickStart = null;
  }

  public void Select()
  {
    Foreground.GetComponent<Outline>().enabled = true;
  }

  public void Unselect()
  {
    Foreground.GetComponent<Outline>().enabled = false;
  }
}
