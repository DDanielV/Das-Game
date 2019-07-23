using UnityEngine;

public class Seamine : ShipBase
{
    [SerializeField] private GameObject _explosionPrefab;
    [SerializeField] private int _explosionDamage = 100;

    private void OnTriggerEnter(Collider other)
    {
        other.GetComponentInParent<ShipBase>()?.DoDamage(_explosionDamage);
        Explode();
    }

    private void Explode()
    {
        Instantiate(_explosionPrefab, transform.position, transform.rotation);
        Destroy(gameObject);
    }

    protected override void Die()
    {
        Explode();
    }
}
