//This script is used to store the data of the players and compares them to set them to the appropriate position

using System.Collections;
using System.Collections.Generic;
using UnityEngine.SocialPlatforms;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.UI;
using UnityEngine;
using System;
using TMPro;

public class Leaderboard : MonoBehaviour
{

    public static List<Scores> todayScores = new List<Scores>();
    public static List<Scores> weekScores = new List<Scores>();
    public static List<Scores> allTimeScores = new List<Scores>();

    public static List<string> todayUsersIds = new List<string>();
    public static List<string> weekUsersIds = new List<string>();
    public static List<string> allTimeUsersIds = new List<string>();

    public GameObject canvas, refreshButton, loading;

    public GameObject[] loadingDots;
    public Color greyColor;

    private int playerPrefsTodayScore, playerPrefsWeekScore, playerPrefsAllTimeScore;
    public bool todayLoaded, weekLoaded, allTimeLoaded;

    public GameObject GPGSManager;

    private void Awake()
    {
        todayLoaded = false;
        weekLoaded = false;
        allTimeLoaded = false;

        FillLeaderboardListsWithPlayerPrefs();
    }
    public void GetPublicTodayLeaderboardValuesFromGoogle()
    {
        refreshButton.SetActive(false);
        loading.GetComponent<Animator>().Play("Start Loading");

        todayLoaded = false;
        weekLoaded = false;
        allTimeLoaded = false;

        todayUsersIds.Clear();
        weekUsersIds.Clear();
        allTimeUsersIds.Clear();

        todayScores.Clear();
        weekScores.Clear();
        allTimeScores.Clear();

        //Load Daily Scores
        PlayGamesPlatform.Instance.LoadScores(GPGSIds.leaderboard_leaderboard, LeaderboardStart.TopScores, 30, LeaderboardCollection.Public, LeaderboardTimeSpan.Daily, (data) =>
        {
            for (int i = 0; i < data.Scores.Length; i++)
            {
                Scores temp = new Scores();

                todayUsersIds.Add(data.Scores[i].userID); //Add the userID in a list
                temp.score = data.Scores[i].value;
                temp.date = data.Scores[i].date.ToString("dd/MM/yyyy");

                todayScores.Add(temp);

                GetUserNames(todayScores, i);
            }
            if (data.Scores.Length == 0)
            {
                todayLoaded = true;
                ScoresBarsSpawn();
                GetPublicWeekLeaderboardValuesFromGoogle();
            }
        });

    }
    void GetPublicWeekLeaderboardValuesFromGoogle()
    {
        PlayGamesPlatform.Instance.LoadScores(GPGSIds.leaderboard_leaderboard, LeaderboardStart.TopScores, 30, LeaderboardCollection.Public, LeaderboardTimeSpan.Weekly, (data) =>
        {
            for (int i = 0; i < data.Scores.Length; i++)
            {
                Scores temp = new Scores();

                weekUsersIds.Add(data.Scores[i].userID); //Add the userID in a list
                temp.score = data.Scores[i].value;
                temp.date = data.Scores[i].date.ToString("dd/MM/yyyy");

                weekScores.Add(temp);

                GetUserNames(weekScores, i);
            }

            if (data.Scores.Length == 0)
            {
                weekLoaded = true;
                ScoresBarsSpawn();
                GetPublicAllTimeLeaderboardValuesFromGoogle();
            }
        });
    }
    void GetPublicAllTimeLeaderboardValuesFromGoogle()
    {
        PlayGamesPlatform.Instance.LoadScores(GPGSIds.leaderboard_leaderboard, LeaderboardStart.TopScores, 30, LeaderboardCollection.Public, LeaderboardTimeSpan.AllTime, (data) =>
        {
            for (int i = 0; i < data.Scores.Length; i++)
            {
                Scores temp = new Scores();

                allTimeUsersIds.Add(data.Scores[i].userID); //Add the userID in a list
                temp.score = data.Scores[i].value;
                temp.date = data.Scores[i].date.ToString("dd/MM/yyyy");

                allTimeScores.Add(temp);

                GetUserNames(allTimeScores, i);
            }

            if (data.Scores.Length == 0)
            {
                allTimeLoaded = true;
                ScoresBarsSpawn();
            }
        });
    }

