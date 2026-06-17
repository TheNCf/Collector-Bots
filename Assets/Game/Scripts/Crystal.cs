using System;
using UnityEngine;

public class Crystal : MonoBehaviour, IPoolableObject
{
    public event Action<Crystal> Catched;

    public void SetCatched()
    {
        Catched?.Invoke(this);
    }

    public void Activate()
    {
        gameObject.SetActive(true);
    }

    public void ResetObject()
    {
        gameObject.SetActive(false);
        Catched = null;
    }
}
