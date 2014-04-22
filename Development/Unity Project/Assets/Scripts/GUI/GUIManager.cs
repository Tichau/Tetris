// <copyright file="GUIManager.cs" company="1WeekEndStudio">Copyright 1WeekEndStudio. All rights reserved.</copyright>

using System.Collections.Generic;
using UnityEngine;

public partial class GUIManager : MonoBehaviour
{
    public const float Margin = 30f;

    public float WidthReference = 1024;
    public float HeightReference = 768;

    public GUIMode Mode;

    private bool virtualCommandsEnabled;

    [SerializeField]
    private float menuButtonSize;

    [SerializeField]
    private VirtualCommandsController virtualCommandsController;

    private Dictionary<GUIStyleCategory, GUIStyle> guiStylesByCategory = new Dictionary<GUIStyleCategory, GUIStyle>();

    [SerializeField]
    private GUIStyle lightStyle;
    [SerializeField]
    private GUIStyle darkStyle;

    [SerializeField]
    private GUIStyle playButtonStyle;
    [SerializeField]
    private GUIStyle pauseButtonStyle;
    [SerializeField]
    private GUIStyle settingsButtonStyle;
    [SerializeField]
    private GUIStyle profileButtonStyle;
    [SerializeField]
    private GUIStyle profileButtonLightStyle;
    [SerializeField]
    private GUIStyle refreshButtonStyle;

    [SerializeField]
    private GUIStyle bigTextStyle;
    [SerializeField]
    private GUIStyle textStyle;
    [SerializeField]
    private GUIStyle smallTextStyle;
    [SerializeField]
    private GUIStyle rightAlignSmallTextStyle;

    [SerializeField]
    private GameObject piecePreviewPanel;

    private float windowWidth = 400f;
    private float windowHeight = 300f;

    public static GUIManager Instance
    {
        get;
        private set;
    }

    public bool VirtualCommandsEnabled
    {
        get
        {
            return this.virtualCommandsEnabled;
        }

        private set
        {
            this.virtualCommandsEnabled = value;
            this.virtualCommandsController.gameObject.SetActive(value);
        }
    }

    public float GetLenght(float lenght)
    {
        float ratio = this.GetGuiRatio();

        return lenght * ratio;
    }

    public Rect GetRect(float left, float top, float width, float height, RectType rectType = RectType.Default)
    {
        float ratio = this.GetGuiRatio();

        float finalLeft = left * ratio;
        float finalTop = top * ratio;
        float finalWidth = width * ratio;
        float finalHeight = height * ratio;

        if (rectType == RectType.Centered)
        {
            finalLeft = (Screen.width / 2f) - (finalWidth / 2f);
            finalTop = (Screen.height / 2f) - (finalHeight / 2f);
        }

        if (rectType == RectType.Absolute)
        {
            finalLeft = left;
            finalTop = top;
        }

        return new Rect(finalLeft, finalTop, finalWidth, finalHeight);
    }

    public GUIStyle GetGuiStyle(GUIStyleCategory category)
    {
        return this.guiStylesByCategory[category];
    }

    private float GetGuiRatio()
    {
        float xRatio = Screen.width / this.WidthReference;
        float yRatio = Screen.height / this.HeightReference;

        return Mathf.Max(xRatio, yRatio);
    }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
#if UNITY_ANDROID
        this.VirtualCommandsEnabled = true;
#else
        this.VirtualCommandsEnabled = false;
#endif

        this.guiStylesByCategory.Add(GUIStyleCategory.PlayButton, new GUIStyle(this.playButtonStyle));
        this.guiStylesByCategory.Add(GUIStyleCategory.PauseButton, new GUIStyle(this.pauseButtonStyle));
        this.guiStylesByCategory.Add(GUIStyleCategory.SettingsButton, new GUIStyle(this.settingsButtonStyle));
        this.guiStylesByCategory.Add(GUIStyleCategory.ProfileButton, new GUIStyle(this.profileButtonStyle));
        this.guiStylesByCategory.Add(GUIStyleCategory.RefreshButton, new GUIStyle(this.refreshButtonStyle));
        this.guiStylesByCategory.Add(GUIStyleCategory.ProfileButtonLight, new GUIStyle(this.profileButtonLightStyle));

