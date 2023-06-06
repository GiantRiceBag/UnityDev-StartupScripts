using System.Collections;
using UnityEngine;

public class GameModeManager : BSingleton<GameModeManager>
{
    IGameMode currentMode;
    bool isSwitching;

    public event System.Action onGameModeChanged;
    
    public void SwitchMode(IGameMode newmode)
    {
        StartCoroutine(CSwitchMode(newmode));
    }

    IEnumerator CSwitchMode(IGameMode newMode)
    {
        yield return new WaitUntil(() => !isSwitching);
        if(currentMode == newMode)
            yield break;

        isSwitching = true;
        yield return App.BackdropService.Require();

        if (currentMode is not null)
            yield return currentMode.OnEnd();
        currentMode = newMode;
        yield return currentMode.OnStart();

        yield return App.BackdropService.Release();
        isSwitching = false;
        onGameModeChanged?.Invoke();
    }
}
