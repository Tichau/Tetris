// <copyright file="TetrominoGenerator.cs" company="BlobTeam">Copyright BlobTeam. All rights reserved.</copyright>

using System;
using System.Collections.Generic;

public class TetrominoGenerator
{
    private readonly List<Tetromino.TetrominoType> tetrominoPool = new List<Tetromino.TetrominoType>(7);

    private readonly Tetromino.TetrominoType[] tetrominoTypes;
    private int previewCount;

    public TetrominoGenerator(int previewCount)
    {
        Array values = System.Enum.GetValues(typeof(Tetromino.TetrominoType));
        this.tetrominoTypes = new Tetromino.TetrominoType[values.Length];
        for (int index = 0; index < values.Length; index++)
        {
            this.tetrominoTypes[index] = (Tetromino.TetrominoType)values.GetValue(index);
        }

        this.previewCount = previewCount;

        this.RefillPool();

        this.NextTetrominos = new Queue<Tetromino.TetrominoType>(this.previewCount);
        for (int index = 0; index < this.previewCount; ++index)
        {
            this.NextTetrominos.Enqueue(this.PickNewTetromino());
        }
    }

    public Queue<Tetromino.TetrominoType> NextTetrominos
    {
        get;
        private set;
    }

    public Tetromino PickNextTetromino()
    {
        Tetromino.TetrominoType nextTetrominoType = this.NextTetrominos.Dequeue();

        this.NextTetrominos.Enqueue(this.PickNewTetromino());

        return new Tetromino(nextTetrominoType, false);
    }

    public void Reset()
    {
        this.RefillPool();
    }

    private Tetromino.TetrominoType PickNewTetromino()
    {
        Tetromino.TetrominoType tetrominoType = this.tetrominoPool[UnityEngine.Random.Range(0, this.tetrominoPool.Count)];
        this.tetrominoPool.Remove(tetrominoType);
        if (this.tetrominoPool.Count == 0)
        {
            this.RefillPool();
        }

        return tetrominoType;
    }

    private void RefillPool()
    {
        this.tetrominoPool.Clear();
        this.tetrominoPool.AddRange(this.tetrominoTypes);
    }
}
