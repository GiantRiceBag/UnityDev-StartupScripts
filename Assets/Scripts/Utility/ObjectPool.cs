using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum PoolType {
    Prefab = 0,
    Audio
}

[CreateAssetMenu(menuName = "Utility/ObjectPool", fileName = "ObjectPool")]
public class ObjectPool : ScriptableObject
{
    [SerializeField] PoolType poolType;
    [ConditionalHide("poolType","Prefab")]
    [SerializeField] GameObject objectPrefab;

    const int SIZE_POOL_INITIAL = 20;
    const int SIZE_POOL_MAX = 100;

    Queue<GameObject> objectQueue = new Queue<GameObject>();

    public void Collect(GameObject content)
    {
        content.SetActive(false);

        if (objectQueue.Count > SIZE_POOL_MAX)
            Destroy(content);
        else
            objectQueue.Enqueue(content);
    }

    public GameObject Get(Scene targetScene)
    {
        if (objectQueue.Count == 0)
        {
            if (!GenerateObject()) { Debug.LogError("Please reference a GameObject for objectPrefab"); return null; }
        }

        objectQueue.Peek().SetActive(true);
        return objectQueue.Dequeue();
    }

    bool GenerateObject()
    {
        switch (poolType)
        {
            case PoolType.Prefab:
                {
                    if (objectPrefab is null)
                    {
                        return false;
                    }

                    for(int i = 0; i < SIZE_POOL_INITIAL; i++)
                    {
                        if (objectPrefab.GetComponent<PoolContent>() is null)
                            objectPrefab.AddComponent(typeof(PoolContent));

                        GameObject obj = Instantiate(objectPrefab);
                        var content = obj.GetComponent<PoolContent>();
                        content.Init(this);

                        objectQueue.Enqueue(obj);
                    }
                    break;
                }
            case PoolType.Audio:
                {
                    for (int i = 0; i < SIZE_POOL_INITIAL; i++)
                    {
                        GameObject obj = new GameObject("poolContent", typeof(PoolContent), typeof(AudioSource));
                        var content = obj.GetComponent<PoolContent>();
                        content.Init(this);

                        var audio = obj.GetComponent<AudioSource>();
                        audio.spatialBlend = 0;
                        audio.playOnAwake = false;

                        objectQueue.Enqueue(obj);
                    }
                    break;
                }
        }

        return true;
    }
}