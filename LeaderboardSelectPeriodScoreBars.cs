//This is script is used to select the wanted period in which the proper scores are shown.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LeaderboardSelectPeriodScoreBars : MonoBehaviour
{
    [Header("Used for changing the score bars")]
    public GameObject canvas;
    public GameObject content; //Used to access the children score bars.
    public GameObject positionScoreBar;
    public GameObject theListIsEmptyMessage;

    [Header("First Three Score Numbers Settings")]
    public Color firstPos, secondPos, thirdPos;
    bool firstThreeColorsFilled;


    private void Start()
    {
        theListIsEmptyMessage.SetActive(false);

        firstThreeColorsFilled = false;
    }
    public void SelectPeriodScoreBars(string buttonNamePressed) 
    {      
        //if (canvas.GetComponent<UIAnimationController>().previousSelectedButton.name != buttonNamePressed) 
        {
            DeleteContentScoreBars();

            theListIsEmptyMessage.SetActive(false);
            firstThreeColorsFilled = false;
            //--------------------------------------------Today Section--------------------------------------------
            if (canvas.GetComponent<UIAnimationController>().periodButtons[0].name == buttonNamePressed)
            {
                if (gameObject.GetComponent<Leaderboard>().todayLoaded && Leaderboard.todayScores.Count != 0) // If the new values are stored, then load them
                {
                    //gameObject.GetComponent<Leaderboard>().flag.text = "Today true";
                    for (int i = 0; i < Leaderboard.todayScores.Count; i++) // Puts the values of the today's period in the leaderboard
                    {

                        int k = i + 1; // Makes easier to refer to specific Scores.

                        ChangeFirstThreePositionNumbersColors(i);

                        positionScoreBar.transform.Find("Number of Position Text").GetComponent<TextMeshProUGUI>().text = k.ToString(); //Modifies the instantiated position number
                        positionScoreBar.transform.Find("Name Text").GetComponent<TextMeshProUGUI>().text = Leaderboard.todayScores[i].name; //Modifies the instantiated player's name
                        positionScoreBar.transform.Find("Score").GetComponent<TextMeshProUGUI>().text = Leaderboard.todayScores[i].score.ToString("n0"); //Modifies the instantiated player's score
                        positionScoreBar.transform.Find("Date").GetComponent<TextMeshProUGUI>().text = Leaderboard.todayScores[i].date; //Modifies the instantiated player's date

                        Instantiate(positionScoreBar, content.transform); // Instantiates the new score bar with the modified values
                    }
                }
                else if (gameObject.GetComponent<Leaderboard>().todayLoaded && Leaderboard.todayScores.Count == 0)
                {
                    theListIsEmptyMessage.SetActive(true);

                    for (int i = 0; i < PlayerPrefs.GetInt("playerPrefsTodayScore"); i++)
                    {
                        PlayerPrefs.DeleteKey(PlayerPrefs.GetString("TodayName" + i));
                        PlayerPrefs.DeleteKey(PlayerPrefs.GetInt("TodayScore" + i).ToString());
                        PlayerPrefs.DeleteKey(PlayerPrefs.GetString("TodayDate" + i));
                    }
                }
                else
                {
                    //gameObject.GetComponent<Leaderboard>().flag.text = "Today false";
                    for (int i = 0; i < PlayerPrefs.GetInt("playerPrefsTodayScore"); i++)
                    {
                        int k = i + 1; // Makes easier to refer to specific Scores.

                        ChangeFirstThreePositionNumbersColors(i);

                        positionScoreBar.transform.Find("Number of Position Text").GetComponent<TextMeshProUGUI>().text = k.ToString(); //Modifies the instantiated position number
                        positionScoreBar.transform.Find("Name Text").GetComponent<TextMeshProUGUI>().text = PlayerPrefs.GetString("TodayName" + i); //Modifies the instantiated player's name
                        positionScoreBar.transform.Find("Score").GetComponent<TextMeshProUGUI>().text = PlayerPrefs.GetInt("TodayScore" + i).ToString(); //Modifies the instantiated player's score
                        positionScoreBar.transform.Find("Date").GetComponent<TextMeshProUGUI>().text = PlayerPrefs.GetString("TodayDate" + i); //Modifies the instantiated player's date

                        Instantiate(positionScoreBar, content.transform); // Instantiates the new score bar with the modified values
                    }
                }
                

                firstThreeColorsFilled = true;
                return;
            }
            //--------------------------------------------Week Section--------------------------------------------
            if (canvas.GetComponent<UIAnimationController>().periodButtons[1].name == buttonNamePressed)
            {
                if (gameObject.GetComponent<Leaderboard>().weekLoaded && Leaderboard.weekScores.Count != 0)
                {
                    //gameObject.GetComponent<Leaderboard>().flag.text = "Week true";
                    for (int i = 0; i < Leaderboard.weekScores.Count; i++) // Puts the values of the today's period in the leaderboard
                    {
                        int k = i + 1; // Makes easier to refer to specific Scores.

                        ChangeFirstThreePositionNumbersColors(i);

                        positionScoreBar.transform.Find("Number of Position Text").GetComponent<TextMeshProUGUI>().text = k.ToString(); //Modifies the instantiated position number
                        positionScoreBar.transform.Find("Name Text").GetComponent<TextMeshProUGUI>().text = Leaderboard.weekScores[i].name; //Modifies the instantiated player's name
                        positionScoreBar.transform.Find("Score").GetComponent<TextMeshProUGUI>().text = Leaderboard.weekScores[i].score.ToString("n0"); //Modifies the instantiated player's score
                        positionScoreBar.transform.Find("Date").GetComponent<TextMeshProUGUI>().text = Leaderboard.weekScores[i].date; //Modifies the instantiated player's date

                        Instantiate(positionScoreBar, content.transform); // Instantiates the new score bar with the modified values
                    }
                }
                else if(gameObject.GetComponent<Leaderboard>().weekLoaded && Leaderboard.weekScores.Count == 0)
                {
                    theListIsEmptyMessage.SetActive(true);

                    for (int i = 0; i < PlayerPrefs.GetInt("playerPrefsWeekScore"); i++)
                    {
                        PlayerPrefs.DeleteKey(PlayerPrefs.GetString("WeekName" + i));
                        PlayerPrefs.DeleteKey(PlayerPrefs.GetInt("WeekScore" + i).ToString());
                        PlayerPrefs.DeleteKey(PlayerPrefs.GetString("WeekDate" + i));
                    }
                }
                else
                {
                   //gameObject.GetComponent<Leaderboard>().flag.text = "Week false";
                    for (int i = 0; i < PlayerPrefs.GetInt("playerPrefsWeekScore"); i++)
                    {
                        int k = i + 1; // Makes easier to refer to specific Scores.

                        ChangeFirstThreePositionNumbersColors(i);

                        positionScoreBar.transform.Find("Number of Position Text").GetComponent<TextMeshProUGUI>().text = k.ToString(); //Modifies the instantiated position number
                        positionScoreBar.transform.Find("Name Text").GetComponent<TextMeshProUGUI>().text = PlayerPrefs.GetString("WeekName" + i); //Modifies the instantiated player's name
                        positionScoreBar.transform.Find("Score").GetComponent<TextMeshProUGUI>().text = PlayerPrefs.GetInt("WeekScore" + i).ToString(); //Modifies the instantiated player's score
                        positionScoreBar.transform.Find("Date").GetComponent<TextMeshProUGUI>().text = PlayerPrefs.GetString("WeekDate" + i); //Modifies the instantiated player's date

                        Instantiate(positionScoreBar, content.transform); // Instantiates the new score bar with the modified values
                    }
                }
                

                firstThreeColorsFilled = true; 
                return;
            }
            //--------------------------------------------All Time Section--------------------------------------------
            if (canvas.GetComponent<UIAnimationController>().periodButtons[2].name == buttonNamePressed)
            {
                if (gameObject.GetComponent<Leaderboard>().allTimeLoaded && Leaderboard.allTimeScores.Count != 0)
                {
                    //gameObject.GetComponent<Leaderboard>().flag.text = "All Time true";
                    for (int i = 0; i < Leaderboard.allTimeScores.Count; i++) // Puts the values of the today's period in the leaderboard
                    {
                        int k = i + 1; // Makes easier to refer to specific Scores.

                        ChangeFirstThreePositionNumbersColors(i);

                        positionScoreBar.transform.Find("Number of Position Text").GetComponent<TextMeshProUGUI>().text = k.ToString(); //Modifies the instantiated position number
                        positionScoreBar.transform.Find("Name Text").GetComponent<TextMeshProUGUI>().text = Leaderboard.allTimeScores[i].name; //Modifies the instantiated player's name
                        positionScoreBar.transform.Find("Score").GetComponent<TextMeshProUGUI>().text = Leaderboard.allTimeScores[i].score.ToString("n0"); //Modifies the instantiated player's score
                        positionScoreBar.transform.Find("Date").GetComponent<TextMeshProUGUI>().text = Leaderboard.allTimeScores[i].date; //Modifies the instantiated player's date

                        Instantiate(positionScoreBar, content.transform); // Instantiates the new score bar with the modified values
                    }
                }
                else if (gameObject.GetComponent<Leaderboard>().allTimeLoaded && Leaderboard.allTimeScores.Count == 0) //If there are no results
                {
                    theListIsEmptyMessage.SetActive(true);

                    for (int i = 0; i < PlayerPrefs.GetInt("playerPrefsAllTimeScore"); i++) //Delete all the playerPrefs
                    {
                        PlayerPrefs.DeleteKey(PlayerPrefs.GetString("AllTimeName" + i));
                        PlayerPrefs.DeleteKey(PlayerPrefs.GetInt("AllTimeScore" + i).ToString());
                        PlayerPrefs.DeleteKey(PlayerPrefs.GetString("AllTimeDate" + i));
                    }
                }
                else //Else show the PlayerPrefs
                {
                    //gameObject.GetComponent<Leaderboard>().flag.text = "All Time false";
                    for (int i = 0; i < PlayerPrefs.GetInt("playerPrefsAllTimeScore"); i++)
                    {
                        int k = i + 1; // Makes easier to refer to specific Scores.

                        ChangeFirstThreePositionNumbersColors(i);

                        positionScoreBar.transform.Find("Number of Position Text").GetComponent<TextMeshProUGUI>().text = k.ToString(); //Modifies the instantiated position number
                        positionScoreBar.transform.Find("Name Text").GetComponent<TextMeshProUGUI>().text = PlayerPrefs.GetString("AllTimeName" + i); //Modifies the instantiated player's name
                        positionScoreBar.transform.Find("Score").GetComponent<TextMeshProUGUI>().text = PlayerPrefs.GetInt("AllTimeScore" + i).ToString(); //Modifies the instantiated player's score
                        positionScoreBar.transform.Find("Date").GetComponent<TextMeshProUGUI>().text = PlayerPrefs.GetString("AllTimeDate" + i); //Modifies the instantiated player's date

                        Instantiate(positionScoreBar, content.transform); // Instantiates the new score bar with the modified values
                    }
                }
                
                firstThreeColorsFilled = true;
                return;
            }
           
        }

    }

    public void DeleteContentScoreBars()
    {
        // First destroy the existing score bars and then add the today scores.
        if (content.transform.childCount > 0) //if the content has children, delete them.
        {
            foreach (RectTransform child in content.transform)
            {
                Destroy(child.gameObject);
            }
        }
    }

    public void ChangeFirstThreePositionNumbersColors(int i)
    {
        firstThreeColorsFilled = false;

        if (i == 0 && !firstThreeColorsFilled)
        {
            positionScoreBar.transform.Find("Number of Position Text").GetComponent<TextMeshProUGUI>().color = firstPos;
        }
        else if (i == 1)
        {
            positionScoreBar.transform.Find("Number of Position Text").GetComponent<TextMeshProUGUI>().color = secondPos;
        }
        else if (i == 2)
        {
            positionScoreBar.transform.Find("Number of Position Text").GetComponent<TextMeshProUGUI>().color = thirdPos;
            firstThreeColorsFilled = true;
        }
        else
        {
            positionScoreBar.transform.Find("Number of Position Text").GetComponent<TextMeshProUGUI>().color = Color.white;
        }
    }

}
