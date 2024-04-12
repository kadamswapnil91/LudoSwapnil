using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using M2MqttUnity;
using UnityEngine.SceneManagement;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System;

// Android Ludo Pro V2

public class WaitingForOpponent : MonoBehaviour
{
	[SerializeField] GameObject waitingForOpponentPanel, PlayAsGuestPanel, OrText, ExitGamePanel, StartButtonObj;
	[SerializeField] TextMeshProUGUI[] playerNames;
	[SerializeField] Button[] markers;
	[SerializeField] Button[] background;
	[SerializeField] Button backPress, quitButton, continueButton;
    [SerializeField] Button startButton;
    [SerializeField] GameObject LoginPanel,ExitPanel,MenuPanel;
    [SerializeField] TextMeshProUGUI mqttMessage;
    [SerializeField] TextMeshProUGUI displayIntentData;
    M2MqttUnity.M2MqttUnityClient m2MqttUnityClient;
    M2MqttUnity.Examples.M2MqttUnityTest m2MqttUnityTest;

  [SerializeField] GameObject firstPlayerNameTextPanel, secondPlayerNameTextPanel, thirdPlayerNameTextPanel, forthPlayerNameTextPanel;
  [SerializeField] TextMeshProUGUI text1,text2,text3,text4;
  [SerializeField] GameObject redDiceHolder, yellowDiceHolder;
  [SerializeField] GameObject redDiceHolderImage, yellowDiceHolderImage;
  [SerializeField] GameObject redCdtImage, yellowCdtImage;
  [SerializeField] GameObject playerMarkers3, playerMarkers4, exitButton;

  Coroutine changePosition_Coroutine;
  Coroutine changeCameraPosition_Coroutine;
  Coroutine objectRotation_Coroutine;

  SpriteHandler spriteHandler;
  private CountDownTimer countDownTimer;
  GameSettings gameSettings;
  private RollingDice rollingDice;
  List<string> colorArray = new List<string>();
  string playerColor;
  [SerializeField] GameObject board, diceHolderMainObject;
  [SerializeField] GameObject[] timer;
  [SerializeField] GameObject timerPanel;

  [SerializeField] GameObject[] playerName;
  [SerializeField] GameObject PlayerNamePanel;
    // fast ludo
    [SerializeField] GameObject[] playerStepsCount;
     // Fast Ludo timer
     private LudoGameTimer gameTimer;
     [SerializeField] Sprite SklashLudo1;
     [SerializeField] Image logoHeaderImg;
  
     private Flavor flavor;
    

    [SerializeField] GameObject[] botCountText;
    [SerializeField] GameObject botCountTextPanel;

    [SerializeField] GameObject[] waitTimerText;
    [SerializeField] GameObject waitTimerTextPanel;

    PathObjectsParent pathParent;
    public System.DateTime startTime;

    AndroidJavaClass UnityPlayer;

    [SerializeField] Sprite redSprite, blueSprite;

    [SerializeField] TextMeshProUGUI message;
    int count = 0;

    APICall aPICall;

    float y_axis = 0f;
    float x_axis = 0f;
    float dice_holder_position = 0f;

    [SerializeField] GameObject LoadingPanel;

    string id1 = "";
    string channel1 = "";
    string contestId1 = "";
    string userId1 = "";
    string contestName1 = "";
    bool is_admin_contest1 = false;

    string contestSize = "2";

    WaitingForOpponentCountDownTimer waitingForOpponentCountDownTimer;


    // Start is called before the first frame update
    void Awake()
    {
            m2MqttUnityClient = FindObjectOfType<M2MqttUnity.M2MqttUnityClient>();
            m2MqttUnityTest = FindObjectOfType<M2MqttUnity.Examples.M2MqttUnityTest>();

            countDownTimer = FindObjectOfType<CountDownTimer>();
            gameSettings = FindObjectOfType<GameSettings>();
            spriteHandler = FindObjectOfType<SpriteHandler>();

            pathParent = FindObjectOfType<PathObjectsParent>();

            aPICall = FindObjectOfType<APICall>();

            waitingForOpponentCountDownTimer = FindObjectOfType<WaitingForOpponentCountDownTimer>();
    // fast ludo timer
            gameTimer = FindObjectOfType<LudoGameTimer>();
          	flavor = FindObjectOfType<Flavor>();
    }


    void Start()
    {
        flavor.flavorCheck();
        setScreen();
        Debug.Log("WFO_Start: ****************** Waiting for Opponent start method called ");
        if (MqttMessageArray.Instance.getMessageList().Count > 0)
        {
            MqttMessageArray.Instance.clearList();
            MqttMessageArray.ClearInstance();
        }
    

    
       

#if UNITY_IOS
        if (Credentials.id == null || Credentials.id == "")
            close();
        else
            connectMQ();
#endif

        setScreen();

        startTime = System.DateTime.UtcNow;

        updateText();


#if UNITY_ANDROID
         string arguments = "";
         string id = "";
         string channel = "";
         string contestId = "";
         string userId = "";
         string contestName = "";
         bool is_admin_contest = false;

         UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
         AndroidJavaObject currentActivity = UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
 
         AndroidJavaObject intent = currentActivity.Call<AndroidJavaObject>("getIntent");
         bool hasExtra = intent.Call<bool>("hasExtra", "GCPlayer");
         bool hasExtra1 = intent.Call<bool>("hasExtra", "tokenID");
         bool hasExtra2 = intent.Call<bool>("hasExtra", "MQChannel");

        //email.text = hasExtra +" : "+ hasExtra1 + " : "+ hasExtra2;
        //GameManager.gm.flow2 = "hasExtra : " + hasExtra1;
        if (hasExtra1)
        {

            AndroidJavaObject extras = intent.Call<AndroidJavaObject>("getExtras");
            arguments = extras.Call<string>("getString", "GCPlayer");
            id = extras.Call<string>("getString", "tokenID");
            contestId = extras.Call<string>("getString", "contest_id_gc");
            userId = extras.Call<string>("getString", "user_id_gc");
            contestName = extras.Call<string>("getString", "contest_name");
            is_admin_contest = extras.Call<bool>("getBoolean", "is_admin_contest");
            contestSize = extras.Call<string>("getString", "contest_size");

            if (id != null)
            {
                Credentials.channel = "arenas/global/matches/" + id;
                Credentials.id = id;
                Credentials.contestId = contestId;
                Credentials.userId = userId;
                Credentials.isAdminContest = is_admin_contest;
                Credentials.userName = arguments;
                if (contestSize != null)
                    Credentials.contestSize = contestSize;

                message.text = contestName;

                string path1 = Application.persistentDataPath + "/loginUserPath.fun";
                bool isLogin = false;

                if (File.Exists(path1))
                {
                    using (Stream stream = File.Open(path1, FileMode.Open))
                    {
                        var bformatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

                        LoginUser loginUser = (LoginUser)bformatter.Deserialize(stream);

                        if (loginUser != null)
                        {

                            isLogin = loginUser.isLogin;
                            Credentials.email = loginUser.email;
                            GameManager.gm.loginUserName = loginUser.first_name;
                            GameManager.gm.loginUsertoken = loginUser.token;
                        }
                    }
                }

                // GameManager.gm.flow2 = GameManager.gm.flow2 + "isLogin : "+ isLogin;
                if (isLogin)
                {
                    aPICall.callMatchStatus();
                    m2MqttUnityClient.Connect();

                    if (is_admin_contest)
                    {
                        objectSetActivie(StartButtonObj, false);
                    }
                    else
                    {
                        // objectSetActivie(StartButtonObj, true);
                        objectSetActivie(StartButtonObj, false);
                    }

                    hasExtra1 = false;
                    intent.Call<AndroidJavaObject>("putExtra", "GCPlayer", null);
                    intent.Call<AndroidJavaObject>("putExtra", "tokenID", null);
                    intent.Call<AndroidJavaObject>("putExtra", "MQChannel", null);
                    intent.Call<AndroidJavaObject>("putExtra", "contest_id_gc", null);
                    intent.Call<AndroidJavaObject>("putExtra", "user_id_gc", null);
                    intent.Call<AndroidJavaObject>("putExtra", "is_admin_contest", null);
                    intent.Call<AndroidJavaObject>("putExtra", "contest_size", null);

                    objectSetActivie(LoginPanel, false);
                }
                else
                {
                    PlayerPrefs.SetInt("playAsGuest", 0);
                    PlayAsGuestPanel.SetActive(false);
                    OrText.SetActive(false);
                    close();
                }


            }
            else
            {
                PlayAsGuestPanel.SetActive(true);
                OrText.SetActive(true);
                close();
            }


        }
        else
        {
            // GameManager.gm.flow2 = GameManager.gm.flow2 + "Close Waiting Screen";
            objectSetActivie(LoadingPanel, true);
            Branch.initSession(CallbackWithBranchUniversalObject);
            StartCoroutine(callBranchIoData());
        }

#endif


        colorArray.Add("green");
        colorArray.Add("red");
        colorArray.Add("blue");
        colorArray.Add("yellow");


        rollingDice = FindObjectOfType<RollingDice>();

        rollingDice.setVisibilityFirstPlayerDice(false);

        backPress.onClick.AddListener(backPressClick);
        startButton.onClick.AddListener(onStartButtonClicked);
        quitButton.onClick.AddListener(quitGame);
        continueButton.onClick.AddListener(continueGame);

    }



