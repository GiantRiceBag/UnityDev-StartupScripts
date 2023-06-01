using UnityEngine;
using UnityEngine.SceneManagement;

public class PoolContent : MonoBehaviour
{
    Scene targetScene;
    ObjectPool pool;
    public ObjectPool Pool { set { pool = value; } }

    public void Init(ObjectPool pool)
    {
        DontDestroyOnLoad(gameObject);
        gameObject.hideFlags = HideFlags.HideInHierarchy;
        targetScene = gameObject.scene;
        this.pool = pool;
        gameObject.SetActive(false);
        name = "PoolContent";
        SceneManager.sceneUnloaded += (scene) =>
        {
            if (scene == targetScene)
                Dispose();
        };
    }

    public virtual void Dispose() { pool.Collect(gameObject); }
}