using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SpawnerBase<T> : MonoBehaviour where T : MonoBehaviour, IPoolableObject
{
    [SerializeField] protected T PrefabToSpawn;
    [SerializeField] protected int PoolSizeAtStart = 100;

    protected ObjectPool<T> ObjectPool;
    private int _spawnCount = 0;

    public event Action<int> Spawned;

    public int SpawnCount 
    {
        get
        {
            return _spawnCount;
        } 

        protected set
        {
            _spawnCount = value;
            Spawned?.Invoke(value);
        }
    }
    public int InsatncesInPool => ObjectPool.CountAll;
    public int ActiveInPool => ObjectPool.CountActive;

    protected virtual void Awake()
    {
        ObjectPool = new ObjectPool<T>(Create, OnGet, OnRelease, OnClear, PoolSizeAtStart);
    }

    protected virtual T Create()
    {
        T obj = Instantiate(PrefabToSpawn);
        obj.transform.parent = transform;
        obj.ResetObject();
        obj.gameObject.name = $"{PrefabToSpawn.gameObject.name}";
        return obj;
    }

    protected virtual void OnGet(T obj)
    {
        obj.Activate();
    }

    protected virtual void OnRelease(T obj)
    {
        obj.transform.parent = transform;
        obj.ResetObject();
    }

    protected virtual void OnClear(T obj)
    {
        Destroy(obj);
    }
}