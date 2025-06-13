// Copyright (c) 2024 Gm.Engine
// Author: Eric Boyd
// Licensed under the MIT License.
// See https://github.com/mtraindog/Gm.Engine/blob/main/LICENSE for details.

using Gm.Engine.Events;
using Gm.Engine.Input;
using Gm.Engine.Screens;
using Microsoft.Xna.Framework.Graphics;
using GameTime = Microsoft.Xna.Framework.GameTime;

namespace Geist.Ecs;

/// <summary>
/// Represents the main container for all ECS-related functionality.
/// Manages entities, components, systems, input, events, and screens.
/// </summary>
public sealed class World
{
    const int INIT_CAPACITY = 131_000;

    readonly int _id;
    readonly InputManager _inputs;
    readonly EventManager _events;
    readonly EntityManager _entities;
    readonly ComponentManager _components;
    readonly SystemManager _systems;
    readonly ScreenManager _screens;

    WorldState _updateState = WorldState.ReadyToUpdate;

    /// <summary>
    /// Gets the unique identifier of this world instance.
    /// </summary>
    public int Id => _id;

    /// <summary>
    /// Gets the input manager for handling user input.
    /// </summary>
    public InputManager Input => _inputs;

    /// <summary>
    /// Gets the event manager for handling game events.
    /// </summary>
    public EventManager Events => _events;

    /// <summary>
    /// Gets the entity manager for handling entity lifecycle.
    /// </summary>
    public EntityManager Entities => _entities;

    /// <summary>
    /// Gets the component manager for handling component storage and retrieval.
    /// </summary>
    public ComponentManager Components => _components;

    /// <summary>
    /// Gets the system manager for handling system lifecycle and updates.
    /// </summary>
    public SystemManager Systems => _systems;

    /// <summary>
    /// Gets the screen manager for handling game screens.
    /// </summary>
    public ScreenManager Screens => _screens;

    /// <summary>
    /// Gets the current game time.
    /// </summary>
    public GameTime GameTime { get; private set; }

    /// <summary>
    /// Initializes a new instance of the World class.
    /// </summary>
    /// <param name="initCapacity">The initial capacity for component storage. Defaults to 131,000.</param>
    public World(int initCapacity = INIT_CAPACITY)
    {
        _id = IdProvider.Instance.RegisterWorld();
        _inputs = new();
        _events = new();
        _entities = new() { World = this };
        _components = new(initCapacity) { World = this };
        _systems = new() { World = this };
        _screens = new() { World = this };
    }
    
    /// <summary>
    /// Begins the update cycle for the world.
    /// Updates input and prepares the world for processing.
    /// </summary>
    /// <param name="gameTime">The current game time.</param>
    /// <exception cref="InvalidOperationException">Thrown when the world is not in a ready state for updates.</exception>
    public void BeginUpdate(GameTime gameTime)
    {
        if (_updateState != WorldState.ReadyToUpdate)
            throw new InvalidOperationException();
        
        GameTime = gameTime;
        _inputs?.Update(gameTime);
        _updateState = WorldState.Updating;
    }

    /// <summary>
    /// Ends the update cycle for the world.
    /// Processes entity removals and clears event state.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown when the world is not in an updating state.</exception>
    public void EndUpdate()
    {
        if (_updateState != WorldState.Updating)
            throw new InvalidOperationException();

        ReadOnlySpan<int> removals = Entities.Removals;

        for (int i = 0; i < removals.Length; i++)
        {
            Entities.Destroy(removals[i]);
        }
        
        Entities?.ClearRemovals();
        _events?.ClearEventState();

        _updateState = WorldState.ReadyToUpdate;
    }

    /// <summary>
    /// Destroys the world and cleans up all resources.
    /// </summary>
    public void Destroy()
    {
        _screens?.Destroy();
    }
}
