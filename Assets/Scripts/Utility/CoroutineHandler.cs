using System.Collections;
using UnityEngine;

public class CoroutineHandler : IEnumerator
{
    public bool IsDone { get;private set; }
    public bool MoveNext() => !IsDone;
    public object Current { get; }
    public void Reset() { }

    public CoroutineHandler(MonoBehaviour mono,IEnumerator coroutine)
    {
        mono.StartCoroutine(Wrap(coroutine));
    }
    public CoroutineHandler(MonoBehaviour mono, System.Action action)
    {
        mono.StartCoroutine(Wrap(AsEnumerator(action)));
    }

    IEnumerator Wrap(IEnumerator coroutine)
    {
        yield return coroutine;
        IsDone = true;
    }
    IEnumerator AsEnumerator(System.Action action)
    {
        action?.Invoke();
        yield break;
    }
}
