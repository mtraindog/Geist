// Copyright (c) 2024 Gm.Engine
// Author: Eric Boyd
// Licensed under the MIT License.
// See https://github.com/mtraindog/Gm.Engine/blob/main/LICENSE for details.

namespace Geist.Ecs;

/// <summary>
/// Represents the current state of a World instance.
/// Used to track and validate the update cycle of the world.
/// </summary>
internal enum WorldState
{
    /// <summary>
    /// The world is ready to begin an update cycle.
    /// </summary>
    ReadyToUpdate,

    /// <summary>
    /// The world is currently in the process of updating.
    /// </summary>
    Updating
}
