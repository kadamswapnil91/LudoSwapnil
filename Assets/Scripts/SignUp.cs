using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text.RegularExpressions;



public class SignUp : MonoBehaviour
{
	[SerializeField] GameObject SignUpPanel, LoginPanel;
	[SerializeField] Button loginButton, signUpButton, showHideButton;
	PlayerPopUp playerPopUp;
	[SerializeField] TMP_InputField inputFieldName, inputFieldEmail, inputFieldPassword;
	[SerializeField] Image nameBorder, emailBorder, passBorder;
	[SerializeField] TextMeshProUGUI placeHolderName, placeHolderEmail, placeHolderPass;
	[SerializeField] Image passwordImg;
	[SerializeField] Sprite showPassSprite, hidePassSprite;
	bool isShown = false;
	// to change Ludo Normal
	[SerializeField] Sprite SklashLudo1;
	[SerializeField] Image logoHeaderImg;
//	public BuildType buildGameType;
	private Flavor flavor;
	public string packageName; 
	public const string MatchEmailPattern = "^[a-zA-Z0-9+_.-]+@[a-zA-Z0-9.-]+$";


	APICall aPICall;
	// Start is called before the first frame update
	void Start()
    {	
		packageName = Application.identifier;
        objectSetActivie(SignUpPanel, false);
        loginButton.onClick.AddListener(showLoginScreen);
        signUpButton.onClick.AddListener(signUp);
		showHideButton.onClick.AddListener(passwordShown);
		flavor.flavorCheck();
	}



     void Awake()
    {
        playerPopUp = FindObjectOfType<PlayerPopUp>();
		aPICall = FindObjectOfType<APICall>();
		flavor = FindObjectOfType<Flavor>();
	}


    void objectSetActivie(GameObject gameObject, bool status)
	{
		if(gameObject)
		{
			gameObject.SetActive(status);
		}
	}

	public void showLoginScreen()
    {
		objectSetActivie(LoginPanel, true);
		objectSetActivie(SignUpPanel, false);
	}

	public void openLoginScreen()
	{
		GameManager.gm.isLogin = true;
		playerPopUp.showPlayerPopup();
		objectSetActivie(LoginPanel, false);
		objectSetActivie(SignUpPanel, false);
	}

	void signUp()
	{
		//playerPopUp.showPlayerPopup();
		//objectSetActivie(SignUpPanel, false);
		string name, email, pass;

		name = inputFieldName.text;
		email = inputFieldEmail.text;
		pass = inputFieldPassword.text;

		bool isValidName;
		bool isValidPass;
		bool isValidEmail;

		if (name == null || name == "")
        {
			nameBorder.color = new Color32(255, 0, 0, 255);
			placeHolderName.color = Color.red;
			placeHolderName.text = "Enter Valid Name";
			isValidName = false;

		}
		else
        {
			nameBorder.color = new Color32(255, 255, 255, 255);
			isValidName = true;
			placeHolderName.color = Color.white;
		}

		if (pass == null || pass.Length< 8)
		{
			passBorder.color = new Color32(255, 0, 0, 255);
			placeHolderPass.color = Color.red;
			inputFieldPassword.text = "";
			placeHolderPass.text = "Password is too short\n(minimum is 8 characters)";
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
			inputFieldEmail.text = "";
			placeHolderEmail.text = "Enter Valid Email";
			isValidEmail = false;
		}
		else
		{
			emailBorder.color = new Color32(255, 255, 255, 255);
			isValidEmail = true;
			placeHolderEmail.color = Color.white;
		}

		if(isValidName && isValidEmail && isValidPass)
        {
			aPICall.callSignUp(name, email, pass);
        }

	}

	public GameObject getPanelObject()
	{
	    return SignUpPanel;
	}

	public static bool validateEmail(string email)
	{
		if (email != null)
			return Regex.IsMatch(email, MatchEmailPattern);
		else
			return false;
	}


	public void passwordShown()
	{
		if (isShown)
		{
			passwordImg.sprite = hidePassSprite;

			if (inputFieldPassword.contentType == TMP_InputField.ContentType.Standard)
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

}
