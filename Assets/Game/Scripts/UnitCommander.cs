using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UnitCommander : MonoBehaviour
{
    [SerializeField] private List<Bot> _bots = new List<Bot>();
    [SerializeField] private GameObject _newUnitPrefab;
    [SerializeField] private Transform _crystalStorage;

    private Action<Crystal> _onCrystalDelivered;

    private int _unitsMaxWidth = 3;
    private int _currentWidthIndex = 0;
    private int _currentLengthIndex = 0;

    private Vector3 _unitStartPosition;
    private float _gapBetweenUnits = 1.5f;

    private void Start()
    {
        _unitStartPosition = transform.position;

        CreateNewUnit();
        CreateNewUnit();
        CreateNewUnit();
    }

    private void OnDisable()
    {
        if (_bots.Count == 0)
            return;

        foreach (Bot bot in _bots)
        {
            bot.CrystalDelivered -= _onCrystalDelivered;
        }
    }

    public void Initialize(Action<Crystal> OnCrystalDelivered)
    {
        _onCrystalDelivered = OnCrystalDelivered;
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
            _targets[targetIndex].OnTrageted();
            targetIndex++;
        }
    }

    public void CreateNewUnit()
    {
        Vector3 position = _unitStartPosition + new Vector3(_currentWidthIndex, 0, -_currentLengthIndex) * _gapBetweenUnits;
        GameObject newUnit = Instantiate(_newUnitPrefab, position, Quaternion.identity);
        newUnit.transform.parent = transform;
        newUnit.transform.forward = transform.forward;
        Bot newBot = newUnit.GetComponentInChildren<Bot>(true);
        newBot.Initialize(_crystalStorage);
        newBot.CrystalDelivered += _onCrystalDelivered;
        _bots.Add(newBot);

        _currentWidthIndex++;

        if (_currentWidthIndex >= _unitsMaxWidth)
        {
            _currentWidthIndex = 0;
            _currentLengthIndex++;
        }
    }
}