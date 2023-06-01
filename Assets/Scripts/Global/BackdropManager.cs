using UnityEngine.UI;
using UnityEngine;

public enum BackdropType
{
    DipToBlack
}

[RequireComponent(typeof(RectTransform),typeof(Canvas),typeof(CanvasScaler))]
public class BackdropManager : BSingleton<BackdropManager>
{
    bool isPlaying = false;

    protected override void Awake()
    {
        base.Awake();
        gameObject.hideFlags = HideFlags.HideInHierarchy;
        gameObject.layer = 5;
        GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
        GetComponent<Canvas>().sortingOrder = 500;
        GetComponent<CanvasScaler>().referenceResolution = new Vector2(1920, 1080);
    }

    public void CreateBackdrop()
    {

    }
}
