using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using SimpleJSON;
using TMPro;
using System.Globalization;

public class ActionHandler : MonoBehaviour
{

	WaitingForOpponent waitingForOpponent;

    [SerializeField]
    public TextMeshProUGUI status;

    [SerializeField]
    public Text msg;

    //[SerializeField]
    //public Button demoButton;

    [SerializeField]
    public TextMeshProUGUI messages,mqttStatus,print_msg;

    // Rolling Dice objects
    GreenRollingDice greenRollingDice;
    RedRollingDice redRollingDice;
    BlueRollingDice blueRollingDice;
    YellowRollingDice yellowRollingDice;
    PathObjectsParent pathParent;
    PlayerPiece playerPiece;
    RollingDice rollingDice;
    PlayerTurnManager playerTurnManager;
    QuitGameScript quitGameScript;
    WinnerResult winnerResult;
    APICall aPICall;
    WaitingForOpponentCountDownTimer waitingForOpponentCountDownTimer;



    // Start is called before the first frame update
    void Start()
    {
      //  demoButton.onClick.AddListener(lastMaqqMessages);
    }

    private void Update()
    {
        //status.text = MqttMessageArray.Instance.getMessageList().Count.ToString()+" : "+GameManager.gm.isBackground.ToString();
        //status.text = "Local Storage Count : "+ MqttMessageArray.Instance.getMessageList().Count.ToString() + "   List Count: "+ GameManager.gm.flow1.ToString();
       // status.text = GameManager.gm.flow2 +"\n"+ MqttMessageArray.Instance.getMessageList().Count.ToString();
    }

    void Awake()
    {
        waitingForOpponent = FindObjectOfType<WaitingForOpponent>();
        greenRollingDice = FindObjectOfType<GreenRollingDice>();
        redRollingDice = FindObjectOfType<RedRollingDice>();
        blueRollingDice = FindObjectOfType<BlueRollingDice>();
        yellowRollingDice = FindObjectOfType<YellowRollingDice>();
        pathParent = FindObjectOfType<PathObjectsParent>();
        playerPiece = FindObjectOfType<PlayerPiece>();
        rollingDice = FindObjectOfType<RollingDice>();
        playerTurnManager = FindObjectOfType<PlayerTurnManager>();
        quitGameScript = FindObjectOfType<QuitGameScript>();
        winnerResult = FindObjectOfType<WinnerResult>();
        aPICall = FindObjectOfType<APICall>();
        waitingForOpponentCountDownTimer = FindObjectOfType<WaitingForOpponentCountDownTimer>();
    }

