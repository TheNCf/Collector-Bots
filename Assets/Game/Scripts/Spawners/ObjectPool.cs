using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ObjectPool<T> where T : UnityEngine.Object, IPoolableObject
{
    private readonly List<T> _pooledObjectList;

    private int _pooledAtStart;
    private int _countAll;

    private Func<T> _createFunction;
    private Action<T> _onGetAction;
    private Action<T> _onReleaseAction;
    private Action<T> _onClearAction;

    public ObjectPool(Func<T> createFunction, Action<T> onGetAction, Action<T> onReleaseAction, Action<T> onClearAction, int pooledAtStart)
    {
        _createFunction = createFunction;
        _onGetAction = onGetAction;
        _onReleaseAction = onReleaseAction;
        _onClearAction = onClearAction;
        _pooledAtStart = pooledAtStart;

        _pooledObjectList = new List<T>();
        Initialize();
    }

    public int CountAll => _countAll;
    public int CountInactive => _pooledObjectList.Count;
    public int CountActive => CountAll - CountInactive;

    public T Get()
    {
        T result;

        if (CountInactive == 0)
        {
            result = _createFunction();
            _countAll++;
        }
        else
        {
            int index = CountInactive - 1;
            result = _pooledObjectList[index];
            _pooledObjectList.RemoveAt(index);
        }

        _onGetAction?.Invoke(result);
        return result;
    }

    public void Release(T obj)
    {
        if (CountInactive > 0)
            foreach (var item in _pooledObjectList)
                if (item == obj)
                    throw new InvalidOperationException("Trying to release already released object!");

        _pooledObjectList.Add(obj);
        _onReleaseAction?.Invoke(obj);
    }

    public void Clear()
    {
        if (_onClearAction != null)
            foreach (var obj in _pooledObjectList)
                _onClearAction?.Invoke(obj);

        _countAll = 0;
        _pooledObjectList.Clear();
    }

    private void Initialize()
    {
        T buffer;

        for (int i = 0; i < _pooledAtStart; i++)
        {
            buffer = _createFunction();
            _pooledObjectList.Add(buffer);
            _countAll++;
        }
    }
}
