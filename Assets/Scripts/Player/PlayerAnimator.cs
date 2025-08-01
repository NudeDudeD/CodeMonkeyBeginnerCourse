using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimator : MonoBehaviour
{
    private static readonly string IS_WALKING = "IsWalking";

    [SerializeField] private PlayerMovement _playerMovement;
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();

        OnWalking(_playerMovement.IsWalking.Value);
    }

    private void OnEnable()
    {
        _playerMovement.IsWalking.Changed += (_, isWalking) => OnWalking(isWalking);
    }

    private void OnDisable()
    {
        _playerMovement.IsWalking.Changed -= (_, isWalking) => OnWalking(isWalking);
    }

    private void OnWalking(bool isWalking)
    {
        _animator.SetBool(IS_WALKING, isWalking);
    }
}