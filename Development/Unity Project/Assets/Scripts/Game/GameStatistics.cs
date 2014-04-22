// <copyright file="GameStatistics.cs" company="1WeekEndStudio">Copyright 1WeekEndStudio. All rights reserved.</copyright>

using System.Text;

using UnityEngine;

public class GameStatistics
{
    public GameStatistics(int level = 0)
    {
        this.Score = 0;
        this.Lines = 0;
        this.StartLevel = level;
        this.Registered = false;
    }

    public GameStatistics(int score, int lines, int startLevel)
    {
        this.Score = score;
        this.Lines = lines;
        this.StartLevel = startLevel;
        this.Registered = false;
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

    public int StartLevel
    {
        get;
        set;
    }

    public bool Registered
    {
        get;
        set;
    }
    
    public override string ToString()
    {
        return string.Format("Score: {0} Lines: {1} Level: {2} Registered: {3}", this.Score, this.Lines, this.StartLevel, this.Registered);
    }

    public virtual void Load(string prefix)
    {
        this.Score = PlayerPrefs.GetInt(string.Format("{0}.Score", prefix));
        this.Lines = PlayerPrefs.GetInt(string.Format("{0}.Lines", prefix));
        this.StartLevel = PlayerPrefs.GetInt(string.Format("{0}.StartLevel", prefix));
        int registered = PlayerPrefs.GetInt(string.Format("{0}.Registered", prefix));
        this.Registered = registered == 1;
    }

    public virtual void Save(string prefix)
    {
        PlayerPrefs.SetInt(string.Format("{0}.Score", prefix), this.Score);
        PlayerPrefs.SetInt(string.Format("{0}.Lines", prefix), this.Lines);
        PlayerPrefs.SetInt(string.Format("{0}.StartLevel", prefix), this.StartLevel);
        PlayerPrefs.SetInt(string.Format("{0}.Registered", prefix), this.Registered ? 1 : 0);
    }

    public void Serialize(StringBuilder stringBuilder)
    {
        stringBuilder.Append("Score=");
        stringBuilder.Append(this.Score);

        stringBuilder.Append(";Lines=");
        stringBuilder.Append(this.Lines);

        stringBuilder.Append(";StartLevel=");
        stringBuilder.Append(this.StartLevel);
    }
}