using UnityEngine;

public class RaycastSelector : MonoBehaviour
{
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private float _maxDistance;

    private readonly ReactiveVariable<RaycastHit> _lastHit = new();

    public IReadOnlyReactiveVariable<RaycastHit> LastHit => _lastHit;

    private void FixedUpdate()
    {
        if (!Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, _maxDistance, _layerMask))
        {
            _lastHit.Value = default;
            return;
        }

        if (hit.collider != _lastHit.Value.collider)
            _lastHit.Value = hit;
    }
}