using System;
using System.Linq;
using Libraries;
using TMPro;
using UnityEngine;

public class UIEquipmentItemHandler : MonoBehaviour
{
    private ItemSlot itemSlot;
    public GameObject Slot;
    public GameObject Items;

    private string NONE_SELECTED = " - ";

    public void SetSlot(ItemSlot itemSlot)
    {
        this.itemSlot = itemSlot;
        Slot.GetComponent<TextMeshProUGUI>().text = itemSlot.ToString();
        Items.GetComponent<TMP_Dropdown>().options = new [] { new TMP_Dropdown.OptionData(NONE_SELECTED) }.ToList().Concat(
            Enum.GetNames(typeof(ItemName))
                .Select(x => (ItemName)Enum.Parse(typeof(ItemName), x))
                .Where(x => (ItemLibrary.Items[x].ItemSlot & itemSlot) > 0)
                .Select(x => new TMP_Dropdown.OptionData(x.ToString())).ToList()).ToList();
    }

    public ItemSlot GetSlot()
    {
        return itemSlot;
    }

    public ItemName? GetItem()
    {
        var dropdown = Items.GetComponent<TMP_Dropdown>();
        var selectedIndex = dropdown.value;
        var selectedName = dropdown.options[selectedIndex].text;
        if(selectedName == NONE_SELECTED)
            return null;
        return Enum.Parse<ItemName>(selectedName);
    }
}
