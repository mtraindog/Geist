// Copyright (c) 2024 Gm.Engine
// Author: Eric Boyd
// Licensed under the MIT License.
// See https://github.com/mtraindog/Gm.Engine/blob/main/LICENSE for details.

namespace Geist.Ecs;
using System.Runtime.CompilerServices;

/// <summary>
/// Provides a fluent interface for iterating and manipulating entities in the ECS system.
/// Supports chaining operations for entity creation, component management, and entity destruction.
/// </summary>
public class EntityIterator
{
    /// <summary>
    /// Initializes a new instance of the EntityIterator class.
    /// </summary>
    public EntityIterator() { }

    /// <summary>
    /// Gets the world instance that this iterator belongs to.
    /// </summary>
    public World World { get; internal init; }

    /// <summary>
    /// Gets the current entity ID being iterated.
    /// </summary>
    public int Id { get; private set; }

    /// <summary>
    /// Moves the iterator to the next entity with the specified ID.
    /// </summary>
    /// <param name="id">The ID of the entity to move to.</param>
    /// <returns>The current iterator instance for method chaining.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public EntityIterator MoveNext(int id) { Id = id; return this; }

    /// <summary>
    /// Moves the iterator to the next entity with the specified ID and casts it to the specified type.
    /// </summary>
    /// <typeparam name="TEntity">The type to cast the iterator to.</typeparam>
    /// <param name="id">The ID of the entity to move to.</param>
    /// <returns>The current iterator instance cast to the specified type.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public TEntity MoveNext<TEntity>(int id) where TEntity : EntityIterator => MoveNext(id) as TEntity;

    /// <summary>
    /// Spawns a new entity and moves the iterator to it.
    /// </summary>
    /// <returns>The current iterator instance for method chaining.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public EntityIterator Spawn() { Id = World.Entities.SpawnEntity(); return this; }

    /// <summary>
    /// Spawns a new entity and moves the iterator to it, casting to the specified type.
    /// </summary>
    /// <typeparam name="TEntity">The type to cast the iterator to.</typeparam>
    /// <returns>The current iterator instance cast to the specified type.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public TEntity Spawn<TEntity>() where TEntity : EntityIterator => Spawn() as TEntity;

    /// <summary>
    /// Adds a new component of the specified type to the current entity.
    /// </summary>
    /// <typeparam name="TComponent">The type of component to add.</typeparam>
    /// <returns>The current iterator instance for method chaining.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public EntityIterator Add<TComponent>() where TComponent : IComponent, new() { World.Components.Add<TComponent>(Id); return this; }

    /// <summary>
    /// Adds a component instance to the current entity.
    /// </summary>
    /// <typeparam name="TComponent">The type of component to add.</typeparam>
    /// <param name="component">The component instance to add.</param>
    /// <returns>The current iterator instance for method chaining.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public EntityIterator Add<TComponent>(TComponent component) where TComponent : IComponent, new() { World.Components.Add<TComponent>(Id, component); return this; }

    /// <summary>
    /// Adds a new component of the specified type to the current entity and casts the iterator.
    /// </summary>
    /// <typeparam name="TComponent">The type of component to add.</typeparam>
    /// <typeparam name="TEntity">The type to cast the iterator to.</typeparam>
    /// <returns>The current iterator instance cast to the specified type.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public TEntity Add<TComponent, TEntity>() where TComponent : IComponent, new() where TEntity : EntityIterator { World.Components.Add<TComponent>(Id); return this as TEntity; }

    /// <summary>
    /// Adds a component instance to the current entity and casts the iterator.
    /// </summary>
    /// <typeparam name="TComponent">The type of component to add.</typeparam>
    /// <typeparam name="TEntity">The type to cast the iterator to.</typeparam>
    /// <param name="component">The component instance to add.</param>
    /// <returns>The current iterator instance cast to the specified type.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public TEntity Add<TComponent, TEntity>(TComponent component) where TComponent : IComponent, new() where TEntity : EntityIterator { World.Components.Add<TComponent>(Id, component); return this as TEntity; }

    /// <summary>
    /// Checks if the current entity has a component of the specified type.
    /// </summary>
    /// <typeparam name="TComponent">The type of component to check for.</typeparam>
    /// <returns>True if the entity has the component, false otherwise.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Has<TComponent>() where TComponent : IComponent, new() => World.Components.Has<TComponent>(Id);

