using System;
using System.Collections.Generic;

//Source idea: https://www.youtube.com/watch?v=aGr9dGLq5qc
[Serializable]
public class ReactiveList<T> : List<T>
{
    public event Action<T> ItemAdded;
    public event Action<T> ItemRemoved;
    public event Action Emptied;

    public new void Add(T item)
    {
        base.Add(item);
        ItemAdded?.Invoke(item);
    }

    public new void AddRange(IEnumerable<T> collection)
    {
        base.AddRange(collection);
        foreach (T item in collection)
            ItemAdded?.Invoke(item);
    }

    public new void Clear()
    { 
        base.Clear();
        Emptied?.Invoke();
    }

    public new void Insert(int index, T item)
    {
        base.Insert(index, item);
        ItemAdded?.Invoke(item);
    }

    public new void InsertRange(int index, IEnumerable<T> collection)
    {
        base.InsertRange(index, collection);
        foreach (T item in collection)
            ItemAdded?.Invoke(item);
    }

    public new void Remove(T item)
    {
        if (!base.Remove(item))
            return;

        ItemRemoved?.Invoke(item);
        if (Count == 0)
            Emptied?.Invoke();
    }

    public new void RemoveAll(Predicate<T> match)
    {
        List<T> removedItems = FindAll(match);
        if (removedItems.Count == 0)
            return;

        base.RemoveAll(match);
        foreach (T item in removedItems)
            ItemRemoved?.Invoke(item);
        if (Count == 0)
            Emptied?.Invoke();
    }

    public new void RemoveAt(int index)
    {
        T item = base[index];
        base.RemoveAt(index);
        ItemRemoved?.Invoke(item);
        if (Count == 0)
            Emptied?.Invoke();
    }

    public new void RemoveRange(int index, int count)
    {
        List<T> removedItems = GetRange(index, count);
        if (removedItems.Count == 0)
            return;

        base.RemoveRange(index, count);
        foreach (T item in removedItems)
            ItemRemoved?.Invoke(item);
        if (Count == 0)
            Emptied?.Invoke();
    }
}