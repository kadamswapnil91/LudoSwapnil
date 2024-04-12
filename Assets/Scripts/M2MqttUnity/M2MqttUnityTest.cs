/*
The MIT License (MIT)

Copyright (c) 2018 Giovanni Paolo Vigano'

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using M2MqttUnity;
using SimpleJSON;



/// <summary>
/// Examples for the M2MQTT library (https://github.com/eclipse/paho.mqtt.m2mqtt),
/// </summary>
namespace M2MqttUnity.Examples
{
    /// <summary>
    /// Script for testing M2MQTT with a Unity UI
    /// </summary>
    public class M2MqttUnityTest : M2MqttUnityClient
    {
        [Tooltip("Set this to true to perform a testing cycle automatically on startup")]
        public bool autoTest = false;
        [Header("User Interface")]
        public InputField consoleInputField;
        public Toggle encryptedToggle;
        public InputField addressInputField;
        public InputField portInputField;
        public Button connectButton;
        public Button disconnectButton;
        public Button testPublishButton;
        public Button clearButton;
        public InputField testPublishMessageInputField;
        Queue<string> strQ;
       // [SerializeField] TextMeshProUGUI mqttMessage;

        public List<JoinPlayerInfo> myObject = new List<JoinPlayerInfo>();

        private List<string> eventMessages = new List<string>();
        private bool updateUI = false;

        APICall apiCall;
        WaitingForOpponent waitingForOpponent;
        GreenRollingDice greenRollingDice;
        PathObjectsParent pathParent;
        ActionHandler actionHandler;
        ErrorDialogScript errorDialogScript;

        [SerializeField] TextMeshProUGUI status;

        void Awake()
        {
            apiCall = FindObjectOfType<APICall>();
            waitingForOpponent = FindObjectOfType<WaitingForOpponent>();
            greenRollingDice = FindObjectOfType<GreenRollingDice>();
            pathParent = FindObjectOfType<PathObjectsParent>();
            actionHandler = FindObjectOfType<ActionHandler>();
            errorDialogScript = FindObjectOfType<ErrorDialogScript>();
        }


        public void TestPublish()
        {
            if(!GameManager.gm.mqttIsConnected)
            {
                MqttMessageArray.Instance.addOfflinePublishSendMsg(Credentials.action);
            }
            else
            {
                client.Publish(Credentials.channel, System.Text.Encoding.UTF8.GetBytes(Credentials.action), MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE, false);
            }
            
            Debug.Log("Test message published");
            AddUiMessage("Test message published.");
        }

        public void SetBrokerAddress(string brokerAddress)
        {
            if (addressInputField && !updateUI)
            {
                this.brokerAddress = brokerAddress;
            }
        }

        public void SetBrokerPort(string brokerPort)
        {
            if (portInputField && !updateUI)
            {
                int.TryParse(brokerPort, out this.brokerPort);
            }
        }

        public void SetEncrypted(bool isEncrypted)
        {
            this.isEncrypted = isEncrypted;
        }


        public void SetUiMessage(string msg)
        {
            if (consoleInputField != null)
            {
                consoleInputField.text = msg;
                updateUI = true;
            }
        }

        public void AddUiMessage(string msg)
        {
            if (consoleInputField != null)
            {
                consoleInputField.text += msg + "\n";
                updateUI = true;
            }
        }

        protected override void OnConnecting()
        {
            base.OnConnecting();
            SetUiMessage("Connecting to broker on " + brokerAddress + ":" + brokerPort.ToString() + "...\n");
        }

        protected override void OnConnected()
        {
            GameManager.gm.mqttIsConnected = true;
            InternetConnectionAvailability.timerIsRunning = false;
            base.OnConnected();
            SetUiMessage("Connected to broker on " + brokerAddress + "\n");

            if (autoTest)
            {
                TestPublish();
            }

            apiCall.fire();
        }

        protected override void SubscribeTopics()
        {
                client.Subscribe(new string[] { Credentials.channel }, new byte[] { MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE });
        }

        protected override void UnsubscribeTopics()
        {
            client.Unsubscribe(new string[] { Credentials.channel });
        }

        protected override void OnConnectionFailed(string errorMessage)
        {
            GameManager.gm.mqttIsConnected = false;
            InternetConnectionAvailability.timerIsRunning = true;
            InternetConnectionAvailability.call = false;
            AddUiMessage("CONNECTION FAILED! " + errorMessage);

        //    errorDialogScript.displayRetryDialog("CONNECTION FAILED! " + errorMessage, true, "JoinMatch", null);

        }

        protected override void OnDisconnected()
        {
            GameManager.gm.mqttIsConnected = false;
            InternetConnectionAvailability.timerIsRunning = true;
            InternetConnectionAvailability.call = false;
            AddUiMessage("Disconnected.");
        }

        protected override void OnConnectionLost()
        {
            GameManager.gm.mqttIsConnected = false;
            InternetConnectionAvailability.timerIsRunning = true;
            InternetConnectionAvailability.call = false;
            Disconnect();
            AddUiMessage("CONNECTION LOST!");

        }

        private void UpdateUI()
        {
            if (client == null)
            {
                if (connectButton != null)
                {
                    connectButton.interactable = true;
                    disconnectButton.interactable = false;
                    testPublishButton.interactable = false;
                }
            }
            else
            {
                if (testPublishButton != null)
                {
                    testPublishButton.interactable = client.IsConnected;
                }
                if (disconnectButton != null)
                {
                    disconnectButton.interactable = client.IsConnected;
                }
                if (connectButton != null)
                {
                    connectButton.interactable = !client.IsConnected;
                }
            }
            if (addressInputField != null && connectButton != null)
            {
                addressInputField.interactable = connectButton.interactable;
                addressInputField.text = brokerAddress;
            }
            if (portInputField != null && connectButton != null)
            {
                portInputField.interactable = connectButton.interactable;
                portInputField.text = brokerPort.ToString();
            }
            if (encryptedToggle != null && connectButton != null)
            {
                encryptedToggle.interactable = connectButton.interactable;
                encryptedToggle.isOn = isEncrypted;
            }
            if (clearButton != null && connectButton != null)
            {
                clearButton.interactable = connectButton.interactable;
            }
            updateUI = false;
        }

        protected override void Start()
        {
            SetUiMessage("Ready.");
            updateUI = true;
            strQ = new Queue<string>();
            base.Start();
        }

        protected override void DecodeMessage(string topic, byte[] message)
        {
            string msg = System.Text.Encoding.UTF8.GetString(message);
            Debug.Log("Received: " + msg);

                try
                {
                    JSONNode pokeInfo = JSON.Parse(msg);

                    JSONNode jsonInfo = pokeInfo["playEvent"];

                    JSONNode ackInfo = pokeInfo["ack"];

                    if (jsonInfo != null || ackInfo != null)
                    {
                        if(GameManager.gm.isAPIcallProcessing)
                        {
                            MqttMessageArray.Instance.addMQTTMsg(new MqMessage(msg));
                        }
                        else
                        {
                            if(!GameManager.gm.isBackground)
                            {
                                strQ.Enqueue(msg);

                                StartCoroutine(dequeMessages());
                            }
                            
                        }
                    
                    }
                    else
                    {

                    string json = "{\"JoinPlayerInfo\":" + msg + "}";
                    Debug.Log("Received: " + json);

                    JSONNode pokeInfo1 = JSON.Parse(json);

                    JSONNode info = pokeInfo1["JoinPlayerInfo"];

                    if (info["status"] != null && info["status"] == "eventGameStart")
                    {
                            actionHandler.action(info["status"], info);
                    }
                    else
                    {
                        for (int i = 0; i < info.Count; i++)
                        {
                            JoinPlayerInfo joinPlayerInfo = new JoinPlayerInfo(info[i]["id"], info[i]["match_id"], info[i]["user_id"], info[i]["email"], info[i]["first_name"], info[i]["last_name"], info[i]["username"]);
                            myObject.Add(joinPlayerInfo);
                        }

                        SameMarker.Instance.addJoinPlayerList(myObject);

                        myObject.Clear();

                        var waitingForOpponentGameObject = waitingForOpponent.getPanelObject();
                        if (waitingForOpponentGameObject.activeInHierarchy)
                            waitingForOpponent.updateText();
                    }
                }   

                }
                catch (Exception e)
                {
                    Debug.LogException(e, this);
                
                }

            // StoreMessage(msg);
            if (topic == Credentials.channel)
            {
                if (autoTest)
                {
                    autoTest = false;
                    Disconnect();
                }
            }
        }

        IEnumerator dequeMessages()
        {
            
            if (strQ.Count > 1)
            {
                for(int i = 0; i < strQ.Count; i++)
                {
                    string msg = strQ.Dequeue();
                    Messages(msg);
                    yield return new WaitForSeconds(2f);
                }
                
            }
            else
            {
                string msg = strQ.Dequeue();
                Messages(msg);
            }
           

        }

        void Messages(string msg)
        {
            JSONNode pokeInfo = JSON.Parse(msg);

            JSONNode jsonInfo = pokeInfo["playEvent"];

            JSONNode ackInfo = pokeInfo["ack"];

            if (jsonInfo != null)
            {
                if (jsonInfo["sender"] != Credentials.email)
                {
                    actionHandler.action(jsonInfo["action"], pokeInfo);
                }

            }
            else if (ackInfo != null)
            {
                if (ackInfo["sender"] != Credentials.email)
                {
                    actionHandler.action(ackInfo["action"], pokeInfo);
                }
            }
        }

        private void StoreMessage(string eventMsg)
        {
            eventMessages.Add(eventMsg);
        }

        private void ProcessMessage(string msg)
        {
            AddUiMessage("Received: " + msg);
        }

        protected override void Update()
        {
            base.Update(); // call ProcessMqttEvents()

            if (eventMessages.Count > 0)
            {
                foreach (string msg in eventMessages)
                {
                    ProcessMessage(msg);
                }
                eventMessages.Clear();
            }
            if (updateUI)
            {
                UpdateUI();
            }
        }

        private void OnDestroy()
        {
            Disconnect();
        }

        private void OnValidate()
        {
            if (autoTest)
            {
                autoConnect = true;
            }
        }
    }
}
