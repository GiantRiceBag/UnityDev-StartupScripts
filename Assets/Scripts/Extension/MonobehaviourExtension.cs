using System.Collections;
using UnityEngine;

public static class MonobehaviourExtension
{
    public static CoroutineHandler RunCoroutine(this MonoBehaviour mono,IEnumerator coroutine)
    {
        return new CoroutineHandler(mono, coroutine);
    }
    public static CoroutineHandler RunCoroutine(this MonoBehaviour mono,System.Action action)
    {
        return new CoroutineHandler(mono, action);
    }
    public static IEnumerator AsEnumerator(this MonoBehaviour mono,System.Action action)
    {
        action?.Invoke();
        yield break;
    }
}
