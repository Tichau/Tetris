// <copyright file="Profile.cs" company="1WeekEndStudio">Copyright 1WeekEndStudio. All rights reserved.</copyright>

using System;
using System.Collections.Generic;

using UnityEngine;

public partial class Profile
{
    public Profile()
    {
        this.Name = string.Empty;
        this.Password = string.Empty;
        this.ID = -1;
        this.BestScore = new GameStatistics();
        this.BestScore.Registered = true;
        this.UnsynchronizedScores = new List<GameStatistics>();
    }

    public Profile(string name, int id)
    {
        this.Name = name;
        this.Password = string.Empty;
        this.ID = id;
        this.BestScore = new GameStatistics();
        this.BestScore.Registered = true;
        this.UnsynchronizedScores = new List<GameStatistics>();
    }

    public event EventHandler<EventArgs> ProfileChange;

    public string Name
    {
        get;
        private set;
    }

    public string Password
    {
        get;
        private set;
    }

    public int ID
    {
        get;
        private set;
    }

    public List<GameStatistics> UnsynchronizedScores
    {
        get;
        private set;
    }

    public GameStatistics BestScore
    {
        get;
        private set;
    }

    public bool IsSynchronized
    {
        get
        {
            return this.ID >= 0 && this.UnsynchronizedScores.Count == 0;
        }
    }

    public static void SaveEmptyProfile(string prefix)
    {
        PlayerPrefs.SetString(string.Format("{0}.Name", prefix), string.Empty);
    }
    
    public void RegisterScore(GameStatistics gameStatistics)
    {
        this.UnsynchronizedScores.Add(gameStatistics);

        float lastScore = this.BestScore != null ? this.BestScore.Score : 0f;
        if (gameStatistics.Score > lastScore)
        {
            this.BestScore = gameStatistics;
        }

        gameStatistics.Registered = true;

        HighScoresManager.Instance.RegisterScore(this, gameStatistics);

        ProfileManager.Instance.SynchronizeProfile(this);

        if (this.ProfileChange != null)
        {
            this.ProfileChange.Invoke(this, null);
        }
    }

    public void LoadProfile(string prefix)
    {
        this.Name = PlayerPrefs.GetString(string.Format("{0}.Name", prefix));
        this.ID = PlayerPrefs.GetInt(string.Format("{0}.ID", prefix));
        this.Password = PlayerPrefs.GetString(string.Format("{0}.Password", prefix));
        this.BestScore = new GameStatistics();
        this.BestScore.Load(string.Format("{0}.BestScore", prefix));

        int unsynchronizedScoresCount = PlayerPrefs.GetInt(string.Format("{0}.UnsynchronizedScoresCount", prefix));
        for (int index = 0; index < unsynchronizedScoresCount; index++)
        {
            GameStatistics gameStatistics = new GameStatistics();
            gameStatistics.Load(string.Format("{0}.UnsynchronizedScore{1}", prefix, index));
            this.UnsynchronizedScores.Add(gameStatistics);
        }
    }

    public void SaveProfile(string prefix)
    {
        PlayerPrefs.SetString(string.Format("{0}.Name", prefix), this.Name);
        PlayerPrefs.SetInt(string.Format("{0}.ID", prefix), this.ID);
        PlayerPrefs.SetString(string.Format("{0}.Password", prefix), this.Password);
        this.BestScore.Save(string.Format("{0}.BestScore", prefix));

        PlayerPrefs.SetInt(string.Format("{0}.UnsynchronizedScoresCount", prefix), this.UnsynchronizedScores.Count);
        for (int index = 0; index < this.UnsynchronizedScores.Count; index++)
        {
            this.UnsynchronizedScores[index].Save(string.Format("{0}.UnsynchronizedScore{1}", prefix, index));
        }
    }

    public override string ToString()
    {
        string result = string.Format("ID: {0} Name: {1} Password: {2}\nBestScore: {3}\n", this.ID, this.Name, this.Password, this.BestScore);

        result += "UnsynchronizedScores\n";
        for (int index = 0; index < this.UnsynchronizedScores.Count; index++)
        {
            result += this.UnsynchronizedScores[index] + "\n";
        }

        return result;
    }
}
