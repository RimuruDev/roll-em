using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverPanelControlls : MonoBehaviour
{
    [SerializeField] private TMP_Text _killsText;

    public void RestartBtn_Click()
    {
        Links.gameManager.CancelPauseTime();
        Links.gameManager.ReloadScene();
    }

    private void OnEnable()
    {
        _killsText.text = PlayerData.kills.ToString();
    }
}