   public void display()
   {
   		objectSetActivie(waitingForOpponentPanel,true);
   		updateText();
   }

   public void updateText()
   {
        for (int i=0; i< playerNames.Length; i++)
        {
            playerNames[i].text = "waiting";
        }

      List<JoinPlayerInfo> joinPlayerList = SameMarker.Instance.getJointPlayerArray();

   		if(joinPlayerList.Count > 0)
   		{
   			for(int i = 0; i < joinPlayerList.Count; i++)
   			{
   				//playerNames[i].text = joinPlayerList[i].first_name;

                if (joinPlayerList[i].username != null && joinPlayerList[i].username != "")
                    playerNames[i].text = joinPlayerList[i].username;
                else if (joinPlayerList[i].last_name != null && joinPlayerList[i].last_name != "")
                    playerNames[i].text = joinPlayerList[i].first_name + " " + joinPlayerList[i].last_name;
                else if (joinPlayerList[i].first_name != null && joinPlayerList[i].first_name != "")
                    playerNames[i].text = joinPlayerList[i].first_name;
                else
                    playerNames[i].text = joinPlayerList[i].email;
            }

   			for(int i = 0 ; i < markers.Length ; i++)
   			{
   				if(i < joinPlayerList.Count)
   				{
   					markers[i].interactable = true;
   					background[i].interactable = true;
   				}
   				else
   				{
   					markers[i].interactable = false;
   					background[i].interactable = false;
   				}
   			}
   		}

      if(joinPlayerList.Count > 1)
      startButton.interactable = true;
      else
      startButton.interactable = false;

      if(joinPlayerList.Count <3)
        {
            markers[1].image.sprite = blueSprite;
            markers[2].image.sprite = redSprite;
        }
      else
        {
            markers[1].image.sprite = redSprite;
            markers[2].image.sprite = blueSprite;
        }

        int size = 0;

        if (contestSize != null && contestSize != "")
        {
            try
            {
                size = Convert.ToInt32(contestSize);

            }
            catch
            {
                size = 0;
            }
        }

        if (joinPlayerList.Count == size)
        {

            if (!GameManager.gm.isStartCountDownTimer && !Credentials.isAdminContest)
            {
                waitingForOpponentCountDownTimer.startTimer(System.DateTime.UtcNow.ToString(), joinPlayerList[0].email);
                GameManager.gm.isStartCountDownTimer = true;
            }
        }
        else
        {
            if (StartButtonObj.activeInHierarchy)
                StartButtonObj.SetActive(false);

            GameManager.gm.isStartCountDownTimer = false;
        }

    }


    // fast ludo 
    void hideStepCount() {
        for (int i = 0; i < playerStepsCount.Length; i++) {
            objectSetActivie(playerStepsCount[i], false);
        }
    }

    void showStepCount(bool isFirstPlayerEnable, bool isSecondPlayerEnable, bool isThirdPlayerEnable, bool isForthPlayerEnable ) {
        if (isFirstPlayerEnable) {
            objectSetActivie(playerStepsCount[0], true);
        }
        if (isSecondPlayerEnable)
        {
            objectSetActivie(playerStepsCount[1], true);
        }
        if (isThirdPlayerEnable)
        {
            objectSetActivie(playerStepsCount[2], true);
        }
        if (isForthPlayerEnable)
        {
            objectSetActivie(playerStepsCount[3], true);
        }
    }


  void objectSetActivie(GameObject gameObject, bool status)
	{
		if(gameObject)
		{
			gameObject.SetActive(status);
		}
	}

	public void close()
	{
        objectSetActivie(waitingForOpponentPanel, false);

        if (LoadingPanel.activeInHierarchy)
        objectSetActivie(LoadingPanel, false);
	}

  public void backPressClick()
  {
        objectSetActivie(ExitGamePanel, true);
       // aPICall.quitGameAPI();
  }

    private void quitGame()
    {
        aPICall.quitGameAPI();
        objectSetActivie(ExitGamePanel, false);
    }

    private void continueGame()
    {
        objectSetActivie(ExitGamePanel, false);
    }

    public GameObject getPanelObject()
	{
	    return waitingForOpponentPanel;
	}



  void stopInputFor2Player()
  {
      objectSetActivie(playerMarkers3,false);
      objectSetActivie(playerMarkers4,false);
      objectSetActivie(redDiceHolder,false);
      objectSetActivie(yellowDiceHolder,false);
      objectSetActivie(redDiceHolderImage,false);
      objectSetActivie(yellowDiceHolderImage,false);
      objectSetActivie(redCdtImage,false);
      objectSetActivie(yellowCdtImage,false);

      //objectSetActivie(secondPlayerNameText,false);
      //objectSetActivie(forthPlayerNameText,false);
  }

