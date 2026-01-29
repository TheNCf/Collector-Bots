using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMover : MonoBehaviour
{
    [SerializeField] private Camera _camera;

    [SerializeField] private float _maxSpeed = 7.0f;
    [SerializeField] private float _mouseSensivity = 0.1f;
    [SerializeField] private float _zoomSensivity = 0.1f;
    [SerializeField] private AnimationCurve _zoomHeight;
    [SerializeField] private AnimationCurve _zoomAngle;

    private InputActions _input;

    private Vector2 _direction;
    private bool _canRotate = false;
    private float _zoomValue = 0.5f;

    private void Awake()
    {
        _input = new InputActions();
        SetZoom();
    }

    private void OnEnable()
    {
        _input.Enable();
        _input.Player.Move.performed += OnMove;
        _input.Player.Move.canceled += OnMove;
        _input.Player.AlternativeAction.performed += OnAlternativeAction;
        _input.Player.AlternativeAction.canceled += OnAlternativeAction;
        _input.Player.MouseMove.performed += Rotate;
        _input.Player.Zoom.performed += OnZoom;
    }

    private void OnDisable()
    {
        _input.Disable();
        _input.Player.Move.performed -= OnMove;
        _input.Player.Move.canceled -= OnMove;
        _input.Player.AlternativeAction.performed -= OnAlternativeAction;
        _input.Player.AlternativeAction.canceled -= OnAlternativeAction;
        _input.Player.MouseMove.performed -= Rotate;
        _input.Player.Zoom.performed -= OnZoom;
    }

    private void Update()
    {
        Move();
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        _direction = context.ReadValue<Vector2>();
    }

    private void OnAlternativeAction(InputAction.CallbackContext context)
    {
        _canRotate = context.ReadValueAsButton();
    }

    private void OnZoom(InputAction.CallbackContext context)
    {
        float zoomDelta = context.ReadValue<float>() * _zoomSensivity;
        _zoomValue += zoomDelta * Time.deltaTime;
        _zoomValue = Mathf.Clamp01(_zoomValue);

        SetZoom();
    }

    private void Move()
    {
        Vector3 velocity = new Vector3(_direction.x, 0, _direction.y) * _maxSpeed * Time.deltaTime;

        transform.Translate(velocity, Space.Self);
    }

    private void Rotate(InputAction.CallbackContext context)
    {
        if (_canRotate == false)
            return;

        Vector2 mouseDelta = context.ReadValue<Vector2>() * _mouseSensivity;

        transform.Rotate(0, mouseDelta.x, 0);
    }

    private void SetZoom()
    {
        Vector3 position = _camera.transform.position;
        position.y = _zoomHeight.Evaluate(_zoomValue);
        Vector3 rotation = _camera.transform.localEulerAngles;
        rotation.x = _zoomAngle.Evaluate(_zoomValue);

        _camera.transform.position = position;
        _camera.transform.localEulerAngles = rotation;
    }
}
