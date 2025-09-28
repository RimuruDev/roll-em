using UnityEngine;

public class PausePanelControlls : MonoBehaviour
{
    public void RestartBtn_Click()
    {
        Links.gameManager.CancelPauseTime();
        Links.gameManager.ReloadScene();
    }
}
