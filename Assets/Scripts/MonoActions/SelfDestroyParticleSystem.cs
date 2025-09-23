using System.Collections;
using UnityEngine;

public class SelfDestroyParticleSystem : MonoBehaviour
{
    [SerializeField] private bool _autoDestroy = false;
    private ParticleSystem _partSys;

    private void Awake()
    {
        _partSys = GetComponent<ParticleSystem>();
    }

    private void Start()
    {
        if (_autoDestroy)
        {
            StartCoroutine(SelfDestroy());
        }
    }

    public void SetSelfDestroyOn()
    {
        StartCoroutine(SelfDestroy());
    }

    private IEnumerator SelfDestroy()
    {
        yield return new WaitForSeconds(1);
        while (_partSys.particleCount > 0)
        {
            yield return null;
        }
        Destroy(gameObject);
    }
}
