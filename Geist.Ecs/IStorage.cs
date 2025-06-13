// Copyright (c) 2024 Gm.Engine
// Author: Eric Boyd
// Licensed under the MIT License.
// See https://github.com/mtraindog/Gm.Engine/blob/main/LICENSE for details.

namespace Geist.Ecs;

/// <summary>
/// Defines the interface for component storage in the ECS system.
/// Provides basic operations for managing component data, including counting, checking existence, removal, and clearing.
/// </summary>
public interface IStorage
{
    /// <summary>
    /// Gets the total number of components stored in this storage.
    /// </summary>
    int Count { get; }

    /// <summary>
    /// Checks if a component exists for a given entity ID and retrieves its index.
    /// </summary>
    /// <param name="id">The ID of the entity to check.</param>
    /// <param name="index">The index of the component if found, -1 otherwise.</param>
    /// <returns>True if a component exists for the entity, false otherwise.</returns>
    bool Contains(int id, out int index);

    /// <summary>
    /// Removes a component at the specified position.
    /// </summary>
    /// <param name="pos">The position of the component to remove.</param>
    /// <returns>True if the component was successfully removed, false otherwise.</returns>
    bool Remove(int pos);

    /// <summary>
    /// Removes all components from the storage.
    /// </summary>
    void Clear();
}