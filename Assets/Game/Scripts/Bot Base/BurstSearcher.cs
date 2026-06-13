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

    private Collider[] _colliders = new Collider[10];
    private List<Crystal> _foundCrystals = new List<Crystal>(10);

    public event Action<IReadOnlyList<Crystal>> SearchConducted;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _searchRadius);
    }

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

    private IReadOnlyList<Crystal> Search()
    {
        _colliders = new Collider[10];

        Physics.OverlapSphereNonAlloc(transform.position, _searchRadius, _colliders, _targetLayer);
        
        _foundCrystals.Clear();

        foreach (Collider collider in _colliders)
        {
            if (collider == null) 
                continue;

            if (collider.TryGetComponent(out Crystal crystal))
                if (crystal.IsTargeted == false)
                    _foundCrystals.Add(crystal);
        }

        _foundCrystals.Sort((a, b) =>
        {
            float distA = (a.transform.position - transform.position).sqrMagnitude;
            float distB = (b.transform.position - transform.position).sqrMagnitude;
            return distA.CompareTo(distB);
        });

        Debug.Log($"Search conducted, found {_foundCrystals.Count} crystals");

        return _foundCrystals;
    }
}
