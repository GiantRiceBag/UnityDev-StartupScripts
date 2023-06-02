using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class PoolContent : MonoBehaviour
{
    Scene targetScene;
    public Scene TargetScene  { set { targetScene = value; } get { return targetScene; } }

    ObjectPool pool;
    public ObjectPool Pool { set { pool = value; } }

    public void Init(ObjectPool pool)
    {
        DontDestroyOnLoad(gameObject);
        gameObject.hideFlags = pool.HidePoolContent?HideFlags.HideInHierarchy : HideFlags.None;
        this.pool = pool;
        gameObject.SetActive(false);
        SceneManager.sceneUnloaded += (scene) =>
        {
            if (gameObject.activeSelf && scene == TargetScene)
                Dispose();
        };
        SceneManager.activeSceneChanged += (current, next) =>
        {
            if (gameObject.activeSelf && current == TargetScene)
                Dispose();
        };
    }

    public virtual void Dispose() { pool.Collect(gameObject); }
}