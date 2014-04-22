// <copyright file="Application.cs" company="1WeekEndStudio">Copyright 1WeekEndStudio. All rights reserved.</copyright>

using System.Collections.Generic;

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

    private List<ApplicationScreen> applicationScreens = new List<ApplicationScreen>();

    public enum ApplicationScreen
    {
        None,
        Game,
        Settings,
        Profile,
        ProfileCreation,
        OnlineProfileConnection,
        OnlineProfileCreation,
        RegisterScore,
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
        get
        {
            if (this.applicationScreens.Count == 0)
            {
                return ApplicationScreen.None;
            }

            return this.applicationScreens[this.applicationScreens.Count - 1];
        }
    }

    public Game Game
    {
        get;
        private set;
    }

    public void BackToLastScreen()
    {
        if (this.applicationScreens.Count <= 1)
        {
            return;
        }

        this.applicationScreens.RemoveAt(this.applicationScreens.Count - 1);

        Debug.Log("Back to last screen. " + this.applicationScreens.Count + " screens queued. Current screen = " + this.CurrentScreen);

        switch (this.CurrentScreen)
        {
            case ApplicationScreen.Game:
                this.wantedOffset = new Vector3(0, 0, 0f);
                break;

            case ApplicationScreen.Settings:
                this.wantedOffset = new Vector3(this.blocGridRenderer.RendererRect.width, 0, 0f);
                break;
        }
    }

    public void BackToScreen(ApplicationScreen screen)
    {
        for (int index = this.applicationScreens.Count - 1; index >= 1; --index)
        {
            if (this.applicationScreens[index] == screen)
            {
                break;
            }

            this.applicationScreens.RemoveAt(index);
        }
        
        Debug.Log("Back to screen " + screen + ". " + this.applicationScreens.Count + " screens queued. Current screen = " + this.CurrentScreen);

        switch (this.CurrentScreen)
        {
            case ApplicationScreen.Game:
                this.wantedOffset = new Vector3(0, 0, 0f);
                break;

            case ApplicationScreen.Settings:
                this.wantedOffset = new Vector3(this.blocGridRenderer.RendererRect.width, 0, 0f);
                break;
        }
    }

    public void PostScreenChange(ApplicationScreen screen)
    {
        if (this.CurrentScreen == screen)
        {
            return;
        }

        this.applicationScreens.Add(screen);

        Debug.Log("screen " + screen + " queued. " + this.applicationScreens.Count + " screens queued. Current screen = " + this.CurrentScreen);

        switch (screen)
        {
            case ApplicationScreen.Game:
                this.wantedOffset = new Vector3(0, 0, 0f);
                break;

            case ApplicationScreen.Settings:
                this.wantedOffset = new Vector3(this.blocGridRenderer.RendererRect.width, 0, 0f);
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

        this.PostScreenChange(ApplicationScreen.Game);
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
