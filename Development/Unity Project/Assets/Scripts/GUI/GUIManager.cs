// <copyright file="GUIManager.cs" company="BlobTeam">Copyright BlobTeam. All rights reserved.</copyright>

using System.Collections.Generic;
using UnityEngine;

public class GUIManager : MonoBehaviour
{
    public const float Margin = 30f;

    public float WidthReference = 1024;
    public float HeightReference = 768;

    public GUIMode Mode;

    private bool virtualCommandsEnabled;

    [SerializeField]
    private float startButtonSize;

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
    private GUIStyle controlButtonStyle;

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

    private float windowWidth = 300f;
    private float windowHeight = 100f;

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
        if (!Application.Instance.Game.IsGameStarted)
        {
            GUI.Label(this.GetRect(0f, 0f, 380f, 50f, RectType.Centered), Localization.GetLocalizedString("%StartGameInfo"), this.GetGuiStyle(GUIStyleCategory.Light));
        }
        else if (Application.Instance.Game.IsPaused)
        {
            GUI.ModalWindow(0, this.GetRect(0f, 0f, this.windowWidth, this.windowHeight, RectType.Centered), this.PauseWindow, Localization.GetLocalizedString("%PauseTitle"), this.GetGuiStyle(GUIStyleCategory.Light));
        }

        if (Application.Instance.Game.IsGameStarted)
        {
            if (GUI.Button(this.GetRect(5f, 5f, this.startButtonSize, this.startButtonSize), string.Empty, this.GetGuiStyle(GUIStyleCategory.PauseButton)))
            {
                Application.Instance.Input(Application.PlayerAction.Pause);
            }
        }
        else
        {
            if (GUI.Button(this.GetRect(5f, 5f, this.startButtonSize, this.startButtonSize), string.Empty, this.GetGuiStyle(GUIStyleCategory.PlayButton)))
            {
                Application.Instance.Input(Application.PlayerAction.Pause);
            }
        }
    }

    private void PauseWindow(int windowId)
    {
        const float ButtonWidth = 100f;
        const float ButtonHeight = 40f;

        float left = (this.windowWidth / 2f) - (ButtonWidth / 2f);
        float top = 50f;
        if (GUI.Button(this.GetRect(left, top, ButtonWidth, ButtonHeight), Localization.GetLocalizedString("%Continue"), this.GetGuiStyle(GUIStyleCategory.Dark)))
        {
            Application.Instance.Input(Application.PlayerAction.Pause);
        }
    }
}
