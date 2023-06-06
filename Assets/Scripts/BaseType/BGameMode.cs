using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public enum GameModeState
{
    Idle = 0,
    Started,
    Ended,
    Starting,
    Ending
}

public abstract class BGameMode : IGameMode
{
    GameModeState modeState = GameModeState.Idle;
    public GameModeState ModeState => modeState;

    public virtual IEnumerator OnEnd()
    {
        throw new System.NotImplementedException();
    }
    public virtual IEnumerator OnStart()
    {
        throw new System.NotImplementedException();
    }
}
