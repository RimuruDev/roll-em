using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;


    //-- Раскопано и навалено by foX_Void (FoxLight-Games) --\\
                //-- https://t.me/foxloft_exe --\\
public static class WinAPIFeatures
{
    #region API data
    [DllImport("user32.dll")]
    private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

    [DllImport("user32.dll")]
    private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

    [DllImport("user32.dll", EntryPoint = "GetWindowLong")]
    private static extern int GetWindowLongInternal32(IntPtr hWnd, int nIndex);

    [DllImport("user32.dll", EntryPoint = "GetWindowLongPtr")]
    private static extern IntPtr GetWindowLongInternal64(IntPtr hWnd, int nIndex);

    [DllImport("user32.dll")]
    static extern bool SetLayeredWindowAttributes(IntPtr hWnd, uint crKey, byte bAlpha, uint dwFlags);

    [DllImport("user32.dll", SetLastError = true)]
    static extern int GetWindowLong(IntPtr hWnd, int nIndex);

    [DllImport("user32.dll")]
    static extern IntPtr GetActiveWindow();


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
    
    private static int GetWindowLongInternal(IntPtr hWnd, int nIndex)
    {
        if (IntPtr.Size == 8)
            return (int)(long)GetWindowLongInternal64(hWnd, nIndex);
        else
            return GetWindowLongInternal32(hWnd, nIndex);
    }
    #endregion

    #region Always On Top
    public static void SetWindowAlwaysOnTop(bool state)
    {
        IntPtr hWnd = GetActiveWindow();
        SetWindowPos(hWnd, state ? HWND_TOPMOST : HWND_NOTOPMOST, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE | SWP_SHOWWINDOW);
    }
    #endregion

    #region Borderless Window
    private static int originalStyle;

    public static void SetBorderless(bool state, int screenWidth, int screenHeight)
    {
        IntPtr hWnd = GetActiveWindow();

        if (hWnd == IntPtr.Zero) return;

        if (state)
        {
            originalStyle = GetWindowLongInternal(hWnd, GWL_STYLE);
            int newStyle = originalStyle & ~(WS_BORDER | WS_CAPTION | WS_SYSMENU | WS_THICKFRAME);
            SetWindowLong(hWnd, GWL_STYLE, newStyle);
            SetWindowPos(hWnd, IntPtr.Zero, 0, 0, screenWidth, screenHeight, SWP_FRAMECHANGED | SWP_NOMOVE | SWP_NOSIZE | SWP_NOZORDER);
        }
        else
        {
            SetWindowLong(hWnd, GWL_STYLE, originalStyle);
            SetWindowPos(hWnd, IntPtr.Zero, 0, 0, screenWidth, screenHeight, SWP_FRAMECHANGED | SWP_NOMOVE | SWP_NOSIZE | SWP_NOZORDER);
        }
    }
    #endregion

    #region Chromakey
    public static void SetChromakeyTransparent(Color chromakeyColor, bool state)
    {
        IntPtr hWnd = GetActiveWindow();
        if (hWnd == IntPtr.Zero)
        {
            Debug.LogError("Не удалось получить дескриптор окна!");
        }

        int extendedStyle = GetWindowLong(hWnd, GWL_EXSTYLE);
        SetWindowLong(hWnd, GWL_EXSTYLE, extendedStyle | WS_EX_LAYERED);

        Color32 color32 = chromakeyColor;
        uint colorKey = (uint)(color32.b << 16 | color32.g << 8 | color32.r);

        bool result = false;
        if (state) 
        {
            result = SetLayeredWindowAttributes(hWnd, colorKey, 0, LWA_COLORKEY);
        }
        else
        {
            extendedStyle = GetWindowLong(hWnd, GWL_EXSTYLE);
            SetWindowLong(hWnd, GWL_EXSTYLE, extendedStyle & ~WS_EX_LAYERED);
        }

        if (!result)
        {
            Debug.LogError("Не удалось установить ключ прозрачности!");
        }

        Debug.Log("Хромакей применен!");
    }
    #endregion
}
