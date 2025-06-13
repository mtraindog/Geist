// Copyright (c) 2024 Gm.Engine
// Author: Eric Boyd
// Licensed under the MIT License.
// See https://github.com/mtraindog/Gm.Engine/blob/main/LICENSE for details.

namespace Geist.Ecs;
using System.Runtime.CompilerServices;

/// <summary>
/// Implements a sparse array storage system for components in the ECS.
/// Uses a dense array for actual component storage and a sparse array for entity-to-index mapping.
/// </summary>
/// <typeparam name="T">The type of component to store. Must implement IComponent.</typeparam>
public class SparseStorage<T> : IStorage<T>
    where T : IComponent
{
    int _denseCap;
    int _sparseCap;
    int _tail;
    int _hiId;
    T[] _dense;
    int[] _sparse;

    /// <summary>
    /// Gets the current number of components in storage.
    /// </summary>
    public int Count => _tail + 1;

    /// <summary>
    /// Clears all components from storage.
    /// </summary>
    public void Clear() => _tail = _hiId = -1;

    /// <summary>
    /// Gets a reference to the component for a specific entity.
    /// </summary>
    /// <param name="id">The ID of the entity.</param>
    /// <returns>A reference to the component.</returns>
    public ref T this[int id] => ref _dense[_sparse[id]];

    /// <summary>
    /// Gets a span of all components in storage.
    /// </summary>
    /// <returns>A span containing all components in storage.</returns>
    public Span<T> AsSpan() => _dense.AsSpan()[0..Count];

    /// <summary>
    /// Gets or sets the function used to calculate the new capacity when the dense array needs to grow.
    /// </summary>
    public Func<int, int> GrowDense { get; set; } = cap => (cap + 1) * 2;

    /// <summary>
    /// Gets or sets the function used to calculate the new capacity when the sparse array needs to grow.
    /// </summary>
    public Func<int, int, int> GrowSparse { get; set; } = (id, cap) => (id + 1) * 2;

    /// <summary>
    /// Initializes a new instance of the SparseStorage class with the specified initial capacity.
    /// </summary>
    /// <param name="capacity">The initial capacity for both dense and sparse arrays.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public SparseStorage(int capacity)
    {
        _tail = _hiId = -1;
        _dense = new T[_denseCap = capacity];
        _sparse = new int[_sparseCap = capacity];
    }    

    /// <summary>
    /// Adds a component to storage for a specific entity.
    /// </summary>
    /// <param name="id">The ID of the entity to add the component to.</param>
    /// <param name="item">The component to add.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Add(int id, T item)
    {
        if (id > _hiId)
            _hiId = id;

        _dense[++_tail] = item;
        _sparse[id] = _tail;
    }

    /// <summary>
    /// Adds a component to storage for a specific entity, resizing internal arrays if necessary.
    /// </summary>
    /// <param name="id">The ID of the entity to add the component to.</param>
    /// <param name="item">The component to add.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AddWithResize(int id, T item)
    {
        if (id >= _sparseCap - 1)
            Array.Resize(ref _sparse, _sparseCap = GrowSparse(id, _sparseCap));

        if (_tail == _denseCap - 1)
            Array.Resize(ref _dense, _denseCap = GrowDense(_denseCap));

        if (id > _hiId)
            _hiId = id;

        _dense[++_tail] = item;
        _sparse[id] = _tail;
    }    

    /// <summary>
    /// Checks if a component exists for a specific entity and retrieves its index.
    /// </summary>
    /// <param name="id">The ID of the entity to check.</param>
    /// <param name="idx">The index of the component if found, -1 otherwise.</param>
    /// <returns>True if the component exists, false otherwise.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Contains(int id, out int idx)
    {
        if (Count == 0 || id > _hiId || id > _sparseCap - 1 || (idx = _sparse[id]) > _tail)
            idx = -1;

        return idx > -1;
    }

    /// <summary>
    /// Removes a component for a specific entity from storage.
    /// </summary>
    /// <param name="id">The ID of the entity whose component should be removed.</param>
    /// <returns>True if the component was removed, false if it didn't exist.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Remove(int id)
    {
        int idx;

        if (Count == 0 || id > _hiId || id > _sparseCap - 1 || (idx = _sparse[id]) > _tail)
            return false;

        int id2 = _dense[_tail].Id;
        (_dense[idx], _dense[_tail]) = (_dense[_tail], _dense[idx]);
        --_tail;
        _sparse[id2] = idx;
        return true;
    }
}
