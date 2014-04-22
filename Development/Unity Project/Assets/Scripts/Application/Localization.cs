// <copyright file="Localization.cs" company="1WeekEndStudio">Copyright 1WeekEndStudio. All rights reserved.</copyright>

using System.Collections.Generic;

public static class Localization
{
    private static Language currentLanguage;
    private static Dictionary<string, string> localizedString = new Dictionary<string, string>();

    static Localization()
    {
        CurrentLanguage = Language.English;
    }

    public enum Language
    {
        English,
        French,
    }

    public static Language CurrentLanguage
    {
        get
        {
            return currentLanguage;
        }

        set
        {
            currentLanguage = value;
            RefreshLocalization();
        }
    }

    public static string GetLocalizedString(string key)
    {
        if (!localizedString.ContainsKey(key))
        {
            return key;
        }

        return localizedString[key];
    }

    private static void RefreshLocalization()
    {
        localizedString.Clear();

        switch (CurrentLanguage)
        {
            case Language.English:
                localizedString.Add("%PauseTitle", "PAUSE");
                localizedString.Add("%StartGameInfo", "Press \"Enter\" to start a new game");
                localizedString.Add("%Level", "Level");
                localizedString.Add("%Lines", "Lines");
                localizedString.Add("%Score", "Score");
                localizedString.Add("%HighScores", "High Scores");
                localizedString.Add("%Continue", "Continue");
                localizedString.Add("%EnterYourName", "Enter your name:");
                localizedString.Add("%Register", "Register");
                localizedString.Add("%Congratulation", "Congratulations you're in the high scores !");
                localizedString.Add("%Player", "Player");
                localizedString.Add("%ProfilesWindowTitle", "Choose profile");
                localizedString.Add("%Back", "Back");
                localizedString.Add("%NewProfile", "New Profile");
                localizedString.Add("%ProfileCreationWindowTitle", "Profile creation");
                localizedString.Add("%Pseudo", "Pseudo");
                localizedString.Add("%NoProfileSelected", "No profile selected");
                localizedString.Add("%Create", "Create");
                localizedString.Add("%AnonymousProfileName", "Anonymous");
                localizedString.Add("%GameOver", "Game Over");
                localizedString.Add("%ScoreRegisteringDescription", "Register for");
                localizedString.Add("%Cancel", "Cancel");
                break;

            case Language.French:
                localizedString.Add("%PauseTitle", "PAUSE");
                localizedString.Add("%StartGameInfo", "Appuyez sur la touche \"Entrée\" pour commencer");
                localizedString.Add("%Level", "Niveau");
                localizedString.Add("%Lines", "Lignes");
                localizedString.Add("%HighScores", "Meilleurs scores");
                localizedString.Add("%Continue", "Continuer");
                localizedString.Add("%EnterYourName", "Entrez votre nom:");
                localizedString.Add("%Register", "Valider");
                localizedString.Add("%Congratulation", "Félicitations vous êtes dans les meilleurs scores !");
                localizedString.Add("%Player", "Joueur");
                localizedString.Add("%ProfilesWindowTitle", "Choix du profil");
                localizedString.Add("%Back", "Retour");
                localizedString.Add("%NewProfile", "Nouveau Profile");
                localizedString.Add("%ProfileCreationWindowTitle", "Création de Profile");
                localizedString.Add("%Pseudo", "Pseudo");
                localizedString.Add("%NoProfileSelected", "Profile non sélectionné");
                localizedString.Add("%Create", "Créer");
                localizedString.Add("%AnonymousProfileName", "Anonyme");
                localizedString.Add("%GameOver", "Game Over");
                break;
        }

        // Plateform specific localization.
#if UNITY_ANDROID
        localizedString["%StartGameInfo"] = "Touch here to start a new game";
#endif
    }
}