    /// <summary>
    /// Gets a component of the specified type from the current entity.
    /// </summary>
    /// <typeparam name="TComponent">The type of component to get.</typeparam>
    /// <returns>The component instance.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public TComponent Get<TComponent>() where TComponent : IComponent, new() => World.Components.Get<TComponent>(Id);

    /// <summary>
    /// Removes a component of the specified type from the current entity.
    /// </summary>
    /// <typeparam name="TComponent">The type of component to remove.</typeparam>
    /// <returns>The current iterator instance for method chaining.</returns>
    public EntityIterator Remove<TComponent>() where TComponent : IComponent, new() { World.Components.Remove<TComponent>(Id); return this; }

    /// <summary>
    /// Removes a component of the specified type from the current entity and casts the iterator.
    /// </summary>
    /// <typeparam name="TComponent">The type of component to remove.</typeparam>
    /// <typeparam name="TEntity">The type to cast the iterator to.</typeparam>
    /// <returns>The current iterator instance cast to the specified type.</returns>
    public TEntity Remove<TComponent, TEntity>() where TComponent : IComponent, new() where TEntity : EntityIterator { World.Components.Remove<TComponent>(Id); return this as TEntity; }

    /// <summary>
    /// Gets a reference to a component of the specified type from the current entity.
    /// </summary>
    /// <typeparam name="TComponent">The type of component to get a reference to.</typeparam>
    /// <returns>A reference to the component.</returns>
    public ref TComponent Ref<TComponent>() where TComponent : IComponent, new() => ref World.Components.Ref<TComponent>(Id);

    /// <summary>
    /// Marks the current entity for destruction at the end of the world update.
    /// </summary>
    public void Destroy() => World.Entities.Remove(Id);

    /// <summary>
    /// Spawns a new entity with a component of the specified type and casts the iterator.
    /// </summary>
    /// <typeparam name="TComponent">The type of component to add.</typeparam>
    /// <typeparam name="TEntity">The type to cast the iterator to.</typeparam>
    /// <returns>The current iterator instance cast to the specified type.</returns>
    public TEntity Spawn<TComponent, TEntity>() where TComponent : IComponent, new() where TEntity : EntityIterator
        => Spawn<TEntity>().Add<TComponent, TEntity>();

    /// <summary>
    /// Spawns a new entity with a component instance and casts the iterator.
    /// </summary>
    /// <typeparam name="TComponent">The type of component to add.</typeparam>
    /// <typeparam name="TEntity">The type to cast the iterator to.</typeparam>
    /// <param name="component">The component instance to add.</param>
    /// <returns>The current iterator instance cast to the specified type.</returns>
    public TEntity Spawn<TComponent, TEntity>(TComponent component) where TComponent : IComponent, new() where TEntity : EntityIterator
        => Spawn<TEntity>().Add<TComponent, TEntity>(component);

    /// <summary>
    /// Spawns a new entity with two components and casts the iterator.
    /// </summary>
    /// <typeparam name="TComp1">The type of the first component to add.</typeparam>
    /// <typeparam name="TComp2">The type of the second component to add.</typeparam>
    /// <typeparam name="TEntity">The type to cast the iterator to.</typeparam>
    /// <returns>The current iterator instance cast to the specified type.</returns>
    public TEntity Spawn<TComp1, TComp2, TEntity>()
        where TComp1 : IComponent, new()
        where TComp2 : IComponent, new()
        where TEntity : EntityIterator
        => Spawn<TEntity>()
            .Add<TComp1>()
            .Add<TComp2, TEntity>();

    /// <summary>
    /// Spawns a new entity with two component instances and casts the iterator.
    /// </summary>
    /// <typeparam name="TComp1">The type of the first component to add.</typeparam>
    /// <typeparam name="TComp2">The type of the second component to add.</typeparam>
    /// <typeparam name="TEntity">The type to cast the iterator to.</typeparam>
    /// <param name="comp1">The first component instance to add.</param>
    /// <param name="comp2">The second component instance to add.</param>
    /// <returns>The current iterator instance cast to the specified type.</returns>
    public TEntity Spawn<TComp1, TComp2, TEntity>(TComp1 comp1, TComp2 comp2)
        where TComp1 : IComponent, new()
        where TComp2 : IComponent, new()
        where TEntity : EntityIterator
        => Spawn<TEntity>()
            .Add<TComp1>(comp1)
            .Add<TComp2, TEntity>(comp2);

