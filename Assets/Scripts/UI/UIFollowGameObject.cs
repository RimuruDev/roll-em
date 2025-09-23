using UnityEngine;
using UnityEngine.UI;

public class UIFollowGameObject : MonoBehaviour
{
    public Transform targetObject { set => _targetObject = value; }
    public float offset { set => _offset = value; }


    private Transform _targetObject; // GameObject за которым нужно следовать
    private float _offset;         // Смещение UI элемента относительно GameObject (необязательно)

    private RectTransform _rectTransform;
    private CanvasScaler _canvasScaler;
    private Canvas _canvas;

    void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
        _canvas = GetComponentInParent<Canvas>();
        _canvasScaler = _canvas.GetComponent<CanvasScaler>();
    }

    void Update()
    {
        if (_targetObject != null)
        {
            // Преобразуем позицию игрового объекта из мирового пространства в пространство экрана
            Vector2 viewportPosition = Camera.main.WorldToViewportPoint(_targetObject.position + Vector3.up * _offset);

            // Преобразуем позицию viewport в позицию внутри Canvas
            Vector2 anchoredPosition = new Vector2(
                ((viewportPosition.x * _canvasScaler.referenceResolution.x) - (_canvasScaler.referenceResolution.x * 0.5f)),
                ((viewportPosition.y * _canvasScaler.referenceResolution.y) - (_canvasScaler.referenceResolution.y * 0.5f))
            );

            _rectTransform.anchoredPosition = anchoredPosition;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
