using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    private Transform _target;
    private Rigidbody2D _rigidBody;
    private float _angleChangingSpeed = 360f;
    private float _movementSpeed = 10f;
    private Transform _thruster;
    private bool _missileFired = false;

    private void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        if (_rigidBody == null)
        {
            Debug.LogError("Rigidbody2D is null in missle");
        }
        _thruster = gameObject.transform.GetChild(0);
        if (_thruster == null)
        {
            Debug.LogError("Thruster in missile is null");
        }
    }

    void Update()
    {
        if (_missileFired == true)
        {
            Vector2 direction = (Vector2)_target.position - _rigidBody.position;
            direction.Normalize();
            float rotateAmount = Vector3.Cross(direction, transform.up).z;
            _rigidBody.angularVelocity = -_angleChangingSpeed * rotateAmount;
            _rigidBody.velocity = transform.up * _movementSpeed;
        }
    }

    public void FireMissile(Transform target)
    {
        _target = target;
        _missileFired = true;
        _thruster.gameObject.SetActive(true);
    }
}
