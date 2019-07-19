using UnityEngine;

public class Commander : PlayerCharacter
{
    [SerializeField]
    private Map _map;

    private void Start()
    {
        // The map is not a child of the Commander because we don't want it to move with the submarine.
        _map = Instantiate(_map);
        _map.SetCameraPosition(Submarine.transform.position);
    }

    public override void Deactivate()
    {
        Destroy(_map.gameObject);
        base.Deactivate();
    }
}