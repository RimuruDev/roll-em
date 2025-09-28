using UnityEngine;

public class Tower : Damageable
{
    [SerializeField] private float _maxFireRate = 50;

    private ParticleSystem _firePartSys;
    private float _fireRatePerHP;

    private new void Awake()
    {
        base.Awake();
        _firePartSys = GetComponentInChildren<ParticleSystem>();

        _fireRatePerHP = _maxFireRate / _maxHP;

        OnDamaged += PlayDamagedSound;
        OnDamaged += SetFireRate;
    }

    private void PlayDamagedSound()
    {
        Links.soundManager.PlayOneshotClip(SoundOneshots[0], GameSettings.soundVolume, Random.Range(_minPitch, _maxPitch));
    }

    private void SetFireRate()
    {
        ParticleSystem.EmissionModule emission = _firePartSys.emission;
        emission.rateOverTime = (_maxHP - _HP) * _fireRatePerHP;
    }

    private void OnDestroy()
    {
        OnDamaged -= PlayDamagedSound;
        OnDamaged -= SetFireRate;
    }
}
