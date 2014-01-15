// <copyright file="CameraController.cs" company="BlobTeam">Copyright BlobTeam. All rights reserved.</copyright>

using UnityEngine;

public class CameraController : MonoBehaviour
{
    private const float TileSize = 64f;
    private const float TileMarginSize = 2f;
    private const int ColumnCount = 10;

    private void Start()
    {
    }
    
    private void Update()
    {
        const float LeftWorldPosition = -0.5f;
        const float WidthWorldPosition = ColumnCount - (TileMarginSize / TileSize * (ColumnCount - 1));

        if (this.camera.aspect > 1.3f)
        {
            // Large mode.
            this.transform.position = new Vector3(LeftWorldPosition + WidthWorldPosition / 2f, 10, -10);

            GUIManager.Instance.Mode = GUIManager.GUIMode.Landscape;
        }
        else
        {
            // Short mode
            Vector3 screenSizeInWorldUnit = this.camera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height)) - this.camera.ScreenToWorldPoint(new Vector3(0, 0));
            this.transform.position = new Vector3(screenSizeInWorldUnit.x / 2f - 1.5f, 10, -10);

            GUIManager.Instance.Mode = GUIManager.GUIMode.Portrait;
        }

        ////Debug.DrawLine(new Vector3(LeftWorldPosition, 0f), new Vector3(LeftWorldPosition + WidthWorldPosition, 0f), Color.red);
    }
}
