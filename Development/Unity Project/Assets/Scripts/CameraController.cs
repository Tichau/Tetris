// <copyright file="CameraController.cs" company="BlobTeam">Copyright BlobTeam. All rights reserved.</copyright>

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
            this.transform.position = new Vector3(left + (width / 2f), 10, -10);

            GUIManager.Instance.Mode = GUIManager.GUIMode.Landscape;
        }
        else
        {
            // Short mode
            Vector3 screenSizeInWorldUnit = this.camera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height)) - this.camera.ScreenToWorldPoint(new Vector3(0, 0));
            this.transform.position = new Vector3((screenSizeInWorldUnit.x / 2f) - 1.5f, 10, -10);

            GUIManager.Instance.Mode = GUIManager.GUIMode.Portrait;
        }

        Debug.DrawLine(new Vector3(left, bottom), new Vector3(left + width, bottom), Color.red);
        Debug.DrawLine(new Vector3(left + width, bottom), new Vector3(left + width, bottom + height), Color.blue);
        Debug.DrawLine(new Vector3(left + width, bottom + height), new Vector3(left, bottom + height), Color.cyan);
        Debug.DrawLine(new Vector3(left, bottom), new Vector3(left, bottom + height), Color.magenta);

        float margin = GUIManager.Instance.GetLenght(30f);
        Vector3 worldMargin = this.Camera.ScreenToWorldPoint(new Vector3(margin, 0f)) - this.Camera.ScreenToWorldPoint(new Vector3(0f, 0f));
        Debug.DrawLine(new Vector3(left + width + worldMargin.x, bottom), new Vector3(left + width + worldMargin.x, bottom + height), Color.blue);
    }
}
