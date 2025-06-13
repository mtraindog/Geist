// Copyright (c) 2024 Gm.Engine
// Author: Eric Boyd
// Licensed under the MIT License.
// See https://github.com/mtraindog/Gm.Engine/blob/main/LICENSE for details.

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Gm.Engine.Collections;

/// <summary>
/// A collection that maintains a list of unique values while preserving insertion order.
/// Provides efficient lookup and iteration capabilities.
/// </summary>
/// <typeparam name="T">The type of elements in the bag.</typeparam>
public class DistinctBag<T>()
{
    readonly HashSet<T> _keys = [];
    readonly List<T> _values = [];

    /// <summary>
    /// Gets the number of elements in the bag.
    /// </summary>
    public int Count => _values.Count;

    /// <summary>
    /// Checks if the bag contains the specified element.
    /// </summary>
    /// <param name="key">The element to locate in the bag.</param>
    /// <returns>True if the element is found; otherwise, false.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Contains(T key) => _keys.Contains(key);

    /// <summary>
    /// Gets the element at the specified index.
    /// </summary>
    /// <param name="index">The zero-based index of the element to get.</param>
    /// <returns>The element at the specified index.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public T GetAt(int index) => _values[index];

    /// <summary>
    /// Gets the element at the specified index.
    /// </summary>
    /// <param name="index">The zero-based index of the element to get.</param>
    /// <returns>The element at the specified index.</returns>
    public T this[int index] => GetAt(index);

    /// <summary>
    /// Removes all elements from the bag.
    /// </summary>
    public void Clear()
    {
        _keys.Clear();
        _values.Clear();
    }

    /// <summary>
    /// Adds an element to the bag if it is not already present.
    /// </summary>
    /// <param name="key">The element to add.</param>
    /// <returns>True if the element was added; false if it was already present.</returns>
    public bool Add(T key)
    {
        if (_keys.Contains(key))
            return false;

        _keys.Add(key);
        _values.Add(key);
        return true;
    }

    /// <summary>
    /// Removes the specified element from the bag.
    /// </summary>
    /// <param name="key">The element to remove.</param>
    public void Remove(T key)
    {
        // TODO: Implement removal logic
    }

    /// <summary>
    /// Gets a read-only span containing all elements in the bag.
    /// </summary>
    public ReadOnlySpan<T> Values => CollectionsMarshal.AsSpan(_values)[..Count];
}