  void stopInputFor3Player()
  {
      objectSetActivie(playerMarkers3,true);
      objectSetActivie(playerMarkers4,false);
      objectSetActivie(redDiceHolder,false);
      objectSetActivie(yellowDiceHolder,false);
      objectSetActivie(redDiceHolderImage,true);
      objectSetActivie(yellowDiceHolderImage,false);
      objectSetActivie(redCdtImage,true);
      objectSetActivie(yellowCdtImage,false);

      //objectSetActivie(secondPlayerNameText,true);
      //objectSetActivie(forthPlayerNameText,false);

  }

  void stopInputFor4Player()
  {
      objectSetActivie(playerMarkers3,true);
      objectSetActivie(playerMarkers4,true);
      objectSetActivie(redDiceHolder,false);
      objectSetActivie(yellowDiceHolder,false);
      objectSetActivie(redDiceHolderImage,true);
      objectSetActivie(yellowDiceHolderImage,true);
      objectSetActivie(redCdtImage,true);
      objectSetActivie(yellowCdtImage,true);
      //objectSetActivie(secondPlayerNameText,true);
      //objectSetActivie(forthPlayerNameText,true);

  }


  void AddPlayerList()
  { 

    List<JoinPlayerInfo> joinPlayerList = SameMarker.Instance.getJointPlayerArray();

    if(joinPlayerList.Count == 2)
    {
      stopInputFor2Player();

      SameMarker.Instance.clearPlayerList();

            //SameMarker.Instance.addPlayer(new Player(1,joinPlayerList[0].email, false, false, false, false, GameManager.gm.firstPlayerColor,joinPlayerList[0].user_id, joinPlayerList[0].first_name + " " + joinPlayerList[0].last_name));
            //SameMarker.Instance.addPlayer(new Player(2,joinPlayerList[1].email, false, false, false, false, GameManager.gm.thirdPlayerColor,joinPlayerList[1].user_id, joinPlayerList[1].first_name + " " + joinPlayerList[1].last_name));

            SameMarker.Instance.addPlayer(new Player(1,joinPlayerList[0].email, false, false, false, false, GameManager.gm.firstPlayerColor,joinPlayerList[0].user_id, returnName(joinPlayerList, 0)));
            SameMarker.Instance.addPlayer(new Player(2,joinPlayerList[1].email, false, false, false, false, GameManager.gm.thirdPlayerColor,joinPlayerList[1].user_id, returnName(joinPlayerList, 1)));

            objectSetActivie(firstPlayerNameTextPanel,true);
      objectSetActivie(thirdPlayerNameTextPanel,true);

            //text1.text = joinPlayerList[0].first_name+" "+ joinPlayerList[0].last_name;
            //text3.text = joinPlayerList[1].first_name + " " + joinPlayerList[1].last_name;

// fast ludo
            showStepCount(true, false, true, false);
            text1.text = returnName(joinPlayerList, 0);
            text3.text = returnName(joinPlayerList, 1);
        }
    else if(joinPlayerList.Count == 3)
    {
      stopInputFor3Player();

      SameMarker.Instance.clearPlayerList();

            //SameMarker.Instance.addPlayer(new Player(1,joinPlayerList[0].email, false, false, false, false, GameManager.gm.firstPlayerColor,joinPlayerList[0].user_id, joinPlayerList[0].first_name + " " + joinPlayerList[0].last_name));
            //SameMarker.Instance.addPlayer(new Player(2,joinPlayerList[1].email, false, false, false, false, GameManager.gm.secondPlayerColor,joinPlayerList[1].user_id, joinPlayerList[1].first_name + " " + joinPlayerList[1].last_name));
            //SameMarker.Instance.addPlayer(new Player(3,joinPlayerList[2].email, false, false, false, false, GameManager.gm.thirdPlayerColor,joinPlayerList[2].user_id, joinPlayerList[2].first_name + " " + joinPlayerList[2].last_name));

            SameMarker.Instance.addPlayer(new Player(1, joinPlayerList[0].email, false, false, false, false, GameManager.gm.firstPlayerColor, joinPlayerList[0].user_id, returnName(joinPlayerList, 0)));
            SameMarker.Instance.addPlayer(new Player(2, joinPlayerList[1].email, false, false, false, false, GameManager.gm.secondPlayerColor, joinPlayerList[1].user_id, returnName(joinPlayerList, 1)));
            SameMarker.Instance.addPlayer(new Player(3, joinPlayerList[2].email, false, false, false, false, GameManager.gm.thirdPlayerColor, joinPlayerList[2].user_id, returnName(joinPlayerList, 2)));

            objectSetActivie(firstPlayerNameTextPanel,true);
      objectSetActivie(secondPlayerNameTextPanel,true);
      objectSetActivie(thirdPlayerNameTextPanel,true);
// fast ludo
          showStepCount(true, true, true, false);

            //text1.text = joinPlayerList[0].first_name + " " + joinPlayerList[0].last_name;
            //text2.text = joinPlayerList[1].first_name + " " + joinPlayerList[1].last_name;
            //text3.text = joinPlayerList[2].first_name + " " + joinPlayerList[2].last_name;

            text1.text = returnName(joinPlayerList, 0);
            text2.text = returnName(joinPlayerList, 1);
            text3.text = returnName(joinPlayerList, 2);
        }
    else if(joinPlayerList.Count == 4)
    {
      stopInputFor4Player();

      SameMarker.Instance.clearPlayerList();

      // SameMarker.Instance.addPlayer(new Player(1,joinPlayerList[0].email, false, false, false, false, GameManager.gm.firstPlayerColor,joinPlayerList[0].user_id, joinPlayerList[0].first_name + " " + joinPlayerList[0].last_name));
      // SameMarker.Instance.addPlayer(new Player(2,joinPlayerList[1].email, false, false, false, false, GameManager.gm.secondPlayerColor,joinPlayerList[1].user_id, joinPlayerList[1].first_name + " " + joinPlayerList[1].last_name));
      // SameMarker.Instance.addPlayer(new Player(3,joinPlayerList[2].email, false, false, false, false, GameManager.gm.thirdPlayerColor,joinPlayerList[2].user_id, joinPlayerList[2].first_name + " " + joinPlayerList[2].last_name));
      // SameMarker.Instance.addPlayer(new Player(4,joinPlayerList[3].email, false, false, false, false, GameManager.gm.fourthPlayerColor,joinPlayerList[3].user_id, joinPlayerList[3].first_name + " " + joinPlayerList[3].last_name));

            SameMarker.Instance.addPlayer(new Player(1, joinPlayerList[0].email, false, false, false, false, GameManager.gm.firstPlayerColor, joinPlayerList[0].user_id, returnName(joinPlayerList, 0)));
            SameMarker.Instance.addPlayer(new Player(2, joinPlayerList[1].email, false, false, false, false, GameManager.gm.secondPlayerColor, joinPlayerList[1].user_id, returnName(joinPlayerList, 1)));
            SameMarker.Instance.addPlayer(new Player(3, joinPlayerList[2].email, false, false, false, false, GameManager.gm.thirdPlayerColor, joinPlayerList[2].user_id, returnName(joinPlayerList, 2)));
            SameMarker.Instance.addPlayer(new Player(4, joinPlayerList[3].email, false, false, false, false, GameManager.gm.fourthPlayerColor, joinPlayerList[3].user_id, returnName(joinPlayerList, 3)));

            objectSetActivie(firstPlayerNameTextPanel,true);
      objectSetActivie(secondPlayerNameTextPanel,true);
      objectSetActivie(thirdPlayerNameTextPanel,true);
      objectSetActivie(forthPlayerNameTextPanel,true);
// fast ludo
            showStepCount(true, true, true, true);

            //text1.text = joinPlayerList[0].first_name + " " + joinPlayerList[0].last_name;
            //text2.text = joinPlayerList[1].first_name + " " + joinPlayerList[1].last_name;
            //text3.text = joinPlayerList[2].first_name + " " + joinPlayerList[2].last_name;
            //text4.text = joinPlayerList[3].first_name + " " + joinPlayerList[3].last_name;

            text1.text = returnName(joinPlayerList, 0);
            text2.text = returnName(joinPlayerList, 1);
            text3.text = returnName(joinPlayerList, 2);
            text4.text = returnName(joinPlayerList, 3);
        }
// Fast LUDO
        SameMarker.Instance.updateCountStepsForOnlineGame();

  }

