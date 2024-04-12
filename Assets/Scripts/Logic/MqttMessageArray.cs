using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class MqttMessageArray
{
    private static MqttMessageArray instance;
    private List<MqMessage> mqMessage;
    private List<MqMessage> messages;
    private List<string> offlinePublishSendMessages;

    private MqttMessageArray()
    {
        mqMessage = new List<MqMessage>();
        messages = new List<MqMessage>();
        offlinePublishSendMessages = new List<string>();
    }

    public static MqttMessageArray Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new MqttMessageArray();
            }
            return instance;
        }
    }

    public void addMessages(MqMessage msg)
    {
        this.mqMessage.Add(msg);
    }

    public void clearMovableList()
    {
        this.mqMessage.Clear();
    }

    public List<MqMessage> getMessageList()
    {
       return this.mqMessage;
    }

    public static void ClearInstance()
    {
        if (instance != null)
            instance = null;
    }

    public void addMQTTMsg(MqMessage msg)
    {
        this.messages.Add(msg);
    }

    public void clearList()
    {
        this.messages.Clear();
    }

    public List<MqMessage> getList()
    {
        return this.messages;
    }


    public void addOfflinePublishSendMsg(string msg)
    {
        this.offlinePublishSendMessages.Add(msg);
    }

    public void clearOfflineList()
    {
        this.offlinePublishSendMessages.Clear();
    }

    public List<string> getOfflinePublishList()
    {
        return this.offlinePublishSendMessages;
    }


}
