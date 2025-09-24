using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private float _lastTimescale;

    private void Awake()
    {
        Wallet.ClearBalance();
        PlayerData.ClearKills();
        PlayerData.ResetGameSpeed();
        //PlayerData.BloodModeOff();
    }

    public void ReloadScene() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    public void ApplicationQuit() => Application.Quit();

    public void PauseTime() => Time.timeScale = 0;

    public void PauseTimeAndSaveCurrentTimescale()
    {
        _lastTimescale = Time.timeScale;
        Time.timeScale = 0;
    }

    public void CancelPauseTime() => Time.timeScale = PlayerData.currentGameSpeed;

    public void CancelPauseAndRestoreSavedTimescale()
    {
        Time.timeScale = _lastTimescale;
    }
}
