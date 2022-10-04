using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

#if UNITY_IOS || UNITY_IPHONE
using UnityEngine.SocialPlatforms.GameCenter;
#endif

public class GameCenterServices : MonoBehaviour
{
#if UNITY_ANDROID
    private void Start()
    {
        Destroy(gameObject);
    }
#endif
#if UNITY_IOS || UNITY_IPHONE
    public static GameCenterServices Instance { get; private set; }
    private ILeaderboard leaderboard;
    private void Awake()
    {
        if (Instance == null) { Instance = this; }
    }
    private void OnDestroy()
    {
        Instance = null;
    }
    private void Start()
    {
        StartCoroutine(DelayToAuth());
    }
    private IEnumerator DelayToAuth()
    {
        yield return new WaitForEndOfFrame();
        Social.localUser.Authenticate(ProcessAuthentication);
        CreateLeaderboard();
    }

    private void CreateLeaderboard()
    {
        leaderboard = Social.CreateLeaderboard();
        leaderboard.id = AGCIds.leaderboard_highest_score;
        leaderboard.LoadScores(OnBeaverboardScoreLoaded);
    }

    private void OnBeaverboardScoreLoaded(bool result)
    {
        if (result)
        {
            Debug.Log("Received " + leaderboard.scores.Length + " scores");
            foreach (IScore score in leaderboard.scores)
            {
                Debug.Log(score);
            }
        }
        else
        {
            Debug.Log("leaderboard score load fail");
        }
    }

    private void ProcessAuthentication(bool success)
    {
        Debug.Log($"logged in: {success}");
        if (success)
        {
            Social.LoadScores(AGCIds.leaderboard_highest_score, scores =>
            {
                Debug.Log("Received " + scores.Length + " scores");
                foreach (IScore score in scores)
                {
                    Debug.Log(score);
                }
            });
        }
    }
    public void PostScoreToHighestScore(int score, Action<bool> success)
    {
        Social.ReportScore(score, AGCIds.leaderboard_highest_score, success);
    }
    public void ShowHighestLeaderboard()
    {
        Social.ShowLeaderboardUI();
    }
#endif
}

