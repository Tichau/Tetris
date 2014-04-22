// <copyright file="GUIManager.Profile.cs" company="1WeekEndStudio">Copyright 1WeekEndStudio. All rights reserved.</copyright>

using System.Collections.Generic;
using UnityEngine;

public partial class GUIManager : MonoBehaviour
{
    public static Profile ProfileToConnect;

    private string newProfileName = string.Empty;
    private string onlineCreateProfileName = null;
    private string password = string.Empty;
    private string passwordConfirmation = string.Empty;

    private void ProfileWindow(int windowId)
    {
        float windowsWidth = this.GetLenght(this.windowWidth);
        float windowsHeight = this.GetLenght(this.windowHeight);

        float buttonWidth = this.GetLenght(200f);
        float buttonHeight = this.GetLenght(35f);

        float left = (windowsWidth / 2f) - (buttonWidth / 2f);
        float topMargin = this.GetLenght(70f);

        for (int index = 0; index < ProfileManager.MaximumNumberOfProfile; index++)
        {
            Profile profile = null;
            if (index < ProfileManager.Instance.ProfilesCollection.Count)
            {
                profile = ProfileManager.Instance.ProfilesCollection[index];
            }

            string buttonTitle = profile != null ? profile.Name : Localization.GetLocalizedString("%NewProfile");

            float top = topMargin + (index * buttonHeight * 1.3f);
            if (GUI.Button(new Rect(left, top, buttonWidth, buttonHeight), buttonTitle, this.GetGuiStyle(GUIStyleCategory.Dark)))
            {
                if (profile != null)
                {
                    ProfileManager.Instance.ChangeCurrentProfile(profile);
                    Application.Instance.BackToLastScreen();
                }
                else
                {
                    this.newProfileName = string.Empty;
                    Application.Instance.PostScreenChange(Application.ApplicationScreen.ProfileCreation);
                }
            }
        }

        {
            float top = topMargin + ((ProfileManager.MaximumNumberOfProfile + 0.5f) * buttonHeight * 1.3f);
            if (GUI.Button(new Rect(left, top, buttonWidth, buttonHeight), Localization.GetLocalizedString("%Back"), this.GetGuiStyle(GUIStyleCategory.Dark)))
            {
                Application.Instance.BackToLastScreen();
            }
        }
    }

    private void ProfileCreationWindow(int windowId)
    {
        float windowsWidth = this.GetLenght(this.windowWidth);
        float windowsHeight = this.GetLenght(this.windowHeight);

        float buttonWidth = this.GetLenght(200f);
        float buttonHeight = this.GetLenght(35f);

        float left = this.GetLenght(30f);
        float topMargin = this.GetLenght(100f);

        float secondColumnLeft = this.GetLenght(150f);
        float secondColumnWidth = this.GetLenght(210f);

        float top = topMargin;
        GUI.Label(new Rect(left, top, buttonWidth, buttonHeight), Localization.GetLocalizedString("%Pseudo"), this.GetGuiStyle(GUIStyleCategory.SmallText));
        this.newProfileName = GUI.TextField(new Rect(secondColumnLeft, top, secondColumnWidth, buttonHeight), this.newProfileName, this.GetGuiStyle(GUIStyleCategory.Dark));

        left = (windowsWidth / 2f) - (buttonWidth / 2f);
        top += 1.5f * buttonHeight * 1.3f;
        GUI.enabled = !string.IsNullOrEmpty(this.newProfileName);
        if (GUI.Button(new Rect(left, top, buttonWidth, buttonHeight), Localization.GetLocalizedString("%Create"), this.GetGuiStyle(GUIStyleCategory.Dark)))
        {
            Profile newProfile = ProfileManager.Instance.CreateNewProfile(this.newProfileName);
            ProfileManager.Instance.ChangeCurrentProfile(newProfile);
            
            Application.Instance.BackToLastScreen();
        }

        GUI.enabled = true;

        top += 1f * buttonHeight * 1.3f;
        if (GUI.Button(new Rect(left, top, buttonWidth, buttonHeight), Localization.GetLocalizedString("%Back"), this.GetGuiStyle(GUIStyleCategory.Dark)))
        {
            Application.Instance.BackToLastScreen();
        }
    }

