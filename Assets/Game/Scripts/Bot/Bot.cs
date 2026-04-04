using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator), typeof(StateMachine), typeof(BotMover))]
public class Bot : MonoBehaviour
{
    private Animator _animator;
    private StateMachine _stateMachine;
    private BotMover _mover;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _stateMachine = GetComponent<StateMachine>();
        _mover = GetComponent<BotMover>();

        _stateMachine.Initialize(InitializeStateMachine);
        _mover.SetTarget(new Vector3(15, 0, 15));
    }

    private void InitializeStateMachine()
    {
        _stateMachine.AddState(new BotIdleState(_stateMachine, _animator, _mover));
        _stateMachine.AddState(new BotRunState(_stateMachine, _animator, _mover));

        _stateMachine.ChangeState(typeof(BotIdleState));
    }
}
