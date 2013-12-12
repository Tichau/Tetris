// <copyright file="GUIManager.cs" company="BlobTeam">Copyright BlobTeam. All rights reserved.</copyright>

using UnityEngine;
using System.Collections;

public class GUIManager : MonoBehaviour
{
    [UnityEngine.SerializeField]
    private GUIText scoreLabel;

    [UnityEngine.SerializeField]
    private GUIText levelLabel;

    [UnityEngine.SerializeField]
    private GUIText linesLabel;

    private void Start()
    {
    }
    
    private void Update()
    {
        this.scoreLabel.text = Application.Instance.Game.Statistics.Score.ToString();
        this.levelLabel.text = string.Format("Level\t  {0}", Application.Instance.Game.Statistics.Level + 1);
        this.linesLabel.text = string.Format("Lines\t  {0}", Application.Instance.Game.Statistics.Lines);
    }
}
