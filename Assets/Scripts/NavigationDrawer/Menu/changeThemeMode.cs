using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;



public class changeThemeMode : MonoBehaviour
{
    PlayerPopUp playerPopUp;

	//Setting Screen
	[SerializeField] Image backgroundPopupImage, closeIconImage, musicToggleBackgroundImage, themeToggleBackgroundImage, backgroundImage, soundImage, backgroundMusicToggleImage, backgroundWinnerScreenPopImage, bgWinnerScreen;
	[SerializeField] TextMeshProUGUI SettingTitleText, musicOnText, musicOffText, chooseThemeText, lightText, darkText;
	[SerializeField] Text indicatorText;

	[Header("Setting Light Colors")]
	[SerializeField] Color SettingTitleTextColorLight;
	[SerializeField] Color chooseThemeTextColorLight , musicOnandOffTextColorLight, lightAndDarkTextColorLight, indicatorTextColorLight, popupLight, closeLight, musicToggleColorLight, themeToggleColorLight;

	[Header("Setting Dark Colors")]
	[SerializeField] Color SettingTitleTextColorDark;
	[SerializeField] Color chooseThemeTextColorDark , musicOnandOffTextColorDark, lightAndDarkTextColorDark, indicatorTextColorDark, popupDark, closeDark, musicToggleColorDark, themeToggleColorDark;

	[Header("background Sprite Image")]
	[SerializeField] Sprite lightBackground;

	[SerializeField] Sprite darkBackground;

	[Header("Sound Sprite Image")]
	[SerializeField] Sprite lightSound;

	[SerializeField] Sprite darkSound;

	[SerializeField]
    Sprite lightOnSprite, lightOffSprite, darkOnSprite, darkOffSprite;

	[Header("Sound Toggle Background Sprite Image")]
	[SerializeField] Sprite lightSoundToggle;

	[SerializeField] Sprite darkSoundToggle;

	[SerializeField]
	private float tweenTime = 0.25f;

    [Header("Exit Screen")]
    [SerializeField]Image exitBackgroundImage;
    [SerializeField]Image exitPopupImage;

    [SerializeField]Sprite exitBackgroundImageSprite, exitBackgroundLightSprite, exitPopupLightSprite, exitPopupDarkSprite;

    [SerializeField] TextMeshProUGUI ExitTitleText;
    [SerializeField] Color exitLightColor , exitDarkColor;
    // Start is called before the first frame update

    [Header("Main Game Screen")]
    [SerializeField] Color mainColorBackgroundLight;
    [SerializeField] Color mainColorBackgroundDark, textColorBackgroundDark, textColorBackgroundLight, textColorDark, textColorLight;
    [SerializeField] Image exitImage;
    [SerializeField] Sprite exitBackgroundDarkIcon, exitBackgroundLightIcon, diceHolderDark, diceHolderLight;
    [SerializeField] List<SpriteRenderer> DiceHolder;
    [SerializeField] TextMeshProUGUI[] botText;

    [SerializeField] List<Image> textColorBackgroundImage;

    //Drawer Pannel Attributes
    [SerializeField] Image drawerbackgroundImg, homeIcon, settingIcon, rateUsIcon, helpAndSupportIcon, aboutUsIcon, logoutIcon, image, slidingImg;
    [SerializeField] Color drawerBackgroundLightColor, drawerBackgroundDarkColor, drawerIconLightColor, drawerIconDarkColor, lightTextColor, darkTextColor;
    [SerializeField] TextMeshProUGUI homeText, settingText, rateUsText, helpAndSupportText, aboutUsText, logoutText, loginText;
    [SerializeField] Sprite slidingDarkSprite, slidingLightSprite;

