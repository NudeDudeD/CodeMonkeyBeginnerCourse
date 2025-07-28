using UnityEngine;

public class GameInput : MonoBehaviour
{
    private PlayerInputActions _playerActions;

    private void Awake()
    {
        _playerActions = new PlayerInputActions();
        _playerActions.Player.Enable();
    }

    public Vector2 GetPlayerMovement()
    {
        Vector2 inputVector = _playerActions.Player.Move.ReadValue<Vector2>();
        return inputVector;
    }
}