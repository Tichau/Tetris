// <copyright file="GUIManager.cs" company="BlobTeam">Copyright BlobTeam. All rights reserved.</copyright>

using UnityEngine;

public class GUIManager : MonoBehaviour
{
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
            const float labelWidth = 370;
            const float labelHeight = 50;
            GUI.Label(new Rect(Screen.width / 2f - labelWidth / 2f, Screen.height / 2f - labelHeight / 2f, labelWidth, labelHeight), Localization.GetLocalizedString("%StartGameInfo"), this.LightStyle);
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

    private void PauseWindow(int windowID)
    {
        const float buttonWidth = 100;

        if (GUI.Button(new Rect(WindowWidth / 2f - buttonWidth / 2f, 50, buttonWidth, 40), "Continue", this.DarkStyle))
        {
            Application.Instance.Input(Application.PlayerAction.Pause);
        }
    }
}
