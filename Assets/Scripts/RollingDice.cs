using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;
using TMPro;
using UnityEngine.UI;
using System.Linq;
using M2MqttUnity;
using UnityEngine.SceneManagement;

// Aandroid Ludo and Sklash Ludo merge - Fast Ludo

public class RollingDice : MonoBehaviour
{
    [SerializeField] int numberGot;
    [SerializeField] GameObject rollingDiceAnim;
    [SerializeField] SpriteRenderer numberedSpHoldere;
    [SerializeField] Sprite[] numberedSprites;
    public PlayerPiece playerPiece;
    PathObjectsParent pathParent;
    List<GameObject> playerObjectList = new List<GameObject>();
    List<GameObject> playerSpriteObjectList = new List<GameObject>();
    HashSet<GameObject> animationList = new HashSet<GameObject>();
    Animator m_Animator;
    Button button, diceNumberButton, gameSettingButton;
    public ExitManager exitManager;
    private CountDownTimer countDownTimer;
    private GameSettings gameSettings;
    public WinnerResult winnerResult;
    InputField diceNumberInputField;
    public TextMeshProUGUI mqttMessage;
    WaitingTimer waitingTimer;

    public M2MqttUnity.Examples.M2MqttUnityTest m2MqttUnityTest;


    Coroutine generateRandNumOnDice_Coroutine;

    Coroutine changePlayerTurn_Coroutine;

    APICall aPICall;

    static bool isAnimationActive;


