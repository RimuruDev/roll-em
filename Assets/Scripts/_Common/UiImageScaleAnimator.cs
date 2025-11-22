using System;
using UnityEngine;
using System.Collections;
using UnityEngine.Scripting;

[Preserve]
[SelectionBase]
[DisallowMultipleComponent]
public sealed class UiImageScaleAnimator : MonoBehaviour
{
    [SerializeField] private RectTransform _target;
    [SerializeField] private float _duration = 0.25f;
    [SerializeField] private float _startScale = 0.9f;
    [SerializeField] private float _endScale = 1f;
    [SerializeField] private bool _useUnscaledTime = true;
    [SerializeField] private bool _playOnEnable;

    private Coroutine _routine;
    private Vector3 _baseScale;

    private void OnValidate()
    {
        if (_target == null)
            _target = GetComponent<RectTransform>();
    }

    private void Awake()
    {
        if (_target == null)
            _target = GetComponent<RectTransform>();

        if (_target != null)
            _baseScale = _target.localScale;
    }

    private void OnEnable()
    {
        if (_playOnEnable)
            Play();
    }

    private void OnDisable()
    {
        if (_routine != null)
            StopCoroutine(_routine);

        if (_target != null)
            _target.localScale = _baseScale;
    }

    public void Play()
    {
        if (_target == null)
            return;

        if (_routine != null)
            StopCoroutine(_routine);

        _baseScale = _target.localScale;

        _routine = StartCoroutine(PlayRoutine());
    }


    private IEnumerator PlayRoutine()
    {
        float time = 0f;

        while (time < _duration)
        {
            float deltaTime = _useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
            time += deltaTime;

            float t = _duration > 0f ? time / _duration : 1f;
            if (t > 1f)
                t = 1f;

            float scaleFactor = Mathf.Lerp(_startScale, _endScale, t);
            _target.localScale = _baseScale * scaleFactor;

            yield return null;
        }

        _target.localScale = _baseScale * _endScale;
        _routine = null;
    }
}