using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Linq;
using System;

namespace PlayerInfo
{
  public class BackpackCardHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
  {
    public PlayerBackpackCardDetails Card { get; private set; }
    bool _inDeck;
    bool _dragging;
    
    public void SetCard(IList<CardSpritePair> cardSprites, PlayerBackpackCardDetails card)
    {
        Card = card;

        transform.localScale = Vector3.one * 2;
        transform.localRotation = Quaternion.Euler(0, 0, UnityEngine.Random.Range(-2, 2));
        var imageComponent = gameObject.AddComponent<UnityEngine.UI.Image>();
        imageComponent.sprite = cardSprites.First(x => x.CardName == card.CardName).Sprite;
        gameObject.AddComponent<CanvasGroup>();
    }

    public void SetInDeck(bool inDeck)
    {
        _inDeck = inDeck;
        var canvasGroup = gameObject.GetComponent<CanvasGroup>();
        
        if (inDeck)
            Blur();
        else
            Unblur();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (_inDeck)
            return;

        var imageComponent = gameObject.GetComponent<UnityEngine.UI.Image>();
        DragBackpackCardHandler.Instance.StartDrag(Card, imageComponent.sprite);
        Blur();
        _dragging = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!_dragging)
            return;

        DragBackpackCardHandler.Instance.UpdateDrag(eventData.position);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!_dragging)
            return;

        var wasCardAdded = DragBackpackCardHandler.Instance.EndDrag(eventData);
        if (!wasCardAdded)
            Unblur();
        else
            _inDeck = true;
        _dragging = false;
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