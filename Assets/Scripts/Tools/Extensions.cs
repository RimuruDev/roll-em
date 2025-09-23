using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public static class RectTransformExtensions
{
    public static Vector3 TransformToWorldPoint(this RectTransform rectTransform)
    {
        return rectTransform.TransformPoint(rectTransform.rect.center);
    }
}