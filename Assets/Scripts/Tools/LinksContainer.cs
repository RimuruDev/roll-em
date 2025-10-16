using TMPro;
using UnityEngine;

public class LinksContainer : MonoBehaviour
{
    [Header("Global")]
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private Transform _tower;
    [SerializeField] private Transform _mainShield;
    [SerializeField] private Transform _enemiesContainer;
    [SerializeField] private SoundManager _soundManager;
    [SerializeField] private EnemiesSpawner _enemiesSpawner;
    [Header("Prefabs")]
    [SerializeField] private GameObject _coinPrefab;
    [SerializeField] private GameObject _bloodshot;
    [Header("Local")]
    [SerializeField] private RectTransform _coinsIcon;
    [SerializeField] private CostText _costText;
    [SerializeField] private TMP_Text _upgradeInfoText;

    private void Awake()
    {
        Links.gameManager = _gameManager;
        Links.tower = _tower;
        Links.mainShield = _mainShield;
        Links.enemiesContainer = _enemiesContainer;
        Links.soundManager = _soundManager;
        Links.enemiesSpawner = _enemiesSpawner;

        Links.coinPrefab = _coinPrefab;
        Links.bloodshot = _bloodshot;

        Links.coinsIcon = _coinsIcon;
        Links.costText = _costText;
        Links.upgradeInfoText = _upgradeInfoText;
    }
}

public static class Links
{
    public static GameManager gameManager;
    public static Transform tower;
    public static Transform mainShield;
    public static Transform enemiesContainer;
    public static SoundManager soundManager;
    public static EnemiesSpawner enemiesSpawner;

    public static GameObject coinPrefab;
    public static GameObject bloodshot;

    public static RectTransform coinsIcon;
    public static CostText costText;
    public static TMP_Text upgradeInfoText;
}