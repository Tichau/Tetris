// <copyright file="Profile.IProfileManagement.cs" company="1WeekEndStudio">Copyright 1WeekEndStudio. All rights reserved.</copyright>

using UnityEngine;

public partial class Profile : IProfileManagement
{
    void IProfileManagement.SetId(int id)
    {
        this.ID = id;

        if (this.ProfileChange != null)
        {
            this.ProfileChange.Invoke(this, null);
        }
    }

    void IProfileManagement.SetPassword(string password)
    {
        this.Password = password;

        if (this.ProfileChange != null)
        {
            this.ProfileChange.Invoke(this, null);
        }
    }

    void IProfileManagement.SetName(string name)
    {
        this.Name = name;

        if (this.ProfileChange != null)
        {
            this.ProfileChange.Invoke(this, null);
        }
    }

    public void ClearUnsynchronizedScores()
    {
        this.UnsynchronizedScores.Clear();

        NetworkManager.Instance.RefreshHighscores(HighScoresManager.HighScoreCount);

        if (this.ProfileChange != null)
        {
            this.ProfileChange.Invoke(this, null);
        }
    }
}