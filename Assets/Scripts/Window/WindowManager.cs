#if (UNITY_STANDALONE_WIN && !UNITY_EDITOR)
#define RIM_DEF_WIN_API
#endif

using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

#if RIM_DEF_WIN_API
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
#endif

public class WindowManager : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private int SceneBuildIndex = 1;
   
    [Header("Warning -> Windows Build Only!")]
    [SerializeField] private Color _chromaKey = Color.white;

    private IEnumerator Start()
    {
#if RIM_DEF_WIN_API
        yield return new WaitForSecondsRealtime(Random.Range(0.3f, 0.6f));

        WinAPIFeatures.SetChromakeyTransparent(_chromaKey, true);

        var screenHeight = Screen.currentResolution.height;
        var windowHeight = (int)(screenHeight * (2f / 3f));

        Screen.SetResolution(windowHeight - 2 - 14, windowHeight - 32 - 7, FullScreenMode.Windowed);

        yield return new WaitForSecondsRealtime(Random.Range(0.3f, 0.6f));

        WinAPIFeatures.SetBorderless(true, 0, 0);
#else
        yield return new WaitForSecondsRealtime(Random.Range(0.3f, 0.6f));
#endif
        yield return new WaitForSecondsRealtime(Random.Range(0.2f, 0.4f));
        SceneManager.LoadScene(SceneBuildIndex);
    }

#if RIM_DEF_WIN_API
    [DllImport("user32.dll")]
    private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

    private static readonly IntPtr HWND_THIS = Process.GetCurrentProcess().MainWindowHandle;
    private const int SW_MINIMIZE = 2;
#endif

    public void MinimizeGameWindow()
    {
#if RIM_DEF_WIN_API
        ShowWindow(HWND_THIS, SW_MINIMIZE);
#endif
    }
}