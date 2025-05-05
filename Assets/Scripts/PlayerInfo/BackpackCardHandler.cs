using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Linq;
using System;

namespace PlayerInfo
{
  public class BackpackCardHandler : MonoBehaviour
  {
    public PlayerBackpackCardDetails Card { get; set; }
    bool _inDeck;
    bool _dragging;
    
    public void SetCard(IList<CardSpritePair> cardSprites, PlayerBackpackCardDetails card)
    {
        transform.localScale = Vector3.one * 2;
        transform.localRotation = Quaternion.Euler(0, 0, UnityEngine.Random.Range(-2, 2));
        var imageComponent = gameObject.AddComponent<UnityEngine.UI.Image>();
        imageComponent.sprite = cardSprites.First(x => x.CardName == card.CardName).Sprite;
        gameObject.AddComponent<CanvasGroup>();

        Card = card;
        GetComponent<DraggableCardHandler>().Card = card;
        GetComponent<DraggableCardHandler>().OnDrop += OnCardDropped;
        GetComponent<DraggableCardHandler>().OnStartDrag += OnStartDrag;
    }

    public void SetInDeck(bool inDeck)
    {
        _inDeck = inDeck;
        var canvasGroup = gameObject.GetComponent<CanvasGroup>();
        
        if (inDeck) {
            Blur();
            GetComponent<DraggableCardHandler>().enabled = false;
        }
        else {
            Unblur();
            GetComponent<DraggableCardHandler>().enabled = true;
        }
    }

    public void OnStartDrag(object sender, PointerEventData e)
    {
        Blur();
    }

    public void OnCardDropped(object sender, PointerEventData e)
    {
        Debug.Log("OnCardDropped");

        var wasCardAdded = DragBackpackCardHandler.Instance.EndDrag(e);
        if (!wasCardAdded)
            Unblur();
        else
            SetInDeck(true);
    }

    void Blur()
    {
        var canvasGroup = gameObject.GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0.6f;
    }

    void Unblur()
    {
        var canvasGroup = gameObject.GetComponent<CanvasGroup>();
        canvasGroup.alpha = 1f;
    }
  }
}