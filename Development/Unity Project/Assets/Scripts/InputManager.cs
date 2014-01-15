// <copyright file="InputManager.cs" company="BlobTeam">Copyright BlobTeam. All rights reserved.</copyright>

using UnityEngine;

public class InputManager : MonoBehaviour
{
    private const float WindowWidth = 480f;
    private const float WindowHeight = 200f;
    private const float DurationBeforeMoveInputRepetition = 0.15f;

    private float lastInputTime;
    private string playerInputName = "Player";
    private GameStatistics statisticToRegister;

    private bool moveInputRepetitionMode = false;

    public static InputManager Instance
    {
        get;
        private set;
    }

    public float DurationBetweenMoveInputs
    {
        get; 
        set;
    }

    public void OnGUI()
    {
        if (this.statisticToRegister == null)
        {
            return;
        }

        float left = (Screen.width / 2f) - (WindowWidth / 2f);
        float top = (Screen.height / 2f) - (WindowHeight / 2f);
        GUI.ModalWindow(0, new Rect(left, top, WindowWidth, WindowHeight), this.DoMyWindow, Localization.GetLocalizedString("%Congratulation"), GUIManager.Instance.LightStyle);
    }

    public void RegisterGameStatistic(GameStatistics statistics)
    {
        this.statisticToRegister = statistics;
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
            if ((this.moveInputRepetitionMode && deltaTime >= this.DurationBetweenMoveInputs) ||
                (!this.moveInputRepetitionMode && deltaTime >= DurationBeforeMoveInputRepetition))
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

    private void DoMyWindow(int windowID)
    {
        const float LabelWidth = 450;
        const float ButtonWidth = 100;
        const float InputNameWidth = WindowWidth - 40f;
        const float InputNameHeight = 40f;
        GUI.Label(new Rect(20, 50, LabelWidth, InputNameHeight), Localization.GetLocalizedString("%EnterYourName"), GUIManager.Instance.LightStyle);
        this.playerInputName = GUI.TextField(new Rect(20, 100, InputNameWidth, InputNameHeight), this.playerInputName, 10, GUIManager.Instance.DarkStyle);

        float left = (WindowWidth / 2f) - (ButtonWidth / 2f);
        float top = 150f;
        if (GUI.Button(new Rect(left, top, ButtonWidth, 40), Localization.GetLocalizedString("%Register"), GUIManager.Instance.DarkStyle))
        {
            this.statisticToRegister.PlayerName = this.playerInputName;
            HighScores.RegisterScore(this.statisticToRegister);
            this.statisticToRegister = null;
        }
    }
}
