using UnityEngine;
using UnityEngine.Networking;

public abstract class ShipBase : NetworkBehaviour
{
    [SyncVar][SerializeField] private int _hitpoints;

    public void DoDamage(int damage)
    {
        _hitpoints -= damage;
        if (_hitpoints <= 0)
        {
            Die();
        }
    }

    protected abstract void Die();
}
