using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class BurstSearcher : MonoBehaviour
{
    [SerializeField] private float _burstDelay = 1.0f;
    [SerializeField] private float _searchRadius = 15.0f;
    [SerializeField] private LayerMask _targetLayer;

    private WaitForSeconds _delay;

    public event Action<Collider[]> SearchConducted;

    private void Awake()
    {
        _delay = new WaitForSeconds(_burstDelay);
        StartCoroutine(SearchCoroutine());
    }

    private IEnumerator SearchCoroutine()
    {
        while (isActiveAndEnabled)
        {
            yield return _delay;
            SearchConducted?.Invoke(Search());
        }
    }

    private Collider[] Search()
    {
        Collider[] colliders = new Collider[0];
        Physics.OverlapSphereNonAlloc(transform.position, _searchRadius, colliders, _targetLayer);
        Debug.Log($"Search conducted, found {colliders.Length} crystals");
        return colliders.OrderBy(collider => (collider.transform.position - transform.position).sqrMagnitude).ToArray();
    }
}
