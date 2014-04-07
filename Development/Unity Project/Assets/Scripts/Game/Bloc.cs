// <copyright file="Bloc.cs" company="1WeekEndStudio">Copyright 1WeekEndStudio. All rights reserved.</copyright>

public class Bloc
{
    public Bloc(Tetromino tetromino, Position localPosition, bool isGhost)
    {
        this.Tetromino = tetromino;
        this.LocalPosition = localPosition;
        this.LastRegisteredPosition = Position.Invalid;
        this.IsGhost = isGhost;
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

    public bool IsGhost
    {
        get;
        private set;
    }
}
