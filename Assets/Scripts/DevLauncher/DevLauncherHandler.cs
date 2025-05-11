using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using System.Linq;
using System.IO;
using Libraries;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEditor;
public class DevLauncherHandler : MonoBehaviour
{
  static bool _initialized = false;
  
  public GameObject EnemyList;
  public GameObject DeckList;
  public GameObject InventoryList;
  public GameObject EquipmentList;
  public GameObject EventList;

  public GameObject DeckManagement;
  public GameObject DeckCardPrefab;
  public GameObject SelectedCards;
  public IList<UIDeckCardHandler> DeckCardHandlers = new List<UIDeckCardHandler>();

  public GameObject InventoryManagement;
  public GameObject InventoryItemPrefab;
  public GameObject SelectedInventory;
  public IList<UIInventoryItemHandler> InventoryItemHandlers = new List<UIInventoryItemHandler>();

  public GameObject EquipmentManagement;
  public GameObject EquipmentItemPrefab;
  public GameObject SelectedEquipment;
  public IList<UIEquipmentItemHandler> EquipmentItemHandlers = new List<UIEquipmentItemHandler>();

  public void Start()
  {
    Debug.Log("Start");
    EnemyList.GetComponent<TMP_Dropdown>().options =
      (Enum.GetNames(typeof(MonsterName))).Select(enemyName => new TMP_Dropdown.OptionData(enemyName)).ToList();

    EventList.GetComponent<TMP_Dropdown>().options =
      (Enum.GetNames(typeof(EventName))).Select(eventName => new TMP_Dropdown.OptionData(eventName)).ToList();
    
    foreach (CardName cardName in Enum.GetValues(typeof(CardName)))
    {
      var cardObj = Instantiate(DeckCardPrefab, DeckList.transform);
      var handler = cardObj.GetComponent<UIDeckCardHandler>();
      handler.SetCard(cardName);
      DeckCardHandlers.Add(handler);
    }
    
    foreach (ItemName itemName in Enum.GetValues(typeof(ItemName)))
    {
      var itemObj = Instantiate(InventoryItemPrefab, InventoryList.transform);
      var handler = itemObj.GetComponent<UIInventoryItemHandler>();
      handler.SetItem(itemName);
      InventoryItemHandlers.Add(handler);
    }
    
    foreach (ItemSlot itemSlot in Enum.GetValues(typeof(ItemSlot)))
    {
      var itemObj = Instantiate(EquipmentItemPrefab, EquipmentList.transform);
      var handler = itemObj.GetComponent<UIEquipmentItemHandler>();
      handler.SetSlot(itemSlot);
      EquipmentItemHandlers.Add(handler);
    }
    
    if (_initialized)
      return;
    
    var loadedState = LoadDevLauncherState();
    if (loadedState != null)
    {
      try
      {
        // This didn't work, how do I set a dropdown???
        //SetDropdownToValue(EnemyList, loadedState.Monster.ToString());
        //SetDropdownToValue(EventList, loadedState.Event.ToString());
        
        DoAction(loadedState);
      }
      catch (Exception e)
      {
        Debug.LogException(e);
        DeleteDevLauncherState();
      }
    }
    
    _initialized = true;
  }

  public void OnClearLauncherDataClick()
  {
    DeleteDevLauncherState();
  }

  public void OnFightClick() {
    Go(ActionType.Fight);
  }

  MonsterName GetMonsterName()
  {
    var dropdown = EnemyList.GetComponent<TMP_Dropdown>();
    var selectedIndex = dropdown.value;
    var selectedName = dropdown.options[selectedIndex].text;
    return Enum.Parse<MonsterName>(selectedName);
  }

  public void OnDeckClick() {
    DeckManagement.SetActive(true);
  }

  public void OnExitDeckClick()
  {
    SelectedCards.GetComponent<TextMeshProUGUI>().text = string.Join(", ", GetSelectedCards().Select(cardName => cardName.ToString()));
    DeckManagement.SetActive(false);
  }

  public IEnumerable<CardName> GetSelectedCards()
  {
    var selectedCards = new List<CardName>();
    foreach (var deckCard in DeckCardHandlers)
    {
      for (var i = 0; i < deckCard.GetAmount(); i++)
        selectedCards.Add(deckCard.GetName());
    }
    return selectedCards;
  }

  public void OnInventoryClick()
  {
    InventoryManagement.SetActive(true);
  }

  public void OnExitInventoryClick()
  {
    SelectedInventory.GetComponent<TextMeshProUGUI>().text = string.Join(", ", GetSelectedInventory().Select(itemName => itemName.ToString()));
    InventoryManagement.SetActive(false);
  }

  public IEnumerable<ItemName> GetSelectedInventory()
  {
    var selectedItems = new List<ItemName>();
    foreach (var item in InventoryItemHandlers)
    {
      for(var i = 0; i < item.GetAmount(); i++)
        selectedItems.Add(item.GetName());
    }

    return selectedItems;
  }

  public void OnEquipmentClick()
  {
    EquipmentManagement.SetActive(true);
  }

  public void OnExitEquipmentClick()
  {
    SelectedEquipment.GetComponent<TextMeshProUGUI>().text = string.Join(", ", GetSelectedEquipment().Select(equipment => $"{equipment.Slot} - {equipment.Name}"));
    EquipmentManagement.SetActive(false);
  }

  public IEnumerable<Equipment> GetSelectedEquipment()
  {
    var selectedEquipment = new List<Equipment>();
    foreach (var item in EquipmentItemHandlers)
    {
      if (item.GetItem() != null)
      {
        selectedEquipment.Add(new Equipment { Slot = item.GetSlot(), Name = item.GetItem().Value });
      }
    }

    return selectedEquipment;
  }

