using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleJSON;
using System;


public class DemoServerMessages : MonoBehaviour
{
    [SerializeField] Button serverMessageButton;
    List<string> mqttMessages;
    YellowPlayerPieces yellowPlayerPieces;

    private void Start()
    {
        mqttMessages = new List<string>();
        serverMessageButton.onClick.AddListener(getServerMessages);
    }

    private void Awake()
    {
        yellowPlayerPieces = FindObjectOfType<YellowPlayerPieces>();
    }


    private void getServerMessages()
    {
        
        PlayerPieceEvent playerPieceEvent = new PlayerPieceEvent();
        playerPieceEvent.playerId = 1;
        playerPieceEvent.figureId = 1;
        playerPieceEvent.pos = 0;
        playerPieceEvent.dice_value = 6;

        PlayEvent playEvent = new PlayEvent();
        playEvent.action = "actionMoveYellowPiece";
        playEvent.sender = Credentials.email;
        playEvent.playerPieceEvent = playerPieceEvent;
        playEvent.bot = null;

        MqttEvent mqttEvent = new MqttEvent();
        mqttEvent.playEvent = playEvent;

        string json = JsonUtility.ToJson(mqttEvent);

      

        PlayerPieceEvent playerPieceEvent2 = new PlayerPieceEvent();
        playerPieceEvent2.playerId = 1;
        playerPieceEvent2.figureId = 1;
        playerPieceEvent2.pos = 0;
        playerPieceEvent2.dice_value = 5;

        PlayEvent playEvent1 = new PlayEvent();
        playEvent1.action = "actionMoveYellowPiece";
        playEvent1.sender = Credentials.email;
        playEvent1.playerPieceEvent = playerPieceEvent;
        playEvent1.bot = null;

        MqttEvent mqttEvent1 = new MqttEvent();
        mqttEvent1.playEvent = playEvent;

        string json2 = JsonUtility.ToJson(mqttEvent1);

        mqttMessages.Add(json);
        mqttMessages.Add(json2);

        StartCoroutine(getMessages());

    }

    IEnumerator getMessages()
    {
        for (int i = 0; i < mqttMessages.Count; i++)
        {
            decodeJson(mqttMessages[i]);

            yield return new WaitForSeconds(3f);
        }
    }


   public void decodeJson(string msg)
    {

        try
        {
            JSONNode pokeInfo = JSON.Parse(msg);

            JSONNode jsonInfo = pokeInfo["playEvent"];

            JSONNode ackInfo = pokeInfo["ack"];

            JSONNode info = pokeInfo["playEvent"]["playerPieceEvent"];

            GameManager.gm.numberOfStepsToMove = info["dice_value"];
            GameManager.gm.playerTurn = info["playerId"];
            GameManager.gm.isReadyToMove = true;

            yellowPlayerPieces.OnPublishMouseDown1(info["playerId"], info["figureId"], info["pos"]);



        }
        catch (Exception e)
        {
            Debug.LogException(e, this);

        }
    }
}


