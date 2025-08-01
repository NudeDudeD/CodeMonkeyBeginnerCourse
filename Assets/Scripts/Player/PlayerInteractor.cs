using UnityEngine;

[RequireComponent(typeof(RaycastSelector))]
public class PlayerInteractor : MonoBehaviour
{
    [SerializeField] private GameInput _gameInput;
    private RaycastSelector _objectSelector;
    private Counter _selectedCounter;

    private void Awake()
    {
        _objectSelector = GetComponent<RaycastSelector>();
    }

    private void OnEnable()
    {
        _gameInput.InteractPerformed += Interact;
        _objectSelector.LastHit.Changed += (_, hit) => SetCounter(hit);
    }

    private void OnDisable()
    {
        _gameInput.InteractPerformed -= Interact;
        _objectSelector.LastHit.Changed -= (_, hit) => SetCounter(hit);
    }

    private void SetCounter(RaycastHit hit)
    {
        if (_selectedCounter != null)
            _selectedCounter.Interactors.Remove(this);
        if (hit.collider != null && hit.collider.gameObject.TryGetComponent(out Counter counter))
        {
            _selectedCounter = counter;
            _selectedCounter.Interactors.Add(this);
        }
        else
            _selectedCounter = null;
    }

    private void Interact()
    {
        Debug.Log($"Interact check: {_selectedCounter}");
    }
}