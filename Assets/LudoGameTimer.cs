using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LudoGameTimer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerTextLabel1;
    // public Text timerText;
    private float timeLeft = 600f; // Initial time in seconds
    private bool isTimerRunning = false;
    public bool isGameTimeEnd = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isTimerRunning)
        {
            if (timeLeft >= 0)
            {
                isGameTimeEnd = false;
                timeLeft -= Time.deltaTime;
                UpdateTimerUI();
            }
            else
            {
                timeLeft = 0;
                isTimerRunning = false;
                timerTextLabel1.text = "00:00";
                Debug.Log("Timer Finished!");
                isGameTimeEnd = true;
            }
        } 
        
    }

      void UpdateTimerUI()
    {
        int minutes = Mathf.FloorToInt(timeLeft / 60f);
        int seconds = Mathf.FloorToInt(timeLeft % 60f);
        timerTextLabel1.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void StartTimer()
    {
        isTimerRunning = true;
        Debug.Log("************************************* StartTimer called for Fast Ludo **************************************");
    }
}
