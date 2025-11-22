#if (UNITY_STANDALONE_WIN && !UNITY_EDITOR)
#define RIM_DEF_WIN_API
#endif

using UnityEngine;

#if RIM_DEF_WIN_API
using System;
using System.Runtime.InteropServices;
#endif

public static class WinAPIFeatures
{
#if RIM_DEF_WIN_API
    [DllImport("user32.dll")]
    private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

    [DllImport("user32.dll")]
    private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

    [DllImport("user32.dll", EntryPoint = "GetWindowLong")]
    private static extern int GetWindowLongInternal32(IntPtr hWnd, int nIndex);

    [DllImport("user32.dll", EntryPoint = "GetWindowLongPtr")]
    private static extern IntPtr GetWindowLongInternal64(IntPtr hWnd, int nIndex);

    [DllImport("user32.dll")]
    private static extern bool SetLayeredWindowAttributes(IntPtr hWnd, uint crKey, byte bAlpha, uint dwFlags);

    [DllImport("user32.dll", SetLastError = true)]
    private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

    [DllImport("user32.dll")]
    private static extern IntPtr GetActiveWindow();

    private static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
    private static readonly IntPtr HWND_NOTOPMOST = new IntPtr(-2);

    private const uint SWP_NOSIZE = 0x0001;
    private const uint SWP_NOMOVE = 0x0002;
    private const uint SWP_SHOWWINDOW = 0x0040;
    private const uint SWP_FRAMECHANGED = 0x0020;
    private const uint SWP_NOZORDER = 0x0004;

    private const int GWL_STYLE = -16;
    private const int WS_BORDER = 0x00800000;
    private const int WS_CAPTION = 0x00C00000;
    private const int WS_SYSMENU = 0x00080000;
    private const int WS_THICKFRAME = 0x00040000;
    private const int GWL_EXSTYLE = -20;
    private const int WS_EX_LAYERED = 0x80000;
    private const int LWA_COLORKEY = 0x1;

    private static int originalStyle;

    private static int GetWindowLongInternal(IntPtr hWnd, int nIndex)
    {
        if (IntPtr.Size == 8)
            return (int)(long)GetWindowLongInternal64(hWnd, nIndex);

        return GetWindowLongInternal32(hWnd, nIndex);
    }

    public static void SetWindowAlwaysOnTop(bool state)
    {
        var hWnd = GetActiveWindow();
        SetWindowPos(hWnd, state ? HWND_TOPMOST : HWND_NOTOPMOST, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE | SWP_SHOWWINDOW);
    }

    public static void SetBorderless(bool state, int screenWidth, int screenHeight)
    {
        var hWnd = GetActiveWindow();
        if (hWnd == IntPtr.Zero) return;

        if (state)
        {
            originalStyle = GetWindowLongInternal(hWnd, GWL_STYLE);
            var newStyle = originalStyle & ~(WS_BORDER | WS_CAPTION | WS_SYSMENU | WS_THICKFRAME);
            SetWindowLong(hWnd, GWL_STYLE, newStyle);
            SetWindowPos(hWnd, IntPtr.Zero, 0, 0, screenWidth, screenHeight, SWP_FRAMECHANGED | SWP_NOMOVE | SWP_NOSIZE | SWP_NOZORDER);
        }
        else
        {
            SetWindowLong(hWnd, GWL_STYLE, originalStyle);
            SetWindowPos(hWnd, IntPtr.Zero, 0, 0, screenWidth, screenHeight, SWP_FRAMECHANGED | SWP_NOMOVE | SWP_NOSIZE | SWP_NOZORDER);
        }
    }

    public static void SetChromakeyTransparent(Color chromakeyColor, bool state)
    {
        var hWnd = GetActiveWindow();
        if (hWnd == IntPtr.Zero) return;

        var extendedStyle = GetWindowLong(hWnd, GWL_EXSTYLE);
        SetWindowLong(hWnd, GWL_EXSTYLE, extendedStyle | WS_EX_LAYERED);

        var c32 = (Color32)chromakeyColor;
        var colorKey = (uint)(c32.b << 16 | c32.g << 8 | c32.r);

        if (state)
        {
            SetLayeredWindowAttributes(hWnd, colorKey, 0, LWA_COLORKEY);
        }
        else
        {
            extendedStyle = GetWindowLong(hWnd, GWL_EXSTYLE);
            SetWindowLong(hWnd, GWL_EXSTYLE, extendedStyle & ~WS_EX_LAYERED);
        }
    }
#else
    public static void SetWindowAlwaysOnTop(bool state) { }

    public static void SetBorderless(bool state, int screenWidth, int screenHeight) { }

    public static void SetChromakeyTransparent(Color chromaKeyColor, bool state) { }
#endif
}
