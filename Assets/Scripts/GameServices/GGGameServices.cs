using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_ANDROID
using GooglePlayGames;
using GooglePlayGames.OurUtils;
using GooglePlayGames.BasicApi;
#endif

public class GGGameServices : MonoBehaviour
{
#if UNITY_IOS
    private void Start() {
        Destroy(gameObject);
    }
#endif
#if UNITY_ANDROID
    public static GGGameServices Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null) { Instance = this; }
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
           //.RequestServerAuthCode(false /* Don't force refresh */)
           //.RequestIdToken()
           .Build();
        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();
    }
    private void OnDestroy()
    {
        Instance = null;
    }
    // Start is called before the first frame update
    private void Start()
    {

        StartCoroutine(DelayToAuth());
    }
    private IEnumerator DelayToAuth()
    {
        yield return new WaitForEndOfFrame();
        PlayGamesPlatform.Instance.Authenticate(SignInInteractivity.CanPromptOnce, ProcessAuthentication);
    }

    public void ProcessAuthentication(SignInStatus status)
    {
        if (status == SignInStatus.Success)
        {
            // Continue with Play Games Services
            Debug.Log("Logging ok");
        }
        else
        {
            Debug.Log($"Loging failed: {status}");
        }
    }

    public void PostScoreToHighestScore(int score, Action<bool> success)
    {
        PlayGamesPlatform.Instance.ReportScore(score, GPGSIds.leaderboard_highest_score, success);
    }
    public void ShowHighestLeaderboard()
    {
        PlayGamesPlatform.Instance.ShowLeaderboardUI(GPGSIds.leaderboard_highest_score);
    }

#endif
}