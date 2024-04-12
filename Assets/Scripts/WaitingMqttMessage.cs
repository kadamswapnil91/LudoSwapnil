using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitingMqttMessage : MonoBehaviour
{
    public static float timeRemaining = 22;
    public static bool timerIsRunning = false;

    public static float quitTimeRemaining = 22;
    public static bool quitTimerIsRunning = false;

    [SerializeField] GameObject WaitingMqttMessagePanel;
    WinnerResult winnerResult;

    QuitGameScript quitGameScript;

    MqttIsDisconnected mqttIsDisconnected;

    M2MqttUnity.Examples.M2MqttUnityTest m2MqttUnityTest;
    APICall aPICall;


    private void Start()
    {
        // Starts the timer automatically
        timeRemaining = 22;
        timerIsRunning = true;
    }

    private void Awake()
    {
        quitGameScript = FindObjectOfType<QuitGameScript>();
        winnerResult = FindObjectOfType<WinnerResult>();
        mqttIsDisconnected = FindObjectOfType<MqttIsDisconnected>();
        aPICall = FindObjectOfType<APICall>();
        m2MqttUnityTest = FindObjectOfType<M2MqttUnity.Examples.M2MqttUnityTest>();
    }


    // Update is called once per frame
    void Update()
    {
        if(GameManager.gm.isOnlineGame && !winnerResult.isShown() && !mqttIsDisconnected.isShown())
        {
            if(SameMarker.Instance.getPlayerArray()[getIndex()].player_name != Credentials.email)
            {

                if (timerIsRunning)
                {
                    if (timeRemaining > 0)
                    {
                        if (WaitingMqttMessagePanel)
                            WaitingMqttMessagePanel.SetActive(false);
                        timeRemaining -= Time.deltaTime;

                        quitTimerIsRunning = false;
                        quitTimeRemaining = 22;
                    }
                    else
                    { 
                        timeRemaining = 0;
                        timerIsRunning = false;
                        if (WaitingMqttMessagePanel)
                            WaitingMqttMessagePanel.SetActive(true);

                        quitTimerIsRunning = true;
                    }
                }

                if(quitTimerIsRunning)
                {
                    if(quitTimeRemaining > 0)
                    {
                        quitTimeRemaining -= Time.deltaTime;
                    }
                    else
                    {
                        if (WaitingMqttMessagePanel)
                            WaitingMqttMessagePanel.SetActive(false);

                        quitTimeRemaining = 0;
                        quitTimerIsRunning = false;
                        publishQuitMessage(GameManager.gm.playerTurn);
                        quitGameScript.quitGame(GameManager.gm.playerTurn);

                        timeRemaining = 22;
                        timerIsRunning = true;
                    }
                }
            } 
            else
            {
                if (WaitingMqttMessagePanel)
                    WaitingMqttMessagePanel.SetActive(false);

                quitTimerIsRunning = false;
                quitTimeRemaining = 22;

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

    void publishQuitMessage(int player)
    {
        QuitPlayer quitPlayer = new QuitPlayer();
        quitPlayer.player = player;

        PlayEvent playEvent = new PlayEvent();
        playEvent.action = "actionQuitPlayer";
        playEvent.sender = Credentials.email;
        playEvent.quitPlayer = quitPlayer;
        playEvent.playerPieceEvent = null;
        playEvent.bot = null;


        MqttEvent mqttEvent = new MqttEvent();
        mqttEvent.playEvent = playEvent;

        string json = JsonUtility.ToJson(mqttEvent);
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
}
