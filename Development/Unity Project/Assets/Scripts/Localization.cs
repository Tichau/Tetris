using System.Collections.Generic;

public static class Localization
{
    private static Language currentLanguage;
    private static Dictionary<string, string> localizedString = new Dictionary<string, string>();

    public enum Language
    {
        English,
        French,
    }

    static Localization()
    {
        CurrentLanguage = Language.English;
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
                localizedString.Add("%HighScores", "High Scores");
                localizedString.Add("%Continue", "Continue");
                localizedString.Add("%EnterYourName", "Enter you're name:");
                localizedString.Add("%Register", "Register");
                localizedString.Add("%Congratulation", "Congratulations you're in the high scores !");
                localizedString.Add("%Player", "Player");
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
                break;
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
}
