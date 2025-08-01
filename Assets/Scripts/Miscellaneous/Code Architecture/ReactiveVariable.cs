using System;
using System.Collections.Generic;
using UnityEngine;

//Source (edited): https://www.youtube.com/watch?v=aGr9dGLq5qc
[Serializable]
public class ReactiveVariable<T> : IReadOnlyReactiveVariable<T>
{
    private readonly IEqualityComparer<T> _comparer;

    [SerializeField] private T _value;

    public event Action<T, T> Changed;

    public ReactiveVariable() : this(default, EqualityComparer<T>.Default) { }
    public ReactiveVariable(T value) : this(value, EqualityComparer<T>.Default) { }

    public ReactiveVariable(T value, IEqualityComparer<T> comparer)
    {
        _value = value;
        _comparer = comparer;
    }

    public T Value
    {
        get => _value;
        set
        {
            if (_comparer.Equals(_value, value))
                return;

            T oldValue = _value;
            _value = value;
            Changed?.Invoke(oldValue, value);
        }
    }

    //public static implicit operator T(ReactableVariable<T> obj) => obj.Value;
}