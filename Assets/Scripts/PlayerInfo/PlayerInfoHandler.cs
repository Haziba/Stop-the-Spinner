using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

namespace PlayerInfo
{
  public class PlayerInfoHandler : MonoBehaviour
  {
    public GameObject Page;

    public GameObject StatsButton;
    public GameObject InventoryButton;
    public GameObject DeckButton;

    public GameObject DeckContent;
    public GameObject BackpackContent;
    public List<CardSpritePair> CardSprites;

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
      SetDeck();
      SetBackpack();
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

    /// Deck handler
    public void SetDeck()
    {
      AddCardsToDeckContent(Player.Instance.Deck.ToList());
    }

    void AddCardsToDeckContent(IList<CardName> cards)
    {
      for (var i = 0; i < cards.Count(); i++)
      {
        var image = new GameObject("CardImage");
        image.transform.SetParent(DeckContent.transform);
        image.transform.localScale = Vector3.one * 2;
        image.transform.localRotation = Quaternion.Euler(0, 0, UnityEngine.Random.Range(-8, 8));
        var imageComponent = image.AddComponent<UnityEngine.UI.Image>();
        imageComponent.sprite = CardSprites.First(x => x.CardName == cards[i]).Sprite;
      }
    }

    public void SetBackpack()
    {
      AddCardsToBackpackContent(Player.Instance.Deck.ToList());
    }

    void AddCardsToBackpackContent(IList<CardName> cards)
    {
      for (var i = 0; i < cards.Count(); i++)
      {
        var image = new GameObject("CardImage");
        image.transform.SetParent(BackpackContent.transform);
        image.transform.localScale = Vector3.one * 2;
        image.transform.localRotation = Quaternion.Euler(0, 0, UnityEngine.Random.Range(-4, 4));
        var imageComponent = image.AddComponent<UnityEngine.UI.Image>();
        imageComponent.sprite = CardSprites.First(x => x.CardName == cards[i]).Sprite;
      }
    }
    
    public void ClickBack()
    {
      SceneManager.LoadScene("WorldPathScene");
    }

    public void ClickStats()
    {
      Page.transform.DOMove(new Vector3(1.8f, 0, 0), 0.5f);
      StatsButton.GetComponent<Button>().interactable = false;
      InventoryButton.GetComponent<Button>().interactable = true;
      DeckButton.GetComponent<Button>().interactable = true;
    }

    public void ClickInventory()
    {
      Page.transform.DOMove(new Vector3(0, 0, 0), 0.5f);
      StatsButton.GetComponent<Button>().interactable = true;
      InventoryButton.GetComponent<Button>().interactable = false;
      DeckButton.GetComponent<Button>().interactable = true;
    }

    public void ClickDeck()
    {
      Page.transform.DOMove(new Vector3(-3.8f, 0, 0), 0.5f);
      StatsButton.GetComponent<Button>().interactable = true;
      InventoryButton.GetComponent<Button>().interactable = true;
      DeckButton.GetComponent<Button>().interactable = false;
    }
  }

  [Serializable]
  public class CardSpritePair
  {
    public CardName CardName;
    public Sprite Sprite;
  }
}
