using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class PseudoBallisticMovement : MonoBehaviour
{
    public Vector3 targetPoint
    {
        set
        {
            _targetPoint = value;
            Initialize();
        }
    }

    [SerializeField] private AnimationCurve _scaleOverTime;
    [SerializeField] private float _startScale;
    [SerializeField] private AnimationCurve _speedOverTime;
    [SerializeField] private float _speedMultiplier;
    [SerializeField] private AnimationCurve _rotationOverTime;
    [SerializeField] private float _rotationMultiplier;
    [SerializeField] private Vector3 _targetPoint;
    [SerializeField] private float _accuracy;

    private float _startDistance;

    public Action OnReachedTarget;

    private void Initialize()
    {
        _startDistance = Vector3.Distance(transform.position, _targetPoint);
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        float distance = Vector3.Distance(transform.position, _targetPoint);
        Vector3 direction = _targetPoint - transform.position;

        if (distance > _accuracy)
        {
            transform.position += direction.normalized * (_speedOverTime.Evaluate(distance / _startDistance) * (_speedMultiplier * _startDistance) * Time.deltaTime);
            transform.localScale = Vector3.one * (_startScale * _scaleOverTime.Evaluate(distance / _startDistance));
            transform.Rotate(0, 0, _rotationMultiplier * _rotationOverTime.Evaluate(distance / _startDistance));
        }
        else
        {
            OnReachedTarget?.Invoke();
        }
    }
}
