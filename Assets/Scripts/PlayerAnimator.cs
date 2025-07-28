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
        _playerMovement.OnWalking += OnWalking;

        OnWalking(_playerMovement.IsWalking);
    }

    private void OnWalking(bool isWalking)
    {
        _animator.SetBool(IS_WALKING, _playerMovement.IsWalking);
    }
}