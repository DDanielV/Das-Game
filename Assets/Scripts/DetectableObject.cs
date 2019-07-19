using UnityEngine;

public class DetectableObject: MonoBehaviour
{
    private SpriteRenderer _icon;

    private void Start()
    {
        _icon = GetComponent<SpriteRenderer>();
    }

    public void SetIconActive(bool value)
    {       
        _icon.enabled = value;
    }
}
