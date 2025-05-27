using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour, Controls.IPlayerActions
{
    public Vector2 MovementValue { get; private set; }

    public event Action<Vector2> MoveEvent;
    public event Action<Vector2> BasicAttackEvent;
    public event Action<int > AbilitySwitchEvent;

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

    public void OnAttackBasic(InputAction.CallbackContext context)
    {
        if(!context.performed) return;
        BasicAttackEvent?.Invoke(MovementValue);
    }

    public void OnAbility1(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        AbilitySwitchEvent?.Invoke(1);
    }

    public void OnAbility2(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        AbilitySwitchEvent?.Invoke(2);
    }

    public void OnAbility3(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        AbilitySwitchEvent?.Invoke(3);
    }

    public void OnAbility4(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        AbilitySwitchEvent?.Invoke(4);
    }
}
