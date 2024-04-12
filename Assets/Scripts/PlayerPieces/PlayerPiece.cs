using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;
using M2MqttUnity;
using TMPro;
using UnityEngine.SceneManagement;

// Merge Ludo and Ludo Pro - Fast Ludo
public class PlayerPiece : MonoBehaviour
{
    public bool isReady;
    public bool canMove;
    public bool moveNow;
    public int numberOfStepsToMove = 0;
    public int numberOfStepsAlreadyMove = 0;
    public JavaBridge javaBridge;
    private int[] array1;
    public int count_step;
// fast ludo timer
    private LudoGameTimer gameTimer;

    Coroutine moveSteps_Coroutine;
    Coroutine unlockFromHouse_Coroutine;

    public PathObjectsParent pathParent;
    public PathPoint previousPathPoint;
    public PathPoint currentPathPoint;
    public WinnerResult winnerResult;
    public ExitManager exitManager;
    private bool isWinner = false;
    private CountDownTimer countDownTimer;
    WaitingTimer waitingTimer;
    APICall aPICall;
    //public TextMeshProUGUI mqttMessage;

    public M2MqttUnity.Examples.M2MqttUnityTest m2MqttUnityTest;


    private void Start()
    {
     //   mqttMessage = GameObject.Find("Canvas/Text (TMP)").GetComponent<TextMeshProUGUI>();
    }

    private void Awake()
    {
        pathParent = FindObjectOfType<PathObjectsParent>();
        winnerResult = FindObjectOfType<WinnerResult>();
        exitManager = FindObjectOfType<ExitManager>();
        previousPathPoint = FindObjectOfType<PathPoint>();
        currentPathPoint = FindObjectOfType<PathPoint>();
        countDownTimer = FindObjectOfType<CountDownTimer>();
        waitingTimer = FindObjectOfType<WaitingTimer>();

        m2MqttUnityTest = FindObjectOfType<M2MqttUnity.Examples.M2MqttUnityTest>();

        aPICall = FindObjectOfType<APICall>();
        gameTimer = FindObjectOfType<LudoGameTimer>(); // fast ludo timer

    }

    public void MoveSteps(int playerId, int figureId, PathPoint[] pathPointsToMoveOn_, List<Safe> isKillingList)
    {
        GameManager.gm.isReadyToMove = false;
        moveSteps_Coroutine = StartCoroutine(MoveSteps_Enum(playerId, figureId, pathPointsToMoveOn_, isKillingList));

        storeMqttMessageLocally(playerId, figureId);
    }

    public void MakePlayerReadyToMove(int playerId, int figureId, PathPoint[] pathPointsToMoveOn_)
    {
        unlockFromHouse_Coroutine = StartCoroutine(UnlockFromHouse(playerId, figureId, pathPointsToMoveOn_));
        storeMqttMessageLocally(playerId, figureId);
    }

        // fast ludo unlock
    public void MakePlayerReadyToMoveFastLudo()
    {
        numberOfStepsAlreadyMove = 0;
        isReady = true;
    }

    //

    IEnumerator UnlockFromHouse(int playerId, int figureId, PathPoint[] pathPointsToMoveOn_)
    {
        SoundManager.PlaySound("move");
        if (GameManager.gm.playerChance > 0)
            GameManager.gm.playerChance--;
        isReady = true;
        GameManager.gm.isReadyToMove = false;
        transform.position = pathPointsToMoveOn_[0].transform.position;
        numberOfStepsAlreadyMove = 1;
        SameMarker.Instance.setNewValue(numberOfStepsAlreadyMove, calculatePosition(playerId, numberOfStepsAlreadyMove, pathPointsToMoveOn_), figureId, playerId);

        previousPathPoint = pathPointsToMoveOn_[0];
        currentPathPoint = pathPointsToMoveOn_[0];
        currentPathPoint.AddPlayerPiece(this);
        GameManager.gm.AddPathPoint(currentPathPoint);

        GameManager.gm.isStartTimer = false;

        if (SameMarker.Instance.getPlayerArray()[getIndex()].player_name == Credentials.email)
        {
            waitingTimer.startTimer(2f);
            yield return new WaitForSeconds(2f);
        }

        GameManager.gm.isAutomaticallyMovePlayerPiece = false;
        GameManager.gm.isTapOnPlayerPiece = false;
        GameManager.gm.canDiceRoll = true;

        GameManager.gm.countDownStartValue = 20;

        if (GameManager.gm.isGameRunning)
            CountDownTimer.Instance.startTimer();
        yield return new WaitForSeconds(0.01f);

        if (unlockFromHouse_Coroutine != null)
        {
            StopCoroutine(unlockFromHouse_Coroutine);
        }
    }

