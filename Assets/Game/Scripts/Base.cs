using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{
    [SerializeField] private BurstSearcher _burstSearcher;
    [SerializeField] private UnitCommander _unitCommander;
    [SerializeField] private int _crystalPrice = 50;
    [SerializeField] private int _newUnitCost = 150;
    [SerializeField] private int _newBaseCost = 250;

    private int _crystalResource = 0;

    private bool _focusOnExpand = false;

    public event Action<int> CrystalResourceChanged;

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

        if (_focusOnExpand)
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
