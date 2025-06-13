// Copyright (c) 2024 Gm.Engine
// Author: Eric Boyd
// Licensed under the MIT License.
// See https://github.com/mtraindog/Gm.Engine/blob/main/LICENSE for details.

using Gm.Engine.Collections;

namespace Geist.Ecs;

/// <summary>
/// Manages entities in the ECS system, providing functionality for entity creation, destruction, and querying.
/// Handles entity lifecycle and maintains relationships between entities and their components.
/// </summary>
public sealed class EntityManager
{
    readonly Dictionary<Type, int> _queries = [];
    readonly DistinctBag<int> _removals = new();

    /// <summary>
    /// Gets or sets the world instance that this entity manager belongs to.
    /// </summary>
    internal World World { get; set; }

    /// <summary>
    /// Gets a read-only span of entity IDs that are marked for removal.
    /// </summary>
    public ReadOnlySpan<int> Removals => _removals.Values;

    /// <summary>
    /// Gets the total number of entities in the world.
    /// </summary>
    public int Count => World.Components.Mapper.Count;

    /// <summary>
    /// Creates a new entity iterator for traversing and manipulating entities.
    /// </summary>
    /// <returns>A new entity iterator instance.</returns>
    public EntityIterator NewIterator() => new() { World = World };

    /// <summary>
    /// Creates a new entity iterator of the specified type for traversing and manipulating entities.
    /// </summary>
    /// <typeparam name="T">The type of entity iterator to create.</typeparam>
    /// <returns>A new entity iterator instance of the specified type.</returns>
    public T NewIterator<T>() where T : EntityIterator, new() => new() { World = World };

    /// <summary>
    /// Gets a read-only span of all entities in the world.
    /// </summary>
    public ReadOnlySpan<Entity> All => World.Components.Mapper.Entities;

    /// <summary>
    /// Marks an entity for removal at the end of the world update.
    /// </summary>
    /// <param name="id">The ID of the entity to mark for removal.</param>
    public void Remove(int id) => _removals.Add(id);

    /// <summary>
    /// Immediately removes an entity and all its components from the world.
    /// </summary>
    /// <param name="id">The ID of the entity to remove.</param>
    public void RemoveImmediate(int id) => Destroy(id);

    /// <summary>
    /// Clears the list of entities marked for removal.
    /// </summary>
    public void ClearRemovals() => _removals.Clear();    

    /// <summary>
    /// Creates a new entity in the world and returns its ID.
    /// </summary>
    /// <returns>The ID of the newly created entity.</returns>
    public int SpawnEntity()
    {
        int id = IdProvider.Instance.NextId(World.Id);
        World.Components.Mapper.AddEntity(id);
        return id;
    }

    /// <summary>
    /// Destroys an entity and all its components, reclaiming its ID for reuse.
    /// </summary>
    /// <param name="id">The ID of the entity to destroy.</param>
    internal void Destroy(int id)
    {
        ReadOnlySpan<Type> types = World.Components.Mapper.GetEntityComponentTypes(id);

        for (int i = 0; i < types.Length; i++)
            World.Components[types[i]].Remove(id);

        World.Components.Mapper.RemoveEntity(id);
        IdProvider.Instance.Reclaim(World.Id, id);
    }

    /// <summary>
    /// Gets all entities that have a component of type T.
    /// </summary>
    /// <typeparam name="T">The type of component to query for.</typeparam>
    /// <returns>A read-only span of entities that have the specified component.</returns>
    public ReadOnlySpan<Entity> With<T>() where T : IComponent =>
        World.Components.Mapper.GetEntities(World.Components.Mapper.CalculateMask([typeof(T)]));

    /// <summary>
    /// Gets all entities that have both components of type T1 and T2.
    /// </summary>
    /// <typeparam name="T1">The type of the first component to query for.</typeparam>
    /// <typeparam name="T2">The type of the second component to query for.</typeparam>
    /// <returns>A read-only span of entities that have both specified components.</returns>
    public ReadOnlySpan<Entity> With<T1, T2>()
        where T1 : IComponent
        where T2 : IComponent =>
        World.Components.Mapper.GetEntities(World.Components.Mapper.CalculateMask([
            typeof(T1),
            typeof(T2)]));

    /// <summary>
    /// Gets all entities that have all three components of type T1, T2, and T3.
    /// </summary>
    /// <typeparam name="T1">The type of the first component to query for.</typeparam>
    /// <typeparam name="T2">The type of the second component to query for.</typeparam>
    /// <typeparam name="T3">The type of the third component to query for.</typeparam>
    /// <returns>A read-only span of entities that have all three specified components.</returns>
    public ReadOnlySpan<Entity> With<T1, T2, T3>()
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent =>
        World.Components.Mapper.GetEntities(World.Components.Mapper.CalculateMask([
            typeof(T1),
            typeof(T2),
            typeof(T3)]));

