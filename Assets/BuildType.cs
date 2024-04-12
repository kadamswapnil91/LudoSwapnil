using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuildType : MonoBehaviour
{
   public string packageName; 
   //public string isSklashLudo;
   public Login loginReference;

  // public SignUp signUpReference;
  // public WaitingForOpponent waitingForOppopnentReference;
  // public PlayerPopUp playerPopupReference;
   [SerializeField] Button loginWithSklash;
   [SerializeField] Image SklashLogo, backSklashLogo;
   [SerializeField] Image logoHeaderImg; 
   [SerializeField] Sprite SklashLudo1;
  // [SerializeField] Image logoHeaderImg;
     void Start()
    {
        packageName = Application.identifier; 
      //  isSklashLudo = packageName;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Awake()
    {
    //  loginReference = FindObjectOfType<Login>();
   //   signUpReference = FindObjectOfType<SignUp>();
   //   playerPopupReference = FindObjectOfType<PlayerPopUp>(); 
   //   waitingForOppopnentReference = FindObjectOfType<WaitingForOpponent>(); 

    }

  public void DisplayNormalLUDOUI()
    {
      /*
      // Old Code
        // Code for normal UI
        if (packageName == "com.sklash.ludo1")
         {  
            Debug.Log("BUILDTYPE: ****************** This is Normal Build ********************");
            loginReference.loginWithSklash.gameObject.SetActive(false);
            loginReference.SklashLogo.gameObject.SetActive(false);
		    	  loginReference.backSklashLogo.gameObject.SetActive(false);
         //   loginReference.logoHeaderImg.sprite = SklashLudo1;
            //signUpReference.logoHeaderImg.sprite = SklashLudo1;
           // playerPopupReference.logoHeaderImg.sprite = SklashLudo1;
          //  waitingForOppopnentReference.logoHeaderImg.sprite = SklashLudo1;
            
		    	 

         }  */

         if (packageName == "com.sklash.ludo1")
         {
           Debug.Log("BUILDTYPE: ****************** This is Normal Build ********************");
          loginWithSklash.gameObject.SetActive(false);
		    	SklashLogo.gameObject.SetActive(false);
		    	backSklashLogo.gameObject.SetActive(false);
			    logoHeaderImg.sprite = SklashLudo1;
			
         }


    
    }
  } 


