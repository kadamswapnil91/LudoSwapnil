    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.EventSystems;
    using System;

    public class YellowRollingDice : RollingDice
    {
	    Coroutine rollDice_Coroutine;

	      private void Update()
	      {
	      	  if(GameManager.gm.isGameRunning)
	      	  {
		      	  	if(GameManager.gm.playerTurn == 4 && SameMarker.Instance.getPlayerArray()[3].isAI && !GameManager.gm.isAIPlayed)
			          {
			            GameManager.gm.isAIPlayed = true;
			            rollDice_Coroutine = StartCoroutine(RollDice());
			          }
	      	  }
	          
	          
	      }

	      public void OnMouseDown()
	      {
			if(GameManager.gm.isOnlineGame)
			{
				if (!SameMarker.Instance.getPlayerArray()[3].isAI && !winnerResult.isShown() && !exitManager.isShown() && SameMarker.Instance.getPlayerArray()[3].player_name == Credentials.email && GameManager.gm.mqttIsConnected)
				{
					GameManager.gm.botCount4 = 0;
					if (GameManager.gm.canDiceRoll)
						GameManager.gm.isStartTimer = false;
					botDiceRolling(0, "actionYellowRollDice");
				}
			}
			else
			{
				if (!SameMarker.Instance.getPlayerArray()[3].isAI && !winnerResult.isShown() && !exitManager.isShown())
				{
					GameManager.gm.botCount4 = 0;
					if (GameManager.gm.canDiceRoll)
						GameManager.gm.isStartTimer = false;
					botDiceRolling(0, "actionYellowRollDice");
				}
			}
	        
	      }



	      IEnumerator RollDice()
	      {
	        yield return new WaitForSeconds(1f);
			if (GameManager.gm.isOnlineGame)
			{
				if (GameManager.gm.playerTurn == 4 && SameMarker.Instance.getPlayerArray()[3].isAI && !winnerResult.isShown() && !exitManager.isShown() && SameMarker.Instance.getPlayerArray()[3].player_name == Credentials.email)
				{

					botDiceRolling(0, "actionYellowRollDice");
				}
			}
			else
			{
				if (GameManager.gm.playerTurn == 4 && SameMarker.Instance.getPlayerArray()[3].isAI && !winnerResult.isShown() && !exitManager.isShown())
				{

					botDiceRolling(0, "actionYellowRollDice");
				}
			}

	          yield return new WaitForEndOfFrame();

	          if(rollDice_Coroutine != null)
	          {
	              StopCoroutine(rollDice_Coroutine);
	          }
	      }

	public void OnPublishMouseDown(int number)
	{
		if (!SameMarker.Instance.getPlayerArray()[3].isAI && !winnerResult.isShown() && !exitManager.isShown())
		{
			GameManager.gm.botCount4 = 0;
			if (GameManager.gm.canDiceRoll)
				GameManager.gm.isStartTimer = false;
				botDiceRolling(number, "");

		}
		else
		{
			if (SameMarker.Instance.getPlayerArray()[3].isAI && !winnerResult.isShown() && !exitManager.isShown())
			{
				botDiceRolling(number, "");
			}
		}
	}


}
