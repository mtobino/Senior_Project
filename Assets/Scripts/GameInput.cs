using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    // Old System
    //private PlayerInputActions playerInputActions;

    public event EventHandler OnCrouchAction;
    public event EventHandler OnJumpAction;
    public event EventHandler OnSprintStart;
    public event EventHandler OnSprintEnd;
    public event EventHandler OnFlashlightToggle;
    public event EventHandler OnCameraToggle;
    public event EventHandler OnAbilityUse;
    public event EventHandler OnMenuToggle;

    // New System
    public static GameInput instance;
    public PlayerInput playerInput { get; private set; }

    public Vector2 MoveInput { get; private set; }
    public bool JumpPressed { get; private set; }
    public bool SprintPressed { get; private set; }
    public bool SprintReleased { get; private set; }
    public bool MenuTogglePressed { get; private set; }
    public bool CameraTogglePressed { get; private set; }
    public bool AbilityPressed { get; private set; }
    public bool FlashlightPressed { get; private set; }
    public bool CrouchPressed { get; private set; }


    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction sprintAction;
    private InputAction menuToggleAction;
    private InputAction cameraToggleAction;
    private InputAction abilityAction;
    private InputAction flashlightAction;
    private InputAction crouchAction;
    // Basically on program start, initialize player input actions and enable input
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        playerInput = GetComponent<PlayerInput>();

        SetupInputActions();
        
    }
    private void Update()
    {
        UpdateInputs();
        FireEvents();
    }
    private void FireEvents()
    {
        if (JumpPressed)
        {
            OnJumpAction?.Invoke(this, EventArgs.Empty);
        }
        if (SprintPressed)
        {
            OnSprintStart?.Invoke(this, EventArgs.Empty);
        }
        if (SprintReleased)
        {
            OnSprintEnd?.Invoke(this, EventArgs.Empty);
        }
        if (CrouchPressed)
        {
            OnCrouchAction?.Invoke(this, EventArgs.Empty);
        }
        if (FlashlightPressed)
        {
            OnFlashlightToggle?.Invoke(this, EventArgs.Empty);
        }
        if (AbilityPressed)
        {
            OnAbilityUse?.Invoke(this, EventArgs.Empty);
        }
        if (MenuTogglePressed)
        {
            OnMenuToggle?.Invoke(this, EventArgs.Empty);
        }
        if (CameraTogglePressed)
        {
            OnCameraToggle?.Invoke(this, EventArgs.Empty);
        }
    }

    private void SetupInputActions()
    {
        moveAction = playerInput.actions["Move"];
        crouchAction = playerInput.actions["Crouch"];
        jumpAction = playerInput.actions["Jump"];
        sprintAction = playerInput.actions["Sprint"];
        flashlightAction = playerInput.actions["Flashlight"];
        abilityAction = playerInput.actions["Ability"];
        cameraToggleAction = playerInput.actions["Camera Toggle"];
        menuToggleAction = playerInput.actions["Open and Close Menu"];
    }
    private void UpdateInputs()
    {
        MoveInput = GetMovementVectorNormalized();
        JumpPressed = jumpAction.WasPressedThisFrame();
        SprintPressed= sprintAction.WasPressedThisFrame();
        SprintReleased= sprintAction.WasReleasedThisFrame();
        CrouchPressed = crouchAction.WasPressedThisFrame();
        FlashlightPressed = flashlightAction.WasPressedThisFrame();
        AbilityPressed = abilityAction.WasPressedThisFrame();
        CameraTogglePressed= cameraToggleAction.WasPressedThisFrame();
        MenuTogglePressed= menuToggleAction.WasPressedThisFrame();
    }


    public Vector2 GetMovementVectorNormalized()
    {
        Vector2 inputVector = moveAction.ReadValue<Vector2>();

        inputVector = inputVector.normalized;

        return inputVector;
    }



    
}
