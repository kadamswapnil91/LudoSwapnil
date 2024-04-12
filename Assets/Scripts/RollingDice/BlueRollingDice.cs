    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.EventSystems;
    using System;

    public class BlueRollingDice : RollingDice
    {
        Coroutine rollDice_Coroutine;

            private void Update()
            {
              if(GameManager.gm.isGameRunning)
              {
                if(SameMarker.Instance.getPlayerArray().Count == 2)
                {
                      if(GameManager.gm.playerTurn == 3 && SameMarker.Instance.getPlayerArray()[1].isAI && !GameManager.gm.isAIPlayed)
                      {
                        GameManager.gm.isAIPlayed = true;
                        rollDice_Coroutine = StartCoroutine(RollDice());
                        print("rollDice_Coroutine");
                      }
                }
                else
                {
                    if(GameManager.gm.playerTurn == 3 && SameMarker.Instance.getPlayerArray()[2].isAI && !GameManager.gm.isAIPlayed)
                    {
                      GameManager.gm.isAIPlayed = true;
                      rollDice_Coroutine = StartCoroutine(RollDice());
                      print("rollDice_Coroutine");
                    }

                }
              }        
                
            }

            public void OnMouseDown()
            {
              if(SameMarker.Instance.getPlayerArray().Count == 2)
              {
                if (GameManager.gm.isOnlineGame)
                {
                    if (!SameMarker.Instance.getPlayerArray()[1].isAI && !winnerResult.isShown() && !exitManager.isShown() && SameMarker.Instance.getPlayerArray()[1].player_name == Credentials.email)
                    {
                        GameManager.gm.botCount3 = 0;
                        if (GameManager.gm.canDiceRoll)
                            GameManager.gm.isStartTimer = false;
                        botDiceRolling(0, "actionBlueRollDice");
                    }
                }
                else
                {
                    if (!SameMarker.Instance.getPlayerArray()[1].isAI && !winnerResult.isShown() && !exitManager.isShown())
                    {
                        GameManager.gm.botCount3 = 0;
                        if (GameManager.gm.canDiceRoll)
                            GameManager.gm.isStartTimer = false;
                        botDiceRolling(0, "actionBlueRollDice");
                    }
                }
              }
              else
              {

                    if (GameManager.gm.isOnlineGame)
                    {
                        if (!SameMarker.Instance.getPlayerArray()[2].isAI && !winnerResult.isShown() && !exitManager.isShown() && SameMarker.Instance.getPlayerArray()[2].player_name == Credentials.email)
                        {
                            GameManager.gm.botCount3 = 0;
                            if (GameManager.gm.canDiceRoll)
                                GameManager.gm.isStartTimer = false;
                            botDiceRolling(0, "actionBlueRollDice");
                        }
            
                    }
                    else
                    {
                        if (!SameMarker.Instance.getPlayerArray()[2].isAI && !winnerResult.isShown() && !exitManager.isShown())
                        {
                            GameManager.gm.botCount3 = 0;
                            if (GameManager.gm.canDiceRoll)
                                GameManager.gm.isStartTimer = false;
                            botDiceRolling(0, "actionBlueRollDice");
                        }
                    }
              }
            }



            IEnumerator RollDice()
            {
              yield return new WaitForSeconds(1f);
                if (GameManager.gm.isOnlineGame)
                {
                    if (SameMarker.Instance.getPlayerArray().Count == 2)
                    {
                        if (GameManager.gm.playerTurn == 3 && SameMarker.Instance.getPlayerArray()[1].isAI && !winnerResult.isShown() && !exitManager.isShown() && SameMarker.Instance.getPlayerArray()[1].player_name == Credentials.email && GameManager.gm.mqttIsConnected)
                        {
                            botDiceRolling(0, "actionBlueRollDice");
                        }
                    }
                    else
                    {
                        if (GameManager.gm.playerTurn == 3 && SameMarker.Instance.getPlayerArray()[2].isAI && !winnerResult.isShown() && !exitManager.isShown() && SameMarker.Instance.getPlayerArray()[2].player_name == Credentials.email && GameManager.gm.mqttIsConnected)
                        {
                            botDiceRolling(0, "actionBlueRollDice");
                        }
                    }
                }
                else
                {
                        if (SameMarker.Instance.getPlayerArray().Count == 2)
                        {
                            if (GameManager.gm.playerTurn == 3 && SameMarker.Instance.getPlayerArray()[1].isAI && !winnerResult.isShown() && !exitManager.isShown())
                            {
                                botDiceRolling(0, "actionBlueRollDice");
                            }
                        }
                        else
                        {
                            if (GameManager.gm.playerTurn == 3 && SameMarker.Instance.getPlayerArray()[2].isAI && !winnerResult.isShown() && !exitManager.isShown())
                            {
                                botDiceRolling(0, "actionBlueRollDice");
                            }
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
        if (SameMarker.Instance.getPlayerArray().Count == 2)
        {
            if (!SameMarker.Instance.getPlayerArray()[1].isAI && !winnerResult.isShown() && !exitManager.isShown())
            {
                GameManager.gm.botCount3 = 0;
                if (GameManager.gm.canDiceRoll)
                    GameManager.gm.isStartTimer = false;
                botDiceRolling(number, "");

            }
            else
            {
                if (SameMarker.Instance.getPlayerArray()[1].isAI && !winnerResult.isShown() && !exitManager.isShown())
                {
                    botDiceRolling(number, "");
                }
            }
        }
        else
        {
            if (!SameMarker.Instance.getPlayerArray()[2].isAI && !winnerResult.isShown() && !exitManager.isShown())
            {
                GameManager.gm.botCount3 = 0;
                if (GameManager.gm.canDiceRoll)
                    GameManager.gm.isStartTimer = false;
                    botDiceRolling(number, "");

            }
            else
            {
                if (SameMarker.Instance.getPlayerArray()[2].isAI && !winnerResult.isShown() && !exitManager.isShown())
                {
                    botDiceRolling(number, "");
                }
            }
        }
    }

}
