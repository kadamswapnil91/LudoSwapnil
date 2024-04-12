using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using System.Text.RegularExpressions;
using System.IO;
using System;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;



public class Login : MonoBehaviour
{
	[SerializeField] GameObject SignUpPanel, LoginPanel, forgotPassPanel;
	[SerializeField] Button signUpButton, loginButton, showHideButton, playAsGuestButton, forgotPassButton, loginWithSklash;
	PlayerPopUp playerPopUp;
	ForgotPassword forgotPassword;

	[SerializeField] TMP_InputField inputFieldEmail, inputFieldPassword;
	//[SerializeField] InputField inputFieldEmail, inputFieldPassword;
	[SerializeField] Image emailBorder, passBorder;
	// To hide Skalsh Logo
	[SerializeField] Image logoHeaderImg, SklashLogo, backSklashLogo; 

	[SerializeField] Sprite SklashLudo1;
	public string packageName; // To decide package name
//	 private BuildType buildGame;
	private Flavor flavor;
//	[SerializeField] public Button loginWithSklash;
//	[SerializeField] public Image SklashLogo, backSklashLogo;
	[SerializeField] TextMeshProUGUI placeHolderEmail, placeHolderPass;
	[SerializeField] Image passwordImg;
	[SerializeField] Sprite showPassSprite, hidePassSprite;
	


	[SerializeField] TextMeshProUGUI message;

	string arguments = "";
	string id = "";
	string channel = "";
	string contestId = "";
	string userId = "";
	string contestName = "";
	string loginGcToken = "";
	string loginGcTokenTimeStamp = "";

	public const string MatchEmailPattern = "^[a-zA-Z0-9+_.-]+@[a-zA-Z0-9.-]+$";


	APICall aPICall;
	AndroidJavaClass UnityPlayer;
	bool isShown = false;


	// Start is called before the first frame update
	void Start()
    {
		Debug.Log("PACKAGE_NAME:******************************* Package Identifier is ************************" + packageName);
		packageName = Application.identifier; 
		string path = Application.persistentDataPath + "/loginUserPath.fun";

		if(File.Exists(path))
        {
			using (Stream stream = File.Open(path, FileMode.Open))
			{
				var bformatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

				LoginUser loginUser = (LoginUser)bformatter.Deserialize(stream);

				if (loginUser != null)
				{
					GameManager.gm.loginUserName = loginUser.first_name;
					GameManager.gm.loginUserEmail = loginUser.email;
					GameManager.gm.loginUsertoken = loginUser.token;
					GameManager.gm.isLogin = true;
					loginSuccessfully();
				}
			}
		}
		else
        {
			notLogin();
		}


		signUpButton.onClick.AddListener(openSignupScreen);
        loginButton.onClick.AddListener(login);
		showHideButton.onClick.AddListener(passwordShown);
		playAsGuestButton.onClick.AddListener(playGuest);
		loginWithSklash.onClick.AddListener(getLoginTokenFromGcApp);
		forgotPassButton.onClick.AddListener(showForgotPasswordScreen);
		flavor.flavorCheck();
		
	}

/*
// to merge code
	void showLudoUI()
	{
				
		if (packageName == "com.sklash.ludo1"){
			loginWithSklash.gameObject.SetActive(false);
			SklashLogo.gameObject.SetActive(false);
			backSklashLogo.gameObject.SetActive(false);
			logoHeaderImg.sprite = SklashLudo1;
			
		}
	}
	*/
	

	void Awake()
    {
        playerPopUp = FindObjectOfType<PlayerPopUp>();
		aPICall = FindObjectOfType<APICall>();
		forgotPassword = FindObjectOfType<ForgotPassword>();
		flavor = FindObjectOfType<Flavor>();
		
	}


	void objectSetActivie(GameObject gameObject, bool status)
	{
		if(gameObject)
		{
			gameObject.SetActive(status);
		}
	}

	void openSignupScreen()
	{
		objectSetActivie(SignUpPanel, true);
		objectSetActivie(LoginPanel, false);
	}

	void login()
	{
		string email, pass;

		email = inputFieldEmail.text;
		pass = inputFieldPassword.text;

		bool isValidPass;
		bool isValidEmail;
		if (pass == null || pass == "")
		{
			passBorder.color = new Color32(255, 0, 0, 255);
			placeHolderPass.color = Color.red;
			placeHolderPass.text = "Enter Valid Password";
			isValidPass = false;
		}
		else
		{
			passBorder.color = new Color32(255, 255, 255, 255);
			isValidPass = true;
			placeHolderPass.color = Color.white;
		}

		if (!validateEmail(email))
		{
			emailBorder.color = new Color32(255, 0, 0, 255);
			placeHolderEmail.color = Color.red;
			placeHolderEmail.text = "Enter Valid Email";
			isValidEmail = false;
		}
		else
		{
			emailBorder.color = new Color32(255, 255, 255, 255);
			isValidEmail = true;
			placeHolderEmail.color = Color.white;
		}

		if (isValidEmail && isValidPass)
		{
			aPICall.callLoginWithUserCredentials(email, pass);
		}
	}

