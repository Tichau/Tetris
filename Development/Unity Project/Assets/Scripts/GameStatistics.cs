// <copyright file="GameStatistics.cs" company="BlobTeam">Copyright BlobTeam. All rights reserved.</copyright>

public class GameStatistics
{
    public GameStatistics(int level = 0)
    {
        this.Score = 0;
        this.Lines = 0;
        this.Level = level;
    }

    public int Score
    {
        get;
        set;
    }

    public int Lines
    {
        get;
        set;
    }

    public int Level
    {
        get;
        set;
    }
}