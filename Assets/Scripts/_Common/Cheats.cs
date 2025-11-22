using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Common
{
    [DisallowMultipleComponent]
    public sealed class Cheats : MonoBehaviour
    {
        private bool isResetProgress;

        private void Start()
        {
            if (GetComponent<ImmortalGameObject>() == null)
                return;

            DontDestroyOnLoadRepository.Register(gameObject);
        }

        private void Update()
        {
            if (isResetProgress)
                return;

            if (!Input.GetKey(KeyCode.Alpha9) || !Input.GetKeyDown(KeyCode.Alpha0))
                return;

            isResetProgress = true;
            ResetProgress();
            RestartGame();
        }

        private static void ResetProgress()
        {
            AppLogWrapper.Send("<color=green>Reset Progress -> Apply Cheats</color>");
            RollEmProgressTools.ClearProgress();
        }

        private static void RestartGame()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            Application.OpenURL(Application.absoluteURL);
#else
            DontDestroyOnLoadRepository.DisposeAndDestroy();
            SceneManager.LoadScene(AppConstants.BootSceneName);
#endif
        }
    }
}