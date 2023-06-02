using UnityEngine;

public class BSingleton<T> : MonoBehaviour where T : Component
{
    protected static T instance;
    protected bool enable;

    public static T Instance
    {
        get
        {
            instance = FindObjectOfType<T>();
            if(instance is null)
            {
                GameObject go = new GameObject(typeof(T).ToString(),typeof(T));
            }
             return instance;
        }
    }

    protected virtual void Awake()
    {
        if (!Application.isPlaying)
        {
            return;
        }

        if(instance is null)
        {
            instance = this as T;
            //DontDestroyOnLoad(gameObject);
            enable = true;
        }
        else
        {
            if (this != instance)
                Destroy(gameObject);
        }
    }
}
