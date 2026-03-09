using UnityEngine;
using UnityEngine.EventSystems;

namespace Runtime
{
    public class Pawn : MonoBehaviour
    {
        private const float MAX_FLOOR_DETECTED_DISTANCE = 5f;
        private Vector3 m_floorDetectOffset = new (0f, 0.2f, 0f);

        [SerializeField] private PawnBody m_pawnBody;
        [SerializeField] private PawnSphere[] m_pawnSpheres;

        private float m_planeDistance;
        private Vector3 m_clickOffset;
        private Plane m_dragPlane;

        private Material m_normalMat;
        private Material m_activeMat;
        private Material m_deleteMat;
        private State m_currentState = State.NORMAL;

        enum State
        {
            NORMAL,
            ACTIVE,
            DELETE
        }

        public void Init(Material _activeMat, Material _deleteMat)
        {
            m_activeMat = _activeMat;
            m_deleteMat = _deleteMat;
        }

        private void Start()
        {
            m_pawnBody.m_onBeginDrag += OnStartDragBody;
            m_pawnBody.m_onEndDrag += OnEndDragBody;
            m_pawnBody.m_onDrag += OnDragBody;
            for (var index = 0; index < m_pawnSpheres.Length; index++)
            {
                m_pawnSpheres[index].m_onClick += OnClickSphere;
                m_pawnSpheres[index].m_onDrag += OnDragSphere;
            }

            m_normalMat = m_pawnBody.m_meshRenderer.sharedMaterial;
        }

        private void OnDragSphere(PointerEventData _pointerEventData)
        { }

        private void OnClickSphere(PointerEventData _pointerEventData)
        { }

        private void OnStartDragBody(PointerEventData _eventData)
        {
            m_dragPlane = new Plane(Vector3.up, transform.position);
            Ray ray = GetCamera(_eventData).ScreenPointToRay(Input.mousePosition);

            if (m_dragPlane.Raycast(ray, out m_planeDistance))
            {
                Vector3 hitPoint = ray.GetPoint(m_planeDistance);
                m_clickOffset = transform.position - hitPoint;
            }
        }

        private void OnDragBody(PointerEventData _eventData)
        {
            MoveToMouse(_eventData);
        }
        
        private void OnEndDragBody(PointerEventData _eventData)
        {
            if (m_currentState == State.DELETE)
            {
                Destroy(gameObject);
                return;
            }

            SetState(State.NORMAL);
        }
        
        private void MoveToMouse(PointerEventData _eventData)
        {
            Ray ray = GetCamera(_eventData).ScreenPointToRay(Input.mousePosition);
            
            if (m_dragPlane.Raycast(ray, out m_planeDistance))
            {
                Vector3 hitPoint = ray.GetPoint(m_planeDistance);
                Vector3 newPosition = hitPoint + m_clickOffset;

                transform.position = newPosition;
            }
            
            UpdateState();
        }
        
        private void UpdateState()
        {
            if (Physics.Raycast(transform.position + m_floorDetectOffset, Vector3.down, out _, MAX_FLOOR_DETECTED_DISTANCE))
            {
                SetState(State.ACTIVE);
            }
            else
            {
                SetState(State.DELETE);
            }
        }

        private void SetState(State _state)
        {
            if (m_currentState == _state) return;
            
            m_currentState = _state;
            switch (_state)
            {
                case State.NORMAL:
                    SetMaterial(m_normalMat);
                    break;
                case State.ACTIVE:
                    SetMaterial(m_activeMat);
                    break;
                case State.DELETE:
                    SetMaterial(m_deleteMat);
                    break;
            }
        }
        
        private void SetMaterial(Material _material)
        {
            m_pawnBody.m_meshRenderer.material = _material;
            for (int i = 0; i < m_pawnSpheres.Length; i++)
            {
                m_pawnSpheres[i].m_meshRenderer.material = _material;
            }
        }
        
        private Camera GetCamera(PointerEventData _eventData)
        {
            return _eventData.enterEventCamera ??
                   _eventData.pressEventCamera ??
                   Camera.main;
        }

        private void OnValidate()
        {
            if (m_pawnBody == null)
                m_pawnBody = gameObject.GetComponentInChildren<PawnBody>(true);
            if (m_pawnSpheres == null || m_pawnSpheres.Length == 0)
                m_pawnSpheres = gameObject.GetComponentsInChildren<PawnSphere>(true);
        }
    }
}