using System.Collections;

public interface IGameMode
{
    public IEnumerator OnStart();
    public IEnumerator OnEnd();
}
