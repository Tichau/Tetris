// <copyright file="GUIManager.cs" company="BlobTeam">Copyright BlobTeam. All rights reserved.</copyright>

using System.Collections.Generic;
using UnityEngine;

public enum GuiStyleCategory
{
    Light,
    Dark,
    PlayButton,
    PauseButton,
    BigText,
    Text,
    SmallText,
    RightAlignSmallText,
}

public enum RectType
{
    Default,
    Centered,
    Absolute
}

public class GUIManager : MonoBehaviour
{
    public float WidthReference = 1024;
    public float HeightReference = 768;

    public GUIMode Mode;

    private Dictionary<GuiStyleCategory, GUIStyle> guiStylesByCategory = new Dictionary<GuiStyleCategory, GUIStyle>();

    [SerializeField]
    private GUIStyle lightStyle;
    [SerializeField]
    private GUIStyle darkStyle;

    [SerializeField]
    private GUIStyle playButtonStyle;
    [SerializeField]
    private GUIStyle pauseButtonStyle;

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

    public enum GUIMode
    {
        Landscape,
        Portrait,
    }

    public static GUIManager Instance
    {
        get;
        private set;
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

    public GUIStyle GetGuiStyle(GuiStyleCategory category)
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
        this.guiStylesByCategory.Add(GuiStyleCategory.PlayButton, new GUIStyle(this.playButtonStyle));
        this.guiStylesByCategory.Add(GuiStyleCategory.PauseButton, new GUIStyle(this.pauseButtonStyle));

        this.guiStylesByCategory.Add(GuiStyleCategory.Light, new GUIStyle(this.lightStyle));
        this.guiStylesByCategory.Add(GuiStyleCategory.Dark, new GUIStyle(this.darkStyle));
        
        this.guiStylesByCategory.Add(GuiStyleCategory.BigText, new GUIStyle(this.bigTextStyle));
        this.guiStylesByCategory.Add(GuiStyleCategory.Text, new GUIStyle(this.textStyle));
        this.guiStylesByCategory.Add(GuiStyleCategory.SmallText, new GUIStyle(this.smallTextStyle));
        this.guiStylesByCategory.Add(GuiStyleCategory.RightAlignSmallText, new GUIStyle(this.rightAlignSmallTextStyle));
    }

    private void Update()
    {
        float guiRatio = this.GetGuiRatio();
        this.guiStylesByCategory[GuiStyleCategory.Light].fontSize = Mathf.FloorToInt(this.lightStyle.fontSize * guiRatio);
        this.guiStylesByCategory[GuiStyleCategory.Dark].fontSize = Mathf.FloorToInt(this.darkStyle.fontSize * guiRatio);

        this.guiStylesByCategory[GuiStyleCategory.BigText].fontSize = Mathf.FloorToInt(this.bigTextStyle.fontSize * guiRatio);
        this.guiStylesByCategory[GuiStyleCategory.Text].fontSize = Mathf.FloorToInt(this.textStyle.fontSize * guiRatio);
        this.guiStylesByCategory[GuiStyleCategory.SmallText].fontSize = Mathf.FloorToInt(this.smallTextStyle.fontSize * guiRatio);
        this.guiStylesByCategory[GuiStyleCategory.RightAlignSmallText].fontSize = Mathf.FloorToInt(this.rightAlignSmallTextStyle.fontSize * guiRatio);

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
            GUI.Label(this.GetRect(0, 0, 380, 50, RectType.Centered), Localization.GetLocalizedString("%StartGameInfo"), this.GetGuiStyle(GuiStyleCategory.Light));
        }
        else if (Application.Instance.Game.IsPaused)
        {
            GUI.ModalWindow(0, this.GetRect(0f, 0f, this.windowWidth, this.windowHeight, RectType.Centered), this.PauseWindow, Localization.GetLocalizedString("%PauseTitle"), this.GetGuiStyle(GuiStyleCategory.Light));
        }

        if (Application.Instance.Game.IsGameStarted)
        {
            if (GUI.Button(this.GetRect(5f, 5f, 36f, 36f), string.Empty, this.GetGuiStyle(GuiStyleCategory.PauseButton)))
            {
                Application.Instance.Input(Application.PlayerAction.Pause);
            }
        }
        else
        {
            if (GUI.Button(this.GetRect(5f, 5f, 36f, 36f), string.Empty, this.GetGuiStyle(GuiStyleCategory.PlayButton)))
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
        if (GUI.Button(this.GetRect(left, top, ButtonWidth, ButtonHeight), Localization.GetLocalizedString("%Continue"), this.GetGuiStyle(GuiStyleCategory.Dark)))
        {
            Application.Instance.Input(Application.PlayerAction.Pause);
        }
    }
}
