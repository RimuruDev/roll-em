using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverPanelControlls : PausePanelControlls
{
    [SerializeField] private TMP_Text _killsText;

    private void OnEnable()
    {
        _killsText.text = PlayerData.kills.ToString();
    }
}
