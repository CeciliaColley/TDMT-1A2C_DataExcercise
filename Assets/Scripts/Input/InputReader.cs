using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputReader : MonoBehaviour
{
    [SerializeField] private InputActionAsset inputActions;
    [SerializeField] private string moveActionName = "Move";
    [SerializeField] private string runActionName = "Run";

    public static event Action<Vector2> OnMove;
    public static event Action<bool> OnRun;

    static InputReader Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        var moveAction = inputActions.FindAction(moveActionName);
        if (moveAction != null)
        {
            moveAction.performed += HandleMoveInput;
            moveAction.canceled += HandleMoveInput;
        }
        var runAction = inputActions.FindAction(runActionName);
        if (runAction != null)
        {
            runAction.started += HandleRunInputStarted;
            runAction.canceled += HandleRunInputCanceled;
        }
    }

    private void HandleRunInputStarted(InputAction.CallbackContext ctx)
    {
        //TODO: Implement event logic
        bool isRunning = ctx.ReadValueAsButton();
        OnRun?.Invoke(isRunning);

        Debug.Log($"{name}: Run input started");
    }

    private void HandleRunInputCanceled(InputAction.CallbackContext ctx)
    {
        //TODO: Implement event logic
        OnRun?.Invoke(false);
        Debug.Log($"{name}: Run input canceled");
    }

    private void HandleMoveInput(InputAction.CallbackContext ctx)
    {
        Vector2 moveInput = ctx.ReadValue<Vector2>();
       
        //TODO: Implement event logic
        OnMove?.Invoke(moveInput);
    }
}
