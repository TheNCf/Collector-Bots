using UnityEngine;

public class Crystal : MonoBehaviour, IPoolableObject
{
    public void Activate()
    {
        gameObject.SetActive(true);
    }

    public void ResetObject()
    {
        gameObject.SetActive(false);
    }
}
