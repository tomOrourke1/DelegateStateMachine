# DelegateStateMachine


Example usage

```c#
public class TestBehavious : MonoBehaviour
{
    DelegateStateMachine SM;

    public TState playingState;
    TState menuState;

    private void Awake()
    {
        menuState = new(tick: Menu);
        playingState = new(tick: Playing);

        SM = new();

        SM.AddTransition(menuState, playingState, () => return Keyboard.current.spaceKey.waspressedthisframe);
        SM.AddTransition(playingState, menuState, () => return Keyboard.current.spaceKey.waspressedthisframe);

        SM.SetState(menuState);
    }

    void Menu()
    {
        Debug.Log("Menu Func");
    }
    void Playing()
    {
        Debug.Log("Playing Func");
    }

    private void Update()
    {
        SM.Tick();
    }
}

```


