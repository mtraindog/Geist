// Copyright (c) 2024 Gm.Engine
// Author: Eric Boyd
// Licensed under the MIT License.
// See https://github.com/mtraindog/Gm.Engine/blob/main/LICENSE for details.

namespace Geist.Ecs;

/// <summary>
/// Manages the lifecycle and execution of systems in the Entity Component System (ECS).
/// Handles system registration, initialization, updates, and cleanup.
/// </summary>
public sealed class SystemManager
{
    /// <summary>
    /// Gets or sets the world instance that this system manager belongs to.
    /// </summary>
    internal World World { get; set; }
    
    readonly Dictionary<Type, ISystem> _systems = [];
    
    /// <summary>
    /// Registers a new system of type T in the system manager.
    /// </summary>
    /// <typeparam name="T">The type of system to register. Must inherit from SystemBase and have a parameterless constructor.</typeparam>
    public void Register<T>() where T : SystemBase, new()
        => _systems.Add(typeof(T), new T() { World = World });

    /// <summary>
    /// Initializes a specific system of type T.
    /// </summary>
    /// <typeparam name="T">The type of system to initialize.</typeparam>
    public void Initialize<T>() where T : ISystem
        => _systems[typeof(T)].Initialize();

    /// <summary>
    /// Updates a specific system of type T with the current game time.
    /// </summary>
    /// <typeparam name="T">The type of system to update.</typeparam>
    public void Update<T>() where T : ISystem
        => _systems[typeof(T)].Update(World.GameTime);

    /// <summary>
    /// Destroys a specific system of type T.
    /// </summary>
    /// <typeparam name="T">The type of system to destroy.</typeparam>
    public void Destroy<T>() where T : ISystem => _systems[typeof(T)].Destroy();

    /// <summary>
    /// Destroys all registered systems.
    /// </summary>
    internal void Destroy()
    {
        foreach (var kvp in _systems)
            kvp.Value.Destroy();
    }
}
