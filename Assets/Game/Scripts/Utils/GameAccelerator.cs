using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAccelerator : MonoBehaviour
{
    [SerializeField] private float _gameSpeedMultiplier = 1.0f;

    private void OnValidate()
    {
        Time.timeScale = _gameSpeedMultiplier;
    }

    private void Awake()
    {
        Time.timeScale = _gameSpeedMultiplier;
    }
}