      IEnumerator MoveSteps_Enum(int playerId, int figureId, PathPoint[] pathPointsToMoveOn_, List<Safe> isKillingList)
    {

        yield return new WaitForSeconds(0.25f);
        if (canMove)
        {
            GameManager.gm.isStartTimer = false;
            numberOfStepsToMove = GameManager.gm.numberOfStepsToMove;

            for (int i = numberOfStepsAlreadyMove; i < (numberOfStepsAlreadyMove + numberOfStepsToMove); i++)
            {
                if (isPathPointsAvailableToMove(numberOfStepsToMove, numberOfStepsAlreadyMove, pathPointsToMoveOn_))
                {
                    transform.position = pathPointsToMoveOn_[i].transform.position;
                    SoundManager.PlaySound("move");
                    yield return new WaitForSeconds(0.25f);
                }

            }
            GameManager.gm.isTapOnPlayerPiece = false;
            GameManager.gm.isAutomaticallyMovePlayerPiece = false;
            GameManager.gm.isStartTimer = true;

        }

        if (isPathPointsAvailableToMove(numberOfStepsToMove, numberOfStepsAlreadyMove, pathPointsToMoveOn_))
        {
            if (GameManager.gm.playerChance > 0)
                GameManager.gm.playerChance--;
            numberOfStepsAlreadyMove += numberOfStepsToMove;
            GameManager.gm.numberOfStepsToMove = 0;
            SameMarker.Instance.setNewValue(numberOfStepsAlreadyMove, calculatePosition(playerId, numberOfStepsAlreadyMove, pathPointsToMoveOn_), figureId, playerId);


            GameManager.gm.RemovePathPoint(previousPathPoint);
            previousPathPoint.RemovePlayerPiece(this);
            currentPathPoint = pathPointsToMoveOn_[numberOfStepsAlreadyMove - 1];
            currentPathPoint.AddPlayerPiece(this);
            GameManager.gm.AddPathPoint(currentPathPoint);
            previousPathPoint = currentPathPoint;

            if (isKillingList.Count > 0)
                killingMarkerScenario(isKillingList);


            // fast ludo timer win logic
            // if timout -> check step count -> declare winner
            if (gameTimer.isGameTimeEnd) {
                Debug.Log("************ Game Time Ended**************");
                int firstPlayerScore = SameMarker.Instance.getPlayerTotalStepsCount(1);
                int secondPlayerScore = SameMarker.Instance.getPlayerTotalStepsCount(2);
                int thirdPlayerScore = SameMarker.Instance.getPlayerTotalStepsCount(3);
                int fourthPlayerScore = SameMarker.Instance.getPlayerTotalStepsCount(4);
                int winnerID = 1;
                if (firstPlayerScore > secondPlayerScore && firstPlayerScore > thirdPlayerScore && firstPlayerScore > fourthPlayerScore)
                {
                    winnerID = 1;
                }
                else if (secondPlayerScore > thirdPlayerScore && secondPlayerScore > fourthPlayerScore)
                {
                    winnerID = 2;
                }
                else if (thirdPlayerScore > fourthPlayerScore)
                {
                    winnerID = 3;
                }
                else {
                    winnerID = 4;
                }

                isWinner = true;

                if (SameMarker.Instance.getPlayerArray().Count == 2)
                {
                    if (winnerID == 3)
                        winnerID = 2;
                }
                SameMarker.Instance.getPlayerArray()[winnerID - 1].isWinner = true;
                SameMarker.Instance.addResult(new Results(winnerID, SameMarker.Instance.getPlayerUserName(winnerID), SameMarker.Instance.getPlayerUserId(winnerID)));

              
                if (GameManager.gm.isOnlineGame)
                {
                    if (SameMarker.Instance.getPlayerArray()[getIndex()].player_name == Credentials.email)
                    {
                        /*  for (int i = 0; i < SameMarker.Instance.getPlayerArray().Count; i++)
                          {
                              if (SameMarker.Instance.getPlayerArray()[i].player_id != playerId)
                              {
                                  print("Result Player Id : " + SameMarker.Instance.getPlayerArray()[i].player_id);
                                  SameMarker.Instance.addResult(new Results(SameMarker.Instance.getPlayerArray()[i].player_id, SameMarker.Instance.getPlayerName(SameMarker.Instance.getPlayerArray()[i].player_id)));
                              }
                          }*/

                        // fast ludo 
                        List<Player> remainingPlayerList = SameMarker.Instance.getRemainingPlayerArray();
                        for (int i = 0; i < SameMarker.Instance.getRemainingPlayerArray().Count; i++)
                        {
                            SameMarker.Instance.addResult(new Results(remainingPlayerList[i].player_id, remainingPlayerList[i].username, remainingPlayerList[i].user_id));
                        }

                        List<Player> quitPlayerList = SameMarker.Instance.getQuitPlayerList();
                        for (int i = SameMarker.Instance.getQuitPlayerArray().Count - 1; i >= 0; i--)
                        {
                            SameMarker.Instance.addResult(new Results(quitPlayerList[i].player_id, quitPlayerList[i].username, quitPlayerList[i].user_id));
                        }
                        // fast ludo


                        // fast ludo move this code end of function
                        //winnerResult.showWinnerList(true);

                        aPICall.callAddWinner();
                  
                        List<Results> resultList = SameMarker.Instance.getResultArray();
                        Debug.Log("winner ***" + resultList[0].name);
                        Winner winner = new Winner();
                        winner.playerId = resultList[0].player_id;
                        winner.playerName = resultList[0].name;
                        winner.user_id = resultList[0].user_id;

                        PlayEvent playEvent = new PlayEvent();
                        playEvent.action = "actionWinner";
                        playEvent.sender = Credentials.email;
                        playEvent.playerPieceEvent = null;
                        playEvent.bot = null;
                        playEvent.winner = winner;

                        MqttEvent mqttEvent = new MqttEvent();
                        mqttEvent.playEvent = playEvent;

                        string json = JsonUtility.ToJson(mqttEvent);

                        Credentials.action = json;
                        m2MqttUnityTest.TestPublish();

                        // fast ludo
                        winnerResult.showWinnerList(true);
                        Debug.Log("******************************** Winner 1 is called**************************");
                    }
                }
                else
                {
                    /*for (int i = 0; i < SameMarker.Instance.getPlayerArray().Count; i++)
                    {
                        if (SameMarker.Instance.getPlayerArray()[i].player_id != playerId)
                        {
                            print("Result Player Id : " + SameMarker.Instance.getPlayerArray()[i].player_id);
                            SameMarker.Instance.addResult(new Results(SameMarker.Instance.getPlayerArray()[i].player_id, SameMarker.Instance.getPlayerName(SameMarker.Instance.getPlayerArray()[i].player_id), SameMarker.Instance.getPlayerUserId(SameMarker.Instance.getPlayerArray()[i].player_id)));
                        }
                    }*/

                    List<Player> remainingPlayerList = SameMarker.Instance.getRemainingPlayerArray();
                    for (int i = 0; i < SameMarker.Instance.getRemainingPlayerArray().Count; i++)
                    {
                        SameMarker.Instance.addResult(new Results(remainingPlayerList[i].player_id, remainingPlayerList[i].username, remainingPlayerList[i].user_id));
                    }

                    List<Player> quitPlayerList = SameMarker.Instance.getQuitPlayerList();
                    for (int i = SameMarker.Instance.getQuitPlayerArray().Count - 1; i >= 0; i--)
                    {
                        SameMarker.Instance.addResult(new Results(quitPlayerList[i].player_id, quitPlayerList[i].username, quitPlayerList[i].user_id));
                    }

                    winnerResult.showWinnerList(true);
                    Debug.Log("******************************** Winner 2 is called**************************");

                    //                        StartCoroutine(openGCApplication());
                }

                yield break;
            }
            /// fast ludo end
            if (numberOfStepsAlreadyMove == 57)
            {
                if (SameMarker.Instance.checkIsItWinner(playerId))
                {
                    if (SameMarker.Instance.getPlayerArray().Count == 2)
                    {
                        if (playerId == 3)
                            playerId = 2;
                    }
                    isWinner = true;
                    SameMarker.Instance.getPlayerArray()[playerId - 1].isWinner = true;
                    SameMarker.Instance.addResult(new Results(playerId, SameMarker.Instance.getPlayerUserName(playerId), SameMarker.Instance.getPlayerUserId(playerId)));

                    /* if(isNotContinuableGame())
                     {
                       List<Player> quitPlayerList = SameMarker.Instance.getQuitPlayerArray();

                         SameMarker.Instance.addResult(new Results(SameMarker.Instance.getRemainingPlayerArray()[0].player_name));

                         for(int i = 0; i < quitPlayerList.Count; i++)
                         {
                           //SameMarker.Instance.addResult(new Results(quitPlayerList[i].player_name + " (Quit Game)"));
                           SameMarker.Instance.addResult(new Results(quitPlayerList[i].player_name));
                         }

                         winnerResult.showWinnerList(true);
                     }
                     else
                     {
                         winnerResult.showWinnerList(false);
                     }*/

                    if (GameManager.gm.isOnlineGame)
                    {
                        if (SameMarker.Instance.getPlayerArray()[getIndex()].player_name == Credentials.email)
                        {

                            /*  for (int i = 0; i < SameMarker.Instance.getPlayerArray().Count; i++)
                              {
                                  if (SameMarker.Instance.getPlayerArray()[i].player_id != playerId)
                                  {
                                      print("Result Player Id : " + SameMarker.Instance.getPlayerArray()[i].player_id);
                                      SameMarker.Instance.addResult(new Results(SameMarker.Instance.getPlayerArray()[i].player_id, SameMarker.Instance.getPlayerName(SameMarker.Instance.getPlayerArray()[i].player_id)));
                                  }
                              }*/

                            List<Player> remainingPlayerList = SameMarker.Instance.getRemainingPlayerArray();
                            for (int i = 0; i < SameMarker.Instance.getRemainingPlayerArray().Count; i++)
                            {
                                SameMarker.Instance.addResult(new Results(remainingPlayerList[i].player_id, remainingPlayerList[i].username, remainingPlayerList[i].user_id));
                            }

                            List<Player> quitPlayerList = SameMarker.Instance.getQuitPlayerList();
                            for (int i = SameMarker.Instance.getQuitPlayerArray().Count - 1; i >= 0; i--)
                            {
                                SameMarker.Instance.addResult(new Results(quitPlayerList[i].player_id, quitPlayerList[i].username, quitPlayerList[i].user_id));
                            }

                            winnerResult.showWinnerList(true);
                            Debug.Log("******************************** Winner 3 is called**************************");

                            aPICall.callAddWinner();

                            List<Results> resultList = SameMarker.Instance.getResultArray();
                            Winner winner = new Winner();
                            winner.playerId = resultList[0].player_id;
                            winner.playerName = resultList[0].name;
                            winner.user_id = resultList[0].user_id;

                            PlayEvent playEvent = new PlayEvent();
                            playEvent.action = "actionWinner";
                            playEvent.sender = Credentials.email;
                            playEvent.playerPieceEvent = null;
                            playEvent.bot = null;
                            playEvent.winner = winner;

                            MqttEvent mqttEvent = new MqttEvent();
                            mqttEvent.playEvent = playEvent;

                            string json = JsonUtility.ToJson(mqttEvent);

                            Credentials.action = json;
                            m2MqttUnityTest.TestPublish();
                        }
                    }
                    else
                    {
                        /*for (int i = 0; i < SameMarker.Instance.getPlayerArray().Count; i++)
                        {
                            if (SameMarker.Instance.getPlayerArray()[i].player_id != playerId)
                            {
                                print("Result Player Id : " + SameMarker.Instance.getPlayerArray()[i].player_id);
                                SameMarker.Instance.addResult(new Results(SameMarker.Instance.getPlayerArray()[i].player_id, SameMarker.Instance.getPlayerName(SameMarker.Instance.getPlayerArray()[i].player_id), SameMarker.Instance.getPlayerUserId(SameMarker.Instance.getPlayerArray()[i].player_id)));
                            }
                        }*/

                         List<Player> remainingPlayerList = SameMarker.Instance.getRemainingPlayerArray();
                            for (int i = 0; i < SameMarker.Instance.getRemainingPlayerArray().Count; i++)
                            {
                                SameMarker.Instance.addResult(new Results(remainingPlayerList[i].player_id, remainingPlayerList[i].username, remainingPlayerList[i].user_id));
                            }

                            List<Player> quitPlayerList = SameMarker.Instance.getQuitPlayerList();
                            for (int i = SameMarker.Instance.getQuitPlayerArray().Count - 1; i >= 0; i--)
                            {
                                SameMarker.Instance.addResult(new Results(quitPlayerList[i].player_id, quitPlayerList[i].username, quitPlayerList[i].user_id));
                            }

                        winnerResult.showWinnerList(true);
                        Debug.Log("******************************** Winner 4 is called**************************");

//                        StartCoroutine(openGCApplication());
                    }

                }
                else
                {
                    SoundManager.PlaySound("reachedGoal");
                    GameManager.gm.isKilled = true;

                    if(numberOfStepsToMove == 6)
                    {
                        GameManager.gm.playerChance = GameManager.gm.playerChance + 1;
                    }
                    else
                    GameManager.gm.playerChance++;
                }

            }

            if (!isNotContinuableGame())
            {

                if (GameManager.gm.isOnlineGame)
                {
                    if (SameMarker.Instance.getPlayerArray()[getIndex()].player_name == Credentials.email)
                    {
                        waitingTimer.startTimer(3f);
                        yield return new WaitForSeconds(3f);


                        PlayEvent playEvent = new PlayEvent();
                        playEvent.action = "actionChangeUserTurnFromPlayerPiece";
                        playEvent.sender = Credentials.email;
                        playEvent.playerPieceEvent = null;
                        playEvent.bot = null;

                        MqttEvent mqttEvent = new MqttEvent();
                        mqttEvent.playEvent = playEvent;

                        string json = JsonUtility.ToJson(mqttEvent);

                        Credentials.action = json;
                        m2MqttUnityTest.TestPublish();

                        /*---------------------------------------Local Storage and Server API call---------------------------------------*/

                        if(GameManager.gm.mqttIsConnected)
                        {
                            PlayerPieceEvent playerPieceEvent1 = new PlayerPieceEvent();
                            playerPieceEvent1.player_chance = GameManager.gm.playerChance;
                            playerPieceEvent1.sixCount = GameManager.gm.sixCount;

                            ServerPlayEvent playEvent1 = new ServerPlayEvent();
                            playEvent1.action = "actionChangeUserTurnFromPlayerPiece";
                            playEvent1.sender = Credentials.email;
                            playEvent1.turn_id = GameManager.gm.turn_id;
                            playEvent1.playerPieceEvent = playerPieceEvent1;

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

                            if (SameMarker.Instance.getPlayerArray()[getIndex()].player_name == Credentials.email)
                            {
                                aPICall.storeMovesOnServer(gameMoveForContestReq);
                            }

                            
                        }

                        /*---------------------------------------Finish Local Storage and Server API call---------------------------------------*/

                        GameManager.gm.isStartTimer = false;
                        GameManager.gm.canDiceRoll = true;
                        GameManager.gm.isReadyToMove = false;

                        userTurn();

                    }

                }
                else
                {
                    GameManager.gm.isStartTimer = false;
                    GameManager.gm.canDiceRoll = true;
                    userTurn();
                }


                /* GameManager.gm.isStartTimer = false;
                 GameManager.gm.canDiceRoll = true;
                 userTurn();*/

                    }


                }


        if (moveSteps_Coroutine != null)
        {
            StopCoroutine(moveSteps_Coroutine);
        }

    }

