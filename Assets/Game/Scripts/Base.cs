using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BurstSearcher))]
[RequireComponent(typeof(UnitCommander))]
public class Base : MonoBehaviour
{
    private BurstSearcher _burstSearcher;
    private UnitCommander _unitCommander;

    private int _crystalResource = 0;

    public event Action<int> CrystalResourceChanged;

    private void Awake()
    {
        _burstSearcher = GetComponent<BurstSearcher>();
        _unitCommander = GetComponent<UnitCommander>();

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
