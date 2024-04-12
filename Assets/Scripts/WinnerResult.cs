using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

public class WinnerResult : MonoBehaviour
{
	[SerializeField] GameObject resultPanel, thirdwinnerPanel, lastWinnerPanel, closeButtonPanel;
    [SerializeField] GameObject firstwinner, secondwinner, thirdwinner, fourthwinner; //, buttonExit, buttonExitGame;
    Text text1,text2,text3,text4;
    Button okButton,exitButton,exitGameButton;
    Coroutine quit_Coroutine;

    [SerializeField] GameObject FirstWinnerTimer, SecondTimer, ThirdWinnerTimer, FourthWinnerTimer;
    [SerializeField] Sprite greenTimerSprite, redTimerSprite, blueTimerSprite, yellowTimerSprite;

    [SerializeField] GameObject FirstWinnerPlayerImage, SecondWinnerPlayerImage, ThirdWinnerPlayerImage, FourthWinnerPlayerImage;
    [SerializeField] Sprite firstPlayerImage, secondPlayerImage, thirdPlayerImage, fourthTimerSprite;

	[SerializeField] Button closeButton;
	List<GameObject> winnerTimer;
    List<GameObject> WinnerPlayerImage;
    List<Sprite> PlayerImage;
	public ExitManager exitManager;

	APICall aPICall;

	// Start is called before the first frame update
	void Start()
    {
        text1 = firstwinner.GetComponent<Text>();
		text2 = secondwinner.GetComponent<Text>();
		text3 = thirdwinner.GetComponent<Text>();
		text4 = fourthwinner.GetComponent<Text>();

		winnerTimer = new List<GameObject>();

		WinnerPlayerImage = new List<GameObject>();

		PlayerImage = new List<Sprite>();

		PlayerImage.Add(firstPlayerImage);
		PlayerImage.Add(secondPlayerImage);
		PlayerImage.Add(thirdPlayerImage);
		PlayerImage.Add(fourthTimerSprite);

		//okButton = GameObject.Find("Canvas/ResultPanel/Panel/OkButton").GetComponent<Button>();
		//okButton.onClick.AddListener(closeResultPanel);

		//exitButton = GameObject.Find("Canvas/ResultPanel/Panel/Exit").GetComponent<Button>();
		//exitButton.onClick.AddListener(exitGame);

		//exitGameButton = GameObject.Find("Canvas/ResultPanel/Panel/ExitGame").GetComponent<Button>();
		//exitGameButton.onClick.AddListener(exitGame);

		closeButton.onClick.AddListener(exitGame);

		closeResultPanel();
    }


	private void Awake()
	{
		aPICall = FindObjectOfType<APICall>();
		exitManager = FindObjectOfType<ExitManager>();
	}

	public void showWinnerList(bool isFromQuit)
    {
		if(GameManager.gm.isOnlineGame)
        {
			closeButtonPanel.SetActive(false);
		}
		else
        {
			closeButtonPanel.SetActive(true);
		}

    	List<Results> resultList = SameMarker.Instance.getResultArray();
		if(resultList.Count > 0)
		{
			objectSetActivie(resultPanel,true);

			if(resultList.Count == 1)
			{
				objectSetActivie(firstwinner,true);
			}
			else if(resultList.Count == 2)
			{
				objectSetActivie(firstwinner,true);
				objectSetActivie(secondwinner,true);
				objectSetActivie(thirdwinnerPanel,false);
				objectSetActivie(lastWinnerPanel,false);

				winnerTimer.Add(FirstWinnerTimer);
				winnerTimer.Add(SecondTimer);

				WinnerPlayerImage.Add(FirstWinnerPlayerImage);
				WinnerPlayerImage.Add(SecondWinnerPlayerImage);

				PlayerImage.Add(firstPlayerImage);
				PlayerImage.Add(thirdPlayerImage);
			}
			else if(resultList.Count == 3)
			{
				objectSetActivie(firstwinner,true);
				objectSetActivie(secondwinner,true);
				objectSetActivie(thirdwinner,true);
				objectSetActivie(thirdwinnerPanel,true);
				objectSetActivie(lastWinnerPanel,false);

				winnerTimer.Add(FirstWinnerTimer);
				winnerTimer.Add(SecondTimer);
				winnerTimer.Add(ThirdWinnerTimer);

				WinnerPlayerImage.Add(FirstWinnerPlayerImage);
				WinnerPlayerImage.Add(SecondWinnerPlayerImage);
				WinnerPlayerImage.Add(ThirdWinnerPlayerImage);

				PlayerImage.Add(firstPlayerImage);
				PlayerImage.Add(secondPlayerImage);
				PlayerImage.Add(thirdPlayerImage);
			}
			else if(resultList.Count == 4)
			{
				objectSetActivie(firstwinner,true);
				objectSetActivie(secondwinner,true);
				objectSetActivie(thirdwinner,true);
				objectSetActivie(fourthwinner,true);
				objectSetActivie(thirdwinnerPanel,true);
				objectSetActivie(lastWinnerPanel,true);

				winnerTimer.Add(FirstWinnerTimer);
				winnerTimer.Add(SecondTimer);
				winnerTimer.Add(ThirdWinnerTimer);
				winnerTimer.Add(FourthWinnerTimer);

				WinnerPlayerImage.Add(FirstWinnerPlayerImage);
				WinnerPlayerImage.Add(SecondWinnerPlayerImage);
				WinnerPlayerImage.Add(ThirdWinnerPlayerImage);
				WinnerPlayerImage.Add(FourthWinnerPlayerImage);

				PlayerImage.Add(firstPlayerImage);
				PlayerImage.Add(secondPlayerImage);
				PlayerImage.Add(thirdPlayerImage);
				PlayerImage.Add(fourthTimerSprite);
			}


			

			//GameManager.gm.isGameRunning = false;
			setWinnerText(resultList);
			setWinnerPlayerImage();

			SoundManager.PlaySound("win");

			if(GameManager.gm.isOnlineGame)
			StartCoroutine(openGCApplication());

			if (isFromQuit)
				showFinalList();
			else
				showFinalWinnerList(resultList);

		}


    }


