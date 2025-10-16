using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveLoadManager : MonoBehaviour
{
    [SerializeField] private bool LoadOnAwake = false;
    [SerializeField] private string savePath;
    private SaveData _gameData = new();

    private void Awake()
    {
        savePath = Application.persistentDataPath + "/save.json";

        if (LoadOnAwake)
        {
            if (File.Exists(savePath))
            {
                LoadGame();
            }
        }
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                PrepareEnemiesData();
                PreparePlayerStats();
                SaveGameData();
            }
            if (Input.GetKeyDown(KeyCode.L))
            {
                LoadGame();
            }
        }
    }

    public void PrepareEnemiesData()
    {
        _gameData.enemiesData = new();

        Vector3 position;
        float rotation;
        float hp;
        int type = -1;

        foreach (Transform enemy in Links.enemiesContainer)
        {
            position = enemy.position;
            rotation = enemy.rotation.eulerAngles.z;
            hp = enemy.GetComponent<Damageable>().hp;
            for (int i = 0; i < Links.enemiesSpawner.enemies.Length; i++)
            {
                Type enemyType = enemy.GetComponent<Damageable>().GetType();
                Type prefabType = Links.enemiesSpawner.enemies[i].GetComponent<Damageable>().GetType();
                if (enemyType == prefabType)
                {
                    type = i;
                    break;
                }
            }

            _gameData.enemiesData.Add(new EnemySLData(position, rotation, hp, type));
        }
    }

    public void PreparePlayerStats()
    {
        _gameData.coinsCount = Wallet.balance;
        _gameData.killsCount = PlayerData.kills;
        _gameData.currentGameSpeed = PlayerData.currentGameSpeed;
    }

    public void SaveGameData()
    {
        string json = JsonUtility.ToJson(_gameData, true);
        File.WriteAllText(savePath, json);
        Debug.Log("Игра сохранена в " + savePath);
    }

    public void LoadGame()
    {
        string json = File.ReadAllText(savePath);
        _gameData = JsonUtility.FromJson<SaveData>(json);

        foreach (EnemySLData data in _gameData.enemiesData)
        {
            Instantiate(Links.enemiesSpawner.enemies[data.typeIndex], data.position, Quaternion.Euler(0, 0, data.rotation), Links.enemiesContainer);
        }

        Wallet.ClearBalance();
        Wallet.AddCoins(_gameData.coinsCount);
        PlayerData.EncountKills(_gameData.killsCount);
        PlayerData.currentGameSpeed = _gameData.currentGameSpeed;
        Time.timeScale = _gameData.currentGameSpeed;
    }
}

[Serializable]
public class SaveData
{
    public List<EnemySLData> enemiesData;
    public int coinsCount;
    public int killsCount;
    public float currentGameSpeed;
}