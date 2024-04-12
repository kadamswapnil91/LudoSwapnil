using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable()]
public class GameManagerPojo
{
    public GameManagerPojo(int numberOfStepsToMove, int dice, int sixCount, bool isKilled, 
        int playerChance, bool isReadyToMove, bool isGameRunning, bool isSixCountGraterThanTwo,
        bool canDiceRoll, int playerTurn, bool isAIPlayed, int countDownStartValue, int botCount1, 
        int botCount2, int botCount3, int botCount4) {
        this.numberOfStepsToMove = numberOfStepsToMove;
        this.dice = dice;
        this.sixCount = sixCount;
        this.isKilled = isKilled;
        this.playerChance = playerChance;
        this.isReadyToMove = isReadyToMove;
        this.isGameRunning = isGameRunning;
        this.isSixCountGraterThanTwo = isSixCountGraterThanTwo;
        this.canDiceRoll = canDiceRoll;
        this.playerTurn = playerTurn;
        this.isAIPlayed = isAIPlayed;
        this.countDownStartValue = countDownStartValue;
        this.botCount1 = botCount1;
        this.botCount2 = botCount2;
        this.botCount3 = botCount3;
        this.botCount4 = botCount4;
    }
    
     public int numberOfStepsToMove { get; set; }
     public int dice { get; set; }
     public int sixCount { get; set; }
     public bool isKilled { get; set; }
     public int playerChance { get; set; }
	 public bool isReadyToMove { get; set; }
	 public bool isGameRunning { get; set; }
	 public bool isSixCountGraterThanTwo { get; set; }
	 public bool canDiceRoll { get; set; }
	 public int playerTurn { get; set; }
     public bool isAIPlayed { get; set; }
     public int countDownStartValue { get; set; }
     public int botCount1 { get; set; }
     public int botCount2 { get; set; }
     public int botCount3 { get; set; }
     public int botCount4 { get; set; }

       
}
