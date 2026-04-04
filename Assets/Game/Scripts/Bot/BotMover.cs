using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class BotMover : MonoBehaviour
{
    private NavMeshAgent _agent;

    private bool _isPathCompleted = true;

    public float CurrentSqrSpeed => _agent.desiredVelocity.sqrMagnitude;

    public event Action TargetAcquired;
    public event Action PathCompleted;

    private void Update()
    {
        if (_isPathCompleted == false && _agent.remainingDistance < 0.1f)
        {
            PathCompleted?.Invoke();
            _isPathCompleted = true;
        }
    }

    public void SetTarget(Vector3 target)
    {
        if (_agent == null)
            _agent = GetComponent<NavMeshAgent>();

        _agent.destination = target;
        _isPathCompleted = false;
        TargetAcquired?.Invoke();
    }
}
