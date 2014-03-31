// <copyright file="Application.cs" company="BlobTeam">Copyright BlobTeam. All rights reserved.</copyright>

using UnityEngine;

public class Application : MonoBehaviour
{
    [SerializeField]
    private CameraController cameraController;

    [SerializeField]
    private GameObject rendererPrefab;

    private BlocGridRenderer blocGridRenderer;

    private Vector3 offset;
    private Vector3 wantedOffset;

    public enum ApplicationScreen
    {
        Game,
        Settings,
    }

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

    public Rect BlocGridRendererArea
    {
        get
        {
            return this.blocGridRenderer.RendererRect;
        }
    }

    public ApplicationScreen CurrentScreen
    {
        get;
        private set;
    }

    public Game Game
    {
        get;
        private set;
    }

    public void PostScreenChange(ApplicationScreen screen)
    {
        if (this.CurrentScreen == screen)
        {
            return;
        }

        this.CurrentScreen = screen;

        switch (screen)
        {
            case ApplicationScreen.Game:
                wantedOffset = new Vector3(0, 0, 0f);
                break;

            case ApplicationScreen.Settings:
                wantedOffset = new Vector3(this.blocGridRenderer.RendererRect.width, 0, 0f);
                break;
        }
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
#if UNITY_ANDROID
        Screen.fullScreen = true;
#endif

        Instance = this;
        this.Game = new Game(10, 22);

        // View.
        GameObject rendererObject = Instantiate(this.rendererPrefab) as GameObject;
        this.blocGridRenderer = rendererObject.GetComponent<BlocGridRenderer>();
        this.blocGridRenderer.Initialize(this.Game, Vector2.zero);
    }

    private void Update()
    {
        float duration = 0.1f;
        this.offset = new Vector3(Mathf.Lerp(this.offset.x, this.wantedOffset.x, duration), Mathf.Lerp(this.offset.y, this.wantedOffset.y, duration), Mathf.Lerp(this.offset.z, this.wantedOffset.z, duration)); 
        this.cameraController.OffsetPosition = this.offset;
    }

    private void LateUpdate()
    {
        this.Game.Update(Time.deltaTime);
    }
}
