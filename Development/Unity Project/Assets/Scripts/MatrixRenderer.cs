// <copyright file="MatrixRenderer.cs" company="BlobTeam">Copyright BlobTeam. All rights reserved.</copyright>

using UnityEngine;

public class MatrixRenderer : MonoBehaviour
{
    [UnityEngine.SerializeField]
    private GameObject blocPrefab;

    [UnityEngine.SerializeField]
    private BlocSpriteDescription[] spriteDescriptions;

    [UnityEngine.SerializeField]
    private int tileSize;

    [UnityEngine.SerializeField]
    private int tileMarginSize;

    private Matrix Matrix;
    private float tileMarginOffset;

    private BlocRenderer[,] blocRenderers;
    private Sprite[] sprites;

    private bool initialized;

    public void Initialize(Matrix matrix)
    {
        this.Matrix = matrix;

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
        this.blocRenderers = new BlocRenderer[this.Matrix.Width, this.Matrix.Height];
        for (int y = 0; y < this.Matrix.Height; ++y)
        {
            for (int x = 0; x < this.Matrix.Width; ++x)
            {
                GameObject gameObject = GameObject.Instantiate(this.blocPrefab) as GameObject;
                gameObject.transform.position = new Vector3(x - (x * this.tileMarginOffset), y - (y * this.tileMarginOffset));
                gameObject.transform.parent = this.gameObject.transform;

                BlocRenderer blocRenderer = gameObject.GetComponent<BlocRenderer>();
                blocRenderer.Initialize(this.sprites);
                this.blocRenderers[x, y] = blocRenderer;
            }
        }

        this.initialized = true;
    }

    private void Update()
    {
        if (!this.initialized)
        {
            return;
        }
        
        for (int y = 0; y < this.Matrix.Height; ++y)
        {
            for (int x = 0; x < this.Matrix.Width; ++x)
            {
                Bloc bloc = this.Matrix.Blocs[x, y];
                if (bloc == null)
                {
                    this.blocRenderers[x, y].SetColor(BlocSpriteDescription.BlocColor.Black);
                    continue;
                }

                if (bloc.Tetromino == null)
                {
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