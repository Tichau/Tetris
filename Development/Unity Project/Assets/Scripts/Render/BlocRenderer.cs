// <copyright file="BlocRenderer.cs" company="BlobTeam">Copyright BlobTeam. All rights reserved.</copyright>

using UnityEngine;

public class BlocRenderer : MonoBehaviour
{
    private bool initialized = false;
    private Sprite[] sprites;

    private SpriteRenderer spriteRenderer;

    private BlocSpriteDescription.BlocColor color = BlocSpriteDescription.BlocColor.Black;

    public void Initialize(Sprite[] sprites)
    {
        this.spriteRenderer = this.GetComponent<SpriteRenderer>();
        this.sprites = sprites;
        this.initialized = true;
        this.spriteRenderer.sprite = this.sprites[(int)this.color];
    }

    public void SetColor(BlocSpriteDescription.BlocColor newColor)
    {
        if (!this.initialized)
        {
            return;
        }

        if (newColor != this.color)
        {
            this.color = newColor;
            this.spriteRenderer.sprite = this.sprites[(int)this.color];
        }
    }
}
