// <copyright file="ScoreDisplayer.cs" company="1WeekEndStudio">Copyright 1WeekEndStudio. All rights reserved.</copyright>

using UnityEngine;

public class ScoreDisplayer : MonoBehaviour
{
    private void OnGUI()
    {
        if (Application.Instance.Game.Statistics == null)
        {
            return;
        }

        GUIManager guiManager = GUIManager.Instance;
        
        float margin = guiManager.GetLenght(GUIManager.Margin);

        float scorePanelHeight = guiManager.GetLenght(125f);
        float scorePanelWidth = guiManager.GetLenght(160f);
        float numbersAlignement = guiManager.GetLenght(100f);
        float insideMargin = guiManager.GetLenght(10f);
        float scoreTitleHeight = guiManager.GetLenght(40f);
        float subTitleHeight = guiManager.GetLenght(30f);
        float secondLineTop = guiManager.GetLenght(50f);
        float thirdLineTop = guiManager.GetLenght(80f);

        Rect rendererArea = Application.Instance.BlocGridRendererArea;
        Vector3 bottomRightPoint = new Vector3(rendererArea.x + rendererArea.width, rendererArea.y);

        Vector2 position = CameraController.Instance.WorldToGUIPosition(bottomRightPoint);

        float left = position.x + margin;
        float top = position.y - scorePanelHeight;

        GUI.BeginGroup(new Rect(left, top, scorePanelWidth, scorePanelHeight), GUIManager.Instance.GetGuiStyle(GUIStyleCategory.Light));

        GUI.Label(new Rect(insideMargin, insideMargin, scorePanelWidth, scoreTitleHeight), Application.Instance.Game.Statistics.Score.ToString(), GUIManager.Instance.GetGuiStyle(GUIStyleCategory.BigText));

        GUI.Label(new Rect(insideMargin, secondLineTop, numbersAlignement, subTitleHeight), Localization.GetLocalizedString("%Level"), GUIManager.Instance.GetGuiStyle(GUIStyleCategory.Text));
        GUI.Label(new Rect(insideMargin + numbersAlignement, secondLineTop, scorePanelWidth - numbersAlignement, subTitleHeight), (Application.Instance.Game.Statistics.Level + 1).ToString(), GUIManager.Instance.GetGuiStyle(GUIStyleCategory.Text));

        GUI.Label(new Rect(insideMargin, thirdLineTop, scorePanelWidth, subTitleHeight), Localization.GetLocalizedString("%Lines"), GUIManager.Instance.GetGuiStyle(GUIStyleCategory.Text));
        GUI.Label(new Rect(insideMargin + numbersAlignement, thirdLineTop, scorePanelWidth - numbersAlignement, subTitleHeight), Application.Instance.Game.Statistics.Lines.ToString(), GUIManager.Instance.GetGuiStyle(GUIStyleCategory.Text));

        GUI.EndGroup();
    }
}
