using System;
using UnityEngine;

public class Crystal : MonoBehaviour, IPoolableObject
{
    public bool IsTargeted { get; private set; } = false;

    public event Action<Crystal> Delivered;

    public void OnDelivered()
    {
        Delivered?.Invoke(this);
    }

    public void OnTargeted()
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
        Delivered = null;
    }
}
