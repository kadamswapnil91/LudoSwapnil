using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class RedPlayerPieces : PlayerPiece, IPointerDownHandler
{
    private RollingDice rollingDice;


    public void OnMouseDown1(int playerId, int figureId, int pos)
    {
        // javaBridge.CallJavaMethodGetCountStep(GameManager.gm.pos);

        print("Move Red Piece");

        if(SameMarker.Instance.getCountStep(pos) == 0)
        {
            //if(GameManager.gm.numberOfStepsToMove == 6 && !GameManager.gm.isSixCountGraterThanTwo && GameManager.gm.isReadyToMove)
            //{
            //      sendMqttMessage(playerId, figureId, pos);
            //  rollingDice.RedPlayerStopAnimaton();
            //    MakePlayerReadyToMove(playerId, figureId, pathParent.redPathPoints);
            //    GameManager.gm.numberOfStepsToMove = 0;
            //    return;
            //}

            // remove code fast ludo
            if ( !GameManager.gm.isSixCountGraterThanTwo && GameManager.gm.isReadyToMove)
            {
                //sendMqttMessage(playerId, figureId, pos);
                //rollingDice.RedPlayerStopAnimaton();
                //MakePlayerReadyToMove(playerId, figureId, pathParent.redPathPoints);
                //GameManager.gm.numberOfStepsToMove = 0;

                sendMqttMessage(playerId, figureId, pos);
                MakePlayerReadyToMoveFastLudo();
                rollingDice.RedPlayerStopAnimaton();
                setMarker(playerId, figureId, pos);
                return;
            }
        }

       if(SameMarker.Instance.getCountStep(pos) > 0 && isPathPointsAvailableToMove(GameManager.gm.numberOfStepsToMove,SameMarker.Instance.getCountStep(pos),pathParent.redPathPoints) && GameManager.gm.isReadyToMove && !GameManager.gm.isSixCountGraterThanTwo)
       {
            sendMqttMessage(playerId, figureId, pos);
            rollingDice.RedPlayerStopAnimaton();
            setMarker(playerId,figureId,pos);
       }
       
        
       //StartCoroutine("MoveStepsEnum");
    }


    private void Start()
    {

      rollingDice = FindObjectOfType<RollingDice>();       
      Physics2DRaycaster physicsRaycaster = GameObject.FindObjectOfType<Physics2DRaycaster>();

           if (physicsRaycaster == null)
        {
            Camera.main.gameObject.AddComponent<Physics2DRaycaster>();
        }
    }

     public void OnPointerDown(PointerEventData eventData)
    {
      int playerId = 0;
      int figureId = 0; 
      int pos = 0;  
        
       if(eventData.pointerCurrentRaycast.gameObject.name == "RedPlayerPieces1")
        {
              playerId = 3;
              figureId = 1;
              pos = 4;
        }else if(eventData.pointerCurrentRaycast.gameObject.name == "RedPlayerPieces2")
        {
              playerId = 3;
              figureId = 2;
              pos = 5;
        }else if(eventData.pointerCurrentRaycast.gameObject.name == "RedPlayerPieces3")
        {
              playerId = 3;
              figureId = 3;
              pos = 6;
        }else if(eventData.pointerCurrentRaycast.gameObject.name == "RedPlayerPieces4")
        {
              playerId = 3;
              figureId = 4;
              pos = 7;
              // javaBridge.CallJavaMethodGetMarkerList();
        }
      //  playerName1.text = SameMarker.Instance.getCountStep(pos).ToString();
      //  playerName2.text = playerId + " : : "+ figureId;

       if(GameManager.gm.isOnlineGame)
        {
            if (GameManager.gm.playerTurn == 3 && !winnerResult.isShown() && !exitManager.isShown() && SameMarker.Instance.getPlayerArray()[getIndex()].player_name == Credentials.email && !GameManager.gm.isAutomaticallyMovePlayerPiece)
            {
                GameManager.gm.isTapOnPlayerPiece = true;
                OnMouseDown1(playerId, figureId, pos);
            }
        }
       else
        {
            if (GameManager.gm.playerTurn == 3 && !winnerResult.isShown() && !exitManager.isShown() && !GameManager.gm.isAutomaticallyMovePlayerPiece)
                OnMouseDown1(playerId, figureId, pos);
        }
       
    }

 public int getNewPosition(int pos)
    {
        //fast ludo

        int markerPosition = SameMarker.Instance.getMarkerPosition(pos);
        if (markerPosition >= 100 && markerPosition <= 103)
        {
            markerPosition = 0;
        }
        else if (markerPosition >= 104 && markerPosition <= 107)
        {
            markerPosition = 26;
        }
        else if (markerPosition >= 108 && markerPosition <= 111)
        {
            markerPosition = 13;
        }
        else if (markerPosition >= 112 && markerPosition <= 115)
        {
            markerPosition = 39;
        }
        //
        //int new_position = SameMarker.Instance.getMarkerPosition(pos) + GameManager.gm.numberOfStepsToMove;
        int new_position = markerPosition + GameManager.gm.numberOfStepsToMove;

        //int new_position = SameMarker.Instance.getMarkerPosition(pos) + GameManager.gm.numberOfStepsToMove;


        if (new_position > 52)
      {
        int step = SameMarker.Instance.getCountStep(pos) + GameManager.gm.numberOfStepsToMove;
          if(step < 52)
          {
              new_position = new_position - 52;
          }
        
      }
     
      return new_position;
    }



    public void setMarker(int playerId, int figureId, int pos)
    {

      List<Safe> isKillingList = new List<Safe>();

      int step = SameMarker.Instance.getCountStep(pos) + GameManager.gm.numberOfStepsToMove;


      if(!checkSafeHouse(getNewPosition(pos)) && step < 52)
      {

        isKillingList = SameMarker.Instance.getNewPositionOpponentMarkerList(getNewPosition(pos),playerId);      
      }
      print("step : "+ step + " isKillingList : "+ isKillingList.Count + " New Position : "+ getNewPosition(pos) + " playerId : "+ playerId);

      canMove = true;
      MoveSteps(playerId, figureId,pathParent.redPathPoints, isKillingList); 

    }


       public void OnPublishMouseDown1(int playerId, int figureId, int pos)
    {
        // fast ludo commnet 
        //if (SameMarker.Instance.getCountStep(pos) == 0)
        //{
        //    if (GameManager.gm.numberOfStepsToMove == 6 && !GameManager.gm.isSixCountGraterThanTwo && GameManager.gm.isReadyToMove)
        //    {
        //        rollingDice.RedPlayerStopAnimaton();
        //        MakePlayerReadyToMove(playerId, figureId, pathParent.redPathPoints);
        //        GameManager.gm.numberOfStepsToMove = 0;
        //        return;
        //    }
        //}


        // fast ludo code
        if (SameMarker.Instance.getCountStep(pos) == 0)
        {
            if (!GameManager.gm.isSixCountGraterThanTwo && GameManager.gm.isReadyToMove)
            {
                MakePlayerReadyToMoveFastLudo();
                rollingDice.RedPlayerStopAnimaton();
                setMarker(playerId, figureId, pos);
                //MakePlayerReadyToMove(playerId, figureId, pathParent.redPathPoints);
                //GameManager.gm.numberOfStepsToMove = 0;
                return;
            }
        }

        if (SameMarker.Instance.getCountStep(pos) > 0 && isPathPointsAvailableToMove(GameManager.gm.numberOfStepsToMove, SameMarker.Instance.getCountStep(pos), pathParent.redPathPoints) && GameManager.gm.isReadyToMove && !GameManager.gm.isSixCountGraterThanTwo)
        {
            rollingDice.RedPlayerStopAnimaton();
            setMarker(playerId, figureId, pos);
        }

    }


    int getIndex()
    {
        int index = GameManager.gm.playerTurn;
        if (SameMarker.Instance.getPlayerArray().Count == 2)
        {
            if (index == 3)
                index = 1;
            else
                index = index - 1;
        }
        else
        {
            index = index - 1;
        }

        return index;
    }


    void sendMqttMessage(int playerId, int figureId, int pos)
    {
        
            PlayerPieceEvent playerPieceEvent = new PlayerPieceEvent();
            playerPieceEvent.playerId = playerId;
            playerPieceEvent.figureId = figureId;
            playerPieceEvent.pos = pos;
            playerPieceEvent.dice_value = GameManager.gm.numberOfStepsToMove;
            playerPieceEvent.player_chance = GameManager.gm.playerChance;
            playerPieceEvent.sixCount = GameManager.gm.sixCount;

            PlayEvent playEvent = new PlayEvent();
            playEvent.action = "actionMoveRedPiece";
            playEvent.sender = Credentials.email;
            playEvent.playerPieceEvent = playerPieceEvent;
            playEvent.bot = null;

            MqttEvent mqttEvent = new MqttEvent();
            mqttEvent.playEvent = playEvent;

            string json = JsonUtility.ToJson(mqttEvent);

            Credentials.action = json;
            m2MqttUnityTest.TestPublish();
    }

}
