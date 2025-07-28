using System;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]
public class PlayerMovement : MonoBehaviour
{
    private const int STEPS_LIMIT = 10;
    private const float SKIN_WIDTH = 0.01f;

    [SerializeField] private GameInput _gameInput;
    [SerializeField] private float _movementSpeed = 1f;
    [SerializeField] private float _rotateSpeed = 1f;
    private float _height;
    private float _radius;
    private bool _isWalking = false;
    public event Action<bool> OnWalking;

    public bool IsWalking
    {
        get => _isWalking;
        set
        {
            if (_isWalking == value)
                return;
            _isWalking = value;
            OnWalking?.Invoke(_isWalking);
        }
    }

    private void Awake()
    {
        CapsuleCollider capsuleCollider = GetComponent<CapsuleCollider>();
        _height = capsuleCollider.height;
        _radius = capsuleCollider.radius;
    }

    private void FixedUpdate()
    {
        Vector2 movementInput = _gameInput.GetPlayerMovement();
        IsWalking = movementInput.magnitude != 0f;

        movementInput = movementInput.normalized;
        Vector3 movementDirection = new Vector3(movementInput.x, 0f, movementInput.y);
        transform.forward = Vector3.Slerp(transform.forward, movementDirection, _rotateSpeed * Time.fixedDeltaTime);
        Vector3 movement = _movementSpeed * Time.fixedDeltaTime * movementDirection;
        transform.position += Move(movement);
    }

    private Vector3 Move(Vector3 movement, int step = 0)
    {
        if (step >= STEPS_LIMIT)
            return Vector3.zero;
        if (Physics.CapsuleCast(transform.position, transform.position + Vector3.up * _height, _radius, movement.normalized, out RaycastHit hit, movement.magnitude))
        {
            Vector3 untilHit = movement.normalized * (hit.distance - SKIN_WIDTH);
            Vector3 afterHit = movement - untilHit;
            Vector3 alongNormal = afterHit - Vector3.Dot(afterHit, hit.normal) * hit.normal;
            if (alongNormal.magnitude == 0)
                return untilHit;
            return untilHit + Move(alongNormal, step++);
        }
        else
            return movement;
    }
}