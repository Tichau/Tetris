// <copyright file="Application.cs" company="BlobTeam">Copyright BlobTeam. All rights reserved.</copyright>

using UnityEngine;

public class Application : MonoBehaviour
{
    [SerializeField]
    private GameObject rendererPrefab;

    private Matrix matrix;

    private MatrixRenderer renderer;

    public enum PlayerAction
    {
        Left,
        Right,
        SpeedUpStart,
        SpeedUpEnd,
        RotateRight,
    }

    public static Application Instance
    {
        get;
        private set;
    }

    public void Input(PlayerAction action)
    {
        switch (action)
        {
            case PlayerAction.Left:
                this.matrix.Left();
                break;

            case PlayerAction.Right:
                this.matrix.Right();
                break;

            case PlayerAction.RotateRight:
                this.matrix.RotateRight();
                break;

            case PlayerAction.SpeedUpStart:
                this.matrix.SpeedOverride = 20f;
                break;

            case PlayerAction.SpeedUpEnd:
                this.matrix.SpeedOverride = -1f;
                break;
        }
    }

    private void Start()
    {
        Instance = this;

        this.matrix = new Matrix(10, 22);

        // View.
        GameObject rendererObject = Instantiate(this.rendererPrefab) as GameObject;
        this.renderer = rendererObject.GetComponent<MatrixRenderer>();
        this.renderer.Initialize(matrix);

        this.matrix.NewTetromino();
    }

    private void LateUpdate()
    {
        this.matrix.Update(UnityEngine.Time.deltaTime);
    }
}
