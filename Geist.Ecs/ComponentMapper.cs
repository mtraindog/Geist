// Copyright (c) 2024 Gm.Engine
// Author: Eric Boyd
// Licensed under the MIT License.
// See https://github.com/mtraindog/Gm.Engine/blob/main/LICENSE for details.

using System.Runtime.InteropServices;
using Geist.Ecs.Collections;
using System.Runtime.CompilerServices;

namespace Geist.Ecs;

/// <summary>
/// Manages component storage and retrieval for entities in the ECS system.
/// Uses a sparse array implementation for efficient entity-component mapping.
/// </summary>
internal class ComponentMapper(int capacity)
{
    int _denseCap = capacity;
    int _sparseCap = capacity;
    int _tail = -1;
    int _hiId = -1;
    int _nextCompMask = -1;
    int[] _sparse = new int[capacity];
    Entity[] _dense = new Entity[capacity];    
    readonly List<(Type Type, ulong Mask)> _compTypeMasks = [];
    readonly Dictionary<Type, ulong> _typeMaskLookup = [];
    readonly DistinctBag<Entity> _qBufferEnts = new();
    readonly List<Type> _qBufferTypes = [];

    /// <summary>
    /// Gets a read-only span of all entities in the component mapper.
    /// </summary>
    internal ReadOnlySpan<Entity> Entities 
        => _dense.AsSpan()[..Count];

    /// <summary>
    /// Gets the current number of entities in the component mapper.
    /// </summary>
    internal int Count
        => _tail + 1;

    /// <summary>
    /// Clears all entities from the component mapper, resetting it to its initial state.
    /// </summary>
    internal void Clear()
        => _tail = _hiId = -1;

    /// <summary>
    /// Gets or sets the function used to calculate the new capacity when the dense array needs to grow.
    /// </summary>
    internal Func<int, int> GrowDense { get; set; } = cap
        => (cap + 1) * 2;  

    /// <summary>
    /// Gets or sets the function used to calculate the new capacity when the sparse array needs to grow.
    /// </summary>
    internal Func<int, int, int> GrowSparse { get; set; } = (id, cap)
        => (id + 1) * 2;

    /// <summary>
    /// Adds a new entity to the component mapper.
    /// </summary>
    /// <param name="id">The unique identifier of the entity to add.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal void AddEntity(int id)
    {
        if (id > _hiId)
            _hiId = id;

        _dense[++_tail] = new Entity(id, 0);
        _sparse[id] = _tail;
    }

    /// <summary>
    /// Adds a new entity to the component mapper, resizing internal arrays if necessary.
    /// </summary>
    /// <param name="id">The unique identifier of the entity to add.</param>
    internal void AddEntityWithResize(int id)
    {
        if (id >= _sparseCap - 1)
            Array.Resize(ref _sparse, _sparseCap = GrowSparse(id, _sparseCap));

        if (_tail == _denseCap - 1)
            Array.Resize(ref _dense, _denseCap = GrowDense(_denseCap));
        
        if (id > _hiId)
            _hiId = id;

        _dense[++_tail] = new Entity(id, 0);
        _sparse[id] = _tail;
    }    

    /// <summary>
    /// Checks if an entity exists in the component mapper and retrieves its index.
    /// </summary>
    /// <param name="id">The unique identifier of the entity to check.</param>
    /// <param name="idx">The index of the entity if found, -1 otherwise.</param>
    /// <returns>True if the entity exists, false otherwise.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal bool ContainsEntity(int id, out int idx)
    {
        if (Count == 0 || id > _hiId || id > _sparseCap - 1 || (idx = _sparse[id]) > _tail)
            idx = -1;

        return idx > -1;
    }

    /// <summary>
    /// Removes an entity from the component mapper.
    /// </summary>
    /// <param name="id">The unique identifier of the entity to remove.</param>
    /// <returns>True if the entity was removed, false if it didn't exist.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal bool RemoveEntity(int id)
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

    /// <summary>
    /// Registers a new component type in the system.
    /// </summary>
    /// <typeparam name="T">The type of component to register.</typeparam>
    /// <exception cref="InvalidOperationException">Thrown when the maximum number of component types (64) is reached.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal void RegisterComponentType<T>() where T : IComponent
    {
        if (_nextCompMask == 63)
            throw new InvalidOperationException("Maximum number of component types (64) reached. Cannot register additional components.");

        Type type = typeof(T);
        ulong mask = 1UL << ++_nextCompMask;
        _typeMaskLookup.Add(type, mask);
        _compTypeMasks.Add((type, mask));
    }

