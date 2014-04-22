// <copyright file="HighScoresManager.cs" company="1WeekEndStudio">Copyright 1WeekEndStudio. All rights reserved.</copyright>

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using UnityEngine;

public class HighScoresManager : MonoBehaviour
{
    public const int HighScoreCount = 10;
    
    private List<NamedGameStatistics> highScores = new List<NamedGameStatistics>(HighScoreCount);
    
    public HighScoresManager()
    {
        this.HighScoresCollection = this.highScores.AsReadOnly();
    }

    public event EventHandler HighScoresChange;

    public static HighScoresManager Instance
    {
        get;
        private set;
    }

    public ReadOnlyCollection<NamedGameStatistics> HighScoresCollection
    {
        get;
        private set;
    }
    
    public void RegisterScore(Profile profile, GameStatistics gameStatistics)
    {
        bool scoreInserted = false;

        for (int index = 0; index < this.highScores.Count; ++index)
        {
            if (this.highScores[index].Score >= gameStatistics.Score)
            {
                continue;
            }

            scoreInserted = true;
            this.highScores.Insert(index, new NamedGameStatistics(profile.Name, gameStatistics));
            break;
        }

        if (this.highScores.Count > HighScoreCount)
        {
            this.highScores.RemoveAt(this.highScores.Count - 1);
        }
        else if (!scoreInserted && this.highScores.Count < HighScoreCount)
        {
            this.highScores.Add(new NamedGameStatistics(profile.Name, gameStatistics));
        }

        this.Save();

        if (this.HighScoresChange != null)
        {
            this.HighScoresChange.Invoke(null, new EventArgs());
        }
    }

    public void RegisterScores(IEnumerable<NamedGameStatistics> gameStatistics)
    {
        this.highScores.Clear();
        this.highScores.AddRange(gameStatistics);

        this.Save();
    }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        this.Load();

        for (int index = 0; index < ProfileManager.Instance.ProfilesCollection.Count; index++)
        {
            Profile profile = ProfileManager.Instance.ProfilesCollection[index];
            for (int scoreIndex = 0; scoreIndex < profile.UnsynchronizedScores.Count; scoreIndex++)
            {
                GameStatistics unsynchronizedScore = profile.UnsynchronizedScores[scoreIndex];
                this.RegisterScore(profile, unsynchronizedScore);
            }
        }

        NetworkManager.Instance.RefreshHighscores(HighScoreCount);
    }

    private void Load()
    {
        for (int index = 0; index < HighScoreCount; index++)
        {
            string prefix = "Highscore" + index;
            NamedGameStatistics gameStatistics = new NamedGameStatistics();
            gameStatistics.Load(prefix);

            if (string.IsNullOrEmpty(gameStatistics.ProfileName))
            {
                continue;
            }

            this.highScores.Add(gameStatistics);
        }
    }

    private void Save()
    {
        for (int index = 0; index < this.highScores.Count; index++)
        {
            string prefix = "Highscore" + index;
            NamedGameStatistics gameStatistics = this.highScores[index];
            gameStatistics.Save(prefix);
        }
    }
}
