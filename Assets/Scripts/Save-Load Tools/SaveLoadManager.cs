using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
using IEnumerator = System.Collections.IEnumerator;

public class SaveLoadManager : MonoBehaviour
{
    [SerializeField] private bool LoadOnAwake = false;
    [SerializeField] private bool SaveOnQuit = false;
    [SerializeField] private string savePath;
    [SerializeField] private GameObject _loadingErrorPanel;
    private SaveData _gameData = new();

    private Damageable _tower;
    private Damageable _trunk;
    private Rigidbody2D _trunkRB;

    public UnityEvent OnGameLoaded;

    private void Awake()
    {
#if UNITY_EDITOR
        savePath = Application.persistentDataPath + "/saves";
#else
        savePath = Application.dataPath + "/saves";
#endif
        if (!Directory.Exists(savePath)) Directory.CreateDirectory(savePath);

        savePath += "/save.json";

        _tower = Links.tower.GetComponent<Damageable>();
        _trunk = Links.trunk.GetComponent<Damageable>();
        _trunkRB = Links.trunk.GetComponent<Rigidbody2D>();

        if (LoadOnAwake)
        {
            if (File.Exists(savePath))
            {
                try
                {
                    StartCoroutine(LoadGame());
                }
                catch (Exception)
                {
                    _loadingErrorPanel.SetActive(true);
                    throw;
                }
            }
        }

    }

    //TODO: temp code
    /*private void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                SaveGameData();
            }
            if (Input.GetKeyDown(KeyCode.L))
            {
                try
                {
                    StartCoroutine(LoadGame());
                }
                catch (Exception)
                {
                    _loadingErrorPanel.SetActive(true);
                    throw;
                }
            }
        }
    }*/

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

