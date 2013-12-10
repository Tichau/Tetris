// <copyright file="InputManager.cs" company="BlobTeam">Copyright BlobTeam. All rights reserved.</copyright>

using UnityEngine;

public class InputManager : MonoBehaviour
{
    private float lastInputTime;

    public static InputManager Instance
    {
        get;
        private set;
    }

    private void Start()
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

        if (Time.time - this.lastInputTime < 0.2f)
        {
            return;
        }

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

        if (Input.GetKey(KeyCode.UpArrow))
        {
            this.lastInputTime = Time.time;
            Application.Instance.Input(Application.PlayerAction.Left);
        }
    }
}
