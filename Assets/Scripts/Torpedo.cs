﻿using UnityEngine;

public class Torpedo : MonoBehaviour
{
    [SerializeField]
    private float _startSpeed = 20;

    [SerializeField]
    private float _maxSpeed = 100;

    [SerializeField]
    private float _acceleration= 100;
    
    private Rigidbody _rigidbody;

    private void Start()
    {
        // Gives the torpedo an initial speed, maybe this should add the current speed of the _submarine.
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.velocity = transform.forward * _startSpeed;

        // Destroy the torpedo after 10 seconds, this is enough for them to escape view. 
        // TODO: Replace this with distance traveled.
        // Object pooling could be used here but is not really needed.
        Destroy(gameObject, 10);
    }

    private void FixedUpdate()
    {
        // Increases the speed of the torpedo over time until the max speed is reached.
        _rigidbody.AddForce(transform.forward * _acceleration);
        _rigidbody.velocity = _rigidbody.velocity.magnitude > _maxSpeed ? transform.forward * _maxSpeed : _rigidbody.velocity;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("boom" + collision.gameObject.name);
    }
}
