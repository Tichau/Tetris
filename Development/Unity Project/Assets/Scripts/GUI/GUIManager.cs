// <copyright file="GUIManager.cs" company="BlobTeam">Copyright BlobTeam. All rights reserved.</copyright>

using UnityEngine;

public class GUIManager : MonoBehaviour
{
    public GUIStyle LightStyle;
    public GUIStyle DarkStyle;

    private const float WindowWidth = 300f;
    private const float WindowHeight = 100f;

    [SerializeField]
    private GameObject scorePanel;

    [SerializeField]
    private float scorePanelBottomOffset;

    [SerializeField]
    private GUIText scoreLabel;

    [SerializeField]
    private GUIText levelLabel;

    [SerializeField]
    private GUIText linesLabel;

    public static GUIManager Instance
    {
        get;
        private set;
    }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        float y = this.scorePanelBottomOffset / Screen.height;
        this.scorePanel.transform.position = new Vector3(0.7f, y, 0f);
    }

    private void Update()
    {
        if (Application.Instance.Game.Statistics == null)
        {
            this.scorePanel.SetActive(false);
        }
        else
        {
            this.scorePanel.SetActive(true);
            this.scoreLabel.text = Application.Instance.Game.Statistics.Score.ToString();
            this.levelLabel.text = string.Format("Level\t  {0}", Application.Instance.Game.Statistics.Level + 1);
            this.linesLabel.text = string.Format("Lines\t  {0}", Application.Instance.Game.Statistics.Lines);
        }
    }

    private void OnGUI()
    {
        if (!Application.Instance.Game.IsGameStarted)
        {
            const float labelWidth = 370;
            const float labelHeight = 50;
            GUI.Label(new Rect(Screen.width / 2f - labelWidth / 2f, Screen.height / 2f - labelHeight / 2f, labelWidth, labelHeight), "Press Enter to start a new game", this.LightStyle);
        }
        else if (Application.Instance.Game.IsPaused)
        {
            GUI.ModalWindow(0, new Rect(Screen.width / 2f - WindowWidth / 2f, Screen.height / 2f - WindowHeight / 2f, WindowWidth, WindowHeight), this.PauseWindow, "PAUSE", this.LightStyle);
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
