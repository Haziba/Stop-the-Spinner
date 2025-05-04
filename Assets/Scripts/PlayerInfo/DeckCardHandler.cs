using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace PlayerInfo
{
  public class DeckCardHandler : MonoBehaviour
  {
    public PlayerCardDetails Card { get; private set; }

    public void SetCard(IList<CardSpritePair> cardSprites, PlayerCardDetails card)
    {
      Card = card;

      transform.localScale = Vector3.one * 2;
      transform.localRotation = Quaternion.Euler(0, 0, UnityEngine.Random.Range(-8, 8));
      var imageComponent = gameObject.AddComponent<UnityEngine.UI.Image>();
      imageComponent.sprite = cardSprites.First(x => x.CardName == card.CardName).Sprite;
    }
  }
}