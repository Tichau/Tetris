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

        Vector3 diff = this.CameraGameObject.camera.WorldToScreenPoint(new Vector3(0f, 1f, 0f)) - this.CameraGameObject.camera.WorldToScreenPoint(new Vector3(0f, 0f, 0f));
        float offset = (Screen.height - (diff.y * 22f * 62f / 64f)) / 2f;

        const float ScorePanelHeight = 125f;
        const float ScorePanelWidth = 160f;
        const float NumbersAlignement = 100f;
        const float Margin = 10f;
        
        float left = 0.68f * Screen.width;
        float top = Screen.height - offset - ScorePanelHeight;

        GUI.BeginGroup(new Rect(left, top, ScorePanelWidth, ScorePanelHeight), GUIManager.Instance.LightStyle);
        GUI.Label(new Rect(Margin, Margin, ScorePanelWidth, 40f), Application.Instance.Game.Statistics.Score.ToString(), GUIManager.Instance.BigTextStyle);

        GUI.Label(new Rect(Margin, 50f, NumbersAlignement, 30f), Localization.GetLocalizedString("%Level"), GUIManager.Instance.TextStyle);
        GUI.Label(new Rect(Margin + NumbersAlignement, 50f, ScorePanelWidth - NumbersAlignement, 30f), (Application.Instance.Game.Statistics.Level + 1).ToString(), GUIManager.Instance.TextStyle);

        GUI.Label(new Rect(Margin, 80f, ScorePanelWidth, 30f), Localization.GetLocalizedString("%Lines"), GUIManager.Instance.TextStyle);
        GUI.Label(new Rect(Margin + NumbersAlignement, 80f, ScorePanelWidth - NumbersAlignement, 30f), Application.Instance.Game.Statistics.Lines.ToString(), GUIManager.Instance.TextStyle);
        
        GUI.EndGroup();
    }
}
