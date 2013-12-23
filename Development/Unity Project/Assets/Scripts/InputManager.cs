// <copyright file="InputManager.cs" company="BlobTeam">Copyright BlobTeam. All rights reserved.</copyright>

using UnityEngine;

public class InputManager : MonoBehaviour
{
    public GUIStyle LightStyle;
    public GUIStyle DarkStyle;

    private const float WindowWidth = 480f;
    private const float WindowHeight = 200f;

    private float lastInputTime;
    private string playerInputName = "Player";
    private GameStatistics statisticToRegister;

    public float DurationBetweenMoveInputs
    {
        get; 
        set;
    }

    public static InputManager Instance
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
        if (this.statisticToRegister != null)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Application.Instance.Input(Application.PlayerAction.SpeedUpStart);
        }
        else if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            Application.Instance.Input(Application.PlayerAction.SpeedUpEnd);
        }

        if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            Application.Instance.Input(Application.PlayerAction.RotateRight);
        }

        if (Input.GetKeyUp(KeyCode.Escape) || Input.GetKeyUp(KeyCode.Pause) || Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.Return))
        {
            Application.Instance.Input(Application.PlayerAction.Pause);
        }

        if (Time.time - this.lastInputTime >= this.DurationBetweenMoveInputs)
        {
            if (Input.GetKey(KeyCode.RightArrow))
            {
                this.lastInputTime = Time.time;
                Application.Instance.Input(Application.PlayerAction.Right);
            }

            if (Input.GetKey(KeyCode.LeftArrow))
            {
                this.lastInputTime = Time.time;
                Application.Instance.Input(Application.PlayerAction.Left);
            }
        }
    }

    public void OnGUI()
    {
        if (this.statisticToRegister == null)
        {
            return;
        }

        GUI.ModalWindow(0, new Rect(Screen.width / 2f - WindowWidth / 2f, Screen.height / 2f - WindowHeight / 2f, WindowWidth, WindowHeight), DoMyWindow, "Congratulations you're in the high scores !", this.LightStyle);
    }

    void DoMyWindow(int windowID)
    {
        const float labelWidth = 450;
        const float buttonWidth = 100;
        const float inputNameWidth = WindowWidth - 40f;
        const float inputNameHeight = 40f;
        GUI.Label(new Rect(20, 50, labelWidth, inputNameHeight), "Enter you're name:", this.LightStyle);
        this.playerInputName = GUI.TextField(new Rect(20, 100, inputNameWidth, inputNameHeight), this.playerInputName, 10, this.DarkStyle);

        if (GUI.Button(new Rect(WindowWidth/2f - buttonWidth/2f, 150, buttonWidth, 40), "Register", this.DarkStyle))
        {
            this.statisticToRegister.PlayerName = this.playerInputName;
            HighScores.RegisterScore(this.statisticToRegister);
            this.statisticToRegister = null;
        }
    }

    public void RegisterGameStatistic(GameStatistics statistics)
    {
        this.statisticToRegister = statistics;
    }
}
