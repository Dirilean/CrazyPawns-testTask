using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Runtime
{
    [RequireComponent(typeof(Collider))]
    public class PawnBody : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        public Action<PointerEventData> onBeginDrag;
        public Action<PointerEventData> onEndDrag;
        public Action<PointerEventData> onDrag;
        

        public void OnDrag(PointerEventData eventData)
        {
            onDrag?.Invoke(eventData);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            onBeginDrag?.Invoke(eventData);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            onDrag?.Invoke(eventData);
        }
    }
}