using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;
using TMPro;

// Android Fast Ludo Pro
public class PlayerPopUp : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject exitPanel , LoadButton , LudoMenuPanel, LogoutPanel, PlayerNamePanel, LoginTextButton, LogoutLoginText;
    [SerializeField] GameObject playerMarkers3, playerMarkers4;
    [SerializeField] GameObject firstPlayerNameText, secondPlayerNameText, thirdPlayerNameText, forthPlayerNameText;
    [SerializeField] GameObject firstPlayerNameTextPanel, secondPlayerNameTextPanel, thirdPlayerNameTextPanel, forthPlayerNameTextPanel;
    [SerializeField] GameObject redDiceHolder, yellowDiceHolder;
    [SerializeField] GameObject redDiceHolderImage, yellowDiceHolderImage;
    [SerializeField] GameObject redCdtImage, yellowCdtImage;
    [SerializeField] GameObject exitButton;
    [SerializeField] GameObject colorSwitch;
    [SerializeField] GameObject button2,button3,button4;
    [SerializeField] Sprite greenIcon, redIcon, blueIcon, yellowIcon;
    InputField inputFieldPlayer, inputFieldPlayer2, inputFieldPlayer3, inputFieldPlayer4;
    Button playerButton2, playerButton3, playerButton4, playerBot1,playerBot2,playerBot3,playerBot4;
    Button botOnIndicatorButton1,botOnIndicatorButton2,botOnIndicatorButton3,botOnIndicatorButton4;
    Button greenMarkerButton,redOrBlueMarkerButton,blueMarkerButton,yellowMarkerButton, toggle;
	// To merge Ludo and Ludo pro
	[SerializeField] Image logoHeaderImg;
	private Flavor flavor;

    // fast ludo
	[SerializeField] GameObject[] playerStepsCount;
	private LudoGameTimer gameTimer;
	PlayerScore playerScore;

	private TextMeshProUGUI text1,text2,text3,text4;
    private ColorBlock theColor;
    private string selectedPlayer;
    private RollingDice rollingDice;
    private CountDownTimer countDownTimer;
	
    GameSettings gameSettings;
    SpriteHandler spriteHandler;
    Button button;
    int toggleCount = 0;

    [Header("UI enabled and disabled Sprite")]
    public Sprite OffSprite1;
 	public Sprite OnSprite1;
 	public Sprite DisableEditTextOffSprite, LightDisableEditTextOffSprite;
 	public Sprite EnableEditTextOnSprite, LightEnableEditTextOnSprite;
 	public Sprite DisablePlayerSprite, LightDisablePlayerSprite;
 	public Sprite EnablePlayerSprite;
	[SerializeField] TextMeshProUGUI userName;

	//Text text1,text2,text3,text4;


	private void Awake()
 	 {
 	 	countDownTimer = FindObjectOfType<CountDownTimer>();
 	 	gameSettings = FindObjectOfType<GameSettings>();
 	 	spriteHandler = FindObjectOfType<SpriteHandler>();
		gameTimer = FindObjectOfType<LudoGameTimer>();
		playerScore = FindObjectOfType<PlayerScore>();
		flavor = FindObjectOfType<Flavor>();

 	 }


    void Start()
    {
		flavor.flavorCheck();
    	rollingDice = FindObjectOfType<RollingDice>();

        button = GetComponentInChildren<Button>();
		button.onClick.AddListener(onUiButtonClicked);

		playerButton2 = button2.GetComponent<Button>();
		playerButton3 = button3.GetComponent<Button>();
		playerButton4 = button4.GetComponent<Button>();

		greenMarkerButton = GameObject.Find("Canvas/exitPanel/Panel/greenMarkerButton").GetComponent<Button>();
		redOrBlueMarkerButton = GameObject.Find("Canvas/exitPanel/Panel/redOrBlueMarkerButton").GetComponent<Button>();
		blueMarkerButton = GameObject.Find("Canvas/exitPanel/Panel/blueMarkerButton").GetComponent<Button>();
		yellowMarkerButton = GameObject.Find("Canvas/exitPanel/Panel/yellowMarkerButton").GetComponent<Button>();

		toggle = colorSwitch.GetComponent<Button>();

		botOnIndicatorButton1 = GameObject.Find("Canvas/TimerPanels/GreenPanel/BotToUserButton1").GetComponent<Button>();
		botOnIndicatorButton2 = GameObject.Find("Canvas/TimerPanels/RedPanel/BotToUserButton2").GetComponent<Button>();
		botOnIndicatorButton3 = GameObject.Find("Canvas/TimerPanels/BluePanel/BotToUserButton3").GetComponent<Button>();
		botOnIndicatorButton4 = GameObject.Find("Canvas/TimerPanels/YellowPanel/BotToUserButton4").GetComponent<Button>();

		inputFieldPlayer = GameObject.Find("Canvas/exitPanel/Panel/InputFieldPlayer").GetComponent<InputField>();
		inputFieldPlayer2 = GameObject.Find("Canvas/exitPanel/Panel/InputFieldPlayer2").GetComponent<InputField>();
		inputFieldPlayer3 = GameObject.Find("Canvas/exitPanel/Panel/InputFieldPlayer3").GetComponent<InputField>();
		inputFieldPlayer4 = GameObject.Find("Canvas/exitPanel/Panel/InputFieldPlayer4").GetComponent<InputField>();

		

		text1 = firstPlayerNameText.GetComponent<TextMeshProUGUI>();
		text2 = secondPlayerNameText.GetComponent<TextMeshProUGUI>();
		text3 = thirdPlayerNameText.GetComponent<TextMeshProUGUI>();
		text4 = forthPlayerNameText.GetComponent<TextMeshProUGUI>();

		playerButton2.onClick.AddListener(stopInputFor2Player);
		playerButton3.onClick.AddListener(stopInputFor3Player);
		playerButton4.onClick.AddListener(stopInputFor4Player);

		theColor = playerButton2.colors;

		selectedPlayer = "4 Players";
		GameManager.gm.isPlayerSelected = selectedPlayer;

		objectSetActivie(exitPanel,false);
		objectSetActivie(LudoMenuPanel,false);
		objectSetActivie(exitButton,false);
		rollingDice.setVisibilityFirstPlayerDice(false);

		objectSetActivie(firstPlayerNameTextPanel,false);
		objectSetActivie(secondPlayerNameTextPanel,false);
		objectSetActivie(thirdPlayerNameTextPanel,false);
		objectSetActivie(forthPlayerNameTextPanel,false);
//fast ludo
		hideStepCount();

		toggle.onClick.AddListener(switchColor);

		//playerBot1 = GameObject.Find("Canvas/exitPanel/PlayerBotButton1").GetComponent<Button>();
		//playerBot2 = GameObject.Find("Canvas/exitPanel/PlayerBotButton2").GetComponent<Button>();
		//playerBot3 = GameObject.Find("Canvas/exitPanel/PlayerBotButton3").GetComponent<Button>();
		//playerBot4 = GameObject.Find("Canvas/exitPanel/PlayerBotButton4").GetComponent<Button>();
		//loadButton = LoadButton.GetComponent<Button>();

		//playerBot1.onClick.AddListener(() => ChangeImage(playerBot1));
		//playerBot2.onClick.AddListener(() => ChangeImage(playerBot2));
		//playerBot3.onClick.AddListener(() => ChangeImage(playerBot3));
		//playerBot4.onClick.AddListener(() => ChangeImage(playerBot4));

		//text1 = firstPlayerNameText.GetComponent<Text>();
		//text2 = secondPlayerNameText.GetComponent<Text>();
		//text3 = thirdPlayerNameText.GetComponent<Text>();
		//text4 = forthPlayerNameText.GetComponent<Text>();

		//loadButton.onClick.AddListener(openSaveDataList);

    }

    private void Update()
    {
		string path = Application.persistentDataPath + "/loginUserPath.fun";

		if (File.Exists(path) && LudoMenuPanel.activeInHierarchy)
		{
			using (Stream stream = File.Open(path, FileMode.Open))
			{
				var bformatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

				LoginUser loginUser = (LoginUser)bformatter.Deserialize(stream);

				if (loginUser != null)
				{
					if (loginUser.username != null && loginUser.username != "")
						userName.text = loginUser.username;
					else if (loginUser.last_name != null && loginUser.last_name != "")
						userName.text = loginUser.first_name + " " + loginUser.last_name;
					else if(loginUser.first_name != null && loginUser.first_name != "")
						userName.text = loginUser.first_name;
					else
						userName.text = loginUser.email;
				}
			}
		}
	}

    void onUiButtonClicked()
	{
	// Fast Ludo - unlock if game is online (Rm Code for local)
		SameMarker.Instance.updateCountStepsForOnlineGame();
		GameManager.gm.isKilled = false;
		SoundManager.PlaySound("click");
		GameManager.gm.isOnlineGame = false;
		spriteHandler.changeBoardSprite(toggleCount);
		rollingDice.setVisibilityFirstPlayerDice(true);
		AddPlayerList();
		objectSetActivie(exitPanel,false);
		objectSetActivie(LudoMenuPanel,false);
		GameManager.gm.isGameRunning = true;
		GameManager.gm.countDownStartValue = 20;
		CountDownTimer.Instance.startTimer();
		gameSettings.showExitButton();
		objectSetActivie(exitButton,true);
		// fast ludo timer	
		gameTimer.StartTimer();
	}

	void stopInputFor2Player()
	{
			objectInputFieldInteractable(inputFieldPlayer2,false);
			objectInputFieldInteractable(inputFieldPlayer4,false);

			InputFieldChangeImage(inputFieldPlayer2,false);
			InputFieldChangeImage(inputFieldPlayer4,false);

		//playerButton2.interactable = false;
		//playerButton3.interactable = true;
		//playerButton4.interactable = true;

		if (GameManager.gm.isDarkMode)
		{
			playerButton2.image.sprite = EnablePlayerSprite;
			playerButton3.image.sprite = DisablePlayerSprite;
			playerButton4.image.sprite = DisablePlayerSprite;
		}
		else
        {
			playerButton2.image.sprite = EnablePlayerSprite;
			playerButton3.image.sprite = LightDisablePlayerSprite;
			playerButton4.image.sprite = LightDisablePlayerSprite;
		}

			//playerBot3.interactable = false;
			//playerBot4.interactable = false;

			redOrBlueMarkerButton.interactable = false;
			yellowMarkerButton.interactable = false;

			objectSetActivie(playerMarkers3,false);
			objectSetActivie(playerMarkers4,false);
			objectSetActivie(redDiceHolder,false);
			objectSetActivie(yellowDiceHolder,false);
			objectSetActivie(redDiceHolderImage,false);
			objectSetActivie(yellowDiceHolderImage,false);
			objectSetActivie(redCdtImage,false);
			objectSetActivie(yellowCdtImage,false);

			objectSetActivie(secondPlayerNameText,false);
			objectSetActivie(forthPlayerNameText,false);

			selectedPlayer = "2 Players";
			GameManager.gm.isPlayerSelected = selectedPlayer;

			//buttonEnabledorDisable();
	}

	void stopInputFor3Player()
	{
			objectInputFieldInteractable(inputFieldPlayer2,true);
			objectInputFieldInteractable(inputFieldPlayer4,false);

			InputFieldChangeImage(inputFieldPlayer2,true);
			InputFieldChangeImage(inputFieldPlayer4,false);

		//playerButton2.interactable = true;
		//playerButton3.interactable = false;
		//playerButton4.interactable = true;

		if (GameManager.gm.isDarkMode)
		{
			playerButton2.image.sprite = DisablePlayerSprite;
			playerButton3.image.sprite = EnablePlayerSprite;
			playerButton4.image.sprite = DisablePlayerSprite;
		}
		else
        {
			playerButton2.image.sprite = LightDisablePlayerSprite;
			playerButton3.image.sprite = EnablePlayerSprite;
			playerButton4.image.sprite = LightDisablePlayerSprite;
		}

			//playerBot3.interactable = true;
			//playerBot4.interactable = false;

			redOrBlueMarkerButton.interactable = true;
			yellowMarkerButton.interactable = false;

			objectSetActivie(playerMarkers3,true);
			objectSetActivie(playerMarkers4,false);
			objectSetActivie(redDiceHolder,false);
			objectSetActivie(yellowDiceHolder,false);
			objectSetActivie(redDiceHolderImage,true);
			objectSetActivie(yellowDiceHolderImage,false);
			objectSetActivie(redCdtImage,true);
			objectSetActivie(yellowCdtImage,false);

			objectSetActivie(secondPlayerNameText,true);
			objectSetActivie(forthPlayerNameText,false);

			selectedPlayer = "3 Players";
			GameManager.gm.isPlayerSelected = selectedPlayer;

		//buttonEnabledorDisable();
	}

	void stopInputFor4Player()
	{
			objectInputFieldInteractable(inputFieldPlayer2,true);
			objectInputFieldInteractable(inputFieldPlayer4,true);

			InputFieldChangeImage(inputFieldPlayer2,true);
			InputFieldChangeImage(inputFieldPlayer4,true);

		if (GameManager.gm.isDarkMode)
		{
			playerButton2.image.sprite = DisablePlayerSprite;
			playerButton3.image.sprite = DisablePlayerSprite;
			playerButton4.image.sprite = EnablePlayerSprite;
		}
		else
        {
			playerButton2.image.sprite = LightDisablePlayerSprite;
			playerButton3.image.sprite = LightDisablePlayerSprite;
			playerButton4.image.sprite = EnablePlayerSprite;
		}
			//playerButton2.interactable = true;
			//playerButton3.interactable = true;
			//playerButton4.interactable = false;

			//playerBot3.interactable = true;
			//playerBot4.interactable = true;

			redOrBlueMarkerButton.interactable = true;
			yellowMarkerButton.interactable = true;

			objectSetActivie(playerMarkers3,true);
			objectSetActivie(playerMarkers4,true);
			objectSetActivie(redDiceHolder,false);
			objectSetActivie(yellowDiceHolder,false);
			objectSetActivie(redDiceHolderImage,true);
			objectSetActivie(yellowDiceHolderImage,true);
			objectSetActivie(redCdtImage,true);
			objectSetActivie(yellowCdtImage,true);

			objectSetActivie(secondPlayerNameText,true);
			objectSetActivie(forthPlayerNameText,true);

			selectedPlayer = "4 Players";
			GameManager.gm.isPlayerSelected = selectedPlayer;

		//buttonEnabledorDisable();
	}