	IEnumerator openGCApplication()
	{

		yield return new WaitForSeconds(5f);

		exitManager.sendDataToGcApp();

		yield return new WaitForEndOfFrame();

	}


	void objectSetActivie(GameObject gameObject, bool status)
	{
		if(gameObject)
		{
			gameObject.SetActive(status);
		}
	}

	void setWinnerText(List<Results> resultList)
	{
			/*if(resultList.Count == 1)
			{
				text1.text = "1. " + resultList[0].name;
			}
			else if(resultList.Count == 2)
			{
				text1.text = "1. " + resultList[0].name;
				text2.text = "2. " + resultList[1].name;
			}
			else if(resultList.Count == 3)
			{
				text1.text = "1. " + resultList[0].name;
				text2.text = "2. " + resultList[1].name;
				text3.text = "3. " + resultList[2].name;
			}
			else if(resultList.Count == 4)
			{
				text1.text = "1. " + resultList[0].name;
				text2.text = "2. " + resultList[1].name;
				text3.text = "3. " + resultList[2].name;
				text4.text = "4. " + resultList[3].name;
			}*/

			if(resultList.Count == 1)
			{
				text1.text = resultList[0].name;
			}
			else if(resultList.Count == 2)
			{
				text1.text = resultList[0].name;
				text2.text = resultList[1].name;
			}
			else if(resultList.Count == 3)
			{
				text1.text = resultList[0].name;
				text2.text = resultList[1].name;
				text3.text = resultList[2].name;


			}
			else if(resultList.Count == 4)
			{
				text1.text = resultList[0].name;
				text2.text = resultList[1].name;
				text3.text = resultList[2].name;
				text4.text = resultList[3].name;
			}

	}

