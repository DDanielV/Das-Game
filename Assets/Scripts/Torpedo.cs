using UnityEngine;

public class Torpedo : MonoBehaviour
{
    [SerializeField] private float _startSpeed = 20;
    [SerializeField] private float _maxSpeed = 100;
    [SerializeField] private float _acceleration = 100;
    [SerializeField] private float _reachInUnityUnits = 500;
    private Vector3 _startingPoint;

    private Rigidbody _rigidbody;

    private void Start()
    {
        // Gives the torpedo an initial speed, maybe this should add the current speed of the _submarine.
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.velocity = transform.forward * _startSpeed;

        _startingPoint = transform.position;
    }

    private void FixedUpdate()
    {
        // Destroy the torpedo after it has traveled it's reach. We could use object pooling instead but it's not really needed.
        if (Vector3.Distance(_startingPoint, transform.position) >= _reachInUnityUnits)
        {
            Destroy(gameObject);
        }

        // Increases the speed of the torpedo over time until the max speed is reached.
        _rigidbody.AddForce(transform.forward * _acceleration);
        _rigidbody.velocity = _rigidbody.velocity.magnitude > _maxSpeed ? transform.forward * _maxSpeed : _rigidbody.velocity;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("boom" + collision.gameObject.name);
    }
}
