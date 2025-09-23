using System.Diagnostics;
using System.Runtime.InteropServices;
using System;
using UnityEngine;

public class MinimizeWindow : MonoBehaviour
{
    [DllImport("user32.dll")]
    private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

    private static readonly IntPtr HWND_THIS = Process.GetCurrentProcess().MainWindowHandle;
    private const int SW_MINIMIZE = 2;

    public void MinimizeGameWindow()
    {
        ShowWindow(HWND_THIS, SW_MINIMIZE);
    }
}
