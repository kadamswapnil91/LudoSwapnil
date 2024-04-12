using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;
using System;
using TMPro;


public class InternetConnectionAvailability : MonoBehaviour
{
    WaitingForOpponent waitingForOpponent;
    [SerializeField] TextMeshProUGUI internetConnectionMessage;
    M2MqttUnity.M2MqttUnityClient m2MqttUnityClient;
    APICall aPICall;

    public static float timeRemaining = 0f;
    public static bool timerIsRunning ;
    public static bool call;
    public static bool isFirstTimeCall;

    void Awake()
    {
        waitingForOpponent = FindObjectOfType<WaitingForOpponent>();
        m2MqttUnityClient = FindObjectOfType<M2MqttUnity.M2MqttUnityClient>();
        aPICall = FindObjectOfType<APICall>();

    }

    private void Start()
    {
        isFirstTimeCall = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.gm.isOnlineGame || waitingForOpponent.getPanelObject().activeInHierarchy)
        {
            /*StartCoroutine(checkInternetConnection((isConnected) => {
                if (isConnected)
                {
                    // internetConnectionMessage.text = "Net is Connected.";
                    //  internetConnectionMessage.text = "Id : "+ Credentials.id.ToString()+ " Email : "+ Credentials.email;
                    m2MqttUnityClient.Connect();
                }    
                else
                {
                  //  internetConnectionMessage.text = "Net is disconnected. please check your internet.";
                }
            }));*/

            //if (timerIsRunning && !GameManager.gm.mqttIsConnected)
            //{
            //    if (timeRemaining > 0)
            //    {
            //        timeRemaining -= Time.deltaTime;
            //    }
            //    else
            //    {
            //        timeRemaining = 0;
            //        if(!call)
            //        StartCoroutine(CheckConnection());

            //    }
            //}

            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
            }
            else
            {
                timeRemaining = 0;
                if (!call && !GameManager.gm.isBackground)
                    aPICall.checkInternetConnection();

                if(!GameManager.gm.mqttIsConnected)
                {
                    if (!call)
                    {
                        StartCoroutine(CheckConnection());
                    }
                    
                }

            }


        }
        
    }

    IEnumerator checkInternetConnection(Action<bool> action)
    {
        UnityWebRequest www = new UnityWebRequest("http://google.com");
        yield return www;
        if (www.error != null)
        {
            action(false);
        }
        else
        {
            action(true);
        }
    }

    IEnumerator CheckConnection()
    {
        if(isFirstTimeCall)
        {
            yield return new WaitForSeconds(20f);
            isFirstTimeCall = false;

        }
        else
        {
            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                timeRemaining = 5f;
            }
            //Check if the device can reach the internet via a carrier data network
            else if (Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork || Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork)
            {
                call = true;
                yield return new WaitForSeconds(1f);
                m2MqttUnityClient.Connect();
                //       aPICall.callMatchStatus();
            }

        }
        
        yield return new WaitForEndOfFrame();
    }

    
}
