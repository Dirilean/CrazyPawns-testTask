using System;
using Clickable;
using UnityEngine;

namespace Runtime
{
    [RequireComponent(typeof(Collider))]
    public class PawnConnector : MonoBehaviour, IClickDown, IClickDragEnd
    {
        [SerializeField] public MeshRenderer m_meshRenderer;

        public bool m_allowConnect = false;
        public Action<PawnConnector> m_onPointerDown;
        public Action<PawnConnector> m_onEndDrag;
        public Action<PawnConnector> m_transformChange;
        public Action m_connectorDestroyed;

        private void OnDestroy()
        {
            m_connectorDestroyed?.Invoke();
        }

        public void OnClickDown()
        {
            m_onPointerDown?.Invoke(this);
        }

        public void OnClickDragEnd()
        {
            m_onEndDrag?.Invoke(this);
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