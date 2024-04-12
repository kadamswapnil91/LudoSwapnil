using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

public class StartGamePopup : MonoBehaviour
{
	[SerializeField] GameObject startGamePanel,newGameButton,continueGameButton;
  [SerializeField] SpriteRenderer greenNumberedSpHoldere, redNumberedSpHoldere, blueNumberedSpHoldere, yellowNumberedSpHoldere;
  [SerializeField] Sprite[] numberedSprites;
	Button newGame, continueGame;
	private PlayerPopUp playerPopUp;
	private SaveDataList saveDataList;
    PathObjectsParent pathParent;
    PathPoint previousPathPoint;
    PathPoint currentPathPoint;
    RollingDice rollingDice;
    PlayerPiece playerPiece;
    private CountDownTimer countDownTimer;



    // Start is called before the first frame update
    void Start()
    {
    	newGame = newGameButton.GetComponent<Button>();
      //  continueGame = continueGameButton.GetComponent<Button>();

        newGame.onClick.AddListener(dismiss);
      //  continueGame.onClick.AddListener(openSaveDataList);
    	
      //  isLoadPlayer();
    }

     void Awake()
    {
    	playerPopUp = FindObjectOfType<PlayerPopUp>();
    	saveDataList = FindObjectOfType<SaveDataList>();

        pathParent = FindObjectOfType<PathObjectsParent>();
        rollingDice = FindObjectOfType<RollingDice>();
        playerPiece = FindObjectOfType<PlayerPiece>();


        previousPathPoint = FindObjectOfType<PathPoint>();
        currentPathPoint = FindObjectOfType<PathPoint>();

       // countDownTimer = FindObjectOfType<CountDownTimer>();
    }

    void objectSetActivie(GameObject gameObject, bool status)
    {
        if(gameObject)
        {
            gameObject.SetActive(status);
        }
    }


    void isLoadPlayer()
    {
        string path = Application.persistentDataPath + "/player.fun";
        string path1 = Application.persistentDataPath + "/playername.fun";
        string path2 = Application.persistentDataPath + "/gamemanager.fun";

        if(File.Exists(path) && File.Exists(path1) && File.Exists(path2))
        {
        	continueGame.interactable = true;

        }else{

        	continueGame.interactable = false;
        }
         
    }

