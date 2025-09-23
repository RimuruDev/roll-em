using System.Collections;
using UnityEngine;

public class CoinDrop : MonoBehaviour
{
    public int coinsCount { set => _coinsCount = value; }

    [SerializeField] private float _moveSpeed = 1;
    [SerializeField] private float _minDelta = .1f;
    private int _coinsCount;

    void Start()
    {
        StartCoroutine(MoveToIcon());
    }

    private IEnumerator MoveToIcon()
    {
        Vector2 coinIconPos = Links.coinsIcon.TransformToWorldPoint();

        while (Vector2.Distance(transform.position, coinIconPos) > _minDelta)
        {
            transform.position = Vector3.Slerp(transform.position, coinIconPos, _moveSpeed * Time.deltaTime);
            yield return null;
        }

        Wallet.AddCoins(_coinsCount);
        Destroy(gameObject);
    }
}
