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

    IEnumerator Wrap(IEnumerator coroutine)
    {
        yield return coroutine;
        IsDone = true;
    }
}