    /*public void OnMouseDown()
    {
    print(!SameMarker.Instance.getPlayerArray()[getIndex()].isAI);
    if(!SameMarker.Instance.getPlayerArray()[getIndex()].isAI)
        generateRandNumOnDice_Coroutine = StartCoroutine(GenerateRandomNumberOnDice_Enum());
    }*/

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
        aPICall = FindObjectOfType<APICall>();

    }

    private void Start()
    {

        addGameObjectInList();
        addSpriteGameObjectInList();

        StopAllAnimation();

        if (!GameManager.gm.isSaveGame)
        {
            changePlayerTurn_Coroutine = StartCoroutine(userTurn());
            GameManager.gm.canDiceRoll = true;

            GameManager.gm.playerChance = 1;
            GameManager.gm.sixCount = 0;
            GameManager.gm.turn_id = 0;
        }

        mqttMessage = GameObject.Find("Canvas/Text (TMP)").GetComponent<TextMeshProUGUI>();

        try
        {
            button = GameObject.Find("Canvas/ExitButton").GetComponent<Button>();
            button.onClick.AddListener(displaySaveBox);
        }
        catch(Exception e)
        {
            Debug.Log(e.Message);
        }
        

    }

    private void Update()
    {
        if(GameManager.gm.isOnlineGame)
        {
            if(isAnimationActive)
            {
                
                foreach (var val in GameManager.gm.onlineAnimationList)
                {
                    val.transform.Rotate(new Vector3(0f, 500f, 0f) * Time.deltaTime);
                }
            }
        }
        
    }



    IEnumerator GenerateRandomNumberOnDice_Enum(int number, string action)
    {
        yield return new WaitForEndOfFrame();

        if (GameManager.gm.canDiceRoll)
        {
            SoundManager.PlaySound("rollDice");
            GameManager.gm.turn_id = GameManager.gm.turn_id+1;
            GameManager.gm.isSixCountGraterThanTwo = false;
            GameManager.gm.isKilled = false;
            GameManager.gm.canDiceRoll = false;
            numberedSpHoldere.gameObject.SetActive(false);
            rollingDiceAnim.SetActive(true);
            yield return new WaitForSeconds(0.5f);

            if (number == 0)
               // numberGot = GameManager.gm.requirdDiceNumber - 1;
             numberGot = UnityEngine.Random.Range(0, 6);
            else
                numberGot = number - 1;

            numberedSpHoldere.sprite = numberedSprites[numberGot];
            numberGot += 1;

            rollingDiceAnim.SetActive(false);
            numberedSpHoldere.gameObject.SetActive(true);

            GameManager.gm.numberOfStepsToMove = numberGot;
            GameManager.gm.dice = numberGot;

            if (action != "" && GameManager.gm.mqttIsConnected)
                publishMessage(numberGot, action);

            if (GameManager.gm.isOnlineGame)
            {
                onlineGameAnimation();
                if (SameMarker.Instance.getPlayerArray()[getIndex()].player_name == Credentials.email)
                {
                    /* if (SameMarker.Instance.isSingleAvailableToMove(GameManager.gm.playerTurn, GameManager.gm.numberOfStepsToMove).Count == 1)
                     {
                         if (GameManager.gm.numberOfStepsToMove == 6)
                         {
                             List<Safe> homeList = SameMarker.Instance.getHomePlayer(GameManager.gm.playerTurn);

                             if (homeList.Count == 1 && SameMarker.Instance.isSingleAvailableToMove(GameManager.gm.playerTurn, GameManager.gm.numberOfStepsToMove).Count == 0)
                             {
                             }
                             else
                             {
                                     waitingTimer.startTimer(5f);
                                     yield return new WaitForSeconds(5f); 
                             }
                         }
                         else
                         {
                             waitingTimer.startTimer(5f);
                             yield return new WaitForSeconds(5f);
                         }

                     }
                     else if (SameMarker.Instance.isSingleAvailableToMove(GameManager.gm.playerTurn, GameManager.gm.numberOfStepsToMove).Count > 0)
                     {
                         List<Safe> list = SameMarker.Instance.isSingleAvailableToMove(GameManager.gm.playerTurn, GameManager.gm.numberOfStepsToMove);

                         if (list.Count > 1)
                         {
                             HashSet<int> similarPosition = new HashSet<int>();

                             for (int i = 0; i < list.Count; i++)
                             {
                                 similarPosition.Add(list[i].count_steps);
                             }

                             if (similarPosition.Count == 1)
                             {

                                 if (GameManager.gm.numberOfStepsToMove == 6)
                                 {
                                     waitingTimer.startTimer(5f);
                                     yield return new WaitForSeconds(5f);
                                 }
                                 else if (SameMarker.Instance.getPlayerArray()[getIndex()].isAI)
                                 {
                                     waitingTimer.startTimer(5f);
                                     yield return new WaitForSeconds(5f);
                                 }
                             }
                             else
                             {
                                 waitingTimer.startTimer(5f);
                                 yield return new WaitForSeconds(5f);
                             }
                         }

                     }
                     else
                     { 
                             waitingTimer.startTimer(5f);
                             yield return new WaitForSeconds(5f);

                     }*/

                  
                  
                  
                  
                    waitingTimer.startTimer(0.75f);
                    yield return new WaitForSeconds(0.75f);

                }
            }

            GameManager.gm.isReadyToMove = true;


            if (GameManager.gm.numberOfStepsToMove == 6)
            {
                if (GameManager.gm.sixCount < 3)
                {
                    GameManager.gm.sixCount++;
                }

                if (GameManager.gm.sixCount < 3)
                {
                    if (GameManager.gm.playerChance == 0)
                        GameManager.gm.playerChance = 1;

                    GameManager.gm.playerChance++;
                }



                if (GameManager.gm.sixCount > 2)
                {
                    GameManager.gm.isSixCountGraterThanTwo = true;
                    GameManager.gm.isSixGraterThanTwo = true;
                    if (GameManager.gm.playerChance > 0)
                        GameManager.gm.playerChance--;

                    GameManager.gm.canDiceRoll = true;

                    StopAllAnimation();

                    GameManager.gm.isReadyToMove = false;
                    GameManager.gm.numberOfStepsToMove = 0;
                    GameManager.gm.sixCount = 0;
                    

                    if (GameManager.gm.isOnlineGame)
                    {
                        waitingTimer.startTimer(1.5f);
                        yield return new WaitForSeconds(1.5f);
                        if (SameMarker.Instance.getPlayerArray()[getIndex()].player_name == Credentials.email)
                        {
                            PlayEvent playEvent = new PlayEvent();
                            playEvent.action = "actionChangeUserTurnFromRollingDice";
                            playEvent.sender = Credentials.email;
                            playEvent.playerPieceEvent = null;
                            playEvent.bot = null;

                            MqttEvent mqttEvent = new MqttEvent();
                            mqttEvent.playEvent = playEvent;

                            string json = JsonUtility.ToJson(mqttEvent);

                            Credentials.action = json;
                            m2MqttUnityTest.TestPublish();


                            /*---------------------------------------Local Storage and Server API call---------------------------------------*/
                            PlayerPieceEvent playerPieceEvent1 = new PlayerPieceEvent();
                            playerPieceEvent1.player_chance = GameManager.gm.playerChance;
                            playerPieceEvent1.sixCount = GameManager.gm.sixCount;

                            ServerPlayEvent playEvent1 = new ServerPlayEvent();
                            playEvent1.action = "actionChangeUserTurnFromRollingDice";
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

                            /*---------------------------------------Finish Local Storage and Server API call---------------------------------------*/

                            changePlayerTurn_Coroutine = StartCoroutine(userTurn());

                        }
                    }
                    else
                    {
                        changePlayerTurn_Coroutine = StartCoroutine(userTurn());
                    }



                    yield return new WaitForEndOfFrame();

                    if (generateRandNumOnDice_Coroutine != null)
                    {
                        StopCoroutine(generateRandNumOnDice_Coroutine);
                    }

                    yield break;
                }
            }
            else
            {
                GameManager.gm.sixCount = 0;
            }


            if (!SameMarker.Instance.getPlayerArray()[getIndex()].isAI)
            {

                if (SameMarker.Instance.isSingleAvailableToMove(GameManager.gm.playerTurn, GameManager.gm.numberOfStepsToMove).Count > 0)
                {

                    if (SameMarker.Instance.getMovableArray().Count != 0)
                        SameMarker.Instance.clearMovableList();

                    List<Safe> list = SameMarker.Instance.isSingleAvailableToMove(GameManager.gm.playerTurn, GameManager.gm.numberOfStepsToMove);

                    for (int i = 0; i < list.Count; i++)
                    {
                        if (!GameManager.gm.isOnlineGame)
                        {
                            GameObject gameObject = playerObjectList[list[i].index];
                            print(gameObject.transform.localScale.x + " : " + gameObject.transform.localScale.y + " : " + gameObject.transform.localScale.z);
                            SameMarker.Instance.addMovableList(new MarkerSize(gameObject, gameObject.transform.localScale.x, gameObject.transform.localScale.y, gameObject.transform.localScale.z));

                            startAnimation(gameObject);
                        }
                            
                    }

                    if (list.Count > 1)
                    {
                        HashSet<int> similarPosition = new HashSet<int>();

                        for (int i = 0; i < list.Count; i++)
                        {
                            similarPosition.Add(list[i].count_steps);
                        }

                        if (similarPosition.Count == 1)
                        {

                            if (GameManager.gm.numberOfStepsToMove != 6)
                            {
                                //waitingTimer.startTimer(1f);
                                //yield return new WaitForSeconds(1f);
                                // mqttMessage.text = "Send Message If Player is Normal User";
                                moveSingleMarkerAutomatically();
                            }
                            else
                            {
                                List<Safe> homeMarkerlist = SameMarker.Instance.getHomePlayer(GameManager.gm.playerTurn);

                                if (homeMarkerlist.Count == 0)
                                {
                                    //waitingTimer.startTimer(1f);
                                    //yield return new WaitForSeconds(1f);
                                    moveSingleMarkerAutomatically();

                                }

                            }
                        }
                        else
                        {
                            GameManager.gm.isStartTimer = false;
                            GameManager.gm.countDownStartValue = 20;
                            if (GameManager.gm.isGameRunning)
                                CountDownTimer.Instance.startTimer();
                        }
                    }
                }

                if (SameMarker.Instance.isSingleAvailableToMove(GameManager.gm.playerTurn, GameManager.gm.numberOfStepsToMove).Count == 1)
                {
                    if (GameManager.gm.numberOfStepsToMove != 6)
                    {
                        //waitingTimer.startTimer(1f);
                        //yield return new WaitForSeconds(1f);
                        moveSingleMarkerAutomatically();
                    }
                    else
                    {
                        List<Safe> homeMarkerlist = SameMarker.Instance.getHomePlayer(GameManager.gm.playerTurn);

                        if (homeMarkerlist.Count == 0)
                        {
                            //waitingTimer.startTimer(1f);
                            //yield return new WaitForSeconds(1f);
                            moveSingleMarkerAutomatically();
                        }

                    }

                }

                if (GameManager.gm.numberOfStepsToMove == 6)
                {
                    List<Safe> list = SameMarker.Instance.getHomePlayer(GameManager.gm.playerTurn);

                    if(list.Count == 1 && SameMarker.Instance.isSingleAvailableToMove(GameManager.gm.playerTurn, GameManager.gm.numberOfStepsToMove).Count == 0)
                    {
                        if (GameManager.gm.isOnlineGame)
                        {
                            if (SameMarker.Instance.getPlayerArray()[getIndex()].player_name == Credentials.email)
                            {
                                unlockFromHouse(list);

                               // waitingTimer.startTimer(1f);
                              //  yield return new WaitForSeconds(1f);

                                PlayerPieceEvent playerPieceEvent = new PlayerPieceEvent();
                                playerPieceEvent.dice_value = GameManager.gm.numberOfStepsToMove;
                                playerPieceEvent.player_chance = GameManager.gm.playerChance;
                                playerPieceEvent.sixCount = GameManager.gm.sixCount;


                                PlayEvent playEvent = new PlayEvent();
                                playEvent.action = "actionUnlockFromHouse";
                                playEvent.sender = Credentials.email;
                                playEvent.playerPieceEvent = playerPieceEvent;
                                playEvent.bot = null;

                                MqttEvent mqttEvent = new MqttEvent();
                                mqttEvent.playEvent = playEvent;

                                string json = JsonUtility.ToJson(mqttEvent);

                                Credentials.action = json;
                                m2MqttUnityTest.TestPublish();
                            }
                        }else
                        {
                            unlockFromHouse(list);
                        }
                        
                    }
                    else
                    {
                        for (int i = 0; i < list.Count; i++)
                        {
                            if (!GameManager.gm.isOnlineGame)
                                startAnimation(playerObjectList[list[i].index]);
                        }

                        if (list.Count > 0)
                        {
                            GameManager.gm.isStartTimer = false;
                            GameManager.gm.countDownStartValue = 20;
                            if (GameManager.gm.isGameRunning)
                                CountDownTimer.Instance.startTimer();
                        }

                    }
                    
                }

            }
            else
            {
// Fast ludo code
                if (GameManager.gm.isOnlineGame)
                {
                    if (SameMarker.Instance.getPlayerArray()[getIndex()].player_name == Credentials.email)
                    {
                        yield return new WaitForSeconds(1f);
                        // OLD Code
                        //PlayEvent playEvent = new PlayEvent();
                        //playEvent.action = "actionBotPlayMove";
                        //playEvent.sender = Credentials.email;
                        //playEvent.playerPieceEvent = null;
                        //playEvent.bot = null;
                        //MqttEvent mqttEvent = new MqttEvent();
                        //mqttEvent.playEvent = playEvent;
                        //string json = JsonUtility.ToJson(mqttEvent);
                        //Credentials.action = json;
                        //m2MqttUnityTest.TestPublish();
                        // old code
                        //if (GameManager.gm.numberOfStepsToMove == 6)
                        //{
                        //    List<Safe> list = SameMarker.Instance.getHomePlayer(GameManager.gm.playerTurn);
                        //    if (list.Count > 0)
                        //    {
                        //        unlockFromHouse(list);
                        //    }
                        //    else
                        //    {
                        //        if (SameMarker.Instance.isSingleAvailableToMove(GameManager.gm.playerTurn, GameManager.gm.numberOfStepsToMove).Count > 0)
                        //            moveSingleMarkerAutomatically();
                        //    }
                        //}
                        //else
                        //{
                        //mqttMessage.text = "Send Message If Player is Bot";
                        //if (SameMarker.Instance.isSingleAvailableToMove(GameManager.gm.playerTurn, GameManager.gm.numberOfStepsToMove).Count > 0)
                        //    moveSingleMarkerAutomatically();
                        //}
                        // fast ludo
                        mqttMessage.text = "Send Message If Player is Bot";
                        if (SameMarker.Instance.isSingleAvailableToMove(GameManager.gm.playerTurn, GameManager.gm.numberOfStepsToMove).Count > 0)
                            moveSingleMarkerAutomatically();
                    }

                }
                else
                {
                    //if (GameManager.gm.numberOfStepsToMove == 6)
                    //{
                    //    List<Safe> list = SameMarker.Instance.getHomePlayer(GameManager.gm.playerTurn);

                    //    if (list.Count > 0)
                    //    {

                    //        unlockFromHouse(list);

                    //    }
                    //    else
                    //    {
                    //        if (SameMarker.Instance.isSingleAvailableToMove(GameManager.gm.playerTurn, GameManager.gm.numberOfStepsToMove).Count > 0)
                    //            moveSingleMarkerAutomatically();
                    //    }

                    //}
                    //else
                    //{
                    //    if (SameMarker.Instance.isSingleAvailableToMove(GameManager.gm.playerTurn, GameManager.gm.numberOfStepsToMove).Count > 0)
                    //        moveSingleMarkerAutomatically();
                    //}

                    // fast ludo
                    if (SameMarker.Instance.isSingleAvailableToMove(GameManager.gm.playerTurn, GameManager.gm.numberOfStepsToMove).Count > 0)
                        moveSingleMarkerAutomatically();
                }

            }

            if (!SameMarker.Instance.isMarkerAvailableToMove(GameManager.gm.playerTurn, GameManager.gm.dice))
            {
                if (GameManager.gm.numberOfStepsToMove != 6)
                {

                    if (GameManager.gm.playerChance > 0)
                        GameManager.gm.playerChance--;

                    StopAllAnimation();

                    GameManager.gm.numberOfStepsToMove = 0;
                    GameManager.gm.isReadyToMove = false;

                    if (GameManager.gm.isOnlineGame)
                    {
                        waitingTimer.startTimer(1.5f);
                        yield return new WaitForSeconds(1.5f);

                        if (SameMarker.Instance.getPlayerArray()[getIndex()].player_name == Credentials.email)
                        {
                            changePlayerTurn_Coroutine = StartCoroutine(userTurn1());

                            PlayEvent playEvent = new PlayEvent();
                            playEvent.action = "actionChangeUserTurnFromRollingDice1";
                            playEvent.sender = Credentials.email;
                            playEvent.playerPieceEvent = null;
                            playEvent.bot = null;

                            MqttEvent mqttEvent = new MqttEvent();
                            mqttEvent.playEvent = playEvent;

                            string json = JsonUtility.ToJson(mqttEvent);

                            Credentials.action = json;
                            m2MqttUnityTest.TestPublish();

                            /*---------------------------------------Local Storage and Server API call---------------------------------------*/
                            PlayerPieceEvent playerPieceEvent1 = new PlayerPieceEvent();
                            playerPieceEvent1.player_chance = GameManager.gm.playerChance;
                            playerPieceEvent1.sixCount = GameManager.gm.sixCount;

                            ServerPlayEvent playEvent1 = new ServerPlayEvent();
                            playEvent1.action = "actionChangeUserTurnFromRollingDice1";
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

                            /*---------------------------------------Finish Local Storage and Server API call---------------------------------------*/

                            

                        }

                    }
                    else
                    {
                        changePlayerTurn_Coroutine = StartCoroutine(userTurn1());
                    }


                }
                else
                {

                    if (SameMarker.Instance.isSingleAvailableToMove(GameManager.gm.playerTurn, GameManager.gm.numberOfStepsToMove).Count == 0 && SameMarker.Instance.getHomePlayer(GameManager.gm.playerTurn).Count == 0)
                    {

                        GameManager.gm.canDiceRoll = true;

                        StopAllAnimation();
                        GameManager.gm.playerChance = 0;
                        GameManager.gm.numberOfStepsToMove = 0;
                        GameManager.gm.isReadyToMove = false;

                        if (GameManager.gm.isOnlineGame)
                        {
                            waitingTimer.startTimer(1.5f);
                            yield return new WaitForSeconds(1.5f);

                            if (SameMarker.Instance.getPlayerArray()[getIndex()].player_name == Credentials.email)
                            {
                                PlayEvent playEvent = new PlayEvent();
                                playEvent.action = "actionChangeUserTurnFromRollingDice";
                                playEvent.sender = Credentials.email;
                                playEvent.playerPieceEvent = null;
                                playEvent.bot = null;

                                MqttEvent mqttEvent = new MqttEvent();
                                mqttEvent.playEvent = playEvent;

                                string json = JsonUtility.ToJson(mqttEvent);

                                Credentials.action = json;
                                m2MqttUnityTest.TestPublish();

                                /*---------------------------------------Local Storage and Server API call---------------------------------------*/
                                PlayerPieceEvent playerPieceEvent1 = new PlayerPieceEvent();
                                playerPieceEvent1.player_chance = GameManager.gm.playerChance;
                                playerPieceEvent1.sixCount = GameManager.gm.sixCount;

                                ServerPlayEvent playEvent1 = new ServerPlayEvent();
                                playEvent1.action = "actionChangeUserTurnFromRollingDice";
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

                                /*---------------------------------------Finish Local Storage and Server API call---------------------------------------*/

                                changePlayerTurn_Coroutine = StartCoroutine(userTurn());

                            }
                        }
                        else
                        {
                            changePlayerTurn_Coroutine = StartCoroutine(userTurn());
                        }


                    }
                }


            }

            yield return new WaitForEndOfFrame();

            if (generateRandNumOnDice_Coroutine != null)
            {
                StopCoroutine(generateRandNumOnDice_Coroutine);
            }
        }



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


    // Auto Move player marker

    public void MoveMarkerAutomatically(List<Safe> list, PathPoint[] pathPointsToMoveOn)
    {


        List<Safe> isKillingList = new List<Safe>();
        int pos = list[0].index;

        print("Pos : " + pos);

        // fast ludo
        if (SameMarker.Instance.getCountStep(pos) == 0)
        {
            pathParent.Pieces[pos].MakePlayerReadyToMoveFastLudo();
        }
        //end 

        int Countstep = GameManager.gm.numberOfStepsToMove;

        int step = SameMarker.Instance.getCountStep(pos) + GameManager.gm.numberOfStepsToMove;

        if (!pathParent.Pieces[pos].checkSafeHouse(getNewPosition(pos)) && step < 52)
        {

            isKillingList = SameMarker.Instance.getNewPositionOpponentMarkerList(getNewPosition(pos), list[0].player_id);

        }

        pathParent.Pieces[pos].canMove = true;

        pathParent.Pieces[pos].MoveSteps(list[0].player_id, list[0].figure_id, pathPointsToMoveOn, isKillingList);
    }



    public int getNewPosition(int pos)
    {
        int new_position = SameMarker.Instance.getMarkerPosition(pos) + GameManager.gm.numberOfStepsToMove;


        if (new_position > 52)
        {
            int step = SameMarker.Instance.getCountStep(pos) + GameManager.gm.numberOfStepsToMove;
            if (step < 52)
            {
                new_position = new_position - 52;
            }

        }

        return new_position;
    }



    //Animation Handling

    public void GreenPlayerStopAnimaton()
    {

        for (int i = 8; i < 12; i++)
        {
            stopAnimaton(playerObjectList[i]);
        }
        rescaleMarker(2);

    }

    public void RedPlayerStopAnimaton()
    {
        for (int i = 4; i < 8; i++)
        {
            stopAnimaton(playerObjectList[i]);
        }

        rescaleMarker(3);
    }

    public void YellowPlayerStopAnimaton()
    {

        for (int i = 0; i < 4; i++)
        {
            stopAnimaton(playerObjectList[i]);
        }

        rescaleMarker(1);
    }

    public void BluePlayerStopAnimaton()
    {

        for (int i = 12; i < 16; i++)
        {
            stopAnimaton(playerObjectList[i]);
        }
        rescaleMarker(4);

    }

    public void StopAllAnimation()
    {
        for (int i = 0; i < 16; i++)
        {
            stopAnimaton(playerObjectList[i]);
        }

        SameMarker.Instance.clearMovableList();

    }



    // Animation Part

    public void startAnimation(GameObject gameObject)
    {
        m_Animator = gameObject.GetComponent<Animator>();
        m_Animator.GetComponent<Animator>().enabled = true;
        gameObject.transform.localScale = new Vector3(0.8f, 0.8f, 1f);
        gameObject.GetComponentInChildren<SpriteRenderer>().sortingOrder = 4;
        animationList.Add(gameObject);
        gameObject.GetComponent<BoxCollider2D>().size = new Vector2(0.6f, 0.6f);
        // GameManager.gm.animationList = animationList.ToList();
        //gameObject.GetComponent<BoxCollider2D>().size = new Vector2(0.6f, 0.5f);

    }

    public void startOnlineAnimation(GameObject gameObject)
    {
        gameObject.transform.localScale = new Vector3(0.8f, 0.8f, 1f);
        gameObject.GetComponentInChildren<SpriteRenderer>().sortingOrder = 4;
        GameManager.gm.onlineAnimationList.Add(gameObject);
        //isAnimationActive = true;
        gameObject.GetComponent<BoxCollider2D>().size = new Vector2(0.6f, 0.6f);
        m_Animator = gameObject.GetComponent<Animator>();
        m_Animator.GetComponent<Animator>().enabled = true;
    }

    public void stopAnimaton(GameObject gameObject)
    {
        isAnimationActive = false;

        m_Animator = gameObject.GetComponent<Animator>();

        if (m_Animator.GetComponent<Animator>().enabled == true)
            m_Animator.GetComponent<Animator>().enabled = false;

        gameObject.transform.eulerAngles = new Vector3(0, 0, 0);
        gameObject.GetComponent<BoxCollider2D>().size = new Vector2(0.01f, 0.01f);

        animationList.Clear();

        if (GameManager.gm.isOnlineGame)
        GameManager.gm.onlineAnimationList.Clear();

        // GameManager.gm.animationList = animationList.ToList();

    }




    public void addGameObjectInList()
    {

        //Yellow Player Object
        playerObjectList.Add(GameObject.Find("LudoHomes/YellowHome/YellowPlayerPieces1"));
        playerObjectList.Add(GameObject.Find("LudoHomes/YellowHome/YellowPlayerPieces2"));
        playerObjectList.Add(GameObject.Find("LudoHomes/YellowHome/YellowPlayerPieces3"));
        playerObjectList.Add(GameObject.Find("LudoHomes/YellowHome/YellowPlayerPieces4"));

        //Red Player Object
        playerObjectList.Add(GameObject.Find("LudoHomes/RedHome/RedPlayerPieces1"));
        playerObjectList.Add(GameObject.Find("LudoHomes/RedHome/RedPlayerPieces2"));
        playerObjectList.Add(GameObject.Find("LudoHomes/RedHome/RedPlayerPieces3"));
        playerObjectList.Add(GameObject.Find("LudoHomes/RedHome/RedPlayerPieces4"));

        //Green Player Object
        playerObjectList.Add(GameObject.Find("LudoHomes/GreenHome/GreenPlayerPieces1"));
        playerObjectList.Add(GameObject.Find("LudoHomes/GreenHome/GreenPlayerPieces2"));
        playerObjectList.Add(GameObject.Find("LudoHomes/GreenHome/GreenPlayerPieces3"));
        playerObjectList.Add(GameObject.Find("LudoHomes/GreenHome/GreenPlayerPieces4"));

        //Blue Player Object
        playerObjectList.Add(GameObject.Find("LudoHomes/BlueHome/BluePlayerPieces1"));
        playerObjectList.Add(GameObject.Find("LudoHomes/BlueHome/BluePlayerPieces2"));
        playerObjectList.Add(GameObject.Find("LudoHomes/BlueHome/BluePlayerPieces3"));
        playerObjectList.Add(GameObject.Find("LudoHomes/BlueHome/BluePlayerPieces4"));

    }

    public void addSpriteGameObjectInList()
    {
        //Green Player Sprite Object 
        playerSpriteObjectList.Add(GameObject.Find("LudoHomes/GreenHome/GreenPlayerPieces1/PlayerSprite"));
        playerSpriteObjectList.Add(GameObject.Find("LudoHomes/GreenHome/GreenPlayerPieces2/PlayerSprite"));
        playerSpriteObjectList.Add(GameObject.Find("LudoHomes/GreenHome/GreenPlayerPieces3/PlayerSprite"));
        playerSpriteObjectList.Add(GameObject.Find("LudoHomes/GreenHome/GreenPlayerPieces4/PlayerSprite"));

        //Blue Player Sprite Object 
        playerSpriteObjectList.Add(GameObject.Find("LudoHomes/BlueHome/BluePlayerPieces1/PlayerSprite"));
        playerSpriteObjectList.Add(GameObject.Find("LudoHomes/BlueHome/BluePlayerPieces2/PlayerSprite"));
        playerSpriteObjectList.Add(GameObject.Find("LudoHomes/BlueHome/BluePlayerPieces3/PlayerSprite"));
        playerSpriteObjectList.Add(GameObject.Find("LudoHomes/BlueHome/BluePlayerPieces4/PlayerSprite"));

        //Red Player Sprite Object 
        playerSpriteObjectList.Add(GameObject.Find("LudoHomes/RedHome/RedPlayerPieces1/PlayerSprite"));
        playerSpriteObjectList.Add(GameObject.Find("LudoHomes/RedHome/RedPlayerPieces2/PlayerSprite"));
        playerSpriteObjectList.Add(GameObject.Find("LudoHomes/RedHome/RedPlayerPieces3/PlayerSprite"));
        playerSpriteObjectList.Add(GameObject.Find("LudoHomes/RedHome/RedPlayerPieces4/PlayerSprite"));

        //Yellow Player Sprite Object 
        playerSpriteObjectList.Add(GameObject.Find("LudoHomes/YellowHome/YellowPlayerPieces1/PlayerSprite"));
        playerSpriteObjectList.Add(GameObject.Find("LudoHomes/YellowHome/YellowPlayerPieces2/PlayerSprite"));
        playerSpriteObjectList.Add(GameObject.Find("LudoHomes/YellowHome/YellowPlayerPieces3/PlayerSprite"));
        playerSpriteObjectList.Add(GameObject.Find("LudoHomes/YellowHome/YellowPlayerPieces4/PlayerSprite"));

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

        yield return new WaitForEndOfFrame();


        if (generateRandNumOnDice_Coroutine != null)
        {
            StopCoroutine(generateRandNumOnDice_Coroutine);
        }

    }

    IEnumerator userTurn1()
    {

        yield return new WaitForSeconds(0.25f);

        int prvTurn = GameManager.gm.playerTurn;

        GameManager.gm.canDiceRoll = true;
        if (GameManager.gm.canDiceRoll)
            GameManager.gm.playerTurn = checkPlayerTurn();

        int newTurn = GameManager.gm.playerTurn;

        bool isChangeTurn = prvTurn != newTurn;

        displayDiceHolder();


        updateBotPlayCount(prvTurn, isChangeTurn);
        mqttMessage.text = GameManager.gm.playerTurn.ToString();

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

            //  mqttMessage.text = "Player Turn 1: " + SameMarker.Instance.getPlayerArray()[getIndex()].player_name + " Login Player : " + Credentials.email;


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

        yield return new WaitForEndOfFrame();

        if (generateRandNumOnDice_Coroutine != null)
        {
            StopCoroutine(generateRandNumOnDice_Coroutine);
        }

        if (changePlayerTurn_Coroutine != null)
        {
            StopCoroutine(changePlayerTurn_Coroutine);
        }



    }

    void moveSingleMarkerAutomatically()
    {
        if(!GameManager.gm.isTapOnPlayerPiece)
        {
            GameManager.gm.isAutomaticallyMovePlayerPiece = true;
            if (GameManager.gm.isOnlineGame)
            {
                if (SameMarker.Instance.getPlayerArray()[getIndex()].player_name == Credentials.email)
                {
                        publishMoveSingleMarkerAutomatically();

                        PlayerPieceEvent playerPieceEvent = new PlayerPieceEvent();
                        playerPieceEvent.dice_value = GameManager.gm.numberOfStepsToMove;
                        playerPieceEvent.player_chance = GameManager.gm.playerChance;
                        playerPieceEvent.sixCount = GameManager.gm.sixCount;

                        PlayEvent playEvent = new PlayEvent();
                        playEvent.action = "actionMoveSingleMarkerAutomatically";
                        playEvent.sender = Credentials.email;
                        playEvent.playerPieceEvent = playerPieceEvent;
                        playEvent.bot = null;

                        MqttEvent mqttEvent = new MqttEvent();
                        mqttEvent.playEvent = playEvent;

                        string json = JsonUtility.ToJson(mqttEvent);

                        Credentials.action = json;
                        m2MqttUnityTest.TestPublish();
                
                }
            }
            else
            {
                publishMoveSingleMarkerAutomatically();
            }
        }
    }


    void unlockFromHouse(List<Safe> list)
    {
        if (!GameManager.gm.isTapOnPlayerPiece)
        {
            GameManager.gm.isAutomaticallyMovePlayerPiece = true;
            int pos = list[0].index;
            PathPoint[] pathPointsToMoveOn = pathParent.greenPathPoints;
            if (GameManager.gm.playerTurn == 1)
            {
                pathPointsToMoveOn = pathParent.yellowPathPoints;

            }
            else if (GameManager.gm.playerTurn == 3)
            {
                pathPointsToMoveOn = pathParent.redPathPoints;

            }
            else if (GameManager.gm.playerTurn == 2)
            {
                pathPointsToMoveOn = pathParent.greenPathPoints;

            }
            else if (GameManager.gm.playerTurn == 4)
            {
                pathPointsToMoveOn = pathParent.bluePathPoints;
            }


            pathParent.Pieces[pos].MakePlayerReadyToMove(list[0].player_id, list[0].figure_id, pathPointsToMoveOn);
            GameManager.gm.numberOfStepsToMove = 0;
            GameManager.gm.canDiceRoll = true;

            if (!exitManager.isShown())
            {
                if (!winnerResult.isShown())
                {
                    if (GameManager.gm.isAIPlayed)
                        GameManager.gm.isAIPlayed = false;
                }
            }
        }
    }

    public void setVisibilityFirstPlayerDice(bool status)
    {
        print("setVisibilityFirstPlayerDice : " + status);

        if (pathParent.diceHolder[0])
        {
            pathParent.diceHolder[0].SetActive(status);
        }
    }

    public void displaySaveBox()
    {
        exitManager.showBox();
    }


    public void rescaleMarker(int playerTurn)
    {
        print("Player Turn Match : " + (GameManager.gm.playerTurn == playerTurn).ToString());
        if (GameManager.gm.playerTurn == playerTurn)
        {
            if(GameManager.gm.isOnlineGame)
            {
                List<MarkerSize> movebaleObjectList = SameMarker.Instance.getOnlineMovableArray();

                for (int i = 0; i < movebaleObjectList.Count; i++)
                {
                    GameObject gameObject = movebaleObjectList[i].gameObject;
                    float x = movebaleObjectList[i].x;
                    float y = movebaleObjectList[i].y;
                    float z = movebaleObjectList[i].z;
                    gameObject.transform.localScale = new Vector3(x, y, z);
                    gameObject.GetComponentInChildren<SpriteRenderer>().sortingOrder = 0;
                }

                SameMarker.Instance.clearOnlineMovableList();
            }
            else
            {
                List<MarkerSize> movebaleObjectList = SameMarker.Instance.getMovableArray();

                print("Movable marker count" + movebaleObjectList.Count);

                for (int i = 0; i < movebaleObjectList.Count; i++)
                {
                    GameObject gameObject = movebaleObjectList[i].gameObject;
                    float x = movebaleObjectList[i].x;
                    float y = movebaleObjectList[i].y;
                    float z = movebaleObjectList[i].z;
                    gameObject.transform.localScale = new Vector3(x, y, z);
                    gameObject.GetComponentInChildren<SpriteRenderer>().sortingOrder = 0;
                }

                SameMarker.Instance.clearMovableList();
            }
            
        }

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
         StartCoroutine(enableDiceHolder());
    }

    public void loadSaveGame()
    {
        if (GameManager.gm.isSaveGame)
        {

            GameManager.gm.isSaveGame = false;

            if (GameManager.gm.playerTurn == 1)
            {
                GreenPlayerStopAnimaton();
            }
            else if (GameManager.gm.playerTurn == 2)
            {
                RedPlayerStopAnimaton();
            }
            else if (GameManager.gm.playerTurn == 3)
            {
                BluePlayerStopAnimaton();
            }
            else if (GameManager.gm.playerTurn == 4)
            {
                YellowPlayerStopAnimaton();
            }

            displayDiceHolder();

            print("Number of steps to move : " + GameManager.gm.numberOfStepsToMove);
            if (GameManager.gm.numberOfStepsToMove > 0)
            {

                if (!SameMarker.Instance.getPlayerArray()[getIndex()].isAI)
                {
                    if (SameMarker.Instance.isSingleAvailableToMove(GameManager.gm.playerTurn, GameManager.gm.numberOfStepsToMove).Count > 0)
                    {

                        if (SameMarker.Instance.getMovableArray().Count != 0)
                            SameMarker.Instance.clearMovableList();

                        List<Safe> list = SameMarker.Instance.isSingleAvailableToMove(GameManager.gm.playerTurn, GameManager.gm.numberOfStepsToMove);

                        for (int i = 0; i < list.Count; i++)
                        {
                            GameObject gameObject = playerObjectList[list[i].index];
                            if (!GameManager.gm.isOnlineGame)
                            {
                                print(gameObject.transform.localScale.x + " : " + gameObject.transform.localScale.y + " : " + gameObject.transform.localScale.z);
                                SameMarker.Instance.addMovableList(new MarkerSize(gameObject, gameObject.transform.localScale.x, gameObject.transform.localScale.y, gameObject.transform.localScale.z));

                                startAnimation(gameObject);
                            }     
                            else
                                startOnlineAnimation(gameObject);
                        }
                    }

                    if (GameManager.gm.numberOfStepsToMove == 6)
                    {
                        List<Safe> list = SameMarker.Instance.getHomePlayer(GameManager.gm.playerTurn);

                        for (int i = 0; i < list.Count; i++)
                        {
                            if (!GameManager.gm.isOnlineGame)
                                startAnimation(playerObjectList[list[i].index]);
                            else
                                startOnlineAnimation(playerObjectList[list[i].index]);
                        }
                    }
                }
                else
                {
                    if (GameManager.gm.numberOfStepsToMove == 6)
                    {

                        if (GameManager.gm.sixCount < 3)
                        {
                            List<Safe> list = SameMarker.Instance.getHomePlayer(GameManager.gm.playerTurn);

                            if (list.Count > 0)
                            {

                                unlockFromHouse(list);

                            }
                            else
                            {
                                if (SameMarker.Instance.isSingleAvailableToMove(GameManager.gm.playerTurn, GameManager.gm.numberOfStepsToMove).Count > 0)
                                    moveSingleMarkerAutomatically();
                            }
                        }

                    }
                    else
                    {
                        if (SameMarker.Instance.isSingleAvailableToMove(GameManager.gm.playerTurn, GameManager.gm.numberOfStepsToMove).Count > 0)
                            moveSingleMarkerAutomatically();
                    }
                }

            }
        }
    }

    public void botDiceRolling(int number, string action)
    {
        generateRandNumOnDice_Coroutine = StartCoroutine(GenerateRandomNumberOnDice_Enum(number, action));
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

    public void changePlayerTurn()
    {
        GameManager.gm.canDiceRoll = true;
        StopAllAnimation();
        GameManager.gm.playerChance = 0;
        GameManager.gm.numberOfStepsToMove = 0;
        changePlayerTurn_Coroutine = StartCoroutine(userTurn());
    }

    public void changePlayerTurn1()
    {
        changePlayerTurn_Coroutine = StartCoroutine(userTurn1());
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
            SameMarker.Instance.addQuitPlayer(SameMarker.Instance.getPlayerArray()[0]);

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
            SameMarker.Instance.addQuitPlayer(SameMarker.Instance.getPlayerArray()[1]);

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
            SameMarker.Instance.addQuitPlayer(SameMarker.Instance.getPlayerArray()[3]);

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

          //  List<Player> quitPlayerList = SameMarker.Instance.getQuitPlayerArray();

            SameMarker.Instance.addResult(new Results(SameMarker.Instance.getRemainingPlayerArray()[0].player_id, SameMarker.Instance.getRemainingPlayerArray()[0].username, SameMarker.Instance.getRemainingPlayerArray()[0].user_id));

            /*  for (int i = 0; i < quitPlayerList.Count; i++)
              {
                  // SameMarker.Instance.addResult(new Results(quitPlayerList[i].player_name + " (Quit Game)"));
                  SameMarker.Instance.addResult(new Results(quitPlayerList[i].player_id, quitPlayerList[i].player_name));
              }*/

            List<Player> quitPlayerList = SameMarker.Instance.getQuitPlayerArray();
            for (int i = SameMarker.Instance.getQuitPlayerArray().Count - 1; i >= 0; i--)
            {
                SameMarker.Instance.addResult(new Results(quitPlayerList[i].player_id, quitPlayerList[i].username, quitPlayerList[i].user_id));
            }


            winnerResult.showWinnerList(true);
            aPICall.callAddWinner();
            GameManager.gm.canDiceRoll = false;

            
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

    void manualDiceNumber()
    {

    }

    void publishMessage(int number, string action)
    {
        PlayEvent playEvent = new PlayEvent();
        playEvent.action = action;
        playEvent.sender = Credentials.email;
        playEvent.numberGot = number;
        playEvent.playerPieceEvent = null;
        playEvent.bot = null;

        MqttEvent mqttEvent = new MqttEvent();
        mqttEvent.playEvent = playEvent;

        string json = JsonUtility.ToJson(mqttEvent);

        Credentials.action = json;
        m2MqttUnityTest.TestPublish();
    }


    public void publishBotPlay()
    {
        if (GameManager.gm.numberOfStepsToMove == 6)
        {
            List<Safe> list = SameMarker.Instance.getHomePlayer(GameManager.gm.playerTurn);

            if (list.Count > 0)
            {

                unlockFromHouse(list);

            }
            else
            {
                if (SameMarker.Instance.isSingleAvailableToMove(GameManager.gm.playerTurn, GameManager.gm.numberOfStepsToMove).Count > 0)
                    moveSingleMarkerAutomatically();
            }

        }
        else
        {
            mqttMessage.text = "Recive Message If Player is Bot";
            if (SameMarker.Instance.isSingleAvailableToMove(GameManager.gm.playerTurn, GameManager.gm.numberOfStepsToMove).Count > 0)
                moveSingleMarkerAutomatically();
        }
    }

    public void publishMoveSingleMarkerAutomatically()
    {
        List<Safe> list = SameMarker.Instance.isSingleAvailableToMove(GameManager.gm.playerTurn, GameManager.gm.numberOfStepsToMove);
        PathPoint[] pathPointsToMoveOn = pathParent.greenPathPoints;
        if (GameManager.gm.playerTurn == 2)
        {
            pathPointsToMoveOn = pathParent.greenPathPoints;

        }
        else if (GameManager.gm.playerTurn == 4)
        {
            pathPointsToMoveOn = pathParent.bluePathPoints;

        }
        else if (GameManager.gm.playerTurn == 3)
        {
            pathPointsToMoveOn = pathParent.redPathPoints;

        }
        else if (GameManager.gm.playerTurn == 1)
        {
            pathPointsToMoveOn = pathParent.yellowPathPoints;
        }

        StopAllAnimation();
        MoveMarkerAutomatically(list, pathPointsToMoveOn);
    }

    public void publishUnlockFromHouse()
    {
        if (GameManager.gm.numberOfStepsToMove == 6)
        {
            List<Safe> list = SameMarker.Instance.getHomePlayer(GameManager.gm.playerTurn);

            if (list.Count == 1 && SameMarker.Instance.isSingleAvailableToMove(GameManager.gm.playerTurn, GameManager.gm.numberOfStepsToMove).Count == 0)
            {
                unlockFromHouse(list);
            }
        }
    }


    public void onlineGameAnimation()
    {
        if(GameManager.gm.numberOfStepsToMove > 0)
        {
            if (!SameMarker.Instance.getPlayerArray()[getIndex()].isAI)
            {
                if (SameMarker.Instance.getOnlineMovableArray().Count != 0)
                    SameMarker.Instance.clearOnlineMovableList();

                int count = GameManager.gm.sixCount;

                if (GameManager.gm.numberOfStepsToMove == 6)
                    count++;

                if (SameMarker.Instance.isSingleAvailableToMove(GameManager.gm.playerTurn, GameManager.gm.numberOfStepsToMove).Count > 1 && count < 3)
                {

                    List<Safe> list = SameMarker.Instance.isSingleAvailableToMove(GameManager.gm.playerTurn, GameManager.gm.numberOfStepsToMove);

                    HashSet<int> similarPosition = new HashSet<int>();

                    for (int i = 0; i < list.Count; i++)
                    {
                        similarPosition.Add(list[i].count_steps);
                    }

               

                            for (int i = 0; i < list.Count; i++)
                            {
                                if (similarPosition.Count != 1)
                                {
                                    GameObject gameObject = playerObjectList[list[i].index];
                                    SameMarker.Instance.addOnlineMovableList(new MarkerSize(gameObject, gameObject.transform.localScale.x, gameObject.transform.localScale.y, gameObject.transform.localScale.z));
                                    startOnlineAnimation(gameObject);
                                }
                                else
                                {
                                    List<Safe> homeList = SameMarker.Instance.getHomePlayer(GameManager.gm.playerTurn);

                                    if (homeList.Count != 0)
                                    {
                                        GameObject gameObject = playerObjectList[list[i].index];
                                        SameMarker.Instance.addOnlineMovableList(new MarkerSize(gameObject, gameObject.transform.localScale.x, gameObject.transform.localScale.y, gameObject.transform.localScale.z));
                                        startOnlineAnimation(gameObject);
                                    }
                                }
                            }
                    
                }

                if(SameMarker.Instance.isSingleAvailableToMove(GameManager.gm.playerTurn, GameManager.gm.numberOfStepsToMove).Count == 1 && count < 3 && SameMarker.Instance.getHomePlayer(GameManager.gm.playerTurn).Count != 0)
                {
                    if (GameManager.gm.numberOfStepsToMove == 6)
                    {
                        List<Safe> list = SameMarker.Instance.isSingleAvailableToMove(GameManager.gm.playerTurn, GameManager.gm.numberOfStepsToMove);
                        GameObject gameObject = playerObjectList[list[0].index];
                        SameMarker.Instance.addOnlineMovableList(new MarkerSize(gameObject, gameObject.transform.localScale.x, gameObject.transform.localScale.y, gameObject.transform.localScale.z));
                        startOnlineAnimation(gameObject);
                    }
                    
                }

                if (GameManager.gm.numberOfStepsToMove == 6 && count < 3)
                {
                    List<Safe> list = SameMarker.Instance.getHomePlayer(GameManager.gm.playerTurn);

                        for (int i = 0; i < list.Count; i++)
                        {
                            if (list.Count > 1)
                            {
                                    startOnlineAnimation(playerObjectList[list[i].index]);
                            }
                            else 
                            {
                               if (SameMarker.Instance.isSingleAvailableToMove(GameManager.gm.playerTurn, GameManager.gm.numberOfStepsToMove).Count != 0)
                               startOnlineAnimation(playerObjectList[list[i].index]);
                            }
                        }   
                }
            }
        }
        
    }

    IEnumerator openGCApplication()
    {
        yield return new WaitForSeconds(7f);

        exitManager.sendDataToGcApp();

        yield return new WaitForEndOfFrame();

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

       IEnumerator enableDiceHolder()
    {
        yield return new WaitForSeconds(1f);
        GameManager.gm.isSixGraterThanTwo = false;
    } 

}
