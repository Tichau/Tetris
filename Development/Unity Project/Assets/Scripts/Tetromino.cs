﻿// <copyright file="Tetromino.cs" company="BlobTeam">Copyright BlobTeam. All rights reserved.</copyright>

using System;
using System.Collections;

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

        switch (this.Type)
        {
            case Tetromino.TetrominoType.I:
                this.Blocs = new Bloc[4];
                this.Blocs[0] = new Bloc(this, new Position(0, 0));
                this.Blocs[1] = new Bloc(this, new Position(1, 0));
                this.Blocs[2] = new Bloc(this, new Position(2, 0));
                this.Blocs[3] = new Bloc(this, new Position(3, 0));
                break;
            case Tetromino.TetrominoType.O:
                this.Blocs = new Bloc[4];
                this.Blocs[0] = new Bloc(this, new Position(0, 0));
                this.Blocs[1] = new Bloc(this, new Position(1, 0));
                this.Blocs[2] = new Bloc(this, new Position(0, 1));
                this.Blocs[3] = new Bloc(this, new Position(1, 1));
                break;
            case Tetromino.TetrominoType.T:
                this.Blocs = new Bloc[4];
                this.Blocs[0] = new Bloc(this, new Position(0, 0));
                this.Blocs[1] = new Bloc(this, new Position(1, 0));
                this.Blocs[2] = new Bloc(this, new Position(2, 0));
                this.Blocs[3] = new Bloc(this, new Position(1, 1));
                break;
            case Tetromino.TetrominoType.L:
                this.Blocs = new Bloc[4];
                this.Blocs[0] = new Bloc(this, new Position(0, 0));
                this.Blocs[1] = new Bloc(this, new Position(1, 0));
                this.Blocs[2] = new Bloc(this, new Position(2, 0));
                this.Blocs[3] = new Bloc(this, new Position(2, 1));
                break;
            case Tetromino.TetrominoType.J:
                this.Blocs = new Bloc[4];
                this.Blocs[0] = new Bloc(this, new Position(0, 0));
                this.Blocs[1] = new Bloc(this, new Position(1, 0));
                this.Blocs[2] = new Bloc(this, new Position(2, 0));
                this.Blocs[3] = new Bloc(this, new Position(0, 1));
                break;
            case Tetromino.TetrominoType.Z:
                this.Blocs = new Bloc[4];
                this.Blocs[0] = new Bloc(this, new Position(1, 0));
                this.Blocs[1] = new Bloc(this, new Position(2, 0));
                this.Blocs[2] = new Bloc(this, new Position(0, 1));
                this.Blocs[3] = new Bloc(this, new Position(1, 1));
                break;
            case Tetromino.TetrominoType.S:
                this.Blocs = new Bloc[4];
                this.Blocs[0] = new Bloc(this, new Position(0, 0));
                this.Blocs[1] = new Bloc(this, new Position(1, 0));
                this.Blocs[2] = new Bloc(this, new Position(1, 1));
                this.Blocs[3] = new Bloc(this, new Position(2, 1));
                break;
        }
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

    public TetrominoType Type
    {
        get;
        private set;
    }
}
