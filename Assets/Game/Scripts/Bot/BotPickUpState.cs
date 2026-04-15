using UnityEngine;

public class BotPickUpState : IState
{
    private StateMachine _stateMachine;
    private Animator _animator;
    private BotMover _mover;

    public BotPickUpState(StateMachine stateMachine, Animator animator, BotMover mover)
    {
        _stateMachine = stateMachine;
        _animator = animator;
        _mover = mover;
    }

    public void Enter()
    {
        _animator.SetTrigger(BotAnimatorData.Params.PickUp);
        _mover.TargetAcquired += OnTargetAcquired;
    }

    public void Exit()
    {
        _mover.TargetAcquired -= OnTargetAcquired;
    }

    public void Tick()
    {
        
    }

    private void OnTargetAcquired()
    {
        _stateMachine.ChangeState(typeof(BotRunState));
    }
}