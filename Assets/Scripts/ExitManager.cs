using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
 using UnityEngine.SceneManagement;
using TMPro;
using System;
using System.Diagnostics;




public class ExitManager : MonoBehaviour
{   
    [SerializeField] GameObject saveGamePanel,saveButton,cancelButton, forgotPassPanel, ExitGamePanel, LogOutPanel, LoadingPanel;
    Button yesButton, noButton;
    StartGamePopup startGamePopup;
    PlayerPopUp playerPopUp;
    WinnerResult winnerResult;
    CountDownTimer countDownTimer;
    Coroutine quit_Coroutine;
    Login login;
    SignUp signUp;
    WaitingForOpponent waitingForOpponent;
    MqttIsDisconnected mqttIsDisconnected;
    public M2MqttUnity.Examples.M2MqttUnityTest m2MqttUnityTest;
    public TextMeshProUGUI mqttMessage;
    public static bool isBackground;
    APICall aPICall;
    bool isApplicationQuit = false;
    SampleWebView sampleWebView;
    SettingMenu settingMenu;
    MenuHandler menuHandler;
    [SerializeField] Button quitButton, continueButton;



    void Start()
    {
    	
        objectSetActivie(saveGamePanel,false);

        yesButton = saveButton.GetComponent<Button>();
        noButton = cancelButton.GetComponent<Button>();

        noButton.onClick.AddListener(exit);
        yesButton.onClick.AddListener(closePopup);

        quitButton.onClick.AddListener(quitGame1);
        continueButton.onClick.AddListener(continueGame);
    }

