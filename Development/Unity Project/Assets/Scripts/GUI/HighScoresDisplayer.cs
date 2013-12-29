using UnityEngine;

public class HighScoresDisplayer : MonoBehaviour
{
    private void OnGUI()
    {
        const float firstOffset = 50f;
        const float offset = 25f;
        float top = 0.3f*Screen.height;
        GUI.Label(new Rect(20f, top, 200f, 40f), Localization.GetLocalizedString("%HighScores"), GUIManager.Instance.BigTextStyle);

        for (int index = 0; index < HighScores.HighScoresCollection.Count; index++)
        {
            GameStatistics gameStatistics = HighScores.HighScoresCollection[index];
            float scoreLineTop = top + firstOffset + index * offset;
            GUI.Label(new Rect(20f, scoreLineTop, 200f, 30f), (index + 1).ToString(), GUIManager.Instance.SmallTextStyle);
            GUI.Label(new Rect(50f, scoreLineTop, 200f, 30f), gameStatistics.PlayerName, GUIManager.Instance.SmallTextStyle);
            GUI.Label(new Rect(200f, scoreLineTop, 70f, 30f), gameStatistics.Score.ToString(), GUIManager.Instance.RightAlignSmallTextStyle);
        }
    }
}
