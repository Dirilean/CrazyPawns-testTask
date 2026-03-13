using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Runtime
{
    [RequireComponent(typeof(Collider))]
    public class PawnConnector : MonoBehaviour, IPointerDownHandler, IDragHandler
    {
        public bool m_allowConnect = false;
        [SerializeField] public MeshRenderer m_meshRenderer;
        
        public Action<PawnConnector> m_onClick;
        public Action<PointerEventData> m_onDrag;
        public Action<PawnConnector> m_transformChange;

        public void OnPointerDown(PointerEventData _eventData)
        {
            m_onClick?.Invoke(this);
        }

        public void OnDrag(PointerEventData _eventData)
        {
            m_onDrag?.Invoke(_eventData);
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