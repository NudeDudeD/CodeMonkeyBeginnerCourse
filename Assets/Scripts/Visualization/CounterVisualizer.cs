using UnityEngine;

[RequireComponent(typeof(Counter))]
public class CounterVisualizer : MonoBehaviour
{
    [SerializeField] private RendererHighlighter _highlighter;
    private Counter _counter;

    private void Awake()
    {
        _counter = GetComponent<Counter>();
        _counter.Interactors.ItemAdded += (_) => _highlighter.Highlighted.Value = true;
        _counter.Interactors.Emptied += () => _highlighter.Highlighted.Value = false;
    }
}