using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMover : MonoBehaviour
{
    [SerializeField] private InputRegisterer _input;

    [SerializeField] private float _maxSpeed = 7.0f;
    [SerializeField] private AnimationCurve _zoomHeight;
    [SerializeField] private AnimationCurve _zoomAngle;

    private float _zoomValue = 0.5f;

    private void OnEnable()
    {
        _input.ZoomChanged += Zoom;
    }

    private void OnDisable()
    {
        _input.ZoomChanged -= Zoom;
    }

    private void Update()
    {
        Move(_input.InputDirection);
    }

    private void Move(Vector2 direction)
    {
        Vector3 velocity = new Vector3(direction.x, 0, direction.y) * _maxSpeed * Time.deltaTime;

        transform.Translate(velocity, Space.World);
    }

    private void Zoom(float zoomDelta)
    {
        _zoomValue += zoomDelta * Time.deltaTime;
        _zoomValue = Mathf.Clamp01(_zoomValue);

        Vector3 position = transform.position;
        position.y = _zoomHeight.Evaluate(_zoomValue);
        Vector3 rotation = transform.localEulerAngles;
        rotation.x = _zoomAngle.Evaluate(_zoomValue);

        transform.position = position;
        transform.localEulerAngles = rotation;
    }
}
