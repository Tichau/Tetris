// <copyright file="SpriteButton.cs" company="1WeekEndStudio">Copyright 1WeekEndStudio. All rights reserved.</copyright>

using UnityEngine;

public class SpriteButton : MonoBehaviour, IInputProvider
{
    public Sprite Background;
    public Sprite ActiveBackground;

    private SpriteRenderer spriteRenderer;

    private Collider2D buttonCollider2D;

    public bool IsActive
    {
        get;
        private set;
    }

    public bool IsUp
    {
        get;
        private set;
    }

    public bool IsDown
    {
        get;
        private set;
    }

    private void Start()
    {
        this.spriteRenderer = this.GetComponent<SpriteRenderer>();
        if (this.spriteRenderer == null)
        {
            Debug.LogError("No sprite renderer found.");
            return;
        }

        this.buttonCollider2D = this.GetComponent<BoxCollider2D>();
        if (this.buttonCollider2D == null)
        {
            Debug.LogError("No collider found.");
            return;
        }

        this.spriteRenderer.sprite = this.Background;
    }

    private void Update()
    {
        bool newActiveState = false;
        this.IsUp = false;
        this.IsDown = false;

#if UNITY_EDITOR
        // Mouse.
        if (Input.GetMouseButton(0))
        {
            Vector3 mousePosition = CameraController.Instance.Camera.ScreenToWorldPoint(Input.mousePosition);
            newActiveState |= this.buttonCollider2D.OverlapPoint(mousePosition);
        }
#endif
        
        // Touch.
        for (int touchIndex = 0; touchIndex < Input.touchCount; touchIndex++)
        {
            Touch touch = Input.GetTouch(touchIndex);
            Vector3 touchPosition = CameraController.Instance.Camera.ScreenToWorldPoint(touch.position);
            newActiveState |= this.buttonCollider2D.OverlapPoint(touchPosition);
        }

        // Change active state.
        if (this.IsActive != newActiveState)
        {
            this.IsActive = newActiveState;

            if (this.IsActive)
            {
                this.IsDown = true;
            }
            else
            {
                this.IsUp = true;
            }

            this.spriteRenderer.sprite = this.IsActive ? this.ActiveBackground : this.Background;
        }
    }
}
