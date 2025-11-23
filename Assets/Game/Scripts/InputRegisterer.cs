using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputRegisterer : MonoBehaviour
{
    private InputActions _input;

    private Vector2 _inputDirection;
    private float _inputZoom;
    private float _inputZoomScale = 0.1f;

    public event Action<float> ZoomChanged;

    public Vector2 InputDirection => _inputDirection;
    public float InputZoom => _inputZoom;

    private void Awake()
    {
        _input = new InputActions();
    }

    private void OnEnable()
    {
        _input.Enable();
        _input.Player.Move.performed += OnMove;
        _input.Player.Move.canceled += OnMove;
        _input.Player.Zoom.performed += OnZoom;
        _input.Player.Zoom.canceled += OnZoom;
    }

    private void OnDisable()
    {
        _input.Player.Move.performed -= OnMove;
        _input.Player.Move.canceled -= OnMove;
        _input.Player.Zoom.performed -= OnZoom;
        _input.Player.Zoom.canceled -= OnZoom;
        _input.Disable();
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        _inputDirection = context.ReadValue<Vector2>();
    }

    private void OnZoom(InputAction.CallbackContext context)
    {
        _inputZoom = context.ReadValue<float>();
        ZoomChanged?.Invoke(_inputZoom * _inputZoomScale);
    }
}
