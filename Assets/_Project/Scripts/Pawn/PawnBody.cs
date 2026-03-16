using System;
using ClickManager;
using UnityEngine;

namespace Pawn
{
    [RequireComponent(typeof(Collider))]
    public class PawnBody : MonoBehaviour, IClickDrag,IClickDown, IClickDragEnd
    {
        [SerializeField] public MeshRenderer m_meshRenderer;
        
        public Action m_onBeginDrag;
        public Action m_onEndDrag;
        public Action m_onDrag;
        
        public void OnClickDrag()
        {
            m_onDrag?.Invoke();
        }

        public void OnClickDown()
        {
            m_onBeginDrag?.Invoke();
        }

        public void OnClickDragEnd()
        {
            m_onEndDrag?.Invoke();
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