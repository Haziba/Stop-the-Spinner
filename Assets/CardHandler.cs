using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardHandler : MonoBehaviour
{
  public event EventHandler OnCardClicked;

  public PlayerCardDetails Card => _card;

  CardName _cardName;
  PlayerCardDetails _card;

  // Start is called before the first frame update
  void Start()
  {
    
  }

  // Update is called once per frame
  void Update()
  {
    
  }

  public void SetCardImage(CardName cardName, PlayerCardDetails card = null)
  {
    _cardName = cardName;
    _card = card;
    HideAllCardImages();
    ShowCardImage(cardName);
  }

  void OnMouseDown()
  {
    OnCardClicked?.Invoke(this, new EventArgs());
  }

  void HideAllCardImages()
  {
    for(var i = 0; i < transform.childCount; i++)
    {
      if (transform.GetChild(i).gameObject.name == "CardBackground")
        transform.GetChild(i).gameObject.SetActive(true);
      else
        transform.GetChild(i).gameObject.SetActive(false);
    }
  }

  void ShowCardImage(CardName cardName)
  {
    transform.Find(cardName.ToString()).gameObject.SetActive(true);;
  }

  void ShowCardBack()
  {
    transform.Find("CardBackground").gameObject.SetActive(false);
    transform.Find("Back").gameObject.SetActive(true);
  }

  public void RevealCard()
  {
    HideAllCardImages();
    ShowCardImage(_cardName);
  }

  public void SetAsEnemyCard()
  {
    transform.Rotate(new Vector3(0, 0, 180f));
    HideAllCardImages();
    ShowCardBack();
  }

  public void HideCard()
  {
    HideAllCardImages();
    ShowCardBack();
  }
}
