using TMPro;
using UnityEngine;

public class UIInventoryItemHandler : MonoBehaviour
{
    private ItemName itemName;
    private int amount;
    public GameObject Amount;
    public GameObject Item;
    public GameObject Name;

    public void SetItem(ItemName itemName)
    {
        this.itemName = itemName;
        this.amount = 0;
        Amount.GetComponent<TextMeshProUGUI>().text = amount.ToString();
        Name.GetComponent<TextMeshProUGUI>().text = itemName.ToString();
        Item.GetComponent<ItemHandler>().SetItemImage(itemName);
    }

    public void OnAdd()
    {
        amount++;
        Amount.GetComponent<TextMeshProUGUI>().text = amount.ToString();
    }

    public void OnRemove()
    {
        amount--;
        Amount.GetComponent<TextMeshProUGUI>().text = amount.ToString();
    }

    public ItemName GetName()
    {
        return itemName;
    }

    public int GetAmount()
    {
        return amount;
    }
}