    void GetUserNames(List<Scores> periodList, int i)
    {
        loading.SetActive(true);

        if (periodList == todayScores)
        {
            Social.LoadUsers(todayUsersIds.ToArray(), (userNames) =>
            {
                periodList[i].name = userNames[i].userName;

                if (userNames[periodList.Count - 1].userName != null)
                {
                    todayLoaded = true;
                    //CheckIfAllUserNamesWereLoaded(periodList);
                    ScoresBarsSpawn();
                    SaveScoresInPlayerPrefs(todayScores);
                    GetPublicWeekLeaderboardValuesFromGoogle();
                }
            });
        }
        //Find Week Names
        else if (periodList == weekScores)
        {
            Social.LoadUsers(weekUsersIds.ToArray(), (userNames) =>
            {
                periodList[i].name = userNames[i].userName;

                if (userNames[periodList.Count - 1].userName != null)
                {
                    weekLoaded = true;
                    //CheckIfAllUserNamesWereLoaded(periodList);
                    SaveScoresInPlayerPrefs(weekScores);
                    GetPublicAllTimeLeaderboardValuesFromGoogle();
                    ScoresBarsSpawn();
                }
            });
        }//Find All Time Names
        else if (periodList == allTimeScores)
        {
            Social.LoadUsers(allTimeUsersIds.ToArray(), (userNames) =>
            {
                periodList[i].name = userNames[i].userName;

                if (userNames[periodList.Count - 1].userName != null)
                {
                    allTimeLoaded = true;
                    //CheckIfAllUserNamesWereLoaded(periodList);
                    ScoresBarsSpawn();
                    SaveScoresInPlayerPrefs(allTimeScores);
                }
            });
        }
    }
    void SaveScoresInPlayerPrefs(List<Scores> period)
    {
        if (period == todayScores)
        {
            playerPrefsTodayScore = 0;

            for (int i = 0; i < todayScores.Count; i++)
            {
                PlayerPrefs.SetString("TodayName" + i, todayScores[i].name);
                PlayerPrefs.SetInt("TodayScore" + i, Convert.ToInt32(todayScores[i].score));
                PlayerPrefs.SetString("TodayDate" + i, todayScores[i].date);
                playerPrefsTodayScore++;
            }
            PlayerPrefs.SetInt("playerPrefsTodayScore", playerPrefsTodayScore);
        }
        else if (period == weekScores)
        {
            playerPrefsWeekScore = 0;

            for (int i = 0; i < weekScores.Count; i++)
            {
                PlayerPrefs.SetString("WeekName" + i, weekScores[i].name);
                PlayerPrefs.SetInt("WeekScore" + i, Convert.ToInt32(weekScores[i].score));
                PlayerPrefs.SetString("WeekDate" + i, weekScores[i].date);
                playerPrefsWeekScore++;
            }
            PlayerPrefs.SetInt("playerPrefsWeekScore", playerPrefsWeekScore);
        }
        else if (period == allTimeScores)
        {
            playerPrefsAllTimeScore = 0;

            for (int i = 0; i < allTimeScores.Count; i++)
            {
                PlayerPrefs.SetString("AllTimeName" + i, allTimeScores[i].name);
                PlayerPrefs.SetInt("AllTimeScore" + i, Convert.ToInt32(allTimeScores[i].score));
                PlayerPrefs.SetString("AllTimeDate" + i, allTimeScores[i].date);
                playerPrefsAllTimeScore++;
            }

            PlayerPrefs.SetInt("playerPrefsAllTimeScore", playerPrefsAllTimeScore);
        }
    }
    void FillLeaderboardListsWithPlayerPrefs() //Used to show the old leaderboard values until the new one comes off.
    {
        todayScores.Clear();
        weekScores.Clear();
        allTimeScores.Clear();

        gameObject.GetComponent<LeaderboardSelectPeriodScoreBars>().SelectPeriodScoreBars(canvas.GetComponent<UIAnimationController>().periodButtons[0].name);
    }
    void ScoresBarsSpawn()
    {
        if (canvas.GetComponent<UIAnimationController>().periodButtons[0].tag == "SelectedButton")
        {
            gameObject.GetComponent<LeaderboardSelectPeriodScoreBars>().SelectPeriodScoreBars(canvas.GetComponent<UIAnimationController>().periodButtons[0].name);
        }
        else if (canvas.GetComponent<UIAnimationController>().periodButtons[1].tag == "SelectedButton")
        {
            gameObject.GetComponent<LeaderboardSelectPeriodScoreBars>().SelectPeriodScoreBars(canvas.GetComponent<UIAnimationController>().periodButtons[1].name);
        }
        else if (canvas.GetComponent<UIAnimationController>().periodButtons[2].tag == "SelectedButton")
        {
            gameObject.GetComponent<LeaderboardSelectPeriodScoreBars>().SelectPeriodScoreBars(canvas.GetComponent<UIAnimationController>().periodButtons[2].name);
        }
        StopLoading();
    }
    void StopLoading()
    {
        if (allTimeLoaded)
        {
            loading.GetComponent<Animator>().Play("Stop Loading");
            refreshButton.SetActive(true);

            foreach (var loadingDot in loadingDots)
            {
                loadingDot.GetComponent<Image>().color = greyColor;
                loadingDot.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);

                loadingDot.GetComponent<RectTransform>().sizeDelta = new Vector2(20, 20); //Changes width and height of the dots
            }

            loadingDots[0].GetComponent<RectTransform>().anchoredPosition = new Vector2(-40, 0);
            loadingDots[2].GetComponent<RectTransform>().anchoredPosition = new Vector2(40, 0);
        }
    }

    public void CheckIfLeaderboardIsStillRefreshing() //Used to check if the loading animations should play when player opens the Leaderboard window
    {
        if (!refreshButton.activeInHierarchy) //If refreshing has stopped 
        {
            loading.GetComponent<Animator>().Play("Loading");
        }
    }
}
[System.Serializable]
public class Scores
{
    public string name;

    public long score;

    public string date;
}