using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

[RequireComponent(typeof(BotBaseSpawner))]
public class BurstSearcher : MonoBehaviour
{
    [SerializeField] private float _burstDelay = 1.0f;
    [SerializeField] private float _searchRadius = 15.0f;
    [SerializeField] private LayerMask _targetLayer;
    [Space(10)]
    [SerializeField] private int _searchBufferSize = 15;

    private BotBaseSpawner _baseSpawner;

    private WaitForSeconds _delay;

    private Collider[] _colliders;
    private Dictionary<Crystal, int> _foundCrystals = new Dictionary<Crystal, int>();

    public event Action<IReadOnlyDictionary<Crystal, int>> SearchConducted;

    public void SetCrystalUnavailable(Crystal crystal)
    {
        if (_foundCrystals.ContainsKey(crystal) == false)
        {
            Debug.LogWarning("Trying to make unavailable undiscovered crystal.");
            return;
        }

        _foundCrystals.Remove(crystal);
        _foundCrystals.Add(crystal, -1);
    }

    public void RemoveCrystal(Crystal crystal)
    {
        if (_foundCrystals.ContainsKey(crystal) == false)
        {
            Debug.LogWarning("Trying to remove undiscovered crystal.");
            return;
        }

        _foundCrystals.Remove(crystal);
    }

    private void OnDrawGizmosSelected()
    {
        if (_baseSpawner == null || _baseSpawner.BotBaseList.Count == 0)
            return;

        Gizmos.color = Color.red;

        foreach (BotBase botBase in _baseSpawner.BotBaseList)
            Gizmos.DrawWireSphere(botBase.transform.position, _searchRadius);
    }

    private void Awake()
    {
        _colliders = new Collider[_searchBufferSize];
        _baseSpawner = GetComponent<BotBaseSpawner>();

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

    private Dictionary<Crystal, int> Search()
    {
        int i = 0;

        foreach (BotBase botBase in _baseSpawner.BotBaseList)
        {
            Physics.OverlapSphereNonAlloc(botBase.transform.position, _searchRadius, _colliders, _targetLayer);

            foreach (Collider collider in _colliders)
            {
                if (collider == null)
                    continue;

                if (collider.TryGetComponent(out Crystal crystal))
                    if (_foundCrystals.ContainsKey(crystal) == false)
                        _foundCrystals.Add(crystal, i);
            }

            i++;
        }

        _foundCrystals = new Dictionary<Crystal, int>(GroupSorter.SortDictionary(_foundCrystals, _baseSpawner.BotBaseList.Select((BotBase botBase) => botBase.transform).ToList()));
        return _foundCrystals;
    }
}