    private void OnlineProfileConnectionWindow(int windowId)
    {
        float windowsWidth = this.GetLenght(this.windowWidth);
        float windowsHeight = this.GetLenght(this.windowHeight);

        float buttonWidth = this.GetLenght(200f);
        float buttonHeight = this.GetLenght(35f);

        float left = this.GetLenght(30f);
        float topMargin = this.GetLenght(70f);

        float secondColumnLeft = this.GetLenght(150f);
        float secondColumnWidth = this.GetLenght(210f);

        float top = topMargin;
        GUI.Label(new Rect(left, top, buttonWidth, buttonHeight), Localization.GetLocalizedString("%Pseudo"), this.GetGuiStyle(GUIStyleCategory.SmallText));
        GUI.Label(new Rect(secondColumnLeft, top, secondColumnWidth, buttonHeight), ProfileToConnect.Name, this.GetGuiStyle(GUIStyleCategory.SmallText));

        GUI.Label(new Rect(left, top + buttonHeight, buttonWidth, buttonHeight), Localization.GetLocalizedString("%Password"), this.GetGuiStyle(GUIStyleCategory.SmallText));
        this.password = GUI.TextField(new Rect(secondColumnLeft, top + buttonHeight, secondColumnWidth, buttonHeight), this.password, this.GetGuiStyle(GUIStyleCategory.Dark));

        left = (windowsWidth / 2f) - (buttonWidth / 2f);
        top += 2.5f * buttonHeight * 1.3f;
        bool isPasswordValid = NetworkManager.Instance.IsPasswordValid(this.password);
        GUI.enabled = isPasswordValid;
        if (GUI.Button(new Rect(left, top, buttonWidth, buttonHeight), Localization.GetLocalizedString("%Synchronize"), this.GetGuiStyle(GUIStyleCategory.Dark)))
        {
            NetworkManager.Instance.ConnectToProfile(ProfileToConnect, this.password);

            Application.Instance.BackToLastScreen();
        }

        GUI.enabled = true;

        top += 1f * buttonHeight * 1.3f;
        if (GUI.Button(new Rect(left, top, buttonWidth, buttonHeight), Localization.GetLocalizedString("%Back"), this.GetGuiStyle(GUIStyleCategory.Dark)))
        {
            Application.Instance.BackToLastScreen();
        }
    }

    private void OnlineProfileCreationWindow(int windowId)
    {
        float windowsWidth = this.GetLenght(this.windowWidth);
        float windowsHeight = this.GetLenght(this.windowHeight);

        float buttonWidth = this.GetLenght(200f);
        float buttonHeight = this.GetLenght(35f);

        float left = this.GetLenght(30f);
        float topMargin = this.GetLenght(50f);

        float secondColumnLeft = this.GetLenght(150f);
        float secondColumnWidth = this.GetLenght(210f);

        if (this.onlineCreateProfileName == null)
        {
            this.onlineCreateProfileName = ProfileToConnect.Name;
        }

        float top = topMargin;
        GUI.Label(new Rect(left, top, buttonWidth, buttonHeight), Localization.GetLocalizedString("%Pseudo"), this.GetGuiStyle(GUIStyleCategory.SmallText));
        this.onlineCreateProfileName = GUI.TextField(new Rect(secondColumnLeft, top, secondColumnWidth, buttonHeight), this.onlineCreateProfileName, this.GetGuiStyle(GUIStyleCategory.Dark));

        GUI.Label(new Rect(left, top + (1.5f * buttonHeight), buttonWidth, buttonHeight), Localization.GetLocalizedString("%Password"), this.GetGuiStyle(GUIStyleCategory.SmallText));
        this.password = GUI.TextField(new Rect(secondColumnLeft, top + (1.5f * buttonHeight), secondColumnWidth, buttonHeight), this.password, this.GetGuiStyle(GUIStyleCategory.Dark));

        GUI.Label(new Rect(left, top + (2.7f * buttonHeight), buttonWidth, buttonHeight), Localization.GetLocalizedString("%Confirm"), this.GetGuiStyle(GUIStyleCategory.SmallText));
        this.passwordConfirmation = GUI.TextField(new Rect(secondColumnLeft, top + (2.7f * buttonHeight), secondColumnWidth, buttonHeight), this.passwordConfirmation, this.GetGuiStyle(GUIStyleCategory.Dark));

        left = (windowsWidth / 2f) - (buttonWidth / 2f);
        top += 3.5f * buttonHeight * 1.3f;
        bool isPasswordValid = NetworkManager.Instance.IsPasswordValid(this.password);
        bool isPasswordsMatch = this.password == this.passwordConfirmation;
        GUI.enabled = isPasswordValid && isPasswordsMatch; 
        if (GUI.Button(new Rect(left, top, buttonWidth, buttonHeight), Localization.GetLocalizedString("%Create"), this.GetGuiStyle(GUIStyleCategory.Dark)))
        {
            NetworkManager.Instance.CreateProfile(ProfileToConnect, this.password);

            this.onlineCreateProfileName = null;
            Application.Instance.BackToLastScreen();
        }

        GUI.enabled = true;

        top += 1f * buttonHeight * 1.3f;
        if (GUI.Button(new Rect(left, top, buttonWidth, buttonHeight), Localization.GetLocalizedString("%Back"), this.GetGuiStyle(GUIStyleCategory.Dark)))
        {
            Application.Instance.BackToLastScreen();
        }
    }

