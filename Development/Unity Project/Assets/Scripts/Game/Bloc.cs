// <copyright file="Bloc.cs" company="BlobTeam">Copyright BlobTeam. All rights reserved.</copyright>

using System.Collections;

public class Bloc
{
    public Bloc(Tetromino tetromino, Position localPosition)
    {
        this.Tetromino = tetromino;
        this.LocalPosition = localPosition;
        this.LastRegisteredPosition = Position.Invalid;
    }

    public Position LastRegisteredPosition
    {
        get;
        set;
    }

    public Position LocalPosition
    {
        get;
        private set;
    }

    public Position Position
    {
        get
        {
            if (Tetromino == null)
            {
                return Position.Invalid;
            }

            return this.Tetromino.Position + this.LocalPosition.Rotate(this.Tetromino.Pivot, this.Tetromino.Angle);
        }
    }

    public Tetromino Tetromino
    {
        get; 
        private set;
    }
}
