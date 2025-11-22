using UnityEngine;

public class UIFollowGameObject : MonoBehaviour
{
    public Transform targetObject { set => _targetObject = value; }
    public float offset { set => _offset = value; }

    private Transform _targetObject;
    private float _offset;

    private RectTransform _rectTransform;
    private Canvas _canvas;
    private Camera _camera;

    private void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
        _canvas = GetComponentInParent<Canvas>();
        _camera = Camera.main;
    }

    private void LateUpdate()
    {
        if (_targetObject == null)
        {
            Destroy(gameObject);
            return;
        }

        if (_rectTransform == null)
            _rectTransform = GetComponent<RectTransform>();

        if (_canvas == null)
            _canvas = GetComponentInParent<Canvas>();

        if (_camera == null)
            _camera = Camera.main;

        if (_canvas == null || _camera == null)
            return;

        RectTransform canvasRect = _canvas.transform as RectTransform;
        if (canvasRect == null)
            return;

        Vector3 worldPosition = _targetObject.position + Vector3.up * _offset;

        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(_camera, worldPosition);

        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect,
            screenPoint,
            _canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : _camera,
            out localPoint
        );

        _rectTransform.anchoredPosition = localPoint;
    }
}