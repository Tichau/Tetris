// <copyright file="PiecePreviewDisplayer.cs" company="BlobTeam">Copyright BlobTeam. All rights reserved.</copyright>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiecePreviewDisplayer : MonoBehaviour
{
    [SerializeField]
    private GameObject rendererPrefab;

    [SerializeField]
    private Sprite backgroundTexture;

    private BlocGrid preview;
    private BlocGridRenderer previewRenderer;

    public IEnumerator Start()
    {
        this.preview = new BlocGrid(6, 4);

        GameObject rendererObject = Instantiate(this.rendererPrefab) as GameObject;
        rendererObject.transform.parent = this.transform;
        rendererObject.transform.localPosition = Vector3.zero;
        this.previewRenderer = rendererObject.GetComponent<BlocGridRenderer>();
        this.previewRenderer.OverrideBlocSpriteDescription(new BlocSpriteDescription(BlocSpriteDescription.BlocColor.Black, this.backgroundTexture));
        //previewRenderer.Initialize(this.preview, new Vector2(10.5f, 17.4f));
        this.previewRenderer.Initialize(this.preview, Vector2.zero);

        while (Application.Instance == null || Application.Instance.Game == null)
        {
            yield return null;
        }

        Application.Instance.Game.CurrentTetrominoChange += this.Game_CurrentTetrominoChange;
    }

    public void Update()
    {
        float left = Application.Instance.BlocGridRendererArea.width;
        float top = Application.Instance.BlocGridRendererArea.height - this.previewRenderer.RendererRect.height;
        float margin = GUIManager.Instance.GetLenght(30f);
        Vector3 worldMargin = CameraController.Instance.Camera.ScreenToWorldPoint(new Vector3(margin, 0f)) - CameraController.Instance.Camera.ScreenToWorldPoint(new Vector3(0f, 0f));
        this.transform.position = new Vector3(left + worldMargin.x, top, 0f);
    }

    private void Game_CurrentTetrominoChange(object sender, System.EventArgs e)
    {
        this.preview.Clear();
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

        this.preview.SetTetromino(tetromino);
    }
}
