// Copyright (c) 2024 Gm.Engine
// Author: Eric Boyd
// Licensed under the MIT License.
// See https://github.com/mtraindog/Gm.Engine/blob/main/LICENSE for details.

using System.Runtime.CompilerServices;

namespace Geist.Ecs;

/// <summary>
/// Manages component storage and operations in the ECS system.
/// Provides functionality for registering, adding, removing, and querying components.
/// </summary>
public sealed class ComponentManager(int capacity)
{
    readonly int _initCapacity = capacity;
    readonly ComponentMapper _mapper = new(Math.Clamp(capacity, 0, int.MaxValue));
    readonly Dictionary<Type, IStorage> _compStorage = [];
    internal World World { get; set; }

    /// <summary>
    /// Gets the component mapper instance used for managing entity-component relationships.
    /// </summary>
    internal ComponentMapper Mapper
        => _mapper;

    /// <summary>
    /// Gets the number of components of type T in the system.
    /// </summary>
    /// <typeparam name="T">The type of component to count.</typeparam>
    /// <returns>The number of components of type T.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int Count<T>() where T : IComponent, new()
        => _compStorage[typeof(T)].Count;

    /// <summary>
    /// Gets the storage for components of the specified type.
    /// </summary>
    /// <param name="type">The type of component storage to retrieve.</param>
    /// <returns>The storage instance for the specified component type.</returns>
    public IStorage this[Type type]
        => _compStorage[type];

    /// <summary>
    /// Gets a span of all components of type T in the system.
    /// </summary>
    /// <typeparam name="T">The type of component to retrieve.</typeparam>
    /// <returns>A span containing all components of type T.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Span<T> OfType<T>() where T : IComponent, new()
        => (_compStorage[typeof(T)] as IStorage<T>).AsSpan();

    /// <summary>
    /// Checks if an entity has a component of type T.
    /// </summary>
    /// <typeparam name="T">The type of component to check for.</typeparam>
    /// <param name="id">The ID of the entity to check.</param>
    /// <returns>True if the entity has the component, false otherwise.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Has<T>(int id) where T : IComponent, new()
        => _mapper.HasComponent<T>(id, out _);

    /// <summary>
    /// Gets a component of type T from an entity.
    /// </summary>
    /// <typeparam name="T">The type of component to retrieve.</typeparam>
    /// <param name="id">The ID of the entity.</param>
    /// <returns>The component of type T associated with the entity.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public T Get<T>(int id) where T : IComponent, new()
        => (_compStorage[typeof(T)] as IStorage<T>)[id];

    /// <summary>
    /// Gets a reference to a component of type T from an entity.
    /// </summary>
    /// <typeparam name="T">The type of component to retrieve.</typeparam>
    /// <param name="id">The ID of the entity.</param>
    /// <returns>A reference to the component of type T associated with the entity.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ref T Ref<T>(int id) where T : IComponent, new()
        => ref (_compStorage[typeof(T)] as IStorage<T>)[id];

    /// <summary>
    /// Adds a new component of type T to an entity.
    /// </summary>
    /// <typeparam name="T">The type of component to add.</typeparam>
    /// <param name="id">The ID of the entity.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Add<T>(int id) where T : IComponent, new()
    {
        (_compStorage[typeof(T)] as IStorage<T>).Add(id, new() { Id = id });
        _mapper.AddComponent<T>(id);
    }

    /// <summary>
    /// Adds a specific component instance of type T to an entity.
    /// </summary>
    /// <typeparam name="T">The type of component to add.</typeparam>
    /// <param name="id">The ID of the entity.</param>
    /// <param name="component">The component instance to add.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Add<T>(int id, T component) where T : IComponent, new()
    {
        component.Id = id;
        (_compStorage[typeof(T)] as IStorage<T>).Add(id, component);
        _mapper.AddComponent<T>(id);
    }    

    /// <summary>
    /// Registers a component type T with the default initial capacity.
    /// </summary>
    /// <typeparam name="T">The type of component to register.</typeparam>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Register<T>() where T : IComponent, new()
        => Register<T>(_initCapacity);

    /// <summary>
    /// Registers a component type T with a specific initial capacity.
    /// </summary>
    /// <typeparam name="T">The type of component to register.</typeparam>
    /// <param name="capacity">The initial capacity for storing components of type T.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Register<T>(int capacity) where T : IComponent, new()
    {
        capacity = Math.Clamp(capacity, 0, int.MaxValue);
        _compStorage.Add(typeof(T), new SparseStorage<T>(capacity));
        _mapper.RegisterComponentType<T>();
    }    
    
    /// <summary>
    /// Checks if an entity has a component of type T and retrieves its index.
    /// </summary>
    /// <typeparam name="T">The type of component to check for.</typeparam>
    /// <param name="id">The ID of the entity.</param>
    /// <param name="componentIndex">The index of the component if found, -1 otherwise.</param>
    /// <returns>True if the entity has the component, false otherwise.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Has<T>(int id, out int componentIndex) where T : IComponent, new()
        => _compStorage[typeof(T)].Contains(id, out componentIndex);

    /// <summary>
    /// Gets the index of a component of type T for an entity.
    /// </summary>
    /// <typeparam name="T">The type of component to find.</typeparam>
    /// <param name="id">The ID of the entity.</param>
    /// <returns>The index of the component in its storage.</returns>
    /// <exception cref="IndexOutOfRangeException">Thrown when the component is not found.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int IndexOf<T>(int id) where T : IComponent, new()
    {
        _compStorage[typeof(T)].Contains(id, out int index);
        if (index < 0) throw new IndexOutOfRangeException(nameof(index));
        return index;
    }       

    /// <summary>
    /// Removes a component of type T from an entity.
    /// </summary>
    /// <typeparam name="T">The type of component to remove.</typeparam>
    /// <param name="id">The ID of the entity.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Remove<T>(int id) where T : IComponent, new()
    {
        _compStorage[typeof(T)].Remove(id);
        _mapper.RemoveComponent<T>(id);
    }
}
