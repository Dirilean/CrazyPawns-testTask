using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Runtime
{
    [RequireComponent(typeof(Collider))]
    public class PawnSphere : MonoBehaviour, IPointerClickHandler, IDragHandler
    {
        [SerializeField] public MeshRenderer m_meshRenderer;
        
        public Action<PointerEventData> m_onClick;
        public Action<PointerEventData> m_onDrag;

        public void OnPointerClick(PointerEventData _eventData)
        {
            m_onClick?.Invoke(_eventData);
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