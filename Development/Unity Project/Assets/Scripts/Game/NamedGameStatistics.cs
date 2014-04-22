// <copyright file="NamedGameStatistics.cs" company="BlobTeam">Copyright BlobTeam. All rights reserved.</copyright>

using System.Collections;

using UnityEngine;

public class NamedGameStatistics : GameStatistics
{
    public NamedGameStatistics() :
        base()
    {
        this.ProfileName = string.Empty;
    }

    public NamedGameStatistics(string profileName, int score, int lines, int startLevel) :
        base(score, lines, startLevel)
    {
        this.ProfileName = profileName;
    }

    public NamedGameStatistics(string profileName, GameStatistics gameStatistics)
        : base(gameStatistics.Score, gameStatistics.Lines, gameStatistics.StartLevel)
    {
        this.ProfileName = profileName;
    }

    public string ProfileName
    {
        get;
        set;
    }

    public override void Load(string prefix)
    {
        base.Load(prefix);

        this.ProfileName = PlayerPrefs.GetString(string.Format("{0}.ProfileName", prefix));
    }

    public override void Save(string prefix)
    {
        base.Save(prefix);

        PlayerPrefs.SetString(string.Format("{0}.ProfileName", prefix), this.ProfileName);
    }

    public override string ToString()
    {
        return string.Format("ProfileName: {0} {1}", this.ProfileName, base.ToString());
    }
}
