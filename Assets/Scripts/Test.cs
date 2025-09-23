using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] private AudioClip[] Clips;

    private SpriteRenderer _sr;
    private Sprite _last;

    private void Awake()
    {
        _sr = GetComponent<SpriteRenderer>();
        _last = _sr.sprite;
    }

    void Start()
    {
        
    }

    void Update()
    {
        if (_sr.sprite != _last)
        {
            AudioSource.PlayClipAtPoint(Clips[Random.Range(0, Clips.Length)], transform.position);
        }
        _last = _sr.sprite;
    }
}
