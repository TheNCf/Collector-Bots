using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CrystalResourceView : MonoBehaviour
{
    [SerializeField] private Base _base;
    [SerializeField] private TextMeshProUGUI _text;

    private void OnEnable()
    {
        _base.CrystalResourceChanged += Render;
    }

    private void OnDisable()
    {
        _base.CrystalResourceChanged -= Render;
    }

    private void Render(int crystals)
    {
        _text.text = crystals.ToString();
    }
}
