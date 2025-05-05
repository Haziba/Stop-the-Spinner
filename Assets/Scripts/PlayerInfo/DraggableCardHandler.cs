using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace PlayerInfo
{
    public class DraggableCardHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public PlayerCardDetails Card { get; set; }
        bool _dragging;
        public event EventHandler<PointerEventData> OnStartDrag;
        public event EventHandler<PointerEventData> OnDrop;
        
        public void OnBeginDrag(PointerEventData eventData)
        {
            var imageComponent = gameObject.GetComponent<UnityEngine.UI.Image>();
            DragBackpackCardHandler.Instance.StartDrag(Card, imageComponent.sprite);
            _dragging = true;

            OnStartDrag?.Invoke(this, eventData);
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

            OnDrop?.Invoke(this, eventData);

            _dragging = false;
        }
    }
}