    /// <summary>
    /// Spawns a new entity with three components and casts the iterator.
    /// </summary>
    /// <typeparam name="TComp1">The type of the first component to add.</typeparam>
    /// <typeparam name="TComp2">The type of the second component to add.</typeparam>
    /// <typeparam name="TComp3">The type of the third component to add.</typeparam>
    /// <typeparam name="TEntity">The type to cast the iterator to.</typeparam>
    /// <returns>The current iterator instance cast to the specified type.</returns>
    public TEntity Spawn<TComp1, TComp2, TComp3, TEntity>()
        where TComp1 : IComponent, new()
        where TComp2 : IComponent, new()
        where TComp3 : IComponent, new()
        where TEntity : EntityIterator
        => Spawn<TEntity>()
            .Add<TComp1>()
            .Add<TComp2>()
            .Add<TComp3, TEntity>();

    /// <summary>
    /// Spawns a new entity with three component instances and casts the iterator.
    /// </summary>
    /// <typeparam name="TComp1">The type of the first component to add.</typeparam>
    /// <typeparam name="TComp2">The type of the second component to add.</typeparam>
    /// <typeparam name="TComp3">The type of the third component to add.</typeparam>
    /// <typeparam name="TEntity">The type to cast the iterator to.</typeparam>
    /// <param name="comp1">The first component instance to add.</param>
    /// <param name="comp2">The second component instance to add.</param>
    /// <param name="comp3">The third component instance to add.</param>
    /// <returns>The current iterator instance cast to the specified type.</returns>
    public TEntity Spawn<TComp1, TComp2, TComp3, TEntity>(TComp1 comp1, TComp2 comp2, TComp3 comp3)
        where TComp1 : IComponent, new()
        where TComp2 : IComponent, new()
        where TComp3 : IComponent, new()
        where TEntity : EntityIterator
        => Spawn<TEntity>()
            .Add<TComp1>(comp1)
            .Add<TComp2>(comp2)
            .Add<TComp3, TEntity>(comp3);

    /// <summary>
    /// Spawns a new entity with four components and casts the iterator.
    /// </summary>
    /// <typeparam name="TComp1">The type of the first component to add.</typeparam>
    /// <typeparam name="TComp2">The type of the second component to add.</typeparam>
    /// <typeparam name="TComp3">The type of the third component to add.</typeparam>
    /// <typeparam name="TComp4">The type of the fourth component to add.</typeparam>
    /// <typeparam name="TEntity">The type to cast the iterator to.</typeparam>
    /// <returns>The current iterator instance cast to the specified type.</returns>
    public TEntity Spawn<TComp1, TComp2, TComp3, TComp4, TEntity>()
        where TComp1 : IComponent, new()
        where TComp2 : IComponent, new()
        where TComp3 : IComponent, new()
        where TComp4 : IComponent, new()
        where TEntity : EntityIterator
        => Spawn<TEntity>()
            .Add<TComp1>()
            .Add<TComp2>()
            .Add<TComp3>()
            .Add<TComp4, TEntity>();

    /// <summary>
    /// Spawns a new entity with four component instances and casts the iterator.
    /// </summary>
    /// <typeparam name="TComp1">The type of the first component to add.</typeparam>
    /// <typeparam name="TComp2">The type of the second component to add.</typeparam>
    /// <typeparam name="TComp3">The type of the third component to add.</typeparam>
    /// <typeparam name="TComp4">The type of the fourth component to add.</typeparam>
    /// <typeparam name="TEntity">The type to cast the iterator to.</typeparam>
    /// <param name="comp1">The first component instance to add.</param>
    /// <param name="comp2">The second component instance to add.</param>
    /// <param name="comp3">The third component instance to add.</param>
    /// <param name="comp4">The fourth component instance to add.</param>
    /// <returns>The current iterator instance cast to the specified type.</returns>
    public TEntity Spawn<TComp1, TComp2, TComp3, TComp4, TEntity>(TComp1 comp1, TComp2 comp2, TComp3 comp3, TComp4 comp4)
        where TComp1 : IComponent, new()
        where TComp2 : IComponent, new()
        where TComp3 : IComponent, new()
        where TComp4 : IComponent, new()
        where TEntity : EntityIterator
        => Spawn<TEntity>()
            .Add<TComp1>(comp1)
            .Add<TComp2>(comp2)
            .Add<TComp3>(comp3)
            .Add<TComp4, TEntity>(comp4);

