using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Animator), typeof(StateMachine), typeof(BotMover))]
public class Bot : MonoBehaviour
{
    [SerializeField] private Transform _crystalStorage;
    [SerializeField] private float _pickUpDistance = 1f;
    [SerializeField] private LayerMask _crystalLayerMask;

    private Animator _animator;
    private StateMachine _stateMachine;
    private BotMover _mover;

    private Vector3 _startPlace;

    private Collider _carriedCrystal;

    public bool IsBusy { get; private set; } = false;

    public event Action<Crystal> CrystalDelivered;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _stateMachine = GetComponent<StateMachine>();
        _mover = GetComponent<BotMover>();

        InitializeStateMachine();

        _startPlace = transform.position;
    }

    public void AquireTarget(Vector3 position)
    {
        _mover.SetTarget(position);
        IsBusy = true;
    }

    public void OnPickUpFinished()
    {
        if (_animator.GetBool(BotAnimatorData.Params.IsCarrying))
        {
            _mover.SetTarget(_startPlace);
            CrystalDelivered?.Invoke(_carriedCrystal.GetComponent<Crystal>());
            Destroy(_carriedCrystal.gameObject);
            IsBusy = false;
            _animator.SetBool(BotAnimatorData.Params.IsCarrying, false);
        }
        else
        {
            if (TryTakeCrystal())
            {
                _mover.SetTarget(_crystalStorage.position);
                _animator.SetBool(BotAnimatorData.Params.IsCarrying, true);
            }
            else
            {
                _mover.SetTarget(_startPlace);
                IsBusy = false;
            }
        }
    }

    private void InitializeStateMachine()
    {
        _stateMachine.AddState(new BotIdleState(_stateMachine, _animator, _mover));
        _stateMachine.AddState(new BotRunState(_stateMachine, _animator, _mover, this));
        _stateMachine.AddState(new BotPickUpState(_stateMachine, _animator, _mover));

        _stateMachine.ChangeState(typeof(BotIdleState));
    }

    private bool TryTakeCrystal()
    {
        _carriedCrystal = Physics.OverlapSphere(transform.position, _pickUpDistance, _crystalLayerMask).FirstOrDefault();

        if (_carriedCrystal != null)
        {
            _carriedCrystal.transform.parent = transform;
        }

        return _carriedCrystal != null;
    }
}
