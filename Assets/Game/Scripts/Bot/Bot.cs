using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Animator), typeof(StateMachine), typeof(BotMover))]
[RequireComponent(typeof(BotAnimationEventHandler), typeof(BotBackpack))]
public class Bot : MonoBehaviour
{
    [SerializeField] private float _pickUpDistance = 1f;
    [SerializeField] private LayerMask _crystalLayerMask;
    
    private Transform _crystalStorage;

    private Animator _animator;
    private StateMachine _stateMachine;
    private BotMover _mover;
    private BotBackpack _backpack;
    private BurstSearcher _burstSearcher;

    private Vector3 _startPlace;

    private bool _isInitialized = false;

    private Crystal _targetCrystal;

    public event Action<Crystal> CrystalDelivered;

    public bool IsBusy { get; private set; } = false;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _stateMachine = GetComponent<StateMachine>();
        _mover = GetComponent<BotMover>();
        _backpack = GetComponent<BotBackpack>();

        InitializeStateMachine();

        _startPlace = transform.position;

        gameObject.SetActive(false);
    }

    public void Initialize(Transform crystalStorage, BurstSearcher burstSearcher)
    {
        _crystalStorage = crystalStorage;
        _burstSearcher = burstSearcher;
        gameObject.SetActive(true);
        _isInitialized = true;
    }

    public void AquireTarget(Vector3 position, Crystal targetCrystal = null)
    {
        _mover.SetTarget(position);
        IsBusy = true;
        _targetCrystal = targetCrystal;
    }

    public void HandleInteraction()
    {
        if (_isInitialized == false)
            throw new Exception($"Bot ({gameObject.name}) can't interract while not initialized!");

        if (IsBusy == false)
            return;

        if (_animator.GetBool(BotAnimatorData.Params.IsCarrying))
        {
            _mover.SetTarget(_startPlace);
            IsBusy = false;
            _animator.SetBool(BotAnimatorData.Params.IsCarrying, false);
            
            if (_backpack.Drop().TryGetComponent(out Crystal crystal))
            {
                crystal.SetCatched();
                CrystalDelivered?.Invoke(crystal);
            }
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
                _burstSearcher.RemoveCrystal(_targetCrystal);
                IsBusy = false;
            }

            _targetCrystal = null;
        }
    }

    public void BuildBase(Vector3 position, BotBaseSpawner botBaseSpawner)
    {
        AquireTarget(position);

        GameObject droppedObject = _backpack.Drop();
        _animator.SetBool(BotAnimatorData.Params.IsCarrying, false);

        if (droppedObject != null)
        {
            droppedObject.TryGetComponent(out Crystal droppedCrystal);
            _burstSearcher.RemoveCrystal(droppedCrystal);
        }

        _burstSearcher.RemoveCrystal(_targetCrystal);

        _mover.PathCompleted += () =>
        {
            botBaseSpawner.SpawnBotBase(position);
            Destroy(gameObject);
        };
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
        Collider crystalCollider = Physics.OverlapSphere(transform.position, _pickUpDistance, _crystalLayerMask).FirstOrDefault();

        if (crystalCollider.TryGetComponent(out Crystal crystal))
        {
            _backpack.Put(crystal.gameObject);
            return true;
        }

        return false;
    }
}
