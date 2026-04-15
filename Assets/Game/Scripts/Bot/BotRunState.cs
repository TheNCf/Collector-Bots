using UnityEngine;

public class BotRunState : IState
{
    private StateMachine _stateMachine;
    private Animator _animator;
    private BotMover _mover;
    private Bot _bot;

    public BotRunState(StateMachine stateMachine, Animator animator, BotMover mover, Bot bot)
    {
        _stateMachine = stateMachine;
        _animator = animator;
        _mover = mover;
        _bot = bot;
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
        if (_bot.IsBusy)
            _stateMachine.ChangeState(typeof(BotPickUpState));
        else
            _stateMachine.ChangeState(typeof(BotIdleState));
    }
}