using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Runtime.InteropServices;

public class DraggableWindow : MonoBehaviour, IPointerDownHandler
{
    [DllImport("user32.dll")]
    public static extern int SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);
    [DllImport("user32.dll")]
    public static extern bool ReleaseCapture();

    private const int WM_NCLBUTTONDOWN = 0xA1;
    private const int HT_CAPTION = 0x2;

    private RectTransform dragArea; // UI элемент, за который перетаскиваем

    private System.IntPtr windowHandle; // Хэндл окна

    private void Awake()
    {
        dragArea = GetComponent<RectTransform>();
    }

    void Start()
    {
        // Получаем хэндл окна в Start, чтобы не делать это каждый кадр
        windowHandle = GetHandle();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (dragArea != null && RectTransformUtility.RectangleContainsScreenPoint(dragArea, eventData.position, Camera.main))
        {
            ReleaseCapture();
            SendMessage(windowHandle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
        }
    }

    // Получение хэндла окна
    private System.IntPtr GetHandle()
    {
        System.Diagnostics.Process p = System.Diagnostics.Process.GetCurrentProcess();
        return p.MainWindowHandle;
    }
}
