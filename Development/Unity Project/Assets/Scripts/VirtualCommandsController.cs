// <copyright file="VirtualCommandsController.cs" company="BlobTeam">Copyright BlobTeam. All rights reserved.</copyright>

using UnityEngine;

public class VirtualCommandsController : MonoBehaviour
{
    [SerializeField]
    private float marginRatio;

    [SerializeField]
    private GameObject spriteButtonPrefab;

    [SerializeField]
    private Sprite leftButtonBackground;
    [SerializeField]
    private Sprite leftButtonActiveBackground;

    [SerializeField]
    private Sprite downButtonBackground;
    [SerializeField]
    private Sprite downButtonActiveBackground;

    [SerializeField]
    private Sprite rotateButtonBackground;
    [SerializeField]
    private Sprite rotateButtonActiveBackground;

    [SerializeField]
    private Sprite rightButtonBackground;
    [SerializeField]
    private Sprite rightButtonActiveBackground;

    private SpriteButton leftButton;
    private SpriteButton downButton;
    private SpriteButton rotateButton;
    private SpriteButton rightButton;

    private void Start()
    {
        if (this.spriteButtonPrefab == null)
        {
            Debug.LogError("The sprite button prefab has not been assigned.");
        }

        GameObject gameObject = Instantiate(this.spriteButtonPrefab) as GameObject;
        gameObject.transform.parent = this.gameObject.transform;
        this.leftButton = gameObject.GetComponent<SpriteButton>();
        this.leftButton.Background = this.leftButtonBackground;
        this.leftButton.ActiveBackground = this.leftButtonActiveBackground;

        gameObject = Instantiate(this.spriteButtonPrefab) as GameObject;
        gameObject.transform.parent = this.gameObject.transform;
        this.downButton = gameObject.GetComponent<SpriteButton>();
        this.downButton.Background = this.downButtonBackground;
        this.downButton.ActiveBackground = this.downButtonActiveBackground;

        gameObject = Instantiate(this.spriteButtonPrefab) as GameObject;
        gameObject.transform.parent = this.gameObject.transform;
        this.rotateButton = gameObject.GetComponent<SpriteButton>();
        this.rotateButton.Background = this.rotateButtonBackground;
        this.rotateButton.ActiveBackground = this.rotateButtonActiveBackground;

        gameObject = Instantiate(this.spriteButtonPrefab) as GameObject;
        gameObject.transform.parent = this.gameObject.transform;
        this.rightButton = gameObject.GetComponent<SpriteButton>();
        this.rightButton.Background = this.rightButtonBackground;
        this.rightButton.ActiveBackground = this.rightButtonActiveBackground;

        // Inputs
        InputManager.Instance.LeftInputOverride = this.leftButton;
        InputManager.Instance.DownInputOverride = this.downButton;
        InputManager.Instance.RotateInputOverride = this.rotateButton;
        InputManager.Instance.RightInputOverride = this.rightButton;
    }

    private void Update()
    {
        // Update position of buttons.
        Rect rendererArea = Application.Instance.BlocGridRendererArea;
        CameraController cameraController = CameraController.Instance;

        Vector3 originPoint = cameraController.Camera.ScreenToWorldPoint(new Vector3(0, 0));
        Vector3 borderSizeInWorldUnit = new Vector3(rendererArea.x, rendererArea.y) - originPoint;

        const int ButtonCount = 4;
        float buttonWidth = (cameraController.ScreenSizeInWorldUnit.x - (2 * borderSizeInWorldUnit.x)) / (ButtonCount + ((ButtonCount - 1) * this.marginRatio));
        float marginWidth = buttonWidth * this.marginRatio;

        this.leftButton.transform.localScale = new Vector3(buttonWidth, buttonWidth, 1f);
        this.downButton.transform.localScale = new Vector3(buttonWidth, buttonWidth, 1f);
        this.rotateButton.transform.localScale = new Vector3(buttonWidth, buttonWidth, 1f);
        this.rightButton.transform.localScale = new Vector3(buttonWidth, buttonWidth, 1f);

        float left = originPoint.x + borderSizeInWorldUnit.x;
        this.leftButton.transform.position = new Vector3(left + (0 * (marginWidth + buttonWidth)), rendererArea.y - cameraController.MarginInWorldUnit.y, -2f);
        this.downButton.transform.position = new Vector3(left + (1 * (marginWidth + buttonWidth)), rendererArea.y - cameraController.MarginInWorldUnit.y, -2f);
        this.rotateButton.transform.position = new Vector3(left + (2 * (marginWidth + buttonWidth)), rendererArea.y - cameraController.MarginInWorldUnit.y, -2f);
        this.rightButton.transform.position = new Vector3(left + (3 * (marginWidth + buttonWidth)), rendererArea.y - cameraController.MarginInWorldUnit.y, -2f);
    }
}
