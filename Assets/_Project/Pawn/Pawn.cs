using UnityEngine;
using UnityEngine.EventSystems;

namespace Runtime
{
    public class Pawn : MonoBehaviour
    {
        [SerializeField] private PawnBody m_pawnBody;
        [SerializeField] private PawnSphere[] m_pawnSpheres;
        
        private float planeDistance;
        private Vector3 clickOffset;
        private Plane dragPlane;
        
        private Camera getCamera(PointerEventData eventData) => eventData.enterEventCamera ??
                                                                eventData.pressEventCamera ??
                                                                Camera.main;
        
        private void Start()
        {
            m_pawnBody.onBeginDrag += OnStartDragBody;
            m_pawnBody.onEndDrag += OnEndDragBody;
            m_pawnBody.onDrag += OnDragBody;
            for (var index = 0; index < m_pawnSpheres.Length; index++)
            {
                m_pawnSpheres[index].onClick += OnClickSphere;
                m_pawnSpheres[index].onDrag += OnDragSphere;
            }
        }

        private void OnDragSphere(PointerEventData _pointerEventData)
        { }

        private void OnClickSphere(PointerEventData _pointerEventData)
        { }
        
        private void OnStartDragBody(PointerEventData eventData)
        {
            dragPlane = new Plane(Vector3.up, transform.position);
            Ray ray = getCamera(eventData).ScreenPointToRay(Input.mousePosition);

            if (dragPlane.Raycast(ray, out planeDistance))
            {
                Vector3 hitPoint = ray.GetPoint(planeDistance);
                clickOffset = transform.position - hitPoint;
            }
        }

        private void OnDragBody(PointerEventData eventData)
        {
            Ray ray = getCamera(eventData).ScreenPointToRay(Input.mousePosition);
            
            if (dragPlane.Raycast(ray, out planeDistance))
            {
                Vector3 hitPoint = ray.GetPoint(planeDistance);
                Vector3 newPosition = hitPoint + clickOffset;

                transform.position = newPosition;
            }
        }
        
        private void OnEndDragBody(PointerEventData _obj)
        {
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