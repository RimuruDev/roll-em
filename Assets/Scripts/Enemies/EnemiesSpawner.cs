using System.Collections;
using UnityEngine;

public class EnemiesSpawner : MonoBehaviour
{
    public Transform[] enemies => Enemies;

    [SerializeField] private Transform[] Enemies;
    [SerializeField] private int[] EnemiesWeights;
    [SerializeField] private Transform _enemiesContainer;
    [SerializeField] private Transform _spawnPivot;
    [SerializeField] private GameObject _hpBar;
    [SerializeField] private Transform _barsContainer;
    [SerializeField] private float _spawnDelay = 5;

    private int _totalWeights;

    private void Awake()
    {
        foreach (int weight in EnemiesWeights)
        {
            _totalWeights += weight;
        }
    }

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

            int index = 0;
            int randomWeight = Random.Range(1, _totalWeights + 1);
            int checkedWeights = 0;
            for (int i = 0; i < EnemiesWeights.Length; i++)
            {
                checkedWeights += EnemiesWeights[i];
                if (randomWeight <= checkedWeights)
                {
                    index = i;
                    break;
                }
            }

            SpawnImmediate(index, _spawnPivot.position, transform.eulerAngles.z);

            yield return new WaitForSeconds(_spawnDelay);
        }
    }

    public void SpawnImmediate(int enemyIndex, Vector3 position, float zEuler, float hp = -1)
    {

        Transform enemy = Instantiate(Enemies[enemyIndex], position, Quaternion.Euler(0, 0, zEuler), _enemiesContainer);

        if (hp > 0) enemy.GetComponent<Damageable>().SetHP(hp);

        UIFollowGameObject bar = Instantiate(_hpBar, enemy.position, Quaternion.identity, _barsContainer).GetComponent<UIFollowGameObject>();
        bar.targetObject = enemy;
        bar.offset = enemy.localScale.y / 2;

        HPBar enemyBar = bar.GetComponent<HPBar>();
        enemyBar.checkingObject = enemy.GetComponent<Damageable>();
        enemyBar.Initialize();
    }
}