	public GameObject getPanelObject()
	{
	    return LoginPanel;
	}

	public static bool validateEmail(string email)
	{
		if (email != null)
			return Regex.IsMatch(email, MatchEmailPattern);
		else
			return false;
	}

	public void loginSuccessfully()
    {
		GameManager.gm.isLogin = true;
		playerPopUp.showPlayerPopup();
		objectSetActivie(LoginPanel, false);
	}

	public void playGuest()
    {
		PlayerPrefs.SetInt("playAsGuest", 1);
		playerPopUp.showPlayerPopupToGuest();
		objectSetActivie(LoginPanel, false);
	}

	public void notLogin()
    {
		if (PlayerPrefs.GetInt("playAsGuest") == 0)
		{

			objectSetActivie(LoginPanel, true);
			objectSetActivie(playerPopUp.getPanelObject(), false);

#if UNITY_ANDROID
			getIntentCredentials();

			if(PlayerPrefs.GetString("loginWithSklashTimeStamp") != null)
            {
				int value = PlayerPrefs.GetInt("loginWithSklash");
				if (PlayerPrefs.GetInt("loginWithSklash") == 1)
				{
					PlayerPrefs.SetInt("loginWithSklash", 0);
					if(PlayerPrefs.GetString("loginWithSklashTimeStamp") == loginGcTokenTimeStamp)
                    {
						PlayerPrefs.GetString("loginWithSklashTimeStamp", null);
						//message.text = loginGcToken+" : "+ id +" : "+ contestId+" : "+ contestName;
						if (loginGcToken != "")
						{
							aPICall.callLogin(loginGcToken);
						}
					}else
                    {
						PlayerPrefs.GetString("loginWithSklashTimeStamp", null);
					}
					
				}
			}

#endif

#if UNITY_IOS
		message.text = Credentials.id + " : " + Credentials.contestId + " : " + Credentials.iosSklashToken;

		if (PlayerPrefs.GetInt("loginWithSklash") == 1)
		{
			PlayerPrefs.SetInt("loginWithSklash", 0);
			//message.text = loginGcToken+" : "+ id +" : "+ contestId+" : "+ contestName;
		
			if (Credentials.iosSklashToken != null && Credentials.iosSklashToken != "")
			{
				string token = Credentials.iosSklashToken;
				Credentials.iosSklashToken = null;
				aPICall.callLogin(token);
			}
		}
#endif
		}
		else
        {
			playGuest();
		}
		}

	public void passwordShown()
    {
		if(isShown)
        {
			passwordImg.sprite = hidePassSprite;
			
			if(inputFieldPassword.contentType == TMP_InputField.ContentType.Standard)
            {
				inputFieldPassword.contentType = TMP_InputField.ContentType.Password;
			}

			isShown = false;

		}
		else
        {
			passwordImg.sprite = showPassSprite;

			if (inputFieldPassword.contentType == TMP_InputField.ContentType.Password)
			{
				inputFieldPassword.contentType = TMP_InputField.ContentType.Standard;

			}

			isShown = true;
		}

		inputFieldPassword.DeactivateInputField();
		inputFieldPassword.ActivateInputField();
	}

	void showForgotPasswordScreen()
    {
		forgotPassword.clearEmailField();
		forgotPassPanel.SetActive(true);
    }


