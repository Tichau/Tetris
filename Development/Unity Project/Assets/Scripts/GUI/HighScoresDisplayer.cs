// <copyright file="HighScoresDisplayer.cs" company="BlobTeam">Copyright BlobTeam. All rights reserved.</copyright>

using UnityEngine;

public class HighScoresDisplayer : MonoBehaviour
{
    private void OnGUI()
    {
        GUIManager guiManager = GUIManager.Instance;

        float highScoreLineHeight = guiManager.GetLenght(30f);
        float highScoreTitleHeight = guiManager.GetLenght(40f);

        float firstOffset = guiManager.GetLenght(50f);
        float offset = guiManager.GetLenght(25f);
        float left = guiManager.GetLenght(20f);
        float top = guiManager.GetLenght(200f); //0.3f * Screen.height;

        float rankIndexWidth = guiManager.GetLenght(30f);
        float playerNameWidth = guiManager.GetLenght(180f);
        float scoreWidth = guiManager.GetLenght(70f);

        if (guiManager.Mode == GUIManager.GUIMode.Portrait)
        {
            float margin = guiManager.GetLenght(30f);

            Rect rendererArea = Application.Instance.BlocGridRendererArea;
            Vector3 worldToScreenPoint = CameraController.Instance.Camera.WorldToScreenPoint(new Vector3(rendererArea.x + rendererArea.width, rendererArea.y + rendererArea.height, 0f));
            left = margin + worldToScreenPoint.x;
        }

        GUI.Label(new Rect(left, top, rankIndexWidth + playerNameWidth + scoreWidth, highScoreTitleHeight), Localization.GetLocalizedString("%HighScores"), guiManager.GetGuiStyle(GuiStyleCategory.BigText));

        for (int index = 0; index < HighScores.HighScoresCollection.Count; index++)
        {
            GameStatistics gameStatistics = HighScores.HighScoresCollection[index];
            float scoreLineTop = top + firstOffset + (index * offset);
            GUI.Label(new Rect(left, scoreLineTop, rankIndexWidth, highScoreLineHeight), (index + 1).ToString(), guiManager.GetGuiStyle(GuiStyleCategory.SmallText));
            GUI.Label(new Rect(left + rankIndexWidth, scoreLineTop, playerNameWidth, highScoreLineHeight), gameStatistics.PlayerName, guiManager.GetGuiStyle(GuiStyleCategory.SmallText));
            GUI.Label(new Rect(left + playerNameWidth, scoreLineTop, scoreWidth, highScoreLineHeight), gameStatistics.Score.ToString(), guiManager.GetGuiStyle(GuiStyleCategory.RightAlignSmallText));
        }
    }
}