  public void onStartButtonClicked()
  {
        aPICall.startMatch();

  }


    public void identifyUserAssignColor()
    {
        for(int i=0; i < SameMarker.Instance.getJointPlayerArray().Count; i++)
        {
            if(Credentials.email == SameMarker.Instance.getJointPlayerArray()[i].email)
            {
                if(SameMarker.Instance.getJointPlayerArray().Count == 2)
                {
                    if (i == 1)
                    {
                        playerColor = colorArray[2];
                        GameManager.gm.player = i + 2;
                    }     
                    else
                    {
                        playerColor = colorArray[i];
                        GameManager.gm.player = i + 1;
                    }                       

                }
                else
                {
                    playerColor = colorArray[i];
                    GameManager.gm.player = i + 1;
                }
            }
                
        }
    }

    public int getSpriteAssignCount(string color)
    { 
        switch(color)
        {
            case "yellow": return 1;
            case "blue": return 2;
            case "red": return 3;
            case "green": return 4;
        }

        return 4;
    }

    public void updateMqttText(string msg)
  {
  //    mqttMessage.text = msg;
  }

       public void rotateBoard(int value)
    {
        float degrees = 0;

        mqttMessage.text = count.ToString();
        count++;

        if (value == 2)
        {
            degrees = 180;

            GameManager.gm.rotation = degrees;

        for (int i = 0; i < pathParent.yellowPlayerPieces.Length; i++)
        {
            pathParent.yellowPlayerPieces[i].transform.rotation = new Quaternion(0, 0, degrees, 0);
        }

        for (int i = 0; i < pathParent.redPlayerPieces.Length; i++)
        {
            pathParent.redPlayerPieces[i].transform.rotation = new Quaternion(0, 0, degrees, 0);
        }

        for (int i = 0; i < pathParent.greenPlayerPieces.Length; i++)
        {
            pathParent.greenPlayerPieces[i].transform.rotation = new Quaternion(0, 0, degrees, 0);
        }


        for (int i = 0; i < pathParent.bluePlayerPieces.Length; i++)
        {
            pathParent.bluePlayerPieces[i].transform.rotation = new Quaternion(0, 0, degrees, 0);
        }

        for (int i = 0; i < timer.Length; i++)
        {
            timer[i].transform.rotation = new Quaternion(0, 0, degrees, 0);
        }

        timerPanel.transform.rotation = new Quaternion(0, 0, degrees, 0);
         //fixed for step counts for fast LUDO
        for (int i = 0; i < playerStepsCount.Length; i++)
        {
            playerStepsCount[i].transform.rotation = new Quaternion(0, 0, degrees, 0);
             Debug.Log(">>>>>>>>>>>>>>>>>>>>>>>>>> Waiting for oppopnent -Fast Ludo ratate stepcount- is called");
        }
       
       
        for (int i = 0; i < playerName.Length; i++)
        {
            playerName[i].transform.rotation = new Quaternion(0, 0, degrees, 0);
        }

        PlayerNamePanel.transform.rotation = new Quaternion(0, 0, degrees, 0);


        for (int i = 0; i < botCountText.Length; i++)
        {
            botCountText[i].transform.rotation = new Quaternion(0, 0, degrees, 0);
        }

        botCountTextPanel.transform.rotation = new Quaternion(0, 0, degrees, 0);


        for (int i = 0; i < waitTimerText.Length; i++)
        {
           waitTimerText[i].transform.rotation = new Quaternion(0, 0, degrees, 0);
        }

        waitTimerTextPanel.transform.rotation = new Quaternion(0, 0, degrees, 0);


        diceHolderMainObject.transform.rotation = new Quaternion(0, 0, degrees, 0);
        board.transform.rotation = new Quaternion(0, 0, degrees, 0);

        }
        else if (value == 3)
        {
            degrees = 90;

            GameManager.gm.rotation = degrees;

            for (int i = 0; i < pathParent.yellowPlayerPieces.Length; i++)
            {
                pathParent.yellowPlayerPieces[i].transform.Rotate(0.0f, 0.0f, -degrees, Space.World);
            }

            for (int i = 0; i < pathParent.redPlayerPieces.Length; i++)
            {
                pathParent.redPlayerPieces[i].transform.Rotate(0.0f, 0.0f, -degrees, Space.World);
            }

            for (int i = 0; i < pathParent.greenPlayerPieces.Length; i++)
            {
                pathParent.greenPlayerPieces[i].transform.Rotate(0.0f, 0.0f, -degrees, Space.World);
            }

            for (int i = 0; i < pathParent.bluePlayerPieces.Length; i++)
            {
                pathParent.bluePlayerPieces[i].transform.Rotate(0.0f, 0.0f, -degrees, Space.World);
            }

            for (int i = 0; i < timer.Length; i++)
            {
                if( i%2 == 0)
                {
                    changePosition_Coroutine = StartCoroutine(ChangeObjectPosition(timer[i], timer[i].transform.localPosition.x * -1, timer[i].transform.localPosition.y));
                }
                else
                {
                    changePosition_Coroutine = StartCoroutine(ChangeObjectPosition(timer[i], timer[i].transform.localPosition.x, timer[i].transform.localPosition.y * -1));
                }
            }



            /* for (int i = 0; i < playerName.Length; i++)
             {
                 if (i % 2 == 0)
                 {
                     changePosition_Coroutine = StartCoroutine(ChangeObjectPosition(playerName[i], playerName[i].transform.localPosition.x * -1, timer[i].transform.localPosition.y));
                 }
                 else
                 {
                     changePosition_Coroutine = StartCoroutine(ChangeObjectPosition(playerName[i], playerName[i].transform.localPosition.x, playerName[i].transform.localPosition.y * -1));
                 }
             }*/

            changePosition_Coroutine = StartCoroutine(ChangeObjectPosition(playerName[0], playerName[0].transform.localPosition.x * -1, playerName[0].transform.localPosition.y));
            changePosition_Coroutine = StartCoroutine(ChangeObjectPosition(playerName[1], playerName[1].transform.localPosition.x, playerName[1].transform.localPosition.y * -1));
            changePosition_Coroutine = StartCoroutine(ChangeObjectPosition(playerName[2], playerName[2].transform.localPosition.x * -1, playerName[2].transform.localPosition.y));
            changePosition_Coroutine = StartCoroutine(ChangeObjectPosition(playerName[3], playerName[3].transform.localPosition.x, playerName[3].transform.localPosition.y * -1));

            // Fast ludo

            changePosition_Coroutine = StartCoroutine(ChangeObjectPosition(playerStepsCount[0], playerStepsCount[0].transform.localPosition.x * -1, playerStepsCount[0].transform.localPosition.y));
            changePosition_Coroutine = StartCoroutine(ChangeObjectPosition(playerStepsCount[1], playerStepsCount[1].transform.localPosition.x, playerStepsCount[1].transform.localPosition.y * -1));
            changePosition_Coroutine = StartCoroutine(ChangeObjectPosition(playerStepsCount[2], playerStepsCount[2].transform.localPosition.x * -1, playerStepsCount[2].transform.localPosition.y));
            changePosition_Coroutine = StartCoroutine(ChangeObjectPosition(playerStepsCount[3], playerName[3].transform.localPosition.x, playerStepsCount[3].transform.localPosition.y * -1));


            for (int i = 0; i < botCountText.Length; i++)
            {
                if (i % 2 == 0)
                {
                    changePosition_Coroutine = StartCoroutine(ChangeObjectPosition(botCountText[i], botCountText[i].transform.localPosition.x * -1, botCountText[i].transform.localPosition.y));
                }
                else
                {
                    changePosition_Coroutine = StartCoroutine(ChangeObjectPosition(botCountText[i], botCountText[i].transform.localPosition.x, botCountText[i].transform.localPosition.y * -1));
                }
            }

            for (int i = 0; i < pathParent.diceHolder.Length; i++)
            {

                if (i % 2 == 0)
                {
                    changePosition_Coroutine = StartCoroutine(ChangeObjectPosition(pathParent.diceHolder[i], pathParent.diceHolder[i].transform.localPosition.x * -1, pathParent.diceHolder[i].transform.localPosition.y));
                   // pathParent.diceHolder[i].transform.Rotate(0.0f, 180.0f, 0.0f, Space.World);
                }
                else
                {
                    changePosition_Coroutine = StartCoroutine(ChangeObjectPosition(pathParent.diceHolder[i], pathParent.diceHolder[i].transform.localPosition.x, pathParent.diceHolder[i].transform.localPosition.y * -1));
                }
            }

            board.transform.Rotate(0.0f, 0.0f, 90.0f, Space.World); 

        }
        else if (value == 1)
        {
            degrees = 270;

            GameManager.gm.rotation = degrees;

            for (int i = 0; i < pathParent.yellowPlayerPieces.Length; i++)
            {
                pathParent.yellowPlayerPieces[i].transform.Rotate(0.0f, 0.0f, -degrees, Space.World);
            }

            for (int i = 0; i < pathParent.redPlayerPieces.Length; i++)
            {
                pathParent.redPlayerPieces[i].transform.Rotate(0.0f, 0.0f, -degrees, Space.World);
            }

            for (int i = 0; i < pathParent.greenPlayerPieces.Length; i++)
            {
                pathParent.greenPlayerPieces[i].transform.Rotate(0.0f, 0.0f, -degrees, Space.World);
            }


            for (int i = 0; i < pathParent.bluePlayerPieces.Length; i++)
            {
                pathParent.bluePlayerPieces[i].transform.Rotate(0.0f, 0.0f, -degrees, Space.World);
            }

           /* for (int i = 0; i < playerName.Length; i++)
            {
                if (i % 2 == 0)
                {
                    changePosition_Coroutine = StartCoroutine(ChangeObjectPosition(playerName[i], playerName[i].transform.localPosition.x * -1, timer[i].transform.localPosition.y));
                }
                else
                {
                    changePosition_Coroutine = StartCoroutine(ChangeObjectPosition(playerName[i], playerName[i].transform.localPosition.x, playerName[i].transform.localPosition.y * -1));
                }
            }
           */

            changePosition_Coroutine = StartCoroutine(ChangeObjectPosition(playerName[0], playerName[0].transform.localPosition.x, playerName[0].transform.localPosition.y * -1));
            changePosition_Coroutine = StartCoroutine(ChangeObjectPosition(playerName[1], playerName[1].transform.localPosition.x * -1, playerName[1].transform.localPosition.y));
            changePosition_Coroutine = StartCoroutine(ChangeObjectPosition(playerName[2], playerName[2].transform.localPosition.x, playerName[2].transform.localPosition.y * - 1));
            changePosition_Coroutine = StartCoroutine(ChangeObjectPosition(playerName[3], playerName[3].transform.localPosition.x * -1, playerName[3].transform.localPosition.y));
            

            // fast ludo
            changePosition_Coroutine = StartCoroutine(ChangeObjectPosition(playerStepsCount[0], playerStepsCount[0].transform.localPosition.x, playerStepsCount[0].transform.localPosition.y * -1));
            changePosition_Coroutine = StartCoroutine(ChangeObjectPosition(playerStepsCount[1], playerStepsCount[1].transform.localPosition.x * -1, playerStepsCount[1].transform.localPosition.y));
            changePosition_Coroutine = StartCoroutine(ChangeObjectPosition(playerStepsCount[2], playerStepsCount[2].transform.localPosition.x, playerStepsCount[2].transform.localPosition.y * -1));
            changePosition_Coroutine = StartCoroutine(ChangeObjectPosition(playerStepsCount[3], playerStepsCount[3].transform.localPosition.x * -1, playerStepsCount[3].transform.localPosition.y));

                        
                        for (int i = 0; i < timer.Length; i++)
                        {
                            if (i % 2 == 0)
                            {
                                changePosition_Coroutine = StartCoroutine(ChangeObjectPosition(timer[i], timer[i].transform.localPosition.x, timer[i].transform.localPosition.y * -1));
                            }
                            else
                            {
                                changePosition_Coroutine = StartCoroutine(ChangeObjectPosition(timer[i], timer[i].transform.localPosition.x * -1, timer[i].transform.localPosition.y));
                            }
                        }


                        for (int i = 0; i < botCountText.Length; i++)
                        {
                            if (i % 2 == 0)
                            {
                                changePosition_Coroutine = StartCoroutine(ChangeObjectPosition(botCountText[i], botCountText[i].transform.localPosition.x, botCountText[i].transform.localPosition.y * -1));
                            }
                            else
                            {
                                changePosition_Coroutine = StartCoroutine(ChangeObjectPosition(botCountText[i], botCountText[i].transform.localPosition.x * -1, botCountText[i].transform.localPosition.y));
                            }
                        }

                        for (int i = 0; i < pathParent.diceHolder.Length; i++)
                        {

                            if (i % 2 == 0)
                            {
                                changePosition_Coroutine = StartCoroutine(ChangeObjectPosition(pathParent.diceHolder[i], pathParent.diceHolder[i].transform.localPosition.x, pathParent.diceHolder[i].transform.localPosition.y * -1));
                            }
                            else
                            {
                                changePosition_Coroutine = StartCoroutine(ChangeObjectPosition(pathParent.diceHolder[i], pathParent.diceHolder[i].transform.localPosition.x * -1, pathParent.diceHolder[i].transform.localPosition.y));
                                pathParent.diceHolder[i].transform.Rotate(0.0f, 180.0f, 0.0f, Space.World);
                }
            }

            board.transform.Rotate(0.0f, 0.0f, degrees, Space.World);

        }

        startGame();
    }