  public void OnEventClick() {
    Go(ActionType.Event);
  }

  EventName GetEventName()
  {
    var dropdown = EventList.GetComponent<TMP_Dropdown>();
    var selectedIndex = dropdown.value;
    var selectedName = dropdown.options[selectedIndex].text;
    return Enum.Parse<EventName>(selectedName);
  }

  public void OnMapClick()
  {
    Go(ActionType.Map);
  }

  public void OnPlayerInfoClick() {
    Go(ActionType.PlayerInfo);
  }

  public void OnWorldPathClick() {
    Go(ActionType.WorldPath);
  }

  public void OnNewGameClick()
  {
    Go(ActionType.NewGame);
  }

  public void OnLoadGameClick() {
    Go(ActionType.LoadGame);
  }

  void Go(ActionType actionType)
  {
    SaveDevLauncherState(actionType);

    var deck = GetSelectedCards().ToArray();
    var inventory = GetSelectedInventory().ToArray();
    var equipment = GetSelectedEquipment().ToArray();
    
    DoAction(new SaveData
    {
      ActionType = actionType,
      Deck = deck.Any() ? deck : null,
      Inventory = inventory.Any() ? inventory : null,
      Equipment = equipment.Any() ? equipment : null,
    });
  }

  void DoAction(SaveData saveData)
  {
    Debug.Log(JsonUtility.ToJson(saveData));
    if (saveData.Deck != null) Debug.Log("Set deck - " + string.Join(", ", saveData.Deck.Select(cardName => cardName.ToString())));
    if(saveData.Inventory != null) Debug.Log("Set inventory - " + string.Join(", ", saveData.Inventory.Select(itemName => itemName.ToString())));
    if(saveData.Equipment != null) Debug.Log("Set equipment - " + string.Join(", ", saveData.Equipment.Select(equip => $"{equip.Slot.ToString()} - {equip.Name.ToString()}")));
    
    Player.ReplaceInstance(saveData.Deck, saveData.Inventory, saveData.Equipment?.Select(x => new Tuple<ItemSlot, ItemName>(x.Slot, x.Name)).ToArray());
    
    switch(saveData.ActionType) {
      case ActionType.Fight:
        SceneDataHandler.UpdateData(new Dictionary<SceneDataKey, object> { [SceneDataKey.Enemy] = new EnemyConfig(saveData.Monster, BackgroundName.DeadEnd) });
        SceneManager.LoadScene("BattleScene");
        break;
      case ActionType.Event:
        SceneDataHandler.UpdateData(new Dictionary<SceneDataKey, object> { [SceneDataKey.Event] = new EventConfig(saveData.Event, BackgroundName.WitchHut) });
        SceneManager.LoadScene("EventScene");
        break;
      case ActionType.Map:
        SceneManager.LoadScene("MapScene");
        break;
      case ActionType.PlayerInfo:
        SceneManager.LoadScene("PlayerInfoScene");
        break;
      case ActionType.WorldPath:
        SceneManager.LoadScene("WorldPathScene");
        break;
      case ActionType.NewGame:
        if (EditorUtility.DisplayDialog("Confirm", "Are you sure you want to clear data and start a new game?", "Yes", "No"))
        {
            Player.NewGame();
            SceneManager.LoadScene("OpenScreen");
        }
        break;
      case ActionType.LoadGame:
        SceneManager.LoadScene("OpenScreen");
        break;
    }
  }

  SaveData LoadDevLauncherState() {
    var path = Path.Combine(Application.persistentDataPath, "DevLauncherState.json");
    try {
      if (File.Exists(path)) {
        return JsonUtility.FromJson<SaveData>(File.ReadAllText(path));
      }
      return null;
    } catch (Exception e) {
      Debug.LogError($"Failed to load DevLauncher state: {e.Message}");
      File.Delete(path);
      return null;
    }
  }

  void DeleteDevLauncherState() {
    var path = Path.Combine(Application.persistentDataPath, "DevLauncherState.json");
    try {
      if (File.Exists(path))
      {
        File.Delete(path);
      }
    } catch (Exception e) {
      Debug.LogError($"Failed to delete DevLauncher state: {e.Message}");
    }
  }
  
  void SaveDevLauncherState(ActionType actionType)
  {
    string json = JsonUtility.ToJson(new SaveData
    {
      ActionType = actionType,
      Monster = GetMonsterName(),
      Event = GetEventName(),
      Deck = GetSelectedCards().ToArray(),
      Inventory = GetSelectedInventory().ToArray(),
      Equipment = GetSelectedEquipment().ToArray()
    });
    Debug.Log($"Save - {json}");
    string path = Path.Combine(Application.persistentDataPath, "DevLauncherState.json");

    try
    {
      File.WriteAllText(path, json);
      Debug.Log($"DevLauncher state saved to {path}");
    }
    catch (Exception e)
    {
      Debug.LogError($"Failed to save DevLauncher state: {e.Message}");
    }
  }

  public class SaveData
  {
    public ActionType ActionType;
    public MonsterName Monster;
    public EventName Event;
    public CardName[] Deck;
    public ItemName[] Inventory;
    public Equipment[] Equipment;
  }

  public enum ActionType
  {
    Fight = 0,
    Event = 1,
    Map = 2,
    PlayerInfo = 3,
    WorldPath = 4,
    NewGame = 5,
    LoadGame = 6
  }

  [Serializable]
  public class Equipment
  {
    public ItemSlot Slot;
    public ItemName Name;
  }
}