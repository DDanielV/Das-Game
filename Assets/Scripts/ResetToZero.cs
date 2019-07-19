using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ResetToZero : MonoBehaviour, IEndDragHandler
{
    private Slider _slider;

    private void Start()
    {
        _slider = gameObject.GetComponent<Slider>();
    }

    public void OnEndDrag(PointerEventData data)
    {
        _slider.value = 0f;
    }
}
