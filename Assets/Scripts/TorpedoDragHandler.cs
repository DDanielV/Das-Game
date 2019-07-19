using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TorpedoDragHandler : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [SerializeField]
    private GameObject _torpedoPrefab;

    public static GameObject _torpedoBeingDragged;
    private Image _torpedoIcon;
    private Image _tempIcon;
    private GridLayoutGroup _panelLayoutGroup;

    private void Start()
    {
        _torpedoIcon = GetComponent<Image>();
        _panelLayoutGroup = GetComponentInParent<GridLayoutGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // Disable the GridLayout of the panel to prevent jumping icons when instantiating the tempIcon.
        _panelLayoutGroup.enabled = false;
      
        _tempIcon = Instantiate(_torpedoIcon, transform.parent);
        _tempIcon.raycastTarget = false;
        _torpedoBeingDragged = _torpedoPrefab;
    }

    public void OnDrag(PointerEventData eventData)
    {
        _tempIcon.transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Destroy(_tempIcon.gameObject);        
    }
}
