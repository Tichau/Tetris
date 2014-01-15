// <copyright file="Application.cs" company="BlobTeam">Copyright BlobTeam. All rights reserved.</copyright>

using UnityEngine;

public class Application : MonoBehaviour
{
    [SerializeField]
    private GameObject rendererPrefab;

    private BlocGridRenderer blocGridRenderer;

    public enum PlayerAction
    {
        Pause,
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

    public Game Game
    {
        get;
        private set;
    }

    public void Input(PlayerAction action)
    {
        switch (action)
        {
            case PlayerAction.Pause:
                this.Game.Pause();
                break;

            case PlayerAction.Left:
                this.Game.Left();
                break;

            case PlayerAction.Right:
                this.Game.Right();
                break;

            case PlayerAction.RotateRight:
                this.Game.RotateRight();
                break;

            case PlayerAction.SpeedUpStart:
                this.Game.SpeedOverride = 20f;
                break;

            case PlayerAction.SpeedUpEnd:
                this.Game.SpeedOverride = -1f;
                break;
        }
    }

    private void Start()
    {
        Instance = this;

        this.Game = new Game(10, 22);

        // View.
        GameObject rendererObject = Instantiate(this.rendererPrefab) as GameObject;
        this.blocGridRenderer = rendererObject.GetComponent<BlocGridRenderer>();
        this.blocGridRenderer.Initialize(this.Game, Vector2.zero);
    }

    private void LateUpdate()
    {
        this.Game.Update(Time.deltaTime);
    }
}
