//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;
//using UnityEngine.Events;

//#if UNITY_EDITOR
//using UnityEditor;
//#endif

//[ExecuteAlways]
//public class SegmentedBar2 : MonoBehaviour
//{
//    [SerializeField] private int _segmentsCount = 1;
//    [SerializeField] private int _activeSegmentsCount = 0;
//    [SerializeField] private GameObject _segmentsPrefab;
//    [SerializeField] private Sprite _leftSegmentSprite;
//    [SerializeField] private Sprite _middleSegmentSprite;
//    [SerializeField] private Sprite _rightSegmentSprite;
//    [SerializeField] private Sprite _monoSegmentSprite;
//    [SerializeField] private Color _enabledColor = Color.white;
//    [SerializeField] private Color _disabledColor = Color.gray;
//    [SerializeField] private bool _inversible = false;

//    private readonly List<Image> Segments = new List<Image>();

//    public UnityEvent OnValueChanged;

//    // Для контроля изменений в инспекторе
//    private int _prevSegmentsCount = -1;
//    private int _prevActiveSegmentsCount = -1;

//    // Очередь объектов для удаления (в редакторе удаляем отложенно)
//#if UNITY_EDITOR
//    private readonly List<GameObject> _toDestroy = new List<GameObject>();
//    private bool _destroyScheduled = false;
//#endif

//    public int SegmentsCount
//    {
//        get => _segmentsCount;
//        set
//        {
//            int newValue = Mathf.Max(1, value);
//            if (newValue == _segmentsCount) return;
//            _segmentsCount = newValue;
//            RebuildSegments();
//#if UNITY_EDITOR
//            EditorUtility.SetDirty(this);
//#endif
//        }
//    }

//    public int ActiveSegmentsCount
//    {
//        get => _activeSegmentsCount;
//        set
//        {
//            int newValue = Mathf.Clamp(value, 0, Mathf.Max(0, _segmentsCount));
//            if (newValue == _activeSegmentsCount) return;
//            _activeSegmentsCount = newValue;
//            UpdateActiveVisuals();
//            OnValueChanged?.Invoke();
//#if UNITY_EDITOR
//            EditorUtility.SetDirty(this);
//#endif
//        }
//    }

//    public float ActiveFraction
//    {
//        get
//        {
//            if (_segmentsCount <= 0) return 0f;
//            return (float)_activeSegmentsCount / _segmentsCount;
//        }
//    }

//    private void Reset()
//    {
//        _segmentsCount = Mathf.Max(1, _segmentsCount);
//        _activeSegmentsCount = Mathf.Clamp(_activeSegmentsCount, 0, _segmentsCount);
//    }

//    private void OnEnable()
//    {
//        RebuildSegments();
//        UpdateActiveVisuals();
//    }

//    private void OnValidate()
//    {
//        // Защита значений, мгновенное применение ограничений
//        _segmentsCount = Mathf.Max(1, _segmentsCount);
//        _activeSegmentsCount = Mathf.Clamp(_activeSegmentsCount, 0, _segmentsCount);

//        // Перестраиваем сегменты, только если было изменение
//        if (_prevSegmentsCount != _segmentsCount)
//        {
//            RebuildSegments();
//        }

//        if (_prevActiveSegmentsCount != _activeSegmentsCount)
//        {
//            UpdateActiveVisuals();
//            OnValueChanged?.Invoke();
//        }

//        _prevSegmentsCount = _segmentsCount;
//        _prevActiveSegmentsCount = _activeSegmentsCount;
//    }

//    private void RebuildSegments()
//    {
//        // Удаляем лишние: НЕ вызывать DestroyImmediate прямо в OnValidate — откладываем в редакторе
//        while (Segments.Count > _segmentsCount)
//        {
//            int lastIndex = Segments.Count - 1;
//            Image img = Segments[lastIndex];
//            GameObject go = img != null ? img.gameObject : null;

//            // Убираем из списка сразу, а удаление планируем (в редакторе отложенно)
//            Segments.RemoveAt(lastIndex);

//#if UNITY_EDITOR
//            if (go != null)
//            {
//                // добавляем в очередь удаляемых объектов
//                _toDestroy.Add(go);
//                ScheduleDestroyIfNeeded();
//            }
//#else
//            if (go != null)
//            {
//                // в рантайме безопасно уничтожать сразу
//                Destroy(go);
//            }
//#endif
//        }

