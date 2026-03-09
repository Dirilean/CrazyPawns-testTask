using EditorAttributes;
using UnityEngine;

namespace Runtime
{
    public class ConnectionLine : MonoBehaviour
    {
        [SerializeField] private LineRenderer m_lineRenderer;
        [SerializeField, ReadOnly] private PawnSphere m_sphereStart;
        [SerializeField, ReadOnly] private PawnSphere m_sphereEnd;

        private void Start()
        {
            m_lineRenderer.endColor = Color.white;
            m_lineRenderer.startColor = Color.white;

            m_lineRenderer.startWidth = 0.07f;
            m_lineRenderer.endWidth = 0.07f;
        }

        public void SetStart(PawnSphere _startSphere)
        {
            m_sphereStart = _startSphere;
            StartPointChange(_startSphere);
            m_sphereStart.m_transformChange += StartPointChange;
        }

        public void SetEnd(PawnSphere _endSphere)
        {
            m_sphereEnd = _endSphere;
            EndPointChange(m_sphereEnd);
            m_sphereEnd.m_transformChange += EndPointChange;
        }

        private void StartPointChange(PawnSphere _obj)
        {
            if (_obj == null || _obj.gameObject == null)
            {
                Destroy(gameObject);
                return;
            }

            m_lineRenderer.SetPosition(0, _obj.transform.position);
        }

        private void EndPointChange(PawnSphere _obj)
        {
            if (_obj == null || _obj.gameObject == null)
            {
                Destroy(gameObject);
                return;
            }

            m_lineRenderer.SetPosition(1, _obj.transform.position);
        }

        // private void Update()
        // {
        //     if (m_sphereEnd != null)
        //         return;
        // }
    }
}