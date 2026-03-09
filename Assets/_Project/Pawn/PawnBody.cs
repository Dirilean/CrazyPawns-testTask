using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Runtime
{
    [RequireComponent(typeof(Collider))]
    public class PawnBody : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        [SerializeField] public MeshRenderer m_meshRenderer;
        
        public Action<PointerEventData> m_onBeginDrag;
        public Action<PointerEventData> m_onEndDrag;
        public Action<PointerEventData> m_onDrag;
        
        public void OnDrag(PointerEventData _eventData)
        {
            m_onDrag?.Invoke(_eventData);
        }

        public void OnBeginDrag(PointerEventData _eventData)
        {
            m_onBeginDrag?.Invoke(_eventData);
        }

        public void OnEndDrag(PointerEventData _eventData)
        {
            m_onEndDrag?.Invoke(_eventData);
        }

        private void OnValidate()
        {
            if (m_meshRenderer == null)
            {
                m_meshRenderer = GetComponent<MeshRenderer>();
            }
        }
    }
}