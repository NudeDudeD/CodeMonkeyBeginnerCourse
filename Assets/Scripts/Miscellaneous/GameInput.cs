using System;
using UnityEngine;

public class GameInput : MonoBehaviour
{
    private PlayerInputActions _playerActions;

    public event Action PlayerMovementStarted;
    public event Action<Vector2> PlayerMovementPerformed;
    public event Action PlayerMovementCanceled;

    public event Action InteractPerformed;

    private void Awake()
    {
        _playerActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        _playerActions.Player.Enable();

        _playerActions.Player.Move.started += (_) => PlayerMovementStarted?.Invoke();
        _playerActions.Player.Move.canceled += (_) => PlayerMovementCanceled?.Invoke();
        _playerActions.Player.Move.performed += (context) => PlayerMovementPerformed?.Invoke(context.ReadValue<Vector2>());

        _playerActions.Player.Interact.performed += (_) => InteractPerformed?.Invoke();
    }

    private void OnDisable()
    {
        _playerActions.Player.Disable();

        _playerActions.Player.Move.started -= (_) => PlayerMovementStarted?.Invoke();
        _playerActions.Player.Move.canceled -= (_) => PlayerMovementCanceled?.Invoke();
        _playerActions.Player.Move.performed -= (context) => PlayerMovementPerformed?.Invoke(context.ReadValue<Vector2>());

        _playerActions.Player.Interact.performed -= (_) => InteractPerformed?.Invoke();
    }
}