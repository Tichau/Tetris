// <copyright file="ProfileManager.cs" company="1WeekEndStudio">Copyright 1WeekEndStudio. All rights reserved.</copyright>

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using UnityEngine;

public class ProfileManager : MonoBehaviour
{
    public const int MaximumNumberOfProfile = 3;

    private List<Profile> profiles = new List<Profile>();

    public ProfileManager()
    {
        this.ProfilesCollection = this.profiles.AsReadOnly();
    }

    public static ProfileManager Instance
    {
        get;
        private set;
    }

    public Profile CurrentProfile
    {
        get;
        private set;
    }

    public ReadOnlyCollection<Profile> ProfilesCollection
    {
        get;
        private set;
    }

    public bool IsLoaded
    {
        get;
        private set;
    }

    public void ChangeCurrentProfile(Profile profile)
    {
        this.CurrentProfile = profile;

        this.Save();
    }

    public Profile CreateNewProfile(string newProfileName, int id = -1)
    {
        Profile profile = new Profile(newProfileName, id);

        this.profiles.Add(profile);

        this.Save();

        profile.ProfileChange += this.ProfileChangeDelegate;

        return profile;
    }

    public void SynchronizeProfile(Profile profile)
    {
        if (profile == null)
        {
            throw new ArgumentNullException("profile");
        }

        if (profile.ID < 0)
        {
            // It is a local profile.
            if (profile.IsSynchronizable)
            {
                NetworkManager.Instance.IsProfileExist(profile, this.CreateProfileDelegate);
            }
            else
            {
                // It is a local profile only, try to create a new profile from the data of the current one.
                this.CreateProfileDelegate(profile, false);
            }
        }
        else
        {
            // It is an already validate profile.
            NetworkManager.Instance.SynchronizeProfile(profile);
        }
    }

    private void ProfileChangeDelegate(object sender, EventArgs e)
    {
        this.Save();
    }

    private void CreateProfileDelegate(Profile profile, bool isProfileExist)
    {
        if (isProfileExist)
        {
            // The profile already exist, prompt a password window.
            GUIManager.ProfileToConnect = profile;
            Application.Instance.PostScreenChange(Application.ApplicationScreen.OnlineProfileConnection);
        }
        else
        {
            // The profile doesn't exist, prompt a profile create window.
            GUIManager.ProfileToConnect = profile;
            Application.Instance.PostScreenChange(Application.ApplicationScreen.OnlineProfileCreation);
        }
    }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        // Load profiles.
        this.Load();

        if (this.profiles.Count == 0)
        {
            // Create a default profile.
            Profile defaultProfile = this.CreateNewProfile(Localization.GetLocalizedString("%AnonymousProfileName"), -2);

            this.CurrentProfile = defaultProfile;
        }

        ////// TEMP!
        ////this.profiles.Clear();
        ////this.CurrentProfile = null;
        ////this.Save();
    }

    private void Load()
    {
        for (int index = 0; index < MaximumNumberOfProfile; index++)
        {
            Profile profile = this.LoadProfile(index);
            if (profile != null)
            {
                Debug.Log("Profile loaded: " + profile);
            }
        }

        string currentProfileName = PlayerPrefs.GetString("CurrentProfileName", string.Empty);
        this.CurrentProfile = string.IsNullOrEmpty(currentProfileName) ? null : this.profiles.Find(profile => profile.Name == currentProfileName);

        this.IsLoaded = true;
    }

    private void Save()
    {
        PlayerPrefs.SetString("CurrentProfileName", this.CurrentProfile != null ? this.CurrentProfile.Name : string.Empty);

        for (int index = 0; index < MaximumNumberOfProfile; index++)
        {
            Profile profile = null;

            if (index < this.profiles.Count)
            {
                profile = this.profiles[index];
            }

            this.SaveProfile(profile, index);
            
            if (profile != null)
            {
                Debug.Log("Save profile: " + profile.ToString());
            }
        }
    }

    private Profile LoadProfile(int slotIndex)
    {
        string prefix = string.Format("Profile{0}.", slotIndex);

        Profile profile = new Profile();
        profile.LoadProfile(prefix);

        if (string.IsNullOrEmpty(profile.Name))
        {
            return null;
        }

        this.profiles.Add(profile);
        profile.ProfileChange += this.ProfileChangeDelegate;

        return profile;
    }

    private void SaveProfile(Profile profile, int slotIndex)
    {
        string prefix = string.Format("Profile{0}.", slotIndex);

        if (profile == null)
        {
            Profile.SaveEmptyProfile(prefix);
            return;
        } 
        
        profile.SaveProfile(prefix);
    }
}
