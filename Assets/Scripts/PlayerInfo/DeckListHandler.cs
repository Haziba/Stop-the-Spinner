using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace PlayerInfo
{
  public class DeckListHandler : MonoBehaviour
  {
      List<GameObject> _cards;
      public GameObject Content;
      public GameObject CardPrefab;

      public static DeckListHandler Instance;

      public void Start()
      {
        Instance = this;

        _cards = new List<GameObject>();

        foreach(var card in Player.Instance.Deck)
          AddCard(card);
      }

      public void AddCard(PlayerCardDetails card)
      {
        var image = Instantiate(CardPrefab, Content.transform);
        image.GetComponent<CardHandler>().SetCardImage(card.CardName, card);
        image.transform.rotation = Quaternion.Euler(0, 0, Random.Range(-6, 6));
        _cards.Add(image);
      }

      public void RemoveCard(PlayerCardDetails card)
      {
        var image = _cards.Find(c => c.GetComponent<CardHandler>().Card == card);
        _cards.Remove(image);
        Destroy(image);
      }

      public void UnequipSlot(ItemSlot itemSlot)
      {
        var images = _cards.Where(c => (c.GetComponent<CardHandler>().Card as PlayerItemCardDetails)?.ItemSlot == itemSlot).ToList();
        foreach(var image in images)
        {
          _cards.Remove(image);
          Destroy(image);
        }
      }
  }
}