	void showFinalWinnerList(List<Results> resultList)
	{
		 SeriouslyDeleteAllSaveFiles();
		 List<string> playerNameList = new List<string>();
		 List<string> winnerName = new List<string>();
		 List<string> lastPlayerName = new List<string>();

		 List<Player> playerList = SameMarker.Instance.getPlayerArray();

		 for(int i = 0; i < playerList.Count ; i++)
		 {
		 	playerNameList.Add(playerList[i].player_name);
		 }

		 for(int i = 0; i < resultList.Count ; i++)
		 {
		 	winnerName.Add(resultList[i].name);
		 }


		var uniqueList = playerNameList.Except(winnerName);

		foreach(var difference in uniqueList)
		{
    		lastPlayerName.Add(difference);
		}

			if(playerList.Count == 2)
			{
				if(resultList.Count == playerList.Count - 1)
				{
					text1.text = "1. " + resultList[0].name;
					objectSetActivie(secondwinner,true);
					text2.text = "2. " + lastPlayerName[0];
					//objectSetActivie(buttonOk,false);
					//objectSetActivie(buttonExit,false);
					//objectSetActivie(buttonExitGame,true);
				}
				
			}
			else if(playerList.Count == 3)
			{
				if(resultList.Count == playerList.Count - 1)
				{
					text1.text = "1. " + resultList[0].name;
					text2.text = "2. " + resultList[1].name;
					objectSetActivie(thirdwinner,true);
					text3.text = "3. " + lastPlayerName[0];
					//objectSetActivie(buttonOk,false);
					//objectSetActivie(buttonExit,false);
					//objectSetActivie(buttonExitGame,true);
				}
			}
			else if(playerList.Count == 4)
			{
				if(resultList.Count == playerList.Count - 1)
				{
					text1.text = "1. " + resultList[0].name;
					text2.text = "2. " + resultList[1].name;
					text3.text = "3. " + resultList[2].name;
					objectSetActivie(fourthwinner,true);
					text4.text = "4. " + lastPlayerName[0];
					//objectSetActivie(buttonOk,false);
					//objectSetActivie(buttonExit,false);
					//objectSetActivie(buttonExitGame,true);
				}
			}
	}

	public void closeResultPanel()
	{
		//GameManager.gm.isGameRunning = true;

		if(GameManager.gm.isAIPlayed)
        GameManager.gm.isAIPlayed = false;

		objectSetActivie(resultPanel,false);

				objectSetActivie(firstwinner,false);
				objectSetActivie(secondwinner,false);
				objectSetActivie(thirdwinner,false);
				objectSetActivie(fourthwinner,false);
			//	objectSetActivie(buttonExitGame,false);

				SoundManager.PlaySound("click");
	}

	public void exitGame()
	{
		SoundManager.PlaySound("click");
		SeriouslyDeleteAllSaveFiles();
        GameManager.gm.isGameRunning = false;
        SameMarker.ClearInstance();
        GameManager.clearGameManager();
        SceneManager.LoadScene(1);
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

     public bool isShown()
	 {
	    return resultPanel.activeInHierarchy;
	 }

	 void showFinalList()
	 {
	 	//objectSetActivie(buttonOk,false);
		//objectSetActivie(buttonExit,false);
		//objectSetActivie(buttonExitGame,true);
		 SeriouslyDeleteAllSaveFiles();
	 }

	  IEnumerator quit()
      {
        yield return new WaitForSeconds(0.1f);

         SceneManager.LoadScene(0);

          yield return new WaitForEndOfFrame();

          if(quit_Coroutine != null)
          {
              StopCoroutine(quit_Coroutine);
          }
      }

      Sprite WinnerColor(string color)
      {

      	  switch(color)
	      {

	      	case "green" : return greenTimerSprite;
	      					break;

	      	case "red" :	return redTimerSprite;
	      					break;

	      	case "blue" :	return blueTimerSprite;
	      					break;

	      	case "yellow" : return yellowTimerSprite;
	      					break;

	      }

	      return greenTimerSprite;

      }

      Sprite WinnerPlayerImageSprite(int id)
      {

      	  switch(id)
	      {

	      	case 1 : return firstPlayerImage;
	      					break;

	      	case 2 : if(SameMarker.Instance.getPlayerArray().Count == 2)
	      			 return thirdPlayerImage;
	      			 else	
	      			 return secondPlayerImage;
	      					break;

	      	case 3 :	return thirdPlayerImage;
	      					break;

	      	case 4 : return fourthTimerSprite;
	      					break;

	      }

	      return firstPlayerImage;

      }


      void setWinnerPlayerImage()
      {
      		List<Results> resultList = SameMarker.Instance.getResultArray();

      		List<Player> playerList = SameMarker.Instance.getPlayerArray();

      		print("Working : "+ resultList.Count + " Player Id : "+  resultList[0].player_id);

			for(int i = 0; i < playerList.Count; i++)
				{
					for(int j = 0; j < playerList.Count; j++)
					{
						print("result player id : "+ resultList[i].player_id + " player id : "+ playerList[j].player_id);
						if(resultList[i].player_id == playerList[j].player_id)
						{
							print("Winner Color : "+ playerList[playerList[j].player_id - 1].color);
							winnerTimer[i].GetComponent<Image>().sprite = WinnerColor(playerList[playerList[j].player_id - 1].color);
							WinnerPlayerImage[i].GetComponent<Image>().sprite = WinnerPlayerImageSprite(playerList[j].player_id);
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


}