            if (enemy.TryGetComponent(out GoblinAI goblin))
            {
                _gameData.enemiesData.Add(new EnemySLData(position, rotation, hp, type, goblin.remainAttackCooldown));
            }
            else
            {
                _gameData.enemiesData.Add(new EnemySLData(position, rotation, hp, type));
            }
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
        _gameData.trunkPosition = _trunkRB.position;
        _gameData.trunkRotation = _trunkRB.rotation;
        _gameData.trunkSpeed = _trunkRB.angularVelocity;
    }

    public void PrepareGameSettings()
    {
        _gameData.soundVolume = GameSettings.soundVolume;
        _gameData.showLearning = PlayerData.showLearning;
    }

    public void PrepareAbilityBtnData()
    {
        _gameData.chargePercent = Links.abilityBtn.currentChargePercent;
    }

    public void PreparePotionsData()
    {
        _gameData.potions = new();
        _gameData.potionZones = new();
        foreach (Transform item in Links.potionsContainer)
        {
            if (item.TryGetComponent(out Potion potion))
            {
                _gameData.potions.Add(new PotionSLData(item.position, potion.targetPoint));
            }
            else if (item.TryGetComponent(out PotionZone zone))
            {
                _gameData.potionZones.Add(new PotionZoneSLData(item.position, zone.passedTime));
            }
        }
    }

    public void PrepareDroppedCoinsData()
    {
        _gameData.droppedCoins = new();
        foreach (Transform coin in Links.droppedCoinsContainer)
        {
            _gameData.droppedCoins.Add(new DroppedCoinsSLData(coin.position, coin.GetComponent<CoinDrop>().coinsCount));
        }
    }

    public void PrepareUpgradesData()
    {
        _gameData.upgradeLevels = new();
        foreach (UpgradeControlls upgrade in Links.Upgrades)
        {
            _gameData.upgradeLevels.Add(upgrade.nextLevel);
        }
    }

    public void SaveGameData()
    {
        PrepareEnemiesData();
        PreparePlayerStats();
        PrepareLevelObjectsData();
        PrepareGameSettings();
        PrepareAbilityBtnData();
        PreparePotionsData();
        PrepareDroppedCoinsData();
        PrepareUpgradesData();

        string json = JsonUtility.ToJson(_gameData, true);
        string encryptedJson = Encryptor.EncryptToBase64(json, "reallysecretkeyphrase123456789");
        File.WriteAllText(savePath, encryptedJson);
        Debug.Log("Игра сохранена в " + savePath);
    }


    public IEnumerator LoadGame()
    {
        //ready
        string encryptedJson = File.ReadAllText(savePath);
        string json = Encryptor.DecryptFromBase64(encryptedJson, "reallysecretkeyphrase123456789");
        _gameData = JsonUtility.FromJson<SaveData>(json);

        //ready
        foreach (EnemySLData data in _gameData.enemiesData)
        {
            GameObject enemy = Links.enemiesSpawner.SpawnImmediate(data.typeIndex, data.position, data.rotation, data.hp);

            if (enemy.TryGetComponent(out GoblinAI goblin))
            {
                goblin.remainAttackCooldown = data.remainAttackCooldown;
            }
        }

        //ready
        for (int i = 0; i < _gameData.upgradeLevels.Count; i++)
        {
            if (_gameData.upgradeLevels[i] > 0)
            {
                Links.Upgrades[i].Initialize();
                Links.Upgrades[i].nextLevel = _gameData.upgradeLevels[i] - 1;
                Links.Upgrades[i].ApplyNextUpgrade(false);
            }
        }

        //ready
        _tower.SetHP(_gameData.towerHP);
        _trunk.SetHP(_gameData.trunkHP);
        Links.trunkPivot.GetComponent<FixedJoint2D>().connectedAnchor = new Vector2(-1.4f, 0);
        _trunkRB.position = _gameData.trunkPosition;
        _trunkRB.rotation = _gameData.trunkRotation;
        _trunkRB.angularVelocity = _gameData.trunkSpeed * 35; //35 is correction factor to solve calculation problems with FixedJoint2D (well, I guess... just don't touch it, 'kay? It works -_-)

        //ready
        Wallet.ClearBalance();
        Wallet.AddCoins(_gameData.coinsCount);
        PlayerData.EncountKills(_gameData.killsCount);

        //ready
        Links.volumeBar.barValue = _gameData.soundVolume;
        Links.volumeBar.OnValueChanged?.Invoke();
        PlayerData.showLearning = _gameData.showLearning;

        Links.abilityBtn.currentChargePercent = _gameData.chargePercent;

        //ready
        PlayerData.currentGameSpeed = _gameData.currentGameSpeed;
        Time.timeScale = _gameData.currentGameSpeed;

        //ready
        foreach (var data in _gameData.potions)
        {
            Potion potion = Instantiate(Links.potionPrefab, data.position, Quaternion.identity, Links.potionsContainer).GetComponent<Potion>();
            potion.targetPoint = data.targetPoint;
        }
        foreach (var data in _gameData.potionZones)
        {
            PotionZone zone = Instantiate(Links.potionZonePrefab, data.position, Quaternion.identity, Links.potionsContainer).GetComponent<PotionZone>();
            zone.passedTime = data.passedTime;
        }

        foreach (var data in _gameData.droppedCoins)
        {
            CoinDrop coin = Instantiate(Links.coinPrefab, data.position, Quaternion.identity, Links.droppedCoinsContainer).GetComponent<CoinDrop>();
            coin.coinsCount = data.coinsCount;
        }

        yield return null;

        OnGameLoaded?.Invoke();
    }

    public void ClearSavedData()
    {
        File.Delete(savePath);
        Debug.Log("Сохранение по пути: " + savePath + " очищено");
    }

    private void OnApplicationQuit()
    {
        if (SaveOnQuit && !Links.gameOverPanel.activeSelf)
        {
            SaveGameData();
        }
    }
}

[Serializable]
public class SaveData
{
    //Enemies data
    public List<EnemySLData> enemiesData = new();

    //Player stats
    public int coinsCount;
    public int killsCount;
    public float currentGameSpeed;

    //Level objects
    public int towerHP;
    public int trunkHP;
    public float trunkRotation;
    public Vector3 trunkPosition;
    public float trunkSpeed;

    //Game Settings
    public float soundVolume = .25f;
    public bool showLearning;

    //Ability btn
    public float chargePercent;

    //Potions
    public List<PotionSLData> potions = new();
    public List<PotionZoneSLData> potionZones = new();

    //DroppedCoins
    public List<DroppedCoinsSLData> droppedCoins = new();

    //Upgrades
    public List<int> upgradeLevels = new();
}