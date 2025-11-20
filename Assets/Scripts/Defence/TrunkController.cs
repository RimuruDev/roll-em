using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TrunkController : MonoBehaviour
{
    public float rotateSpeed { get => _rotateSpeed; set => _rotateSpeed = value; }

    [SerializeField] private float _rotateSpeed = 5;
    private Rigidbody2D _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (Input.GetAxis("Horizontal") != 0)
        {
            RotatePivot();
        }
    }

    private void RotatePivot()
    {
        float direction = -Input.GetAxis("Horizontal");

        if (direction == 0) return;

        //_rb.AddTorque(_rotateSpeed * direction);
        _rb.MoveRotation(_rb.rotation + direction * _rotateSpeed);
    }
}
