using ClickManager;
using UnityEngine;

public class CameraController : MonoBehaviour, IScroll, IClickDrag, IClickDown
{
    [SerializeField] private float m_dragSpeed = 0.05f;
    [SerializeField] private float m_scrollSpeed = 5f;

    private Vector3 m_cameraStartPos;
    private Vector3 m_dragStartWorldPos;
    private Camera m_camera;
    private Plane m_groundPlane;
    float m_distance;
    private Vector3 m_dragOrigin;

    private void Start()
    {
        m_camera = GetComponent<Camera>();
        if (m_camera == null)
            m_camera = Camera.main;

        m_groundPlane = new Plane(Vector3.up, Vector3.zero);
    }

    public void OnScroll(float _scrollDelta)
    {
        Ray ray = m_camera.ScreenPointToRay(Input.mousePosition);

        //находим точку на плоскости Y=0
        Vector3 targetPoint;
        if (m_groundPlane.Raycast(ray, out m_distance))
        {
            targetPoint = ray.GetPoint(m_distance);
        }
        else
        {
            //если луч не попадает в плоскость
            targetPoint = transform.position + transform.forward * 10f;
        }

        //направление от камеры к целевой точке
        Vector3 direction = (targetPoint - transform.position).normalized;

        // Двигаем камеру по направлению к/от курсора
        float moveDistance = _scrollDelta * m_scrollSpeed;
        Vector3 newPosition = transform.position + direction * moveDistance;

        transform.position = newPosition;
    }

    public void OnClickDown()
    {
        // Запоминаем начальную позицию мыши И камеры
        m_dragOrigin = Input.mousePosition;
        m_cameraStartPos = transform.position;
    }

    public void OnClickDrag()
    {
        // Вычисляем смещение мыши от начальной точки
        Vector3 mouseDelta = Input.mousePosition - m_dragOrigin;

        Vector3 move = new Vector3(-mouseDelta.x, 0, -mouseDelta.y) * m_dragSpeed;

        // Применяем смещение относительно начальной позиции камеры
        transform.position = m_cameraStartPos + move;
    }
}