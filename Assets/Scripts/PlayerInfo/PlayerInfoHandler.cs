using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace PlayerInfo
{
  public class PlayerInfoHandler : MonoBehaviour
  {
    public GameObject InventoryContent;
    public GameObject InventoryItemPrefab;
    public List<InventoryItemSlot> ItemSlots;
    public List<ItemSprite> ItemSprites;

    List<GameObject> _inventoryItems = new List<GameObject>();

    GameObject _selectedItem;

    public void Start()
    {
      ItemSlots.ForEach(InitSlot);
      SetItemInventory();
    }

    void InitSlot(InventoryItemSlot slot)
    {
      UpdateSlotImage(slot);
      slot.GameObject.GetComponent<Button>().onClick.AddListener(() => OnItemSlotClick(slot));
    }

    void UpdateSlotImage(InventoryItemSlot slot)
    {
      var item = Player.Instance.Items.FirstOrDefault(pair => pair.Key == slot.Slot).Value;
      slot.GameObject.GetComponent<Image>().sprite = item != null ? ItemSprites.First(x => x.Name == item.Name).Sprite : null;
    }

    void SetItemInventory()
    {
      foreach(var item in Player.Instance.Inventory)
        AddInventoryItem(item);
      foreach(var item in Player.Instance.Inventory)
        AddInventoryItem(item);
      foreach(var item in Player.Instance.Inventory)
        AddInventoryItem(item);
      foreach(var item in Player.Instance.Inventory)
        AddInventoryItem(item);
      foreach(var item in Player.Instance.Inventory)
        AddInventoryItem(item);
      foreach(var item in Player.Instance.Inventory)
        AddInventoryItem(item);
      foreach(var item in Player.Instance.Inventory)
        AddInventoryItem(item);
      foreach(var item in Player.Instance.Inventory)
        AddInventoryItem(item);
    }

    void AddInventoryItem(Item item)
    {
      var image = Instantiate(InventoryItemPrefab);
      image.transform.SetParent(InventoryContent.transform);
      image.transform.localScale = Vector3.one;
      image.GetComponent<EquipmentHandler>().SetItem(item, ItemSprites.First(x => x.Name == item.Name).Sprite);
      image.GetComponent<EquipmentHandler>().OnItemClicked += ClickItem;

      _inventoryItems.Add(image);
    }

    void UnselectInventoryItems()
    {
      _inventoryItems.ForEach(item => item.GetComponent<EquipmentHandler>().Unselect());
      _selectedItem = null;
      ItemSlots.ForEach((slot) => slot.GameObject.GetComponent<Image>().color = Color.white);
    }

    void ClickItem(object sender, EventArgs e)
    {
      UnselectInventoryItems();
      (sender as EquipmentHandler).Select();
      ItemSlots.ForEach((slot) => slot.GameObject.GetComponent<Image>().color = (sender as EquipmentHandler).Item.ItemSlot.HasFlag(slot.Slot) ? Color.white : Color.red);
      _selectedItem = (sender as EquipmentHandler).gameObject;
    }

    void OnItemSlotClick(InventoryItemSlot slot)
    {
      if (_selectedItem == null)
        return;

      var item = _selectedItem.GetComponent<EquipmentHandler>().Item;

      if (!item.ItemSlot.HasFlag(slot.Slot))
        return;

      if(Player.Instance.Items.ContainsKey(slot.Slot))
        AddInventoryItem(Player.Instance.Items[slot.Slot]);
      Player.Instance.EquipItem(slot.Slot, item);
      Destroy(_selectedItem);
      _inventoryItems.Remove(_selectedItem);
      UnselectInventoryItems();
      UpdateSlotImage(slot);
    }

    public void ClickBack()
    {
      SceneManager.LoadScene("WorldPathScene");
    }
  }
}
