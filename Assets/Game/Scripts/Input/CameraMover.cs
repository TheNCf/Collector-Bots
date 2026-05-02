using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class CameraMover : MonoBehaviour
{
    [SerializeField] private Camera _camera;

    [SerializeField] private float _maxSpeed = 7.0f;
    [SerializeField] private float _mouseSensivity = 0.1f;
    [SerializeField] private float _zoomSensivity = 0.1f;
    [SerializeField] private float _smoothTime = 3.0f;
    [SerializeField] private AnimationCurve _zoomHeight;
    [SerializeField] private AnimationCurve _zoomAngle;

    private InputActions _input;

    private Transform _target;

    private Vector2 _direction;
    private bool _canRotate = false;
    private float _zoomValue = 0.5f;

    private Vector3 _cameraTargetPosition;
    private Vector3 _cameraTargetRotation;

    private void Awake()
    {
        _input = new InputActions();

        _target = new GameObject().transform;
        _target.transform.position = transform.position;

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

        _target.Translate(velocity, Space.Self);

        transform.position = Vector3.Lerp(transform.position, _target.position, Time.deltaTime * _smoothTime);
        _camera.transform.localPosition = Vector3.Lerp(_camera.transform.localPosition, _cameraTargetPosition, Time.deltaTime * _smoothTime);
        _camera.transform.localEulerAngles = Vector3.Lerp(_camera.transform.localEulerAngles, _cameraTargetRotation, Time.deltaTime * _smoothTime);
    }

    private void Rotate(InputAction.CallbackContext context)
    {
        if (_canRotate == false)
            return;

        Vector2 mouseDelta = context.ReadValue<Vector2>() * _mouseSensivity;

        transform.Rotate(0, mouseDelta.x, 0);
        _target.Rotate(0, mouseDelta.x, 0);
    }

    private void SetZoom()
    {
        _cameraTargetPosition = _camera.transform.localPosition;
        _cameraTargetPosition.y = _zoomHeight.Evaluate(_zoomValue);
        _cameraTargetRotation = _camera.transform.localEulerAngles;
        _cameraTargetRotation.x = _zoomAngle.Evaluate(_zoomValue);
    }
}