    /// <summary>
    /// Gets all entities that have all four components of type T1, T2, T3, and T4.
    /// </summary>
    /// <typeparam name="T1">The type of the first component to query for.</typeparam>
    /// <typeparam name="T2">The type of the second component to query for.</typeparam>
    /// <typeparam name="T3">The type of the third component to query for.</typeparam>
    /// <typeparam name="T4">The type of the fourth component to query for.</typeparam>
    /// <returns>A read-only span of entities that have all four specified components.</returns>
    public ReadOnlySpan<Entity> With<T1, T2, T3, T4>()
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
        where T4 : IComponent =>
        World.Components.Mapper.GetEntities(World.Components.Mapper.CalculateMask([
            typeof(T1),
            typeof(T2),
            typeof(T3),
            typeof(T4)]));

    /// <summary>
    /// Gets all entities that have all five components of type T1, T2, T3, T4, and T5.
    /// </summary>
    /// <typeparam name="T1">The type of the first component to query for.</typeparam>
    /// <typeparam name="T2">The type of the second component to query for.</typeparam>
    /// <typeparam name="T3">The type of the third component to query for.</typeparam>
    /// <typeparam name="T4">The type of the fourth component to query for.</typeparam>
    /// <typeparam name="T5">The type of the fifth component to query for.</typeparam>
    /// <returns>A read-only span of entities that have all five specified components.</returns>
    public ReadOnlySpan<Entity> With<T1, T2, T3, T4, T5>()
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
        where T4 : IComponent
        where T5 : IComponent =>
        World.Components.Mapper.GetEntities(World.Components.Mapper.CalculateMask([
            typeof(T1),
            typeof(T2),
            typeof(T3),
            typeof(T4),
            typeof(T5)]));

    /// <summary>
    /// Gets all entities that have all six components of type T1, T2, T3, T4, T5, and T6.
    /// </summary>
    /// <typeparam name="T1">The type of the first component to query for.</typeparam>
    /// <typeparam name="T2">The type of the second component to query for.</typeparam>
    /// <typeparam name="T3">The type of the third component to query for.</typeparam>
    /// <typeparam name="T4">The type of the fourth component to query for.</typeparam>
    /// <typeparam name="T5">The type of the fifth component to query for.</typeparam>
    /// <typeparam name="T6">The type of the sixth component to query for.</typeparam>
    /// <returns>A read-only span of entities that have all six specified components.</returns>
    public ReadOnlySpan<Entity> With<T1, T2, T3, T4, T5, T6>()
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
        where T4 : IComponent
        where T5 : IComponent
        where T6 : IComponent =>
        World.Components.Mapper.GetEntities(World.Components.Mapper.CalculateMask([
            typeof(T1),
            typeof(T2),
            typeof(T3),
            typeof(T4),
            typeof(T5),
            typeof(T6)]));

    /// <summary>
    /// Gets all entities that have all seven components of type T1, T2, T3, T4, T5, T6, and T7.
    /// </summary>
    /// <typeparam name="T1">The type of the first component to query for.</typeparam>
    /// <typeparam name="T2">The type of the second component to query for.</typeparam>
    /// <typeparam name="T3">The type of the third component to query for.</typeparam>
    /// <typeparam name="T4">The type of the fourth component to query for.</typeparam>
    /// <typeparam name="T5">The type of the fifth component to query for.</typeparam>
    /// <typeparam name="T6">The type of the sixth component to query for.</typeparam>
    /// <typeparam name="T7">The type of the seventh component to query for.</typeparam>
    /// <returns>A read-only span of entities that have all seven specified components.</returns>
    public ReadOnlySpan<Entity> With<T1, T2, T3, T4, T5, T6, T7>()
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
        where T4 : IComponent
        where T5 : IComponent
        where T6 : IComponent
        where T7 : IComponent =>
        World.Components.Mapper.GetEntities(World.Components.Mapper.CalculateMask([
            typeof(T1),
            typeof(T2),
            typeof(T3),
            typeof(T4),
            typeof(T5),
            typeof(T6),
            typeof(T7)]));

    /// <summary>
    /// Gets all entities that have all eight components of type T1, T2, T3, T4, T5, T6, T7, and T8.
    /// </summary>
    /// <typeparam name="T1">The type of the first component to query for.</typeparam>
    /// <typeparam name="T2">The type of the second component to query for.</typeparam>
    /// <typeparam name="T3">The type of the third component to query for.</typeparam>
    /// <typeparam name="T4">The type of the fourth component to query for.</typeparam>
    /// <typeparam name="T5">The type of the fifth component to query for.</typeparam>
    /// <typeparam name="T6">The type of the sixth component to query for.</typeparam>
    /// <typeparam name="T7">The type of the seventh component to query for.</typeparam>
    /// <typeparam name="T8">The type of the eighth component to query for.</typeparam>
    /// <returns>A read-only span of entities that have all eight specified components.</returns>
    public ReadOnlySpan<Entity> With<T1, T2, T3, T4, T5, T6, T7, T8>()
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
        where T4 : IComponent
        where T5 : IComponent
        where T6 : IComponent
        where T7 : IComponent
        where T8 : IComponent =>
        World.Components.Mapper.GetEntities(World.Components.Mapper.CalculateMask([
            typeof(T1),
            typeof(T2),
            typeof(T3),
            typeof(T4),
            typeof(T5),
            typeof(T6),
            typeof(T7),
            typeof(T8)]));
}
