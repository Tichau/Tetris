// <copyright file="VirtualCommandsController.cs" company="BlobTeam">Copyright BlobTeam. All rights reserved.</copyright>

using System;

using UnityEngine;

public class VirtualCommandsController : MonoBehaviour
{
    [SerializeField]
    private float marginRatio;
    
    [SerializeField]
    private float buttonSizeInCm;
    
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

        float dpi = Screen.dpi;

        Vector3 originPoint = cameraController.Camera.ScreenToWorldPoint(new Vector3(0, 0));
        Vector3 borderSizeInWorldUnit = new Vector3(rendererArea.x, rendererArea.y) - originPoint;

        float externalMargin = borderSizeInWorldUnit.x;
        float internalMargin = this.marginRatio * borderSizeInWorldUnit.x;

        // Compute the size of the space between 2 groups of buttons.
        float buttonSizeInInch = this.buttonSizeInCm / 2.54f; // 1 inch is equal to 2.54 cm.
        float buttonWidth = (cameraController.Camera.ScreenToWorldPoint(new Vector3(dpi * buttonSizeInInch, 0f, 0f)) - cameraController.Camera.ScreenToWorldPoint(new Vector3(0f, 0f, 0f))).x;
        float screenWidth = cameraController.ScreenSizeInWorldUnit.x;
        float spaceWidth = screenWidth - (2 * externalMargin) - (2 * internalMargin) - (4 * buttonWidth);

        if (spaceWidth < internalMargin || Math.Abs(dpi - 0f) < float.Epsilon)
        {
            buttonWidth = (screenWidth - (2 * externalMargin) - (3 * internalMargin)) / 4;
            spaceWidth = internalMargin;
        }

        this.leftButton.transform.localScale = new Vector3(buttonWidth, buttonWidth, 1f);
        this.downButton.transform.localScale = new Vector3(buttonWidth, buttonWidth, 1f);
        this.rotateButton.transform.localScale = new Vector3(buttonWidth, buttonWidth, 1f);
        this.rightButton.transform.localScale = new Vector3(buttonWidth, buttonWidth, 1f);

        float left = originPoint.x + externalMargin;
        this.leftButton.transform.position = new Vector3(left, rendererArea.y - cameraController.MarginInWorldUnit.y, -2f);
        this.downButton.transform.position = new Vector3(left + internalMargin + buttonWidth, rendererArea.y - cameraController.MarginInWorldUnit.y, -2f);
        
        float secondGroupLeft = left + internalMargin + 2 * buttonWidth + spaceWidth;
        this.rotateButton.transform.position = new Vector3(secondGroupLeft, rendererArea.y - cameraController.MarginInWorldUnit.y, -2f);
        this.rightButton.transform.position = new Vector3(secondGroupLeft + internalMargin + buttonWidth, rendererArea.y - cameraController.MarginInWorldUnit.y, -2f);
    }
}