    /// <summary>
    /// Adds a component of type T to an entity.
    /// </summary>
    /// <typeparam name="T">The type of component to add.</typeparam>
    /// <param name="id">The unique identifier of the entity.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal void AddComponent<T>(int id) where T : IComponent
        => _dense[_sparse[id]].Components |= _typeMaskLookup[typeof(T)];

    /// <summary>
    /// Checks if an entity has a component of type T.
    /// </summary>
    /// <typeparam name="T">The type of component to check for.</typeparam>
    /// <param name="id">The unique identifier of the entity.</param>
    /// <param name="idx">The index of the entity if found, -1 otherwise.</param>
    /// <returns>True if the entity has the component, false otherwise.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal bool HasComponent<T>(int id, out int idx) where T : IComponent
    {
        if (!ContainsEntity(id, out idx))
            return false;

        return (_dense[idx].Components & _typeMaskLookup[typeof(T)]) != 0;
    }

    /// <summary>
    /// Checks if an entity has all components specified by the mask.
    /// </summary>
    /// <param name="id">The unique identifier of the entity.</param>
    /// <param name="mask">The bitmask representing the required components.</param>
    /// <returns>True if the entity has all required components, false otherwise.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal bool HasComponents(int id, ulong mask)
        => (_dense[_sparse[id]].Components & mask) == mask;

    /// <summary>
    /// Removes a component of type T from an entity.
    /// </summary>
    /// <typeparam name="T">The type of component to remove.</typeparam>
    /// <param name="id">The unique identifier of the entity.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal void RemoveComponent<T>(int id) where T : IComponent
        => _dense[_sparse[id]].Components &= ~_typeMaskLookup[typeof(T)];

    /// <summary>
    /// Removes all components specified by the mask from an entity.
    /// </summary>
    /// <param name="id">The unique identifier of the entity.</param>
    /// <param name="mask">The bitmask representing the components to remove.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal void RemoveComponents(int id, ulong mask)
        => _dense[_sparse[id]].Components ^= mask;

    /// <summary>
    /// Gets the bitmask for a component type.
    /// </summary>
    /// <typeparam name="T">The type of component.</typeparam>
    /// <returns>The bitmask representing the component type.</returns>
    internal ulong GetMask<T>() where T : IComponent
        => _typeMaskLookup[typeof(T)];

    /// <summary>
    /// Calculates a combined bitmask for multiple component types.
    /// </summary>
    /// <param name="types">The types of components to include in the mask.</param>
    /// <returns>A bitmask representing all specified component types.</returns>
    internal ulong CalculateMask(ReadOnlySpan<Type> types)
    {
        ulong mask = 0;
        
        for (int i = 0; i < types.Length; i++)
        {
            mask |= _typeMaskLookup[types[i]];
        }

        return mask;
    }

    /// <summary>
    /// Gets all entities that have the components specified by the mask.
    /// </summary>
    /// <param name="mask">The bitmask representing the required components.</param>
    /// <returns>A read-only span of entities that have all specified components.</returns>
    internal ReadOnlySpan<Entity> GetEntities(ulong mask)
    {
        _qBufferEnts.Clear();

        for (int i = 0; i < Count; i++)
        {
            if ((_dense[i].Components & mask) == mask)
                _qBufferEnts.Add(_dense[i]);
        }

        return _qBufferEnts.Values;
    }

    /// <summary>
    /// Gets all component types attached to an entity.
    /// </summary>
    /// <param name="id">The unique identifier of the entity.</param>
    /// <returns>A read-only span of component types attached to the entity.</returns>
    internal ReadOnlySpan<Type> GetEntityComponentTypes(int id)
    {
        _qBufferTypes.Clear();

        if (!ContainsEntity(id, out int idx))
            return [];

        ulong mask = _dense[idx].Components;

        for (int i = 0; i < _compTypeMasks.Count; i++)
        {
            if ((mask & _compTypeMasks[i].Mask) != 0)
                _qBufferTypes.Add(_compTypeMasks[i].Type);
        }

        return CollectionsMarshal.AsSpan(_qBufferTypes);
    }
}
