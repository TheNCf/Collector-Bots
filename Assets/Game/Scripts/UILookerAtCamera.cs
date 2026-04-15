using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILookerAtCamera : MonoBehaviour
{
    [SerializeField] private Transform _target;

    private void Update()
    {
        if (_target == null)
            return;

        transform.forward = _target.forward;
    }
}