    /// <summary>
    /// Spawns a new entity with five components and casts the iterator.
    /// </summary>
    /// <typeparam name="TComp1">The type of the first component to add.</typeparam>
    /// <typeparam name="TComp2">The type of the second component to add.</typeparam>
    /// <typeparam name="TComp3">The type of the third component to add.</typeparam>
    /// <typeparam name="TComp4">The type of the fourth component to add.</typeparam>
    /// <typeparam name="TComp5">The type of the fifth component to add.</typeparam>
    /// <typeparam name="TEntity">The type to cast the iterator to.</typeparam>
    /// <returns>The current iterator instance cast to the specified type.</returns>
    public TEntity Spawn<TComp1, TComp2, TComp3, TComp4, TComp5, TEntity>()
        where TComp1 : IComponent, new()
        where TComp2 : IComponent, new()
        where TComp3 : IComponent, new()
        where TComp4 : IComponent, new()
        where TComp5 : IComponent, new()
        where TEntity : EntityIterator
        => Spawn<TEntity>()
            .Add<TComp1>()
            .Add<TComp2>()
            .Add<TComp3>()
            .Add<TComp4>()
            .Add<TComp5, TEntity>();

    /// <summary>
    /// Spawns a new entity with five component instances and casts the iterator.
    /// </summary>
    /// <typeparam name="TComp1">The type of the first component to add.</typeparam>
    /// <typeparam name="TComp2">The type of the second component to add.</typeparam>
    /// <typeparam name="TComp3">The type of the third component to add.</typeparam>
    /// <typeparam name="TComp4">The type of the fourth component to add.</typeparam>
    /// <typeparam name="TComp5">The type of the fifth component to add.</typeparam>
    /// <typeparam name="TEntity">The type to cast the iterator to.</typeparam>
    /// <param name="comp1">The first component instance to add.</param>
    /// <param name="comp2">The second component instance to add.</param>
    /// <param name="comp3">The third component instance to add.</param>
    /// <param name="comp4">The fourth component instance to add.</param>
    /// <param name="comp5">The fifth component instance to add.</param>
    /// <returns>The current iterator instance cast to the specified type.</returns>
    public TEntity Spawn<TComp1, TComp2, TComp3, TComp4, TComp5, TEntity>(TComp1 comp1, TComp2 comp2, TComp3 comp3, TComp4 comp4, TComp5 comp5)
        where TComp1 : IComponent, new()
        where TComp2 : IComponent, new()
        where TComp3 : IComponent, new()
        where TComp4 : IComponent, new()
        where TComp5 : IComponent, new()
        where TEntity : EntityIterator
        => Spawn<TEntity>()
            .Add<TComp1>(comp1)
            .Add<TComp2>(comp2)
            .Add<TComp3>(comp3)
            .Add<TComp4>(comp4)
            .Add<TComp5, TEntity>(comp5);

    /// <summary>
    /// Spawns a new entity with six components and casts the iterator.
    /// </summary>
    /// <typeparam name="TComp1">The type of the first component to add.</typeparam>
    /// <typeparam name="TComp2">The type of the second component to add.</typeparam>
    /// <typeparam name="TComp3">The type of the third component to add.</typeparam>
    /// <typeparam name="TComp4">The type of the fourth component to add.</typeparam>
    /// <typeparam name="TComp5">The type of the fifth component to add.</typeparam>
    /// <typeparam name="TComp6">The type of the sixth component to add.</typeparam>
    /// <typeparam name="TEntity">The type to cast the iterator to.</typeparam>
    /// <returns>The current iterator instance cast to the specified type.</returns>
    public TEntity Spawn<TComp1, TComp2, TComp3, TComp4, TComp5, TComp6, TEntity>()
        where TComp1 : IComponent, new()
        where TComp2 : IComponent, new()
        where TComp3 : IComponent, new()
        where TComp4 : IComponent, new()
        where TComp5 : IComponent, new()
        where TComp6 : IComponent, new()
        where TEntity : EntityIterator
        => Spawn<TEntity>()
            .Add<TComp1>()
            .Add<TComp2>()
            .Add<TComp3>()
            .Add<TComp4>()
            .Add<TComp5>()
            .Add<TComp6, TEntity>();

