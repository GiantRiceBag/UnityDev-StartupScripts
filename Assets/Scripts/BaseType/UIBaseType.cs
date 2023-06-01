using UnityEngine;

public class UIBaseType : MonoBehaviour
{
    public event System.Action onDestory;
    public event System.Action onEnable;

    public virtual UIBaseType Disable() { RunOnAniamtionComplete(); return this; }
    public virtual UIBaseType Enable() { gameObject.SetActive(true);onEnable?.Invoke(); return this; }
    public virtual void RunOnAniamtionComplete() { gameObject.SetActive(false); onDestory?.Invoke(); }
    protected virtual void Start() { Init(); }
    protected virtual void Init() { }

}