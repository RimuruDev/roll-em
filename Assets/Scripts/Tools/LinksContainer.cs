using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class LinksContainer : MonoBehaviour
{
    [Header("Global")]
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private Transform _tower;
    [SerializeField] private Transform _trunk;
    [SerializeField] private Transform _trunkPivot;
    [SerializeField] private Transform _mainShield;
    [SerializeField] private Transform _enemiesContainer;
    [SerializeField] private Transform _potionsContainer;
    [SerializeField] private Transform _droppedCoinsContainer;
    [SerializeField] private SoundManager _soundManager;
    [SerializeField] private EnemiesSpawner _enemiesSpawner;
    [SerializeField] private SegmentedBar _volumeBar;
    [SerializeField] private RechargeableAbilityButton _abilityBtn;
    [SerializeField] private SaveLoadManager _saveLoadManager;
    [SerializeField] private GameObject _gameOverPanel;
    [Header("Prefabs")]
    [SerializeField] private GameObject _coinPrefab;
    [SerializeField] private GameObject _bloodshot;
    [SerializeField] private GameObject _potionPrefab;
    [SerializeField] private GameObject _potionZonePrefab;
    [Header("Local")]
    [SerializeField] private RectTransform _coinsIcon;
    [SerializeField] private CostText _costText;
    [SerializeField] private TMP_Text _upgradeInfoText;
    [Header("Collections")]
    [SerializeField] private UpgradeControlls[] Upgrades;

    private void Awake()
    {
        Links.gameManager = _gameManager;
        Links.tower = _tower;
        Links.trunk = _trunk;
        Links.trunkPivot = _trunkPivot;
        Links.mainShield = _mainShield;
        Links.enemiesContainer = _enemiesContainer;
        Links.potionsContainer = _potionsContainer;
        Links.droppedCoinsContainer = _droppedCoinsContainer;
        Links.soundManager = _soundManager;
        Links.enemiesSpawner = _enemiesSpawner;
        Links.volumeBar = _volumeBar;
        Links.abilityBtn = _abilityBtn;
        Links.saveLoadManager = _saveLoadManager;
        Links.gameOverPanel = _gameOverPanel;

        Links.coinPrefab = _coinPrefab;
        Links.bloodshot = _bloodshot;
        Links.potionPrefab = _potionPrefab;
        Links.potionZonePrefab = _potionZonePrefab;

        Links.coinsIcon = _coinsIcon;
        Links.costText = _costText;
        Links.upgradeInfoText = _upgradeInfoText;
        Links.Upgrades = Upgrades;
    }
}

public static class Links
{
    public static GameManager gameManager;
    public static Transform tower;
    public static Transform trunk;
    public static Transform trunkPivot;
    public static Transform mainShield;
    public static Transform enemiesContainer;
    public static Transform potionsContainer;
    public static Transform droppedCoinsContainer;
    public static SoundManager soundManager;
    public static EnemiesSpawner enemiesSpawner;
    public static SegmentedBar volumeBar;
    public static RechargeableAbilityButton abilityBtn;
    public static SaveLoadManager saveLoadManager;
    public static GameObject gameOverPanel;

    public static GameObject coinPrefab;
    public static GameObject bloodshot;
    public static GameObject potionPrefab;
    public static GameObject potionZonePrefab;

    public static RectTransform coinsIcon;
    public static CostText costText;
    public static TMP_Text upgradeInfoText;

    public static UpgradeControlls[] Upgrades;
}