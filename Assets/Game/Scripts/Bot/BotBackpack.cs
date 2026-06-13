using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotBackpack : MonoBehaviour
{
    private GameObject _item;

    public void Put(GameObject item)
    {
        if (_item != null)
            Drop();

        _item = item;

        _item.SetActive(false);
    }

    public GameObject Drop()
    {
        _item.transform.position = transform.position;
        _item.SetActive(true);
        GameObject buffer = _item;
        _item = null;
        return buffer;
    }
}