    //Player Selection Screen
    [SerializeField] Image PlayerSelectionBackgroundImage,menuImage, PlayerSelectionPanelImage, newGameLabel, colorSwitchToggleButton;
    [SerializeField] Sprite PlayerSelectionLightBackgroundTheme, darkThemePanelImage, LightThemePanelImage, darkActiveInput, lightActiveInput, darkNewGameBg, LightNewGameBg, darkUncheckedImg, lightUncheckedImg, lightMenuImage, darkMenuImage, colorSwitchToggleButtonLight, colorSwitchToggleButtonDark;
    [SerializeField] List<Image> NameInputField, NumberOfPlayerButton;
    [SerializeField] List<Text> NameText;
    [SerializeField] Color darkNameTextColor, lightNameTextColor, PoweredByGCTextDarkColor, PoweredByGCTextLightColor;
    [SerializeField] Text PoweredByGCText, PoweredByGCText1;

    [Header("Winner Screen")]
    [SerializeField] Sprite lightWinnerBackground, darkWinnerBackground;

    [SerializeField] Sprite lightBgWinner, darkBgWinner;

    [Header("Waiting For Opponent Screen")]
    [SerializeField] Image waitingForOpponentBackgroundImg;
    [SerializeField] Image waitingForOpponentPopImg, playerOneBgImg, playerTwoBgImg, playerThreeBgImg, playerFourBgImg, ExitIconImg, TitleBgImg;
    [SerializeField] TextMeshProUGUI playerOneText, playerTwoText, playerThreeText, playerFourText, titleText;
    [SerializeField] Sprite lightwaitingForOpponentBackgroundImg, darkwaitingForOpponentBackgroundImg, lightExitButton, darkExitButton;

    [Header("Login Screen")]
    [SerializeField] Image EmailEditTextBackground;
    [SerializeField] Image PassEditTextBackground, LoginMainBackground, LoginPopBackground,LoginEmailIcon,LoginPasswordIcon, LogginToggleIcon, LoginButton,EmailInputFieldBackground, PassInputFieldBackground;
    [SerializeField] Sprite editTextDark, editTextLight, LightLoginMainBackground, DarkLoginMainBackground;
    [SerializeField] TextMeshProUGUI LoginText, emailText, emailPlaceholderText, passText, passPlaceholderText, loginButtonText, loginWithFacebookText, loginWithGCText, playAsGuestText, OrText;

    [Header("Sign-up Screen")]
    [SerializeField] Image SignUpEmailEditTextBackground;
    [SerializeField] Image SignUpUserNameEditTextBackground, SignUpPassEditTextBackground, SignUpMainBackground, SignUpPopBackground, SignUpUserIcon, SignUpEmailIcon, SignUpPasswordIcon, SignUpToggleIcon, SignUpButton, SignUpUserInputFieldBackground,SignUpInputFieldBackground, SignUpPassInputFieldBackground;
    [SerializeField] TextMeshProUGUI SignUpText, SignUpUserText, SignUpUserPlaceholderText, SignUpemailText, SignUpemailPlaceholderText, SignUpPassText, SignUpPassPlaceholderText, SignUpButtonText;

    [Header("Forget-Pass Screen")]
    [SerializeField] Image ForgetPassBackgroundImage;
    [SerializeField] Image ForgotPasswordPopupImage, ForgotPasswordEditTextBackground, ForgotPassEmailIcon, ForgetPasswordBackIcon;
    [SerializeField] Sprite ForgetPassBackgroundImageDark, ForgetPassBackgroundImageLight, FPDBackIcon, FPLBackIcon;
    [SerializeField] TextMeshProUGUI ForgotPassEmailText, ForgotPassPlaceholderText, ForgotPassHeaderText, ForgotPassDescriptionText;

    [Header("Waiting Exit Screen")]
    [SerializeField] Image WaitingExitBackgroundImage;
    [SerializeField] Image WaitingExitPopupImage;

    [SerializeField] Sprite waitingExitBackgroundImageSprite, waitingExitBackgroundLightSprite, waitingExitPopupLightSprite, waitingExitPopupDarkSprite;

    [SerializeField] TextMeshProUGUI WaitingExitTitleText;
    [SerializeField] Color waitingExitLightColor, waitingExitDarkColor;

