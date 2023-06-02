using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameModeState
{
    Idle = 0,
    Running,
    Ended
}

public class BGameMode : IGameMode
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
