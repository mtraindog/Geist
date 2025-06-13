// Copyright (c) 2024 Gm.Engine
// Author: Eric Boyd
// Licensed under the MIT License.
// See https://github.com/mtraindog/Gm.Engine/blob/main/LICENSE for details.

using Microsoft.Xna.Framework;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Gm.Engine.Collections;

/// <summary>
/// A spatial hash grid implementation that divides a 2D field into cells for efficient spatial queries.
/// </summary>
/// <typeparam name="T">The type of data to store in the spatial hash grid.</typeparam>
public class Hash2D<T>
{
    readonly int _numRows;
    readonly int _numCols;
    readonly int _fieldWidth;
    readonly int _fieldHeight;
    readonly int _cellWidth; 
    readonly int _cellHeight;

    readonly DistinctBag<int> _hashKeys = new();
    readonly Array2D<List<T>> _grid;

    /// <summary>
    /// Initializes a new instance of the Hash2D class with the specified dimensions.
    /// </summary>
    /// <param name="numRows">The number of rows in the grid.</param>
    /// <param name="numCols">The number of columns in the grid.</param>
    /// <param name="fieldWidth">The total width of the field.</param>
    /// <param name="fieldHeight">The total height of the field.</param>
    public Hash2D(int numRows, int numCols, int fieldWidth, int fieldHeight)
    {
        _numRows = numRows;
        _numCols = numCols;
        _fieldWidth = fieldWidth;
        _fieldHeight = fieldHeight;
        _cellWidth = fieldWidth / numCols;
        _cellHeight = fieldHeight / numRows;
        _grid = new(numRows, numCols);

        for (int i = 0; i < _grid.Capacity; i++)
            _grid[i] = [];
    }

    /// <summary>
    /// Gets a read-only span of all hash keys in the grid.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ReadOnlySpan<int> GetKeys() => _hashKeys.Values;

    /// <summary>
    /// Gets a read-only span of all hash keys in the grid.
    /// </summary>
    public ReadOnlySpan<int> Keys => GetKeys();

    /// <summary>
    /// Gets a span of all cells in the grid.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Span<List<T>> GetSpan() => _grid.AsSpan();

    /// <summary>
    /// Gets a span of all cells in the grid.
    /// </summary>
    public Span<List<T>> AsSpan() => GetSpan();

    /// <summary>
    /// Gets a span of elements in the specified cell.
    /// </summary>
    /// <param name="index">The cell index.</param>
    /// <returns>A span containing the elements in the cell, or an empty span if the index is invalid.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Span<T> GetCell(int index) => index < 0 ? [] : CollectionsMarshal.AsSpan(_grid[index]);

    /// <summary>
    /// Gets a span of elements in the specified cell.
    /// </summary>
    /// <param name="index">The cell index.</param>
    /// <returns>A span containing the elements in the cell, or an empty span if the index is invalid.</returns>
    public Span<T> this[int index] => GetCell(index);

    /// <summary>
    /// Calculates the hash (cell index) for a given position in the field.
    /// </summary>
    /// <param name="pos">The position to calculate the hash for.</param>
    /// <returns>The cell index, or -1 if the position is outside the field bounds.</returns>
    public int CalculateHash(Vector2 pos)
    {
        if (pos.X < 0 || pos.X > _fieldWidth || pos.Y < 0 || pos.Y > _fieldHeight)
            return -1;

        return _grid.IndexOf((int)pos.Y / _cellHeight % _numRows, (int)pos.X / _cellWidth % _numCols);
    }

    /// <summary>
    /// Removes all elements from all cells in the grid.
    /// </summary>
    public void Clear()
    {
        for (int i = 0; i < _grid.Capacity; i++)
            _grid[i].Clear();
    }

    /// <summary>
    /// Adds an element to the cell containing the specified position.
    /// </summary>
    /// <param name="position">The position to add the element at.</param>
    /// <param name="data">The element to add.</param>
    /// <returns>The cell index where the element was added, or -1 if the position is invalid.</returns>
    public int Add(Vector2 position, T data)
    {
        int index = CalculateHash(position);

        if (index < 0)
            return index;

        _hashKeys.Add(index);
        _grid[index].Add(data);
        return index;
    }
}



