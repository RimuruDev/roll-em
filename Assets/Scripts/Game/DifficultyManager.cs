using System.Collections;
using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    [SerializeField] private float _maxGameSpeed = 3;
    [SerializeField] private float _growthPerSec = .01f;

    public void StartTimer()
    {
        StartCoroutine(DifficultyTimer());
    }

    private IEnumerator DifficultyTimer()
    {
        while (PlayerData.currentGameSpeed < _maxGameSpeed)
        {
            yield return new WaitForSecondsRealtime(1);
            if (Time.timeScale > 0)
            {
                PlayerData.currentGameSpeed += _growthPerSec;
                Links.gameManager.CancelPauseTime();
            }
        }
        PlayerData.currentGameSpeed = _maxGameSpeed;
        Links.gameManager.CancelPauseTime();
    }
}