    void Awake()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        startGamePopup = FindObjectOfType<StartGamePopup>();
        playerPopUp = FindObjectOfType<PlayerPopUp>();
        winnerResult = FindObjectOfType<WinnerResult>();
        countDownTimer = FindObjectOfType<CountDownTimer>();
        login = FindObjectOfType<Login>();
        signUp = FindObjectOfType<SignUp>();
        waitingForOpponent = FindObjectOfType<WaitingForOpponent>();
        m2MqttUnityTest = FindObjectOfType<M2MqttUnity.Examples.M2MqttUnityTest>();
        mqttIsDisconnected = FindObjectOfType<MqttIsDisconnected>();
        aPICall = FindObjectOfType<APICall>();
        sampleWebView = FindObjectOfType<SampleWebView>();
        settingMenu = FindObjectOfType<SettingMenu>();
        menuHandler = FindObjectOfType<MenuHandler>();
    }

    void Update()
    {
        
        // Check if Back was pressed this frame
        if (Input.GetKeyDown(KeyCode.Escape)) {

            
            if (GameManager.gm.isGameRunning)
            {
                if(!winnerResult.isShown())
                objectSetActivie(saveGamePanel,true);
                else
                {
                    if (winnerResult.isShown())
                    {
                        if (GameManager.gm.isOnlineGame)
                        {
                            sendDataToGcApp();

                        }
                        else
                        {
                            ReloadScene();
                        }

                    }
                    else
                        ReloadScene();
                }
                

                GameManager.gm.isStartTimer = false;
            }
            else
            {
                if (forgotPassPanel.activeInHierarchy)
                {
                    forgotPassPanel.SetActive(false);
                }
                else
                {
                    if (waitingForOpponent.getPanelObject().activeInHierarchy)
                    {
                        objectSetActivie(ExitGamePanel,true);
                        //aPICall.quitGameAPI();
                    }
                    else if (login.getPanelObject().activeInHierarchy)
                    {
                        Application.Quit();
                    }
                    else if (signUp.getPanelObject().activeInHierarchy)
                    {
                        objectSetActivie(gameObject, false);
                        ReloadScene();
                    }
                    else if (playerPopUp.getPanelObject().activeInHierarchy)
                    {
                        if (GameManager.gm.isWebPageOpen)
                        {
                            sampleWebView.closeGUI();
                            menuHandler.resetSelectedButton();
                        }
                        else if (settingMenu.getSettingPanelObject().activeInHierarchy)
                        {
                            settingMenu.getSettingPanelObject().SetActive(false);
                            menuHandler.resetSelectedButton();
                        }
                        else if(LogOutPanel.activeInHierarchy)
                        {
                            LogOutPanel.SetActive(false);
                        }
                        else
                            Application.Quit();
                        //if (GameManager.gm.isLogin)
                        //{
                        //        Application.Quit();
                        //}
                        //else
                        //    ReloadScene();
                    }
                    else
                    {
                        ReloadScene();
                    }
                }
                
            }
            
        }


    }


    void objectSetActivie(GameObject gameObject, bool status)
    {
        if(gameObject)
        {
            gameObject.SetActive(status);
        }
    }

    void exit()
    {
        if (GameManager.gm.isOnlineGame)
        {
            if (GameManager.gm.mqttIsConnected)
            {
                // StartCoroutine(quitGame());

                publishQuitMessage();
                SoundManager.PlaySound("click");

                //  Caching.ClearCache();
                //  PlayerPrefs.DeleteAll();
                //  SceneManager.LoadScene(1);
                SeriouslyDeleteAllSaveFiles();
                GameManager.gm.isGameRunning = false;
                SameMarker.ClearInstance();
                MqttMessageArray.ClearInstance();
                GameManager.clearGameManager();
                ReloadScene();
                //sendDataToGcApp();
            }
        }
        else
        {
            SoundManager.PlaySound("click");
            SeriouslyDeleteAllSaveFiles();
            GameManager.gm.isGameRunning = false;
            SameMarker.ClearInstance();
            MqttMessageArray.ClearInstance();
            GameManager.clearGameManager();
            ReloadScene();
        }

        
    }

    private void quitGame1()
    {
        aPICall.quitGameAPI();
        objectSetActivie(ExitGamePanel, false);
    }

    private void continueGame()
    {
        objectSetActivie(ExitGamePanel, false);
    }

    void saveData()
    {
        string path = Application.persistentDataPath + "/player.fun";
        FileStream fs = new FileStream(path, FileMode.Create);
        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(fs, SameMarker.Instance.getArray());
        fs.Close();
        print("saveData");

        string path1 = Application.persistentDataPath + "/playername.fun";
        FileStream fs1 = new FileStream(path1, FileMode.Create);
        BinaryFormatter bf1 = new BinaryFormatter();
        bf1.Serialize(fs1, SameMarker.Instance.getPlayerArray());
        fs1.Close();

        string path2 = Application.persistentDataPath + "/gamemanager.fun";
        FileStream fs2 = new FileStream(path2, FileMode.Create);
        BinaryFormatter bf2 = new BinaryFormatter();
        GameManagerPojo gameManagerPojo = new GameManagerPojo(GameManager.gm.numberOfStepsToMove, GameManager.gm.dice, GameManager.gm.sixCount,
            GameManager.gm.isKilled, GameManager.gm.playerChance,GameManager.gm.isReadyToMove, GameManager.gm.isGameRunning, GameManager.gm.isSixCountGraterThanTwo,
            GameManager.gm.canDiceRoll ,GameManager.gm.playerTurn, GameManager.gm.isAIPlayed, GameManager.gm.countDownStartValue, GameManager.gm.botCount1, 
            GameManager.gm.botCount2, GameManager.gm.botCount3, GameManager.gm.botCount4);
        bf2.Serialize(fs2, gameManagerPojo);
        fs2.Close();

        string path3 = Application.persistentDataPath + "/winnerList.fun";
        FileStream fs3 = new FileStream(path3, FileMode.Create);
        BinaryFormatter bf3 = new BinaryFormatter();
        bf3.Serialize(fs3, SameMarker.Instance.getResultArray());
        fs3.Close();

        GameManager.gm.isGameRunning = false;
        SameMarker.ClearInstance();
        GameManager.clearGameManager();
        ReloadScene();
       // Application.Quit();
    }

    public void showBox()
    {
        SoundManager.PlaySound("popup");
        objectSetActivie(saveGamePanel,true);
    }

    public static void SeriouslyDeleteAllSaveFiles()
     {
         string path = Application.persistentDataPath +  "/player.fun";
         File.Delete(path);

         string path1 = Application.persistentDataPath +  "/playername.fun";
         File.Delete(path1);

         string path2 = Application.persistentDataPath +  "/gamemanager.fun";
         File.Delete(path2);

         string path3 = Application.persistentDataPath + "/winnerList.fun";
         File.Delete(path3);

         /*DirectoryInfo directory = new DirectoryInfo(path);
         directory.Delete(true);
         Directory.CreateDirectory(path);*/
     }

     public bool isShown()
     {
        return saveGamePanel.activeInHierarchy;
     }

     public void closePopup()
     {
        SoundManager.PlaySound("click");
        
        if(isShown())
        objectSetActivie(saveGamePanel,false);

        GameManager.gm.isStartTimer = true;
        GameManager.gm.isSaveGame = true;
        countDownTimer.startTimer();
        GameManager.gm.isAIPlayed = false;
     }

     IEnumerator quit()
     {
        yield return new WaitForSeconds(0.1f);

        ReloadScene();

          yield return new WaitForEndOfFrame();

          if(quit_Coroutine != null)
          {
              StopCoroutine(quit_Coroutine);
          }
      }

    void publishQuitMessage()
    {
        QuitPlayer quitPlayer = new QuitPlayer();
        quitPlayer.player = GameManager.gm.player;

        PlayEvent playEvent = new PlayEvent();
        playEvent.action = "actionQuitPlayer";
        playEvent.sender = Credentials.email;
        playEvent.quitPlayer = quitPlayer;
        playEvent.playerPieceEvent = null;
        playEvent.bot = null;


        MqttEvent mqttEvent = new MqttEvent();
        mqttEvent.playEvent = playEvent;

        string json = JsonUtility.ToJson(mqttEvent);
        mqttMessage.text = json;
        Credentials.action = json;
        m2MqttUnityTest.TestPublish();


        ServerPlayEvent playEvent1 = new ServerPlayEvent();
        playEvent1.action = "actionQuitPlayer";
        playEvent1.sender = Credentials.email;
        playEvent1.turn_id = GameManager.gm.turn_id;
        playEvent1.quitPlayer = quitPlayer;
       
        ServerMqttEvent mqttEvent1 = new ServerMqttEvent();
        mqttEvent1.playEvent = playEvent1;

        string json1 = JsonUtility.ToJson(mqttEvent1);

        print(json1);

        MqttMessageArray.Instance.addMessages(new MqMessage(json1));

        GameMove gameMove = new GameMove();
        gameMove.info = json1;


        GameMoveForContestReq gameMoveForContestReq = new GameMoveForContestReq();
        gameMoveForContestReq.game_move = gameMove;

        string json2 = JsonUtility.ToJson(gameMoveForContestReq);

        json2.Replace(@"\", "");

        
       aPICall.storeMovesOnServer(gameMoveForContestReq);
    }

    IEnumerator quitGame()
    {
        publishQuitMessage();
        yield return new WaitForSeconds(2f);

         SoundManager.PlaySound("click");
         SeriouslyDeleteAllSaveFiles();
         GameManager.gm.isGameRunning = false;
         SameMarker.ClearInstance();
         GameManager.clearGameManager();
        ReloadScene();
    }

    public void sendDataToGcApp()
    {
#if UNITY_ANDROID
        bool fail = false;
        //string bundleId = "com.sklash.sklash"; // your target bundle id
        string bundleId = "com.sklash"; // your target bundle id
        AndroidJavaClass up = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject ca = up.GetStatic<AndroidJavaObject>("currentActivity");
        AndroidJavaObject packageManager = ca.Call<AndroidJavaObject>("getPackageManager");

        AndroidJavaObject launchIntent = null;
        try
        {
            launchIntent = packageManager.Call<AndroidJavaObject>("getLaunchIntentForPackage", bundleId);
            

            if (winnerResult.isShown())
                {
                    launchIntent.Call<AndroidJavaObject>("putExtra", "Winner", SameMarker.Instance.getResultArray()[0].name.ToString());
                    launchIntent.Call<AndroidJavaObject>("putExtra", "MatchId", Credentials.id);
                    launchIntent.Call<AndroidJavaObject>("putExtra", "contest_id_gc", Credentials.contestId);
                    launchIntent.Call<AndroidJavaObject>("putExtra", "IntentFrom", "LUDO");
            }
            else
            {
                launchIntent.Call<AndroidJavaObject>("putExtra", "Winner", null);
            }
               
        }
        catch (System.Exception e)
        {
            fail = true;
        }

        WaitingMqttMessage.quitTimerIsRunning = false;
        WaitingMqttMessage.quitTimeRemaining = 20;
        
        SeriouslyDeleteAllSaveFiles();
        GameManager.gm.isGameRunning = false;
        SameMarker.ClearInstance();
        MqttMessageArray.Instance.clearList();
        MqttMessageArray.ClearInstance();
        GameManager.clearGameManager();

        if (fail)
        { //open app in store
            Application.OpenURL("https://google.com");
        }
        else //open the app
            ca.Call("startActivity", launchIntent);

        up.Dispose();
        ca.Dispose();
        packageManager.Dispose();
        launchIntent.Dispose();
#endif

#if UNITY_IOS
// string url = "GameCenter://?match_id=" + Credentials.id.ToString() + "&contest_id=" + Credentials.contestId.ToString() + "&winner=" + SameMarker.Instance.getResultArray()[0].name.ToString();
 Application.OpenURL("sklash://?contest_id=" + Credentials.contestId.ToString());
#endif

        Application.Quit();  
    }

#if UNITY_ANDROID
    void OnApplicationPause(bool paused)
    {
        if (paused)
        {
            GameManager.gm.isBackground = true;

            if (GameManager.gm.isOnlineGame)
            {
                if (GameManager.gm.isGameRunning)
                {
                    GameManager.gm.isStartTimer = false;
                    GameManager.gm.isSaveGame = false;
                    isBackground = true;
                }

            }
            
        }
        else
        {
            GameManager.gm.flow = "Restart the application ";

            if (GameManager.gm.isOnlineGame)
            {
                if (GameManager.gm.isGameRunning)
                {
                    if(isBackground)
                    {
                        //if (GameManager.gm.mqttIsConnected)
                        //{
                        //    publishQuitMessage();
                        //}
                        //sendDataToGcApp();
                        isBackground = false;
                        GameManager.gm.isStartTimer = true;
                        GameManager.gm.isSaveGame = true;
                        StartCoroutine(forground());
                    }    
                }
            }

            if (waitingForOpponent.getPanelObject().activeInHierarchy)
            {
                // aPICall.callMatchStatus();
                StartCoroutine(forground());
            }


            if (login.getPanelObject().activeInHierarchy)
            {
                ReloadScene();
            }
        }
    }

#endif


#if UNITY_IOS
    private void OnApplicationFocus(bool focus)
    {
        if (focus)
        {
            GameManager.gm.flow = "Restart the application ";

            if (GameManager.gm.isOnlineGame)
            {
                if (GameManager.gm.isGameRunning)
                {
                    if(isBackground)
                    {
                        if (GameManager.gm.mqttIsConnected)
                        {
                            publishQuitMessage();
                        }
                        sendDataToGcApp();
                    }    
                }
            }


            if(login.getPanelObject().activeInHierarchy)
            {
                ReloadScene();
            }
        }
        else
        {
            if(GameManager.gm.isOnlineGame)
            {
                if (GameManager.gm.isGameRunning)
                {
                    isBackground = true;
                }

            }
        }
    }
#endif


    private void OnApplicationQuit()
    {
        ProcessThreadCollection pt = Process.GetCurrentProcess().Threads;
        foreach (ProcessThread p in pt)
        {
            p.Dispose();
        }
 
    }

    void clearIntent()
    {
        AndroidJavaClass UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject currentActivity = UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

        AndroidJavaObject intent = currentActivity.Call<AndroidJavaObject>("getIntent");

        if(intent != null)
        {
            intent.Call<AndroidJavaObject>("putExtra", "GCPlayer", null);
            intent.Call<AndroidJavaObject>("putExtra", "tokenID", null);
            intent.Call<AndroidJavaObject>("putExtra", "MQChannel", null);
            intent.Call<AndroidJavaObject>("putExtra", "contest_id_gc", null);
            intent.Call<AndroidJavaObject>("putExtra", "user_id_gc", null);
        }
        
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
    }

    private void ReloadScene()
    {

#if UNITY_ANDROID
        clearIntent();
#endif

        clearCredentials();

        SceneManager.LoadScene(1);
    }

    IEnumerator forground()
    {
        if (LoadingPanel)
            LoadingPanel.SetActive(true);

        yield return new WaitForSeconds(2f);

        aPICall.callMatchStatus();

        aPICall.getMovesOnServer();

    }


}
