using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour, Controls.IPlayerActions
{
    public Vector2 MovementValue { get; private set; }

    public event Action<Vector2> MoveEvent;

    private Controls controls;

    private void Start()
    {
        controls = new Controls();
        controls.Player.SetCallbacks(this);
        controls.Player.Enable();
    }

    private void OnDestroy()
    {
        controls.Player.Disable();
    }

    //public void OnJump(InputAction.CallbackContext context)
    //{
    //    if (!context.performed) return;
    //    JumpEvent?.Invoke();
    //}

    //public void OnDodge(InputAction.CallbackContext context)
    //{
    //    if (!context.performed) return;
    //    DodgeEvent?.Invoke();
    //}

    public void OnPosition(InputAction.CallbackContext context)
    {
        MovementValue = context.ReadValue<Vector2>();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        MoveEvent?.Invoke(MovementValue); // Pass movement value
    }
}
