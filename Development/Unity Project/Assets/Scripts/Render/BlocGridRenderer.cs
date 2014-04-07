// <copyright file="BlocGridRenderer.cs" company="1WeekEndStudio">Copyright 1WeekEndStudio. All rights reserved.</copyright>

using UnityEngine;

public class BlocGridRenderer : MonoBehaviour
{
    [SerializeField]
    private GameObject blocPrefab;

    [SerializeField]
    private BlocSpriteDescription[] spriteDescriptions;

    [SerializeField]
    private int tileSize;

    [SerializeField]
    private int tileMarginSize;

    private BlocGrid blocGrid;
    private float tileMarginOffset;

    private BlocRenderer[,] blocRenderers;
    private Sprite[] sprites;

    private bool initialized;

    public Rect RendererRect
    {
        get;
        private set;
    }

    public void OverrideBlocSpriteDescription(BlocSpriteDescription description)
    {
        for (int index = 0; index < this.spriteDescriptions.Length; index++)
        {
            if (this.spriteDescriptions[index].Color == description.Color)
            {
                this.spriteDescriptions[index] = description;
                break;
            }
        }
    }

    public void Initialize(BlocGrid grid, Vector2 offsetPosition)
    {
        this.blocGrid = grid;

        if (this.tileSize == 0)
        {
            Debug.LogError("The tile size is incorect.");
            return;
        }
        
        this.tileMarginOffset = (float)this.tileMarginSize / (float)this.tileSize;

        // Initialize sprites.
        this.sprites = new Sprite[this.spriteDescriptions.Length];
        for (int index = 0; index < this.spriteDescriptions.Length; index++)
        {
            BlocSpriteDescription spriteDescription = this.spriteDescriptions[index];
            if (spriteDescription.Sprite == null)
            {
                Debug.LogError("The sprite of the bloc " + spriteDescription.Color + " is null.");
            }

            this.sprites[(int)spriteDescription.Color] = spriteDescription.Sprite;
        }

        // Initialize blocRenderers.
        int rowCount = this.blocGrid.Height;
        int columnCount = this.blocGrid.Width;
        this.blocRenderers = new BlocRenderer[columnCount, rowCount];
        for (int y = 0; y < rowCount; ++y)
        {
            for (int x = 0; x < columnCount; ++x)
            {
                GameObject gameObject = GameObject.Instantiate(this.blocPrefab) as GameObject;
                gameObject.transform.position = new Vector3(offsetPosition.x + x - (x * this.tileMarginOffset), offsetPosition.y + y - (y * this.tileMarginOffset));
                gameObject.transform.parent = this.gameObject.transform;

                BlocRenderer blocRenderer = gameObject.GetComponent<BlocRenderer>();
                blocRenderer.Initialize(this.sprites);
                this.blocRenderers[x, y] = blocRenderer;
            }
        }

        // Compute the renderer's area.
        float width = columnCount - ((float)this.tileMarginSize / (float)this.tileSize * (columnCount - 1));
        float height = rowCount - ((float)this.tileMarginSize / (float)this.tileSize * (rowCount - 1));
        this.RendererRect = new Rect(-0.5f, -0.5f, width, height);

        this.initialized = true;
    }

    private void Update()
    {
        if (!this.initialized)
        {
            return;
        }
        
        for (int y = 0; y < this.blocGrid.Height; ++y)
        {
            for (int x = 0; x < this.blocGrid.Width; ++x)
            {
                Bloc bloc = this.blocGrid.Blocs[x, y];
                Vector3 position = this.blocRenderers[x, y].gameObject.transform.position;

                if (bloc == null)
                {
                    this.blocRenderers[x, y].SetColor(BlocSpriteDescription.BlocColor.Black);
                    
                    this.blocRenderers[x, y].gameObject.transform.position = new Vector3(position.x, position.y, 0);
                    continue;
                }

                if (bloc.Tetromino == null)
                {
                    continue;
                }

                this.blocRenderers[x, y].gameObject.transform.position = new Vector3(position.x, position.y, -1);

                if (bloc.IsGhost)
                {
                    this.blocRenderers[x, y].SetColor(BlocSpriteDescription.BlocColor.Ghost);
                    continue;
                }

                switch (bloc.Tetromino.Type)
                {
                    case Tetromino.TetrominoType.I:
                        this.blocRenderers[x, y].SetColor(BlocSpriteDescription.BlocColor.Cyan);
                        break;
                    case Tetromino.TetrominoType.O:
                        this.blocRenderers[x, y].SetColor(BlocSpriteDescription.BlocColor.Yellow);
                        break;
                    case Tetromino.TetrominoType.T:
                        this.blocRenderers[x, y].SetColor(BlocSpriteDescription.BlocColor.Purple);
                        break;
                    case Tetromino.TetrominoType.L:
                        this.blocRenderers[x, y].SetColor(BlocSpriteDescription.BlocColor.Orange);
                        break;
                    case Tetromino.TetrominoType.J:
                        this.blocRenderers[x, y].SetColor(BlocSpriteDescription.BlocColor.Blue);
                        break;
                    case Tetromino.TetrominoType.Z:
                        this.blocRenderers[x, y].SetColor(BlocSpriteDescription.BlocColor.Red);
                        break;
                    case Tetromino.TetrominoType.S:
                        this.blocRenderers[x, y].SetColor(BlocSpriteDescription.BlocColor.Green);
                        break;
                }
            }
        }
    }
}