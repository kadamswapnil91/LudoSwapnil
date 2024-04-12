using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;


public class SaveDataList : MonoBehaviour
{
	[SerializeField] GameObject LoadData,firstText,secondText,ThirdText,FourthText,CloseButton;
    Button closeButton;
    Text marker1,marker2,marker3,marker4;
    PathObjectsParent pathParent;
    PathPoint previousPathPoint;
    PathPoint currentPathPoint;
    RollingDice rollingDice;
    PlayerPiece playerPiece;
    PlayerPopUp playerPopUp;


    // Start is called before the first frame update
    void Start()
    {
        objectSetActivie(LoadData,false);

        closeButton = CloseButton.GetComponent<Button>();
        marker1 = firstText.GetComponent<Text>();
		marker2 = secondText.GetComponent<Text>();
		marker3 = ThirdText.GetComponent<Text>();
		marker4 = FourthText.GetComponent<Text>();

		closeButton.onClick.AddListener(exit);
    }

    void Awake()
    {
    	pathParent = FindObjectOfType<PathObjectsParent>();
    	rollingDice = FindObjectOfType<RollingDice>();
    	playerPiece = FindObjectOfType<PlayerPiece>();
        playerPopUp = FindObjectOfType<PlayerPopUp>();


        previousPathPoint = FindObjectOfType<PathPoint>();
        currentPathPoint = FindObjectOfType<PathPoint>();
    }


    void objectSetActivie(GameObject gameObject, bool status)
    {
        if(gameObject)
        {
            gameObject.SetActive(status);
        }
    }

    void LoadPlayer()
    {
        string path = Application.persistentDataPath + "/player.fun";
        string path1 = Application.persistentDataPath + "/playername.fun";

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

        	marker1.text = "Path not found";
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

            marker1.text = "Path not found";
        }
         

    }

    public void showData()
    {
    	objectSetActivie(LoadData,true);
		LoadPlayer();
    }

    void fetchData(List<Safe> items)
    {
    	marker1.text = items[0].figure_id + " , "+ items[0].count_steps +  " , "+ items[0].position; 
    	marker2.text = items[1].figure_id + " , "+ items[1].count_steps +  " , "+ items[1].position;
    	marker3.text = items[2].figure_id + " , "+ items[2].count_steps +  " , "+ items[2].position;
    	marker4.text = items[3].figure_id + " , "+ items[3].count_steps +  " , "+ items[3].position;

    	loadSaveData(items);

    }

    void fetchPlayerName(List<Player> playerList)
    {

        playerPopUp.LoadPlayerList(playerList);

    }

    void exit()
    {
    	objectSetActivie(LoadData,false);
    	rollingDice.setVisibilityFirstPlayerDice(true);
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



   
}
