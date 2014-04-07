// <copyright file="HighScores.cs" company="1WeekEndStudio">Copyright 1WeekEndStudio. All rights reserved.</copyright>

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public static class HighScores
{
    public const int HighScoreCount = 10;

    private static List<GameStatistics> highScores = new List<GameStatistics>(HighScoreCount);

    static HighScores()
    {
        HighScoresCollection = highScores.AsReadOnly();

        int index = 0;
        while (index < HighScoreCount)
        {
            string slotName = "Highscore" + index;
            if (!PlayerPrefs.HasKey(slotName))
            {
                break;
            }

            GameStatistics gameStatistics = new GameStatistics();
            gameStatistics.Load(slotName);
            highScores.Add(gameStatistics);
            index++;
        }
    }

    public static event EventHandler HighScoresChange;

    public static ReadOnlyCollection<GameStatistics> HighScoresCollection
    {
        get;
        private set;
    }

    public static bool CanRegisterGameStatistic(GameStatistics gameStatistics)
    {
        if (gameStatistics.Score <= 0)
        {
            return false;
        }

        if (highScores.Count >= HighScoreCount && gameStatistics.Score <= highScores[highScores.Count - 1].Score)
        {
            return false;
        }

        return true;
    }

    public static void RegisterScore(GameStatistics gameStatistics)
    {
        bool scoreInserted = false;

        for (int index = 0; index < highScores.Count; ++index)
        {
            if (highScores[index].Score >= gameStatistics.Score)
            {
                continue;
            }

            scoreInserted = true;
            highScores.Insert(index, gameStatistics);
            break;
        }

        if (highScores.Count > HighScoreCount)
        {
            highScores.RemoveAt(highScores.Count - 1);
        }
        else if (!scoreInserted && highScores.Count < HighScoreCount)
        {
            highScores.Add(gameStatistics);
        }

        for (int index = 0; index < highScores.Count; ++index)
        {
            string slotName = "Highscore" + index;
            highScores[index].Save(slotName);
            PlayerPrefs.SetInt(slotName, index);
            PlayerPrefs.Save();
        }

        if (HighScoresChange != null)
        {
            HighScoresChange.Invoke(null, new EventArgs());
        }
    }
}
