using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveLoadManager : MonoBehaviour
{
    [SerializeField] private bool LoadOnAwake = false;
    [SerializeField] private string savePath;
    private SaveData _gameData = new();
    private Damageable _tower;
    private Damageable _trunk;

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

        _tower = Links.tower.GetComponent<Damageable>();
        _trunk = Links.trunk.GetComponent<Damageable>();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                PrepareEnemiesData();
                PreparePlayerStats();
                PrepareLevelObjectsData();
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

    public void PrepareLevelObjectsData()
    {
        _gameData.towerHP = (int)_tower.hp;
        _gameData.trunkHP = (int)_trunk.hp;
        _gameData.trunkRotation = Links.trunkPivot.GetComponent<Rigidbody2D>().rotation;
    }

    public void SaveGameData()
    {
        string json = JsonUtility.ToJson(_gameData, true);
        File.WriteAllText(savePath, json);
        Debug.Log("Игра сохранена в " + savePath);
    }

    public void LoadGame()
    {
        //ready
        string json = File.ReadAllText(savePath);
        _gameData = JsonUtility.FromJson<SaveData>(json);

        //ready
        foreach (EnemySLData data in _gameData.enemiesData)
        {
            Links.enemiesSpawner.SpawnImmediate(data.typeIndex, data.position, data.rotation, data.hp);
        }

        //ready
        Wallet.ClearBalance();
        Wallet.AddCoins(_gameData.coinsCount);
        PlayerData.EncountKills(_gameData.killsCount);

        _tower.SetHP(_gameData.towerHP);
        _trunk.SetHP(_gameData.trunkHP);

        Links.trunkPivot.GetComponent<Rigidbody2D>().rotation = _gameData.trunkRotation;

        //test or rework
        PlayerData.currentGameSpeed = _gameData.currentGameSpeed;
        Time.timeScale = _gameData.currentGameSpeed;
    }
}

[Serializable]
public class SaveData
{
    //Enemies data
    public List<EnemySLData> enemiesData;

    //Player stats
    public int coinsCount;
    public int killsCount;
    public float currentGameSpeed;

    //Level objects
    public int towerHP;
    public int trunkHP;
    public float trunkRotation;
    public float trunkSpeed;
}