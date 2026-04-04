using UnityEngine;

public class BotRunState : IState
{
    private StateMachine _stateMachine;
    private Animator _animator;
    private BotMover _mover;

    public BotRunState(StateMachine stateMachine, Animator animator, BotMover mover)
    {
        _stateMachine = stateMachine;
        _animator = animator;
        _mover = mover;
    }

    public void Enter()
    {
        _mover.PathCompleted += OnPathCompleted;
    }

    public void Exit()
    {
        _mover.PathCompleted -= OnPathCompleted;
    }

    public void Tick()
    {
        _animator.SetFloat(BotAnimatorData.Params.Speed, _mover.CurrentSqrSpeed);
    }

    private void OnPathCompleted()
    {
        _stateMachine.ChangeState(typeof(BotIdleState));
    }
}