    /// <summary>
    /// Spawns a new entity with six component instances and casts the iterator.
    /// </summary>
    /// <typeparam name="TComp1">The type of the first component to add.</typeparam>
    /// <typeparam name="TComp2">The type of the second component to add.</typeparam>
    /// <typeparam name="TComp3">The type of the third component to add.</typeparam>
    /// <typeparam name="TComp4">The type of the fourth component to add.</typeparam>
    /// <typeparam name="TComp5">The type of the fifth component to add.</typeparam>
    /// <typeparam name="TComp6">The type of the sixth component to add.</typeparam>
    /// <typeparam name="TEntity">The type to cast the iterator to.</typeparam>
    /// <param name="comp1">The first component instance to add.</param>
    /// <param name="comp2">The second component instance to add.</param>
    /// <param name="comp3">The third component instance to add.</param>
    /// <param name="comp4">The fourth component instance to add.</param>
    /// <param name="comp5">The fifth component instance to add.</param>
    /// <param name="comp6">The sixth component instance to add.</param>
    /// <returns>The current iterator instance cast to the specified type.</returns>
    public TEntity Spawn<TComp1, TComp2, TComp3, TComp4, TComp5, TComp6, TEntity>(TComp1 comp1, TComp2 comp2, TComp3 comp3, TComp4 comp4, TComp5 comp5, TComp6 comp6)
        where TComp1 : IComponent, new()
        where TComp2 : IComponent, new()
        where TComp3 : IComponent, new()
        where TComp4 : IComponent, new()
        where TComp5 : IComponent, new()
        where TComp6 : IComponent, new()
        where TEntity : EntityIterator
        => Spawn<TEntity>()
            .Add<TComp1>(comp1)
            .Add<TComp2>(comp2)
            .Add<TComp3>(comp3)
            .Add<TComp4>(comp4)
            .Add<TComp5>(comp5)
            .Add<TComp6, TEntity>(comp6);

    /// <summary>
    /// Spawns a new entity with seven components and casts the iterator.
    /// </summary>
    /// <typeparam name="TComp1">The type of the first component to add.</typeparam>
    /// <typeparam name="TComp2">The type of the second component to add.</typeparam>
    /// <typeparam name="TComp3">The type of the third component to add.</typeparam>
    /// <typeparam name="TComp4">The type of the fourth component to add.</typeparam>
    /// <typeparam name="TComp5">The type of the fifth component to add.</typeparam>
    /// <typeparam name="TComp6">The type of the sixth component to add.</typeparam>
    /// <typeparam name="TComp7">The type of the seventh component to add.</typeparam>
    /// <typeparam name="TEntity">The type to cast the iterator to.</typeparam>
    /// <returns>The current iterator instance cast to the specified type.</returns>
    public TEntity Spawn<TComp1, TComp2, TComp3, TComp4, TComp5, TComp6, TComp7, TEntity>()
        where TComp1 : IComponent, new()
        where TComp2 : IComponent, new()
        where TComp3 : IComponent, new()
        where TComp4 : IComponent, new()
        where TComp5 : IComponent, new()
        where TComp6 : IComponent, new()
        where TComp7 : IComponent, new()
        where TEntity : EntityIterator
        => Spawn<TEntity>()
            .Add<TComp1>()
            .Add<TComp2>()
            .Add<TComp3>()
            .Add<TComp4>()
            .Add<TComp5>()
            .Add<TComp6>()
            .Add<TComp7, TEntity>();

