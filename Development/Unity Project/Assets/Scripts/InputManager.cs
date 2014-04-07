// <copyright file="InputManager.cs" company="1WeekEndStudio">Copyright 1WeekEndStudio. All rights reserved.</copyright>

using UnityEngine;

public class InputManager : MonoBehaviour
{
    public IInputProvider LeftInputOverride;
    public IInputProvider DownInputOverride;
    public IInputProvider RotateInputOverride;
    public IInputProvider RightInputOverride;

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
        
        GUIManager guiManager = GUIManager.Instance;
        GUI.ModalWindow(0, guiManager.GetRect(0f, 0f, WindowWidth, WindowHeight, RectType.Centered), this.CongratulationWindow, Localization.GetLocalizedString("%Congratulation"), guiManager.GetGuiStyle(GUIStyleCategory.Light));
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

        if (Input.GetKeyDown(KeyCode.DownArrow) || this.DownInputOverride.IsDown)
        {
            Application.Instance.Input(Application.PlayerAction.SpeedUpStart);
        }
        else if (Input.GetKeyUp(KeyCode.DownArrow) || this.DownInputOverride.IsUp)
        {
            Application.Instance.Input(Application.PlayerAction.SpeedUpEnd);
        }

        if (Input.GetKeyUp(KeyCode.Escape) || Input.GetKeyUp(KeyCode.Pause) || Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.Return))
        {
            Application.Instance.Input(Application.PlayerAction.Pause);
        }

        // Rotation
        if (Input.GetKeyDown(KeyCode.UpArrow) || this.RotateInputOverride.IsDown)
        {
            Application.Instance.Input(Application.PlayerAction.RotateRight);
        }

        // Left & Right moves.
        bool rightMove = Input.GetKeyDown(KeyCode.RightArrow) || this.RightInputOverride.IsDown;
        bool leftMove = Input.GetKeyDown(KeyCode.LeftArrow) || this.LeftInputOverride.IsDown;

        if (Input.GetKeyUp(KeyCode.RightArrow) || this.RightInputOverride.IsUp)
        {
            this.moveInputRepetitionMode = false;   
        }

        if (Input.GetKeyUp(KeyCode.LeftArrow) || this.LeftInputOverride.IsUp)
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
                if (Input.GetKey(KeyCode.RightArrow) || this.RightInputOverride.IsActive)
                {
                    this.moveInputRepetitionMode = true;
                    rightMove = true;
                }

                if (Input.GetKey(KeyCode.LeftArrow) || this.LeftInputOverride.IsActive)
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

    private void CongratulationWindow(int windowID)
    {
        const float LabelWidth = 450;
        const float ButtonWidth = 100;
        const float InputNameWidth = WindowWidth - 40f;
        const float InputNameHeight = 40f;

        GUIManager guiManager = GUIManager.Instance;

        GUI.Label(guiManager.GetRect(20f, 50f, LabelWidth, InputNameHeight), Localization.GetLocalizedString("%EnterYourName"), GUIManager.Instance.GetGuiStyle(GUIStyleCategory.Light));
        this.playerInputName = GUI.TextField(guiManager.GetRect(20, 100, InputNameWidth, InputNameHeight), this.playerInputName, 10, GUIManager.Instance.GetGuiStyle(GUIStyleCategory.Dark));

        float left = (WindowWidth / 2f) - (ButtonWidth / 2f);
        float top = 150f;
        if (GUI.Button(guiManager.GetRect(left, top, ButtonWidth, 40f), Localization.GetLocalizedString("%Register"), GUIManager.Instance.GetGuiStyle(GUIStyleCategory.Dark)))
        {
            this.statisticToRegister.PlayerName = this.playerInputName;
            HighScores.RegisterScore(this.statisticToRegister);
            this.statisticToRegister = null;
        }
    }
}
