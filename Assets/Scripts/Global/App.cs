using UnityEngine;

public class App : BSingleton<App>
{
    public static AudioManager AudioService => AudioManager.Instance;
    public static BackdropManager BackdropService => BackdropManager.Instance;
    public static GameModeManager GameModeService => GameModeManager.Instance;
    public static SaveManager SaveService => SaveManager.Instance;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Bootstrap()
    {
        // TODO : load prefab
        // TEMP
        instance = new GameObject(typeof(App).ToString(), typeof(App)).GetComponent<App>();
        AudioService.transform.SetParent(Instance.transform);
        BackdropService.transform.SetParent(Instance.transform);
        GameModeService.transform.SetParent(Instance.transform);
        SaveService.transform.SetParent(Instance.transform);

        DontDestroyOnLoad(Instance);
    }
}