    [Header("Logout Screen")]
    [SerializeField] Image logoutPopupImage;
    [SerializeField] Image logoutclose, backgroundLogoutPanel;

    [SerializeField] TextMeshProUGUI logoutTitleText, logoutHeaderText;
    [SerializeField] Color logoutLightColor, logoutDarkColor;

    void Start()
    {
        
    }

    private void Awake()
    {
        playerPopUp = FindObjectOfType<PlayerPopUp>();
    }

    public void changeTheme()
    {
    	if(GameManager.gm.isDarkMode)
    	{
    		 SettingTitleText.color = SettingTitleTextColorDark;
    		 musicOnText.color = musicOnandOffTextColorDark;
    		 musicOffText.color = musicOnandOffTextColorDark;
    		 chooseThemeText.color = chooseThemeTextColorDark;
    		 lightText.color = lightAndDarkTextColorDark;
    		 darkText.color = lightAndDarkTextColorDark;
    		 indicatorText.color = indicatorTextColorDark;

    		 backgroundPopupImage.DOColor(popupDark, tweenTime);
    		 closeIconImage.DOColor(closeDark, tweenTime);
    		 //musicToggleBackgroundImage.DOColor(musicToggleColorDark, tweenTime);
    		 themeToggleBackgroundImage.DOColor(themeToggleColorDark, tweenTime);

    		 backgroundImage.sprite = darkBackground;
    		// soundImage.sprite = darkSound;
    		 backgroundMusicToggleImage.sprite = darkSoundToggle;

            //Waiting For Opponent Screen
            waitingForOpponentBackgroundImg.sprite = darkwaitingForOpponentBackgroundImg;
            ExitIconImg.sprite = darkExitButton;
            waitingForOpponentPopImg.color = new Color32(25, 25, 69, 255);
            TitleBgImg.color = new Color32(25, 25, 69, 255);
            titleText.color = new Color32(186, 190, 224, 255);
            playerOneBgImg.color = new Color32(36, 42, 85, 255);
            playerTwoBgImg.color = new Color32(36, 42, 85, 255);
            playerThreeBgImg.color = new Color32(36, 42, 85, 255);
            playerFourBgImg.color = new Color32(36, 42, 85, 255);
            playerOneText.color = new Color32(255, 255, 255, 255);
            playerTwoText.color = new Color32(255, 255, 255, 255);
            playerThreeText.color = new Color32(255, 255, 255, 255);
            playerFourText.color = new Color32(255, 255, 255, 255);

            //Exit Screen
            exitBackgroundImage.sprite = exitBackgroundImageSprite;
             exitPopupImage.sprite = exitPopupDarkSprite;

             ExitTitleText.color = exitDarkColor;

            // Drawer Screen
            drawerbackgroundImg.color = drawerBackgroundDarkColor;
            image.color = new Color(255, 255, 255, 0);

            homeIcon.color = drawerIconDarkColor;
            settingIcon.color = drawerIconDarkColor;
            rateUsIcon.color = drawerIconDarkColor;
            helpAndSupportIcon.color = drawerIconDarkColor;
            aboutUsIcon.color = drawerIconDarkColor;
            logoutIcon.color = drawerIconDarkColor;

            homeText.color = darkTextColor;
            settingText.color = darkTextColor;
            rateUsText.color = darkTextColor;
            helpAndSupportText.color = darkTextColor;
            aboutUsText.color = darkTextColor;
            logoutText.color = darkTextColor;
            loginText.color = darkTextColor;

            slidingImg.sprite = slidingDarkSprite;
            slidingImg.color = new Color32(0, 0, 0, 240);

            //Player Selection Background Image
            PlayerSelectionBackgroundImage.sprite = exitBackgroundImageSprite;
            colorSwitchToggleButton.sprite = colorSwitchToggleButtonDark;
            PlayerSelectionBackgroundImage.color = new Color32(255, 255, 255, 255);
            //menuImage.color = new Color32(255, 255, 255, 0);
            menuImage.sprite = darkMenuImage;
            PlayerSelectionPanelImage.sprite = darkThemePanelImage;
            newGameLabel.sprite = darkNewGameBg;

            for (int i = 0; i < NameInputField.Count; i++)
            {
                NameInputField[i].sprite = darkActiveInput;
            }

            for (int i = 0; i < NameText.Count; i++)
            {
                NameText[i].color = darkNameTextColor;
            }

            PoweredByGCText.color = PoweredByGCTextDarkColor;
            PoweredByGCText1.color = PoweredByGCTextDarkColor;

            for (int i=0; i < NumberOfPlayerButton.Count; i++)
            {
                NumberOfPlayerButton[i].sprite = darkUncheckedImg;
            }


            //Main Game Screen
            Camera.main.GetComponent<Camera>().backgroundColor = mainColorBackgroundDark;

             for(int i = 0; i < textColorBackgroundImage.Count; i++)
             {
                textColorBackgroundImage[i].DOColor(textColorBackgroundDark, tweenTime);
             }

            for(int i=0; i< DiceHolder.Count; i++)
            {
                Color32 color = new Color32(45, 51, 98,255);
                DiceHolder[i].color = color;
            }

            exitImage.sprite = exitBackgroundDarkIcon;

            for(int i=0; i < botText.Length; i++)
            {
                botText[i].color = textColorDark;
            }

            //Winner Screen Background
            backgroundWinnerScreenPopImage.sprite = darkWinnerBackground;
            bgWinnerScreen.sprite = darkBgWinner;

            //Login Screen
            EmailEditTextBackground.sprite = editTextDark;
            PassEditTextBackground.sprite = editTextDark;
            LoginMainBackground.sprite = DarkLoginMainBackground;
            LoginPopBackground.color = new Color32(30,35,71,255);
            LoginEmailIcon.color = new Color32(255, 255, 255, 255);
            LoginPasswordIcon.color = new Color32(255, 255, 255, 255);
            LogginToggleIcon.color = new Color32(255, 255, 255, 255);
           // LoginButton.color = new Color32(0, 255, 0, 255);
            EmailInputFieldBackground.color = new Color32(30, 35, 71, 255);
            PassInputFieldBackground.color = new Color32(30, 35, 71, 255);

            LoginText.color = new Color32(255,255,255,255);
            emailText.color= new Color32(255,255,255,255);
            emailPlaceholderText.color= new Color32(255,255,255,255);
            passText.color= new Color32(255,255,255,255);
            passPlaceholderText.color= new Color32(255,255,255,255);
          //  loginButtonText.color= new Color32(255,255,255,255);
            loginWithFacebookText.color= new Color32(255,255,255,255);
            loginWithGCText.color= new Color32(255,255,255,255);
            playAsGuestText.color= new Color32(18,27,77,255);
            OrText.color = new Color32(255, 255, 255, 255);

            //Sign-up Screen
            SignUpUserNameEditTextBackground.sprite = editTextDark;
            SignUpEmailEditTextBackground.sprite = editTextDark;
            SignUpPassEditTextBackground.sprite = editTextDark;
            SignUpMainBackground.sprite = DarkLoginMainBackground;
            SignUpPopBackground.color = new Color32(30, 35, 71, 255);
            SignUpUserIcon.color = new Color32(255, 255, 255, 255);
            SignUpEmailIcon.color = new Color32(255, 255, 255, 255);
            SignUpPasswordIcon.color = new Color32(255, 255, 255, 255);
            SignUpToggleIcon.color = new Color32(255, 255, 255, 255);
           // SignUpButton.color = new Color32(0, 255, 0, 255);
            SignUpUserInputFieldBackground.color = new Color32(30, 35, 71, 255);
            SignUpInputFieldBackground.color = new Color32(30, 35, 71, 255);
            SignUpPassInputFieldBackground.color = new Color32(30, 35, 71, 255);

            SignUpText.color = new Color32(255, 255, 255, 255);
            SignUpUserText.color = new Color32(255, 255, 255, 255);
            SignUpUserPlaceholderText.color = new Color32(255, 255, 255, 255);
            SignUpemailText.color = new Color32(255, 255, 255, 255);
            SignUpemailPlaceholderText.color = new Color32(255, 255, 255, 255);
            SignUpPassText.color = new Color32(255, 255, 255, 255);
            SignUpPassPlaceholderText.color = new Color32(255, 255, 255, 255);
           // SignUpButtonText.color = new Color32(255, 255, 255, 255);

            //Forget-Pass Screen
            ForgetPassBackgroundImage.sprite = ForgetPassBackgroundImageDark;
            ForgotPassEmailText.color = new Color32(255, 255, 255, 255);
            ForgotPassPlaceholderText.color = new Color32(255, 255, 255, 255);
            ForgotPassHeaderText.color = new Color32(255, 255, 255, 255);
            ForgotPassDescriptionText.color = new Color32(255, 255, 255, 255);

            ForgotPasswordPopupImage.color = new Color32(30, 35, 71, 255);
            ForgotPasswordEditTextBackground.color = new Color32(30, 35, 71, 255); ;
            ForgotPassEmailIcon.color = new Color32(255, 255, 255, 255);
            ForgetPasswordBackIcon.sprite = FPDBackIcon;

            //Waiting Exit Screen
            WaitingExitBackgroundImage.sprite = waitingExitBackgroundImageSprite;
            WaitingExitPopupImage.sprite = waitingExitPopupDarkSprite;

            WaitingExitTitleText.color = waitingExitDarkColor;

            //Logout Screen
            logoutPopupImage.color = new Color32(42, 52, 104, 255);
            logoutclose.color = logoutDarkColor;

            logoutTitleText.color = logoutDarkColor;
            logoutHeaderText.color = logoutDarkColor;
            backgroundLogoutPanel.color = new Color32(0, 0, 0, 240);
        }
    	else
    	{
    		 SettingTitleText.color = SettingTitleTextColorLight;
    		 musicOnText.color = musicOnandOffTextColorLight;
    		 musicOffText.color = musicOnandOffTextColorLight;
    		 chooseThemeText.color = chooseThemeTextColorLight;
    		 lightText.color = lightAndDarkTextColorLight;
    		 darkText.color = lightAndDarkTextColorLight;
    		 indicatorText.color = indicatorTextColorLight;

    		 backgroundPopupImage.DOColor(popupLight, tweenTime);
    		 closeIconImage.DOColor(closeLight, tweenTime);
    		// musicToggleBackgroundImage.DOColor(musicToggleColorLight, tweenTime);
    		 themeToggleBackgroundImage.DOColor(themeToggleColorLight, tweenTime);

    		 backgroundImage.sprite = lightBackground;
    		// soundImage.sprite = lightSound;
             backgroundMusicToggleImage.sprite = lightSoundToggle;

            //Waiting For Opponent Screen
            waitingForOpponentBackgroundImg.sprite = lightwaitingForOpponentBackgroundImg;
            ExitIconImg.sprite = lightExitButton;
            waitingForOpponentPopImg.color = new Color32(244, 244, 244, 255);
            TitleBgImg.color = new Color32(255, 255, 255, 50);
            titleText.color = new Color32(186, 190, 224, 255);
            playerOneBgImg.color = new Color32(220, 220, 220, 255);
            playerTwoBgImg.color = new Color32(220, 220, 220, 255);
            playerThreeBgImg.color = new Color32(220, 220, 220, 255);
            playerFourBgImg.color = new Color32(220, 220, 220, 255);
            playerOneText.color = new Color32(41, 41, 41, 255);
            playerTwoText.color = new Color32(41, 41, 41, 255);
            playerThreeText.color = new Color32(41, 41, 41, 255);
            playerFourText.color = new Color32(41, 41, 41, 255);

            //Exit Screen
            exitBackgroundImage.sprite = exitBackgroundLightSprite;
             exitPopupImage.sprite = exitPopupLightSprite;

             ExitTitleText.color = exitLightColor;

            // Drawer Screen
            drawerbackgroundImg.color = drawerBackgroundLightColor;
            image.color = new Color(255, 255, 255, 1);

            homeIcon.color = drawerIconLightColor;
            settingIcon.color = drawerIconLightColor;
            rateUsIcon.color = drawerIconLightColor;
            helpAndSupportIcon.color = drawerIconLightColor;
            aboutUsIcon.color = drawerIconLightColor;
            logoutIcon.color = drawerIconLightColor;

            homeText.color = lightTextColor;
            settingText.color = lightTextColor;
            rateUsText.color = lightTextColor;
            helpAndSupportText.color = lightTextColor;
            aboutUsText.color = lightTextColor;
            logoutText.color = lightTextColor;
            loginText.color = lightTextColor;

            slidingImg.sprite = slidingLightSprite;
            slidingImg.color = new Color32(255, 255, 255, 240);
            //Player Selection Background Image
            PlayerSelectionBackgroundImage.sprite = PlayerSelectionLightBackgroundTheme;
            colorSwitchToggleButton.sprite = colorSwitchToggleButtonLight;
            PlayerSelectionBackgroundImage.color = new Color32(244, 244, 244, 255);
            // menuImage.color = new Color32(255, 255, 255, 0);
            menuImage.sprite = lightMenuImage;
            PlayerSelectionPanelImage.sprite = LightThemePanelImage;
            newGameLabel.sprite = LightNewGameBg;

            for (int i=0; i< NameInputField.Count; i++)
            {
                NameInputField[i].sprite = lightActiveInput; 
            }

            for (int i = 0; i < NameText.Count; i++)
            {
                NameText[i].color = lightNameTextColor;
            }

            PoweredByGCText.color = PoweredByGCTextLightColor;
            PoweredByGCText1.color = PoweredByGCTextLightColor;

            for (int i = 0; i < NumberOfPlayerButton.Count; i++)
            {
                NumberOfPlayerButton[i].sprite = lightUncheckedImg;
            }

            //Main Screen 
            Camera.main.GetComponent<Camera>().backgroundColor = mainColorBackgroundLight;

             for(int i = 0; i < textColorBackgroundImage.Count; i++)
             {
                textColorBackgroundImage[i].DOColor(textColorBackgroundLight, tweenTime);
             }

            for (int i = 0; i < DiceHolder.Count; i++)
            {
                DiceHolder[i].color = new Color(255f, 255f, 255f, 1f);
            }

            exitImage.sprite = exitBackgroundLightIcon;

            for (int i = 0; i < botText.Length; i++)
            {
                botText[i].color = textColorLight;
            }

            //Winner Screen Background
            backgroundWinnerScreenPopImage.sprite = lightWinnerBackground;
            bgWinnerScreen.sprite = lightBgWinner;

            //Login Screen
            EmailEditTextBackground.sprite = editTextLight;
            PassEditTextBackground.sprite = editTextLight;
            LoginMainBackground.sprite = LightLoginMainBackground;
            LoginPopBackground.color = new Color32(255, 255, 255, 255);
            SignUpUserIcon.color = new Color32(75, 75, 75, 255);
            LoginEmailIcon.color = new Color32(75, 75, 75, 255);
            LoginPasswordIcon.color = new Color32(75, 75, 75, 255);
            LogginToggleIcon.color = new Color32(75, 75, 75, 255);
            LoginButton.color = new Color32(255, 245, 3, 255);

            LoginText.color = new Color32(0, 0, 0, 255);
            SignUpUserText.color = new Color32(0, 0, 0, 255);
            SignUpUserPlaceholderText.color = new Color32(129, 129, 129, 255);
            emailText.color = new Color32(0, 0, 0, 255);
            emailPlaceholderText.color = new Color32(129, 129, 129, 255);
            passText.color = new Color32(0, 0, 0, 255);
            passPlaceholderText.color = new Color32(129, 129, 129, 255);
            loginButtonText.color = new Color32(18, 27, 77, 255);
            loginWithFacebookText.color = new Color32(0, 122, 194, 255);
            loginWithGCText.color = new Color32(0, 122, 194, 255);
            playAsGuestText.color = new Color32(0, 0, 0, 255);
            OrText.color = new Color32(18, 27, 77, 255);
            EmailInputFieldBackground.color = new Color32(255, 255, 255, 255);
            PassInputFieldBackground.color = new Color32(255, 255, 255, 255);

            //Sign-up Screen
            SignUpUserNameEditTextBackground.sprite = editTextLight;
            SignUpEmailEditTextBackground.sprite = editTextLight;
            SignUpPassEditTextBackground.sprite = editTextLight;
            SignUpMainBackground.sprite = LightLoginMainBackground;
            SignUpPopBackground.color = new Color32(255, 255, 255, 255);
            SignUpUserIcon.color = new Color32(75, 75, 75, 255);
            SignUpEmailIcon.color = new Color32(75, 75, 75, 255);
            SignUpPasswordIcon.color = new Color32(75, 75, 75, 255);
            SignUpToggleIcon.color = new Color32(75, 75, 75, 255);
            SignUpButton.color = new Color32(255, 245, 3, 255);
            SignUpUserInputFieldBackground.color = new Color32(255, 255, 255, 255);
            SignUpInputFieldBackground.color = new Color32(255, 255, 255, 255);
            SignUpPassInputFieldBackground.color = new Color32(255, 255, 255, 255);

            SignUpText.color = new Color32(0, 0, 0, 255);
            SignUpUserText.color = new Color32(0, 0, 0, 255);
            SignUpUserPlaceholderText.color = new Color32(129, 129, 129, 255);
            SignUpemailText.color = new Color32(0, 0, 0, 255);
            SignUpemailPlaceholderText.color = new Color32(129, 129, 129, 255);
            SignUpPassText.color = new Color32(0, 0, 0, 255);
            SignUpPassPlaceholderText.color = new Color32(129, 129, 129, 255);
            SignUpButtonText.color = new Color32(18, 27, 77, 255);

            //Forget-Pass Screen
            ForgetPassBackgroundImage.sprite = ForgetPassBackgroundImageLight;
            ForgotPassEmailText.color = new Color32(0, 0, 0, 255);
            ForgotPassPlaceholderText.color = new Color32(0, 0, 0, 255);
            ForgotPassHeaderText.color = new Color32(0, 0, 0, 255);
            ForgotPassDescriptionText.color = new Color32(0, 0, 0, 255);

            ForgotPasswordPopupImage.color = new Color32(255, 255, 255, 255);
            ForgotPasswordEditTextBackground.color = new Color32(255, 255, 255, 255);
            ForgotPassEmailIcon.color = new Color32(30, 35, 71, 255);

            ForgetPasswordBackIcon.sprite = FPLBackIcon;

            //Waiting Exit Screen
            WaitingExitBackgroundImage.sprite = waitingExitBackgroundLightSprite;
            WaitingExitPopupImage.sprite = waitingExitPopupLightSprite;

            WaitingExitTitleText.color = waitingExitLightColor;

            //Logout Screen
            logoutPopupImage.color = new Color32(255, 255, 255, 255);
            logoutclose.color = logoutLightColor;

            logoutTitleText.color = logoutLightColor;
            logoutHeaderText.color = logoutLightColor;
            backgroundLogoutPanel.color = new Color32(240, 240, 240, 240);
        }

    	changeIcon();
    }

    private void changeIcon()
    {
        if(GameManager.gm.isDarkMode)
        {
            if(GameManager.gm.isMusicOn)
                soundImage.sprite = darkOnSprite;
            else
                soundImage.sprite = darkOffSprite;
        }
        else
        {
            if(GameManager.gm.isMusicOn)
                soundImage.sprite = lightOnSprite;
            else
                soundImage.sprite = lightOffSprite;
        }

        if(playerPopUp)
        playerPopUp.switchTheme();
    }





}
