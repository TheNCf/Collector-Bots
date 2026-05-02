using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{
    [SerializeField] private BurstSearcher _burstSearcher;
    [SerializeField] private UnitCommander _unitCommander;

    private int _crystalResource = 0;

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
        _crystalResource += 50;
        CrystalResourceChanged?.Invoke(_crystalResource);
        crystal.OnDelivered();
    }
}
