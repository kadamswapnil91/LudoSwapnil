using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;
using UnityEngine.UI;
using System.Linq;
using TMPro;


public class CountDownTimer : Singleton<CountDownTimer>
{

    private static TextMeshProUGUI timer1, botCountText1, botCountText2, botCountText3, botCountText4;
    private static int countDownStartValue;

    private WinnerResult winnerResult;
    private ExitManager exitManager;
    private RollingDice rollingDice;
    public GameObject cdtImage1, cdtImage2, cdtImage3, cdtImage4;
    Image fillImage1, fillImage2, fillImage3, fillImage4;
    float time;
    bool isTimerOn = false;
    TextMeshProUGUI mqttMessage;

    Button playerBot1, playerBot2, playerBot3, playerBot4;
    M2MqttUnity.Examples.M2MqttUnityTest m2MqttUnityTest;

    APICall aPICall;


    [SerializeField] GameObject botPlayedCountText1, botPlayedCountText2, botPlayedCountText3, botPlayedCountText4;

    [Header("Bot On and Off")]
    public Sprite OffSprite1;
    public Sprite OffSprite2;
    public Sprite OffSprite3;
    public Sprite OffSprite4;
    public Sprite OnSprite;
    public Sprite botLightSprite;
    public Sprite botDarkSprite;

    void Update()
    {
            if (GameManager.gm.isStartTimer && !winnerResult.isShown() && !exitManager.isShown() && !SameMarker.Instance.getPlayerArray()[getIndex()].isAI)
            {
                if (time > 0)
                {
                    time -= Time.deltaTime;
                    showRadicalTimer(time, GameManager.gm.playerTurn);
                }
                else
                {
                    hideAllRadicalImage();
                }
            }
    }

    void Start()
    {
        fillImage1 = cdtImage1.GetComponent<Image>();
        fillImage2 = cdtImage2.GetComponent<Image>();
        fillImage3 = cdtImage3.GetComponent<Image>();
        fillImage4 = cdtImage4.GetComponent<Image>();

        //timer1 = GameObject.Find("Canvas/Timer1").GetComponent<TextMeshProUGUI>();
        botCountText1 = botPlayedCountText1.GetComponent<TextMeshProUGUI>();
        botCountText2 = botPlayedCountText2.GetComponent<TextMeshProUGUI>();
        botCountText3 = botPlayedCountText3.GetComponent<TextMeshProUGUI>();
        botCountText4 = botPlayedCountText4.GetComponent<TextMeshProUGUI>();

        playerBot1 = GameObject.Find("Canvas/TimerPanels/GreenPanel/BotToUserButton1").GetComponent<Button>();
        playerBot2 = GameObject.Find("Canvas/TimerPanels/RedPanel/BotToUserButton2").GetComponent<Button>();
        playerBot3 = GameObject.Find("Canvas/TimerPanels/BluePanel/BotToUserButton3").GetComponent<Button>();
        playerBot4 = GameObject.Find("Canvas/TimerPanels/YellowPanel/BotToUserButton4").GetComponent<Button>();

        playerBot1.onClick.AddListener(BotToUser1);
        playerBot2.onClick.AddListener(BotToUser2);
        playerBot3.onClick.AddListener(BotToUser3);
        playerBot4.onClick.AddListener(BotToUser4);

        objectSetActivie(botPlayedCountText1, false);
        objectSetActivie(botPlayedCountText2, false);
        objectSetActivie(botPlayedCountText3, false);
        objectSetActivie(botPlayedCountText4, false);

        mqttMessage = GameObject.Find("Canvas/Text (TMP)").GetComponent<TextMeshProUGUI>();
    }

    private void Awake()
    {
        winnerResult = FindObjectOfType<WinnerResult>();
        exitManager = FindObjectOfType<ExitManager>();
        rollingDice = FindObjectOfType<RollingDice>();
        m2MqttUnityTest = FindObjectOfType<M2MqttUnity.Examples.M2MqttUnityTest>();
        aPICall = FindObjectOfType<APICall>();
    }


