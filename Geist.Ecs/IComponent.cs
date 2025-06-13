// Copyright (c) 2024 Gm.Engine
// Author: Eric Boyd
// Licensed under the MIT License.
// See https://github.com/mtraindog/Gm.Engine/blob/main/LICENSE for details.

namespace Geist.Ecs;

/// <summary>
/// Represents a component in the Entity Component System (ECS).
/// Components are data structures that can be attached to entities to provide specific functionality or attributes.
/// </summary>
public interface IComponent
{
    /// <summary>
    /// Gets or sets the unique identifier of the entity that this component is attached to.
    /// </summary>
    public int Id { get; set; }
}