    IEnumerator ChangeObjectPosition(GameObject gameObject, float x, float y)
    {

        gameObject.transform.localPosition = new Vector3(x, y, 0);

        mqttMessage.text = playerName[0].transform.localPosition.x.ToString() + " : " + playerName[0].transform.localPosition.y.ToString();

        yield return new WaitForEndOfFrame();

    }

        IEnumerator ChangeCameraPosition(Vector3 vector)
    {

        Camera.main.transform.position = vector;

        yield return new WaitForEndOfFrame();

    }

    void startGame()
    {
        SoundManager.PlaySound("click");
        // spriteHandler.changeBoardSprite(4);
        GameManager.gm.isGameRunning = true;
        GameManager.gm.isOnlineGame = true;


        rollingDice.setVisibilityFirstPlayerDice(true);
        AddPlayerList();
        objectSetActivie(waitingForOpponentPanel, false);
        GameManager.gm.countDownStartValue = 20;
        CountDownTimer.Instance.startTimer();
        gameSettings.showExitButton();
        objectSetActivie(exitButton, true);
        objectSetActivie(redDiceHolder, true);
    }

    public void publishStartGame(string email)
    {
        // Store message Locally
        ServerPlayEvent playEvent1 = new ServerPlayEvent();
        playEvent1.action = "actionStart";
        playEvent1.sender = email;

        ServerMqttEvent mqttEvent1 = new ServerMqttEvent();
        mqttEvent1.playEvent = playEvent1;

        string json1 = JsonUtility.ToJson(mqttEvent1);

        print(json1);

        MqttMessageArray.Instance.addMessages(new MqMessage(json1));
        // end Store message Locally



        GameManager.gm.isFromIosGcApp = false;
        identifyUserAssignColor();
        rotateBoard(getSpriteAssignCount(playerColor));
// fast ludo timer
        gameTimer.StartTimer();
    }

