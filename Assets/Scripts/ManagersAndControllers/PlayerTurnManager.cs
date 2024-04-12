using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerTurnManager : MonoBehaviour
{
    public ExitManager exitManager;
    private CountDownTimer countDownTimer;
    private GameSettings gameSettings;
    public WinnerResult winnerResult;
    WaitingTimer waitingTimer;
    public PlayerPiece playerPiece;
    PathObjectsParent pathParent;
    public M2MqttUnity.Examples.M2MqttUnityTest m2MqttUnityTest;

    [SerializeField] TextMeshProUGUI mqttMessage;

    Coroutine changePlayerTurn_Coroutine;

    Coroutine generateRandNumOnDice_Coroutine;



    private void Awake()
    {
        Application.runInBackground = true;
        winnerResult = FindObjectOfType<WinnerResult>();
        playerPiece = FindObjectOfType<PlayerPiece>();
        pathParent = FindObjectOfType<PathObjectsParent>();
        exitManager = FindObjectOfType<ExitManager>();
        countDownTimer = FindObjectOfType<CountDownTimer>();
        gameSettings = FindObjectOfType<GameSettings>();
        m2MqttUnityTest = FindObjectOfType<M2MqttUnity.Examples.M2MqttUnityTest>();
        waitingTimer = FindObjectOfType<WaitingTimer>();
    }


    IEnumerator userTurn()
    {

        yield return new WaitForSeconds(0.25f);

        int prvTurn = GameManager.gm.playerTurn;

        if (GameManager.gm.canDiceRoll)
            GameManager.gm.playerTurn = checkPlayerTurn();

        int newTurn = GameManager.gm.playerTurn;

        bool isChangeTurn = prvTurn != newTurn;

        displayDiceHolder();

        updateBotPlayCount(prvTurn, isChangeTurn);

        if (!exitManager.isShown())
        {
            if (!winnerResult.isShown())
            {
                if (GameManager.gm.isAIPlayed)
                    GameManager.gm.isAIPlayed = false;
            }
        }

        /*GameManager.gm.isStartTimer = false;
        GameManager.gm.countDownStartValue = 20;
        if (GameManager.gm.isGameRunning)
            CountDownTimer.Instance.startTimer();*/


        if (!GameManager.gm.isOnlineGame)
        {
            GameManager.gm.isStartTimer = false;
            GameManager.gm.countDownStartValue = 20;
            if (GameManager.gm.isGameRunning)
                CountDownTimer.Instance.startTimer();
        }
        else
        {
            // mqttMessage.text = "Player Turn 0: " + SameMarker.Instance.getPlayerArray()[getIndex()].player_name + " Login Player : " + Credentials.email;

            if (SameMarker.Instance.getPlayerArray()[getIndex()].player_name == Credentials.email)
            {
                yield return new WaitForSeconds(1f);

                Acknowledgement ack = new Acknowledgement();
                ack.timeStamp = System.DateTime.Now.ToString();
                ack.messageId = System.DateTime.UtcNow.Millisecond;
                ack.reciver = Credentials.email;

                PlayEvent ackEvent = new PlayEvent();
                ackEvent.action = "actionAcknowledgement";
                ackEvent.sender = Credentials.email;
                ackEvent.ack = ack;

                MqttEvent mqttEvent = new MqttEvent();
                mqttEvent.playEvent = ackEvent;

                string json = JsonUtility.ToJson(mqttEvent);

                Credentials.action = json;
                m2MqttUnityTest.TestPublish();

                GameManager.gm.isStartTimer = false;
                GameManager.gm.countDownStartValue = 20;
                if (GameManager.gm.isGameRunning)
                    CountDownTimer.Instance.startTimer();

            }

        }

        yield return new WaitForEndOfFrame();


        if (generateRandNumOnDice_Coroutine != null)
        {
            StopCoroutine(generateRandNumOnDice_Coroutine);
        }

    }


    IEnumerator userTurn1()
    {

        yield return new WaitForSeconds(0.25f);

        // mqttMessage.text = GameManager.gm.playerTurn.ToString();

        int prvTurn = GameManager.gm.playerTurn;

        print(GameManager.gm.playerChance);

        GameManager.gm.canDiceRoll = true;
        if (GameManager.gm.canDiceRoll)
            GameManager.gm.playerTurn = checkPlayerTurn();

        int newTurn = GameManager.gm.playerTurn;

        bool isChangeTurn = prvTurn != newTurn;

        displayDiceHolder();


        updateBotPlayCount(prvTurn, isChangeTurn);

        if (!exitManager.isShown())
        {
            if (!winnerResult.isShown())
            {
                if (GameManager.gm.isAIPlayed)
                    GameManager.gm.isAIPlayed = false;
            }
        }

        /* GameManager.gm.isStartTimer = false;
                GameManager.gm.countDownStartValue = 20;
                if (GameManager.gm.isGameRunning)
                    CountDownTimer.Instance.startTimer();*/


        if (!GameManager.gm.isOnlineGame)
        {
            GameManager.gm.isStartTimer = false;
            GameManager.gm.countDownStartValue = 20;
            if (GameManager.gm.isGameRunning)
                CountDownTimer.Instance.startTimer();
        }
        else
        {

            if (SameMarker.Instance.getPlayerArray()[getIndex()].player_name == Credentials.email)
            {
                Acknowledgement ack = new Acknowledgement();
                ack.timeStamp = System.DateTime.UtcNow.ToString();
                ack.messageId = System.DateTime.UtcNow.Millisecond;
                ack.reciver = Credentials.email;

                PlayEvent ackEvent = new PlayEvent();
                ackEvent.action = "actionAcknowledgement";
                ackEvent.sender = Credentials.email;
                ackEvent.ack = ack;

                MqttEvent mqttEvent = new MqttEvent();
                mqttEvent.playEvent = ackEvent;

                string json = JsonUtility.ToJson(mqttEvent);

                Credentials.action = json;
                m2MqttUnityTest.TestPublish();

                GameManager.gm.isStartTimer = false;
                GameManager.gm.countDownStartValue = 20;
                if (GameManager.gm.isGameRunning)
                    CountDownTimer.Instance.startTimer();

            }

        }


        yield break;

    }



    // Check Player turn

    int checkPlayerTurn()
    {
        int count;
        int dice;
        bool isKilled;

        if (GameManager.gm.playerTurn == null || GameManager.gm.playerTurn == 0)
        {
            count = 1;
            print("Player Turn 1 : " + count);
            return count;

        }
        else
        {

            count = GameManager.gm.playerTurn;

        }

        if (GameManager.gm.dice == null)
        {
            dice = 0;
        }
        else
        {
            dice = GameManager.gm.dice;
        }

        if (GameManager.gm.isKilled == null)
        {
            isKilled = false;
        }
        else
        {
            isKilled = GameManager.gm.isKilled;
        }


        // If six count is grater than 2

        if (GameManager.gm.playerChance == 1 && GameManager.gm.sixCount > 2)
        {
            GameManager.gm.playerChance = 1;

            if (SameMarker.Instance.getPlayerArray().Count == 2 && count != 0)
            {
                count = count + 2;

                if (count > 4)
                {
                    count = 1;
                }
                GameManager.gm.sixCount = 0;
                print("Player Turn 2 : " + count);
                return count;
            }

            count++;

            if (count > SameMarker.Instance.getPlayerArray().Count)
            {
                count = 1;
            }


            if (SameMarker.Instance.checkIsItWinner(count) || SameMarker.Instance.isPlayerQuit(count))
            {
                count++;

                if (count > SameMarker.Instance.getPlayerArray().Count)
                {
                    count = 1;
                }

                if (SameMarker.Instance.checkIsItWinner(count) || SameMarker.Instance.isPlayerQuit(count))
                {
                    count++;
                }

                if (count > SameMarker.Instance.getPlayerArray().Count)
                {
                    count = 1;
                }
            }
            GameManager.gm.sixCount = 0;

            print("Player Turn 3 : " + count);

            return count;

        }



        // If player chance is 0
        if (GameManager.gm.playerChance == 0)
        {
            GameManager.gm.playerChance = 1;

            if (SameMarker.Instance.getPlayerArray().Count == 2 && count != 0)
            {
                count = count + 2;

                if (count > 4)
                {
                    count = 1;
                }
                GameManager.gm.sixCount = 0;
                print("Player Turn 4 : " + count);
                return count;
            }

            count++;

            if (count > SameMarker.Instance.getPlayerArray().Count)
            {
                count = 1;
            }



            if (SameMarker.Instance.checkIsItWinner(count) || SameMarker.Instance.isPlayerQuit(count))
            {
                count++;


                if (count > SameMarker.Instance.getPlayerArray().Count)
                {
                    count = 1;
                }

                if (SameMarker.Instance.checkIsItWinner(count) || SameMarker.Instance.isPlayerQuit(count))
                {
                    count++;
                }


                if (count > SameMarker.Instance.getPlayerArray().Count)
                {
                    count = 1;
                }


            }
            GameManager.gm.sixCount = 0;

        }

        print("Player Turn 5 : " + count);

        return count;


    }


    public void displayDiceHolder()
    {
        for (int i = 0; i < pathParent.diceHolder.Length; i++)
        {
            if (i == (GameManager.gm.playerTurn - 1))
            {
                if (pathParent.diceHolder[i])
                {
                    pathParent.diceHolder[i].SetActive(true);
                }
            }
            else
            {
                if (pathParent.diceHolder[i])
                {
                    pathParent.diceHolder[i].SetActive(false);
                }
            }
        }
    }


    void updateBotPlayCount(int prvTurn, bool isChangeTurn)
    {
        if (prvTurn == 1 && isChangeTurn)
        {
            if (SameMarker.Instance.getPlayerArray()[0].isConvertedToBot)
            {
                GameManager.gm.botCount1++;

                if (GameManager.gm.botCount1 > 4)
                    quitPlayer(1);
            }

        }
        else if (prvTurn == 2 && isChangeTurn)
        {
            if (SameMarker.Instance.getPlayerArray()[1].isConvertedToBot)
            {
                GameManager.gm.botCount2++;

                if (GameManager.gm.botCount2 > 4)
                    quitPlayer(2);
            }

        }
        else if (prvTurn == 3 && isChangeTurn)
        {
            if (SameMarker.Instance.getPlayerArray().Count == 2)
            {
                if (SameMarker.Instance.getPlayerArray()[1].isConvertedToBot)
                {
                    GameManager.gm.botCount3++;

                    if (GameManager.gm.botCount3 > 4)
                        quitPlayer(3);
                }
            }
            else
            {
                if (SameMarker.Instance.getPlayerArray()[2].isConvertedToBot)
                {
                    GameManager.gm.botCount3++;

                    if (GameManager.gm.botCount3 > 4)
                        quitPlayer(3);
                }
            }
        }
        else if (prvTurn == 4 && isChangeTurn)
        {
            if (SameMarker.Instance.getPlayerArray()[3].isConvertedToBot)
            {
                GameManager.gm.botCount4++;

                if (GameManager.gm.botCount4 > 4)
                    quitPlayer(4);
            }

        }

        countDownTimer.displayBotPlayedCount();

    }

    void quitPlayer(int prvTurn)
    {

        if (prvTurn == 1)
        {
            SameMarker.Instance.getPlayerArray()[0].isQuit = true;

            for (int i = 0; i < 4; i++)
            {
                playerPiece.setInitialState(SameMarker.Instance.getArray()[i].player_id, SameMarker.Instance.getArray()[i].figure_id, true);
            }

            for (int i = 0; i < pathParent.yellowPlayerPieces.Length; i++)
            {
                objectSetActivie(pathParent.yellowPlayerPieces[i]);
            }
        }
        else if (prvTurn == 2)
        {
            SameMarker.Instance.getPlayerArray()[1].isQuit = true;

            for (int i = 8; i < 12; i++)
            {
                playerPiece.setInitialState(SameMarker.Instance.getArray()[i].player_id, SameMarker.Instance.getArray()[i].figure_id, true);
            }

            for (int i = 0; i < pathParent.greenPlayerPieces.Length; i++)
            {
                objectSetActivie(pathParent.greenPlayerPieces[i]);
            }
        }
        else if (prvTurn == 3)
        {
            if (SameMarker.Instance.getPlayerArray().Count == 2)
                SameMarker.Instance.getPlayerArray()[1].isQuit = true;
            else
                SameMarker.Instance.getPlayerArray()[2].isQuit = true;

            for (int i = 4; i < 8; i++)
            {
                playerPiece.setInitialState(SameMarker.Instance.getArray()[i].player_id, SameMarker.Instance.getArray()[i].figure_id, true);
            }

            for (int i = 0; i < pathParent.redPlayerPieces.Length; i++)
            {
                objectSetActivie(pathParent.redPlayerPieces[i]);
            }

        }
        else if (prvTurn == 4)
        {
            SameMarker.Instance.getPlayerArray()[3].isQuit = true;
            for (int i = 12; i < 16; i++)
            {
                playerPiece.setInitialState(SameMarker.Instance.getArray()[i].player_id, SameMarker.Instance.getArray()[i].figure_id, true);
            }

            for (int i = 0; i < pathParent.bluePlayerPieces.Length; i++)
            {
                objectSetActivie(pathParent.bluePlayerPieces[i]);
            }

        }

        if (isNotContinuableGame())
        {

            List<Player> quitPlayerList = SameMarker.Instance.getQuitPlayerArray();

            SameMarker.Instance.addResult(new Results(SameMarker.Instance.getRemainingPlayerArray()[0].player_id, SameMarker.Instance.getRemainingPlayerArray()[0].username, SameMarker.Instance.getRemainingPlayerArray()[0].user_id));

            for (int i = 0; i < quitPlayerList.Count; i++)
            {
                // SameMarker.Instance.addResult(new Results(quitPlayerList[i].player_name + " (Quit Game)"));
                SameMarker.Instance.addResult(new Results(quitPlayerList[i].player_id, quitPlayerList[i].username, quitPlayerList[i].user_id));
            }

            winnerResult.showWinnerList(true);
            GameManager.gm.canDiceRoll = false;
        }

    }

    void objectSetActivie(GameObject gameObject)
    {
        if (gameObject)
        {
            gameObject.SetActive(false);
        }
    }

    public bool isNotContinuableGame()
    {
        int remainingPlayerToPlay = SameMarker.Instance.getPlayerArray().Count - SameMarker.Instance.getResultArray().Count - SameMarker.Instance.getQuitPlayerCount();

        return remainingPlayerToPlay == 1;
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

    public void changePlayerTurn1(string email)
    {
        /*---------------------------------------Local Storage---------------------------------------*/
        if (SameMarker.Instance.getPlayerArray()[getIndex()].player_name != Credentials.email)
        {
            ServerPlayEvent playEvent1 = new ServerPlayEvent();
            playEvent1.action = "actionChangeUserTurnFromRollingDice1";
            playEvent1.sender = email;
            playEvent1.turn_id = GameManager.gm.turn_id;

            ServerMqttEvent mqttEvent1 = new ServerMqttEvent();
            mqttEvent1.playEvent = playEvent1;

            string json1 = JsonUtility.ToJson(mqttEvent1);

            print(json1);

            MqttMessageArray.Instance.addMessages(new MqMessage(json1));
        }

        /*---------------------------------------Finish Local Storage---------------------------------------*/

        changePlayerTurn_Coroutine = StartCoroutine(userTurn1());
    }

    public void changePlayerTurn(string email)
    {
        /*---------------------------------------Local Storage---------------------------------------*/
        if (SameMarker.Instance.getPlayerArray()[getIndex()].player_name != Credentials.email)
        {
            ServerPlayEvent playEvent1 = new ServerPlayEvent();
            playEvent1.action = "actionChangeUserTurnFromRollingDice";
            playEvent1.sender = email;
            playEvent1.turn_id = GameManager.gm.turn_id;

            ServerMqttEvent mqttEvent1 = new ServerMqttEvent();
            mqttEvent1.playEvent = playEvent1;

            string json1 = JsonUtility.ToJson(mqttEvent1);

            print(json1);

            MqttMessageArray.Instance.addMessages(new MqMessage(json1));
        }

        /*---------------------------------------Finish Local Storage---------------------------------------*/

        changePlayerTurn_Coroutine = StartCoroutine(userTurn());
    }

    public void quitChangePlayerTurn()
    {
        GameManager.gm.isStartTimer = false;
        GameManager.gm.canDiceRoll = true;
        GameManager.gm.playerChance = 0;
        GameManager.gm.numberOfStepsToMove = 0;
        changePlayerTurn_Coroutine = StartCoroutine(userTurn());
    }

    public void playerPiecePlayerTurn(string email)
    {
        /*---------------------------------------Local Storage and Server API call---------------------------------------*/
        if (SameMarker.Instance.getPlayerArray()[getIndex()].player_name != Credentials.email)
        {
            ServerPlayEvent playEvent1 = new ServerPlayEvent();
            playEvent1.action = "actionChangeUserTurnFromPlayerPiece";
            playEvent1.sender = email;
            playEvent1.turn_id = GameManager.gm.turn_id;

            ServerMqttEvent mqttEvent1 = new ServerMqttEvent();
            mqttEvent1.playEvent = playEvent1;

            string json1 = JsonUtility.ToJson(mqttEvent1);

            print(json1);

            if (!MqttMessageArray.Instance.getMessageList()[MqttMessageArray.Instance.getMessageList().Count - 1].message.Equals(json1))
                MqttMessageArray.Instance.addMessages(new MqMessage(json1));

        }

        /*---------------------------------------Local Storage---------------------------------------*/

        GameManager.gm.isStartTimer = false;
        GameManager.gm.canDiceRoll = true;
        playerPieceuserTurn();
    }

    public void playerPieceuserTurn()
    {

        int prvTurn = GameManager.gm.playerTurn;

        if (GameManager.gm.canDiceRoll)
            GameManager.gm.playerTurn = checkPlayerTurn();

        int newTurn = GameManager.gm.playerTurn;

        bool isChangeTurn = prvTurn != newTurn;

        for (int i = 0; i < pathParent.diceHolder.Length; i++)
        {
            if (i == (GameManager.gm.playerTurn - 1))
            {
                if (pathParent.diceHolder[i])
                {
                    pathParent.diceHolder[i].SetActive(true);
                }
            }
            else
            {
                if (pathParent.diceHolder[i])
                {
                    pathParent.diceHolder[i].SetActive(false);
                }
            }
        }

        updateBotPlayCount(prvTurn, isChangeTurn);

        if (!exitManager.isShown())
        {
            if (!winnerResult.isShown())
            {
                if (GameManager.gm.isAIPlayed)
                    GameManager.gm.isAIPlayed = false;
            }
        }

        if (!GameManager.gm.isOnlineGame)
        {
            GameManager.gm.isStartTimer = false;
            GameManager.gm.countDownStartValue = 20;
            if (GameManager.gm.isGameRunning)
                CountDownTimer.Instance.startTimer();
        }
        else
        {

            //mqttMessage.text = "Player Turn 1: " + SameMarker.Instance.getPlayerArray()[getIndex()].player_name + " Login Player : " + Credentials.email;


            if (SameMarker.Instance.getPlayerArray()[getIndex()].player_name == Credentials.email)
            {
                GameManager.gm.flow = System.DateTime.UtcNow.ToString();
                Acknowledgement ack = new Acknowledgement();
                ack.timeStamp = System.DateTime.UtcNow.ToString();
                ack.messageId = System.DateTime.UtcNow.Millisecond;
                ack.reciver = Credentials.email;

                PlayEvent ackEvent = new PlayEvent();
                ackEvent.action = "actionAcknowledgement";
                ackEvent.sender = Credentials.email;
                ackEvent.ack = ack;

                MqttEvent mqttEvent = new MqttEvent();
                mqttEvent.playEvent = ackEvent;

                string json = JsonUtility.ToJson(mqttEvent);

                Credentials.action = json;
                m2MqttUnityTest.TestPublish();

                GameManager.gm.isStartTimer = false;
                GameManager.gm.countDownStartValue = 20;
                if (GameManager.gm.isGameRunning)
                    CountDownTimer.Instance.startTimer();

            }

        }


    }
}
