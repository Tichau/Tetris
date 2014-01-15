// <copyright file="InputManager.cs" company="BlobTeam">Copyright BlobTeam. All rights reserved.</copyright>

using UnityEngine;

public class InputManager : MonoBehaviour
{
    private const float WindowWidth = 480f;
    private const float WindowHeight = 200f;

    private float lastInputTime;
    private string playerInputName = "Player";
    private GameStatistics statisticToRegister;

    private const float DurationBeforeMoveInputRepetition = 0.15f;
    private bool moveInputRepetitionMode = false;

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

    private void Start()
    {
        this.playerInputName = Localization.GetLocalizedString("%Player");
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

        if (Input.GetKeyUp(KeyCode.Escape) || Input.GetKeyUp(KeyCode.Pause) || Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.Return))
        {
            Application.Instance.Input(Application.PlayerAction.Pause);
        }

        // Rotation
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Application.Instance.Input(Application.PlayerAction.RotateRight);
        }

        // Left & Right moves.
        bool rightMove = Input.GetKeyDown(KeyCode.RightArrow);
        bool leftMove = Input.GetKeyDown(KeyCode.LeftArrow);

        if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            this.moveInputRepetitionMode = false;   
        }

        if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            this.moveInputRepetitionMode = false;
        }

        // Repetition.
        if (!rightMove && !leftMove)
        {
            float deltaTime = Time.time - this.lastInputTime;
            if (this.moveInputRepetitionMode && deltaTime >= this.DurationBetweenMoveInputs ||
                !this.moveInputRepetitionMode && deltaTime >= DurationBeforeMoveInputRepetition)
            {
                if (Input.GetKey(KeyCode.RightArrow))
                {
                    this.moveInputRepetitionMode = true;
                    rightMove = true;
                }

                if (Input.GetKey(KeyCode.LeftArrow))
                {
                    this.moveInputRepetitionMode = true;
                    leftMove = true;
                }
            }
        }

        if (rightMove)
        {
            this.lastInputTime = Time.time;
            Application.Instance.Input(Application.PlayerAction.Right);
        }

        if (leftMove)
        {
            this.lastInputTime = Time.time;
            Application.Instance.Input(Application.PlayerAction.Left);
        }
    }

    public void OnGUI()
    {
        if (this.statisticToRegister == null)
        {
            return;
        }

        GUI.ModalWindow(0, new Rect(Screen.width / 2f - WindowWidth / 2f, Screen.height / 2f - WindowHeight / 2f, WindowWidth, WindowHeight), DoMyWindow, Localization.GetLocalizedString("%Congratulation"), GUIManager.Instance.LightStyle);
    }

    private void DoMyWindow(int windowID)
    {
        const float labelWidth = 450;
        const float buttonWidth = 100;
        const float inputNameWidth = WindowWidth - 40f;
        const float inputNameHeight = 40f;
        GUI.Label(new Rect(20, 50, labelWidth, inputNameHeight), Localization.GetLocalizedString("%EnterYourName"), GUIManager.Instance.LightStyle);
        this.playerInputName = GUI.TextField(new Rect(20, 100, inputNameWidth, inputNameHeight), this.playerInputName, 10, GUIManager.Instance.DarkStyle);

        if (GUI.Button(new Rect(WindowWidth / 2f - buttonWidth / 2f, 150, buttonWidth, 40), Localization.GetLocalizedString("%Register"), GUIManager.Instance.DarkStyle))
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
