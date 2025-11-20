using UnityEngine;
using UnityEditor;

// Определяем атрибут ReadOnly
public class ReadOnlyAttribute : PropertyAttribute
{
}

// Создаем PropertyDrawer для атрибута ReadOnly
#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
public class ReadOnlyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        GUI.enabled = false; // Отключаем редактирование поля
        EditorGUI.PropertyField(position, property, label, true); // Отображаем поле
        GUI.enabled = true; // Включаем редактирование для остальных полей
    }
}
#endif