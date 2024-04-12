using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;


public class RestartPanelScript : MonoBehaviour
{
    WaitingForOpponent waitingForOpponent;
    bool isCheck;
    [SerializeField] GameObject RestartPanel, waitingForOpponentPanel;
    [SerializeField] Button Yes, No;

    string arguments = "";
    string id = "";
    string channel = "";
    string contestId = "";
    string userId = "";
    string contestName = "";

    AndroidJavaClass UnityPlayer;
    AndroidJavaObject currentActivity;
    AndroidJavaObject intent;

    [SerializeField] TextMeshProUGUI contestIdText, playerNameText;

    void Awake()
    {
        waitingForOpponent = FindObjectOfType<WaitingForOpponent>();
    }

    private void Start()
    {
        Yes.onClick.AddListener(closePopup);
        No.onClick.AddListener(closeApplication);
    }

    // Update is called once per frame
    void Update()
    {

        StartCoroutine(checkIntent());   
        
    }

    void closePopup()
    {
        if (RestartPanel)
            RestartPanel.SetActive(false);

        //if(waitingForOpponent.getPanelObject())
        //waitingForOpponent.getPanelObject().SetActive(true);


        SceneManager.LoadScene(0);
    }

    void closeApplication()
    {
        Application.Quit();
    }

    IEnumerator checkIntent()
    {
        if(!isCheck)
        yield return new WaitForSeconds(2f);

#if UNITY_ANDROID

        isCheck = true;
        UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        currentActivity = UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

        intent = currentActivity.Call<AndroidJavaObject>("getIntent");

        bool hasExtra = intent.Call<bool>("hasExtra", "GCPlayer");
        bool hasExtra1 = intent.Call<bool>("hasExtra", "tokenID");
        bool hasExtra2 = intent.Call<bool>("hasExtra", "MQChannel");

        //email.text = hasExtra +" : "+ hasExtra1 + " : "+ hasExtra2;

        if (hasExtra)
        {

            AndroidJavaObject extras = intent.Call<AndroidJavaObject>("getExtras");
            arguments = extras.Call<string>("getString", "GCPlayer");
            id = extras.Call<string>("getString", "tokenID");
            contestId = extras.Call<string>("getString", "contest_id_gc");
            userId = extras.Call<string>("getString", "user_id_gc");
            contestName = extras.Call<string>("getString", "contest_name");

            if (arguments != null)
            {
                if ((id != Credentials.id || id == null) && GameManager.gm.isLogin && (GameManager.gm.isOnlineGame|| waitingForOpponentPanel.activeInHierarchy))
                {
                    contestIdText.text = contestName;
                    playerNameText.text = arguments;

                    if (RestartPanel)
                        RestartPanel.SetActive(true);
                    
                }

            }
    }
#endif


    }
}