    public bool isPathPointsAvailableToMove(int numberOfStepsToMove_, int numberOfStepsAlreadyMove_, PathPoint[] pathPointsToMoveOn_)
    {
        int leftNumberOfPathPoints = pathPointsToMoveOn_.Length - numberOfStepsAlreadyMove_;

        if (leftNumberOfPathPoints >= numberOfStepsToMove_)
        {
            return true;
        }

        return false;
    }

    public int calculatePosition(int playerId, int numberOfStepsAlreadyMove_, PathPoint[] pathPointsToMoveOn_)
    {
        int leftNumberOfPathPoints = pathPointsToMoveOn_.Length - numberOfStepsAlreadyMove_;
        int position = 0;

        if (playerId == 2)
        {

            if ((numberOfStepsAlreadyMove_ + 13) > 52)
            {
                if (leftNumberOfPathPoints > 5)
                    position = (numberOfStepsAlreadyMove_ + 13) - 52;
                else
                    position = numberOfStepsAlreadyMove_ + 13;
            }
            else
                position = numberOfStepsAlreadyMove_ + 13;
        }
        else if (playerId == 3)
        {

            if ((numberOfStepsAlreadyMove_ + 26) > 52)
            {
                if (leftNumberOfPathPoints > 5)
                    position = (numberOfStepsAlreadyMove_ + 26) - 52;
                else
                    position = numberOfStepsAlreadyMove_ + 26;
            }
            else
                position = numberOfStepsAlreadyMove_ + 26;
        }
        else if (playerId == 4)
        {

            if ((numberOfStepsAlreadyMove_ + 39) > 52)
            {
                if (leftNumberOfPathPoints > 5)
                    position = (numberOfStepsAlreadyMove_ + 39) - 52;
                else
                    position = numberOfStepsAlreadyMove_ + 39;
            }
            else
                position = numberOfStepsAlreadyMove_ + 39;
        }
        else
        {
            position = numberOfStepsAlreadyMove_;
        }

        return position;
    }

