using UnityEngine;
using UnityEngine.Networking;

// The PlayerCharacter class is the base class for the different PlayerCharacters.
public class PlayerCharacter : NetworkBehaviour
{
    private Camera _mainCamera;
    protected Camera _characterCamera;

    protected Canvas _characterCanvas;

    public GameObject Submarine { get; set; }

    private bool _initialized;

    private void Initialize()
    {
        // Get a reference to the main camera so we can disable and activate it.
        _mainCamera = Camera.main;

        // Get a reference to the character specific camera and canvas.
        _characterCamera = GetComponentInChildren<Camera>();
        _characterCanvas = GetComponentInChildren<Canvas>();

        // Place the character in the submarine.
        transform.parent = Submarine.transform;
        transform.localPosition = new Vector3(0, 0, 0);

        _initialized = true;
    }

    public virtual void Activate()
    {
        gameObject.SetActive(true);

        if (!_initialized)
        {
            Initialize();
        }

        _mainCamera.gameObject.SetActive(false);
    }

    public virtual void Deactivate()
    {
        _mainCamera.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }
}
