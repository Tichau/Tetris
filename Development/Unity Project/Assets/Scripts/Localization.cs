using System.Collections.Generic;

public static class Localization
{
    private static Dictionary<string, string> localizedString = new Dictionary<string, string>();
    static Localization()
    {
        localizedString.Add("%PauseTitle", "PAUSE");
        localizedString.Add("%StartGameInfo", "Press Enter to start a new game");
        localizedString.Add("%Level", "Level");
        localizedString.Add("%Lines", "Lines");
        localizedString.Add("%HighScores", "High Scores");
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
