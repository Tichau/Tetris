// <copyright file="InputManager.cs" company="1WeekEndStudio">Copyright 1WeekEndStudio. All rights reserved.</copyright>

using UnityEngine;

public class InputManager : MonoBehaviour
{
    public IInputProvider LeftInputOverride;
    public IInputProvider DownInputOverride;
    public IInputProvider RotateInputOverride;
    public IInputProvider RightInputOverride;

    private const float DurationBeforeMoveInputRepetition = 0.15f;

    private float lastInputTime;
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
    
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
    }
    
    private void Update()
    {
        if (Application.Instance.CurrentScreen != Application.ApplicationScreen.Game)
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
}
