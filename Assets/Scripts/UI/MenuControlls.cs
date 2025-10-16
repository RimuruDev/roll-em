using TMPro;
using UnityEngine;

public class MenuControlls : MonoBehaviour
{
    [SerializeField] private TMP_Text _coinsText;
    [SerializeField] private TMP_Text _killsText;
    [SerializeField] private GameObject _gameOverPanel;

    private void Awake()
    {
        Wallet.OnBalanceChanged += UpdateCoinsText;
        //EnemyAI.OnAnyDeath += UpdateKillsText;
        PlayerData.OnKillsCountChanged += UpdateKillsText;
        Links.tower.GetComponent<Damageable>().OnBroken += ShowGameOverPanel;
        Links.mainShield.GetComponent<Damageable>().OnBroken += ShowGameOverPanel;
    }

    private void UpdateCoinsText()
    {
        _coinsText.text = $"x{Wallet.balance}";
    }

    private void UpdateKillsText()
    {
        _killsText.text = $"{PlayerData.kills}x";
    }

    private void ShowGameOverPanel()
    {
        Links.gameManager.PauseTime();
        _gameOverPanel.SetActive(true);
    }

    private void OnDestroy()
    {
        Wallet.OnBalanceChanged -= UpdateCoinsText;
        //EnemyAI.OnAnyDeath -= UpdateKillsText;
        PlayerData.OnKillsCountChanged -= UpdateKillsText;
        //Links.tower.GetComponent<Damageable>().OnBroken -= ShowGameOverPanel;
    }
}
