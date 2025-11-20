using System.Collections;
using UnityEngine;

public class GoblinAI : DefaultEnemyAI
{
    public float remainAttackCooldown { get => _remainAttackCooldown; set => _remainAttackCooldown = value; }

    [Header("Goblin settings")]
    [SerializeField] private float _animationSpeed = 1;

    private Animator _animator;
    private Coroutine _currentAttack;
    private float _remainAttackCooldown;


    private new void Awake()
    {
        base.Awake();
        _animator = GetComponent<Animator>();
        _remainAttackCooldown = _attackCooldown;

        OnReachTarget += StartAttack;
        OnLeaveTarget += StopAttack;
    }

    private void Update()
    {
        _animator.SetFloat(_animator.parameters[0].name, _rb.linearVelocity.magnitude / _maxSpeed * _animationSpeed);
    }

    protected virtual void StartAttack()
    {
        _currentAttack = StartCoroutine(Attack());
    }

    private void StopAttack()
    {
        if (_currentAttack != null)
        {
            StopCoroutine(_currentAttack);
        }
    }

    private IEnumerator Attack()
    {
        while (true)
        {
            while (_remainAttackCooldown > 0)
            {
                yield return null;
                _remainAttackCooldown -= Time.deltaTime;
            }
            _targetDamageable.TakeDamage(_damage);
            _remainAttackCooldown = _attackCooldown;
        }
    }

    private new void OnDestroy()
    {
        base.OnDestroy();
        OnReachTarget -= StartAttack;
        OnLeaveTarget -= StopAttack;
    }
}
