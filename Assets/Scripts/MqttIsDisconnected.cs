using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MqttIsDisconnected : MonoBehaviour
{
    WaitingForOpponent waitingForOpponent;
    [SerializeField] GameObject mqttIsDisconnectedPanel,connectionFailedPanel;
    [SerializeField] Button exitButton, closeButton;
    CountDownTimer countDownTimer;

    public static float quitTimeRemaining = 20;
    public static bool quitTimerIsRunning = false;
    M2MqttUnity.Examples.M2MqttUnityTest m2MqttUnityTest;
    ExitManager exitManager;
    APICall aPICall;
    ErrorDialogScript errorDialogScript;

    bool status = false;
    bool timeOut = false;

    private void Start()
    {
        exitButton.onClick.AddListener(exitGame);
        closeButton.onClick.AddListener(exitGame);
    }



    void Awake()
    {
        waitingForOpponent = FindObjectOfType<WaitingForOpponent>();
        countDownTimer = FindObjectOfType<CountDownTimer>();
        m2MqttUnityTest = FindObjectOfType<M2MqttUnity.Examples.M2MqttUnityTest>();
        exitManager = FindObjectOfType<ExitManager>();
        aPICall = FindObjectOfType<APICall>();
        errorDialogScript = FindObjectOfType<ErrorDialogScript>();

    }

    void Update()
    {
        if(GameManager.gm.isOnlineGame)
        {
            if (GameManager.gm.mqttIsConnected)
            {
                quitTimerIsRunning = false;
                quitTimeRemaining = 20;

                if (mqttIsDisconnectedPanel.activeInHierarchy)
                {

                    if (SameMarker.Instance.getPlayerArray()[getIndex()].player_name == Credentials.email)
                    {
                        PlayEvent playEvent = new PlayEvent();
                        playEvent.action = "actionInternetIsCame";
                        playEvent.sender = Credentials.email;
                        playEvent.playerPieceEvent = null;
                        playEvent.bot = null;

                        MqttEvent mqttEvent = new MqttEvent();
                        mqttEvent.playEvent = playEvent;

                        string json = JsonUtility.ToJson(mqttEvent);

                        //  mqttMessage.text = json;

                        Credentials.action = json;
                        m2MqttUnityTest.TestPublish();
                    }

                    mqttIsDisconnectedPanel.SetActive(false);

                    

                    if(!timeOut)
                    {
                        GameManager.gm.isStartTimer = true;
                        GameManager.gm.isSaveGame = true;
                        countDownTimer.startTimer();


                        if(!GameManager.gm.isBackground)
                        {
                            aPICall.callMatchStatus();
                            aPICall.getMovesOnServer();
                        }
                    }
                    else
                    {
                        GameManager.gm.isOnlineGame = false;
                    }
                    
                }
                
            }
            else
            {
                if (!mqttIsDisconnectedPanel.activeInHierarchy)
                {
                    mqttIsDisconnectedPanel.SetActive(true);

                    if (SameMarker.Instance.getPlayerArray()[getIndex()].player_name != Credentials.email)
                    {
                        quitTimeRemaining = 20;
                        quitTimerIsRunning = true;
                    }
                }
                    
            }

            if (quitTimerIsRunning)
            {
                if (quitTimeRemaining > 0)
                {
                    quitTimeRemaining -= Time.deltaTime;
                }
                else
                {
                    quitTimeRemaining = 0;
                    quitTimerIsRunning = false;

                    //SceneManager.LoadScene(0);
                    // exitManager.sendDataToGcApp();

                    if (!connectionFailedPanel.activeInHierarchy)
                        connectionFailedPanel.SetActive(true);

                    timeOut = true;
                }
            }
        }
        else if(waitingForOpponent.getPanelObject().activeInHierarchy && !InternetConnectionAvailability.isFirstTimeCall)
        {
            if (!status)
                StartCoroutine(displayErrorDialog());


            if (quitTimerIsRunning)
            {
                if (quitTimeRemaining > 0)
                {
                    quitTimeRemaining -= Time.deltaTime;
                }
                else
                {
                    quitTimeRemaining = 0;
                    quitTimerIsRunning = false;

                    //SceneManager.LoadScene(0);
                    // exitManager.sendDataToGcApp();

                    if (!connectionFailedPanel.activeInHierarchy)
                        connectionFailedPanel.SetActive(true);
                    GameManager.gm.isOnlineGame = false;
                    timeOut = true;
                }
            }

        }
        
    }

    public GameObject getPanelObject()
    {
        return mqttIsDisconnectedPanel;
    }

    public bool isShown()
    {
        return mqttIsDisconnectedPanel.activeInHierarchy;
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

    IEnumerator displayErrorDialog()
    {
        status = true;
        yield return new WaitForSeconds(5f);

        if (!GameManager.gm.mqttIsConnected)
        {
            quitTimeRemaining = 20;
            quitTimerIsRunning = true;

            if (waitingForOpponent.getPanelObject().activeInHierarchy && !mqttIsDisconnectedPanel.activeInHierarchy)
                mqttIsDisconnectedPanel.SetActive(true);
            //if(waitingForOpponent.getPanelObject().activeInHierarchy)
            //errorDialogScript.displayRetryDialog("Please check Internet connection...", true, "getMovesOnServer", null);
        }
        else
        {
          if (mqttIsDisconnectedPanel.activeInHierarchy)
            {
                quitTimerIsRunning = false;
                quitTimeRemaining = 20;
                mqttIsDisconnectedPanel.SetActive(false);
                aPICall.getMovesOnServer();
            }
        }

        status = false;
    }

    private void exitGame()
    {
        GameManager.gm.isGameRunning = false;
        SameMarker.ClearInstance();
        MqttMessageArray.ClearInstance();
        GameManager.clearGameManager();
        timeOut = false;
        ReloadScene();
    }

    void clearCredentials()
    {
        Credentials.id = null;
        Credentials.channel = null;
        Credentials.id = null;
        Credentials.old_id = null;
        Credentials.email = null;
        Credentials.action = null;
        Credentials.contestId = null;
        Credentials.userId = null;
        Credentials.contestSize = null;

        timeOut = false;

    }

    private void ReloadScene()
    {

#if UNITY_ANDROID
        clearIntent();
#endif

        clearCredentials();

        SceneManager.LoadScene(1);
    }

    void clearIntent()
    {
        AndroidJavaClass UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject currentActivity = UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

        AndroidJavaObject intent = currentActivity.Call<AndroidJavaObject>("getIntent");

        if (intent != null)
        {
            intent.Call<AndroidJavaObject>("putExtra", "GCPlayer", null);
            intent.Call<AndroidJavaObject>("putExtra", "tokenID", null);
            intent.Call<AndroidJavaObject>("putExtra", "MQChannel", null);
            intent.Call<AndroidJavaObject>("putExtra", "contest_id_gc", null);
            intent.Call<AndroidJavaObject>("putExtra", "user_id_gc", null);
            intent.Call<AndroidJavaObject>("putExtra", "contest_size", null);
        }

    }
}
