#if (UNITY_STANDALONE_WIN && !UNITY_EDITOR)
#define RIM_DEF_WIN_API
#endif

using UnityEngine;
using UnityEngine.EventSystems;

#if RIM_DEF_WIN_API
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
#endif

public class DraggableWindow : MonoBehaviour, IPointerDownHandler
{
#if RIM_DEF_WIN_API
    private const int WM_NCLBUTTONDOWN = 0xA1;
    private const int HT_CAPTION = 0x2;

    private RectTransform dragArea;
#endif

#if RIM_DEF_WIN_API
    [DllImport("user32.dll")]
    private static extern int SendMessage(IntPtr hWnd, int wMsg, int wParam, int lParam);

    [DllImport("user32.dll")]
    private static extern bool ReleaseCapture();

    private IntPtr windowHandle;
#endif

    private void Awake()
    {
#if RIM_DEF_WIN_API
        dragArea = GetComponent<RectTransform>();
        
        var process = Process.GetCurrentProcess();
        windowHandle = process.MainWindowHandle;
#endif
    }

    public void OnPointerDown(PointerEventData eventData)
    {
#if RIM_DEF_WIN_API
        if (dragArea != null && RectTransformUtility.RectangleContainsScreenPoint(dragArea, eventData.position, Camera.main))
        {
            ReleaseCapture();
            SendMessage(windowHandle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
        }
#endif
    }
}