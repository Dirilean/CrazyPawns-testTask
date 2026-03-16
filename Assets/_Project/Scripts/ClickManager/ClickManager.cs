using System.Collections.Generic;
using UnityEngine;

namespace ClickManager
{
    public class ClickManager : MonoBehaviour
    {
        [SerializeField] private float m_dragAllowance = 0.05f;

        [SerializeField] private float m_maxDistance = 100f;
        [SerializeField] private GameObject[] m_clicableNotPhysicBody;

        private List<IClickDown> m_mouseDownDummy = new();
        private List<IClickDrag> m_mouseDragDummy = new();
        private List<IClickDragEnd> m_mouseDragEndDummy = new();
        private List<IScroll> m_mouseScrollDummy = new();

        private Camera m_camera;
        private IClickDrag m_draggingObj;
        private Vector3 m_lastMouseDownPos;
        private bool m_isDragProcess;
        private bool m_clickOnInteractable;
        
        void Start()
        {
            m_camera = Camera.main;
            //Переводим все объекты в соотвествующие интерфейсы
            for (int i = 0; i < m_clicableNotPhysicBody.Length; i++)
            {
                if (m_clicableNotPhysicBody[i] == null) continue;

                if (m_clicableNotPhysicBody[i].TryGetComponent(out IClickDown obj1))
                    m_mouseDownDummy.Add(obj1);

                if (m_clicableNotPhysicBody[i].TryGetComponent(out IClickDrag obj2))
                    m_mouseDragDummy.Add(obj2);

                if (m_clicableNotPhysicBody[i].TryGetComponent(out IClickDragEnd obj3))
                    m_mouseDragEndDummy.Add(obj3);

                if (m_clicableNotPhysicBody[i].TryGetComponent(out IScroll obj4))
                    m_mouseScrollDummy.Add(obj4);
            }
        }

        void Update()
        {
            ProcessClick();
            ProcessScroll();
        }

        private void ProcessScroll()
        {
            float scrollDelta = Input.GetAxis("Mouse ScrollWheel");
            if (Mathf.Abs(scrollDelta) > 0.01f)
            {
                for (int i = 0; i < m_mouseScrollDummy.Count; i++)
                {
                    m_mouseScrollDummy[i]?.OnScroll(scrollDelta);
                }
            }
        }

        //Если нажата лкм то выбираем соотвествующий предмет и вызываем его метод,
        //если объекта нет то вызываем для всех не имеющих тела
        private void ProcessClick()
        {
            //обрабатываем клик
            if (Input.GetMouseButtonDown(0))
            {
                m_isDragProcess = false;
                m_lastMouseDownPos = Input.mousePosition;
                Ray ray = m_camera.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit, m_maxDistance))
                {
                    hit.collider.TryGetComponent<IClickDown>(out var clickable);
                    hit.collider.TryGetComponent<IClickDrag>(out m_draggingObj);
                    if (clickable != null)
                    {
                        m_clickOnInteractable = true;
                        clickable.OnClickDown();
                        return;
                    }
                }

                for (int i = 0; i < m_mouseDownDummy.Count; i++)
                {
                    if (m_mouseDownDummy[i] != null)
                    {
                        m_mouseDownDummy[i].OnClickDown();
                    }
                }
            }
            //обрабатываем окончание перетаскивания
            else if (Input.GetMouseButtonUp(0) && m_isDragProcess)
            {
                m_clickOnInteractable = false;
                m_isDragProcess = false;
                m_draggingObj = null;

                Ray ray = m_camera.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit, m_maxDistance))
                {
                    hit.collider.TryGetComponent<IClickDragEnd>(out var clickable);
                    if (clickable != null)
                    {
                        clickable.OnClickDragEnd();
                        return;
                    }
                }

                for (int i = 0; i < m_mouseDragEndDummy.Count; i++)
                {
                    m_mouseDragEndDummy[i]?.OnClickDragEnd();
                }
            }
            //обратаываем процесс перетаскивания
            else if (Input.GetMouseButton(0) &&
                     (m_lastMouseDownPos - Input.mousePosition).sqrMagnitude > m_dragAllowance)
            {
                m_isDragProcess = true;
                if (m_draggingObj != null)
                {
                    m_draggingObj.OnClickDrag();
                    return;
                }

                //если кликнули по объекту и его действия выполняются то не нужно выполнять перетаскивание для пустышек
                if (!m_clickOnInteractable)
                {
                    for (int i = 0; i < m_mouseDragDummy.Count; i++)
                    {
                        m_mouseDragDummy[i]?.OnClickDrag();
                    }
                }
            }
        }
    }
}