    public void startTimer()
    {
        CancelInvoke();

        if (!SameMarker.Instance.getPlayerArray()[getIndex()].isAI)
        {
            isTimerOn = true;
            if (GameManager.gm.isStartTimer && !winnerResult.isShown())
            {
                countDownStartValue = GameManager.gm.countDownStartValue;
                time = (float)countDownStartValue;
            }
            else
            {
                //Fast Ludo 20 sec changed
                //countDownStartValue = 20;
                //time = 20f;
                countDownStartValue = 15;
                time = 15f;
            }

            GameManager.gm.isStartTimer = true;
            countDownTimer();
        }
        else
        {
            isTimerOn = false;
            hideAllRadicalImage();
            //timer1.text = "";
            CancelInvoke();
        }
    }


    private void countDownTimer()
    {
        if (GameManager.gm.isStartTimer && !winnerResult.isShown() && !exitManager.isShown())
        {
            if (countDownStartValue > 0)
            {
               
                    GameManager.gm.countDownStartValue = countDownStartValue;
                    // timer1.text = countDownStartValue.ToString();
                    countDownStartValue--;
                    //float val1 = (float)countDownStartValue / (float)20;
                    //fillImage.fillAmount = val1;
                    Invoke("countDownTimer", 1.0f);
                    isTimerOn = true;
                    if (countDownStartValue < 10)
                    {
                        SoundManager.PlaySound("lessTime");
                    }

                    if(GameManager.gm.isOnlineGame)
                    {
                        if(!GameManager.gm.mqttIsConnected)
                        {
                            if (countDownStartValue == 1)
                            {
                                GameManager.gm.isStartTimer = false;
                                MqttIsDisconnected.quitTimerIsRunning = true;
                            }

                        }
                    }
                
            }
            else
            {

                if (GameManager.gm.isOnlineGame)
                {
                    if (SameMarker.Instance.getPlayerArray()[getIndex()].player_name == Credentials.email)
                    {
                        isTimerOn = false;
                        // hideAllRadicalImage();
                        SameMarker.Instance.getPlayerArray()[getIndex()].isAI = true;
                        SameMarker.Instance.getPlayerArray()[getIndex()].isConvertedToBot = true;
                        publishMessage(getIndex() + 1, true, "actionConvertBot");
                        userToBot(getIndex());
                        GameManager.gm.isStartTimer = false;
                        CancelInvoke();

                        GameManager.gm.isSaveGame = true;
                        if (GameManager.gm.numberOfStepsToMove > 0)
                        {
                            rollingDice.loadSaveGame();
                        }
                    }

                }
                else
                {
                    //  timer1.text = 0.ToString();
                    isTimerOn = false;
                    // hideAllRadicalImage();
                    SameMarker.Instance.getPlayerArray()[getIndex()].isAI = true;
                    SameMarker.Instance.getPlayerArray()[getIndex()].isConvertedToBot = true;
                    userToBot(getIndex());
                    GameManager.gm.isStartTimer = false;
                    CancelInvoke();

                    GameManager.gm.isSaveGame = true;
                    if (GameManager.gm.numberOfStepsToMove > 0)
                    {
                        rollingDice.loadSaveGame();
                    }
                }

            }

        }
        else
        {
            isTimerOn = false;
            //hideAllRadicalImage();
            if (exitManager.isShown())
            {
                GameManager.gm.countDownStartValue = countDownStartValue;
            }
            else
            {
                if (GameManager.gm.isOnlineGame)
                {
                    if (!GameManager.gm.mqttIsConnected)
                    {
                        GameManager.gm.countDownStartValue = countDownStartValue;
                    }
                    else
                    {
                       // countDownStartValue = 20;
                       countDownStartValue = 15;
                        GameManager.gm.countDownStartValue = countDownStartValue;
                    }
                }
                else
                {
                  //  countDownStartValue = 20;
                  countDownStartValue = 15;
                    GameManager.gm.countDownStartValue = countDownStartValue;

                }
                
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

    public void BotToUser1()
    {
        if (GameManager.gm.isOnlineGame)
        {
            if (GameManager.gm.player == 1)
            {
                if (playerBot1.image.sprite == OnSprite)
                {
                    resetTimer();
                    SameMarker.Instance.getPlayerArray()[0].isAI = false;
                    SameMarker.Instance.getPlayerArray()[0].isConvertedToBot = false;
                    playerBot1.image.sprite = OffSprite1;
                    if (GameManager.gm.playerTurn == 1)
                        startTimer();
                    publishMessage(1, false, "actionConvertPlayer");
                }
            }
        }
        else
        {
            if (playerBot1.image.sprite == OnSprite)
            {
                SameMarker.Instance.getPlayerArray()[0].isAI = false;
                SameMarker.Instance.getPlayerArray()[0].isConvertedToBot = false;
                playerBot1.image.sprite = OffSprite1;
               // resetTimer();
                startTimer();
            }
        }


    }

    public void BotToUser2()
    {
        if (GameManager.gm.isOnlineGame)
        {
            if (GameManager.gm.player == 2)
            {
                if (playerBot2.image.sprite == OnSprite)
                {
                    SameMarker.Instance.getPlayerArray()[1].isAI = false;
                    SameMarker.Instance.getPlayerArray()[1].isConvertedToBot = false;
                    playerBot2.image.sprite = OffSprite2;
                    resetTimer();
                    if (GameManager.gm.playerTurn == 2)
                        startTimer();
                    publishMessage(2, false, "actionConvertPlayer");
                }


            }
        }
        else
        {
            if (playerBot2.image.sprite == OnSprite)
            {
                SameMarker.Instance.getPlayerArray()[1].isAI = false;
                SameMarker.Instance.getPlayerArray()[1].isConvertedToBot = false;
                playerBot2.image.sprite = OffSprite2;
               // resetTimer();
                startTimer();
            }
        }
    }

    public void BotToUser3()
    {
        if (GameManager.gm.isOnlineGame)
        {
            if (GameManager.gm.player == 3)
            {
                if (playerBot3.image.sprite == OnSprite)
                {
                    if (SameMarker.Instance.getPlayerArray().Count == 2)
                    {
                        SameMarker.Instance.getPlayerArray()[1].isAI = false;
                        SameMarker.Instance.getPlayerArray()[1].isConvertedToBot = false;
                    }
                    else
                    {
                        SameMarker.Instance.getPlayerArray()[2].isAI = false;
                        SameMarker.Instance.getPlayerArray()[2].isConvertedToBot = false;
                    }

                    playerBot3.image.sprite = OffSprite3;
                    resetTimer();
                    if (GameManager.gm.playerTurn == 3)
                        startTimer();

                    if (SameMarker.Instance.getPlayerArray().Count == 2)
                        publishMessage(2, false, "actionConvertPlayer");
                    else
                        publishMessage(3, false, "actionConvertPlayer");
                }
            }
        }
        else
        {
            if (playerBot3.image.sprite == OnSprite)
            {
                if (SameMarker.Instance.getPlayerArray().Count == 2)
                {
                    SameMarker.Instance.getPlayerArray()[1].isAI = false;
                    SameMarker.Instance.getPlayerArray()[1].isConvertedToBot = false;
                }
                else
                {
                    SameMarker.Instance.getPlayerArray()[2].isAI = false;
                    SameMarker.Instance.getPlayerArray()[2].isConvertedToBot = false;
                }

                playerBot3.image.sprite = OffSprite3;
               // resetTimer();
                startTimer();
            }
        }
    }

    public void BotToUser4()
    {
        if (GameManager.gm.isOnlineGame)
        {
            if (GameManager.gm.player == 4)
            {
                if (playerBot4.image.sprite == OnSprite)
                {
                    SameMarker.Instance.getPlayerArray()[3].isAI = false;
                    SameMarker.Instance.getPlayerArray()[3].isConvertedToBot = false;
                    playerBot4.image.sprite = OffSprite4;
                    resetTimer();
                    if (GameManager.gm.playerTurn == 4)
                        startTimer();
                    publishMessage(4, false, "actionConvertPlayer");
                }
            }
        }
        else
        {
            if (playerBot4.image.sprite == OnSprite)
            {
                SameMarker.Instance.getPlayerArray()[3].isAI = false;
                SameMarker.Instance.getPlayerArray()[3].isConvertedToBot = false;
                playerBot4.image.sprite = OffSprite4;
               // resetTimer();
                startTimer();
            }
        }
    }

    public void userToBot(int i)
    {
        if (GameManager.gm.isDarkMode)
            OnSprite = botDarkSprite;
        else
            OnSprite = botLightSprite;

        switch (i)
        {
            case 0:
                playerBot1.image.sprite = OnSprite;
                break;

            case 1:
                if (SameMarker.Instance.getPlayerArray().Count == 2)
                    playerBot3.image.sprite = OnSprite;
                else
                    playerBot2.image.sprite = OnSprite;
                break;

            case 2:
                playerBot3.image.sprite = OnSprite;
                break;

            case 3:
                playerBot4.image.sprite = OnSprite;
                break;
        }
    }


    public void publishUserToBot(int i)
    {
        if (GameManager.gm.isDarkMode)
            OnSprite = botDarkSprite;
        else
            OnSprite = botLightSprite;

        switch (i)
        {
            case 0:
                playerBot1.image.sprite = OnSprite;
                SameMarker.Instance.getPlayerArray()[0].isAI = true;
                SameMarker.Instance.getPlayerArray()[0].isConvertedToBot = true;

                GameManager.gm.isStartTimer = false;
                CancelInvoke();

                GameManager.gm.isSaveGame = true;
                if (GameManager.gm.numberOfStepsToMove > 0)
                {
                    rollingDice.loadSaveGame();
                }
                break;

            case 1:
                if (SameMarker.Instance.getPlayerArray().Count == 2)
                {
                    playerBot3.image.sprite = OnSprite;
                }
                else
                {
                    playerBot2.image.sprite = OnSprite;
                }
                SameMarker.Instance.getPlayerArray()[1].isAI = true;
                SameMarker.Instance.getPlayerArray()[1].isConvertedToBot = true;


                GameManager.gm.isStartTimer = false;
                CancelInvoke();

                GameManager.gm.isSaveGame = true;
                if (GameManager.gm.numberOfStepsToMove > 0)
                {
                    rollingDice.loadSaveGame();
                }

                break;

            case 2:
                playerBot3.image.sprite = OnSprite;
                SameMarker.Instance.getPlayerArray()[2].isAI = true;
                SameMarker.Instance.getPlayerArray()[2].isConvertedToBot = true;

                GameManager.gm.isStartTimer = false;
                CancelInvoke();

                GameManager.gm.isSaveGame = true;
                if (GameManager.gm.numberOfStepsToMove > 0)
                {
                    rollingDice.loadSaveGame();
                }
                break;

            case 3:
                playerBot4.image.sprite = OnSprite;
                SameMarker.Instance.getPlayerArray()[3].isAI = true;
                SameMarker.Instance.getPlayerArray()[3].isConvertedToBot = true;

                GameManager.gm.isStartTimer = false;
                CancelInvoke();

                GameManager.gm.isSaveGame = true;
                if (GameManager.gm.numberOfStepsToMove > 0)
                {
                    rollingDice.loadSaveGame();
                }
                break;
        }
    }

    public void displayBotPlayedCount()
    {
        try
        {
            if (GameManager.gm.botCount1 == 0 && !SameMarker.Instance.getPlayerArray()[0].isQuit)
            {
                objectSetActivie(botPlayedCountText1, false);
            }
            else
            {
                objectSetActivie(botPlayedCountText1, true);

                if (GameManager.gm.botCount1 == 5 || SameMarker.Instance.getPlayerArray()[0].isQuit)
                    botCountText1.text = "Quit The Game";
                else
                    botCountText1.text = "Bot Played : " + GameManager.gm.botCount1.ToString();
            }

            if(SameMarker.Instance.getPlayerArray().Count == 3 || SameMarker.Instance.getPlayerArray().Count == 4)
            {
                if (GameManager.gm.botCount2 == 0 && !SameMarker.Instance.getPlayerArray()[1].isQuit)
                {
                    objectSetActivie(botPlayedCountText2, false);
                }
                else
                {
                    objectSetActivie(botPlayedCountText2, true);
                    if (GameManager.gm.botCount2 == 5 || SameMarker.Instance.getPlayerArray()[1].isQuit)
                        botCountText2.text = "Quit The Game";
                    else
                        botCountText2.text = "Bot Played : " + GameManager.gm.botCount2.ToString();
                }
            }


            if (SameMarker.Instance.getPlayerArray().Count == 2)
            {
                if (GameManager.gm.botCount3 == 0 && !SameMarker.Instance.getPlayerArray()[1].isQuit)
                {
                    objectSetActivie(botPlayedCountText3, false);
                }
                else
                {
                    objectSetActivie(botPlayedCountText3, true);
                    if (GameManager.gm.botCount3 == 5 || SameMarker.Instance.getPlayerArray()[1].isQuit)
                        botCountText3.text = "Quit The Game";
                    else
                        botCountText3.text = "Bot Played : " + GameManager.gm.botCount3.ToString();
                }
            }
            else
            {
                if (GameManager.gm.botCount3 == 0 && !SameMarker.Instance.getPlayerArray()[2].isQuit)
                {
                    objectSetActivie(botPlayedCountText3, false);
                }
                else
                {
                    objectSetActivie(botPlayedCountText3, true);
                    if (GameManager.gm.botCount3 == 5 || SameMarker.Instance.getPlayerArray()[2].isQuit)
                        botCountText3.text = "Quit The Game";
                    else
                        botCountText3.text = "Bot Played : " + GameManager.gm.botCount3.ToString();
                }
            }

            if(SameMarker.Instance.getPlayerArray().Count == 4)
            {
                if (GameManager.gm.botCount4 == 0 && !SameMarker.Instance.getPlayerArray()[3].isQuit)
                {
                    objectSetActivie(botPlayedCountText4, false);
                }
                else
                {
                    objectSetActivie(botPlayedCountText4, true);
                    if (GameManager.gm.botCount4 == 5 || SameMarker.Instance.getPlayerArray()[3].isQuit)
                        botCountText4.text = "Quit The Game";
                    else
                        botCountText4.text = "Bot Played : " + GameManager.gm.botCount4.ToString();
                }

            }
        }
        catch(Exception e)
        {
            Debug.Log(e.Message);
        }


    }

    void objectSetActivie(GameObject gameObject, bool status)
    {
        if (gameObject)
        {
            gameObject.SetActive(status);
        }
    }

    void showRadicalTimer(float time, int playerTurn)
    {
        switch (playerTurn)
        {
            case 1:
               /* fillImage2.fillAmount = 20f;
                fillImage3.fillAmount = 20f;
                fillImage4.fillAmount = 20f;
                fillImage1.fillAmount = time / 20f;*/
                 fillImage2.fillAmount = 15f;
                fillImage3.fillAmount = 15f;
                fillImage4.fillAmount = 15f;
                fillImage1.fillAmount = time / 15f;
                break;

            case 2:
               /* fillImage1.fillAmount = 20f;
                fillImage3.fillAmount = 20f;
                fillImage4.fillAmount = 20f;
                fillImage2 = cdtImage2.GetComponent<Image>();
                fillImage2.fillAmount = time / 20f;*/
                fillImage1.fillAmount = 15f;
                fillImage3.fillAmount = 15f;
                fillImage4.fillAmount = 15f;
                fillImage2 = cdtImage2.GetComponent<Image>();
                fillImage2.fillAmount = time / 15f;
                break;

            case 3:
             /*   fillImage1.fillAmount = 20f;
                fillImage2.fillAmount = 20f;
                fillImage4.fillAmount = 20f;
                fillImage3 = cdtImage3.GetComponent<Image>();
                fillImage3.fillAmount = time / 20f; */
                fillImage1.fillAmount = 15f;
                fillImage2.fillAmount = 15f;
                fillImage4.fillAmount = 15f;
                fillImage3 = cdtImage3.GetComponent<Image>();
                fillImage3.fillAmount = time / 15f;
                break;

            case 4:
                /*objectSetActivie(cdtImage1,false);
                objectSetActivie(cdtImage2,false);
                objectSetActivie(cdtImage3,false);
                objectSetActivie(cdtImage4,true);
                print(isAv(cdtImage4).ToString());*/
             /*   fillImage1.fillAmount = 20f;
                fillImage2.fillAmount = 20f;
                fillImage3.fillAmount = 20f;
                fillImage4 = cdtImage4.GetComponent<Image>();
                fillImage4.fillAmount = time / 20f;*/
                fillImage1.fillAmount = 15f;
                fillImage2.fillAmount = 15f;
                fillImage3.fillAmount = 15f;
                fillImage4 = cdtImage4.GetComponent<Image>();
                fillImage4.fillAmount = time / 15f;
                break;

        }
    }

    void hideAllRadicalImage()
    {
      /*  fillImage1.fillAmount = 20f;
        fillImage2.fillAmount = 20f;
        fillImage3.fillAmount = 20f;
        fillImage4.fillAmount = 20f; */
        fillImage1.fillAmount = 15f;
        fillImage2.fillAmount = 15f;
        fillImage3.fillAmount = 15f;
        fillImage4.fillAmount = 15f;
    }

    bool isAv(GameObject gameObject)
    {
        if (gameObject)
        {
            return true;
        }
        return false;
    }


    void publishMessage(int playerId, bool status, string action)
    {

        Bot bot = new Bot();
        bot.playerId = playerId;
        bot.isBot = status;

        PlayEvent playEvent = new PlayEvent();
        playEvent.action = action;
        playEvent.sender = Credentials.email;
        playEvent.playerPieceEvent = null;
        playEvent.bot = bot;

        MqttEvent mqttEvent = new MqttEvent();
        mqttEvent.playEvent = playEvent;

        string json = JsonUtility.ToJson(mqttEvent);

        Credentials.action = json;
        m2MqttUnityTest.TestPublish();

        //store on local and server

       if (GameManager.gm.mqttIsConnected)
        {
            Bot bot1 = new Bot();
            bot1.playerId = playerId;
            bot1.isBot = status;

            ServerPlayEvent playEvent1 = new ServerPlayEvent();
            playEvent1.action = action;
            playEvent1.sender = Credentials.email;
            playEvent1.turn_id = GameManager.gm.turn_id;
            playEvent1.playerPieceEvent = null;
            playEvent1.bot = bot1;

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

    }


    public void PublishBotToUser1()
    {
        if (playerBot1.image.sprite == OnSprite)
        {
            SameMarker.Instance.getPlayerArray()[0].isAI = false;
            SameMarker.Instance.getPlayerArray()[0].isConvertedToBot = false;
            playerBot1.image.sprite = OffSprite1;
            startTimer();
        }
    }

    public void PublishBotToUser2()
    {
        if (playerBot2.image.sprite == OnSprite)
        {
            SameMarker.Instance.getPlayerArray()[1].isAI = false;
            SameMarker.Instance.getPlayerArray()[1].isConvertedToBot = false;
            playerBot2.image.sprite = OffSprite2;
            startTimer();
        }
    }

    public void PublishBotToUser3()
    {
        if (playerBot3.image.sprite == OnSprite)
        {
            if (SameMarker.Instance.getPlayerArray().Count == 2)
            {
                SameMarker.Instance.getPlayerArray()[1].isAI = false;
                SameMarker.Instance.getPlayerArray()[1].isConvertedToBot = false;
            }
            else
            {
                SameMarker.Instance.getPlayerArray()[2].isAI = false;
                SameMarker.Instance.getPlayerArray()[2].isConvertedToBot = false;
            }

            playerBot3.image.sprite = OffSprite3;
            startTimer();
        }
    }

    public void PublishBotToUser4()
    {
        if (playerBot4.image.sprite == OnSprite)
        {
            SameMarker.Instance.getPlayerArray()[3].isAI = false;
            SameMarker.Instance.getPlayerArray()[3].isConvertedToBot = false;
            playerBot4.image.sprite = OffSprite4;
            startTimer();
        }
    }

    void resetTimer()
    {
        GameManager.gm.isStartTimer = false;
      //  countDownStartValue = 20;
      countDownStartValue = 15;
        time = 20f;
    }


}
