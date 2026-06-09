using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{
    [SerializeField] private BurstSearcher _burstSearcher;
    [SerializeField] private UnitCommander _unitCommander;
    [SerializeField] private Flag _flagPrefab;
    [SerializeField] private int _crystalPrice = 50;
    [SerializeField] private int _newUnitCost = 150;
    [SerializeField] private int _newBaseCost = 250;

    private Flag _currentFlag;

    private int _crystalResource = 0;

    public bool FocusOnExpand { get; private set; } = false;

    public bool CanExpand => _unitCommander.UnitsUnderCommand > 1;

    public event Action<int> CrystalResourceChanged;

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

    private void Awake()
    {
        _unitCommander.Initialize(AddCrystal);
    }

    private void OnEnable()
    {
        _burstSearcher.SearchConducted += _unitCommander.AquireTargets;
    }

    private void OnDisable()
    {
        _burstSearcher.SearchConducted -= _unitCommander.AquireTargets;
    }

    private void AddCrystal(Crystal crystal)
    {
        _crystalResource += _crystalPrice;
        CrystalResourceChanged?.Invoke(_crystalResource);
        crystal.OnDelivered();

        if (FocusOnExpand)
        {
            if (_crystalResource >= _newBaseCost || _unitCommander.UnitsUnderCommand < 2)
                return;
        }
        else
        {
            if (_crystalResource < _newUnitCost)
                return;

            _unitCommander.CreateNewUnit();
            _crystalResource -= _newUnitCost;
            CrystalResourceChanged?.Invoke(_crystalResource);
        }
    }
}
