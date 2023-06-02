using UnityEngine;

public class BUIBaseType : MonoBehaviour
{
    public event System.Action onDestory;
    public event System.Action onEnable;

    public virtual BUIBaseType Disable() { RunOnAniamtionComplete(); return this; }
    public virtual BUIBaseType Enable() { gameObject.SetActive(true);onEnable?.Invoke(); return this; }
    public virtual void RunOnAniamtionComplete() { gameObject.SetActive(false); onDestory?.Invoke(); }
    protected virtual void Start() { Init(); }
    protected virtual void Init() { }

}