    void OnJavaCallbackGetUserTurn(int result)
    {
        GameManager.gm.playerTurn = result;
    }

    public void setInitialState(int playerId, int figureId, bool isQuit)
    {
        if (!isQuit)
        {
            GameManager.gm.isKilled = true;
            GameManager.gm.playerChance++;
            SoundManager.PlaySound("kill");
        }

        if (playerId == 2 && figureId == 1)
        {
            GameObject.Find("LudoHomes/GreenHome/GreenPlayerPieces1").transform.position = pathParent.homePathPoints[0].transform.position;
            int point = SameMarker.Instance.getArray()[8].position;
           // mqttMessage.text = "Remove Successfully " + point.ToString();
            SameMarker.Instance.setNewValue(0, 108, 1, 2);

            if (isQuit)
            {

                if (point < 52)
                {
                    print("previous Path Point : " + pathParent.commonPathPoints[point - 1] + " : : " + previousPathPoint);
                    previousPathPoint = pathParent.commonPathPoints[point - 1];
                    previousPathPoint.RemovePlayerPiece(pathParent.Pieces[8]);

             //       mqttMessage.text = "Remove Successfully " + pathParent.Pieces[8].ToString();
                }

            }
            else
                previousPathPoint.RemovePlayerPiece(pathParent.Pieces[8]);

            pathParent.Pieces[8].transform.localScale = new Vector3(0.8f, 0.8f, 1f);

        }
        else if (playerId == 2 && figureId == 2)
        {
            GameObject.Find("LudoHomes/GreenHome/GreenPlayerPieces2").transform.position = pathParent.homePathPoints[1].transform.position;
            int point = SameMarker.Instance.getArray()[9].position;

            SameMarker.Instance.setNewValue(0, 109, 2, 2);

            if (isQuit)
            {
                if (point < 52)
                {
                    print("previous Path Point : " + pathParent.commonPathPoints[point - 1] + " : : " + previousPathPoint);
                    previousPathPoint = pathParent.commonPathPoints[point - 1];
                    previousPathPoint.RemovePlayerPiece(pathParent.Pieces[9]);
                }

            }
            else
                previousPathPoint.RemovePlayerPiece(pathParent.Pieces[9]);

            pathParent.Pieces[9].transform.localScale = new Vector3(0.8f, 0.8f, 1f);

        }
        else if (playerId == 2 && figureId == 3)
        {
            GameObject.Find("LudoHomes/GreenHome/GreenPlayerPieces3").transform.position = pathParent.homePathPoints[2].transform.position;
            int point = SameMarker.Instance.getArray()[10].position;

            SameMarker.Instance.setNewValue(0, 110, 3, 2);

            if (isQuit)
            {
                if (point < 52)
                {
                    print("previous Path Point : " + pathParent.commonPathPoints[point - 1] + " : : " + previousPathPoint);
                    previousPathPoint = pathParent.commonPathPoints[point - 1];
                    previousPathPoint.RemovePlayerPiece(pathParent.Pieces[10]);
                }

            }
            else
                previousPathPoint.RemovePlayerPiece(pathParent.Pieces[10]);

            pathParent.Pieces[10].transform.localScale = new Vector3(0.8f, 0.8f, 1f);

        }
        else if (playerId == 2 && figureId == 4)
        {
            GameObject.Find("LudoHomes/GreenHome/GreenPlayerPieces4").transform.position = pathParent.homePathPoints[3].transform.position;
            int point = SameMarker.Instance.getArray()[11].position;

            SameMarker.Instance.setNewValue(0, 111, 4, 2);

            if (isQuit)
            {
                if (point < 52)
                {
                    print("previous Path Point : " + pathParent.commonPathPoints[point - 1] + " : : " + previousPathPoint);
                    previousPathPoint = pathParent.commonPathPoints[point - 1];
                    previousPathPoint.RemovePlayerPiece(pathParent.Pieces[11]);
                }

            }
            else
                previousPathPoint.RemovePlayerPiece(pathParent.Pieces[11]);

            pathParent.Pieces[11].transform.localScale = new Vector3(0.8f, 0.8f, 1f);

        }
        else if (playerId == 3 && figureId == 1)
        {
            GameObject.Find("LudoHomes/RedHome/RedPlayerPieces1").transform.position = pathParent.homePathPoints[4].transform.position;
            int point = SameMarker.Instance.getArray()[4].position;

            SameMarker.Instance.setNewValue(0, 104, 1, 3);

            if (isQuit)
            {
                if (point < 52)
                {
                    print("previous Path Point : " + pathParent.commonPathPoints[point - 1] + " : : " + previousPathPoint);
                    previousPathPoint = pathParent.commonPathPoints[point - 1];
                    previousPathPoint.RemovePlayerPiece(pathParent.Pieces[4]);
                }

            }
            else
                previousPathPoint.RemovePlayerPiece(pathParent.Pieces[4]);

            pathParent.Pieces[4].transform.localScale = new Vector3(0.8f, 0.8f, 1f);

        }
        else if (playerId == 3 && figureId == 2)
        {
            GameObject.Find("LudoHomes/RedHome/RedPlayerPieces2").transform.position = pathParent.homePathPoints[5].transform.position;
            int point = SameMarker.Instance.getArray()[5].position;

            SameMarker.Instance.setNewValue(0, 105, 2, 3);

            if (isQuit)
            {
                if (point < 52)
                {
                    print("previous Path Point : " + pathParent.commonPathPoints[point - 1] + " : : " + previousPathPoint);
                    previousPathPoint = pathParent.commonPathPoints[point - 1];
                    previousPathPoint.RemovePlayerPiece(pathParent.Pieces[5]);
                }

            }
            else
                previousPathPoint.RemovePlayerPiece(pathParent.Pieces[5]);

            pathParent.Pieces[5].transform.localScale = new Vector3(0.8f, 0.8f, 1f);

        }
        else if (playerId == 3 && figureId == 3)
        {
            GameObject.Find("LudoHomes/RedHome/RedPlayerPieces3").transform.position = pathParent.homePathPoints[6].transform.position;
            int point = SameMarker.Instance.getArray()[6].position;

            SameMarker.Instance.setNewValue(0, 106, 3, 3);

            if (isQuit)
            {
                if (point < 52)
                {
                    print("previous Path Point : " + pathParent.commonPathPoints[point - 1] + " : : " + previousPathPoint);
                    previousPathPoint = pathParent.commonPathPoints[point - 1];
                    previousPathPoint.RemovePlayerPiece(pathParent.Pieces[6]);
                }

            }
            else
                previousPathPoint.RemovePlayerPiece(pathParent.Pieces[6]);

            pathParent.Pieces[6].transform.localScale = new Vector3(0.8f, 0.8f, 1f);

        }
        else if (playerId == 3 && figureId == 4)
        {
            GameObject.Find("LudoHomes/RedHome/RedPlayerPieces4").transform.position = pathParent.homePathPoints[7].transform.position;
            int point = SameMarker.Instance.getArray()[7].position;

            SameMarker.Instance.setNewValue(0, 107, 4, 3);

            if (isQuit)
            {

                if (point < 52)
                {
                    print("previous Path Point : " + pathParent.commonPathPoints[point - 1] + " : : " + previousPathPoint);
                    previousPathPoint = pathParent.commonPathPoints[point - 1];
                    previousPathPoint.RemovePlayerPiece(pathParent.Pieces[7]);
                }

            }
            else
                previousPathPoint.RemovePlayerPiece(pathParent.Pieces[7]);

            pathParent.Pieces[7].transform.localScale = new Vector3(0.8f, 0.8f, 1f);

        }
        else if (playerId == 4 && figureId == 1)
        {
            GameObject.Find("LudoHomes/BlueHome/BluePlayerPieces1").transform.position = pathParent.homePathPoints[9].transform.position;
            int point = SameMarker.Instance.getArray()[12].position;

            SameMarker.Instance.setNewValue(0, 112, 1, 4);

            if (isQuit)
            {
                if (point < 52)
                {
                    print("previous Path Point : " + pathParent.commonPathPoints[point - 1] + " : : " + previousPathPoint);
                    previousPathPoint = pathParent.commonPathPoints[point - 1];
                    previousPathPoint.RemovePlayerPiece(pathParent.Pieces[12]);
                }

            }
            else
                previousPathPoint.RemovePlayerPiece(pathParent.Pieces[12]);

            pathParent.Pieces[12].transform.localScale = new Vector3(0.8f, 0.8f, 1f);

        }
        else if (playerId == 4 && figureId == 2)
        {
            GameObject.Find("LudoHomes/BlueHome/BluePlayerPieces2").transform.position = pathParent.homePathPoints[10].transform.position;
            int point = SameMarker.Instance.getArray()[13].position;

            SameMarker.Instance.setNewValue(0, 113, 2, 4);

            if (isQuit)
            {
                if (point < 52)
                {
                    print("previous Path Point : " + pathParent.commonPathPoints[point - 1] + " : : " + previousPathPoint);
                    previousPathPoint = pathParent.commonPathPoints[point - 1];
                    previousPathPoint.RemovePlayerPiece(pathParent.Pieces[13]);
                }

            }
            else
                previousPathPoint.RemovePlayerPiece(pathParent.Pieces[13]);

            pathParent.Pieces[13].transform.localScale = new Vector3(0.8f, 0.8f, 1f);

        }
        else if (playerId == 4 && figureId == 3)
        {
            GameObject.Find("LudoHomes/BlueHome/BluePlayerPieces3").transform.position = pathParent.homePathPoints[8].transform.position;
            int point = SameMarker.Instance.getArray()[14].position;

            SameMarker.Instance.setNewValue(0, 114, 3, 4);

            if (isQuit)
            {
                if (point < 52)
                {
                    print("previous Path Point : " + pathParent.commonPathPoints[point - 1] + " : : " + previousPathPoint);
                    previousPathPoint = pathParent.commonPathPoints[point - 1];
                    previousPathPoint.RemovePlayerPiece(pathParent.Pieces[14]);
                }

            }
            else
                previousPathPoint.RemovePlayerPiece(pathParent.Pieces[14]);

            pathParent.Pieces[14].transform.localScale = new Vector3(0.8f, 0.8f, 1f);

        }
        else if (playerId == 4 && figureId == 4)
        {
            GameObject.Find("LudoHomes/BlueHome/BluePlayerPieces4").transform.position = pathParent.homePathPoints[11].transform.position;
            int point = SameMarker.Instance.getArray()[15].position;

            SameMarker.Instance.setNewValue(0, 115, 4, 4);

            if (isQuit)
            {
                if (point < 52)
                {
                    print("previous Path Point : " + pathParent.commonPathPoints[point - 1] + " : : " + previousPathPoint);
                    previousPathPoint = pathParent.commonPathPoints[point - 1];
                    previousPathPoint.RemovePlayerPiece(pathParent.Pieces[15]);
                }
            }
            else
                previousPathPoint.RemovePlayerPiece(pathParent.Pieces[15]);

            pathParent.Pieces[15].transform.localScale = new Vector3(0.8f, 0.8f, 1f);

        }
        else if (playerId == 1 && figureId == 1)
        {
            GameObject.Find("LudoHomes/YellowHome/YellowPlayerPieces1").transform.position = pathParent.homePathPoints[12].transform.position;
            int point = SameMarker.Instance.getArray()[0].position;

            SameMarker.Instance.setNewValue(0, 100, 1, 1);

            if (isQuit)
            {
                if (point < 52)
                {
                    print("previous Path Point : " + pathParent.commonPathPoints[point - 1] + " : : " + previousPathPoint);
                    previousPathPoint = pathParent.commonPathPoints[point - 1];
                    previousPathPoint.RemovePlayerPiece(pathParent.Pieces[0]);
                }

            }
            else
                previousPathPoint.RemovePlayerPiece(pathParent.Pieces[0]);

            pathParent.Pieces[0].transform.localScale = new Vector3(0.8f, 0.8f, 1f);

        }
        else if (playerId == 1 && figureId == 2)
        {
            GameObject.Find("LudoHomes/YellowHome/YellowPlayerPieces2").transform.position = pathParent.homePathPoints[13].transform.position;
            int point = SameMarker.Instance.getArray()[1].position;

            SameMarker.Instance.setNewValue(0, 101, 2, 1);
            if (isQuit)
            {
                if (point < 52)
                {
                    print("previous Path Point : " + pathParent.commonPathPoints[point - 1] + " : : " + previousPathPoint);
                    previousPathPoint = pathParent.commonPathPoints[point - 1];
                    previousPathPoint.RemovePlayerPiece(pathParent.Pieces[1]);
                }

            }
            else
                previousPathPoint.RemovePlayerPiece(pathParent.Pieces[1]);

            pathParent.Pieces[1].transform.localScale = new Vector3(0.8f, 0.8f, 1f);

        }
        else if (playerId == 1 && figureId == 3)
        {
            GameObject.Find("LudoHomes/YellowHome/YellowPlayerPieces3").transform.position = pathParent.homePathPoints[14].transform.position;
            int point = SameMarker.Instance.getArray()[2].position;

            SameMarker.Instance.setNewValue(0, 102, 3, 1);

            if (isQuit)
            {
                if (point < 52)
                {
                    print("previous Path Point : " + pathParent.commonPathPoints[point - 1] + " : : " + previousPathPoint);
                    previousPathPoint = pathParent.commonPathPoints[point - 1];
                    previousPathPoint.RemovePlayerPiece(pathParent.Pieces[2]);
                }
            }
            else
                previousPathPoint.RemovePlayerPiece(pathParent.Pieces[2]);

            pathParent.Pieces[2].transform.localScale = new Vector3(0.8f, 0.8f, 1f);

        }
        else if (playerId == 1 && figureId == 4)
        {
            GameObject.Find("LudoHomes/YellowHome/YellowPlayerPieces4").transform.position = pathParent.homePathPoints[15].transform.position;
            int point = SameMarker.Instance.getArray()[3].position;

            SameMarker.Instance.setNewValue(0, 103, 4, 1);

            if (isQuit)
            {
                if (point < 52)
                {
                    print("previous Path Point : " + pathParent.commonPathPoints[point - 1] + " : : " + previousPathPoint);
                    previousPathPoint = pathParent.commonPathPoints[point - 1];
                    previousPathPoint.RemovePlayerPiece(pathParent.Pieces[3]);
                }
            }
            else
                previousPathPoint.RemovePlayerPiece(pathParent.Pieces[3]);

            pathParent.Pieces[3].transform.localScale = new Vector3(0.8f, 0.8f, 1f);
        }


    }

