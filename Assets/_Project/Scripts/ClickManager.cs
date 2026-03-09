using Runtime;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickManager : MonoBehaviour
{
    [SerializeField] private LayerMask m_clickableLayers;
    [SerializeField] private float m_maxDistance = 100f;
    
    private Camera m_camera;
    
    void Start() => m_camera = Camera.main;
    
    void Update()
    {
        if (!Input.GetMouseButtonDown(0)) return;
        
        if (EventSystem.current?.IsPointerOverGameObject() == true)
            return;
        
        Ray ray = m_camera.ScreenPointToRay(Input.mousePosition);
        
        if (Physics.Raycast(ray, out RaycastHit hit, m_maxDistance, m_clickableLayers))
        {
            OnClick(hit.collider.gameObject);
        }
    }
    
    private void OnClick(GameObject _clickedObject)
    {
        if (_clickedObject.TryGetComponent<IClickable>(out var clickable))
        {
            clickable.OnClick();
        }
    }
}