    public void sucessfullyMatchStart()
    {
         // fast ludo timer
        gameTimer.StartTimer();
        System.TimeSpan ts = System.DateTime.UtcNow - startTime;


        //Credentials.action = "action : start";
        // m2MqttUnityTest.TestPublish();
        PlayEvent playEvent = new PlayEvent();
        playEvent.action = "actionStart";
        playEvent.sender = Credentials.email;
        playEvent.playerPieceEvent = null;
        playEvent.bot = null;

        MqttEvent mqttEvent = new MqttEvent();
        mqttEvent.playEvent = playEvent;

        string json = JsonUtility.ToJson(mqttEvent);

        //  mqttMessage.text = json;

        Credentials.action = json;
        m2MqttUnityTest.TestPublish();


        if (GameManager.gm.mqttIsConnected)
        {
            ServerPlayEvent playEvent1 = new ServerPlayEvent();
            playEvent1.action = "actionStart";
            playEvent1.sender = Credentials.email;

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

        GameManager.gm.isFromIosGcApp = false;
        identifyUserAssignColor();
        rotateBoard(getSpriteAssignCount(playerColor));
    }


    void setScreen()
    {
        int width = Screen.currentResolution.width;
        int height = Screen.currentResolution.height;

        if (width == 480 && height == 800)
        {
            //Camera.main.transform.position = new Vector3(0f, 0f, -7.7f);
            changeCameraPosition_Coroutine = StartCoroutine(ChangeCameraPosition(new Vector3(0f, 0f, -7.7f)));
        }
        else if (width == 720 && height == 1280)
        {
            //Camera.main.transform.position = new Vector3(0f, 0f, -8.3f);
            changeCameraPosition_Coroutine = StartCoroutine(ChangeCameraPosition(new Vector3(0f, 0f, -8.3f)));
            y_axis = 51f;
            dice_holder_position = 0.15f;
        }
        else if (width == 1080 && height == 1920)
        {
            // Camera.main.transform.position = new Vector3(0f, 0f, -8.3f);
            changeCameraPosition_Coroutine = StartCoroutine(ChangeCameraPosition(new Vector3(0f, 0f, -8.3f)));
            y_axis = 51f;
            dice_holder_position = 0.15f;
        }
        else if (width == 1080 && height == 2160)
        {
            // Camera.main.transform.position = new Vector3(0f, 0f, -9.3f);
            changeCameraPosition_Coroutine = StartCoroutine(ChangeCameraPosition(new Vector3(0f, 0f, -9.3f)));
            y_axis = 21f;
            dice_holder_position = 0.03f;
        }
        else if (width == 1440 && height == 2560)
        {
            // Camera.main.transform.position = new Vector3(0f, 0f, -8.3f);
            changeCameraPosition_Coroutine = StartCoroutine(ChangeCameraPosition(new Vector3(0f, 0f, -8.3f)));
            y_axis = 51f;
            dice_holder_position = 0.15f;
        }
        else if (width == 1440 && height == 2960)
        {
            // Camera.main.transform.position = new Vector3(0f, 0f, -9.6f);
            changeCameraPosition_Coroutine = StartCoroutine(ChangeCameraPosition(new Vector3(0f, 0f, -9.6f)));
            y_axis = 11f;
            dice_holder_position = 0.02f;
        }
        else if (width == 1080 && height == 2340)
        {
            //Camera.main.transform.position = new Vector3(0f, 0f, -10f);
            changeCameraPosition_Coroutine = StartCoroutine(ChangeCameraPosition(new Vector3(0f, 0f, -10f)));
            y_axis = 0f;
            dice_holder_position = 0f;
        }
        else if (width == 1080 && height == 2400)
        {
            //Camera.main.transform.position = new Vector3(0f, 0f, -10.3f);
            changeCameraPosition_Coroutine = StartCoroutine(ChangeCameraPosition(new Vector3(0f, 0f, -10.3f)));
            y_axis = 0f;
            dice_holder_position = -0.15f;
        }
        else
        {
            float screen_width = Screen.currentResolution.width;
            float screen_height = Screen.currentResolution.height;
            float ratio = screen_width / screen_height;
            int decimalLimit = 3;
            string screenRatio = ratio.ToString("F3");

            float ratio16_9 = 9f / 16f;
            string screen16_9 = ratio16_9.ToString("F3");
            float ratio18_9 = 9f / 18f;
            string screen18_9 = ratio18_9.ToString("F3");
            float ratio195_9 = 9f / 19.5f;
            string screen195_9 = ratio195_9.ToString("F3");
            float ratio20_9 = 9f / 20f;
            string screen20_9 = ratio20_9.ToString("F3");
            float ratio21_9 = 9f / 21f;
            string screen21_9 = ratio21_9.ToString("F3");
            float ratio19_85 = 8.5f / 19f;
            string screen19_85 = ratio19_85.ToString("F3");

            if (string.Equals(screenRatio, screen16_9))
            {
                changeCameraPosition_Coroutine = StartCoroutine(ChangeCameraPosition(new Vector3(0f, 0f, -8.2f)));
                y_axis = 53f;
                x_axis = 62f;
                dice_holder_position = 0.15f;
            }
            else if (string.Equals(screenRatio, screen18_9))
            {
                changeCameraPosition_Coroutine = StartCoroutine(ChangeCameraPosition(new Vector3(0f, 0f, -9.2f)));
                y_axis = 20f;
                x_axis = 27f;
                dice_holder_position = -0.05f;
            }
            else if (string.Equals(screenRatio, screen195_9))
            {
                changeCameraPosition_Coroutine = StartCoroutine(ChangeCameraPosition(new Vector3(0f, 0f, -10f)));
                y_axis = 0f;
                dice_holder_position = 0f;
            }
            else if (string.Equals(screenRatio, screen20_9))
            {
                changeCameraPosition_Coroutine = StartCoroutine(ChangeCameraPosition(new Vector3(0f, 0f, -10.22f)));
                y_axis = -5f;
                dice_holder_position = -0.06f;
            }
            else if (string.Equals(screenRatio, screen21_9))
            {
                changeCameraPosition_Coroutine = StartCoroutine(ChangeCameraPosition(new Vector3(0f, 0f, -10.8f)));
                y_axis = -19f;
                x_axis = -8f;
                dice_holder_position = -0.1f;
            }
            else if (string.Equals(screenRatio, screen19_85))
            {
                changeCameraPosition_Coroutine = StartCoroutine(ChangeCameraPosition(new Vector3(0f, 0f, -10.4f)));
                y_axis = -9f;
                dice_holder_position = -0.07f;
            }
            else
            {
                Debug.Log("Unknown or custom screen ratio");
            }


        }

        changePosition_Coroutine = StartCoroutine(ChangeObjectPosition(playerName[0], playerName[0].transform.localPosition.x - x_axis, playerName[0].transform.localPosition.y - y_axis));
        changePosition_Coroutine = StartCoroutine(ChangeObjectPosition(playerName[1], playerName[1].transform.localPosition.x - x_axis, playerName[1].transform.localPosition.y + y_axis));
        changePosition_Coroutine = StartCoroutine(ChangeObjectPosition(playerName[2], playerName[2].transform.localPosition.x + x_axis, playerName[2].transform.localPosition.y + y_axis));
        changePosition_Coroutine = StartCoroutine(ChangeObjectPosition(playerName[3], playerName[3].transform.localPosition.x + x_axis, playerName[3].transform.localPosition.y - y_axis));

// Fast ludo
        changePosition_Coroutine = StartCoroutine(ChangeObjectPosition(playerStepsCount[0], playerStepsCount[0].transform.localPosition.x, playerStepsCount[0].transform.localPosition.y - y_axis));
        changePosition_Coroutine = StartCoroutine(ChangeObjectPosition(playerStepsCount[1], playerStepsCount[1].transform.localPosition.x, playerStepsCount[1].transform.localPosition.y + y_axis));
        changePosition_Coroutine = StartCoroutine(ChangeObjectPosition(playerStepsCount[2], playerStepsCount[2].transform.localPosition.x, playerStepsCount[2].transform.localPosition.y + y_axis));
        changePosition_Coroutine = StartCoroutine(ChangeObjectPosition(playerStepsCount[3], playerStepsCount[3].transform.localPosition.x, playerStepsCount[3].transform.localPosition.y - y_axis));


        changePosition_Coroutine = StartCoroutine(ChangeObjectPosition(timer[0], timer[0].transform.localPosition.x - x_axis, timer[0].transform.localPosition.y - y_axis));
        changePosition_Coroutine = StartCoroutine(ChangeObjectPosition(timer[1], timer[1].transform.localPosition.x - x_axis, timer[1].transform.localPosition.y + y_axis));
        changePosition_Coroutine = StartCoroutine(ChangeObjectPosition(timer[2], timer[2].transform.localPosition.x + x_axis, timer[2].transform.localPosition.y + y_axis));
        changePosition_Coroutine = StartCoroutine(ChangeObjectPosition(timer[3], timer[3].transform.localPosition.x + x_axis, timer[3].transform.localPosition.y - y_axis));

        changePosition_Coroutine = StartCoroutine(ChangeObjectPosition(botCountText[0], botCountText[0].transform.localPosition.x - x_axis, botCountText[0].transform.localPosition.y - y_axis));
        changePosition_Coroutine = StartCoroutine(ChangeObjectPosition(botCountText[1], botCountText[1].transform.localPosition.x - x_axis, botCountText[1].transform.localPosition.y + y_axis));
        changePosition_Coroutine = StartCoroutine(ChangeObjectPosition(botCountText[2], botCountText[2].transform.localPosition.x + x_axis, botCountText[2].transform.localPosition.y + y_axis));
        changePosition_Coroutine = StartCoroutine(ChangeObjectPosition(botCountText[3], botCountText[3].transform.localPosition.x + x_axis, botCountText[3].transform.localPosition.y - y_axis));

        changePosition_Coroutine = StartCoroutine(ChangeObjectPosition(pathParent.diceHolder[0], pathParent.diceHolder[0].transform.localPosition.x - dice_holder_position, pathParent.diceHolder[0].transform.localPosition.y + dice_holder_position));
        changePosition_Coroutine = StartCoroutine(ChangeObjectPosition(pathParent.diceHolder[1], pathParent.diceHolder[1].transform.localPosition.x - dice_holder_position, pathParent.diceHolder[1].transform.localPosition.y - dice_holder_position));
        changePosition_Coroutine = StartCoroutine(ChangeObjectPosition(pathParent.diceHolder[2], pathParent.diceHolder[2].transform.localPosition.x + dice_holder_position, pathParent.diceHolder[2].transform.localPosition.y - dice_holder_position));
        changePosition_Coroutine = StartCoroutine(ChangeObjectPosition(pathParent.diceHolder[3], pathParent.diceHolder[3].transform.localPosition.x + dice_holder_position, pathParent.diceHolder[3].transform.localPosition.y + dice_holder_position));

        changePosition_Coroutine = StartCoroutine(ChangeObjectPosition(waitTimerText[0], waitTimerText[0].transform.localPosition.x, waitTimerText[0].transform.localPosition.y - y_axis));
        changePosition_Coroutine = StartCoroutine(ChangeObjectPosition(waitTimerText[1], waitTimerText[1].transform.localPosition.x, waitTimerText[1].transform.localPosition.y + y_axis));
    }

    public void connectMQ()
    {
        StartCoroutine(callIosMQTTConnection());
    }


    public IEnumerator callIosMQTTConnection()
    {
        if (!LoadingPanel.activeInHierarchy)
            LoadingPanel.SetActive(true);

        if (MqttMessageArray.Instance.getMessageList().Count > 0)
        {
            MqttMessageArray.Instance.clearList();
            MqttMessageArray.ClearInstance();
        }

        m2MqttUnityClient.Disconnect();
        string path1 = Application.persistentDataPath + "/loginUserPath.fun";
        bool isLogin = false;

        if (File.Exists(path1))
        {
            using (Stream stream = File.Open(path1, FileMode.Open))
            {
                var bformatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

                LoginUser loginUser = (LoginUser)bformatter.Deserialize(stream);

                if (loginUser != null)
                {

                    isLogin = loginUser.isLogin;
                    Credentials.email = loginUser.email;
                    GameManager.gm.loginUserName = loginUser.first_name;
                }
            }
        }

        yield return new WaitForSeconds(1);
        if (isLogin)
        {
            if(Credentials.id != null && Credentials.id != "")
            {
                objectSetActivie(LoginPanel, false);
                objectSetActivie(ExitPanel, false);
                objectSetActivie(MenuPanel, false);

                yield return new WaitForSeconds(1);
                m2MqttUnityClient.Connect();

            }
            else
            {
                PlayerPrefs.SetInt("playAsGuest", 0);
                close();  
            }
        }
        else
        {
            PlayerPrefs.SetInt("playAsGuest", 0);
            waitingForOpponentPanel.SetActive(false);
            LoginPanel.SetActive(true);
            PlayAsGuestPanel.SetActive(false);
            OrText.SetActive(false);
           // close();
        }

        if (LoadingPanel.activeInHierarchy)
            LoadingPanel.SetActive(false);
    }


    public string returnName(List<JoinPlayerInfo> joinPlayerList, int index)
    {
        string name = "";

        if (joinPlayerList[index].username != null && joinPlayerList[index].username != "")
            name = joinPlayerList[index].username;
        else if (joinPlayerList[index].last_name != null && joinPlayerList[index].last_name != "")
            name = joinPlayerList[index].first_name + " " + joinPlayerList[index].last_name;
        else if (joinPlayerList[index].first_name != null && joinPlayerList[index].first_name != "")
            name = joinPlayerList[index].first_name;
        else
            name = joinPlayerList[index].email;

        return name;
    }

    void CallbackWithBranchUniversalObject(BranchUniversalObject buo,
                                                BranchLinkProperties linkProps,
                                                string error)
    {
        if (error != null)
        {
            System.Console.WriteLine("Error : "
                                    + error);

            message.text = "Error : " + error;

            close();
        }
        else if (linkProps.controlParams.Count > 0)
        {
            System.Console.WriteLine("Deeplink params : "
                                    + buo.ToJsonString()
                                    + linkProps.ToJsonString());
            Dictionary<string, string> data = buo.metadata.GetCustomMetadata();
            
            Credentials.isBranchIOhasData = true;
          
             
            id1 = data["tokenID"];
            channel1 = "arenas/global/matches/" + id1;
            contestId1 = data["contest_id_gc"];
            userId1 = data["user_id_gc"];
            contestName1 = data["contest_name"];
            if(data["is_admin_contest"] != null && data["is_admin_contest"] != "")
            is_admin_contest1 = System.Convert.ToBoolean(data["is_admin_contest"]);

            if(data["contest_size"] != null)
                contestSize = data["contest_size"];

            // displayIntentData.text = id1;
            message.text = "contest id : " + contestId1 + " contest name : " + contestName1 + " Match Id : " + id1+" User Id : "+ userId1;
        }
    }

    public IEnumerator callBranchIoData()
    {
        yield return new WaitForSeconds(4);
        if (Credentials.isBranchIOhasData)
        {
            if(Credentials.id == null || Credentials.id == "")
            {
                Credentials.isBranchIOhasData = false;
                Credentials.channel = channel1;
                Credentials.id = id1;
                Credentials.contestId = contestId1;
                Credentials.userId = userId1;
                Credentials.contestName = contestName1;
                Credentials.isAdminContest = is_admin_contest1;
                Credentials.contestSize = contestSize;
            }
            

            message.text = Credentials.contestName;
            if (Credentials.id != null)
            {
                string path1 = Application.persistentDataPath + "/loginUserPath.fun";
                bool isLogin = false;

                if (File.Exists(path1))
                {
                    using (Stream stream = File.Open(path1, FileMode.Open))
                    {
                        var bformatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

                        LoginUser loginUser = (LoginUser)bformatter.Deserialize(stream);

                        if (loginUser != null)
                        {

                            isLogin = loginUser.isLogin;
                            Credentials.email = loginUser.email;
                            GameManager.gm.loginUserName = loginUser.first_name;
                            GameManager.gm.loginUsertoken = loginUser.token;
                        }
                    }
                }

                // GameManager.gm.flow2 = GameManager.gm.flow2 + "isLogin : "+ isLogin;
                if (isLogin)
                {
                    objectSetActivie(LoginPanel, false);
                    objectSetActivie(ExitPanel, false);
                    objectSetActivie(MenuPanel, false);

                    aPICall.callMatchStatus();
                    m2MqttUnityClient.Connect();

                    if (Credentials.isAdminContest)
                    {
                        objectSetActivie(StartButtonObj, false);
                    }
                    else
                    {
                        // objectSetActivie(StartButtonObj, true);
                        objectSetActivie(StartButtonObj, false);
                    }

                }
                else
                {
                    PlayerPrefs.SetInt("playAsGuest", 0);
                    PlayAsGuestPanel.SetActive(false);
                    OrText.SetActive(false);
                    close();
                }

                objectSetActivie(LoadingPanel, false);
            }
            else
            {
                PlayAsGuestPanel.SetActive(true);
                OrText.SetActive(true);
                close();
                objectSetActivie(LoadingPanel, false);
            }
        }
        else
        {
            objectSetActivie(LoadingPanel, false);
            close();
        }
    }

}
