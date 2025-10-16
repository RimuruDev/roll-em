using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SegmentedBar : MonoBehaviour
{
    public int activeSegmentsCount => _activeSegmentsCount;
    public int segmentsCount
    {
        get => _segmentsCount;
        set
        {
            if (value > 0)
            {
                _segmentsCount = value;
                UpdateSegments();
            }
        }
    }
    public float barValue
    {
        get => (float)_activeSegmentsCount / Segments.Count;
        set
        {
            value = Mathf.Clamp01(value);

            _activeSegmentsCount = Mathf.RoundToInt(value * Segments.Count);

            SetSegmentsActiveness();
        }
    }


    [SerializeField] private int _segmentsCount;
    [SerializeField] private int _activeSegmentsCount;
    [SerializeField] private GameObject _segmentsPrefab;
    [SerializeField] private Sprite _leftSegmentSprite;
    [SerializeField] private Sprite _middleSegmentSprite;
    [SerializeField] private Sprite _rightSegmentSprite;
    [SerializeField] private Sprite _monoSegmentSprite;
    [SerializeField] private Color _enabledColor = Color.white;
    [SerializeField] private Color _disabledColor = Color.gray9;
    [SerializeField] private bool _inversible = false;

    private List<Image> Segments = new List<Image>();

    public UnityEvent OnValueChanged;


    private void Awake()
    {
        FillSegmentsList();
        UpdateSegments();
    }

    private void OnValidate()
    {
        if (Segments.Count != transform.childCount)
        {
            FillSegmentsList();
        }

        if (_inversible)
        {
            if (_activeSegmentsCount < 0)
            {
                _activeSegmentsCount = Segments.Count;
            }
            else if (_activeSegmentsCount > Segments.Count)
            {
                _activeSegmentsCount = 0;
            }
        }
        else
        {
            _activeSegmentsCount = Mathf.Clamp(_activeSegmentsCount, 0, Segments.Count);
        }

        SetSegmentsActiveness();
    }

    private void UpdateSegments()
    {

        foreach (Transform segment in transform)
        {
            Destroy(segment.gameObject);
        }

        Segments = new();

        for (int i = 0; i < _segmentsCount; i++)
        {
            GameObject segment = Instantiate(_segmentsPrefab, transform);
            segment.name = $"segment_{i}";
            Segments.Add(segment.GetComponent<Image>());
        }

        for (int i = 0; i < Segments.Count; i++)
        {
            if (_segmentsCount == 1)
            {
                Segments[i].sprite = _monoSegmentSprite;
            }
            else
            {
                if (i == 0)
                {
                    Segments[i].sprite = _leftSegmentSprite;
                }
                else if (i == Segments.Count - 1)
                {
                    Segments[i].sprite = _rightSegmentSprite;
                }
                else
                {
                    Segments[i].sprite = _middleSegmentSprite;
                }
            }
        }

        SetSegmentsActiveness();
    }

    public void EnableNewSegments(int count)
    {
        if (_activeSegmentsCount + count <= Segments.Count)
        {
            _activeSegmentsCount += count;
        }
        else if (_inversible)
        {
            _activeSegmentsCount = 0;
        }
        SetSegmentsActiveness();

        OnValueChanged?.Invoke();
    }

    public void DisableNewSegments(int count)
    {
        if (_activeSegmentsCount - count >= 0)
        {
            _activeSegmentsCount -= count;
        }
        else if (_inversible)
        {
            _activeSegmentsCount = Segments.Count;
        }
        SetSegmentsActiveness();

        OnValueChanged?.Invoke();
    }

    public void SetEnabledCount(int count)
    {
        if (count <= Segments.Count)
        {
            _activeSegmentsCount = count;
        }
        else if (_inversible)
        {
            _activeSegmentsCount = 0;
        }
        SetSegmentsActiveness();

        OnValueChanged?.Invoke();
    }

    private void FillSegmentsList()
    {
        Segments = new List<Image>();
        foreach (Transform child in transform)
        {
            Segments.Add(child.GetComponent<Image>());
        }
    }

    private void SetSegmentsActiveness()
    {
        for (int i = 0; i < Segments.Count; i++)
        {
            if (i < _activeSegmentsCount)
            {
                Segments[i].color = _enabledColor;
            }
            else
            {
                Segments[i].color = _disabledColor;
            }
        }
    }
}