//        // Добавляем недостающие сегменты
//        while (Segments.Count < _segmentsCount)
//        {
//            if (_segmentsPrefab == null)
//            {
//                Debug.LogError($"SegmentedBar2: _segmentsPrefab is null on '{name}'. Cannot create segments.");
//                break;
//            }

//            GameObject go = null;
//#if UNITY_EDITOR
//            if (!Application.isPlaying)
//            {
//                // В редакторе сохраняем связь с prefab
//                var instantiated = PrefabUtility.InstantiatePrefab(_segmentsPrefab, this.gameObject.scene) as GameObject;
//                go = instantiated ?? Object.Instantiate(_segmentsPrefab);
//                // Установим родителя и регистрируем как изменение сцены (Undo)
//                if (go != null)
//                {
//                    Undo.RegisterCreatedObjectUndo(go, "Create segmented bar segment");
//                }
//            }
//            else
//            {
//                go = Instantiate(_segmentsPrefab);
//            }
//#else
//            go = Instantiate(_segmentsPrefab);
//#endif
//            if (go != null)
//            {
//                go.name = _segmentsPrefab.name;
//                go.transform.SetParent(this.transform, false);

//                Image img = go.GetComponent<Image>();
//                if (img == null) img = go.GetComponentInChildren<Image>();
//                if (img == null)
//                {
//#if UNITY_EDITOR
//                    Debug.LogError($"SegmentedBar2: prefab '{_segmentsPrefab.name}' must contain an Image component.");
//#else
//                    Debug.LogError($"SegmentedBar2: segments prefab must contain an Image component.");
//#endif
//                    img = go.AddComponent<Image>();
//                }
//                Segments.Add(img);
//            }
//        }

//        ApplySpritesToSegments();
//        UpdateActiveVisuals();
//    }

//#if UNITY_EDITOR
//    // Планирование отложенного удаления — вызывается только в редакторе
//    private void ScheduleDestroyIfNeeded()
//    {
//        if (_destroyScheduled) return;
//        _destroyScheduled = true;
//        EditorApplication.delayCall += ProcessPendingDestroy;
//    }

//    // Выполняется из delayCall, то есть уже не в контексте OnValidate
//    private void ProcessPendingDestroy()
//    {
//        _destroyScheduled = false;

//        // Используем Undo.DestroyObjectImmediate чтобы поддержать Undo/Redo в редакторе
//        for (int i = 0; i < _toDestroy.Count; i++)
//        {
//            var go = _toDestroy[i];
//            if (go == null) continue;
//            // Если объект уже уничтожен — пропускаем
//            if (Application.isPlaying)
//            {
//                Destroy(go);
//            }
//            else
//            {
//                // Удаляем через Undo систему (может использовать DestroyImmediate внутри, но уже не в OnValidate)
//                Undo.DestroyObjectImmediate(go);
//            }
//        }

//        _toDestroy.Clear();
//    }
//#endif

//    private void ApplySpritesToSegments()
//    {
//        int count = Segments.Count;
//        if (count == 0) return;

//        for (int i = 0; i < count; i++)
//        {
//            Image img = Segments[i];
//            if (img == null) continue;

//            if (count == 1)
//            {
//                img.sprite = _monoSegmentSprite;
//            }
//            else if (count == 2)
//            {
//                img.sprite = (i == 0) ? _leftSegmentSprite : _rightSegmentSprite;
//            }
//            else
//            {
//                if (i == 0) img.sprite = _leftSegmentSprite;
//                else if (i == count - 1) img.sprite = _rightSegmentSprite;
//                else img.sprite = _middleSegmentSprite;
//            }
//        }
//    }

//    private void UpdateActiveVisuals()
//    {
//        // Если нужно — убедимся, что количество сегментов соответствует полю
//        if (Segments.Count != _segmentsCount)
//        {
//            RebuildSegments();
//        }

//        int active = Mathf.Clamp(_activeSegmentsCount, 0, Segments.Count);

//        for (int i = 0; i < Segments.Count; i++)
//        {
//            var img = Segments[i];
//            if (img == null) continue;
//            img.color = (i < active) ? _enabledColor : _disabledColor;
//        }
//    }

//    public void Refresh()
//    {
//        RebuildSegments();
//        UpdateActiveVisuals();
//    }
//}
