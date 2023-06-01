using System.Collections;
using UnityEngine;

public static class MonobehaviourExtension
{
    public static CoroutineHandler RunCoroutine(this MonoBehaviour mono,IEnumerator coroutine)
    {
        return new CoroutineHandler(mono, coroutine);
    }
}