    public void OnJavaCallbackMarkerList(int[] toArray)
    {
        // Pass the result to the C# event that we register to in the UI class
        Debug.Log("Show All position");

    }

    public void OnJavaCallbackGetCountStep(int countStep)
    {
        count_step = countStep;
        numberOfStepsAlreadyMove = countStep;
    }

    public void killingMarkerScenario(List<Safe> isKillingList)
    {
        int player1 = 0;
        int player2 = 0;
        int player3 = 0;
        int player4 = 0;

        if (isKillingList.Count == 1)
        {
            setInitialState(isKillingList[0].player_id, isKillingList[0].figure_id, false);
        }
        else
        {

            for (int i = 0; i < isKillingList.Count; i++)
            {
                if (isKillingList[i].player_id == 1)
                {
                    player1++;
                }
                else if (isKillingList[i].player_id == 2)
                {
                    player2++;
                }
                else if (isKillingList[i].player_id == 3)
                {
                    player3++;
                }
                else if (isKillingList[i].player_id == 4)
                {
                    player4++;
                }
            }

            for (int i = 0; i < isKillingList.Count; i++)
            {
                if (isKillingList[i].player_id == 1 && player1 == 1)
                {
                    setInitialState(isKillingList[i].player_id, isKillingList[i].figure_id, false);

                }
                else if (isKillingList[i].player_id == 2 && player2 == 1)
                {
                    setInitialState(isKillingList[i].player_id, isKillingList[i].figure_id, false);

                }
                else if (isKillingList[i].player_id == 3 && player3 == 1)
                {
                    setInitialState(isKillingList[i].player_id, isKillingList[i].figure_id, false);

                }
                else if (isKillingList[i].player_id == 4 && player4 == 1)
                {
                    setInitialState(isKillingList[i].player_id, isKillingList[i].figure_id, false);
                }
            }

        }
    }


