using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[Serializable]
public class PlayEvent
{	
   public string action;
   public string sender;
   public int numberGot;
   public PlayerPieceEvent playerPieceEvent;
   public Bot bot;
   public Acknowledgement ack;
   public QuitPlayer quitPlayer;
   public Winner winner;

}