using UnityEngine;

public static class RectTransformExtensions
{
    public static void SetLeft(this RectTransform rt, float left)
    {
        if(rt.offsetMin.x != left) rt.offsetMin = new Vector2(left, rt.offsetMin.y);
    }

    public static void SetBottom(this RectTransform rt, float bottom)
    {
        if (rt.offsetMin.y != bottom) rt.offsetMin = new Vector2(rt.offsetMin.x, bottom);
    }

    public static void SetRight(this RectTransform rt, float right)
    {
        if (rt.offsetMax.x != -right) rt.offsetMax = new Vector2(-right, rt.offsetMax.y);
    }

    public static void SetTop(this RectTransform rt, float top)
    {
        if (rt.offsetMax.y != -top) rt.offsetMax = new Vector2(rt.offsetMax.x, -top);
    }
}