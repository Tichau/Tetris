// <copyright file="GUIManager.cs" company="BlobTeam">Copyright BlobTeam. All rights reserved.</copyright>

using UnityEngine;
using System.Collections;

public class GUIManager : MonoBehaviour
{
    [UnityEngine.SerializeField]
    private GUIText scoreLabel;

    private void Start()
    {
    }
    
    private void Update()
    {
        this.scoreLabel.text = Application.Instance.Game.Statistics.Score.ToString();
    }
}
