using UnityEngine;

public class Potion : MonoBehaviour
{
    public Vector3 targetPoint
    {
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

    void SpawnPotionZone()
    {
        Instantiate(_potionZonePrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        _movement.OnReachedTarget -= SpawnPotionZone;
    }
}
