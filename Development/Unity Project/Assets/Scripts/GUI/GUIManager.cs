// <copyright file="GUIManager.cs" company="BlobTeam">Copyright BlobTeam. All rights reserved.</copyright>

using UnityEngine;

public class GUIManager : MonoBehaviour
{
    public GUIMode Mode;

    public GUIStyle LightStyle;
    public GUIStyle DarkStyle;
    public GUIStyle PlayButtonStyle;
    public GUIStyle PauseButtonStyle;

    public GUIStyle BigTextStyle;
    public GUIStyle TextStyle;
    public GUIStyle SmallTextStyle;
    public GUIStyle RightAlignSmallTextStyle;

    private const float WindowWidth = 300f;
    private const float WindowHeight = 100f;

    [SerializeField]
    private GameObject piecePreviewPanel;

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

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
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
            const float LabelWidth = 380;
            const float LabelHeight = 50;
            float left = (Screen.width / 2f) - (LabelWidth / 2f);
            float top = (Screen.height / 2f) - (LabelHeight / 2f);
            GUI.Label(new Rect(left, top, LabelWidth, LabelHeight), Localization.GetLocalizedString("%StartGameInfo"), this.LightStyle);
        }
        else if (Application.Instance.Game.IsPaused)
        {
            GUI.ModalWindow(0, new Rect(Screen.width / 2f - WindowWidth / 2f, Screen.height / 2f - WindowHeight / 2f, WindowWidth, WindowHeight), this.PauseWindow, Localization.GetLocalizedString("%PauseTitle"), this.LightStyle);
        }

        if (Application.Instance.Game.IsGameStarted)
        {
            if (GUI.Button(new Rect(5f, 5f, 36f, 36f), string.Empty, this.PauseButtonStyle))
            {
                Application.Instance.Input(Application.PlayerAction.Pause);
            }
        }
        else
        {
            if (GUI.Button(new Rect(5f, 5f, 36f, 36f), string.Empty, this.PlayButtonStyle))
            {
                Application.Instance.Input(Application.PlayerAction.Pause);
            }
        }
    }

    private void PauseWindow(int windowId)
    {
        const float ButtonWidth = 100;

        if (GUI.Button(new Rect(WindowWidth / 2f - ButtonWidth / 2f, 50, ButtonWidth, 40), Localization.GetLocalizedString("%Continue"), this.DarkStyle))
        {
            Application.Instance.Input(Application.PlayerAction.Pause);
        }
    }
}
