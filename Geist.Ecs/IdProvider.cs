// Copyright (c) 2024 Gm.Engine
// Author: Eric Boyd
// Licensed under the MIT License.
// See https://github.com/mtraindog/Gm.Engine/blob/main/LICENSE for details.

namespace Geist.Ecs;
using System.Runtime.CompilerServices;

/// <summary>
/// Provides unique identifier management for worlds and entities in the ECS system.
/// Implements a singleton pattern to ensure consistent ID generation across the application.
/// </summary>
internal sealed class IdProvider
{
    static readonly Lazy<IdProvider> _lazy = new(() => new IdProvider());
    readonly Dictionary<int, int> _worldNextIds = [];
    readonly Dictionary<int, Stack<int>> _worldReclaims = [];
    int _nextWorldId = -1;

    /// <summary>
    /// Gets the singleton instance of the IdProvider.
    /// </summary>
    public static IdProvider Instance => _lazy.Value;

    /// <summary>
    /// Initializes a new instance of the IdProvider class.
    /// Private constructor to enforce singleton pattern.
    /// </summary>
    private IdProvider() { }    
    
    /// <summary>
    /// Registers a new world and returns its unique identifier.
    /// </summary>
    /// <returns>The unique identifier for the newly registered world.</returns>
    public int RegisterWorld()
    {
        _worldNextIds.Add(++_nextWorldId, -1);
        _worldReclaims.Add(_nextWorldId, []);
        return _nextWorldId;
    }

    /// <summary>
    /// Gets the next available entity ID for a specific world.
    /// Reuses reclaimed IDs if available, otherwise generates a new one.
    /// </summary>
    /// <param name="worldId">The ID of the world requesting a new entity ID.</param>
    /// <returns>The next available entity ID.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int NextId(int worldId)
    {
        if (_worldReclaims[worldId].Count > 0)
            return _worldReclaims[worldId].Pop();

        return ++_worldNextIds[worldId];
    }

    /// <summary>
    /// Reclaims an entity ID for reuse in a specific world.
    /// </summary>
    /// <param name="worldId">The ID of the world the entity belongs to.</param>
    /// <param name="id">The ID of the entity to reclaim.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Reclaim(int worldId, int id)
        => _worldReclaims[worldId].Push(id);
}
