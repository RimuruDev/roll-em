#if (UNITY_STANDALONE_WIN && !UNITY_EDITOR)
#define RIM_DEF_WIN_API
#endif

using UnityEngine;

#if RIM_DEF_WIN_API
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
#endif

public class MinimizeWindow : MonoBehaviour
{
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