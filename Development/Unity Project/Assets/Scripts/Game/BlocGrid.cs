// <copyright file="BlocGrid.cs" company="1WeekEndStudio">Copyright 1WeekEndStudio. All rights reserved.</copyright>

public class BlocGrid
{
    public BlocGrid(int width, int height)
    {
        this.Width = width;
        this.Height = height;
        this.Blocs = new Bloc[this.Width, this.Height];
    }

    public Bloc[,] Blocs
    {
        get;
        private set;
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

    public void Clear()
    {
        for (int x = 0; x < this.Width; x++)
        {
            for (int y = 0; y < this.Height; y++)
            {
                this.Blocs[x, y] = null;
            }
        }
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

    public void ClearTetromino(Tetromino tetromino)
    {
        for (int index = 0; index < tetromino.Blocs.Length; index++)
        {
            Bloc tetrominoBloc = tetromino.Blocs[index];
            Bloc currentBloc = this.GetBloc(tetrominoBloc.LastRegisteredPosition);
            if (currentBloc == null || currentBloc.Tetromino != tetromino)
            {
                continue;
            }

            this.SetBloc(tetrominoBloc.LastRegisteredPosition, null);
        }
    }

    public void SetTetromino(Tetromino tetromino)
    {
        for (int index = 0; index < tetromino.Blocs.Length; index++)
        {
            Bloc tetrominoBloc = tetromino.Blocs[index];
            this.SetBloc(tetrominoBloc.Position, tetrominoBloc);
        }
    }

    protected void SetBloc(Position position, Bloc bloc)
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
}
