// <copyright file="NetworkManager.cs" company="BlobTeam">Copyright BlobTeam. All rights reserved.</copyright>

using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour
{
    [UnityEngine.SerializeField]
    private string secretKey = "12345";

    [UnityEngine.SerializeField]
    private string profileManagementUrl;

    [UnityEngine.SerializeField]
    private string scoreManagementUrl;

    private enum ProfileManagementOperation
    {
        CreateProfile,
        IsProfileExist,
        GetProfile,
    }

    private enum ScoreManagementOperation
    {
        SyncProfile,
        GetHighscores,
    }

    public static NetworkManager Instance
    {
        get;
        private set;
    }

    public bool IsPasswordValid(string password)
    {
        Regex myRegex = new Regex("^[a-zA-Z0-9_-]{6,18}$");
        return myRegex.IsMatch(password);
    }

    public void CreateProfile(Profile profile, string password)
    {
        if (profile.IsSynchronized)
        {
            Debug.LogError("The profile " + profile.Name + " don't need to be synchronized.");
        }

        if (profile.ID >= 0)
        {
            Debug.LogError("The profile " + profile.Name + " has already been created.");
        }

        this.StartCoroutine(this.CreateProfileRequest(profile, password));
    }

    public void ConnectToProfile(Profile profileToConnect, string password)
    {
        this.StartCoroutine(this.ConnectToProfileRequest(profileToConnect, password));
    }

    public void IsProfileExist(Profile profile, System.Action<Profile, bool> resultDelegate)
    {
        this.StartCoroutine(this.IsProfileExistRequest(profile, resultDelegate));
    }

    public void SynchronizeProfile(Profile profile)
    {
        if (profile.IsSynchronized)
        {
            Debug.Log("The profile " + profile.Name + " don't need to be synchronized.");
            return;
        }

        if (profile.ID < 0)
        {
            Debug.LogError("The profile " + profile.Name + " doesn't exist.");
        }

        if (profile.UnsynchronizedScores.Count == 0)
        {
            Debug.LogError("The profile " + profile.Name + " is already synchronized.");
        }

        this.StartCoroutine(this.RegisterScoresRequest(profile));
    }

    public void RefreshHighscores(int scoreCount)
    {
        this.StartCoroutine(this.GetHighsoresRequest(null, scoreCount));
    }

    private IEnumerator ConnectToProfileRequest(Profile profile, string password)
    {
        if (!this.IsPasswordValid(password))
        {
            Debug.LogError("Invalid password.");
            yield break;
        }

        Debug.Log("Start ConnectToProfile request.");

        string operation = ProfileManagementOperation.GetProfile.ToString();
        string profileName = profile.Name.ToLower();
        
        password = this.Md5Sum(password);

        string hash = this.Md5Sum(operation + profileName + password + this.secretKey).ToLower();

        WWWForm form = new WWWForm();
        form.AddField("operation", operation);
        form.AddField("profileName", profileName);
        form.AddField("password", password);
        form.AddField("hash", hash);

        WWW www = new WWW(this.profileManagementUrl, form);

        // Highscore state: Wait.
        yield return www;

        string[] results = www.text.Split('|');

        if (results.Length > 1 && results[0] == "Success")
        {
            int profileId = -1;
            if (int.TryParse(results[1], out profileId))
            {
                ((IProfileManagement)profile).SetId(profileId);
                ((IProfileManagement)profile).SetPassword(password);
            }
            else
            {
                Debug.LogError("Can't parse id: " + results[1]);
            }
        }
        else
        {
            // Highscore state: Error.
            Debug.LogError("There was an error during the profile creation request: " + www.error);

            if (results.Length > 0)
            {
                for (int index = 0; index < results.Length; ++index)
                {
                    Debug.LogError("HTML error " + index + ": " + results[index]);
                }
            }
        }
    }

    private IEnumerator IsProfileExistRequest(Profile profile, System.Action<Profile, bool> resultDelegate)
    {
        Debug.Log("Start IsProfileExist request.");

        string operation = ProfileManagementOperation.IsProfileExist.ToString();
        string profileName = profile.Name.ToLower();
        
        string hash = this.Md5Sum(operation + profileName + this.secretKey).ToLower();

        WWWForm form = new WWWForm();
        form.AddField("operation", operation);
        form.AddField("profileName", profileName);
        form.AddField("password", string.Empty);
        form.AddField("hash", hash);

        WWW www = new WWW(this.profileManagementUrl, form);

        // Highscore state: Wait.
        yield return www;

        string[] results = www.text.Split('|');

        if (results.Length > 1 && results[0] == "Success")
        {
            bool isProfileExist = false;
            if (bool.TryParse(results[1], out isProfileExist))
            {
                resultDelegate.Invoke(profile, isProfileExist);
            }
            else
            {
                Debug.LogError("Can't parse result: " + results[1]);
            }
        }
        else
        {
            // Highscore state: Error.
            Debug.LogError("There was an error during the profile operation " + operation + " request: " + www.error);

            if (results.Length > 0)
            {
                for (int index = 0; index < results.Length; ++index)
                {
                    Debug.LogError("HTML error " + index + ": " + results[index]);
                }
            }
        }
    }

    private IEnumerator CreateProfileRequest(Profile profile, string password)
    {
        Debug.Log("Start CreateProfile request.");

        string operation = ProfileManagementOperation.CreateProfile.ToString();
        string profileName = profile.Name.ToLower();

        password = this.Md5Sum(password);

        string hash = this.Md5Sum(operation + profileName + password + this.secretKey).ToLower();

        WWWForm form = new WWWForm();
        form.AddField("operation", operation);
        form.AddField("profileName", profileName);
        form.AddField("password", password);
        form.AddField("hash", hash);

        WWW www = new WWW(this.profileManagementUrl, form);

        // Highscore state: Wait.
        yield return www;

        string[] results = www.text.Split('|');

        if (results.Length > 1 && results[0] == "Success")
        {
            int profileId = -1;
            if (int.TryParse(results[1], out profileId))
            {
                ((IProfileManagement)profile).SetId(profileId);
                ((IProfileManagement)profile).SetPassword(password);
            }
            else
            {
                Debug.LogError("Can't parse id: " + results[1]);
            }
        }
        else
        {
            // Highscore state: Error.
            Debug.LogError("There was an error during the profile creation request: " + www.error);

            if (results.Length > 0)
            {
                for (int index = 0; index < results.Length; ++index)
                {
                    Debug.LogError("HTML error " + index + ": " + results[index]);
                }
            }
        }
    }

    private IEnumerator RegisterScoresRequest(Profile profile)
    {
        Debug.Log("Start RegisterScores request.");

        string operation = ScoreManagementOperation.SyncProfile.ToString();
        string profileId = profile.ID.ToString();
        string password = profile.Password;

        if (string.IsNullOrEmpty(password))
        {
            Debug.LogError("No password stored in the profile.");
            yield break;
        }

        StringBuilder stringBuilder = new StringBuilder();
        for (int index = 0; index < profile.UnsynchronizedScores.Count; index++)
        {
            GameStatistics gameStatistics = profile.UnsynchronizedScores[index];
            gameStatistics.Serialize(stringBuilder);
            stringBuilder.Append("|");
        }

        string content = stringBuilder.ToString();

        string hash = this.Md5Sum(operation + profileId + password + content + this.secretKey).ToLower();

        WWWForm form = new WWWForm();
        form.AddField("operation", operation);
        form.AddField("profileId", profileId);
        form.AddField("password", password);
        form.AddField("content", content);
        form.AddField("hash", hash);

        WWW www = new WWW(this.scoreManagementUrl, form);

        // Highscore state: Wait.
        yield return www;

        string[] results = www.text.Split('|');

        if (results.Length > 0 && results[0] == "Success")
        {
            ((IProfileManagement)profile).ClearUnsynchronizedScores();
        }
        else
        {
            // Highscore state: Error.
            Debug.LogError("There was an error during the profile creation request: " + www.error);

            if (results.Length > 0)
            {
                for (int index = 0; index < results.Length; ++index)
                {
                    Debug.LogError("HTML error " + index + ": " + results[index]);
                }
            }
        }
    }

    private IEnumerator GetHighsoresRequest(Profile profile, int scoreCount)
    {
        if (scoreCount <= 0)
        {
            throw new ArgumentOutOfRangeException("scoreCount");
        }

        Debug.Log("Start GetHighsores request.");

        string operation = ScoreManagementOperation.GetHighscores.ToString();
        int id = profile != null ? profile.ID : -1;
        string profileId = id.ToString();
        string password = string.Empty;
        string content = string.Format("Count={0}", scoreCount);

        string hash = this.Md5Sum(operation + profileId + password + content + this.secretKey).ToLower();

        WWWForm form = new WWWForm();
        form.AddField("operation", operation);
        form.AddField("profileId", profileId);
        form.AddField("password", password);
        form.AddField("content", content);
        form.AddField("hash", hash);

        WWW www = new WWW(this.scoreManagementUrl, form);

        // Highscore state: Wait.
        yield return www;

        string[] results = www.text.Split('|');

        if (results.Length > 1 && results[0] == "Success")
        {
            List<NamedGameStatistics> requestResult = new List<NamedGameStatistics>(results.Length - 1);
            
            // Parse request result.
            for (int index = 1; index < results.Length; index++)
            {
                string profileName = string.Empty;
                int score = 0;
                int lines = 0;
                int startLevel = 0;

                // Parse infos.
                string[] scoreInfos = results[index].Split(';');
                for (int infoIndex = 0; infoIndex < scoreInfos.Length; infoIndex++)
                {
                    string scoreInfo = scoreInfos[infoIndex];
                    if (scoreInfo.StartsWith("ProfileName="))
                    {
                        profileName = scoreInfo.Substring(12);
                        continue;
                    }

                    if (scoreInfo.StartsWith("Score="))
                    {
                        if (!int.TryParse(scoreInfo.Substring(6), out score))
                        {
                            Debug.LogError("Can't deserialize score infos " + results[index]);
                            break;
                        }

                        continue;
                    }

                    if (scoreInfo.StartsWith("Lines="))
                    {
                        if (!int.TryParse(scoreInfo.Substring(6), out lines))
                        {
                            Debug.LogError("Can't deserialize score infos " + results[index]);
                            break;
                        }

                        continue;
                    }

                    if (scoreInfo.StartsWith("StartLevel="))
                    {
                        if (!int.TryParse(scoreInfo.Substring(11), out startLevel))
                        {
                            Debug.LogError("Can't deserialize score infos " + results[index]);
                            break;
                        }

                        continue;
                    }
                }

                if (string.IsNullOrEmpty(profileName) || score <= 0 || lines <= 0 || startLevel < 0 || startLevel > 20)
                {
                    // Invalid score.
                    Debug.LogError("Invalid score infos " + results[index]);
                    continue;
                }

                requestResult.Add(new NamedGameStatistics(profileName, score, lines, startLevel));
            }

            HighScoresManager.Instance.RegisterScores(requestResult);
        }
        else
        {
            // Highscore state: Error.
            Debug.LogError("There was an error during the get highscores request: " + www.error);

            if (results.Length > 0)
            {
                for (int index = 0; index < results.Length; ++index)
                {
                    Debug.LogError("HTML error " + index + ": " + results[index]);
                }
            }
        }
    }

    private string Md5Sum(string input)
    {
        System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
        byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
        byte[] hash = md5.ComputeHash(inputBytes);

        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < hash.Length; i++)
        {
            sb.Append(hash[i].ToString("X2"));
        }

        return sb.ToString();
    }

    private void Awake()
    {
        Instance = this;
    }
}