    public void action(string actionMsg, JSONNode pokeInfo)
    {
        JSONNode jsonInfo = pokeInfo["playEvent"];
        JSONNode info = pokeInfo["playEvent"]["playerPieceEvent"];
        JSONNode botInfo = pokeInfo["playEvent"]["bot"];
        JSONNode ackInfo = pokeInfo["playEvent"]["ack"];
        JSONNode quitPlayerInfo = pokeInfo["playEvent"]["quitPlayer"];
        JSONNode winnerPlayerInfo = pokeInfo["playEvent"]["winner"];

        WaitingMqttMessage.timeRemaining = 22f;
        WaitingMqttMessage.timerIsRunning = true;

        if (actionMsg == "actionMoveGreenPiece" || actionMsg == "actionMoveRedPiece" || actionMsg == "actionMoveBluePiece" || actionMsg == "actionMoveYellowPiece")
        {
            GameManager.gm.numberOfStepsToMove = info["dice_value"];
            GameManager.gm.isReadyToMove = true;

            GameManager.gm.playerTurn = info["playerId"];

            if (info["player_chance"] != null)
                GameManager.gm.playerChance = info["player_chance"];

            if (info["sixCount"] != null)
                GameManager.gm.sixCount = info["sixCount"];
        }

        if (actionMsg == "actionStart" || actionMsg == "eventGameStart")
        {
            var waitingForOpponentGameObject = waitingForOpponent.getPanelObject();
            if(actionMsg == "actionStart")
            {
                if (waitingForOpponentGameObject.activeInHierarchy)
                    waitingForOpponent.publishStartGame(jsonInfo["sender"].ToString());
                messages.text = "action : " + actionMsg + "  ,Sender : " + jsonInfo["sender"].ToString();
            }else
            {
                //aPICall.startMatch();
                //if (waitingForOpponentGameObject.activeInHierarchy)
                //    waitingForOpponent.publishStartGame("Admin");
                List<JoinPlayerInfo> joinPlayerList = SameMarker.Instance.getJointPlayerArray();

                if (joinPlayerList.Count > 0)
                {
                    waitingForOpponentCountDownTimer.startTimer(System.DateTime.UtcNow.ToString(), joinPlayerList[0].email);
                    GameManager.gm.isStartCountDownTimer = true;
                }

                messages.text = "action : " + actionMsg + "  ,Sender : " + "Admin";
            }
            
        }

        if (GameManager.gm.isOnlineGame)
        {
            switch (actionMsg)
            {

                case "actionGreenRollDice":
                    if (pathParent.diceHolder[0].activeInHierarchy)
                        greenRollingDice.OnPublishMouseDown(jsonInfo["numberGot"]);
                    messages.text = "action : " + actionMsg + "  ,Sender : " + jsonInfo["sender"].ToString() + "  ,Number Got : " + jsonInfo["numberGot"].ToString();
                    break;

                case "actionRedRollDice":
                    if (pathParent.diceHolder[1].activeInHierarchy)
                        redRollingDice.OnPublishMouseDown(jsonInfo["numberGot"]);
                    messages.text = "action : " + actionMsg + "  ,Sender : " + jsonInfo["sender"].ToString() + "  ,Number Got : " + jsonInfo["numberGot"].ToString();
                    break;

                case "actionBlueRollDice":
                    if (pathParent.diceHolder[2].activeInHierarchy)
                        blueRollingDice.OnPublishMouseDown(jsonInfo["numberGot"]);
                    messages.text = "action : " + actionMsg + "  ,Sender : " + jsonInfo["sender"].ToString() + "  ,Number Got : " + jsonInfo["numberGot"].ToString();
                    break;

                case "actionYellowRollDice":
                    if (pathParent.diceHolder[3].activeInHierarchy)
                        yellowRollingDice.OnPublishMouseDown(jsonInfo["numberGot"]);
                    messages.text = "action : " + actionMsg + "  ,Sender : " + jsonInfo["sender"].ToString() + "  ,Number Got : " + jsonInfo["numberGot"].ToString();
                    break;

                case "actionMoveGreenPiece":
                    if (info != null)
                    {
                        GreenMovePlayerPiece(pathParent.greenPieces[info["pos"] - 8], info["playerId"], info["figureId"], info["pos"]);
                    }
                    messages.text = "action : " + actionMsg + "  ,Sender : " + jsonInfo["sender"].ToString() + "  ,Player Id : " + info["playerId"].ToString() + "  ,Figure Id : " + info["figureId"].ToString() + "  ,Position : " + info["pos"].ToString();

                    break;

                case "actionMoveYellowPiece":
                    if (info != null)
                    {
                        YellowMovePlayerPiece(pathParent.yellowPieces[info["pos"]], info["playerId"], info["figureId"], info["pos"]);
                    }
                    messages.text = "action : " + "actionMoveGreenPiece" + "  ,Sender : " + jsonInfo["sender"].ToString() + "  ,Player Id : " + info["playerId"].ToString() + "  ,Figure Id : " + info["figureId"].ToString() + "  ,Position : " + info["pos"].ToString();
                    break;

                case "actionMoveBluePiece":
                    if (info != null)
                    {
                        BlueMovePlayerPiece(pathParent.bluePieces[info["pos"] - 12], info["playerId"], info["figureId"], info["pos"]);
                    }
                    messages.text = "action : " + actionMsg + "  ,Sender : " + jsonInfo["sender"].ToString() + "  ,Player Id : " + info["playerId"].ToString() + "  ,Figure Id : " + info["figureId"].ToString() + "  ,Position : " + info["pos"].ToString();
                    break;

                case "actionMoveRedPiece":
                    if (info != null)
                    {
                        RedMovePlayerPiece(pathParent.redPieces[info["pos"] - 4], info["playerId"], info["figureId"], info["pos"]);
                    }
                    messages.text = "action : " + "actionMoveBluePiece" + "  ,Sender : " + jsonInfo["sender"].ToString() + "  ,Player Id : " + info["playerId"].ToString() + "  ,Figure Id : " + info["figureId"].ToString() + "" + info["pos"].ToString();
                    break;

                case "actionConvertBot":
                    if (botInfo != null)
                    {
                        if (botInfo["isBot"])
                            CountDownTimer.Instance.publishUserToBot(botInfo["playerId"] - 1);

                        storeBotMessagetoLocalServer(botInfo["playerId"], actionMsg, jsonInfo["sender"].ToString(), botInfo["isBot"]);
                    }
                    messages.text = "action : " + actionMsg + "  ,Sender : " + jsonInfo["sender"].ToString() + "  ,Is Bot: " + botInfo["isBot"].ToString() + "  ,Player Id : " + botInfo["playerId"].ToString();
                    break;

                case "actionConvertPlayer":
                    if (botInfo != null)
                    {
                        if (!botInfo["isBot"])
                        {
                            int id = botInfo["playerId"];
                            switch (id)
                            {
                                case 1:
                                    CountDownTimer.Instance.PublishBotToUser1();
                                    break;

                                case 2:
                                    if (SameMarker.Instance.getPlayerArray().Count == 2)
                                        CountDownTimer.Instance.PublishBotToUser3();
                                    else
                                        CountDownTimer.Instance.PublishBotToUser2();
                                    break;

                                case 3:
                                    CountDownTimer.Instance.PublishBotToUser3();
                                    break;

                                case 4:
                                    CountDownTimer.Instance.PublishBotToUser4();
                                    break;
                            }
                        }
                        storeBotMessagetoLocalServer(botInfo["playerId"], actionMsg, jsonInfo["sender"].ToString(), botInfo["isBot"]);
                        //store on local and server
                        messages.text = "action : " + actionMsg + "  ,Sender : " + jsonInfo["sender"].ToString() + "  ,Is Bot: " + botInfo["isBot"].ToString() + "  ,Player Id : " + botInfo["playerId"].ToString();

                    }
                    break;

                case "actionAcknowledgement":
                    if (ackInfo != null)
                    {
                        mqttStatus.text = ackInfo["timeStamp"].ToString();

                        try
                        {
                            System.DateTime startTime = System.DateTime.ParseExact(ackInfo["timeStamp"], "MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                            System.TimeSpan ts = System.DateTime.UtcNow - startTime;

                            GameManager.gm.isStartTimer = true;
                            GameManager.gm.countDownStartValue = (20 - Mathf.RoundToInt((float)ts.TotalSeconds));

                        }
                        catch (Exception e)
                        {
                            try
                            {
                                System.DateTime startTime = System.DateTime.ParseExact(ackInfo["timeStamp"], "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                                System.TimeSpan ts = System.DateTime.UtcNow - startTime;

                                GameManager.gm.isStartTimer = true;
                                GameManager.gm.countDownStartValue = (20 - Mathf.RoundToInt((float)ts.TotalSeconds));
                            }
                            catch (Exception error)
                            {
                                GameManager.gm.isStartTimer = true;
                                GameManager.gm.countDownStartValue = 20;
                            }


                        }

                        if (GameManager.gm.isGameRunning)
                            CountDownTimer.Instance.startTimer();

                    }
                    break;

                case "actionChangeUserTurnFromPlayerPiece":
                    playerTurnManager.playerPiecePlayerTurn(jsonInfo["sender"].ToString());
                    messages.text = "action : " + actionMsg + "  ,Sender : " + jsonInfo["sender"].ToString();
                    break;


                case "actionChangeUserTurnFromRollingDice":
                    playerTurnManager.changePlayerTurn(jsonInfo["sender"].ToString());
                    messages.text = "action : " + actionMsg + "  ,Sender : " + jsonInfo["sender"].ToString();
                    break;

                case "actionChangeUserTurnFromRollingDice1":
                    playerTurnManager.changePlayerTurn1(jsonInfo["sender"].ToString());
                    messages.text = "action : " + actionMsg + "  ,Sender : " + jsonInfo["sender"].ToString();
                    break;

                case "actionBotPlayMove":
                    rollingDice.publishBotPlay();
                    messages.text = "action : " + actionMsg + "  ,Sender : " + jsonInfo["sender"].ToString();
                    break;

                case "actionQuitPlayer":
                    if (quitPlayerInfo["player"] != 0)
                    {
                        messages.text = "action : " + actionMsg + "  ,Sender : " + jsonInfo["sender"].ToString() + "  ,Quit Player Id : " + quitPlayerInfo["player"].ToString();
                        quitGameScript.quitGame(quitPlayerInfo["player"]);

                        QuitPlayer quitPlayer = new QuitPlayer();
                        quitPlayer.player = quitPlayerInfo["player"];

                        ServerPlayEvent playEvent1 = new ServerPlayEvent();
                        playEvent1.action = "actionQuitPlayer";
                        playEvent1.sender = Credentials.email;
                        playEvent1.turn_id = GameManager.gm.turn_id;
                        playEvent1.quitPlayer = quitPlayer;

                        ServerMqttEvent mqttEvent1 = new ServerMqttEvent();
                        mqttEvent1.playEvent = playEvent1;

                        string json1 = JsonUtility.ToJson(mqttEvent1);

                        MqttMessageArray.Instance.addMessages(new MqMessage(json1));
                    }
                    break;

                case "actionWinner":
                    if (winnerPlayerInfo != null)
                    {
                        SameMarker.Instance.getPlayerArray()[winnerPlayerInfo["playerId"] - 1].isWinner = true;
                        SameMarker.Instance.addResult(new Results(winnerPlayerInfo["playerId"], winnerPlayerInfo["playerName"], winnerPlayerInfo["user_id"]));

                        for (int i = 0; i < SameMarker.Instance.getPlayerArray().Count; i++)
                        {
                            if (SameMarker.Instance.getPlayerArray()[i].player_id != winnerPlayerInfo["playerId"])
                            {
                                print("Result Player Id : " + SameMarker.Instance.getPlayerArray()[i].player_id);
                                SameMarker.Instance.addResult(new Results(SameMarker.Instance.getPlayerArray()[i].player_id, SameMarker.Instance.getPlayerName(SameMarker.Instance.getPlayerArray()[i].player_id), SameMarker.Instance.getPlayerArray()[i].user_id));
                            }
                        }

                        winnerResult.showWinnerList(true);


                        messages.text = "action : " + actionMsg + "  ,Sender : " + jsonInfo["sender"].ToString() + "  ,Player Id : " + winnerPlayerInfo["playerId"].ToString() + "  ,Player Name : " + winnerPlayerInfo["playerName"];

                    }
                    break;

                case "actionMoveSingleMarkerAutomatically":
                    GameManager.gm.numberOfStepsToMove = info["dice_value"];
                    GameManager.gm.isReadyToMove = true;
                    GameManager.gm.playerChance = info["player_chance"];
                    GameManager.gm.sixCount = info["sixCount"];
                    rollingDice.publishMoveSingleMarkerAutomatically();
                    messages.text = "action : " + actionMsg + "  ,Sender : " + jsonInfo["sender"].ToString();
                    break;

                case "actionUnlockFromHouse":
                    GameManager.gm.numberOfStepsToMove = 6;
                    GameManager.gm.isReadyToMove = true;
                    GameManager.gm.playerChance = info["player_chance"];
                    GameManager.gm.sixCount = info["sixCount"];
                    rollingDice.publishUnlockFromHouse();
                    messages.text = "action : " + actionMsg + "  ,Sender : " + jsonInfo["sender"].ToString();
                    break;

            }

        }

    }


    void GreenMovePlayerPiece(GreenPlayerPieces playerPiece, int playerId, int figureId, int pos)
    {
        playerPiece.OnPublishMouseDown1(playerId, figureId, pos);
    }

    void RedMovePlayerPiece(RedPlayerPieces playerPiece, int playerId, int figureId, int pos)
    {
        playerPiece.OnPublishMouseDown1(playerId, figureId, pos);
    }

    void BlueMovePlayerPiece(BluePlayerPieces playerPiece, int playerId, int figureId, int pos)
    {
        playerPiece.OnPublishMouseDown1(playerId, figureId, pos);
    }

    void YellowMovePlayerPiece(YellowPlayerPieces playerPiece, int playerId, int figureId, int pos)
    {
        playerPiece.OnPublishMouseDown1(playerId, figureId, pos);
    }


    void storeBotMessagetoLocalServer(int playerId, string action, string email, bool status)
    {
        //store on local and server

            Bot bot1 = new Bot();
            bot1.playerId = playerId;
            bot1.isBot = status;

            ServerPlayEvent playEvent1 = new ServerPlayEvent();
            playEvent1.action = action;
            playEvent1.sender = email;
            playEvent1.playerPieceEvent = null;
            playEvent1.bot = bot1;

            ServerMqttEvent mqttEvent1 = new ServerMqttEvent();
            mqttEvent1.playEvent = playEvent1;

            string json1 = JsonUtility.ToJson(mqttEvent1);

            MqttMessageArray.Instance.addMessages(new MqMessage(json1));

    }

    void lastMaqqMessages()
    {
        GameManager.gm.flow2 = GameManager.gm.flow2.ToString()+ (MqttMessageArray.Instance.getMessageList().Count - 1).ToString() + (MqttMessageArray.Instance.getMessageList().Count - 6).ToString() + "\n";
        for (int i = MqttMessageArray.Instance.getMessageList().Count -1; i > MqttMessageArray.Instance.getMessageList().Count - 6; i--)
        {
            JSONNode pokeInfo = JSON.Parse(MqttMessageArray.Instance.getList()[i].message);
            JSONNode jsonInfo = pokeInfo["playEvent"];

            if (jsonInfo != null)
            {
                string action = pokeInfo["playEvent"]["action"];
                GameManager.gm.flow2 = GameManager.gm.flow2.ToString() + "\n" + action.ToString();
            }
        }
    }
}
