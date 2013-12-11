// <copyright file="Tetromino.cs" company="BlobTeam">Copyright BlobTeam. All rights reserved.</copyright>

using System;
using System.Collections;

using UnityEngine;

public class Tetromino
{
    public enum TetrominoType
    {
        I,
        O,
        T,
        L,
        J,
        Z,
        S
    }

    public Tetromino(TetrominoType type)
    {
        this.Type = type;
        this.Angle = 180;

        switch (this.Type)
        {
            case Tetromino.TetrominoType.I:
                this.Blocs = new Bloc[4];
                this.Blocs[0] = new Bloc(this, new Position(0, 0));
                this.Blocs[1] = new Bloc(this, new Position(1, 0));
                this.Blocs[2] = new Bloc(this, new Position(2, 0));
                this.Blocs[3] = new Bloc(this, new Position(3, 0));
                this.Pivot = new Vector2(1.5f, 0.5f);
                this.SpawnLocationOffset = new Position(0, 0);
                break;
            case Tetromino.TetrominoType.O:
                this.Blocs = new Bloc[4];
                this.Blocs[0] = new Bloc(this, new Position(0, 0));
                this.Blocs[1] = new Bloc(this, new Position(1, 0));
                this.Blocs[2] = new Bloc(this, new Position(0, 1));
                this.Blocs[3] = new Bloc(this, new Position(1, 1));
                this.Pivot = new Vector2(0.5f, 0.5f);
                this.SpawnLocationOffset = new Position(1, 0);
                break;
            case Tetromino.TetrominoType.T:
                this.Blocs = new Bloc[4];
                this.Blocs[0] = new Bloc(this, new Position(0, 0));
                this.Blocs[1] = new Bloc(this, new Position(1, 0));
                this.Blocs[2] = new Bloc(this, new Position(2, 0));
                this.Blocs[3] = new Bloc(this, new Position(1, 1));
                this.Pivot = new Vector2(1f, 0f);
                this.SpawnLocationOffset = new Position(0, 0);
                break;
            case Tetromino.TetrominoType.L:
                this.Blocs = new Bloc[4];
                this.Blocs[0] = new Bloc(this, new Position(0, 0));
                this.Blocs[1] = new Bloc(this, new Position(1, 0));
                this.Blocs[2] = new Bloc(this, new Position(2, 0));
                this.Blocs[3] = new Bloc(this, new Position(2, 1));
                this.Pivot = new Vector2(1f, 0f);
                this.SpawnLocationOffset = new Position(0, 0);
                break;
            case Tetromino.TetrominoType.J:
                this.Blocs = new Bloc[4];
                this.Blocs[0] = new Bloc(this, new Position(0, 0));
                this.Blocs[1] = new Bloc(this, new Position(1, 0));
                this.Blocs[2] = new Bloc(this, new Position(2, 0));
                this.Blocs[3] = new Bloc(this, new Position(0, 1));
                this.Pivot = new Vector2(1f, 0f);
                this.SpawnLocationOffset = new Position(0, 0);
                break;
            case Tetromino.TetrominoType.Z:
                this.Blocs = new Bloc[4];
                this.Blocs[0] = new Bloc(this, new Position(1, 0));
                this.Blocs[1] = new Bloc(this, new Position(2, 0));
                this.Blocs[2] = new Bloc(this, new Position(0, 1));
                this.Blocs[3] = new Bloc(this, new Position(1, 1));
                this.Pivot = new Vector2(1f, 0f);
                this.SpawnLocationOffset = new Position(0, 0);
                break;
            case Tetromino.TetrominoType.S:
                this.Blocs = new Bloc[4];
                this.Blocs[0] = new Bloc(this, new Position(0, 0));
                this.Blocs[1] = new Bloc(this, new Position(1, 0));
                this.Blocs[2] = new Bloc(this, new Position(1, 1));
                this.Blocs[3] = new Bloc(this, new Position(2, 1));
                this.Pivot = new Vector2(1f, 0f);
                this.SpawnLocationOffset = new Position(0, 0);
                break;
        }
    }

    public float Angle
    {
        get;
        set;
    }

    public Vector2 Pivot
    {
        get; 
        private set;
    }

    public Bloc[] Blocs
    {
        get;
        private set;
    }

    public Position Position
    {
        get;
        set;
    }

    public Position SpawnLocationOffset
    {
        get;
        private set;
    }

    public TetrominoType Type
    {
        get;
        private set;
    }
}
