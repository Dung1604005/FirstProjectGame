using System.Collections;
using System.Collections.Generic;


public class PriorityQueue<T>
{
    private readonly List<(T Item, int Priority)> _heap = new List<(T, int)>();

    public int Count => _heap.Count;
    public bool IsEmpty => _heap.Count == 0;

    public void Enqueue(T item, int priority)
    {
        _heap.Add((item, priority));
        HeapifyUp(_heap.Count - 1);
    }

    public T Dequeue()
    {
        if (_heap.Count == 0)
            throw new System.Exception("PriorityQueue is empty");

        T result = _heap[0].Item;

        int lastIndex = _heap.Count - 1;
        _heap[0] = _heap[lastIndex];
        _heap.RemoveAt(lastIndex);

        if (_heap.Count > 0)
            HeapifyDown(0);

        return result;
    }

    public T Peek()
    {
        if (_heap.Count == 0)
            throw new System.Exception("PriorityQueue is empty");

        return _heap[0].Item;
    }

    private void HeapifyUp(int index)
    {
        while (index > 0)
        {
            int parent = (index - 1) / 2;
            if (_heap[index].Priority >= _heap[parent].Priority)
                break;

            Swap(index, parent);
            index = parent;
        }
    }

    private void HeapifyDown(int index)
    {
        int count = _heap.Count;

        while (true)
        {
            int left = index * 2 + 1;
            int right = index * 2 + 2;
            int smallest = index;

            if (left < count && _heap[left].Priority < _heap[smallest].Priority)
                smallest = left;

            if (right < count && _heap[right].Priority < _heap[smallest].Priority)
                smallest = right;

            if (smallest == index)
                break;

            Swap(index, smallest);
            index = smallest;
        }
    }

    private void Swap(int i, int j)
    {
        var tmp = _heap[i];
        _heap[i] = _heap[j];
        _heap[j] = tmp;
    }
}
