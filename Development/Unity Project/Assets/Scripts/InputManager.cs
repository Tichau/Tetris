// <copyright file="InputManager.cs" company="BlobTeam">Copyright BlobTeam. All rights reserved.</copyright>

using UnityEngine;

public class InputManager : MonoBehaviour
{
    private float lastInputTime;

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

    public string GetName()
    {
        return "PLayer";
    }
}
