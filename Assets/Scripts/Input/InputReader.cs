using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputReader : MonoBehaviour
{
    [Header("Input Actions")]
    [SerializeField] private InputActionReference moveLeftAction;
    [SerializeField] private InputActionReference moveRightAction;
    [SerializeField] private InputActionReference jumpAction;
    [SerializeField] private InputActionReference slideAction;
    [SerializeField] private InputActionReference realmShiftAction;
    [SerializeField] private InputActionReference pointerPressAction;
    [SerializeField] private InputActionReference pointerPositionAction;

    [Header("Swipe Settings")]
    [SerializeField] private float minSwipeDistance = 80f;
    [SerializeField] private float maxTapMovement = 25f;

    public event Action OnMoveLeft;
    public event Action OnMoveRight;
    public event Action OnJump;
    public event Action OnSlide;
    public event Action OnRealmShift;

    private Vector2 pointerStartPosition;
    private bool isPointerPressed;

    private void OnEnable()
    {
        EnableActions();

        moveLeftAction.action.performed += HandleMoveLeft;
        moveRightAction.action.performed += HandleMoveRight;
        jumpAction.action.performed += HandleJump;
        slideAction.action.performed += HandleSlide;
        realmShiftAction.action.performed += HandleRealmShift;

        pointerPressAction.action.started += HandlePointerPressed;
        pointerPressAction.action.canceled += HandlePointerReleased;
    }

    private void OnDisable()
    {
        moveLeftAction.action.performed -= HandleMoveLeft;
        moveRightAction.action.performed -= HandleMoveRight;
        jumpAction.action.performed -= HandleJump;
        slideAction.action.performed -= HandleSlide;
        realmShiftAction.action.performed -= HandleRealmShift;

        pointerPressAction.action.started -= HandlePointerPressed;
        pointerPressAction.action.canceled -= HandlePointerReleased;

        DisableActions();
    }

    private void EnableActions()
    {
        moveLeftAction.action.Enable();
        moveRightAction.action.Enable();
        jumpAction.action.Enable();
        slideAction.action.Enable();
        realmShiftAction.action.Enable();
        pointerPressAction.action.Enable();
        pointerPositionAction.action.Enable();
    }

    private void DisableActions()
    {
        moveLeftAction.action.Disable();
        moveRightAction.action.Disable();
        jumpAction.action.Disable();
        slideAction.action.Disable();
        realmShiftAction.action.Disable();
        pointerPressAction.action.Disable();
        pointerPositionAction.action.Disable();
    }

    private void HandleMoveLeft(InputAction.CallbackContext context)
    {
        OnMoveLeft?.Invoke();
    }

    private void HandleMoveRight(InputAction.CallbackContext context)
    {
        OnMoveRight?.Invoke();
    }

    private void HandleJump(InputAction.CallbackContext context)
    {
        OnJump?.Invoke();
    }

    private void HandleSlide(InputAction.CallbackContext context)
    {
        OnSlide?.Invoke();
    }

    private void HandleRealmShift(InputAction.CallbackContext context)
    {
        OnRealmShift?.Invoke();
    }

    private void HandlePointerPressed(InputAction.CallbackContext context)
    {
        isPointerPressed = true;
        pointerStartPosition = pointerPositionAction.action.ReadValue<Vector2>();
    }

    private void HandlePointerReleased(InputAction.CallbackContext context)
    {
        if (!isPointerPressed) return;

        isPointerPressed = false;

        Vector2 endPosition = pointerPositionAction.action.ReadValue<Vector2>();
        Vector2 delta = endPosition - pointerStartPosition;

        ProcessSwipeOrTap(delta);
    }

    private void ProcessSwipeOrTap(Vector2 delta)
    {
        float distance = delta.magnitude;

        if (distance <= maxTapMovement)
        {
            OnRealmShift?.Invoke();
            return;
        }

        if (distance < minSwipeDistance) return;

        if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
        {
            if (delta.x > 0f)
            {
                OnMoveRight?.Invoke();
            }
            else
            {
                OnMoveLeft?.Invoke();
            }
        }
        else
        {
            if (delta.y > 0f)
            {
                OnJump?.Invoke();
            }
            else
            {
                OnSlide?.Invoke();
            }
        }
    }
}