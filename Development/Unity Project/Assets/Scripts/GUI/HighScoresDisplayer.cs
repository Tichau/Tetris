using System;
using UnityEngine;

public class HighScoresDisplayer : MonoBehaviour
{
    public GameObject ScoreLinePrefab;

    private ScoreLine[] scoreLines;

    private void Start()
    {
        float firstOffset = 50f/Screen.height;
        float offset = 25f/Screen.height;

        this.scoreLines = new ScoreLine[HighScores.HighScoreCount];
        for (int index = 0; index < HighScores.HighScoreCount; index++)
        {
            GameObject scoreLineGameObject = GameObject.Instantiate(this.ScoreLinePrefab) as GameObject;
            scoreLineGameObject.transform.parent = this.transform;
            scoreLineGameObject.transform.localPosition = new Vector3(0f, -firstOffset - index * offset, 0f);

            ScoreLine scoreLine = scoreLineGameObject.GetComponent<ScoreLine>();
            this.scoreLines[index] = scoreLine;

            scoreLine.gameObject.SetActive(false);
            this.HighScores_HighScoresChange(this, null);
        }

        HighScores.HighScoresChange += this.HighScores_HighScoresChange;
    }

    private void HighScores_HighScoresChange(object sender, EventArgs e)
    {
        for (int index = 0; index < this.scoreLines.Length; index++)
        {
            ScoreLine scoreLine = this.scoreLines[index];
            if (scoreLine == null)
            {
                Debug.LogWarning("Can't retrieve score line " + index);
                continue;
            }

            if (index >= HighScores.HighScoresCollection.Count)
            {
                scoreLine.gameObject.SetActive(false);
                continue;
            }

            GameStatistics gameStatistics = HighScores.HighScoresCollection[index];
            if (gameStatistics == null)
            {
                scoreLine.gameObject.SetActive(false);
                continue;
            }

            scoreLine.gameObject.SetActive(true);

            scoreLine.RankLabel.text = (index + 1).ToString();
            scoreLine.PlayerLabel.text = gameStatistics.PlayerName;
            scoreLine.ScoreLabel.text = gameStatistics.Score.ToString();
        }
    }
}
