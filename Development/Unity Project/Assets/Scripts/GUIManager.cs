// <copyright file="GUIManager.cs" company="BlobTeam">Copyright BlobTeam. All rights reserved.</copyright>

using UnityEngine;

public class GUIManager : MonoBehaviour
{
    [SerializeField]
    private GUIText scoreLabel;

    [SerializeField]
    private GUIText levelLabel;

    [SerializeField]
    private GUIText linesLabel;

    private void Start()
    {
    }
    
    private void Update()
    {
        if (Application.Instance.Game.Statistics == null)
        {
            this.scoreLabel.enabled = false;
            this.levelLabel.enabled = false;
            this.linesLabel.enabled = false;
        }
        else
        {
            this.scoreLabel.enabled = true;
            this.levelLabel.enabled = true;
            this.linesLabel.enabled = true;
            this.scoreLabel.text = Application.Instance.Game.Statistics.Score.ToString();
            this.levelLabel.text = string.Format("Level\t  {0}", Application.Instance.Game.Statistics.Level + 1);
            this.linesLabel.text = string.Format("Lines\t  {0}", Application.Instance.Game.Statistics.Lines);
        }
    }
}
