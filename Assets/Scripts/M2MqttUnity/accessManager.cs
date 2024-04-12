using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using M2MqttUnity;

public class accessManager : MonoBehaviour
{
	public Text email;

    M2MqttUnity.M2MqttUnityClient m2MqttUnityClient;
    WaitingForOpponent waitingForOpponent;
    [SerializeField] GameObject LoginPanel;


     void Awake()
        {
            m2MqttUnityClient = FindObjectOfType<M2MqttUnity.M2MqttUnityClient>();
            waitingForOpponent = FindObjectOfType<WaitingForOpponent>();
        }

   void Start () {
        /* string arguments = "";
         string id = "";
         string channel = "";
 
         AndroidJavaClass UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
         AndroidJavaObject currentActivity = UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
 
         AndroidJavaObject intent = currentActivity.Call<AndroidJavaObject>("getIntent");
         bool hasExtra = intent.Call<bool>("hasExtra", "GCPlayer");
         bool hasExtra1 = intent.Call<bool>("hasExtra", "tokenID");
         bool hasExtra2 = intent.Call<bool>("hasExtra", "MQChannel");

         //email.text = hasExtra +" : "+ hasExtra1 + " : "+ hasExtra2;

         print(hasExtra);
         if (hasExtra)
         {
             AndroidJavaObject extras = intent.Call<AndroidJavaObject>("getExtras");
             arguments = extras.Call<string>("getString", "GCPlayer");
             id = extras.Call<string>("getString", "tokenID");

             Credentials.channel = "arenas/global/matches/"+id;
             Credentials.id = id;
             Credentials.email = arguments;


             email.text = Credentials.channel;

             m2MqttUnityClient.Connect();

             objectSetActivie(LoginPanel,false);
         }
         else
         {
            waitingForOpponent.close();
         }*/
     }


      void objectSetActivie(GameObject gameObject, bool status)
    {
        if(gameObject)
        {
            gameObject.SetActive(status);
        }
    }


}
