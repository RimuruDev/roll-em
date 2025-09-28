using System;
using System.Collections;
using UnityEngine;

using Random = UnityEngine.Random;

public class DefaultEnemyAI : EnemyAI
{
    [Header("Default enemy settings")]
    [SerializeField] protected float _speed;
    [SerializeField] protected float _maxSpeed;
    [SerializeField] protected float _rotationSpeed = 200f;
    [SerializeField] protected float _attackCooldown = 1f;

    protected Rigidbody2D _rb;
    protected Damageable _targetDamageable;

    private Transform _target;
    private ParticleSystem _bloodPartSys;

    protected Action OnReachTarget;
    protected Action OnLeaveTarget;

    protected new void Awake()
    {
        base.Awake();

        _bloodPartSys = GetComponentInChildren<ParticleSystem>();

        OnDamaged += EmitBlood;
        OnDamaged += PlayDamagedSound;
        OnDeath += PlayDeathSound;
        OnDeath += SetBloodshot;
    }

    protected void Start()
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

        OnBroken += () => Death();
    }

    protected void FixedUpdate()
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
            OnReachTarget?.Invoke();
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform == _target)
        {
            OnLeaveTarget?.Invoke();
        }
    }

    private void PlayDamagedSound()
    {
        Links.soundManager.PlayOneshotClip(SoundOneshots[0], GameSettings.soundVolume, Random.Range(_minPitch, _maxPitch), true, transform.position);
    }

    private void EmitBlood()
    {
        _bloodPartSys.Emit((int)(_maxHP - _HP) * (PlayerData.bloodMode ? 100 : 1));
    }

    private void SetBloodshot()
    {
        Instantiate(Links.bloodshot, transform.position, Quaternion.identity);
    }

    private void PlayDeathSound()
    {
        Links.soundManager.PlayOneshotClip(SoundOneshots[1], GameSettings.soundVolume, Random.Range(_minPitch, _maxPitch), true, transform.position);
    }

    protected void Death(bool dropCoins = true, bool countKill = true)
    {
        if (countKill) PlayerData.kills++;
        if (dropCoins) DropCoins();
        OnDeath?.Invoke();
        OnAnyDeath?.Invoke();
        Destroy(gameObject);
    }

    private void DropCoins()
    {
        CoinDrop drop = Instantiate(Links.coinPrefab, transform.position, Quaternion.identity).GetComponent<CoinDrop>();
        drop.coinsCount = Mathf.RoundToInt(_reward * PlayerData.currentGameSpeed);
    }

    protected void OnDestroy()
    {
        _bloodPartSys.transform.SetParent(null);
        EmitBlood();
        _bloodPartSys.GetComponent<SelfDestroyParticleSystem>().enabled = true;
        _bloodPartSys.GetComponent<SelfDestroyParticleSystem>().SetSelfDestroyOn();

        OnBroken -= () => Death();
        OnDamaged -= EmitBlood;
        OnDamaged -= PlayDamagedSound;
        OnDeath -= PlayDeathSound;
        OnDeath -= SetBloodshot;
    }
}
