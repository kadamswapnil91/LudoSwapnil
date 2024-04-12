using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WaitingForOpponentCountDownTimer : MonoBehaviour
{
    static string startTime = "0";
    private static bool isStartTime;
    WaitingForOpponent waitingForOpponent;
    [SerializeField] TextMeshProUGUI waitingTimer;
    [SerializeField] GameObject waitingTimerText, waitingForOpponentPanel, waittingOpponent;
    [SerializeField] Image fillImage1;
    APICall aPICall;
    string user;

    // Update is called once per frame
    void Update()
    {
        if (GameManager.gm.isStartCountDownTimer)
        {
            //if (time > 0)
            //{
            //    time -= Time.deltaTime;
            //    showCoundownTimer((int)time);
            //}
            //else
            //{
            //    time = 0;
            //    showCoundownTimer((int)time);

            //    if (user == Credentials.email)
            //        aPICall.startMatch();
            //}

            StartCoroutine(Timer());

        }

    }

    private void Awake()
    {
        aPICall = FindObjectOfType<APICall>();
        waitingForOpponent = FindObjectOfType<WaitingForOpponent>();
    }

    public void startTimer(string countDownStartValue, string userMail)
    {
        startTime = countDownStartValue;
        user = userMail;
    }

    void showCoundownTimer(int timerValue)
    {
        if (timerValue >= 0)
        {
            waitingTimer.text = timerValue.ToString();
            fillImage1.fillAmount = timerValue / 10f;
        }

    }

    IEnumerator Timer()
    {
        if (!Credentials.isAdminContest)
        {
            Credentials.adminContestStartTime = 10;
        }else
        {
            Credentials.adminContestStartTime = 10;
        }

        var secondsLeft = Credentials.adminContestStartTime + Utils.getRemainigSec(startTime, System.DateTime.UtcNow.ToString());
        showCoundownTimer(int.Parse(secondsLeft.ToString()));


        if (secondsLeft <= 0)
        {
            if (Credentials.isAdminContest)
            {
                GameManager.gm.isStartCountDownTimer = false;
                aPICall.startMatch();
                if (waitingForOpponentPanel.activeInHierarchy)
                    waitingForOpponent.publishStartGame("Admin");
            }
            else
            {
                if (user == Credentials.email)
                {
                    GameManager.gm.isStartCountDownTimer = false;
                    if (waitingForOpponentPanel.activeInHierarchy)
                        aPICall.startMatch();
                }
            }
            //else
            //{
            //    if (waitingForOpponentPanel.activeInHierarchy)
            //    {
            //        if (secondsLeft < -2 && secondsLeft > -19)
            //        {
            //            waittingOpponent.SetActive(true);
            //        }
            //        else if (secondsLeft < -20)
            //        {
            //            waittingOpponent.SetActive(false);
            //            aPICall.quitOpponentGameAPI();
            //            Manager.gm.isStartCountDownTimer = false;

            //        }
            //    }

            //}

        }
        yield return new WaitForEndOfFrame();
    }

}

