using System.Collections;
using UnityEngine;

public class Bloodshot : MonoBehaviour
{
    [SerializeField] private Sprite[] Sprites;
    [SerializeField] private float _disolveSpeed;
    [SerializeField] private float _bloodModeDisolveSpeed;
    private SpriteRenderer _sr;

    private void Awake()
    {
        _sr = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        _sr.sprite = Sprites[Random.Range(0, Sprites.Length)];
        _sr.flipX = Random.Range(0, 2) == 0;
        _sr.flipY = Random.Range(0, 2) == 0;
        transform.Rotate(0, 0, Random.Range(0, 4) * 90);

        StartCoroutine(Disolve());
    }

    private IEnumerator Disolve()
    {
        while (_sr.color.a > 0)
        {
            _sr.color -= new Color(0, 0, 0, PlayerData.bloodMode ? _bloodModeDisolveSpeed : _disolveSpeed) * Time.deltaTime;
            yield return null;
        }
        Destroy(gameObject);
    }
}
