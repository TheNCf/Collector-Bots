using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalSpawner : SpawnerBase<Crystal>
{
    [SerializeField] private float _radius;
    [SerializeField] private float _delay;

    private WaitForSeconds _wait;

    private float RandomPointOnAxis => Random.Range(-_radius, _radius);

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        Vector3 center = transform.position;
        Vector3 size = new Vector3(_radius, 0, _radius) * 2;

        Gizmos.DrawWireCube(center, size);
    }

    protected override void Awake()
    {
        base.Awake();
        _wait = new WaitForSeconds(_delay);

        StartCoroutine(SpawnCoroutine());
    }

    protected override Crystal Create()
    {
        Crystal obj = base.Create();
        obj.transform.parent = transform;
        obj.transform.position = transform.position;
        return obj;
    }

    private IEnumerator SpawnCoroutine()
    {
        while (isActiveAndEnabled)
        {
            yield return _wait;

            if (PoolSizeAtStart <= ActiveInPool)
                continue;

            Vector3 randomModifier = new Vector3 (RandomPointOnAxis, 0, RandomPointOnAxis);
            Vector3 rayOrigin = transform.position + randomModifier;
            Ray ray = new Ray(rayOrigin, Vector3.down);

            if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue) == false)
                continue;

            if (hit.collider.TryGetComponent(out Ground ground) == false)
                continue;

            Crystal crystal = ObjectPool.Get();
            crystal.transform.position = hit.point;
            crystal.Catched += ObjectPool.Release;
        }
    }
}
