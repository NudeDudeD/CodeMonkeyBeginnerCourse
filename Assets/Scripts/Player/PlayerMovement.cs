using System;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]
public class PlayerMovement : MonoBehaviour
{
    private const int STEPS_LIMIT = 10;
    private const float SKIN_WIDTH = 0.1f;

    private readonly ReactiveVariable<bool> _isWalking = new();

    [SerializeField] private GameInput _gameInput;
    [SerializeField] private float _movementSpeed = 1f;
    [SerializeField] private float _rotateSpeed = 1f;
    private Vector3 _heightVector;
    private Vector2 _movementInput;
    private float _radius;

    public IReadOnlyReactiveVariable<bool> IsWalking => _isWalking;

    private void Awake()
    {
        CapsuleCollider capsuleCollider = GetComponent<CapsuleCollider>();
        _heightVector = capsuleCollider.height * Vector3.up;
        _radius = capsuleCollider.radius;
    }

    private void OnEnable()
    {
        _gameInput.PlayerMovementStarted += () => _isWalking.Value = true;
        _gameInput.PlayerMovementCanceled += () => _isWalking.Value = false;
        _gameInput.PlayerMovementPerformed += (movementInput) => _movementInput = movementInput;
    }

    private void OnDisable()
    {
        _gameInput.PlayerMovementStarted -= () => _isWalking.Value = true;
        _gameInput.PlayerMovementCanceled -= () => _isWalking.Value = false;
        _gameInput.PlayerMovementPerformed -= (movementInput) => _movementInput = movementInput;
    }

    private void FixedUpdate()
    {
        if (!_isWalking.Value)
            return;

        Vector3 movementDirection = new Vector3(_movementInput.x, 0f, _movementInput.y).normalized;
        transform.forward = Vector3.Slerp(transform.forward, movementDirection, _rotateSpeed * Time.fixedDeltaTime);

        Vector3 movement = _movementSpeed * Time.fixedDeltaTime * movementDirection;
        transform.position += SlideAndCollide(movement);
    }

    private Vector3 SlideAndCollide(Vector3 movement, int step = 0)
    {
        if (step >= STEPS_LIMIT)
            return Vector3.zero;
        if (Physics.CapsuleCast(transform.position, transform.position + _heightVector, _radius, movement.normalized, out RaycastHit hit, movement.magnitude))
        {
            Vector3 untilHit = movement.normalized * (hit.distance - SKIN_WIDTH);
            Vector3 afterHit = movement - untilHit;
            Vector3 alongNormal = afterHit - Vector3.Dot(afterHit, hit.normal) * hit.normal;
            if (alongNormal.magnitude == 0)
                return untilHit;
            return untilHit + SlideAndCollide(alongNormal, step++);
        }
        else
            return movement;
    }
}