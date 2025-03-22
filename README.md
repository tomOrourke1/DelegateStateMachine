# DelegateStateMachine


Example usage

```c#
public class TestBehavious : MonoBehaviour
{
    enum states
    {
        none,
        playing,
        menu
    }

    DelegateStateMachine<states> SM;

    private void Awake()
    {
        SM = new();

        SM.Configure(states.menu)
            .AddTick(Menu)
            .Transition(states.playing, () => return Keyboard.current.spaceKey.waspressedthisframe);

        SM.Configure(states.playing).AddTick(Playing)
            .Transition(states.menu, () => return Keyboard.current.spaceKey.waspressedthisframe);
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
        SM.Check();
        SM.Tick();
    }
}

```


