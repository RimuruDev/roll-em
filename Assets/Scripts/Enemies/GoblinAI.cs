using UnityEngine;

public class GoblinAI : DefaultEnemyAI
{
    [Header("Goblin settings")]
    [SerializeField] private float _animationSpeed = 1;
    private Animator _animator;
    [SerializeField] private ParticleSystem _partSys;

    private void Awake()
    {
        InitializeHP();
        _animator = GetComponent<Animator>();
        _partSys = GetComponentInChildren<ParticleSystem>();
        OnDamaged += EmitBlood;
        OnDamaged += PlayDamagedSound;
        OnDeath += PlayDeathSound;
        OnDeath += SetBloodshot;
    }

    private void SetBloodshot()
    {
        Instantiate(Links.bloodshot, transform.position, Quaternion.identity);
    }

    private void Update()
    {
        _animator.SetFloat("RunSpeed", _rb.linearVelocity.magnitude / _maxSpeed * _animationSpeed);
    }

    private void EmitBlood()
    {
        _partSys.Emit((int)(_maxHP - _HP) * (PlayerData.bloodMode ? 100 : 1));
    }

    private void PlayDamagedSound()
    {
        Links.soundManager.PlayOneshotClip(SoundOneshots[0], GameSettings.soundVolume, Random.Range(_minPitch, _maxPitch), true, transform.position);
    }

    private void PlayDeathSound()
    {
        Links.soundManager.PlayOneshotClip(SoundOneshots[1], GameSettings.soundVolume, Random.Range(_minPitch, _maxPitch), true, transform.position);
    }

    private void OnDestroy()
    {
        _partSys.transform.SetParent(null);
        EmitBlood();
        _partSys.GetComponent<SelfDestroyParticleSystem>().enabled = true;
        _partSys.GetComponent<SelfDestroyParticleSystem>().SetSelfDestroyOn();
        OnDamaged -= EmitBlood;
        OnDamaged -= PlayDamagedSound;
        OnDeath -= PlayDeathSound;
        OnDeath -= SetBloodshot;
    }
}