    /// <summary>
    /// Spawns a new entity with seven component instances and casts the iterator.
    /// </summary>
    /// <typeparam name="TComp1">The type of the first component to add.</typeparam>
    /// <typeparam name="TComp2">The type of the second component to add.</typeparam>
    /// <typeparam name="TComp3">The type of the third component to add.</typeparam>
    /// <typeparam name="TComp4">The type of the fourth component to add.</typeparam>
    /// <typeparam name="TComp5">The type of the fifth component to add.</typeparam>
    /// <typeparam name="TComp6">The type of the sixth component to add.</typeparam>
    /// <typeparam name="TComp7">The type of the seventh component to add.</typeparam>
    /// <typeparam name="TEntity">The type to cast the iterator to.</typeparam>
    /// <param name="comp1">The first component instance to add.</param>
    /// <param name="comp2">The second component instance to add.</param>
    /// <param name="comp3">The third component instance to add.</param>
    /// <param name="comp4">The fourth component instance to add.</param>
    /// <param name="comp5">The fifth component instance to add.</param>
    /// <param name="comp6">The sixth component instance to add.</param>
    /// <param name="comp7">The seventh component instance to add.</param>
    /// <returns>The current iterator instance cast to the specified type.</returns>
    public TEntity Spawn<TComp1, TComp2, TComp3, TComp4, TComp5, TComp6, TComp7, TEntity>(TComp1 comp1, TComp2 comp2, TComp3 comp3, TComp4 comp4, TComp5 comp5, TComp6 comp6, TComp7 comp7)
        where TComp1 : IComponent, new()
        where TComp2 : IComponent, new()
        where TComp3 : IComponent, new()
        where TComp4 : IComponent, new()
        where TComp5 : IComponent, new()
        where TComp6 : IComponent, new()
        where TComp7 : IComponent, new()
        where TEntity : EntityIterator
        => Spawn<TEntity>()
            .Add<TComp1>(comp1)
            .Add<TComp2>(comp2)
            .Add<TComp3>(comp3)
            .Add<TComp4>(comp4)
            .Add<TComp5>(comp5)
            .Add<TComp6>(comp6)
            .Add<TComp7, TEntity>(comp7);

    /// <summary>
    /// Spawns a new entity with eight components and casts the iterator.
    /// </summary>
    /// <typeparam name="TComp1">The type of the first component to add.</typeparam>
    /// <typeparam name="TComp2">The type of the second component to add.</typeparam>
    /// <typeparam name="TComp3">The type of the third component to add.</typeparam>
    /// <typeparam name="TComp4">The type of the fourth component to add.</typeparam>
    /// <typeparam name="TComp5">The type of the fifth component to add.</typeparam>
    /// <typeparam name="TComp6">The type of the sixth component to add.</typeparam>
    /// <typeparam name="TComp7">The type of the seventh component to add.</typeparam>
    /// <typeparam name="TComp8">The type of the eighth component to add.</typeparam>
    /// <typeparam name="TEntity">The type to cast the iterator to.</typeparam>
    /// <returns>The current iterator instance cast to the specified type.</returns>
    public TEntity Spawn<TComp1, TComp2, TComp3, TComp4, TComp5, TComp6, TComp7, TComp8, TEntity>()
        where TComp1 : IComponent, new()
        where TComp2 : IComponent, new()
        where TComp3 : IComponent, new()
        where TComp4 : IComponent, new()
        where TComp5 : IComponent, new()
        where TComp6 : IComponent, new()
        where TComp7 : IComponent, new()
        where TComp8 : IComponent, new()
        where TEntity : EntityIterator
        => Spawn<TEntity>()
            .Add<TComp1>()
            .Add<TComp2>()
            .Add<TComp3>()
            .Add<TComp4>()
            .Add<TComp5>()
            .Add<TComp6>()
            .Add<TComp7>()
            .Add<TComp8, TEntity>();

