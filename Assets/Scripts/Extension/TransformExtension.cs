using UnityEngine;

public static class TransformExtension
{
    public static void ClearChilds(this Transform transform)
    {
        for (int i = transform.childCount; i >= 0; i--)
        {
            GameObject.Destroy(transform.GetChild(i).gameObject);
        }
    }

    public static void SetLastSiblingIndex(this Transform transform)
    {
        if (transform.parent is null)
            return;
        transform.SetSiblingIndex(transform.parent.childCount - 1);
    }
}
