using UnityEngine;

public class BomberAI : GoblinAI
{
    [Header("Bomber settings")]
    [SerializeField] private float _detonationRadius;
    [SerializeField] private int _detonationEmissionRate;
    [SerializeField] private ParticleSystem _detonationPartSys;

    protected override void StartAttack()
    {
        Detonate();
    }

    private void Detonate()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, _detonationRadius);

        foreach (Collider2D collider in colliders)
        {
            if (collider.TryGetComponent(out Damageable damageable) && damageable != this)
            {
                int distanceDamage = Mathf.RoundToInt(_damage * (1 - (Vector2.Distance(transform.position, collider.transform.position) / _detonationRadius)));
                damageable.TakeDamage(distanceDamage);

                _detonationPartSys.transform.SetParent(null);
                _detonationPartSys.GetComponent<SelfDestroyParticleSystem>().SetSelfDestroyOn();
                _detonationPartSys.Emit(_detonationEmissionRate);

                Links.soundManager.PlayOneshotClip(SoundOneshots[2], GameSettings.soundVolume, 1, true, transform.position);

                Death(false, false);
            }
        }
    }
}
