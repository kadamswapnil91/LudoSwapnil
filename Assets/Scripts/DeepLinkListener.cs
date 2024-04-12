using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Runtime.InteropServices;
using UnityEngine.Events;
using System.Linq;
using TMPro;
using System;
using System.Web;
using UnityEngine.SceneManagement;




public class DeepLinkListener : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void DeepLinkReceiverIsAlive();
    [System.Serializable]
    public class StringEvent : UnityEvent<string> { }
    public StringEvent urlOpenedEvent;
  //  public bool dontDestroyOnLoad = true;
    //[SerializeField] TextMeshProUGUI message;
    WaitingForOpponent waitingForOpponent;
    M2MqttUnity.M2MqttUnityClient m2MqttUnityClient;
    M2MqttUnity.Examples.M2MqttUnityTest m2MqttUnityTest;
    //[SerializeField] GameObject LoadingPanel;
    //bool status = false;

    void Start()
    {
#if UNITY_IOS
        //if (dontDestroyOnLoad)
        //{
        //    DontDestroyOnLoad(this.gameObject);
        //}
           
        DeepLinkReceiverIsAlive(); // Let the App Controller know it's ok to call URLOpened now.
#endif

    }

    private void Awake()
    {
        waitingForOpponent = FindObjectOfType<WaitingForOpponent>();
        m2MqttUnityClient = FindObjectOfType<M2MqttUnity.M2MqttUnityClient>();
        m2MqttUnityTest = FindObjectOfType<M2MqttUnity.Examples.M2MqttUnityTest>();
    }

    public void URLOpened(string url)
    {
        //  urlOpenedEvent.Invoke(url);
#if UNITY_IOS
        
      //  GameManager.gm.isFromIosGcApp = true;

        var uri = new Uri(url);
        var query = HttpUtility.ParseQueryString(uri.Query);

        if (Credentials.id != null && Credentials.id != "")
            Credentials.old_id = Credentials.id;
        Credentials.id = query.Get("match_id");
        Credentials.contestId = query.Get("contest_id");
        Credentials.userId = query.Get("user_id");
        Credentials.channel = "arenas/global/matches/" + Credentials.id;
        Credentials.iosSklashToken = query.Get("ios_sklash_token");


        if (!Credentials.isSplashScreen)
        {
                if(!waitingForOpponent.getPanelObject().activeInHierarchy)
                waitingForOpponent.getPanelObject().SetActive(true);

                waitingForOpponent.connectMQ();
        }

        //status = !status;

        //if (!LoadingPanel.activeInHierarchy && status)
        //    LoadingPanel.SetActive(true);

        //if(!waitingForOpponent.getPanelObject().activeInHierarchy)
        //waitingForOpponent.getPanelObject().SetActive(true);

        //if(status)
        //waitingForOpponent.connectMQ();
#endif

    }

}
