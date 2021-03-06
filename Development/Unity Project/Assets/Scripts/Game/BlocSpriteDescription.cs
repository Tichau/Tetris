﻿// <copyright file="BlocSpriteDescription.cs" company="1WeekEndStudio">Copyright 1WeekEndStudio. All rights reserved.</copyright>

[System.Serializable]
public class BlocSpriteDescription
{
    [UnityEngine.SerializeField]
    public BlocColor Color;

    [UnityEngine.SerializeField]
    public UnityEngine.Sprite Sprite;

    public BlocSpriteDescription(BlocColor color, UnityEngine.Sprite sprite)
    {
        this.Color = color;
        this.Sprite = sprite;
    }

    public enum BlocColor
    {
        Black = 0,
        Blue = 1,
        Cyan = 2,
        Green = 3,
        Orange = 4,
        Purple = 5,
        Red = 6,
        Yellow = 7,

        Ghost = 8,
    }
}