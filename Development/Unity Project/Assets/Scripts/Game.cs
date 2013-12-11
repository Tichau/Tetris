// <copyright file="Game.cs" company="BlobTeam">Copyright BlobTeam. All rights reserved.</copyright>

using System;
using System.Collections;
using System.Linq;

using UnityEngine;

public class Game
{
    private readonly int[] pointsAtLevel0 = new[] { 40, 100, 300, 1200 };

    private float lastTetrominoTime = float.MinValue;

    private float speed = 0.0f;

    private TetrominoGenerator tetrominoGenerator = new TetrominoGenerator();

    public Game(int width, int height)
    {
        this.Width = width;
        this.Height = height;
        this.Blocs = new Bloc[this.Width, this.Height];
    }

    public GameStatistics Statistics
    {
        get; 
        private set;
    }

    public float SpeedOverride
    {
        get; 
        set;
    }

    public float Speed
    {
        get
        {
            if (this.SpeedOverride > 0f)
            {
                return this.SpeedOverride;
            }

            return this.speed;
        }
    }

    public int Height
    {
        get; 
        private set;
    }

    public int Width
    {
        get;
        private set;
    }

    public Bloc[,] Blocs
    {
        get;
        private set;
    }

    public Tetromino CurrentTetromino 
    { 
        get; 
        private set; 
    }

    public void StartGame()
    {
        this.Statistics = new GameStatistics();
        this.speed = this.GetSpeed(this.Statistics.Level);
        this.tetrominoGenerator.Reset();
    }

    public Bloc GetBloc(Position position)
    {
        if (position.Y < 0 || position.Y >= this.Height)
        {
            return null;
        }

        if (position.X < 0 || position.X >= this.Width)
        {
            return null;
        }

        return this.Blocs[position.X, position.Y];
    }

    public void NewTetromino()
    {
        Tetromino tetromino = this.tetrominoGenerator.PickNewTetromino();
        Position originSpawnLocation = new Position((this.Width / 2) - 2, this.Height) + tetromino.SpawnLocationOffset;
        tetromino.Position = originSpawnLocation;
        this.CurrentTetromino = tetromino;

        for (int index = 0; index < this.CurrentTetromino.Blocs.Length; index++)
        {
            Bloc tetrominoBloc = this.CurrentTetromino.Blocs[index];
            this.SetBloc(tetrominoBloc.Position, tetrominoBloc);
        }
    }

    public void Right()
    {
        if (this.CurrentTetromino == null)
        {
            return;
        }

        for (int index = 0; index < this.CurrentTetromino.Blocs.Length; index++)
        {
            Bloc bloc = this.CurrentTetromino.Blocs[index];
            bool isAtRight = bloc.Position.X == this.Width - 1;

            Bloc blocAtRight = this.GetBloc(new Position(bloc.Position.X + 1, bloc.Position.Y));
            isAtRight |= blocAtRight != null && blocAtRight.Tetromino != bloc.Tetromino;

            if (isAtRight)
            {
                return;
            }
        }

        this.CurrentTetromino.Position = new Position(this.CurrentTetromino.Position.X + 1, this.CurrentTetromino.Position.Y);
    }

    public void Left()
    {
        if (this.CurrentTetromino == null)
        {
            return;
        }

        for (int index = 0; index < this.CurrentTetromino.Blocs.Length; index++)
        {
            Bloc bloc = this.CurrentTetromino.Blocs[index];
            bool isAtLeft = bloc.Position.X == 0;

            Bloc blocAtLeft = this.GetBloc(new Position(bloc.Position.X - 1, bloc.Position.Y));
            isAtLeft |= blocAtLeft != null && blocAtLeft.Tetromino != bloc.Tetromino;

            if (isAtLeft)
            {
                return;
            }
        }

        this.CurrentTetromino.Position = new Position(this.CurrentTetromino.Position.X - 1, this.CurrentTetromino.Position.Y);
    }

    public void RotateRight()
    {
        if (this.CurrentTetromino == null)
        {
            return;
        }

        int angle = (int)this.CurrentTetromino.Angle;
        this.CurrentTetromino.Angle = (angle + 90) % 360;

        bool isSomeBlocOnNewPosition = false;
        for (int index = 0; index < this.CurrentTetromino.Blocs.Length; index++)
        {
            Bloc bloc = this.CurrentTetromino.Blocs[index];

            isSomeBlocOnNewPosition |= bloc.Position.X < 0 || bloc.Position.X >= this.Width || bloc.Position.Y < 0;

            Bloc blocAtPosition = this.GetBloc(bloc.Position);
            isSomeBlocOnNewPosition |= blocAtPosition != null && blocAtPosition.Tetromino != bloc.Tetromino;

            if (isSomeBlocOnNewPosition)
            {
                break;
            }
        }

        if (isSomeBlocOnNewPosition)
        {
            this.CurrentTetromino.Angle = angle;
        }
    }

