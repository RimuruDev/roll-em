using System.Collections;
using UnityEngine;

public class EnemiesSpawner : MonoBehaviour
{
    [SerializeField] private Transform[] Enemies;
    [SerializeField] private Transform _enemiesContainer;
    [SerializeField] private Transform _spawnPivot;
    [SerializeField] private GameObject _hpBar;
    [SerializeField] private Transform _barsContainer;
    [SerializeField] private float _spawnDelay = 5;

    void Start()
    {
        StartCoroutine(Spawn());
    }

    private IEnumerator Spawn()
    {
        while (true)
        {
            int randomRotation = Random.Range(0, 180);

            transform.Rotate(0, 0, randomRotation);

            Transform enemy = Instantiate(Enemies[0], _spawnPivot.position, transform.rotation, _enemiesContainer);

            UIFollowGameObject bar = Instantiate(_hpBar, enemy.position, Quaternion.identity, _barsContainer).GetComponent<UIFollowGameObject>();
            bar.targetObject = enemy;
            bar.offset = enemy.localScale.y / 2;

            HPBar enemyBar = bar.GetComponent<HPBar>();
            enemyBar.checkingObject = enemy.GetComponent<Damageable>();
            enemyBar.Initialize();

            yield return new WaitForSeconds(_spawnDelay);
        }
    }
}