// fast ludo 
	void hideStepCount()
	{
		for (int i = 0; i < playerStepsCount.Length; i++)
		{
			objectSetActivie(playerStepsCount[i], false);
			Debug.Log("*************** Check player steps hide or not ***********************");
		}
	}

	void showStepCount(bool isFirstPlayerEnable, bool isSecondPlayerEnable, bool isThirdPlayerEnable, bool isForthPlayerEnable)
	{
		if (isFirstPlayerEnable)
		{
			objectSetActivie(playerStepsCount[0], true);
			Debug.Log("*************** 2Player Game, 2nd player steps hide ***********************");
		}
		if (isSecondPlayerEnable)
		{
			objectSetActivie(playerStepsCount[1], true);
			Debug.Log("*************** 3Player Game, 2nd player steps hide ***********************");
		}
		if (isThirdPlayerEnable)
		{
			objectSetActivie(playerStepsCount[2], true);
			Debug.Log("*************** 3Player Game, 3rd player steps hide ***********************");
		}
		if (isForthPlayerEnable)
		{
			objectSetActivie(playerStepsCount[3], true);
			Debug.Log("*************** 4Player Game, all steps show ***********************");
		}
	}




	void objectSetActivie(GameObject gameObject, bool status)
	{
		if(gameObject)
		{
			gameObject.SetActive(status);
		}
	}

	void objectInputFieldInteractable(InputField inputField, bool status)
	{
		if(inputField != null)
        {
			inputField.enabled = status;
			inputField.interactable = status;
		}
	}

	void AddPlayerList()
	{
		string playerName1, playerName2, playerName3, playerName4;

		if(inputFieldPlayer.text == "" || inputFieldPlayer.text == null)
		{
			//if(playerBotIsActivie(playerBot1))
			//	playerName1 = "Computer 1";
			//else
				playerName1 = "Player 1";
		}
		else
		{
			playerName1 = inputFieldPlayer.text;
		}

		if(inputFieldPlayer2.text == "" || inputFieldPlayer2.text == null)
		{
			//if(playerBotIsActivie(playerBot2))
			//	playerName2 = "Computer 2";
			//else
				playerName2 = "Player 2";
		}
		else
		{
			playerName2 = inputFieldPlayer2.text;
		}

		if(inputFieldPlayer3.text == "" || inputFieldPlayer3.text == null)
		{
			//if(playerBotIsActivie(playerBot3))
			//	playerName3 = "Computer 3";
			//else
				playerName3 = "Player 3";
		}
		else
		{
			 playerName3 = inputFieldPlayer3.text;
		}

		if(inputFieldPlayer4.text == "" || inputFieldPlayer4.text == null)
		{
			//if(playerBotIsActivie(playerBot4))
			//	playerName4 = "Computer 4";
			//else
				playerName4 = "Player 4";
		}
		else
		{
			playerName4 = inputFieldPlayer4.text;
		}
		

		if(selectedPlayer == "2 Players")
		{
			SameMarker.Instance.clearPlayerList();
			//SameMarker.Instance.addPlayer(new Player(1,playerName1, playerBotIsActivie(playerBot1), false, false, false));
			//SameMarker.Instance.addPlayer(new Player(2,playerName2, playerBotIsActivie(playerBot2), false, false, false));

			SameMarker.Instance.addPlayer(new Player(1,playerName1, false, false, false, false, GameManager.gm.firstPlayerColor,0, playerName1));
			SameMarker.Instance.addPlayer(new Player(2,playerName3, false, false, false, false, GameManager.gm.thirdPlayerColor,0, playerName3));

			objectSetActivie(firstPlayerNameTextPanel,true);
			objectSetActivie(thirdPlayerNameTextPanel,true);
			showStepCount(true, false, true, false);

			text1.text = playerName1;
			text3.text = playerName3;
		}
		else if(selectedPlayer == "3 Players")
		{
			SameMarker.Instance.clearPlayerList();
			//SameMarker.Instance.addPlayer(new Player(1,playerName1, playerBotIsActivie(playerBot1), false, false, false));
			//SameMarker.Instance.addPlayer(new Player(2,playerName2, playerBotIsActivie(playerBot2), false, false, false));
			//SameMarker.Instance.addPlayer(new Player(3,playerName3, playerBotIsActivie(playerBot3), false, false, false));

			SameMarker.Instance.addPlayer(new Player(1,playerName1, false, false, false, false, GameManager.gm.firstPlayerColor,0, playerName1));
			SameMarker.Instance.addPlayer(new Player(2,playerName2, false, false, false, false, GameManager.gm.secondPlayerColor,0, playerName2));
			SameMarker.Instance.addPlayer(new Player(3,playerName3, false, false, false, false, GameManager.gm.thirdPlayerColor,0, playerName3));

			objectSetActivie(firstPlayerNameTextPanel,true);
			objectSetActivie(secondPlayerNameTextPanel,true);
			objectSetActivie(thirdPlayerNameTextPanel,true);
			showStepCount(true, true, true, false);

			text1.text = playerName1;
			text2.text = playerName2;
			text3.text = playerName3;
		}
		else if(selectedPlayer == "4 Players")
		{
			SameMarker.Instance.clearPlayerList();
			//SameMarker.Instance.addPlayer(new Player(1,playerName1, playerBotIsActivie(playerBot1), false, false, false));
			//SameMarker.Instance.addPlayer(new Player(2,playerName2, playerBotIsActivie(playerBot2), false, false, false));
			//SameMarker.Instance.addPlayer(new Player(3,playerName3, playerBotIsActivie(playerBot3), false, false, false));
			//SameMarker.Instance.addPlayer(new Player(4,playerName4, playerBotIsActivie(playerBot4), false, false, false));

			SameMarker.Instance.addPlayer(new Player(1,playerName1, false, false, false, false, GameManager.gm.firstPlayerColor,0, playerName1));
			SameMarker.Instance.addPlayer(new Player(2,playerName2, false, false, false, false, GameManager.gm.secondPlayerColor,0, playerName2));
			SameMarker.Instance.addPlayer(new Player(3,playerName3, false, false, false, false, GameManager.gm.thirdPlayerColor,0, playerName3));
			SameMarker.Instance.addPlayer(new Player(4,playerName4, false, false, false, false, GameManager.gm.fourthPlayerColor,0, playerName4));

			objectSetActivie(firstPlayerNameTextPanel,true);
			objectSetActivie(secondPlayerNameTextPanel,true);
			objectSetActivie(thirdPlayerNameTextPanel,true);
			objectSetActivie(forthPlayerNameTextPanel,true);
			showStepCount(true, true, true, true);

			text1.text = playerName1;
			text2.text = playerName2;
			text3.text = playerName3;
			text4.text = playerName4;
		}

	}

	
	public void LoadPlayerList(List<Player> playerList)
	{
		int playerSize = playerList.Count;
		
		if(playerSize == 2)
		{
			SameMarker.Instance.clearPlayerList();
			SameMarker.Instance.addPlayer(new Player(1,playerList[0].player_name,playerList[0].isAI, playerList[0].isConvertedToBot, playerList[0].isQuit, playerList[0].isWinner, GameManager.gm.firstPlayerColor,0, playerList[0].player_name));
			SameMarker.Instance.addPlayer(new Player(2,playerList[1].player_name, playerList[1].isAI, playerList[1].isConvertedToBot,playerList[1].isQuit, playerList[1].isWinner, GameManager.gm.thirdPlayerColor,0, playerList[1].player_name));

			text1.text = playerList[0].player_name;
			text3.text = playerList[1].player_name;
			text2.text = "";
			text4.text = "";

			objectSetActivie(playerMarkers3,false);
			objectSetActivie(playerMarkers4,false);

			objectSetActivie(redDiceHolder,false);
			objectSetActivie(yellowDiceHolder,false);

			objectSetActivie(redDiceHolderImage,false);
			objectSetActivie(yellowDiceHolderImage,false);

			objectSetActivie(redCdtImage,false);
			objectSetActivie(yellowCdtImage,false);

			isBotImage(botOnIndicatorButton1,playerList[0].isConvertedToBot);
			isBotImage(botOnIndicatorButton3,playerList[1].isConvertedToBot);
		}
		else if(playerSize == 3)
		{
			SameMarker.Instance.clearPlayerList();
			SameMarker.Instance.addPlayer(new Player(1,playerList[0].player_name,playerList[0].isAI, playerList[0].isConvertedToBot, playerList[0].isQuit, playerList[0].isWinner, GameManager.gm.firstPlayerColor,0, playerList[0].player_name));
			SameMarker.Instance.addPlayer(new Player(2,playerList[1].player_name, playerList[1].isAI, playerList[1].isConvertedToBot,playerList[1].isQuit, playerList[1].isWinner, GameManager.gm.secondPlayerColor,0, playerList[1].player_name));
			SameMarker.Instance.addPlayer(new Player(3,playerList[2].player_name, playerList[2].isAI, playerList[2].isConvertedToBot,playerList[2].isQuit, playerList[2].isWinner, GameManager.gm.thirdPlayerColor,0, playerList[2].player_name));

			text1.text = playerList[0].player_name;
			text2.text = playerList[1].player_name;
			text3.text = playerList[2].player_name;
			text4.text = "";

			objectSetActivie(playerMarkers3,true);
			objectSetActivie(playerMarkers4,false);
			objectSetActivie(redDiceHolder,true);
			objectSetActivie(yellowDiceHolder,false);
			objectSetActivie(redDiceHolderImage,true);
			objectSetActivie(yellowDiceHolderImage,false);

			objectSetActivie(redCdtImage,true);
			objectSetActivie(yellowCdtImage,false);

			isBotImage(botOnIndicatorButton1,playerList[0].isConvertedToBot);
			isBotImage(botOnIndicatorButton2,playerList[1].isConvertedToBot);
			isBotImage(botOnIndicatorButton3,playerList[2].isConvertedToBot);
		}
		else if(playerSize == 4)
		{
			SameMarker.Instance.clearPlayerList();
			SameMarker.Instance.addPlayer(new Player(1,playerList[0].player_name,playerList[0].isAI, playerList[0].isConvertedToBot, playerList[0].isQuit, playerList[0].isWinner, GameManager.gm.firstPlayerColor,0, playerList[0].player_name));
			SameMarker.Instance.addPlayer(new Player(2,playerList[1].player_name, playerList[1].isAI, playerList[1].isConvertedToBot,playerList[1].isQuit, playerList[1].isWinner, GameManager.gm.secondPlayerColor,0, playerList[1].player_name));
			SameMarker.Instance.addPlayer(new Player(3,playerList[2].player_name, playerList[2].isAI, playerList[2].isConvertedToBot,playerList[2].isQuit, playerList[2].isWinner, GameManager.gm.thirdPlayerColor,0, playerList[2].player_name));
			SameMarker.Instance.addPlayer(new Player(4,playerList[3].player_name, playerList[3].isAI, playerList[3].isConvertedToBot,playerList[3].isQuit, playerList[3].isWinner, GameManager.gm.fourthPlayerColor,0, playerList[3].player_name));

			text1.text = playerList[0].player_name;
			text2.text = playerList[1].player_name;
			text3.text = playerList[2].player_name;
			text4.text = playerList[3].player_name;

			objectSetActivie(playerMarkers3,true);
			objectSetActivie(playerMarkers4,true);
			objectSetActivie(redDiceHolder,true);
			objectSetActivie(yellowDiceHolder,true);
			objectSetActivie(redDiceHolderImage,true);
			objectSetActivie(yellowDiceHolderImage,true);
			objectSetActivie(redCdtImage,true);
			objectSetActivie(yellowCdtImage,true);
			isBotImage(botOnIndicatorButton1,playerList[0].isConvertedToBot);
			isBotImage(botOnIndicatorButton2,playerList[1].isConvertedToBot);
			isBotImage(botOnIndicatorButton3,playerList[2].isConvertedToBot);
			isBotImage(botOnIndicatorButton4,playerList[3].isConvertedToBot);

		}

		GameManager.gm.isGameRunning = true;

	}
	

	public void showPlayerPopup()
	{
		objectSetActivie(LogoutPanel, true);
		objectSetActivie(PlayerNamePanel, true);
		objectSetActivie(LoginTextButton, false);
		objectSetActivie(exitPanel,true);
		objectSetActivie(LudoMenuPanel,true);
		//rollingDice.setVisibilityFirstPlayerDice(true);
	}

	public void showPlayerPopupToGuest()
    {
		objectSetActivie(exitPanel, true);
		objectSetActivie(LudoMenuPanel, true);
		objectSetActivie(LogoutPanel, false);
		objectSetActivie(LogoutLoginText, true);
		objectSetActivie(PlayerNamePanel, false);
		objectSetActivie(LoginTextButton, true);
	}

	public GameObject getPanelObject()
	{
	    return exitPanel;
	}

	public void ChangeImage(Button botButton){

     if (botButton.image.sprite == OnSprite1)
         botButton.image.sprite = OffSprite1;
     else {
         botButton.image.sprite = OnSprite1;
     }

     buttonEnabledorDisable();
 }


 public void InputFieldChangeImage(InputField inputField, bool status){


	if(GameManager.gm.isDarkMode)
	{
		if (status)
			inputField.image.sprite = EnableEditTextOnSprite;
		else
		{
			inputField.image.sprite = DisableEditTextOffSprite;
		}
	}
	else
    {
			if (status)
				inputField.image.sprite = LightEnableEditTextOnSprite;
			else
			{
				inputField.image.sprite = LightDisableEditTextOffSprite;
			}
	}


	}

 public bool playerBotIsActivie(Button botButton)
 {
 	return botButton.image.sprite == OnSprite1;
 }

 public void isBotImage(Button botButton,bool status)
 {
 	if(status)
 	botButton.image.sprite = OffSprite1;
 }

 void buttonEnabledorDisable()
 {

 		if(selectedPlayer == "2 Players")
		{
			if(playerBotIsActivie(playerBot1) && playerBotIsActivie(playerBot2))
			{
					button.interactable = false;
			}
			else
			{
					button.interactable = true;
			}
		}
		else if(selectedPlayer == "3 Players")
		{
			if(playerBotIsActivie(playerBot1) && playerBotIsActivie(playerBot2) && playerBotIsActivie(playerBot3))
			{
					button.interactable = false;
			}
			else
			{
					button.interactable = true;
			}
		}
		else if(selectedPlayer == "4 Players")
		{
			if(playerBotIsActivie(playerBot1) && playerBotIsActivie(playerBot2) && playerBotIsActivie(playerBot3) && playerBotIsActivie(playerBot4))
			{
					button.interactable = false;
			}
			else
			{
					button.interactable = true;
			}
		}
 }

 void switchColor()
 {
 	toggleCount++;

 	if(toggleCount > 4)
 	toggleCount = 1;

 	if(toggleCount == 1)
 	{
 		greenMarkerButton.image.sprite = yellowIcon;
		redOrBlueMarkerButton.image.sprite = greenIcon;
		blueMarkerButton.image.sprite = redIcon;
		yellowMarkerButton.image.sprite = blueIcon;
 	}
 	else if(toggleCount == 2)
 	{
 		greenMarkerButton.image.sprite = blueIcon;
		redOrBlueMarkerButton.image.sprite = yellowIcon;
		blueMarkerButton.image.sprite = greenIcon;
		yellowMarkerButton.image.sprite = redIcon;
 	}
 	else if(toggleCount == 3)
 	{
 		greenMarkerButton.image.sprite = redIcon;
		redOrBlueMarkerButton.image.sprite = blueIcon;
		blueMarkerButton.image.sprite = yellowIcon;
		yellowMarkerButton.image.sprite = greenIcon;
 	}
 	else if(toggleCount == 4)
 	{
 		greenMarkerButton.image.sprite = greenIcon;
		redOrBlueMarkerButton.image.sprite = redIcon;
		blueMarkerButton.image.sprite = blueIcon;
		yellowMarkerButton.image.sprite = yellowIcon;
 	}
// Fast Ludo
		playerScore.updateColor(toggleCount);
 }


	public void switchTheme()
    {
		if(GameManager.gm.isPlayerSelected == "2 Players")
        {
			stopInputFor2Player();
        }
		else if(GameManager.gm.isPlayerSelected == "3 Players")
        {
			stopInputFor3Player();
		}
		else
        {
			stopInputFor4Player();
		}
    }


}
