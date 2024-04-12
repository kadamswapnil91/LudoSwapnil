using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NavigationDrawer.UI;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.SceneManagement;


public class MenuHandler : MonoBehaviour
{
   [Header("Menu Item")]
   [SerializeField] public Button Home;

   [SerializeField] public Button Settings, RateUs, HelpAndSupport, AboutUs, LogOut, Login, cancel, close, logoutYes;

   [Header("Setting Item")]
   [SerializeField] public GameObject SettingPanel;

   [SerializeField] public GameObject LoginPanel, SignUpPanel, LogOutPanel;

   public List<Button> buttonList;

   NavDrawerPanel navDrawerPanel;
   WaitingForOpponent waitingForOpponent;
   ExitManager exitManager;
   Login login;
   APICall aPICall;

    void Start()
   {
         objectSetActivie(SettingPanel,false);

   		Home.onClick.AddListener(() => selectedMenu(1));
   		Settings.onClick.AddListener(() => selectedMenu(2));
   		RateUs.onClick.AddListener(() => selectedMenu(3));
   		HelpAndSupport.onClick.AddListener(() => selectedMenu(4));
   		AboutUs.onClick.AddListener(() => selectedMenu(5));
   		LogOut.onClick.AddListener(() => selectedMenu(6));
        Login.onClick.AddListener(() => selectedMenu(7));
        cancel.onClick.AddListener(closeLogoutPanel);
        close.onClick.AddListener(closeLogoutPanel);
        logoutYes.onClick.AddListener(logout);
    }

   private void Awake()
 	 {
 	 	navDrawerPanel= FindObjectOfType<NavDrawerPanel>();
        waitingForOpponent = FindObjectOfType<WaitingForOpponent>();
        exitManager = FindObjectOfType<ExitManager>();
        login = FindObjectOfType<Login>();
        aPICall = FindObjectOfType<APICall>();
    }


   void selectedMenu(int id)
   {
   		switch(id)
   		{
   			case 1: showSelectedButton(0);
                       break;

   			case 2: showSelectedButton(1);
                    objectSetActivie(SettingPanel,true);
   					break;

   			case 3: 
                  showSelectedButton(2);
   					break;

   			case 4: showSelectedButton(3);
                aPICall.checkConnection("help_and_support");
                break;

   			case 5: showSelectedButton(4);
                aPICall.checkConnection("about_us");
                break;

   			case 6:

                if(GameManager.gm.isLogin)
                {
                    showSelectedButton(5);
                    objectSetActivie(LogOutPanel, true);
                    //                    string path = Application.persistentDataPath + "/loginUserPath.fun";
                    //                    if (File.Exists(path))
                    //                        File.Delete(path);
                    //                    GameManager.gm.isLogin = false;
                    //                    PlayerPrefs.SetInt("playAsGuest", 0);



                    //#if UNITY_ANDROID
                    //                    SceneManager.LoadScene(1);
                    //#endif

                    //#if UNITY_IOS
                    //                GameManager.gm.isGameRunning = false;
                    //                SameMarker.ClearInstance();
                    //                MqttMessageArray.ClearInstance();
                    //                GameManager.clearGameManager();
                    //                SceneManager.LoadScene(1);
                    //#endif
                }
                else
                {
                    PlayerPrefs.SetInt("playAsGuest", 0);
                    SceneManager.LoadScene(1);
                }



                break;

            case 7:
                PlayerPrefs.SetInt("playAsGuest", 0);
                SceneManager.LoadScene(1);
                break;
        }
   }

   void showSelectedButton(int id)
   {

   		for(int i=0; i < buttonList.Count ; i++)
   		{
   			if(i == id)
   			{
                if(i == 0 || i == 2)
                {
                    //don't select menu
                }
                else
   				buttonList[i].image.color = new Color(buttonList[i].image.color.r, buttonList[i].image.color.g, buttonList[i].image.color.b, 0.5f);
   			}
   			else
   			{
   				buttonList[i].image.color = new Color(buttonList[i].image.color.r, buttonList[i].image.color.g, buttonList[i].image.color.b, 0f);
   			}
   		}

   		navDrawerPanel.Close();

   }

   void objectSetActivie(GameObject gameObject, bool status)
   {
      if(gameObject)
      {
         gameObject.SetActive(status);
      }
   }


    public void resetSelectedButton()
    {

        for (int i = 0; i < buttonList.Count; i++)
        {
            buttonList[i].image.color = new Color(buttonList[i].image.color.r, buttonList[i].image.color.g, buttonList[i].image.color.b, 0f);
        }

    }

    void logout()
    {
        string path = Application.persistentDataPath + "/loginUserPath.fun";
        if (File.Exists(path))
            File.Delete(path);
        GameManager.gm.isLogin = false;
        PlayerPrefs.SetInt("playAsGuest", 0);



#if UNITY_ANDROID
        GameManager.gm.isGameRunning = false;
        SameMarker.ClearInstance();
        MqttMessageArray.ClearInstance();
        GameManager.clearGameManager();
        clearIntent();
        SceneManager.LoadScene(1);
#endif

#if UNITY_IOS
                GameManager.gm.isGameRunning = false;
                SameMarker.ClearInstance();
                MqttMessageArray.ClearInstance();
                GameManager.clearGameManager();
                SceneManager.LoadScene(1);
#endif
    }

    void closeLogoutPanel()
    {
        objectSetActivie(LogOutPanel, false);
    }

    void clearIntent()
    {
        AndroidJavaClass UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject currentActivity = UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

        AndroidJavaObject intent = currentActivity.Call<AndroidJavaObject>("getIntent");

        intent.Call<AndroidJavaObject>("putExtra", "GCPlayer", null);
        intent.Call<AndroidJavaObject>("putExtra", "tokenID", null);
        intent.Call<AndroidJavaObject>("putExtra", "MQChannel", null);
        intent.Call<AndroidJavaObject>("putExtra", "contest_id_gc", null);
        intent.Call<AndroidJavaObject>("putExtra", "user_id_gc", null);
        intent.Call<AndroidJavaObject>("putExtra", "is_admin_contest", false);

        UnityPlayer = null;
        currentActivity = null;
        intent = null;
    }


}
