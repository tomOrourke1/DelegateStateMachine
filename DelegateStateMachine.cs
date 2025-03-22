
using System;
using System.Collections.Generic;


// Note TO SELF
// create a second delegate state machine that takes in a data object on the StateMachine level
// then intrinsically pass it around the states/ have states have access to it
// that way you can have best of both worlds?
// the usecase being the animStates needing playing data?
public class DelegateStateMachine<T> where T : Enum
{
    private Dictionary<T, TState<T>> states;

    T currState;


    public DelegateStateMachine()
    {
        states = new();
        currState = default;
    }


    // need to add state
    // need to configure state settings
    // need to add state transitions

    public TState<T> Configure(T state)
    {
        // if default state is not set set it here
        if (Convert.ToInt32(currState) == 0)
        {
            currState = state;
        }


        if (states.ContainsKey(state))
        {
            return states[state];
        }
        else
        {
            TState<T> st = new(state);
            states.Add(state, st);

            return st;
        }
    }

    public void Check()
    {
        // if I want any transitions they would go here
        // and the data would be stored in the State Machine itself

        var state = states[currState].CheckTransitions();

        if (Convert.ToInt32(state) != 0) // if not null then set state
        {
            SetState(state);
        }
    }

    public void SetState(T state)
    {
        states[currState]?.Exit();
        currState = state;
        states[currState]?.Enter();
    }


    public void Tick()
    {
        states[currState].Tick();
    }


    public void Fixed()
    {
        states[currState].Fixed();
    }

    public void Late()
    {
        states[currState].Late();
    }

}



public class TState<T> where T : Enum
{
    public delegate void Func();

    private Func OnEnter;
    private Func OnExit;

    private Func OnTick;
    private Func OnFixed;
    private Func OnLate;


    public readonly T State;

    Dictionary<T, Func<bool>> transitions;


    public TState(T state)
    {
        State = state;
        transitions = new();
    }

    public TState<T> Transition(T to, Func<bool> cond)
    {
        if (transitions.ContainsKey(to))
        {
            transitions[to] = cond;
        }
        else
        {
            transitions.Add(to, cond);
        }

        return this;
    }

    public T CheckTransitions()
    {
        foreach (var train in transitions)
        {
            if (train.Value())
            {
                return train.Key;
            }
        }

        return default;
    }


    public TState<T> AddTick(Func tick)
    {
        OnTick += tick;

        return this;
    }

    public TState<T> RemTick(Func tick)
    {
        OnTick -= tick;

        return this;
    }

    public TState<T> AddLate(Func late)
    {
        OnLate += late;

        return this;
    }
    public TState<T> RemLate(Func late)
    {
        OnLate -= late;

        return this;
    }

    public TState<T> AddFixed(Func @fixed)
    {
        OnFixed += @fixed;

        return this;
    }
    public TState<T> RemFixed(Func @fixed)
    {
        OnFixed += @fixed;

        return this;
    }


    public TState<T> AddEnter(Func enter)
    {
        OnEnter += enter;

        return this;
    }
    public TState<T> RemEnter(Func enter)
    {
        OnEnter -= enter;

        return this;
    }

    public TState<T> AddExit(Func exit)
    {
        OnExit += exit;

        return this;
    }
    public TState<T> RemExit(Func exit)
    {
        OnExit -= exit;

        return this;
    }



    public void Tick()
    {
        OnTick?.Invoke();
    }
    public void Late()
    {
        OnLate?.Invoke();
    }
    public void Fixed()
    {
        OnFixed?.Invoke();
    }

    public void Enter()
    {
        OnEnter?.Invoke();
    }
    public void Exit()
    {
        OnExit?.Invoke();
    }



}
