using EditorAttributes;
using UnityEngine;

namespace Runtime
{
    public class ConnectionLine : MonoBehaviour
    {
        [SerializeField] private LineRenderer m_lineRenderer;
        [SerializeField, ReadOnly] private PawnConnector m_sphereStart;
        [SerializeField, ReadOnly] private PawnConnector m_sphereEnd;

        private Color endNormalColor = Color.white;
        private Color endInfinityColor = new(1, 0, 0, 0);
        private Color startNormalColor = Color.white;
        private Color startInfinityColor = new(1, 0, 0, 1);

        private bool hasEnd = false;
        private Camera m_camera;
        private RaycastHit hitInfo;

        private void Start()
        {
            m_lineRenderer.endColor = endNormalColor;
            m_lineRenderer.startColor = startNormalColor;

            m_lineRenderer.startWidth = 0.07f;
            m_lineRenderer.endWidth = 0.07f;

            m_camera = Camera.main;
        }

        private void Update()
        {
            UpdateEndToMouse();
        }

        public void SetStart(PawnConnector _startSphere)
        {
            m_sphereStart = _startSphere;
            StartPointChange(_startSphere);
            //Чтобы не было мигания из начала координат
            EndPointChange(_startSphere);
            m_sphereStart.m_transformChange += StartPointChange;
            m_sphereStart.m_connectorDestroyed += ConnectorDestroyed;
            hasEnd = false;
        }

        public void SetEnd(PawnConnector _endSphere)
        {
            m_sphereEnd = _endSphere;
            EndPointChange(m_sphereEnd);
            m_sphereEnd.m_transformChange += EndPointChange;
            m_sphereEnd.m_connectorDestroyed += ConnectorDestroyed;
            hasEnd = true;
        }
        
        private void StartPointChange(PawnConnector _obj)
        {
            if (_obj == null || _obj.gameObject == null)
            {
                return;
            }

            m_lineRenderer.SetPosition(0, _obj.transform.position);
        }

        private void EndPointChange(PawnConnector _obj)
        {
            if (_obj == null || _obj.gameObject == null)
            {
                return;
            }

            m_lineRenderer.SetPosition(1, _obj.transform.position);
            m_lineRenderer.startColor = startNormalColor;
            m_lineRenderer.endColor = endNormalColor;
        }

        private void UpdateEndToMouse()
        {
            if (hasEnd)
                return;

            Ray ray = m_camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hitInfo))
            {
                Vector3 hitPoint = ray.GetPoint(hitInfo.distance);
                m_lineRenderer.SetPosition(1, hitPoint);

                if (m_lineRenderer.endColor != endNormalColor)
                    m_lineRenderer.endColor = endNormalColor;
                if (m_lineRenderer.startColor != startNormalColor)
                    m_lineRenderer.startColor = startNormalColor;
            }
            else
            {
                Vector3 hitPoint = ray.GetPoint(10);
                m_lineRenderer.SetPosition(1, hitPoint);

                if (m_lineRenderer.endColor != endInfinityColor)
                    m_lineRenderer.endColor = endInfinityColor;
                if (m_lineRenderer.startColor != startInfinityColor)
                    m_lineRenderer.startColor = startInfinityColor;
            }
        }
        
        private void ConnectorDestroyed()
        {
            Destroy(gameObject);
        }

        private void OnDestroy()
        {
            if (m_sphereStart != null)
            {
                m_sphereStart.m_transformChange -= StartPointChange;
                m_sphereStart.m_connectorDestroyed -= ConnectorDestroyed;
            }

            if (m_sphereEnd != null)
            {
                m_sphereEnd.m_transformChange -= EndPointChange;
                m_sphereEnd.m_connectorDestroyed -= ConnectorDestroyed;
            }
        }
    }
}