    public void Update(float deltaTime)
    {
        if (this.CurrentTetromino == null)
        {
            return;
        }

        float time = Time.time;
        bool fall = time - this.lastTetrominoTime >= 1f / this.Speed;

        if (fall)
        {
            bool isAtTheBottom = false;
            for (int index = 0; index < this.CurrentTetromino.Blocs.Length; index++)
            {
                Bloc tetrominoBloc = this.CurrentTetromino.Blocs[index];
                isAtTheBottom |= tetrominoBloc.Position.Y == 0;

                Bloc blocUnder = this.GetBloc(new Position(tetrominoBloc.Position.X, tetrominoBloc.Position.Y - 1));
                isAtTheBottom |= blocUnder != null && blocUnder.Tetromino != tetrominoBloc.Tetromino;

                if (isAtTheBottom)
                {
                    if (this.CheckDefeat())
                    {
                        this.speed = 0f;
                        this.CurrentTetromino = null;
                        return;
                    }

                    this.CurrentTetromino = null;

                    this.CheckLines();

                    this.CheckLevel();

                    this.NewTetromino();
                    return;
                }
            }
        }

        // Clear old positions.
        for (int index = 0; index < this.CurrentTetromino.Blocs.Length; index++)
        {
            Bloc tetrominoBloc = this.CurrentTetromino.Blocs[index];
            this.SetBloc(tetrominoBloc.LastRegisteredPosition, null);
        }

        if (fall)
        {
            this.CurrentTetromino.Position = new Position(this.CurrentTetromino.Position.X, this.CurrentTetromino.Position.Y - 1);
            this.lastTetrominoTime = time;
        }

        for (int index = 0; index < this.CurrentTetromino.Blocs.Length; index++)
        {
            Bloc tetrominoBloc = this.CurrentTetromino.Blocs[index];
            this.SetBloc(tetrominoBloc.Position, tetrominoBloc);
        }
    }

    private void CheckLevel()
    {
        int newLevel = this.Statistics.Lines / 10;
        if (this.Statistics.Level != newLevel)
        {
            // level up !
            this.Statistics.Level = newLevel;
            this.speed = this.GetSpeed(newLevel);
        }
    }

    private bool CheckDefeat()
    {
        if (this.CurrentTetromino.Blocs.Any(bloc => bloc.Position.Y >= this.Height))
        {
            Debug.Log("DEFEAT !");
            return true;
        }

        return false;
    }

    private void CheckLines()
    {
        int lineCount = 0;
        for (int y = 0; y < this.Height; ++y)
        {
            bool lineComplete = true;
            for (int x = 0; x < this.Width; ++x)
            {
                Bloc bloc = this.GetBloc(new Position(x, y));
                lineComplete &= bloc != null;

                if (!lineComplete)
                {
                    break;
                }
            }

            if (lineComplete)
            {
                // Fall tetrominos.
                for (int y2 = y; y2 < this.Height; ++y2)
                {
                    for (int x = 0; x < this.Width; ++x)
                    {
                        this.SetBloc(new Position(x, y2), this.GetBloc(new Position(x, y2 + 1)));
                    }
                }

                y--;
                lineCount++;
            }
        }

        if (lineCount > 0)
        {
            this.Statistics.Lines += lineCount;
            this.Statistics.Score += this.GetScore(lineCount, this.Statistics.Level);
        }
    }

    private void SetBloc(Position position, Bloc bloc)
    {
        if (position.Y < 0 || position.Y >= this.Height)
        {
            return;
        }

        if (position.X < 0 || position.X >= this.Width)
        {
            return;
        }

        if (bloc != null)
        {
            bloc.LastRegisteredPosition = position;
        }

        this.Blocs[position.X, position.Y] = bloc;
    }

    private int GetScore(int numberOfLines, int level)
    {
        if (numberOfLines < 1 || numberOfLines > 4)
        {
            throw new ArgumentOutOfRangeException("numberOfLines", "The number of lines parameters must be 1, 2, 3 or 4 integer value.");
        }

        return this.pointsAtLevel0[numberOfLines - 1] * (level + 1);
    }

    private float GetSpeed(int level)
    {
        return 1.25f; // lvl 0
        //return 30f; // lvl 19
    }
}