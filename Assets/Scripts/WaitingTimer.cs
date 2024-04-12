using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class WaitingTimer : MonoBehaviour
{
    float time = 0;
    private static int countDownStartValue;
    [SerializeField] TextMeshProUGUI[] waitingTimer;

    // Update is called once per frame
    void Update()
    {
        if (GameManager.gm.isGameRunning && GameManager.gm.isOnlineGame)
        {
            if (time > 0)
            {
                time -= Time.deltaTime;
                showCoundownTimer((int)time);
            }
            else
            {
                time = 0;
                showCoundownTimer((int)time);
            }
        }
    }

    public void startTimer(float countDownStartValue)
    {
        time = countDownStartValue;
    }

    void showCoundownTimer(int timerValue)
    {
        if(SameMarker.Instance.getPlayerArray().Count == 2)
        {
            switch(getIndex())
            {
                case 0:
                    waitingTimer[0].text = timerValue.ToString();
                    waitingTimer[1].text = "00";
                    break;

                case 1:
                    waitingTimer[1].text = timerValue.ToString();
                    waitingTimer[0].text = "00";
                    break;
            }

        }
    }

    int getIndex()
    {
        int index = GameManager.gm.playerTurn;
        if (SameMarker.Instance.getPlayerArray().Count == 2)
        {
            if (index == 3)
                index = 1;
            else
                index = index - 1;
        }
        else
        {
            index = index - 1;
        }

        return index;
    }

}
