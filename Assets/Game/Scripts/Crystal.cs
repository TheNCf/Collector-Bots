using System;
using UnityEngine;

public class Crystal : MonoBehaviour, IPoolableObject
{
    public event Action<Crystal> Catched;

    public bool IsTargeted { get; private set; } = false;

    public void SetCatched()
    {
        Catched?.Invoke(this);
    }

    public void SetTargeted()
    {
        IsTargeted = true;
    }

    public void Activate()
    {
        gameObject.SetActive(true);
    }

    public void ResetObject()
    {
        gameObject.SetActive(false);
        IsTargeted = false;
        Catched = null;
    }
}
