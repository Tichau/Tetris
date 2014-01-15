// <copyright file="Game.cs" company="BlobTeam">Copyright BlobTeam. All rights reserved.</copyright>

using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

public class Game : BlocGrid
{
    private readonly int[] pointsAtLevel0 = new[] { 40, 100, 300, 1200 };

    private readonly float[] speedByLevel = new float[]
                                   {
                                       1.126981132075472f, 
                                       1.218979591836735f,
                                       1.327333333333333f,
                                       1.456829268292683f,
                                       1.614324324324324f,
                                       1.81f,
                                       2.133214285714286f,
                                       2.715f,
                                       3.513529411764706f,
                                       5.43f,
                                       5.973f,
                                       6.636666666666667f,
                                       7.46625f,
                                       8.532857142857143f,
                                       9.955f,
                                       9.955f,
                                       11.946f,
                                       11.946f,
                                       14.9325f,
                                       14.9325f,
                                       19.91f
                                   };

    private float lastTetrominoTime = float.MinValue;
    private float speed = 0.0f;
    private TetrominoGenerator tetrominoGenerator = new TetrominoGenerator(1);
    private int startLevel = 0;
    
    public Game(int width, int height)
        : base(width, height)
    {
    }

    public event EventHandler CurrentTetrominoChange;

    public Queue<Tetromino.TetrominoType> NextTetrominos
    {
        get { return this.tetrominoGenerator.NextTetrominos; }
    }

    public bool IsGameStarted
    {
        get;
        private set;
    }

    public bool IsPaused
    {
        get;
        private set;
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

    public Tetromino CurrentTetromino 
    { 
        get; 
        private set; 
    }

    public void StartGame(int startLevel = 0)
    {
        this.Clear();

        this.startLevel = startLevel;

        this.Statistics = new GameStatistics();
        this.Statistics.Level = startLevel;
        this.SetSpeed(this.GetSpeed(this.Statistics.Level));
        this.tetrominoGenerator.Reset();
        this.NewTetromino();
        this.IsGameStarted = true;
    }

    public void NewTetromino()
    {
        Tetromino tetromino = this.tetrominoGenerator.PickNextTetromino();
        Position originSpawnLocation = new Position((this.Width / 2) - 2, this.Height) + tetromino.SpawnLocationOffset;
        tetromino.Position = originSpawnLocation;
        this.CurrentTetromino = tetromino;

        for (int index = 0; index < this.CurrentTetromino.Blocs.Length; index++)
        {
            Bloc tetrominoBloc = this.CurrentTetromino.Blocs[index];
            this.SetBloc(tetrominoBloc.Position, tetrominoBloc);
        }

        if (this.CurrentTetrominoChange != null)
        {
            this.CurrentTetrominoChange.Invoke(this, null);
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

        this.MoveCurrentTetromino(new Position(this.CurrentTetromino.Position.X + 1, this.CurrentTetromino.Position.Y), this.CurrentTetromino.Angle);
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

        this.MoveCurrentTetromino(new Position(this.CurrentTetromino.Position.X - 1, this.CurrentTetromino.Position.Y), this.CurrentTetromino.Angle);
    }

    public void RotateRight()
    {
        if (this.CurrentTetromino == null)
        {
            return;
        }

        int angle = (int)this.CurrentTetromino.Angle;
        float newAngle = (angle + 90) % 360;
        this.CurrentTetromino.Angle = newAngle;

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

        this.CurrentTetromino.Angle = angle;

        if (!isSomeBlocOnNewPosition)
        {
            this.MoveCurrentTetromino(this.CurrentTetromino.Position, newAngle);
        }
    }

    public void MoveCurrentTetromino(Position newPosition, float angle)
    {
        // Clear old positions.
        this.ClearTetromino(this.CurrentTetromino);

        this.CurrentTetromino.Position = newPosition;
        this.CurrentTetromino.Angle = angle;

        this.SetTetromino(this.CurrentTetromino);
    }

    public void Pause()
    {
        if (!this.IsGameStarted)
        {
            this.StartGame();
            return;
        }

        if (this.IsPaused)
        {
            this.IsPaused = false;
            this.speed = this.GetSpeed(this.Statistics.Level);
        }
        else
        {
            this.IsPaused = true;
            this.speed = 0;
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
                        this.EndGame();
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

    private void EndGame()
    {
        this.SpeedOverride = -1f;
        if (HighScores.CanRegisterGameStatistic(this.Statistics))
        {
            InputManager.Instance.RegisterGameStatistic(this.Statistics);
        }

        this.speed = 0f;
        this.CurrentTetromino = null;
        this.IsGameStarted = false;
    }

    private void CheckLevel()
    {
        int newLevel = (this.Statistics.Lines / 10) + this.startLevel;
        if (this.Statistics.Level != newLevel)
        {
            // level up !
            this.Statistics.Level = newLevel;
            this.SetSpeed(this.GetSpeed(newLevel));
        }
    }

    private bool CheckDefeat()
    {
        if (this.CurrentTetromino.Blocs.Any(bloc => bloc.Position.Y >= this.Height))
        {
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

    private void SetSpeed(float newSpeed)
    {
        this.speed = newSpeed;
        InputManager.Instance.DurationBetweenMoveInputs = 1f / (5f * newSpeed);
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
        if (level >= this.speedByLevel.Length)
        {
            return this.speedByLevel[this.speedByLevel.Length - 1];
        }

        return this.speedByLevel[level];
    }
}