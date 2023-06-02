using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;
using System.Collections;

public enum BackdropType
{
    DipToBlack
}

public class BackdropEvent
{
    public System.Action onBackdropCreated;
    public System.Action onBackdropIn;
    public System.Action onBackdropOut;
}

[RequireComponent(typeof(RectTransform),typeof(Canvas),typeof(CanvasScaler))]
public class BackdropManager : BSingleton<BackdropManager>
{
    bool isPlaying = false;
    // TEMP
    GameObject backdrop;

    protected override void Awake()
    {
        base.Awake();
        gameObject.layer = 5;
        GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
        GetComponent<Canvas>().sortingOrder = 500;
        GetComponent<CanvasScaler>().uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        GetComponent<CanvasScaler>().referenceResolution = new Vector2(1920, 1080);

        // TEMP
        backdrop = new GameObject("backdrop", typeof(RectTransform), typeof(Image));
        backdrop.transform.SetParent(transform);
        var rect = backdrop.GetComponent<RectTransform>();
        backdrop.transform.localScale = Vector3.one;
        rect.anchorMin = Vector3.zero;
        rect.anchorMax = Vector3.one;
        rect.offsetMax = rect.offsetMin = Vector2.zero;
        var image = backdrop.GetComponent<Image>();
        image.color = new Color(0, 0, 0, 0);
    }

    public BackdropEvent CreateBackdrop(BackdropType backdropType = BackdropType.DipToBlack)
    {
        if (isPlaying) return null;

        BackdropEvent backdropEvent = new BackdropEvent();

        // TEMP
        switch (backdropType)
        {
            case BackdropType.DipToBlack:
                {
                    isPlaying = true;
                    backdropEvent.onBackdropCreated?.Invoke();
                    var image = backdrop.GetComponent<Image>();
                    image.color = new Color(0, 0, 0, 0);
                    image.DOColor(new Color(0, 0, 0, 1), .5f).onComplete = ()=> {
                        backdropEvent.onBackdropIn?.Invoke();
                        image.DOColor(new Color(0, 0, 0, 0), .5f).onComplete = () =>
                            {
                                backdropEvent.onBackdropOut?.Invoke();
                                isPlaying = false;
                                Destroy(backdrop);
                            };
                        };

                    break;
                }
            default:break;
        }

        return backdropEvent;
    }

    public IEnumerator Require(BackdropType backdropType = BackdropType.DipToBlack)
    {
        if (isPlaying)
            yield break;
        isPlaying = true;
        // TEMP
        var image = backdrop.GetComponent<Image>();
        image.color = new Color(0, 0, 0, 0);
        yield return image.DOColor(new Color(0, 0, 0, 1), .5f).WaitForCompletion();
    }
    public IEnumerator Release()
    {
        if (!isPlaying)
            yield break;
        isPlaying = false;
        // TEMP
        var image = backdrop.GetComponent<Image>();
        image.color = new Color(0, 0, 0, 1);
        yield return image.DOColor(new Color(0, 0, 0, 0), .5f).WaitForCompletion();
    }
}
