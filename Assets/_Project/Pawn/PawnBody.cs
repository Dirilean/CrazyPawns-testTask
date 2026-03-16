using System;
using Clickable;
using UnityEngine;

namespace Runtime
{
    [RequireComponent(typeof(Collider))]
    public class PawnBody : MonoBehaviour, IClickDrag,IClickDown, IClickDragEnd
    {
        [SerializeField] public MeshRenderer m_meshRenderer;
        
        public Action m_onBeginDrag;
        public Action m_onEndDrag;
        public Action m_onDrag;
        
        private bool isDrag = false;
        
        public void OnClickDrag()
        {
            isDrag = true;
            m_onDrag?.Invoke();
        }

        public void OnClickDown()
        {
            isDrag = false;
            m_onBeginDrag?.Invoke();
        }

        public void OnClickDragEnd()
        {
            if (isDrag)
            {
                m_onEndDrag?.Invoke();
            }
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