using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiecePreviewDisplayer : MonoBehaviour
{
    [SerializeField]
    private GameObject rendererPrefab;

    [SerializeField]
    private Sprite backgroundTexture;

    private BlocGrid preview1;

    public IEnumerator Start()
    {
        this.preview1 = new BlocGrid(6, 4);

        GameObject rendererObject = Instantiate(this.rendererPrefab) as GameObject;
        rendererObject.transform.parent = this.transform;
        rendererObject.transform.localPosition = Vector3.zero;
        BlocGridRenderer blocGridRenderer = rendererObject.GetComponent<BlocGridRenderer>();
        blocGridRenderer.OverrideBlocSpriteDescription(new BlocSpriteDescription(BlocSpriteDescription.BlocColor.Black, backgroundTexture));
        blocGridRenderer.Initialize(this.preview1, new Vector2(10.5f, 17.4f));

        while (Application.Instance == null || Application.Instance.Game == null)
        {
            yield return null;
        }

        Application.Instance.Game.CurrentTetrominoChange += this.Game_CurrentTetrominoChange;
    }

    private void Game_CurrentTetrominoChange(object sender, System.EventArgs e)
    {
        this.preview1.Clear();
        Queue<Tetromino.TetrominoType> nextTetrominos = Application.Instance.Game.NextTetrominos;
        if (nextTetrominos == null)
        {
            return;
        }

        Tetromino tetromino = new Tetromino(nextTetrominos.Peek());

        switch (tetromino.Type)
        {
            case Tetromino.TetrominoType.I:
                tetromino.Position = new Position(1, 1);
                break;
            case Tetromino.TetrominoType.O:
                tetromino.Position = new Position(2, 1);
                break;
            case Tetromino.TetrominoType.T:
                tetromino.Position = new Position(1, 2);
                break;
            case Tetromino.TetrominoType.L:
                tetromino.Position = new Position(1, 2);
                break;
            case Tetromino.TetrominoType.J:
                tetromino.Position = new Position(1, 2);
                break;
            case Tetromino.TetrominoType.Z:
                tetromino.Position = new Position(1, 2);
                break;
            case Tetromino.TetrominoType.S:
                tetromino.Position = new Position(1, 2);
                break;
        }

        this.preview1.SetTetromino(tetromino);
    }
}
