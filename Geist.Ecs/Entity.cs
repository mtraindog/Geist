// Copyright (c) 2024 Gm.Engine
// Author: Eric Boyd
// Licensed under the MIT License.
// See https://github.com/mtraindog/Gm.Engine/blob/main/LICENSE for details.

namespace Geist.Ecs;

/// <summary>
/// Represents an entity in the Entity Component System (ECS).
/// An entity is a unique identifier paired with a bitmask of its components.
/// </summary>
/// <param name="Id">The unique identifier of the entity.</param>
/// <param name="Components">A bitmask representing the components attached to this entity.</param>
public record struct Entity(int Id, ulong Components) : IComponent;