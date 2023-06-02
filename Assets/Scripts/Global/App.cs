using UnityEngine;

public class App : BSingleton<App>
{
    public static AudioManager audioService => AudioManager.Instance;
    public static BackdropManager backdropService => BackdropManager.Instance;
    public static GameModeManager gameModeService => GameModeManager.Instance;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Bootstrap()
    {
        // TODO : load prefab
        // TEMP
        instance = new GameObject(typeof(App).ToString(), typeof(App)).GetComponent<App>();
        audioService.transform.SetParent(Instance.transform);
        backdropService.transform.SetParent(Instance.transform);
        gameModeService.transform.SetParent(Instance.transform);

        DontDestroyOnLoad(Instance);
    }
}