        this.guiStylesByCategory.Add(GUIStyleCategory.Light, new GUIStyle(this.lightStyle));
        this.guiStylesByCategory.Add(GUIStyleCategory.Dark, new GUIStyle(this.darkStyle));
        
        this.guiStylesByCategory.Add(GUIStyleCategory.BigText, new GUIStyle(this.bigTextStyle));
        this.guiStylesByCategory.Add(GUIStyleCategory.Text, new GUIStyle(this.textStyle));
        this.guiStylesByCategory.Add(GUIStyleCategory.SmallText, new GUIStyle(this.smallTextStyle));
        this.guiStylesByCategory.Add(GUIStyleCategory.RightAlignSmallText, new GUIStyle(this.rightAlignSmallTextStyle));
    }

    private void Update()
    {
        float guiRatio = this.GetGuiRatio();
        this.guiStylesByCategory[GUIStyleCategory.Light].fontSize = Mathf.FloorToInt(this.lightStyle.fontSize * guiRatio);
        this.guiStylesByCategory[GUIStyleCategory.Dark].fontSize = Mathf.FloorToInt(this.darkStyle.fontSize * guiRatio);
        this.guiStylesByCategory[GUIStyleCategory.ProfileButton].fontSize = Mathf.FloorToInt(this.profileButtonStyle.fontSize * guiRatio);
        this.guiStylesByCategory[GUIStyleCategory.ProfileButtonLight].fontSize = Mathf.FloorToInt(this.profileButtonStyle.fontSize * guiRatio);

        this.guiStylesByCategory[GUIStyleCategory.BigText].fontSize = Mathf.FloorToInt(this.bigTextStyle.fontSize * guiRatio);
        this.guiStylesByCategory[GUIStyleCategory.Text].fontSize = Mathf.FloorToInt(this.textStyle.fontSize * guiRatio);
        this.guiStylesByCategory[GUIStyleCategory.SmallText].fontSize = Mathf.FloorToInt(this.smallTextStyle.fontSize * guiRatio);
        this.guiStylesByCategory[GUIStyleCategory.RightAlignSmallText].fontSize = Mathf.FloorToInt(this.rightAlignSmallTextStyle.fontSize * guiRatio);

        if (Application.Instance.Game.IsGameStarted)
        {
            this.piecePreviewPanel.SetActive(true);
        }
        else
        {
            this.piecePreviewPanel.SetActive(false);
        }
    }

    private void OnGUI()
    {
        Game game = Application.Instance.Game;

        if (game.IsGameStarted && !game.IsPaused)
        {
            if (GUI.Button(this.GetRect(5f, 5f, this.menuButtonSize, this.menuButtonSize), string.Empty, this.GetGuiStyle(GUIStyleCategory.PauseButton)))
            {
                Application.Instance.Input(Application.PlayerAction.Pause);
            }
        }
        else
        {
            if (GUI.Button(this.GetRect(5f, 5f, this.menuButtonSize, this.menuButtonSize), string.Empty, this.GetGuiStyle(GUIStyleCategory.PlayButton)))
            {
                Application.Instance.BackToScreen(Application.ApplicationScreen.Game);
                Application.Instance.Input(Application.PlayerAction.Pause);
            }
        }

        string currentProfileName = ProfileManager.Instance.CurrentProfile != null ? ProfileManager.Instance.CurrentProfile.Name : Localization.GetLocalizedString("%NoProfileSelected");
        if (GUI.Button(this.GetRect(5f + this.menuButtonSize + 5f, 5f, this.menuButtonSize, this.menuButtonSize), currentProfileName, this.GetGuiStyle(GUIStyleCategory.ProfileButton)))
        {
            if (game.IsGameStarted && !game.IsPaused)
            {
                Application.Instance.Input(Application.PlayerAction.Pause);
            }

            Application.Instance.PostScreenChange(Application.ApplicationScreen.Profile);
        }

        float profileButtonSize = this.menuButtonSize + this.GetGuiStyle(GUIStyleCategory.ProfileButton).CalcSize(new GUIContent(currentProfileName)).x;
        if (ProfileManager.Instance.CurrentProfile != null && !ProfileManager.Instance.CurrentProfile.IsSynchronized)
        {
            if (GUI.Button(this.GetRect(5f + this.menuButtonSize + 5f + profileButtonSize, 5f, this.menuButtonSize, this.menuButtonSize), string.Empty, this.GetGuiStyle(GUIStyleCategory.RefreshButton)))
            {
                ProfileManager.Instance.SynchronizeProfile(ProfileManager.Instance.CurrentProfile);
            }
        }

        if (Application.Instance.CurrentScreen == Application.ApplicationScreen.Profile)
        {
            GUI.ModalWindow(0, this.GetRect(0f, 0f, this.windowWidth, this.windowHeight, RectType.Centered), this.ProfileWindow, Localization.GetLocalizedString("%ProfilesWindowTitle"), this.GetGuiStyle(GUIStyleCategory.Light));
        }
        else if (Application.Instance.CurrentScreen == Application.ApplicationScreen.ProfileCreation)
        {
            GUI.ModalWindow(0, this.GetRect(0f, 0f, this.windowWidth, this.windowHeight, RectType.Centered), this.ProfileCreationWindow, Localization.GetLocalizedString("%ProfileCreationWindowTitle"), this.GetGuiStyle(GUIStyleCategory.Light));
        }
        else if (Application.Instance.CurrentScreen == Application.ApplicationScreen.RegisterScore)
        {
            GUI.ModalWindow(0, this.GetRect(0f, 0f, this.windowWidth, this.windowHeight, RectType.Centered), this.CongratulationWindow, Localization.GetLocalizedString("%GameOver"), this.GetGuiStyle(GUIStyleCategory.Light));
        }
        else if (Application.Instance.CurrentScreen == Application.ApplicationScreen.OnlineProfileConnection)
        {
            GUI.ModalWindow(0, this.GetRect(0f, 0f, this.windowWidth, this.windowHeight, RectType.Centered), this.OnlineProfileConnectionWindow, Localization.GetLocalizedString("%ProfileConnectionWindowTitle"), this.GetGuiStyle(GUIStyleCategory.Light));
        }
        else if (Application.Instance.CurrentScreen == Application.ApplicationScreen.OnlineProfileCreation)
        {
            GUI.ModalWindow(0, this.GetRect(0f, 0f, this.windowWidth, this.windowHeight, RectType.Centered), this.OnlineProfileCreationWindow, Localization.GetLocalizedString("%ProfileCreationWindowTitle"), this.GetGuiStyle(GUIStyleCategory.Light));
        }

        float margin = this.GetLenght(5f);
        float startButtonSize = this.GetLenght(this.menuButtonSize);
        if (GUI.Button(new Rect(Screen.width - startButtonSize - margin, margin, startButtonSize, startButtonSize), string.Empty, this.GetGuiStyle(GUIStyleCategory.SettingsButton)))
        {
            if (game.IsGameStarted && !game.IsPaused)
            {
                Application.Instance.Input(Application.PlayerAction.Pause);
            }

            switch (Application.Instance.CurrentScreen)
            {
                case Application.ApplicationScreen.Game:
                    Application.Instance.PostScreenChange(Application.ApplicationScreen.Settings);
                    break;

                case Application.ApplicationScreen.Settings:
                    Application.Instance.BackToLastScreen();
                    break;
            }
        }
    }
}
