using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flag : MonoBehaviour
{
    private Animator _animator;

    public event Action Dropped;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void Drop()
    {
        _animator.SetTrigger(FlagAnimationData.Params.Drop);
        Dropped?.Invoke();
    }

    private void OnDestroy()
    {
        Dropped = null;
    }
}
