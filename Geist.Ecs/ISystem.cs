// Copyright (c) 2024 Gm.Engine
// Author: Eric Boyd
// Licensed under the MIT License.
// See https://github.com/mtraindog/Gm.Engine/blob/main/LICENSE for details.

using GameTime = Microsoft.Xna.Framework.GameTime;

namespace Geist.Ecs;

/// <summary>
/// Defines the interface for systems in the Entity Component System (ECS).
/// Systems are responsible for processing entities and their components.
/// </summary>
public interface ISystem
{
    /// <summary>
    /// Gets the world instance that this system belongs to.
    /// </summary>
    World World { get; }

    /// <summary>
    /// Called when the system is first created.
    /// Use this method to perform any initialization logic.
    /// </summary>
    void Initialize();

    /// <summary>
    /// Called every frame to update the system's logic.
    /// </summary>
    /// <param name="gameTime">Provides timing information for the current frame.</param>
    void Update(GameTime gameTime);

    /// <summary>
    /// Called when the system is being destroyed.
    /// Use this method to perform any cleanup logic.
    /// </summary>
    void Destroy();
}
