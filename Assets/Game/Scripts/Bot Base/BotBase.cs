using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotBase : MonoBehaviour
{
    [SerializeField] private BurstSearcher _burstSearcher;
    [SerializeField] private BotCommander _botCommander;
    [SerializeField] private Flag _flagPrefab;
    [SerializeField] private int _crystalPrice = 50;
    [SerializeField] private int _newBotCost = 150;
    [SerializeField] private int _newBaseCost = 250;

    private Flag _currentFlag;

    private int _crystalResource = 0;

    private Vector3 _expandPosition;

    public bool FocusOnExpand { get; private set; } = false;

    public bool CanExpand => _botCommander.BotsUnderCommand > 1;

    public event Action<int> CrystalResourceChanged;

    private void Awake()
    {
        transform.rotation = Quaternion.Euler(0f, 180f, 0f);
    }

    private void OnEnable()
    {
        _burstSearcher.SearchConducted += _botCommander.AquireTargets;
    }

    private void OnDisable()
    {
        _burstSearcher.SearchConducted -= _botCommander.AquireTargets;
    }

    public void Initialize(BotBaseSpawner botBaseSpawner)
    {
        _botCommander.Initialize(AddCrystal, botBaseSpawner);
    }

    public Flag CreateFlag()
    {
        _currentFlag = Instantiate(_flagPrefab, transform.position, Quaternion.identity);
        _currentFlag.Dropped += OnFlagDropped;
        return _currentFlag;
    }

    public void RemoveCurrentFlag()
    {
        if (_currentFlag == null)
            return;

        Destroy(_currentFlag.gameObject);
        FocusOnExpand = false;
    }

    private void OnFlagDropped()
    {
        FocusOnExpand = true;
        _currentFlag.Dropped -= OnFlagDropped;
    }

    private void AddCrystal()
    {
        _crystalResource += _crystalPrice;
        CrystalResourceChanged?.Invoke(_crystalResource);

        if (FocusOnExpand)
        {
            if (_crystalResource < _newBaseCost || _botCommander.BotsUnderCommand < 2)
                return;

            _crystalResource -= _newBaseCost;
            CrystalResourceChanged?.Invoke(_crystalResource);
            _expandPosition = _currentFlag.transform.position;
            _botCommander.SendBotToBuildBase(_expandPosition);
            _currentFlag = null;
            FocusOnExpand = false;
        }
        else
        {
            if (_crystalResource < _newBotCost)
                return;

            _botCommander.CreateNewBot();
            _crystalResource -= _newBotCost;
            CrystalResourceChanged?.Invoke(_crystalResource);
        }
    }
}
