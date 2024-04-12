using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable()]
public class Player
{
	public Player(int player_id, string player_name, bool isAI, bool isConvertedToBot, bool isQuit, bool isWinner, string color, int user_id, string username) {
        this.player_id = player_id;
        this.player_name = player_name;
        this.isAI = isAI;
        this.isConvertedToBot = isConvertedToBot;
        this.isQuit = isQuit;
        this.isWinner = isWinner;
        this.color = color;
        this.user_id = user_id;
        this.username = username;
    }
    
     public int player_id { get; set; }
     public string player_name { get; set; }
     public bool isAI { get; set; }
     public bool isConvertedToBot { get; set; }
     public bool isQuit { get; set; }
     public bool isWinner { get; set; }
     public string color { get; set; }
     public int user_id { get; set; }
     public string username { get; set; }
}

