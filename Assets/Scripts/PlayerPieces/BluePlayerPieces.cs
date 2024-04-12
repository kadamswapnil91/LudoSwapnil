using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;


public class BluePlayerPieces : PlayerPiece, IPointerDownHandler
{
  private RollingDice rollingDice;

 
  
  public void OnMouseDown1(int playerId, int figureId, int pos)
    {
        // javaBridge.CallJavaMethodGetCountStep(GameManager.gm.pos)

        if (SameMarker.Instance.getCountStep(pos) == 0)
        {
            /*  if(GameManager.gm.numberOfStepsToMove == 6 && !GameManager.gm.isSixCountGraterThanTwo && GameManager.gm.isReadyToMove)
              {
                sendMqttMessage(playerId, figureId, pos);
                rollingDice.BluePlayerStopAnimaton();
                  MakePlayerReadyToMove(playerId, figureId, pathParent.bluePathPoints);
                  GameManager.gm.numberOfStepsToMove = 0;
                  return;
              }*/

            /// fast ludo clone 
            if ( !GameManager.gm.isSixCountGraterThanTwo && GameManager.gm.isReadyToMove)
            {
                //sendMqttMessage(playerId, figureId, pos);
                //rollingDice.BluePlayerStopAnimaton();
                //MakePlayerReadyToMove(playerId, figureId, pathParent.bluePathPoints);
                //GameManager.gm.numberOfStepsToMove = 0;

                sendMqttMessage(playerId, figureId, pos);
                MakePlayerReadyToMoveFastLudo();
                rollingDice.BluePlayerStopAnimaton();
                setMarker(playerId, figureId, pos);
                return;
            }
        }

      
       if(SameMarker.Instance.getCountStep(pos) > 0 && isPathPointsAvailableToMove(GameManager.gm.numberOfStepsToMove,SameMarker.Instance.getCountStep(pos),pathParent.bluePathPoints) && GameManager.gm.isReadyToMove && !GameManager.gm.isSixCountGraterThanTwo)
       {
            sendMqttMessage(playerId, figureId, pos);
            rollingDice.BluePlayerStopAnimaton();
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

        if(eventData.pointerCurrentRaycast.gameObject.name == "BluePlayerPieces1")
        {
              playerId = 4;
              figureId = 1;
              pos = 12;
        }else if(eventData.pointerCurrentRaycast.gameObject.name == "BluePlayerPieces2")
        {
              playerId = 4;
              figureId = 2;
              pos = 13;
        }else if(eventData.pointerCurrentRaycast.gameObject.name == "BluePlayerPieces3")
        {
              playerId = 4;
              figureId = 3;
              pos = 14;
        }else if(eventData.pointerCurrentRaycast.gameObject.name == "BluePlayerPieces4")
        {
              playerId = 4;
              figureId = 4;
              pos = 15;
        }

        //print(pos);
        if (GameManager.gm.isOnlineGame)
        {
            if (GameManager.gm.playerTurn == 4 && !winnerResult.isShown() && !exitManager.isShown() && SameMarker.Instance.getPlayerArray()[3].player_name == Credentials.email && !GameManager.gm.isAutomaticallyMovePlayerPiece)
            {
                GameManager.gm.isTapOnPlayerPiece = true;
                OnMouseDown1(playerId, figureId, pos);
            }
        }
        else
        {
            if (GameManager.gm.playerTurn == 4 && !winnerResult.isShown() && !exitManager.isShown() && !GameManager.gm.isAutomaticallyMovePlayerPiece)
                OnMouseDown1(playerId, figureId, pos);
        }
    }

   public int getNewPosition(int pos)
    {
        //fast ludo

        int markerPosition = SameMarker.Instance.getMarkerPosition(pos);
        if (markerPosition >= 100 && markerPosition <= 103) {
            markerPosition = 0;
        }
        else if (markerPosition >= 104 && markerPosition <= 107) {
            markerPosition = 26;
        }
        else if (markerPosition >= 108 && markerPosition <= 111) {
            markerPosition = 13;
        }
        else if (markerPosition >= 112 && markerPosition <= 115) {
            markerPosition = 39;
        }
        //
     // int new_position = SameMarker.Instance.getMarkerPosition(pos) + GameManager.gm.numberOfStepsToMove;
      int new_position = markerPosition + GameManager.gm.numberOfStepsToMove;

      if(new_position > 52)
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

      //print("Is SafeHouse : "+checkSafeHouse(getNewPosition(pos)));
      //print("New Position : "+getNewPosition(pos));
      int step = SameMarker.Instance.getCountStep(pos) + GameManager.gm.numberOfStepsToMove;

      if(!checkSafeHouse(getNewPosition(pos)) && step < 52)
      {

        isKillingList = SameMarker.Instance.getNewPositionOpponentMarkerList(getNewPosition(pos),playerId);

        //print(playerId);
      
      }

      print("step : "+ step + " isKillingList : "+ isKillingList.Count + " New Position : "+ getNewPosition(pos) + " playerId : "+ playerId);


      //print("Blue Marker Killing List Count : "+ isKillingList.Count);
      
      canMove = true;
      MoveSteps(playerId, figureId,pathParent.bluePathPoints, isKillingList);

    }


     public void OnPublishMouseDown1(int playerId, int figureId, int pos)
    {

        //fast ludo comment
        //if (SameMarker.Instance.getCountStep(pos) == 0)
        //{
        //    if (GameManager.gm.numberOfStepsToMove == 6 && !GameManager.gm.isSixCountGraterThanTwo && GameManager.gm.isReadyToMove)
        //    {
        //        rollingDice.BluePlayerStopAnimaton();
        //        MakePlayerReadyToMove(playerId, figureId, pathParent.bluePathPoints);
        //        GameManager.gm.numberOfStepsToMove = 0;
        //        return;
        //    }
        //}

        if (SameMarker.Instance.getCountStep(pos) == 0)
        {
            if (!GameManager.gm.isSixCountGraterThanTwo && GameManager.gm.isReadyToMove)
            {
                //rollingDice.BluePlayerStopAnimaton();
                //MakePlayerReadyToMove(playerId, figureId, pathParent.bluePathPoints);
                //GameManager.gm.numberOfStepsToMove = 0;
                MakePlayerReadyToMoveFastLudo();
                rollingDice.BluePlayerStopAnimaton();
                setMarker(playerId, figureId, pos);
                return;
            }
        }

        if (SameMarker.Instance.getCountStep(pos) > 0 && isPathPointsAvailableToMove(GameManager.gm.numberOfStepsToMove, SameMarker.Instance.getCountStep(pos), pathParent.bluePathPoints) && GameManager.gm.isReadyToMove && !GameManager.gm.isSixCountGraterThanTwo)
        {
            rollingDice.BluePlayerStopAnimaton();
            setMarker(playerId, figureId, pos);
        }

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
            playEvent.action = "actionMoveBluePiece";
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
