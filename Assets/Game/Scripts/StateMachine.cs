using System;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    private List<IState> _states = new List<IState>();

    private IState _currentState;

    private void Update()
    {
        _currentState?.Tick();
    }

    public void AddState(IState state)
    {
        foreach (var existingState in _states)
        {
            if (existingState.GetType() == state.GetType())
            {
                Debug.LogWarning("Tried to add state, that already exists!");
                return;
            }
        }

        _states.Add(state);
    }

    public void ChangeState(Type stateType)
    {
        foreach (var existingState in _states)
        {
            if ( existingState.GetType() == stateType)
            {
                _currentState?.Exit();
                _currentState = existingState;
                _currentState.Enter();
                return;
            }
        }

        Debug.LogWarning("Tried change to state, that doesn't exist or added!");
    }
}