using System;
using UnityEngine;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance { get; private set; }

    public event EventHandler OnJumpPerformed; 

    private InputSystem_Actions input; 

    private void Awake()
    {
        Instance = this;

        input = new InputSystem_Actions();
        input.Player.Enable();

        input.Player.Jump.performed += Jump_performed;
    }

    private void Jump_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnJumpPerformed?.Invoke(this, EventArgs.Empty);  
    }

    private void OnDestroy()
    {
        input.Player.Jump.performed -= Jump_performed;

        input.Dispose();
    }

    public Vector2 GetMovementVectorNormalized()
    {
        Vector2 moveInput = input.Player.Move.ReadValue<Vector2>();
        return moveInput.normalized;
    }

    public Vector2 GetLookVector()
    {
        Vector2 lookInput = input.Player.Look.ReadValue<Vector2>();
        return lookInput; 
    }

    public bool IsSprintPressed()
    {
        return input.Player.Sprint.IsPressed(); 
    }
}
