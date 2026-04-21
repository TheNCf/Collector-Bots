using System;
using System.Collections.Generic;
using UnityEngine;

public class UnitCommander : MonoBehaviour
{
    [SerializeField] private List<Bot> _bots = new List<Bot>();

    private List<Transform> _accuiredTargets = new List<Transform>();

    private Action<Crystal> _onCrystalDelivered;

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

    public void AquireTargets(Collider[] _targets)
    {
        int targetIndex = 0;

        foreach (Bot bot in _bots)
        {
            if (bot.IsBusy)
                continue;

            if (targetIndex >= _targets.Length)
                return;

            if (_targets[targetIndex] == null)
                return;

            if (_accuiredTargets.Contains(_targets[targetIndex].transform))
                continue;

            bot.AquireTarget(_targets[targetIndex].transform.position);
            targetIndex++;
        }
    }
}