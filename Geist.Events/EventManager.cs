// Copyright (c) 2024 Gm.Engine
// Author: Eric Boyd
// Licensed under the MIT License.
// See https://github.com/mtraindog/Gm.Engine/blob/main/LICENSE for details.

namespace Geist.Events;

/// <summary>
/// Manages the event system, handling event triggers, raising, and handling.
/// Provides functionality for setting up event triggers, raising events, and processing event handlers.
/// </summary>
public sealed class EventManager
{
    readonly Dictionary<Type, List<(Func<bool>, IEventData)>> _triggers = [];
    readonly Dictionary<Type, IEventData> _raisedEvents = [];
    readonly HashSet<Type> _handledEvents = [];

    /// <summary>
    /// Sets a trigger for an event type with an optional event data.
    /// </summary>
    /// <typeparam name="TEvent">The type of event to trigger.</typeparam>
    /// <param name="predicate">The condition that must be met to trigger the event.</param>
    /// <param name="eventData">Optional data associated with the event.</param>
    public void SetTrigger<TEvent>(Func<bool> predicate, IEventData eventData = default)
        where TEvent : IEvent
    {
        Type type = typeof(TEvent);

        if (!_triggers.TryGetValue(type, out _))
            _triggers.Add(type, [(predicate, eventData)]);
        else
            _triggers[type].Add((predicate, eventData));
    }

    /// <summary>
    /// Sets multiple triggers for an event type.
    /// </summary>
    /// <typeparam name="TEvent">The type of event to trigger.</typeparam>
    /// <param name="triggers">Array of trigger conditions and their associated event data.</param>
    public void SetTriggers<TEvent>((Func<bool>, IEventData)[] triggers)
        where TEvent : IEvent
    {
        Type type = typeof(TEvent);

        foreach (var (predicate, eventData) in triggers)
        {
            if (!_triggers.TryGetValue(type, out _))
                _triggers.Add(type, [(predicate, eventData)]);
            else
                _triggers[type].Add((predicate, eventData));
        }
    }

    /// <summary>
    /// Raises an event with optional event data.
    /// </summary>
    /// <typeparam name="TEvent">The type of event to raise.</typeparam>
    /// <param name="eventData">Optional data associated with the event.</param>
    public void Raise<TEvent>(IEventData eventData = default)
        where TEvent : IEvent =>
            _raisedEvents.Add(typeof(TEvent), eventData);

    /// <summary>
    /// Marks an event as handled, preventing further processing.
    /// </summary>
    /// <typeparam name="TEvent">The type of event to mark as handled.</typeparam>
    public void Ignore<TEvent>()
        where TEvent : IEvent =>
            _handledEvents.Add(typeof(TEvent));

    /// <summary>
    /// Attempts to handle an event by checking its triggers.
    /// </summary>
    /// <typeparam name="TEvent">The type of event to handle.</typeparam>
    /// <returns>True if the event was handled, false otherwise.</returns>
    public bool TryHandle<TEvent>()
        where TEvent : IEvent
    {
        Type type = typeof(TEvent);

        if (_handledEvents.Contains(type))
            return false;

        if (_triggers.TryGetValue(type, out List<(Func<bool>, IEventData)> predicates))
        {
            for (int i = 0; i < predicates.Count; i++)
            {
                if (predicates[i].Item1.Invoke())
                {
                    Ignore<TEvent>();
                    return true;
                }
            }
        }
        return false;
    }

    /// <summary>
    /// Attempts to handle an event by checking its triggers and executing an action if triggered.
    /// </summary>
    /// <typeparam name="TEvent">The type of event to handle.</typeparam>
    /// <param name="action">The action to execute when the event is triggered.</param>
    /// <returns>True if the event was handled, false otherwise.</returns>
    public bool TryHandle<TEvent>(Action action)
        where TEvent : IEvent
    {
        Type type = typeof(TEvent);

        if (_handledEvents.Contains(type))
            return false;

        if (_triggers.TryGetValue(type, out List<(Func<bool>, IEventData)> predicates))
        {
            for (int i = 0; i < predicates.Count; i++)
            {
                if (predicates[i].Item1.Invoke())
                {
                    action.Invoke();
                    Ignore<TEvent>();
                    return true;
                }
            }
        }
        return false;
    }

    /// <summary>
    /// Attempts to handle an event by checking its triggers and executing an action with event data if triggered.
    /// </summary>
    /// <typeparam name="TEvent">The type of event to handle.</typeparam>
    /// <param name="action">The action to execute with event data when the event is triggered.</param>
    /// <returns>True if the event was handled, false otherwise.</returns>
    public bool TryHandle<TEvent>(Action<IEventData> action)
        where TEvent : IEvent
    {
        Type type = typeof(TEvent);

        if (_handledEvents.Contains(type))
            return false;

        if (_triggers.TryGetValue(type, out List<(Func<bool>, IEventData)> predicates))
        {
            for (int i = 0; i < predicates.Count; i++)
            {
                if (predicates[i].Item1.Invoke())
                {
                    action(predicates[i].Item2);
                    Ignore<TEvent>();
                    return true;
                }
            }
        }
        return false;
    }

    /// <summary>
    /// Clears all raised events and handled event states.
    /// </summary>
    public void ClearEventState()
    {
        _raisedEvents.Clear();
        _handledEvents.Clear();
    }

    /// <summary>
    /// Clears all event triggers.
    /// </summary>
    public void ClearTriggers()
    {
        foreach (Type type in _triggers.Keys)
            _triggers[type].Clear();

        _triggers.Clear();
    }

    /// <summary>
    /// Clears all triggers for a specific event type.
    /// </summary>
    /// <typeparam name="TEvent">The type of event whose triggers should be cleared.</typeparam>
    public void ClearTriggers<TEvent>() where TEvent : IEvent => _triggers[typeof(TEvent)].Clear();
} 