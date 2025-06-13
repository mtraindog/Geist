// Copyright (c) 2024 Gm.Engine
// Author: Eric Boyd
// Licensed under the MIT License.
// See https://github.com/mtraindog/Gm.Engine/blob/main/LICENSE for details.

using GameTime = Microsoft.Xna.Framework.GameTime;

namespace Geist.Ecs;

/// <summary>
/// Base class for all systems in the Entity Component System (ECS).
/// Provides common functionality and lifecycle methods for systems.
/// </summary>
public abstract class SystemBase : ISystem
{
    /// <summary>
    /// Gets the world instance that this system belongs to.
    /// </summary>
    public World World { get; internal init; }

    /// <summary>
    /// Called when the system is first created.
    /// Override this method to perform any initialization logic.
    /// </summary>
    public virtual void Initialize() { }

    /// <summary>
    /// Called every frame to update the system's logic.
    /// </summary>
    /// <param name="gameTime">Provides timing information for the current frame.</param>
    public virtual void Update(GameTime gameTime) { }

    /// <summary>
    /// Called when the system is being destroyed.
    /// Override this method to perform any cleanup logic.
    /// </summary>
    public virtual void Destroy() { }
}
