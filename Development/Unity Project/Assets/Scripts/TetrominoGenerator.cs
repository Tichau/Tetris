// <copyright file="TetrominoGenerator.cs" company="BlobTeam">Copyright BlobTeam. All rights reserved.</copyright>

using System;
using System.Collections.Generic;

public class TetrominoGenerator
{
    private readonly List<Tetromino.TetrominoType> tetrominoPool = new List<Tetromino.TetrominoType>(7);

    private readonly Tetromino.TetrominoType[] tetrominoTypes;

    public TetrominoGenerator()
    {
        Array values = System.Enum.GetValues(typeof(Tetromino.TetrominoType));
        this.tetrominoTypes = new Tetromino.TetrominoType[values.Length];
        for (int index = 0; index < values.Length; index++)
        {
            this.tetrominoTypes[index] = (Tetromino.TetrominoType)values.GetValue(index);
        }

        this.RefillPool();
    }

    public Tetromino PickNewTetromino()
    {
        Tetromino.TetrominoType tetrominoType = this.tetrominoPool[UnityEngine.Random.Range(0, this.tetrominoPool.Count)];
        this.tetrominoPool.Remove(tetrominoType);
        if (this.tetrominoPool.Count == 0)
        {
            this.RefillPool();
        }

        return new Tetromino(tetrominoType);
    }

    private void RefillPool()
    {
        this.tetrominoPool.Clear();
        this.tetrominoPool.AddRange(this.tetrominoTypes);
    }

    public void Reset()
    {
        this.RefillPool();
    }
}
