using UnityEngine;

public class Shield : Damageable
{
    [Header("Shield settings")]
    [SerializeField] private float _damageMultiplier = 1f;
    [SerializeField] private float _enemyCollisionDamage = 1f;
    private Rigidbody2D _rb;
    private Animator _animator;
    private SpriteRenderer _sr;

    private Sprite _lastSprite;

    private new void Awake()
    {
        base.Awake();
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _sr = GetComponent<SpriteRenderer>();

        _lastSprite = _sr.sprite;
    }

    private void Update()
    {
        _animator.SetFloat("RotationSpeed", _rb.angularVelocity / 100);

        if (_sr.sprite != _lastSprite)
        {
            AudioClip clip = SoundOneshots[Random.Range(0, SoundOneshots.Length)];
            float pitch = Random.Range(_minPitch, _maxPitch);
            Links.soundManager.PlayOneshotClip(clip, GameSettings.soundVolume, pitch, true, transform.position);
        }
        _lastSprite = _sr.sprite;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.TryGetComponent(out EnemyAI enemy))
        {
            Rigidbody2D rb = GetComponent<Rigidbody2D>();

            float normalImpulse = 0;
            for (int i = 0; i < collision.contactCount; i++)
            {
                normalImpulse += collision.GetContact(i).normalImpulse;
            }

            TakeDamage(_enemyCollisionDamage);

            enemy.TakeDamage(normalImpulse * _damageMultiplier);
        }
    }
}
