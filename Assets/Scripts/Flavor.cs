using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Flavor : MonoBehaviour
{
   [SerializeField] Button loginWithSklash;
   [SerializeField] Image SklashLogo, backSklashLogo;
   [SerializeField] Image logoHeaderImg, signupLogoHeaderImg, playerPopUpLogoHeaderImg, waitingForOppLogoHeaderImg; 
   [SerializeField] Sprite SklashLudo1, SklashLudo1signup, sklashLudoPlayerPopup, sklashLudowaitingForOpp;
   private SplashScreen mySplashScreen;
   [SerializeField] Image SplashBackgroundImage;
   [SerializeField] Sprite darkLudoSplash, lightLudoSplash; 
   public string sklashLudo = "com.sklash.ludo1";
   public string packageName;
    // Start is called before the first frame update
    void Start()
    {
        packageName = Application.identifier; 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Awake()
    {
		mySplashScreen = FindObjectOfType<SplashScreen>();
	}


    void Splashsetup()
    {
       if (PlayerPrefs.GetInt("themeStatus") == 0)
        {
            SplashBackgroundImage.sprite = darkLudoSplash;
        }
        else
        {
            SplashBackgroundImage.sprite = lightLudoSplash;
        }
    }
    
 public void flavorCheck()
 {
     packageName = Application.identifier; 
        if (packageName == sklashLudo)
         {
            Debug.Log("BUILDTYPE: ****************** This is Normal Ludo Build ********************");
            loginWithSklash.gameObject.SetActive(false);
		    SklashLogo.gameObject.SetActive(false);
		    backSklashLogo.gameObject.SetActive(false);
			logoHeaderImg.sprite = SklashLudo1;
            signupLogoHeaderImg.sprite = SklashLudo1signup;
            playerPopUpLogoHeaderImg.sprite = sklashLudoPlayerPopup;
            waitingForOppLogoHeaderImg.sprite = sklashLudowaitingForOpp;
            Splashsetup();
			
         }

 }

}
