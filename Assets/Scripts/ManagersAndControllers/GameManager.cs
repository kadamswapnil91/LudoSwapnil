using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
   public static GameManager gm;

   public int numberOfStepsToMove;

   public int dice;
   public int sixCount;

   public int requirdDiceNumber;

   public int player;

   public int turn_id;

   public float rotation;

   public int pos;
   public bool isKilled;
   public int playerChance;
   public bool isReadyToMove;
   public bool isGameRunning;
   public bool isSixCountGraterThanTwo;

   public bool canDiceRoll;

   public int playerId;
   public int figureId;

   public int playerTurn;
   public bool isSaveGame;

   public bool isAIPlayed;
   public bool isStartTimer;

   public int botCount1;
   public int botCount2;
   public int botCount3;
   public int botCount4;

   public int countDownStartValue;

   public string firstPlayerColor;
   public string secondPlayerColor;
   public string thirdPlayerColor;
   public string fourthPlayerColor;

   public bool isDarkMode;
   public bool isMusicOn;

   public bool mqttIsConnected;
   public bool isOnlineGame;

    public string loginUserName;
    public string loginUserEmail;
    public string loginUsertoken;
    public bool isLogin;

    public bool isAPIcallProcessing;

    public bool isFromIosGcApp;
    public string flow;
    public string flow1;
    public string flow2;

    public bool isWebPageOpen;

    public string isPlayerSelected;

    public bool isBackground;

    public bool isTapOnPlayerPiece;
    public bool isAutomaticallyMovePlayerPiece;

    public bool isStartCountDownTimer;
    public bool isSixGraterThanTwo;
    public string diceStatus;

    private void Awake()
   {
   		gm = this;
   }

   public static void clearGameManager()
   {
      if(gm != null)
      gm = null;
   }

   public HashSet<GameObject> onlineAnimationList = new HashSet<GameObject>();

    List<PathPoint> playerOnPathPointsList = new List<PathPoint>(); 

   public List<JoinPlayerInfo> joinPlayer = new List<JoinPlayerInfo>();

   public void AddPathPoint(PathPoint pathPoint)
   {
      playerOnPathPointsList.Add(pathPoint);
   }

   public void RemovePathPoint(PathPoint pathPoint)
   {
      if(playerOnPathPointsList.Contains(pathPoint))
      playerOnPathPointsList.Add(pathPoint);
   }

}
