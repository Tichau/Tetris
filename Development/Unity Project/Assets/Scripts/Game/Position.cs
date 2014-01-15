// <copyright file="Position.cs" company="BlobTeam">Copyright BlobTeam. All rights reserved.</copyright>

using System;

using UnityEngine;

public struct Position
{
    public static Position Invalid = new Position(-1, -1);

    public int X;
    public int Y;
    
    public Position(int x, int y)
    {
        this.X = x;
        this.Y = y;
    }

    public static Position operator +(Position a, Position b)
    {
        return new Position(a.X + b.X, a.Y + b.Y);
    }

    public static Position operator -(Position a, Position b)
    {
        return new Position(a.X - b.X, a.Y - b.Y);
    }

    public Position Rotate(Vector2 pivot, float angleInDegree)
    {
        Vector2 relativePosition = new Vector2(this.X - pivot.x, this.Y - pivot.y);

        if (Math.Abs(relativePosition.x) < float.Epsilon && Math.Abs(relativePosition.y) < float.Epsilon)
        {
            return new Position(Mathf.RoundToInt(pivot.x), Mathf.RoundToInt(pivot.y));
        }
        
        float atan2 = Mathf.Atan2(relativePosition.y, relativePosition.x);
        atan2 += angleInDegree * Mathf.Deg2Rad;
        float magnitude = relativePosition.magnitude;
        Vector2 result = new Vector2(magnitude * Mathf.Cos(atan2), magnitude * Mathf.Sin(atan2));

        return new Position(Mathf.RoundToInt(result.x + pivot.x), Mathf.RoundToInt(result.y + pivot.y));
    }
}