    public bool checkSafeHouse(int position)
    {

        return position == 1 || position == 9 || position == 14 ||
          position == 22 || position == 27 || position == 35 ||
           position == 40 || position == 48;
    }

    public void userTurn()
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
        // Fast Ludo iOS
        GameManager.gm.isSixGraterThanTwo = false;
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

           // mqttMessage.text = "Player Turn 1: " + SameMarker.Instance.getPlayerArray()[getIndex()].player_name + " Login Player : " + Credentials.email;


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

    }

    int checkPlayerTurn()
    {
        int count;
        int dice;
        bool isKilled;

        if (GameManager.gm.playerTurn == null)
        {
            count = 1;
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


        if (isWinner)
        {
            isWinner = false;

            count++;

            if (count > SameMarker.Instance.getPlayerArray().Count)
            {
                count = 1;
            }

            if (SameMarker.Instance.checkIsItWinner(count) || SameMarker.Instance.isPlayerQuit(count))
            {
                count++;
            }


            if (count > SameMarker.Instance.getPlayerArray().Count || SameMarker.Instance.isPlayerQuit(count))
            {
                count = 1;
            }

            GameManager.gm.playerChance = 1;

            GameManager.gm.sixCount = 0;

            return count;

        }


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
        return count;

        /*if(dice == 6 && GameManager.gm.sixCount != 3)
        {
        if(SameMarker.Instance.isSingleAvailableToMove(GameManager.gm.playerTurn,GameManager.gm.numberOfStepsToMove).Count == 0 && SameMarker.Instance.getHomePlayer(GameManager.gm.playerTurn).Count == 0)
        {

          if(SameMarker.Instance.getPlayerArray().Count == 2 && count != 0)
         {
          count = count + 2;

         if(count > 4)
         {
            count = 1;
         }
            return count;
         }
        else
        {
            count++;

          if(count>SameMarker.Instance.getPlayerArray().Count)
         {
            count = 1;
         }

        }

        }
        return count;

        }else if(isKilled)
        {
        return count;
        }
        else{

         if(SameMarker.Instance.getPlayerArray().Count == 2 && count != 0)
         {

          count = count + 2;

         if(count > 4)
         {
            count = 1;
         }
            GameManager.gm.sixCount = 0;
            return count;
         }
        else
        {
            count++;
        }


         if(count>SameMarker.Instance.getPlayerArray().Count)
         {
            count = 1;
         }


         if(SameMarker.Instance.checkIsItWinner(count))
         {
            count ++;

            if(SameMarker.Instance.checkIsItWinner(count))
            {
                count++;
            }

            if(count>SameMarker.Instance.getPlayerArray().Count)
            {
                count = 1;
            }
         }
         GameManager.gm.sixCount = 0;
         return count;
        }*/
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

    public void quitPlayer(int prvTurn)
    {

        if (prvTurn == 1)
        {
            SameMarker.Instance.getPlayerArray()[0].isQuit = true;
            SameMarker.Instance.addQuitPlayer(SameMarker.Instance.getPlayerArray()[0]);

            for (int i = 0; i < 4; i++)
            {
                setInitialState(SameMarker.Instance.getArray()[i].player_id, SameMarker.Instance.getArray()[i].figure_id, true);
            }

            for (int i = 0; i < pathParent.yellowPlayerPieces.Length; i++)
            {
                objectSetActivie(pathParent.yellowPlayerPieces[i]);
            }

        }
        else if (prvTurn == 2)
        {
            SameMarker.Instance.getPlayerArray()[1].isQuit = true;
            SameMarker.Instance.addQuitPlayer(SameMarker.Instance.getPlayerArray()[1]);

            for (int i = 8; i < 12; i++)
            {
                setInitialState(SameMarker.Instance.getArray()[i].player_id, SameMarker.Instance.getArray()[i].figure_id, true);
            }

            for (int i = 0; i < pathParent.greenPlayerPieces.Length; i++)
            {
                objectSetActivie(pathParent.greenPlayerPieces[i]);
            }
        }
        else if (prvTurn == 3)
        {
            if (SameMarker.Instance.getPlayerArray().Count == 2)
            {
                SameMarker.Instance.getPlayerArray()[1].isQuit = true;
                SameMarker.Instance.addQuitPlayer(SameMarker.Instance.getPlayerArray()[1]);
            }     
            else
            {
                SameMarker.Instance.getPlayerArray()[2].isQuit = true;
                SameMarker.Instance.addQuitPlayer(SameMarker.Instance.getPlayerArray()[2]);
            }
                
            for (int i = 4; i < 8; i++)
            {
                setInitialState(SameMarker.Instance.getArray()[i].player_id, SameMarker.Instance.getArray()[i].figure_id, true);
            }

            for (int i = 0; i < pathParent.redPlayerPieces.Length; i++)
            {
                objectSetActivie(pathParent.redPlayerPieces[i]);
            }

        }
        else if (prvTurn == 4)
        {
            SameMarker.Instance.getPlayerArray()[3].isQuit = true;
            SameMarker.Instance.addQuitPlayer(SameMarker.Instance.getPlayerArray()[3]);

            for (int i = 12; i < 16; i++)
            {
                setInitialState(SameMarker.Instance.getArray()[i].player_id, SameMarker.Instance.getArray()[i].figure_id, true);

            }

            for (int i = 0; i < pathParent.bluePlayerPieces.Length; i++)
            {
                objectSetActivie(pathParent.bluePlayerPieces[i]);
            }

        }

        if (isNotContinuableGame())
        {
           // List<Player> quitPlayerList = SameMarker.Instance.getQuitPlayerArray();

            SameMarker.Instance.addResult(new Results(SameMarker.Instance.getRemainingPlayerArray()[0].player_id, SameMarker.Instance.getRemainingPlayerArray()[0].username, SameMarker.Instance.getRemainingPlayerArray()[0].user_id));

            /* for (int i = 0; i < quitPlayerList.Count; i++)
             {
                 SameMarker.Instance.addResult(new Results(quitPlayerList[i].player_id, quitPlayerList[i].player_name));
             }*/

            List<Player> quitPlayerList = SameMarker.Instance.getQuitPlayerArray();
            for (int i = SameMarker.Instance.getQuitPlayerArray().Count - 1; i >= 0; i--)
            {
                SameMarker.Instance.addResult(new Results(quitPlayerList[i].player_id, quitPlayerList[i].username, quitPlayerList[i].user_id));
            }

            winnerResult.showWinnerList(true);

            aPICall.callAddWinner();
        }
        else
        {
            if (SameMarker.Instance.getPlayerArray()[prvTurn - 1].player_name == Credentials.email)
            {
                //exitManager.sendDataToGcApp();

#if UNITY_ANDROID
                clearIntent();
#endif

                clearCredentials();

                SceneManager.LoadScene(1);

        }
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

    private void storeMqttMessageLocally(int playerId, int figureId)
    {
        //store mqtt message in local storage and server

        if(GameManager.gm.mqttIsConnected)
        {
            PlayerPieceEvent playerPieceEvent1 = new PlayerPieceEvent();
            playerPieceEvent1.playerId = playerId;
            playerPieceEvent1.figureId = figureId;
            playerPieceEvent1.pos = SameMarker.Instance.getMarkerIndex(playerId, figureId);
            playerPieceEvent1.dice_value = GameManager.gm.numberOfStepsToMove;
            playerPieceEvent1.player_chance = GameManager.gm.playerChance;
            playerPieceEvent1.sixCount = GameManager.gm.sixCount;

            ServerPlayEvent playEvent1 = new ServerPlayEvent();
            playEvent1.action = getAction(playerId);
            playEvent1.sender = Credentials.email;
            playEvent1.turn_id = GameManager.gm.turn_id;
            playEvent1.playerPieceEvent = playerPieceEvent1;

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

            if (SameMarker.Instance.getPlayerArray()[getIndex()].player_name == Credentials.email)
            {
                aPICall.storeMovesOnServer(gameMoveForContestReq);
            }


        }
        
        //finish store mqtt message in local storage and server
    }


    private string getAction(int playerId)
    {
        string action = "";
        switch(playerId)
        {
            case 1: action = "actionMoveYellowPiece";
                    break;

            case 2: if (SameMarker.Instance.getPlayerArray().Count == 2)
                        action = "actionMoveRedPiece";
                    else
                        action = "actionMoveGreenPiece";
                    break;
            case 3:
                    action = "actionMoveRedPiece";
                    break;

            case 4: action = "actionMoveBluePiece";
                    break;
        }

        return action;

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
        }

    }

    void clearCredentials()
    {
        GameManager.gm.isGameRunning = false;
        if (SameMarker.Instance != null)
            SameMarker.ClearInstance();
        if (MqttMessageArray.Instance != null)
        {
            MqttMessageArray.Instance.clearList();
            MqttMessageArray.ClearInstance();
        }
        GameManager.clearGameManager();

        Credentials.id = null;
        Credentials.channel = null;
        Credentials.id = null;
        Credentials.old_id = null;
        Credentials.email = null;
        Credentials.action = null;
        Credentials.contestId = null;
        Credentials.userId = null;

    }


}