    /// <summary>
    /// Spawns a new entity with eight component instances and casts the iterator.
    /// </summary>
    /// <typeparam name="TComp1">The type of the first component to add.</typeparam>
    /// <typeparam name="TComp2">The type of the second component to add.</typeparam>
    /// <typeparam name="TComp3">The type of the third component to add.</typeparam>
    /// <typeparam name="TComp4">The type of the fourth component to add.</typeparam>
    /// <typeparam name="TComp5">The type of the fifth component to add.</typeparam>
    /// <typeparam name="TComp6">The type of the sixth component to add.</typeparam>
    /// <typeparam name="TComp7">The type of the seventh component to add.</typeparam>
    /// <typeparam name="TComp8">The type of the eighth component to add.</typeparam>
    /// <typeparam name="TEntity">The type to cast the iterator to.</typeparam>
    /// <param name="comp1">The first component instance to add.</param>
    /// <param name="comp2">The second component instance to add.</param>
    /// <param name="comp3">The third component instance to add.</param>
    /// <param name="comp4">The fourth component instance to add.</param>
    /// <param name="comp5">The fifth component instance to add.</param>
    /// <param name="comp6">The sixth component instance to add.</param>
    /// <param name="comp7">The seventh component instance to add.</param>
    /// <param name="comp8">The eighth component instance to add.</param>
    /// <returns>The current iterator instance cast to the specified type.</returns>
    public TEntity Spawn<TComp1, TComp2, TComp3, TComp4, TComp5, TComp6, TComp7, TComp8, TEntity>(TComp1 comp1, TComp2 comp2, TComp3 comp3, TComp4 comp4, TComp5 comp5, TComp6 comp6, TComp7 comp7, TComp8 comp8)
        where TComp1 : IComponent, new()
        where TComp2 : IComponent, new()
        where TComp3 : IComponent, new()
        where TComp4 : IComponent, new()
        where TComp5 : IComponent, new()
        where TComp6 : IComponent, new()
        where TComp7 : IComponent, new()
        where TComp8 : IComponent, new()
        where TEntity : EntityIterator
        => Spawn<TEntity>()
            .Add<TComp1>(comp1)
            .Add<TComp2>(comp2)
            .Add<TComp3>(comp3)
            .Add<TComp4>(comp4)
            .Add<TComp5>(comp5)
            .Add<TComp6>(comp6)
            .Add<TComp7>(comp7)
            .Add<TComp8, TEntity>(comp8);

    /// <summary>
    /// Gets two components from the current entity.
    /// </summary>
    /// <typeparam name="T1">The type of the first component to get.</typeparam>
    /// <typeparam name="T2">The type of the second component to get.</typeparam>
    /// <returns>A tuple containing the two components.</returns>
    public (T1, T2) Get<T1, T2>()
        where T1 : IComponent, new()
        where T2 : IComponent, new()
        => (World.Components.Get<T1>(Id), World.Components.Get<T2>(Id));

    /// <summary>
    /// Gets three components from the current entity.
    /// </summary>
    /// <typeparam name="T1">The type of the first component to get.</typeparam>
    /// <typeparam name="T2">The type of the second component to get.</typeparam>
    /// <typeparam name="T3">The type of the third component to get.</typeparam>
    /// <returns>A tuple containing the three components.</returns>
    public (T1, T2, T3) Get<T1, T2, T3>()
        where T1 : IComponent, new()
        where T2 : IComponent, new()
        where T3 : IComponent, new()
        => (World.Components.Get<T1>(Id), World.Components.Get<T2>(Id), World.Components.Get<T3>(Id));

    /// <summary>
    /// Gets four components from the current entity.
    /// </summary>
    /// <typeparam name="T1">The type of the first component to get.</typeparam>
    /// <typeparam name="T2">The type of the second component to get.</typeparam>
    /// <typeparam name="T3">The type of the third component to get.</typeparam>
    /// <typeparam name="T4">The type of the fourth component to get.</typeparam>
    /// <returns>A tuple containing the four components.</returns>
    public (T1, T2, T3, T4) Get<T1, T2, T3, T4>()
        where T1 : IComponent, new()
        where T2 : IComponent, new()
        where T3 : IComponent, new()
        where T4 : IComponent, new()
        => (World.Components.Get<T1>(Id), World.Components.Get<T2>(Id), World.Components.Get<T3>(Id), World.Components.Get<T4>(Id));

    /// <summary>
    /// Gets five components from the current entity.
    /// </summary>
    /// <typeparam name="T1">The type of the first component to get.</typeparam>
    /// <typeparam name="T2">The type of the second component to get.</typeparam>
    /// <typeparam name="T3">The type of the third component to get.</typeparam>
    /// <typeparam name="T4">The type of the fourth component to get.</typeparam>
    /// <typeparam name="T5">The type of the fifth component to get.</typeparam>
    /// <returns>A tuple containing the five components.</returns>
    public (T1, T2, T3, T4, T5) Get<T1, T2, T3, T4, T5>()
        where T1 : IComponent, new()
        where T2 : IComponent, new()
        where T3 : IComponent, new()
        where T4 : IComponent, new()
        where T5 : IComponent, new()
        => (World.Components.Get<T1>(Id), World.Components.Get<T2>(Id), World.Components.Get<T3>(Id), World.Components.Get<T4>(Id), World.Components.Get<T5>(Id));

