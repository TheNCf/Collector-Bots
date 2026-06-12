using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Camera))]
public class PlayerInteractor : MonoBehaviour
{
    [SerializeField] private LayerMask _groundLayerMask;
    [SerializeField] private LayerMask _baseLayerMask;

    private Camera _camera;

    private InputActions _input;

    private BotBase _selectedBase;
    private Flag _flagToPlace;

    private Vector2 _mousePositionOnScreen;

    private bool _canPlaceFlag = false;

    private void Awake()
    {
        _camera = GetComponent<Camera>();

        _input = new InputActions();
    }

    private void Update()
    {
        if (_flagToPlace == null)
            return;

        Ray ray = _camera.ScreenPointToRay(_mousePositionOnScreen);

        if (Physics.Raycast(ray, out RaycastHit hitInfo) == false || hitInfo.collider.TryGetComponent(out Ground _) == false)
        {
            _flagToPlace.transform.position = _selectedBase.transform.position;
            _canPlaceFlag = false;
            return;
        }

        _canPlaceFlag = true;
        _flagToPlace.transform.position = hitInfo.point;
    }

    private void OnEnable()
    {
        _input.Enable();
        _input.Player.PrimaryAction.performed += OnPrimaryAction;
        _input.Player.MousePosition.performed += OnMousePositionChanged;
        _input.Player.Cancel.performed += OnCancel;
    }

    private void OnDisable()
    {
        _input.Disable();
        _input.Player.PrimaryAction.performed -= OnPrimaryAction;
        _input.Player.MousePosition.performed -= OnMousePositionChanged;
        _input.Player.Cancel.performed -= OnCancel;
    }

    private void OnMousePositionChanged(InputAction.CallbackContext context)
    {
        _mousePositionOnScreen = context.ReadValue<Vector2>();
    }

    private void OnPrimaryAction(InputAction.CallbackContext context)
    {
        if (_flagToPlace == null)
            InteractWithBase();
        else
            TryPlaceFlag();
    }

    private void OnCancel(InputAction.CallbackContext context)
    {
        if (_flagToPlace == null)
            return;

        _selectedBase.RemoveCurrentFlag();
        _flagToPlace = null;
    }

    private void InteractWithBase()
    {
        Ray ray = _camera.ScreenPointToRay(_mousePositionOnScreen);

        Debug.DrawRay(ray.origin, ray.direction * 100, Color.red, 5.0f);

        if (Physics.Raycast(ray, out RaycastHit hitInfo, float.MaxValue, _baseLayerMask) == false)
            return;

        if (hitInfo.collider.TryGetComponent(out BotBase crystalBase) == false)
            return;

        if (crystalBase.CanExpand == false)
            return;

        _selectedBase = crystalBase;

        if (crystalBase.FocusOnExpand == true)
            _selectedBase.RemoveCurrentFlag();
        else
            _flagToPlace = _selectedBase.CreateFlag();
    }

    private void TryPlaceFlag()
    {
        if (_canPlaceFlag == false)
            return;

        Ray ray = _camera.ScreenPointToRay(_mousePositionOnScreen);

        if (Physics.Raycast(ray, out RaycastHit hitInfo, float.MaxValue, _groundLayerMask) == false)
            return;

        if (_selectedBase == null)
            return;

        if (hitInfo.collider.TryGetComponent(out Ground ground) == false)
            return;

        _flagToPlace.Drop();
        _flagToPlace = null;
    }
}
