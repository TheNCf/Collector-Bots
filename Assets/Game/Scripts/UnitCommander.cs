using System;
using System.Collections.Generic;
using UnityEngine;

public class UnitCommander : MonoBehaviour
{
    [SerializeField] private List<Bot> _bots = new List<Bot>();
    [SerializeField] private GameObject _newUnitPrefab;

    private Action<Crystal> _onCrystalDelivered;

    private int _unitsMaxWidth = 3;
    private int _currentWidthIndex = 0;
    private int _currentLengthIndex = 0;

    private Vector3 _unitStartPosition;
    private float _gapBetweenUnits = 1.5f;

    private void Start()
    {
        if (_bots[0] == null)
            return;

        _unitStartPosition = transform.position;
    }

    private void OnDisable()
    {
        foreach (Bot bot in _bots)
        {
            bot.CrystalDelivered -= _onCrystalDelivered;
        }
    }

    public void Initialize(Action<Crystal> OnCrystalDelivered)
    {
        _onCrystalDelivered = OnCrystalDelivered;

        foreach (Bot bot in _bots)
        {
            bot.CrystalDelivered += OnCrystalDelivered;
        }
    }

    public void AquireTargets(IReadOnlyList<Crystal> _targets)
    {
        int targetIndex = 0;

        foreach (Bot bot in _bots)
        {
            if (bot.IsBusy)
                continue;

            if (targetIndex >= _targets.Count)
                return;

            bot.AquireTarget(_targets[targetIndex].transform.position);
            targetIndex++;
        }
    }

    public void CreateNewUnit()
    {
        _currentWidthIndex++;

        if (_currentWidthIndex >= _unitsMaxWidth)
        {
            _currentWidthIndex = 0;
            _currentLengthIndex++;
        }

        Vector3 position = _unitStartPosition + new Vector3(_currentWidthIndex, 0, -_currentLengthIndex) * _gapBetweenUnits;
        GameObject newUnit = Instantiate(_newUnitPrefab, position, Quaternion.identity);
        newUnit.transform.parent = transform;
        newUnit.transform.forward = transform.forward;
        Bot newBot = newUnit.GetComponentInChildren<Bot>();
        _bots.Add(newBot);
    }
}