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

        Vector3 diff = CameraGameObject.camera.WorldToScreenPoint(new Vector3(0f, 1f, 0f)) - CameraGameObject.camera.WorldToScreenPoint(new Vector3(0f, 0f, 0f));
        float offset = (Screen.height - (diff.y * 22f * 62f/64f)) / 2f;

        const float scorePanelHeight = 125f;
        const float scorePanelWidth = 160f;
        const float numbersAlignement = 100f;
        const float margin = 10f;
        
        float left = 0.68f*Screen.width;
        float top = Screen.height - offset - scorePanelHeight;

        GUI.BeginGroup(new Rect(left, top, scorePanelWidth, scorePanelHeight), GUIManager.Instance.LightStyle);
        GUI.Label(new Rect(margin, margin, scorePanelWidth, 40f), Application.Instance.Game.Statistics.Score.ToString(), GUIManager.Instance.BigTextStyle);

        GUI.Label(new Rect(margin, 50f, numbersAlignement, 30f), Localization.GetLocalizedString("%Level"), GUIManager.Instance.TextStyle);
        GUI.Label(new Rect(margin + numbersAlignement, 50f, scorePanelWidth - numbersAlignement, 30f), (Application.Instance.Game.Statistics.Level + 1).ToString(), GUIManager.Instance.TextStyle);

        GUI.Label(new Rect(margin, 80f, scorePanelWidth, 30f), Localization.GetLocalizedString("%Lines"), GUIManager.Instance.TextStyle);
        GUI.Label(new Rect(margin + numbersAlignement, 80f, scorePanelWidth - numbersAlignement, 30f), Application.Instance.Game.Statistics.Lines.ToString(), GUIManager.Instance.TextStyle);
        
        GUI.EndGroup();
    }
}
