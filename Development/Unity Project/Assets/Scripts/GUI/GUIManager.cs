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

    private Dictionary<GuiStyleCategory, GUIStyle> GuiStylesByCategory = new Dictionary<GuiStyleCategory, GUIStyle>();

    [SerializeField]
    public GUIStyle LightStyle;
    [SerializeField]
    private GUIStyle DarkStyle;

    [SerializeField]
    private GUIStyle PlayButtonStyle;
    [SerializeField]
    private GUIStyle PauseButtonStyle;

    [SerializeField]
    private GUIStyle BigTextStyle;
    [SerializeField]
    private GUIStyle TextStyle;
    [SerializeField]
    private GUIStyle SmallTextStyle;
    [SerializeField]
    private GUIStyle RightAlignSmallTextStyle;

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
        return this.GuiStylesByCategory[category];
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
        this.GuiStylesByCategory.Add(GuiStyleCategory.PlayButton, new GUIStyle(this.PlayButtonStyle));
        this.GuiStylesByCategory.Add(GuiStyleCategory.PauseButton, new GUIStyle(this.PauseButtonStyle));

        this.GuiStylesByCategory.Add(GuiStyleCategory.Light, new GUIStyle(this.LightStyle));
        this.GuiStylesByCategory.Add(GuiStyleCategory.Dark, new GUIStyle(this.DarkStyle));

        
        this.GuiStylesByCategory.Add(GuiStyleCategory.BigText, new GUIStyle(this.BigTextStyle));
        this.GuiStylesByCategory.Add(GuiStyleCategory.Text, new GUIStyle(this.TextStyle));
        this.GuiStylesByCategory.Add(GuiStyleCategory.SmallText, new GUIStyle(this.SmallTextStyle));
        this.GuiStylesByCategory.Add(GuiStyleCategory.RightAlignSmallText, new GUIStyle(this.RightAlignSmallTextStyle));
    }

    private void Update()
    {
        float guiRatio = this.GetGuiRatio();
        this.GuiStylesByCategory[GuiStyleCategory.Light].fontSize = Mathf.FloorToInt(this.LightStyle.fontSize * guiRatio);
        this.GuiStylesByCategory[GuiStyleCategory.Dark].fontSize = Mathf.FloorToInt(this.DarkStyle.fontSize * guiRatio);

        this.GuiStylesByCategory[GuiStyleCategory.BigText].fontSize = Mathf.FloorToInt(this.BigTextStyle.fontSize * guiRatio);
        this.GuiStylesByCategory[GuiStyleCategory.Text].fontSize = Mathf.FloorToInt(this.TextStyle.fontSize * guiRatio);
        this.GuiStylesByCategory[GuiStyleCategory.SmallText].fontSize = Mathf.FloorToInt(this.SmallTextStyle.fontSize * guiRatio);
        this.GuiStylesByCategory[GuiStyleCategory.RightAlignSmallText].fontSize = Mathf.FloorToInt(this.RightAlignSmallTextStyle.fontSize * guiRatio);

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

        float left = (windowWidth / 2f) - (ButtonWidth / 2f);
        float top = 50f;
        if (GUI.Button(this.GetRect(left, top, ButtonWidth, ButtonHeight), Localization.GetLocalizedString("%Continue"), this.GetGuiStyle(GuiStyleCategory.Dark)))
        {
            Application.Instance.Input(Application.PlayerAction.Pause);
        }
    }
}
