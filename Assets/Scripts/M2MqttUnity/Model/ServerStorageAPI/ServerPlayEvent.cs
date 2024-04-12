using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class ServerPlayEvent
{
    public string action;
    public string sender;
    public int turn_id;
    public PlayerPieceEvent playerPieceEvent;
    public QuitPlayer quitPlayer;
    public Bot bot;
}
