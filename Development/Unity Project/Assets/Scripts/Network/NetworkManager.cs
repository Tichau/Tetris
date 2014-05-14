// <copyright file="NetworkManager.cs" company="BlobTeam">Copyright BlobTeam. All rights reserved.</copyright>

using System.Text;
using System.Text.RegularExpressions;

using UnityEngine;

public partial class NetworkManager : MonoBehaviour
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

    public bool IsProfileNameValid(string profileName)
    {
        // TODO: better check.
        if (profileName.ToLower() == "anonymous")
        {
            return false;
        }

        Regex myRegex = new Regex("^[a-zA-Z0-9_-]{3,16}$");
        return myRegex.IsMatch(profileName);
    }

    public void CreateProfile(Profile profile, string profileName, string password)
    {
        if (profile.IsSynchronized)
        {
            Debug.LogError("The profile " + profile.Name + " don't need to be synchronized.");
        }

        if (profile.ID >= 0)
        {
            Debug.LogError("The profile " + profile.Name + " has already been created.");
        }

        this.StartCoroutine(this.CreateProfileRequest(profile, profileName, password));
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
