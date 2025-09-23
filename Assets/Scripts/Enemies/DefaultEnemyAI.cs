using System.Collections;
using UnityEngine;
using static UnityEngine.GridBrushBase;

public class DefaultEnemyAI : EnemyAI
{
    [Header("Default enemy settings")]
    [SerializeField] protected float _speed;
    [SerializeField] protected float _maxSpeed;
    [SerializeField] protected float _rotationSpeed = 200f;
    [SerializeField] protected float _attackCooldown = 1f;

    protected Rigidbody2D _rb;
    private Transform _target;
    private Damageable _targetDamageable;
    private Coroutine _currentAttack;

    void Start()
    {
        switch (_targetType)
        {
            case Targets.tower:
                _target = Links.tower;
                break;
            case Targets.mainShield:
                _target = Links.mainShield;
                break;
        }

        _targetDamageable = _target.GetComponent<Damageable>();

        _rb = GetComponent<Rigidbody2D>();

        OnBroken += Death;
    }

    void FixedUpdate()
    {
        Vector2 direction = (_target.position - transform.position).normalized;

        // 2. Определить целевой угол поворота
        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;

        // 3. Рассчитать разницу углов (учитывая кратчайший путь)
        float angleDifference = Mathf.DeltaAngle(transform.rotation.eulerAngles.z, targetAngle);

        // 4. Рассчитать величину поворота в этом кадре
        float rotationThisFrame = Mathf.Clamp(_rotationSpeed * Time.fixedDeltaTime, 0, Mathf.Abs(angleDifference));

        // 5. Определить направление поворота
        float rotationDirection = Mathf.Sign(angleDifference);

        // 6. Применить поворот к RigidBody2D
        _rb.MoveRotation(transform.rotation.eulerAngles.z + rotationThisFrame * rotationDirection);

        if (_rb.linearVelocity.magnitude < _maxSpeed)
        {
            _rb.AddForce(direction * _speed);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform == _target)
        { 
            _currentAttack = StartCoroutine(Attack());
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform == _target)
        {
            StopCoroutine(_currentAttack);
        }
    }

    private IEnumerator Attack()
    {
        while (true)
        {
            _targetDamageable.TakeDamage(_damage);
            yield return new WaitForSeconds(_attackCooldown);
        }
    }

    private void Death()
    {
        PlayerData.kills++;
        OnDeath?.Invoke();
        OnAnyDeath?.Invoke();
        DropCoins();
        Destroy(gameObject);
    }

    private void DropCoins()
    {
        CoinDrop drop = Instantiate(Links.coinPrefab, transform.position, Quaternion.identity).GetComponent<CoinDrop>();
        drop.coinsCount = Mathf.RoundToInt(_reward * PlayerData.currentGameSpeed);
    }

    private void OnDestroy()
    {
        OnBroken -= Death;
    }
}
