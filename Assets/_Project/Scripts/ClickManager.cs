using Runtime;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickManager : MonoBehaviour
{
    [SerializeField] private LayerMask clickableLayers;
    [SerializeField] private float maxDistance = 100f;
    
    private Camera m_camera;
    
    void Start() => m_camera = Camera.main;
    
    void Update()
    {
        if (!Input.GetMouseButtonDown(0)) return;
        
        if (EventSystem.current?.IsPointerOverGameObject() == true)
            return;
        
        Ray ray = m_camera.ScreenPointToRay(Input.mousePosition);
        
        if (Physics.Raycast(ray, out RaycastHit hit, maxDistance, clickableLayers))
        {
            OnClick(hit.collider.gameObject);
        }
    }
    
    private void OnClick(GameObject clickedObject)
    {
        if (clickedObject.TryGetComponent<IClickable>(out var clickable))
        {
            clickable.OnClick();
        }
    }
}