using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Runtime
{
    [RequireComponent(typeof(Collider))]
    public class PawnConnector : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
    {
        public bool m_allowConnect = false;
        [SerializeField] public MeshRenderer m_meshRenderer;
        
        public Action<PawnConnector> m_onPointerDown;
        public Action<PointerEventData> m_onEndDrag;
        public Action<PawnConnector> m_transformChange;
        public Action m_connectorDestroyed;

        private bool isDrag = false;
        
        public void OnPointerDown(PointerEventData _eventData)
        {
            isDrag = false;
            m_onPointerDown?.Invoke(this);
        }

        public void OnPointerUp(PointerEventData _eventData)
        {
            if (isDrag)
            {
                m_onEndDrag?.Invoke(_eventData);
            }
        }

        private void OnValidate()
        {
            if (m_meshRenderer == null)
            {
                m_meshRenderer = GetComponent<MeshRenderer>();
            }
        }
        
        public void OnDrag(PointerEventData eventData)
        {
            isDrag = true;
        }

        private void OnDestroy()
        {
            m_connectorDestroyed?.Invoke();
        }
    }
}