	public void getLoginTokenFromGcApp()
	{
		PlayerPrefs.SetInt("loginWithSklash", 1);
		string timeStamp = DateTime.UtcNow.Millisecond.ToString();
		PlayerPrefs.SetString("loginWithSklashTimeStamp", timeStamp);
		getIntentCredentials();

#if UNITY_ANDROID
		bool fail = false;
		string bundleId = "com.sklash"; // your target bundle id
		AndroidJavaClass up = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		AndroidJavaObject ca = up.GetStatic<AndroidJavaObject>("currentActivity");
		AndroidJavaObject packageManager = ca.Call<AndroidJavaObject>("getPackageManager");

		AndroidJavaObject launchIntent = null;
		try
		{
			launchIntent = packageManager.Call<AndroidJavaObject>("getLaunchIntentForPackage", bundleId);
			launchIntent.Call<AndroidJavaObject>("putExtra", "package_name", "com.sklash.ludopro");
			if (packageName == "com.sklash.ludo1"){
				launchIntent.Call<AndroidJavaObject>("putExtra", "package_name", "com.sklash.ludo1");
			}
			launchIntent.Call<AndroidJavaObject>("putExtra", "intent_type", "intent_request_token");
			launchIntent.Call<AndroidJavaObject>("putExtra", "screen", "SIGN_IN");
			launchIntent.Call<AndroidJavaObject>("putExtra", "tokenID", Credentials.id);
			launchIntent.Call<AndroidJavaObject>("putExtra", "contest_id_gc", Credentials.contestId);
			launchIntent.Call<AndroidJavaObject>("putExtra", "contest_name", Credentials.contestName);
			launchIntent.Call<AndroidJavaObject>("putExtra", "user_id_gc", Credentials.userId);
			launchIntent.Call<AndroidJavaObject>("putExtra", "GCPlayer", Credentials.userName);
			launchIntent.Call<AndroidJavaObject>("putExtra", "is_admin_contest", Credentials.isAdminContest);
			launchIntent.Call<AndroidJavaObject>("putExtra", "loginWithSklashTimeStamp", timeStamp);
			launchIntent.Call<AndroidJavaObject>("putExtra", "contest_size", Credentials.contestSize);
		}
		catch (System.Exception e)
		{
			string msg = e.Message.ToString();
			fail = true;
		}

		if (fail)
		{ //open app in store
		  //Application.OpenURL("https://google.com");
			aPICall.checkConnection("home");
		}
		else //open the app
		{
			ca.Call("startActivity", launchIntent);
			up.Dispose();
			ca.Dispose();
			packageManager.Dispose();
			launchIntent.Dispose();
			Application.Quit();
		}



#endif

#if UNITY_IOS
		// string url = "GameCenter://?match_id=" + Credentials.id.ToString() + "&contest_id=" + Credentials.contestId.ToString() + "&winner=" + Manager.gm.winnerPlayerName.ToString();
		// Application.OpenURL("GameCenter://?contest_id=" + Credentials.contestId.ToString());
		//message.text = "Open Sklash Url : " + "sklash://?getLaunchIntentForPackage= com.sklash&package_name = com.sklash.ludo&intent_type = intent_request_token&screen = SIGN_IN&tokenID" + Credentials.id + "&contest_id_gc" + Credentials.contestId + "&contest_name" + contestName;
		//Application.OpenURL("sklash://");
		Application.OpenURL("sklash://?getLaunchIntentForPackage=com_sklash&package_name=sklash_ludo&intent_type=intent_request_token&screen=SIGN_IN&tokenID" + Credentials.id+ "&contest_id_gc"+Credentials.contestId+ "&contest_name"+contestName);
		Application.Quit();
#endif


	}


	public void getIntentCredentials()
	{

#if UNITY_ANDROID
		UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		AndroidJavaObject currentActivity = UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

		AndroidJavaObject intent = currentActivity.Call<AndroidJavaObject>("getIntent");
		bool hasExtra = intent.Call<bool>("hasExtra", "GCPlayer");
		bool hasExtra1 = intent.Call<bool>("hasExtra", "tokenID");
		bool hasExtra2 = intent.Call<bool>("hasExtra", "MQChannel");

		AndroidJavaObject extras = intent.Call<AndroidJavaObject>("getExtras");
		arguments = extras.Call<string>("getString", "GCPlayer");
		id = extras.Call<string>("getString", "tokenID");
		contestId = extras.Call<string>("getString", "contest_id_gc");
		userId = extras.Call<string>("getString", "user_id_gc");
		contestName = extras.Call<string>("getString", "contest_name");
		loginGcToken = extras.Call<string>("getString", "intent_request_token");
		loginGcTokenTimeStamp = extras.Call<string>("getString", "loginWithSklashTimeStamp");

		//intent.Call<AndroidJavaObject>("putExtra", "GCPlayer", null);
		//intent.Call<AndroidJavaObject>("putExtra", "tokenID", null);
		//intent.Call<AndroidJavaObject>("putExtra", "MQChannel", null);
		//intent.Call<AndroidJavaObject>("putExtra", "contest_id_gc", null);
		//intent.Call<AndroidJavaObject>("putExtra", "user_id_gc", null);
		//intent.Call<AndroidJavaObject>("putExtra", "contest_name", null);
		//intent.Call<AndroidJavaObject>("putExtra", "intent_request_token", null);

#endif

		//aPICall.callMatchStatus(10053, 7153);
	}

}