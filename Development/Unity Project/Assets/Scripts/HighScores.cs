﻿using System.Collections.Generic;
using UnityEngine;

public static class HighScores
{
    private const int HighScoreCount = 10;
    private static List<GameStatistics> highScores = new List<GameStatistics>(HighScoreCount);

    static HighScores()
    {
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

        Debug.Log("Loaded scores:");
        for (index = 0; index < highScores.Count; ++index)
        {
            Debug.Log((index + 1) + ": " + highScores[index]);
        }
    }

    public static void RegisterGameStatistic(GameStatistics gameStatistics)
    {
        if (gameStatistics.Score <= 0)
        {
            return;
        }

        if (highScores.Count >= HighScoreCount && gameStatistics.Score <= highScores[highScores.Count - 1].Score)
        {
            return;
        }

        gameStatistics.PlayerName = InputManager.Instance.GetName();
        
        RegisterScore(gameStatistics);
    }

    private static void RegisterScore(GameStatistics gameStatistics)
    {
        bool scoreInserted = false;

        for (int index = 0; index < highScores.Count; ++index)
        {
            if (highScores[index].Score > gameStatistics.Score)
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

            Debug.Log((index + 1) + ": " + highScores[index]);
        }
    }
}
