using TMPro;
using UnityEngine;

public class UIDeckCardHandler : MonoBehaviour
{
    private CardName cardName;
    private int amount;
    public GameObject Amount;
    public GameObject Card;
    public GameObject Name;

    public void SetCard(CardName cardName)
    {
        this.cardName = cardName;
        this.amount = 0;
        Amount.GetComponent<TextMeshProUGUI>().text = amount.ToString();
        Name.GetComponent<TextMeshProUGUI>().text = cardName.ToString();
        Card.GetComponent<CardHandler>().SetCardImage(cardName);
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

    public CardName GetName()
    {
        return cardName;
    }

    public int GetAmount()
    {
        return amount;
    }
}
