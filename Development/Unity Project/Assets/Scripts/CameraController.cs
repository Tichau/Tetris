// <copyright file="CameraController.cs" company="1WeekEndStudio">Copyright 1WeekEndStudio. All rights reserved.</copyright>

using UnityEngine;

public class CameraController : MonoBehaviour
{
    private const float TileSize = 64f;
    private const float TileMarginSize = 2f;
    private const int ColumnCount = 10;
    private const int RowCount = 22;

    public static CameraController Instance
    {
        get;
        private set;
    }

    public Camera Camera
    {
        get { return this.camera; }
    }

    public Vector2 ScreenSizeInWorldUnit
    {
        get;
        private set;
    }

    public Vector2 MarginInWorldUnit
    {
        get;
        private set;
    }

    public Vector3 OffsetPosition
    {
        get;
        set;
    }

    public Vector2 WorldToGUIPosition(Vector3 position)
    {
        Vector3 screenPoint = this.Camera.WorldToScreenPoint(position);

        //// Unity GUI and Unity Camera doesn't use the same referentiel. 
        //// UnityGUI: Y=0 -> Top of the screen. UnityCameraScreenPoint: Y=0 -> Bottom of the screen.

        return new Vector2(screenPoint.x, Screen.height - screenPoint.y);
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
        float bottom = Application.Instance.BlocGridRendererArea.y;
        float left = Application.Instance.BlocGridRendererArea.x;
        float width = Application.Instance.BlocGridRendererArea.width;
        float height = Application.Instance.BlocGridRendererArea.height;

        if (this.camera.aspect > 1.3f)
        {
            // Large mode.
            this.transform.position = new Vector3(left + (width / 2f), 10f, -10f);

            GUIManager.Instance.Mode = GUIMode.Landscape;
        }
        else
        {
            // Short mode
            Vector3 screenSizeInWorldUnit = this.camera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height)) - this.camera.ScreenToWorldPoint(new Vector3(0, 0));
            this.ScreenSizeInWorldUnit = new Vector2(screenSizeInWorldUnit.x, screenSizeInWorldUnit.y);

            if (GUIManager.Instance.VirtualCommandsEnabled)
            {
                this.Camera.orthographicSize = 14f;
                this.transform.position = new Vector3((screenSizeInWorldUnit.x / 2f) - 1.5f, 9f, -10f);
            }
            else
            {
                this.Camera.orthographicSize = 12f;
                this.transform.position = new Vector3((screenSizeInWorldUnit.x / 2f) - 1.5f, 10f, -10f);
            }

            GUIManager.Instance.Mode = GUIMode.Portrait;
        }

        // Camera position.
        this.transform.position = this.transform.position + this.OffsetPosition;

        // Debug
        Debug.DrawLine(new Vector3(left, bottom), new Vector3(left + width, bottom), Color.red);
        Debug.DrawLine(new Vector3(left + width, bottom), new Vector3(left + width, bottom + height), Color.blue);
        Debug.DrawLine(new Vector3(left + width, bottom + height), new Vector3(left, bottom + height), Color.cyan);
        Debug.DrawLine(new Vector3(left, bottom), new Vector3(left, bottom + height), Color.magenta);

        float margin = GUIManager.Instance.GetLenght(GUIManager.Margin);
        this.MarginInWorldUnit = this.Camera.ScreenToWorldPoint(new Vector3(margin, margin)) - this.Camera.ScreenToWorldPoint(new Vector3(0f, 0f));
        Debug.DrawLine(new Vector3(left + width + this.MarginInWorldUnit.x, bottom), new Vector3(left + width + this.MarginInWorldUnit.x, bottom + height), Color.green);
        Debug.DrawLine(new Vector3(left, bottom - this.MarginInWorldUnit.y), new Vector3(left + width, bottom - this.MarginInWorldUnit.y), Color.green);
    }
}
