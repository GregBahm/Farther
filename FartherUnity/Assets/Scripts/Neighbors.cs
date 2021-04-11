using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

public class Neighbors<T> : IEnumerable<T>
{
    private readonly List<T> values;

    public T UpRight { get; }
    public T Right { get; }
    public T DownRight { get; }
    public T DownLeft { get; }
    public T Left { get; }
    public T UpLeft { get; }

    public Neighbors(
        T upRight, 
        T right, 
        T downRight, 
        T downLeft, 
        T left, 
        T upLeft)
    {
        UpRight = upRight;
        Right = right;
        DownRight = downRight;
        DownLeft = downLeft;
        Left = left;
        UpLeft = upLeft;
        values = new List<T>() { UpRight, Right, DownRight, DownLeft, Left, UpLeft };
    }

    public IEnumerator<T> GetEnumerator()
    {
        return values.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}

public struct NeighborOffset
{
    public static ReadOnlyCollection<NeighborOffset> Offsets { get; } = new List<NeighborOffset>()
    {
        new NeighborOffset(0, -1),
        new NeighborOffset(-1, 0),
        new NeighborOffset(-1, 1),
        new NeighborOffset(0, 1),
        new NeighborOffset(1, 0),
        new NeighborOffset(1, -1),
    }.AsReadOnly();

    public int X { get; }
    public int Y { get; }

    public NeighborOffset(int x, int y)
    {
        X = x;
        Y = y;
    }
}