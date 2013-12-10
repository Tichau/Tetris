// <copyright file="Position.cs" company="BlobTeam">Copyright BlobTeam. All rights reserved.</copyright>

public struct Position
{
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
}
