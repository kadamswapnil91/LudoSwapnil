    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.EventSystems;
    using System;
    using TMPro;
    using M2MqttUnity;



    public class GreenRollingDice : RollingDice
    {
      Coroutine rollDice_Coroutine;
      
     /* void Awake()
      {
        m2MqttUnityTest = FindObjectOfType<M2MqttUnity.Examples.M2MqttUnityTest>();
      }*/

      private void Update()
      {
        if(GameManager.gm.isGameRunning)
        {      	
          if(GameManager.gm.playerTurn == 1 && SameMarker.Instance.getPlayerArray()[0].isAI && !GameManager.gm.isAIPlayed)
          {
            GameManager.gm.isAIPlayed = true;
            rollDice_Coroutine = StartCoroutine(RollDice());
            print("rollDice_Coroutine");
          }
        }
      }

      public void OnMouseDown()
      {
        if(GameManager.gm.isOnlineGame)
        {
            if (!SameMarker.Instance.getPlayerArray()[0].isAI && !winnerResult.isShown() && !exitManager.isShown() && SameMarker.Instance.getPlayerArray()[0].player_name == Credentials.email)
            {
                GameManager.gm.botCount1 = 0;
                if (GameManager.gm.canDiceRoll)
                    GameManager.gm.isStartTimer = false;
                botDiceRolling(0, "actionGreenRollDice");
            }
        }
        else
        {
            if (!SameMarker.Instance.getPlayerArray()[0].isAI && !winnerResult.isShown() && !exitManager.isShown())
            {
                GameManager.gm.botCount1 = 0;
                if (GameManager.gm.canDiceRoll)
                    GameManager.gm.isStartTimer = false;
                botDiceRolling(0, "actionGreenRollDice");
            }
        }
        
      }


      IEnumerator RollDice()
      {
        yield return new WaitForSeconds(1f);

        if (GameManager.gm.isOnlineGame)
        {
          //mqttMessage.text = SameMarker.Instance.getPlayerArray()[0].isAI.ToString() + " : " + (SameMarker.Instance.getPlayerArray()[0].player_name == Credentials.email).ToString()+" : "+ GameManager.gm.canDiceRoll.ToString();
            if (GameManager.gm.playerTurn == 1 && SameMarker.Instance.getPlayerArray()[0].isAI && !winnerResult.isShown() && !exitManager.isShown() && SameMarker.Instance.getPlayerArray()[0].player_name == Credentials.email && GameManager.gm.mqttIsConnected)
            {
                botDiceRolling(0, "actionGreenRollDice");
            }
        }
        else
        {
            if (GameManager.gm.playerTurn == 1 && SameMarker.Instance.getPlayerArray()[0].isAI && !winnerResult.isShown() && !exitManager.isShown())
            {
                botDiceRolling(0, "actionGreenRollDice");
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
        if (!SameMarker.Instance.getPlayerArray()[0].isAI && !winnerResult.isShown() && !exitManager.isShown())
        {
            GameManager.gm.botCount1 = 0;
            if (GameManager.gm.canDiceRoll)
                GameManager.gm.isStartTimer = false;
            botDiceRolling(number,"");
        }
        else
        {
            if (SameMarker.Instance.getPlayerArray()[0].isAI && !winnerResult.isShown() && !exitManager.isShown())
            {
                botDiceRolling(number, "");
            }
        }
    }

}
