using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Runtime
{
    [RequireComponent(typeof(Collider))]
    public class PawnSphere : MonoBehaviour, IPointerClickHandler, IDragHandler
    {
        public Action<PointerEventData> onClick;
        public Action<PointerEventData> onDrag;

        public void OnPointerClick(PointerEventData eventData)
        {
            onClick?.Invoke(eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            onDrag?.Invoke(eventData);
        }
    }
}