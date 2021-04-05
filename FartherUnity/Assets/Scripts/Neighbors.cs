using System.Collections;
using System.Collections.Generic;

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