// Copyright (c) 2024 Gm.Engine
// Author: Eric Boyd
// Licensed under the MIT License.
// See https://github.com/mtraindog/Gm.Engine/blob/main/LICENSE for details.

using System.Runtime.CompilerServices;

namespace Geist.Ecs.Collections;

/// <summary>
/// A two-dimensional array implementation that provides efficient access and manipulation of grid-like data.
/// </summary>
/// <typeparam name="T">The type of elements in the array.</typeparam>
public sealed class Array2D<T>(int rows, int cols)
{
    readonly int _rows = rows;
    readonly int _cols = cols;
    readonly int _capacity = rows * cols;
    readonly T[] _array = new T[rows * cols];

    /// <summary>
    /// Gets a span representing the entire array.
    /// </summary>
    /// <returns>A span containing all elements in the array.</returns>
    public Span<T> AsSpan() => _array.AsSpan();

    /// <summary>
    /// Gets the number of rows in the array.
    /// </summary>
    public int Rows => _rows;

    /// <summary>
    /// Gets the number of columns in the array.
    /// </summary>
    public int Cols => _cols;

    /// <summary>
    /// Gets the total capacity of the array (rows * columns).
    /// </summary>
    public int Capacity => _capacity;

    /// <summary>
    /// Calculates the linear index for a given row and column.
    /// </summary>
    /// <param name="row">The row index.</param>
    /// <param name="col">The column index.</param>
    /// <returns>The linear index in the underlying array.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int IndexOf(int row, int col) => row * _cols + col;

    /// <summary>
    /// Calculates the row and column indices for a given linear index.
    /// </summary>
    /// <param name="index">The linear index in the underlying array.</param>
    /// <returns>A tuple containing the row and column indices.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public (int row, int column) IndexOf(int index) => (index / _cols, index % _cols);    

    /// <summary>
    /// Gets the row index for a given linear index.
    /// </summary>
    /// <param name="index">The linear index in the underlying array.</param>
    /// <returns>The row index.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int RankOf(int index) => index / _cols;

    /// <summary>
    /// Gets the column index for a given linear index.
    /// </summary>
    /// <param name="index">The linear index in the underlying array.</param>
    /// <returns>The column index.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int FileOf(int index) => index % _cols;

    /// <summary>
    /// Gets a span representing a complete row in the array.
    /// </summary>
    /// <param name="row">The row index.</param>
    /// <returns>A span containing all elements in the specified row.</returns>
    public Span<T> Row(int row) => new(_array, row * _cols, _cols);

    /// <summary>
    /// Gets a span representing a portion of a row in the array.
    /// </summary>
    /// <param name="row">The row index.</param>
    /// <param name="startColumn">The starting column index.</param>
    /// <param name="count">The number of elements to include.</param>
    /// <returns>A span containing the specified elements from the row.</returns>
    public Span<T> Row(int row, int startColumn, int count) => Row(row).Slice(startColumn, count);

    /// <summary>
    /// Gets or sets the element at the specified row and column.
    /// </summary>
    /// <param name="row">The row index.</param>
    /// <param name="col">The column index.</param>
    /// <returns>The element at the specified position.</returns>
    public T this[int row, int col]
    {
        get => _array[row * _cols + col];
        set => _array[row * _cols + col] = value;
    }

    /// <summary>
    /// Gets or sets the element at the specified linear index.
    /// </summary>
    /// <param name="index">The linear index in the underlying array.</param>
    /// <returns>The element at the specified index.</returns>
    public T this[int index]
    {
        get => _array[index];
        set => _array[index] = value;
    }
}