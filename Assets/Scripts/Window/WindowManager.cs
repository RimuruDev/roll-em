using System;
using System.Collections;
using System.Diagnostics;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.SceneManagement;

using Random = UnityEngine.Random;

public class WindowManager : MonoBehaviour
{
    [SerializeField] private Color _chromaKey = Color.white;

    IEnumerator Start()
    {
#if !UNITY_EDITOR
        yield return new WaitForSecondsRealtime(Random.Range(0.3f, 0.6f));

        WinAPIFeatures.SetChromakeyTransparent(_chromaKey, true);

        int screenHeight = Screen.currentResolution.height;
        int windowHeight = (int)(screenHeight * (2f / 3f));

        Screen.SetResolution(windowHeight - 2 - 14, windowHeight - 32 - 7, FullScreenMode.Windowed);

        yield return new WaitForSecondsRealtime(Random.Range(0.3f, 0.6f));

        WinAPIFeatures.SetBorderless(true, 0, 0);
#endif

        yield return new WaitForSecondsRealtime(Random.Range(0.2f, 0.4f));

        SceneManager.LoadScene(1);
    }

    [DllImport("user32.dll")]
    private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

    private static readonly IntPtr HWND_THIS = Process.GetCurrentProcess().MainWindowHandle;
    private const int SW_MINIMIZE = 2;

    public void MinimizeGameWindow()
    {
        ShowWindow(HWND_THIS, SW_MINIMIZE);
    }
}
