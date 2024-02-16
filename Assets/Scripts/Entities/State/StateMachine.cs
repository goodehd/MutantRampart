using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{
    public EState CurrentStateName { get; private set; }

    private BaseState _curState;
    private Dictionary<EState, BaseState> _states;

    public StateMachine()
    {
        _states = new Dictionary<EState, BaseState>();
    }

    public void AddState(EState stateName, BaseState state)
    {
        if (!_states.ContainsKey(stateName))
        {
            _states.Add(stateName, state);
        }
    }

    public void ChangeState(EState stateName)
    {
        if (_states.TryGetValue(stateName, out BaseState nextState))
        {
            if(_curState == nextState)
            {
                return;
            }
            _curState?.ExitState();
            _curState = nextState;
            CurrentStateName = stateName;
        }
        nextState?.EnterState();
    }

    public void UpdateState()
    {
        _curState?.UpdateState();
    }

    public void InitState()
    {
        foreach (var state in _states.Values)
        {
            state.Init();
        }
    }
}
