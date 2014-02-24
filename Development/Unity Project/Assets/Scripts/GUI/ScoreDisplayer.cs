// <copyright file="ScoreDisplayer.cs" company="BlobTeam">Copyright BlobTeam. All rights reserved.</copyright>

using UnityEngine;

public class ScoreDisplayer : MonoBehaviour
{
    public GameObject CameraGameObject;

    [SerializeField]
    private float scorePanelBottomOffset;
    
    private void OnGUI()
    {
        if (Application.Instance.Game.Statistics == null)
        {
            return;
        }

        GUIManager guiManager = GUIManager.Instance;
        
        float scorePanelHeight = guiManager.GetLenght(125f);
        float scorePanelWidth = guiManager.GetLenght(160f);
        float numbersAlignement = guiManager.GetLenght(100f);
        float margin = guiManager.GetLenght(30f);
        float insideMargin = guiManager.GetLenght(10f);
        float scoreTitleHeight = guiManager.GetLenght(40f);
        float subTitleHeight = guiManager.GetLenght(30f);
        float secondLineTop = guiManager.GetLenght(50f);
        float thirdLineTop = guiManager.GetLenght(80f);

        Rect rendererArea = Application.Instance.BlocGridRendererArea;
        Vector3 bottomRightPoint = new Vector3(rendererArea.x + rendererArea.width, rendererArea.y);
        Vector3 worldToScreenPoint = this.CameraGameObject.camera.WorldToScreenPoint(bottomRightPoint);

        float left = worldToScreenPoint.x + margin;

        // Unity GUI and Unity Camera doesn't use the same referentiel. 
        // UnityGUI: Y=0 -> Top of the screen. UnityCameraScreenPoint: Y=0 -> Bottom of the screen.
        float top = Screen.height - worldToScreenPoint.y - scorePanelHeight;

        GUI.BeginGroup(new Rect(left, top, scorePanelWidth, scorePanelHeight), GUIManager.Instance.GetGuiStyle(GuiStyleCategory.Light));

        GUI.Label(new Rect(insideMargin, insideMargin, scorePanelWidth, scoreTitleHeight), Application.Instance.Game.Statistics.Score.ToString(), GUIManager.Instance.GetGuiStyle(GuiStyleCategory.BigText));

        GUI.Label(new Rect(insideMargin, secondLineTop, numbersAlignement, subTitleHeight), Localization.GetLocalizedString("%Level"), GUIManager.Instance.GetGuiStyle(GuiStyleCategory.Text));
        GUI.Label(new Rect(insideMargin + numbersAlignement, secondLineTop, scorePanelWidth - numbersAlignement, subTitleHeight), (Application.Instance.Game.Statistics.Level + 1).ToString(), GUIManager.Instance.GetGuiStyle(GuiStyleCategory.Text));

        GUI.Label(new Rect(insideMargin, thirdLineTop, scorePanelWidth, subTitleHeight), Localization.GetLocalizedString("%Lines"), GUIManager.Instance.GetGuiStyle(GuiStyleCategory.Text));
        GUI.Label(new Rect(insideMargin + numbersAlignement, thirdLineTop, scorePanelWidth - numbersAlignement, subTitleHeight), Application.Instance.Game.Statistics.Lines.ToString(), GUIManager.Instance.GetGuiStyle(GuiStyleCategory.Text));

        GUI.EndGroup();
    }
}