    void dismiss()
    {
    	SeriouslyDeleteAllSaveFiles();
    	objectSetActivie(startGamePanel,false);
    	playerPopUp.showPlayerPopup();
      SoundManager.PlaySound("click");
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

  void openSaveDataList()
	{
		objectSetActivie(startGamePanel,false);
		LoadPlayer();
    if(GameManager.gm.isAIPlayed)
    GameManager.gm.isAIPlayed = false;

    GameManager.gm.isSaveGame = true;
    countDownTimer = CountDownTimer.Instance;
    CountDownTimer.Instance.startTimer();
    CountDownTimer.Instance.displayBotPlayedCount();
	}


    public void displayStartGamePanel()
    {
        objectSetActivie(startGamePanel,true);
        isLoadPlayer();
    }




    void LoadPlayer()
    {
        string path = Application.persistentDataPath + "/player.fun";
        string path1 = Application.persistentDataPath + "/playername.fun";
        string path2 = Application.persistentDataPath +  "/gamemanager.fun";
        string path3 = Application.persistentDataPath + "/winnerList.fun";

        if(File.Exists(path))
        {
            using (Stream stream = File.Open(path, FileMode.Open))
         {
             var bformatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
 
             List<Safe> items = (List<Safe>) bformatter.Deserialize(stream);

             if(items != null)
             {
                fetchData(items);
             }
         }

        }else{

           // marker1.text = "Path not found";
        }


        if(File.Exists(path1))
        {
            using (Stream stream1 = File.Open(path1, FileMode.Open))
         {
             var bformatter1 = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
 
             List<Player> playerList = (List<Player>) bformatter1.Deserialize(stream1);

             if(playerList != null)
             {
                fetchPlayerName(playerList);
             }
         }

        }else{

            // marker1.text = "Path not found";
        }


        if(File.Exists(path2))
        {
            using (Stream stream2 = File.Open(path2, FileMode.Open))
         {
             var bformatter2 = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
 
             GameManagerPojo gameManagerPojo = (GameManagerPojo) bformatter2.Deserialize(stream2);

             if(gameManagerPojo != null)
             {
                GameManager.gm.numberOfStepsToMove = gameManagerPojo.numberOfStepsToMove;
                GameManager.gm.dice = gameManagerPojo.dice;
                GameManager.gm.sixCount = gameManagerPojo.sixCount;
                GameManager.gm.isKilled = gameManagerPojo.isKilled;
                GameManager.gm.playerChance = gameManagerPojo.playerChance;
                GameManager.gm.isReadyToMove = gameManagerPojo.isReadyToMove;
                GameManager.gm.isGameRunning = gameManagerPojo.isGameRunning;
                GameManager.gm.isSixCountGraterThanTwo = gameManagerPojo.isSixCountGraterThanTwo;
                GameManager.gm.canDiceRoll = gameManagerPojo.canDiceRoll;
                GameManager.gm.playerTurn = gameManagerPojo.playerTurn;
                GameManager.gm.isAIPlayed = gameManagerPojo.isAIPlayed;
                GameManager.gm.countDownStartValue = gameManagerPojo.countDownStartValue;
                GameManager.gm.botCount1 = gameManagerPojo.botCount1;
                GameManager.gm.botCount2 = gameManagerPojo.botCount2;
                GameManager.gm.botCount3 = gameManagerPojo.botCount3;
                GameManager.gm.botCount4 = gameManagerPojo.botCount4;

                print(gameManagerPojo.botCount1 + " : " + gameManagerPojo.botCount2+" : "+gameManagerPojo.botCount3+" : "+ gameManagerPojo.botCount4);

                if(GameManager.gm.numberOfStepsToMove > 0)
                {
                    print("numberOfStepsToMove : "+ GameManager.gm.playerTurn);
                    if(GameManager.gm.playerTurn == 1)
                      greenNumberedSpHoldere.sprite = numberedSprites[GameManager.gm.numberOfStepsToMove - 1];
                    else if(GameManager.gm.playerTurn == 2)
                      redNumberedSpHoldere.sprite = numberedSprites[GameManager.gm.numberOfStepsToMove - 1];
                    else if(GameManager.gm.playerTurn == 3)
                      blueNumberedSpHoldere.sprite = numberedSprites[GameManager.gm.numberOfStepsToMove - 1];
                    else if(GameManager.gm.playerTurn == 4)
                      yellowNumberedSpHoldere.sprite = numberedSprites[GameManager.gm.numberOfStepsToMove - 1];
                }

                GameManager.gm.isSaveGame = true;
                rollingDice.loadSaveGame();
             }
         }

        }else{

            // marker1.text = "Path not found";
        }

        if(File.Exists(path3))
        {
            using (Stream stream3 = File.Open(path3, FileMode.Open))
         {
             var bformatter3 = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
 
             List<Results> resultList = (List<Results>) bformatter3.Deserialize(stream3);

             if(resultList != null)
             {
                SameMarker.Instance.clearResultList();

                for(int i = 0; i < resultList.Count; i++)
                {
                    SameMarker.Instance.addResult(resultList[i]);
                }
             }
         }

        }else{

            // marker1.text = "Path not found";
        }


    }



     void fetchData(List<Safe> items)
    {
        loadSaveData(items);
    }

    void fetchPlayerName(List<Player> playerList)
    {

        playerPopUp.LoadPlayerList(playerList);
        hideQuitPlayer(playerList);

    }


public void loadSaveData(List<Safe> items)
{
    SameMarker.Instance.updateList(items);
    PathPoint[] pathPointsToMoveOn = pathParent.greenPathPoints;
      

      for(int i = 0; i < 16 ; i++)
      {
        if(items[i].player_id == 1)
        {
          if(items[i].count_steps != 0)
          {
              pathPointsToMoveOn = pathParent.greenPathPoints;
              if(items[i].count_steps == 57)
              {
                    pathParent.Pieces[i].transform.position = pathPointsToMoveOn[items[i].count_steps - 1].transform.position;
              }
              else{
                    pathParent.Pieces[i].transform.position = pathPointsToMoveOn[items[i].count_steps].transform.position;
                }   
              pathParent.Pieces[i].isReady =true;
              pathParent.Pieces[i].numberOfStepsAlreadyMove = items[i].count_steps;
                pathParent.Pieces[i].currentPathPoint = pathPointsToMoveOn[pathParent.Pieces[i].numberOfStepsAlreadyMove - 1];
                pathParent.Pieces[i].currentPathPoint.AddPlayerPiece(pathParent.Pieces[i]);
                GameManager.gm.AddPathPoint(pathParent.Pieces[i].currentPathPoint);
                pathParent.Pieces[i].previousPathPoint = pathParent.Pieces[i].currentPathPoint;
          }

        }
        else if(items[i].player_id == 2)
        {
          if(items[i].count_steps != 0)
          {
              pathPointsToMoveOn = pathParent.redPathPoints;
              if(items[i].count_steps == 57)
              {
                    pathParent.Pieces[i].transform.position = pathPointsToMoveOn[items[i].count_steps - 1].transform.position;
              }
              else{
                    pathParent.Pieces[i].transform.position = pathPointsToMoveOn[items[i].count_steps].transform.position;
                }   
              pathParent.Pieces[i].isReady =true;
              pathParent.Pieces[i].numberOfStepsAlreadyMove = items[i].count_steps;
                 pathParent.Pieces[i].currentPathPoint = pathPointsToMoveOn[pathParent.Pieces[i].numberOfStepsAlreadyMove - 1];
                 pathParent.Pieces[i].currentPathPoint.AddPlayerPiece(pathParent.Pieces[i]);
                GameManager.gm.AddPathPoint( pathParent.Pieces[i].currentPathPoint);
                 pathParent.Pieces[i].previousPathPoint =  pathParent.Pieces[i].currentPathPoint;
          }

        }
        else if(items[i].player_id == 3)
        {
          if(items[i].count_steps != 0)
          {
            pathPointsToMoveOn = pathParent.bluePathPoints;
              if(items[i].count_steps == 57)
              {
                    pathParent.Pieces[i].transform.position = pathPointsToMoveOn[items[i].count_steps - 1].transform.position;
              }
              else{
                    pathParent.Pieces[i].transform.position = pathPointsToMoveOn[items[i].count_steps].transform.position;
                }   
              pathParent.Pieces[i].isReady =true;
              pathParent.Pieces[i].numberOfStepsAlreadyMove = items[i].count_steps;
                 pathParent.Pieces[i].currentPathPoint = pathPointsToMoveOn[pathParent.Pieces[i].numberOfStepsAlreadyMove - 1];
                 pathParent.Pieces[i].currentPathPoint.AddPlayerPiece(pathParent.Pieces[i]);
                GameManager.gm.AddPathPoint( pathParent.Pieces[i].currentPathPoint);
                 pathParent.Pieces[i].previousPathPoint =  pathParent.Pieces[i].currentPathPoint;

          }

        }
        else if(items[i].player_id == 4)
        {
          if(items[i].count_steps != 0)
          {
            pathPointsToMoveOn = pathParent.yellowPathPoints;
              if(items[i].count_steps == 57)
              {
                    pathParent.Pieces[i].transform.position = pathPointsToMoveOn[items[i].count_steps - 1].transform.position;
              }
              else{
                    pathParent.Pieces[i].transform.position = pathPointsToMoveOn[items[i].count_steps].transform.position;
                }   
              pathParent.Pieces[i].isReady =true;
              pathParent.Pieces[i].numberOfStepsAlreadyMove = items[i].count_steps;
                pathParent.Pieces[i].currentPathPoint = pathPointsToMoveOn[pathParent.Pieces[i].numberOfStepsAlreadyMove - 1];
                 pathParent.Pieces[i].currentPathPoint.AddPlayerPiece(pathParent.Pieces[i]);
                 GameManager.gm.AddPathPoint( pathParent.Pieces[i].currentPathPoint);
                 pathParent.Pieces[i].previousPathPoint =  pathParent.Pieces[i].currentPathPoint;
          }

        }
          
      }

}

public bool isShown()
{
    return startGamePanel.activeInHierarchy;
}

 void hideQuitPlayer(List<Player> playerList)
 {
      if(playerList.Count == 2)
      {
          if(playerList[0].isQuit)
          {
              for(int i = 0 ; i < pathParent.greenPlayerPieces.Length ; i++)
              {
                  objectSetActivie(pathParent.greenPlayerPieces[i]);
              }
          }

          if(playerList[1].isQuit)
          {
              for(int i = 0 ; i < pathParent.bluePlayerPieces.Length ; i++)
               {
                   objectSetActivie(pathParent.bluePlayerPieces[i]);
               }
          }
      }
      else if(playerList.Count == 3)
      {
          if(playerList[0].isQuit)
          {
              for(int i = 0 ; i < pathParent.greenPlayerPieces.Length ; i++)
              {
                  objectSetActivie(pathParent.greenPlayerPieces[i]);
              }
          }

          if(playerList[1].isQuit)
          {
            for(int i = 0 ; i < pathParent.redPlayerPieces.Length ; i++)
            {
                objectSetActivie(pathParent.redPlayerPieces[i]);
            }
          }

          if(playerList[2].isQuit)
          {
              for(int i = 0 ; i < pathParent.bluePlayerPieces.Length ; i++)
               {
                   objectSetActivie(pathParent.bluePlayerPieces[i]);
               }
          }
      } 
      else
      {
          if(playerList[0].isQuit)
          {
              for(int i = 0 ; i < pathParent.greenPlayerPieces.Length ; i++)
              {
                  objectSetActivie(pathParent.greenPlayerPieces[i]);
              }
          }

          if(playerList[1].isQuit)
          {
            for(int i = 0 ; i < pathParent.redPlayerPieces.Length ; i++)
            {
                objectSetActivie(pathParent.redPlayerPieces[i]);
            }
          }

          if(playerList[2].isQuit)
          {
              for(int i = 0 ; i < pathParent.bluePlayerPieces.Length ; i++)
               {
                   objectSetActivie(pathParent.bluePlayerPieces[i]);
               }
          }

          if(playerList[3].isQuit)
          {
              for(int i = 0 ; i < pathParent.yellowPlayerPieces.Length ; i++)
              {
                  objectSetActivie(pathParent.yellowPlayerPieces[i]);
              }
          }
      }
 }

 void objectSetActivie(GameObject gameObject)
{
  if(gameObject)
  {
    gameObject.SetActive(false);
  }
}



  
}
