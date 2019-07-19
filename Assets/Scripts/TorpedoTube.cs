using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;

public class TorpedoTube : MonoBehaviour, IDropHandler
{
    // For now this is fine, we might want to change this in the future when we different types of submarines.
    [SerializeField]
    private Transform _torpedoTubeTransform;
    private GameObject _loadedTorpedoPrefab;
    private Image _tubeIcon;
    private bool _isLoaded;
    private Color _unloadedColor;

    private void Start()
    {
        _tubeIcon = GetComponent<Image>();
        _unloadedColor = _tubeIcon.color;
    }

    public void OnDrop(PointerEventData eventData)
    {
        Load(TorpedoDragHandler._torpedoBeingDragged);
    }

    public void Fire()
    {
        if (_isLoaded)
        {
            GameObject torpedo = Instantiate(_loadedTorpedoPrefab, _torpedoTubeTransform.position, _torpedoTubeTransform.rotation);
            NetworkServer.Spawn(torpedo);
            Unload();
        }
    }

    private void Unload()
    {
        _isLoaded = false;
        _tubeIcon.color = _unloadedColor;        
    }

    private void Load(GameObject torpedoPrefab)
    {
        _loadedTorpedoPrefab = torpedoPrefab;
        _tubeIcon.color = Color.black;
        _isLoaded = true;
    }
}
