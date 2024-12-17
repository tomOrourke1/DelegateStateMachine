
public class DelegateStateMachine
{

    Dictionary<TState, List<TTransition>> stateTransitions;
    List<TTransition> anyTransitions;

    TState currentState;


    public DelegateStateMachine()
    {
        stateTransitions = new();
        anyTransitions = new();
    }

    public void AddAnyTransition(TState to, Func<bool> cond)
    {
        anyTransitions.Add(new TTransition(from: null, to: to, cond: cond));
    }

    public void AddTransition(TState from, TState to, Func<bool> cond)
    {
        if (!stateTransitions.ContainsKey(from))
        {
            stateTransitions.Add(from, new List<TTransition>());
        }

        stateTransitions[from].Add(new TTransition(from, to, cond));
    }

    public void SetState(TState state)
    {
        currentState?.OnExit?.Invoke();

        currentState = state;

        currentState?.OnEnter?.Invoke();
    }

    public void Tick()
    {
        var s = CheckTransition(currentState);
        if (s != null)
        {
            SetState(s.To);
        }

        // currentState?.Tick?.Invoke();

        currentState?.OnTick?.Invoke();
    }

    public void FixedTick()
    {
        currentState?.OnFixedTick?.Invoke();
    }
    public void LateTick()
    {
        currentState?.OnLateTick?.Invoke();
    }

    private TTransition CheckTransition(TState curr)
    {
        foreach (var s in anyTransitions)
        {
            if (s.Cond())
            {
                return s;
            }
        }

        if (stateTransitions.ContainsKey(curr))
        {
            foreach (var s in stateTransitions[curr])
            {
                if (s.Cond())
                {
                    return s;
                }
            }
        }

        return null;
    }

}




public class TTransition
{

    public TState From;
    public TState To;
    public Func<bool> Cond;

    public TTransition(TState from, TState to, Func<bool> cond)
    {
        From = from;
        To = to;
        Cond = cond;
    }
}

public class TState
{

    public delegate void Foo();

    public Foo OnTick;
    public Foo OnLateTick;
    public Foo OnFixedTick;
    public Foo OnEnter;
    public Foo OnExit;

    public TState(Foo tick = null, Foo lateTick = null, Foo fixedTick = null, Foo enter = null, Foo exit = null)
    {
        OnTick = tick;
        OnLateTick = lateTick;
        OnFixedTick = fixedTick;
        OnEnter = enter;
        OnExit = exit;
    }

}
