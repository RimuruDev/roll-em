using System.Collections;
using UnityEngine;

public class PotionZone : MonoBehaviour
{
    [SerializeField] private int _emissionRate = 20;
    [SerializeField] private float _damagePerSec = 1;
    [SerializeField] private int _ticksPerSec = 1;
    [SerializeField] private float _radius = 1;
    [SerializeField] private float _lifeTime = 10;

    private ParticleSystem _ps;
    private ParticleSystem.ShapeModule _shape;
    private ParticleSystem.EmissionModule _emission;
    private float _time;

    private void Awake()
    {
        _ps = GetComponent<ParticleSystem>();
        _shape = _ps.shape;
        _emission = _ps.emission;
    }

    private void Start()
    {
        _shape.radius = _radius;
        _emission.rateOverTime = _emissionRate * _radius;

        StartCoroutine(CauseDamage());
    }

    private IEnumerator CauseDamage()
    {
        Collider2D[] colliders;

        while (true)
        {
            _emission.rateOverTime = (_emissionRate * _radius) * (1 - _time / _lifeTime);

            colliders = Physics2D.OverlapCircleAll(transform.position, _radius);

            foreach (Collider2D collider in colliders)
            {
                if (collider.TryGetComponent(out EnemyAI enemy))
                {
                    enemy.GetComponent<Damageable>().TakeDamage(_damagePerSec / _ticksPerSec);
                }
            }


            yield return new WaitForSeconds(1f / _ticksPerSec);
            _time += 1f / _ticksPerSec;
        }
    }
}
