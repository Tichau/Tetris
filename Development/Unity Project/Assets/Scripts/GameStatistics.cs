// <copyright file="GameStatistics.cs" company="1WeekEndStudio">Copyright 1WeekEndStudio. All rights reserved.</copyright>

public class GameStatistics
{
    public GameStatistics(int level = 0)
    {
        this.Score = 0;
        this.Lines = 0;
        this.Level = level;
    }

    public string PlayerName
    {
        get;
        set;
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

    public void Load(string slotName)
    {
        this.PlayerName = UnityEngine.PlayerPrefs.GetString(slotName + "_PlayerName");
        this.Score = UnityEngine.PlayerPrefs.GetInt(slotName + "_Score");
        this.Lines = UnityEngine.PlayerPrefs.GetInt(slotName + "_Lines");
        this.Level = UnityEngine.PlayerPrefs.GetInt(slotName + "_Level");
    }

    public void Save(string slotName)
    {
        UnityEngine.PlayerPrefs.SetString(slotName + "_PlayerName", this.PlayerName);
        UnityEngine.PlayerPrefs.SetInt(slotName + "_Score", this.Score);
        UnityEngine.PlayerPrefs.SetInt(slotName + "_Lines", this.Lines);
        UnityEngine.PlayerPrefs.SetInt(slotName + "_Level", this.Level);
    }

    public override string ToString()
    {
        return string.Format("{0} {1} {2} lines", this.PlayerName, this.Score, this.Lines);
    }
}