    private void CongratulationWindow(int windowID)
    {
        float windowsWidth = this.GetLenght(this.windowWidth);
        float windowsHeight = this.GetLenght(this.windowHeight);

        float leftMargin = this.GetLenght(30f);
        float topMargin = this.GetLenght(50f);

        float scoreLabelWidth = this.GetLenght(90f);
        float scoreLabelHeight = this.GetLenght(40f);

        float labelWidth = this.GetLenght(140f);
        float labelHeight = this.GetLenght(60f);

        float buttonWidth = this.GetLenght(120f);
        float buttonHeight = this.GetLenght(42f);

        GameStatistics gameStatistics = Application.Instance.Game.Statistics;

        Profile currentProfile = ProfileManager.Instance.CurrentProfile;

        // Score.
        GUI.Label(new Rect(leftMargin, topMargin, scoreLabelWidth, scoreLabelHeight), Localization.GetLocalizedString("%Score"), GUIManager.Instance.GetGuiStyle(GUIStyleCategory.Text));
        GUI.Label(new Rect(leftMargin + scoreLabelWidth, topMargin, scoreLabelWidth, scoreLabelHeight), gameStatistics.Score.ToString(), GUIManager.Instance.GetGuiStyle(GUIStyleCategory.Text));

        // Profile.
        float top = topMargin + (scoreLabelHeight * 1.5f);
        GUI.Label(new Rect(leftMargin, top, labelWidth, labelHeight), Localization.GetLocalizedString("%ScoreRegisteringDescription"), GUIManager.Instance.GetGuiStyle(GUIStyleCategory.SmallText));

        string currentProfileName = currentProfile != null ? currentProfile.Name : Localization.GetLocalizedString("%NoProfileSelected");
        if (GUI.Button(new Rect(leftMargin + labelWidth, top, this.menuButtonSize, this.menuButtonSize), currentProfileName, this.GetGuiStyle(GUIStyleCategory.ProfileButtonLight)))
        {
            Application.Instance.PostScreenChange(Application.ApplicationScreen.Profile);
        }

        // Buttons.
        float step = windowsWidth / 3f;
        float left = step - (buttonWidth / 2f);
        top = windowsHeight - (1.5f * buttonHeight);
        if (GUI.Button(new Rect(left, top, buttonWidth, buttonHeight), Localization.GetLocalizedString("%Cancel"), GUIManager.Instance.GetGuiStyle(GUIStyleCategory.Dark)))
        {
            gameStatistics.Registered = true;
            Application.Instance.BackToScreen(Application.ApplicationScreen.Game);
        }

        GUI.enabled = !gameStatistics.Registered;
        if (GUI.Button(new Rect(left + step, top, buttonWidth, buttonHeight), Localization.GetLocalizedString("%Register"), GUIManager.Instance.GetGuiStyle(GUIStyleCategory.Dark)))
        {
            currentProfile.RegisterScore(gameStatistics);
            Application.Instance.BackToScreen(Application.ApplicationScreen.Game);
        }

        GUI.enabled = true;
    }
}