    /// <summary>
    /// Gets six components from the current entity.
    /// </summary>
    /// <typeparam name="T1">The type of the first component to get.</typeparam>
    /// <typeparam name="T2">The type of the second component to get.</typeparam>
    /// <typeparam name="T3">The type of the third component to get.</typeparam>
    /// <typeparam name="T4">The type of the fourth component to get.</typeparam>
    /// <typeparam name="T5">The type of the fifth component to get.</typeparam>
    /// <typeparam name="T6">The type of the sixth component to get.</typeparam>
    /// <returns>A tuple containing the six components.</returns>
    public (T1, T2, T3, T4, T5, T6) Get<T1, T2, T3, T4, T5, T6>()
        where T1 : IComponent, new()
        where T2 : IComponent, new()
        where T3 : IComponent, new()
        where T4 : IComponent, new()
        where T5 : IComponent, new()
        where T6 : IComponent, new()
        => (World.Components.Get<T1>(Id), World.Components.Get<T2>(Id), World.Components.Get<T3>(Id), World.Components.Get<T4>(Id), World.Components.Get<T5>(Id), World.Components.Get<T6>(Id));

    /// <summary>
    /// Gets seven components from the current entity.
    /// </summary>
    /// <typeparam name="T1">The type of the first component to get.</typeparam>
    /// <typeparam name="T2">The type of the second component to get.</typeparam>
    /// <typeparam name="T3">The type of the third component to get.</typeparam>
    /// <typeparam name="T4">The type of the fourth component to get.</typeparam>
    /// <typeparam name="T5">The type of the fifth component to get.</typeparam>
    /// <typeparam name="T6">The type of the sixth component to get.</typeparam>
    /// <typeparam name="T7">The type of the seventh component to get.</typeparam>
    /// <returns>A tuple containing the seven components.</returns>
    public (T1, T2, T3, T4, T5, T6, T7) Get<T1, T2, T3, T4, T5, T6, T7>()
        where T1 : IComponent, new()
        where T2 : IComponent, new()
        where T3 : IComponent, new()
        where T4 : IComponent, new()
        where T5 : IComponent, new()
        where T6 : IComponent, new()
        where T7 : IComponent, new()
        => (World.Components.Get<T1>(Id), World.Components.Get<T2>(Id), World.Components.Get<T3>(Id), World.Components.Get<T4>(Id), World.Components.Get<T5>(Id), World.Components.Get<T6>(Id), World.Components.Get<T7>(Id));

    /// <summary>
    /// Gets eight components from the current entity.
    /// </summary>
    /// <typeparam name="T1">The type of the first component to get.</typeparam>
    /// <typeparam name="T2">The type of the second component to get.</typeparam>
    /// <typeparam name="T3">The type of the third component to get.</typeparam>
    /// <typeparam name="T4">The type of the fourth component to get.</typeparam>
    /// <typeparam name="T5">The type of the fifth component to get.</typeparam>
    /// <typeparam name="T6">The type of the sixth component to get.</typeparam>
    /// <typeparam name="T7">The type of the seventh component to get.</typeparam>
    /// <typeparam name="T8">The type of the eighth component to get.</typeparam>
    /// <returns>A tuple containing the eight components.</returns>
    public (T1, T2, T3, T4, T5, T6, T7, T8) Get<T1, T2, T3, T4, T5, T6, T7, T8>()
        where T1 : IComponent, new()
        where T2 : IComponent, new()
        where T3 : IComponent, new()
        where T4 : IComponent, new()
        where T5 : IComponent, new()
        where T6 : IComponent, new()
        where T7 : IComponent, new()
        where T8 : IComponent, new()
        => (World.Components.Get<T1>(Id), World.Components.Get<T2>(Id), World.Components.Get<T3>(Id), World.Components.Get<T4>(Id), World.Components.Get<T5>(Id), World.Components.Get<T6>(Id), World.Components.Get<T7>(Id), World.Components.Get<T8>(Id));
}