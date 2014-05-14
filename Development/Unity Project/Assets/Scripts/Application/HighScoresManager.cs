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
    
    public void RegisterScore(Profile profile, GameStatistics gameStatistics, bool isLocal)
    {
        bool scoreInserted = false;
        NamedGameStatistics namedGameStatistics = null;

        for (int index = 0; index < this.highScores.Count; ++index)
        {
            if (this.highScores[index].Score >= gameStatistics.Score)
            {
                continue;
            }

            scoreInserted = true;

            namedGameStatistics = new NamedGameStatistics(profile.Name, gameStatistics, isLocal);
            this.highScores.Insert(index, namedGameStatistics);
            break;
        }

        if (this.highScores.Count > HighScoreCount)
        {
            this.highScores.RemoveAt(this.highScores.Count - 1);
        }
        else if (!scoreInserted && this.highScores.Count < HighScoreCount)
        {
            namedGameStatistics = new NamedGameStatistics(profile.Name, gameStatistics, isLocal);
            this.highScores.Add(namedGameStatistics);
        }

        Debug.Log(string.Format("RegisterScore {0}", namedGameStatistics != null ? namedGameStatistics.ToString() : "null"));
         
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

        this.RegisterUnsynchronizedScores();

        this.Save();
    }

    private void Awake()
    {
        Instance = this;
    }

    private System.Collections.IEnumerator Start()
    {
        this.Load();

        while (!ProfileManager.Instance.IsLoaded)
        {
            yield return null;
        }

        this.RegisterUnsynchronizedScores();

        NetworkManager.Instance.RefreshHighscores(HighScoreCount);
    }

    private void Load()
    {
        for (int index = 0; index < HighScoreCount; index++)
        {
            string prefix = "Highscore" + index;
            NamedGameStatistics gameStatistics = new NamedGameStatistics(false);
            gameStatistics.Load(prefix);

            if (string.IsNullOrEmpty(gameStatistics.ProfileName))
            {
                continue;
            }

            this.highScores.Add(gameStatistics);
        }
    }

    private void RegisterUnsynchronizedScores()
    {
        for (int index = 0; index < ProfileManager.Instance.ProfilesCollection.Count; index++)
        {
            Profile profile = ProfileManager.Instance.ProfilesCollection[index];
            for (int scoreIndex = 0; scoreIndex < profile.UnsynchronizedScores.Count; scoreIndex++)
            {
                GameStatistics unsynchronizedScore = profile.UnsynchronizedScores[scoreIndex];
                this.RegisterScore(profile, unsynchronizedScore, true);
            }
        }
    }

    private void Save()
    {
        NamedGameStatistics emptyStatistics = new NamedGameStatistics(string.Empty, 0, 0, 0, true);
        for (int index = 0; index < HighScoreCount; index++)
        {
            string prefix = "Highscore" + index;

            if (index >= this.highScores.Count || this.highScores[index].IsLocal)
            {
                emptyStatistics.Save(prefix);
                Debug.Log("Save in slot " + prefix + " empty profile");
                continue;
            }
            
            this.highScores[index].Save(prefix);
            Debug.Log("Save in slot " + prefix + ": " + this.highScores[index]);
        }
    }
}
