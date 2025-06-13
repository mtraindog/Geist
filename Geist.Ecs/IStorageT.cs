// Copyright (c) 2024 Gm.Engine
// Author: Eric Boyd
// Licensed under the MIT License.
// See https://github.com/mtraindog/Gm.Engine/blob/main/LICENSE for details.

namespace Geist.Ecs;

/// <summary>
/// Represents a strongly-typed storage interface for components in the ECS system.
/// Provides methods for accessing and manipulating components of a specific type.
/// </summary>
/// <typeparam name="T">The type of component to store.</typeparam>
public interface IStorage<T> : IStorage
{
    /// <summary>
    /// Gets a span of all components in the storage.
    /// </summary>
    /// <returns>A span containing all components in the storage.</returns>
    Span<T> AsSpan();

    /// <summary>
    /// Adds a component to the storage for a specific entity.
    /// </summary>
    /// <param name="id">The ID of the entity to add the component to.</param>
    /// <param name="compnent">The component to add.</param>
    void Add(int id, T compnent);

    /// <summary>
    /// Gets a reference to the component for a specific entity.
    /// </summary>
    /// <param name="id">The ID of the entity.</param>
    /// <returns>A reference to the component.</returns>
    ref T this[int id] { get; }
}