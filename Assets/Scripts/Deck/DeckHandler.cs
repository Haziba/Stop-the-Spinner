using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Deck
{
  public class DeckHandler : MonoBehaviour
  {
    public GameObject DeckPane;

    public List<CardSpritePair> CardSprites;
    
    public void Start()
    {
      AddCardsToDeckPane(Player.Instance.Deck.ToList());
    }

    void AddCardsToDeckPane(IList<CardName> cards)
    {
      for (var i = 0; i < cards.Count(); i++)
      {
        var image = new GameObject("CardImage");
        image.transform.SetParent(DeckPane.transform);
        image.transform.localScale = Vector3.one * 2;
        var imageComponent = image.AddComponent<UnityEngine.UI.Image>();
        imageComponent.sprite = CardSprites.First(x => x.CardName == cards[i]).Sprite;
      }
    }

    public void GoBack()
    {
      SceneManager.LoadScene("WorldPathScene");
    }
  }

  [Serializable]
  public class CardSpritePair
  {
    public CardName CardName;
    public Sprite Sprite;
  }
}
