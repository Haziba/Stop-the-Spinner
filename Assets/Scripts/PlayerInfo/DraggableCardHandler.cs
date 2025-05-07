using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace PlayerInfo
{
    public class DraggableCardHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public PlayerCardDetails Details { get; set; }
        bool _dragging;
        public event EventHandler<PointerEventData> OnStartDrag;
        public event EventHandler<PointerEventData> OnDrop;
        
        public void OnBeginDrag(PointerEventData eventData)
        {
            DragBackpackCardHandler.Instance.StartDrag(Details);
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