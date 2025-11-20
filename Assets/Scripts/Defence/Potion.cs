using UnityEngine;

public class Potion : MBWithAudio
{
    public Vector3 targetPoint
    {
        get => _target;
        set
        {
            _target = value;
            Initialize();
        }
    }

    [SerializeField] private GameObject _potionZonePrefab;

    private Vector3 _target;
    private PseudoBallisticMovement _movement;


    private void Initialize()
    {
        _movement = GetComponent<PseudoBallisticMovement>();
        _movement.targetPoint = _target;
        _movement.OnReachedTarget += SpawnPotionZone;
    }

    private void SpawnPotionZone()
    {
        Instantiate(_potionZonePrefab, transform.position, Quaternion.identity, transform.parent);
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        Links.soundManager.PlayOneshotClip(SoundOneshots[0], GameSettings.soundVolume, Random.Range(_minPitch, _maxPitch), true, transform.position);
        _movement.OnReachedTarget -= SpawnPotionZone;
    }
}
