using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteHandler : MonoBehaviour
{
    [SerializeField] GameObject greenBoard, redBoard, blueBoard, yellowBoard, winingBoard;

	[SerializeField] GameObject greenSucessPath, redSucessPath, blueSucessPath, yellowSucessPath;

	[SerializeField] GameObject greenSafeHouse, redSafeHouse, blueSafeHouse, yellowSafeHouse;

	[SerializeField] GameObject greenTimer, redTimer, blueTimer, yellowTimer;

    [SerializeField] Sprite greenSprite, redSprite, blueSprite, yellowSprite;

    [SerializeField] Sprite greenPathSprite, redPathSprite, bluePathSprite, yellowPathSprite;

    [SerializeField] Sprite greenSafeHouseSprite, redSafeHouseSprite, blueSafeHouseSprite, yellowSafeHouseSprite;

    [SerializeField] Sprite greenTimerSprite, redTimerSprite, blueTimerSprite, yellowTimerSprite;

   	public GameObject[] greenPieces;
   	public GameObject[] redPieces;
   	public GameObject[] bluePieces;
   	public GameObject[] yellowPieces;

    [SerializeField] Sprite greenMarker, redMarker, blueMarker, yellowMarker;

    private SpriteRenderer GreenBoardSpriteRenderer,RedBoardSpriteRenderer,BlueBoardSpriteRenderer,YellowBoardSpriteRenderer; 

    private SpriteRenderer GreenPathSpriteRenderer,RedPathSpriteRenderer,BluePathSpriteRenderer,YellowPathSpriteRenderer; 

    void Start ()
	{
	    GreenBoardSpriteRenderer = greenBoard.GetComponent<SpriteRenderer>();
	    RedBoardSpriteRenderer = redBoard.GetComponent<SpriteRenderer>();
	    BlueBoardSpriteRenderer = blueBoard.GetComponent<SpriteRenderer>();
	    YellowBoardSpriteRenderer = yellowBoard.GetComponent<SpriteRenderer>();

        GameManager.gm.firstPlayerColor = "green";
        GameManager.gm.secondPlayerColor = "red";
        GameManager.gm.thirdPlayerColor = "blue";
        GameManager.gm.fourthPlayerColor = "yellow";

	    GreenPathSpriteRenderer = greenSucessPath.GetComponent<SpriteRenderer>();
	    RedPathSpriteRenderer = redSucessPath.GetComponent<SpriteRenderer>();
	    BluePathSpriteRenderer = blueSucessPath.GetComponent<SpriteRenderer>();
	    YellowPathSpriteRenderer = yellowSucessPath.GetComponent<SpriteRenderer>();


         YellowBoardSpriteRenderer.sprite = greenSprite;
         GreenBoardSpriteRenderer.sprite = redSprite;
         RedBoardSpriteRenderer.sprite = blueSprite;
         BlueBoardSpriteRenderer.sprite = yellowSprite;

         winingBoard.transform.localRotation = Quaternion.Euler(0f, 0f, -270f);

         YellowPathSpriteRenderer.sprite = greenPathSprite;
         GreenPathSpriteRenderer.sprite = redPathSprite;
         RedPathSpriteRenderer.sprite = bluePathSprite;
         BluePathSpriteRenderer.sprite = yellowPathSprite;

         yellowSafeHouse.GetComponent<SpriteRenderer>().sprite = greenSafeHouseSprite;
         greenSafeHouse.GetComponent<SpriteRenderer>().sprite = redSafeHouseSprite;
         redSafeHouse.GetComponent<SpriteRenderer>().sprite = blueSafeHouseSprite;
         blueSafeHouse.GetComponent<SpriteRenderer>().sprite = yellowSafeHouseSprite;

         greenTimer.GetComponent<Image>().sprite = greenTimerSprite;
         redTimer.GetComponent<Image>().sprite = redTimerSprite;
         blueTimer.GetComponent<Image>().sprite = blueTimerSprite;
         yellowTimer.GetComponent<Image>().sprite = yellowTimerSprite;


            for(int i=0; i < yellowPieces.Length ; i++)
            {
               yellowPieces[i].GetComponent<SpriteRenderer>().sprite = greenMarker;
            }

            for(int i=0; i < greenPieces.Length ; i++)
            {

               greenPieces[i].GetComponent<SpriteRenderer>().sprite = redMarker;
            }
            
            for(int i=0; i < redPieces.Length ; i++)
            {
               redPieces[i].GetComponent<SpriteRenderer>().sprite = blueMarker;
            }

            for(int i=0; i < bluePieces.Length ; i++)
            {
               bluePieces[i].GetComponent<SpriteRenderer>().sprite = yellowMarker;
            }


	}


   	public void changeBoardSprite(int count)
   	{
   		if(count == 1)
   		{

   		YellowBoardSpriteRenderer.sprite = yellowSprite;
			GreenBoardSpriteRenderer.sprite = greenSprite;
			RedBoardSpriteRenderer.sprite = redSprite;
			BlueBoardSpriteRenderer.sprite = blueSprite;

         GameManager.gm.firstPlayerColor = "yellow";
         GameManager.gm.secondPlayerColor = "green";
         GameManager.gm.thirdPlayerColor = "red";
         GameManager.gm.fourthPlayerColor = "blue";

			winingBoard.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);

			YellowPathSpriteRenderer.sprite = yellowPathSprite;
			GreenPathSpriteRenderer.sprite = greenPathSprite;
			RedPathSpriteRenderer.sprite = redPathSprite;
			BluePathSpriteRenderer.sprite = bluePathSprite;

			yellowSafeHouse.GetComponent<SpriteRenderer>().sprite = yellowSafeHouseSprite;
			greenSafeHouse.GetComponent<SpriteRenderer>().sprite = greenSafeHouseSprite;
			redSafeHouse.GetComponent<SpriteRenderer>().sprite = redSafeHouseSprite;
			blueSafeHouse.GetComponent<SpriteRenderer>().sprite = blueSafeHouseSprite;

			greenTimer.GetComponent<Image>().sprite = yellowTimerSprite;
			redTimer.GetComponent<Image>().sprite = greenTimerSprite;
			blueTimer.GetComponent<Image>().sprite = redTimerSprite;
			yellowTimer.GetComponent<Image>().sprite = blueTimerSprite;

            for(int i=0; i < yellowPieces.Length ; i++)
            {
               yellowPieces[i].GetComponent<SpriteRenderer>().sprite = yellowMarker;
            }

   			for(int i=0; i < greenPieces.Length ; i++)
   			{

   				greenPieces[i].GetComponent<SpriteRenderer>().sprite = greenMarker;
   			}
   		  	
   			for(int i=0; i < redPieces.Length ; i++)
   			{
   				redPieces[i].GetComponent<SpriteRenderer>().sprite = redMarker;
   			}

   			for(int i=0; i < bluePieces.Length ; i++)
   			{
   				bluePieces[i].GetComponent<SpriteRenderer>().sprite = blueMarker;
   			}

   		}
   		else if(count == 2)
   		{

   		YellowBoardSpriteRenderer.sprite = blueSprite;
			GreenBoardSpriteRenderer.sprite = yellowSprite;
			RedBoardSpriteRenderer.sprite = greenSprite;
			BlueBoardSpriteRenderer.sprite = redSprite;

         GameManager.gm.firstPlayerColor = "blue";
         GameManager.gm.secondPlayerColor = "yellow";
         GameManager.gm.thirdPlayerColor = "green";
         GameManager.gm.fourthPlayerColor = "red";

			winingBoard.transform.localRotation = Quaternion.Euler(0f, 0f, -90f);

			YellowPathSpriteRenderer.sprite = bluePathSprite;
			GreenPathSpriteRenderer.sprite = yellowPathSprite;
			RedPathSpriteRenderer.sprite = greenPathSprite;
			BluePathSpriteRenderer.sprite = redPathSprite;

			yellowSafeHouse.GetComponent<SpriteRenderer>().sprite = blueSafeHouseSprite;
			greenSafeHouse.GetComponent<SpriteRenderer>().sprite = yellowSafeHouseSprite;
			redSafeHouse.GetComponent<SpriteRenderer>().sprite = greenSafeHouseSprite;
			blueSafeHouse.GetComponent<SpriteRenderer>().sprite = redSafeHouseSprite;

			greenTimer.GetComponent<Image>().sprite = blueTimerSprite;
			redTimer.GetComponent<Image>().sprite = yellowTimerSprite;
			blueTimer.GetComponent<Image>().sprite = greenTimerSprite;
			yellowTimer.GetComponent<Image>().sprite = redTimerSprite;

            for(int i=0; i < yellowPieces.Length ; i++)
            {
               yellowPieces[i].GetComponent<SpriteRenderer>().sprite = blueMarker;
            }

   			for(int i=0; i < greenPieces.Length ; i++)
   			{

   				greenPieces[i].GetComponent<SpriteRenderer>().sprite = yellowMarker;
   			}
   		  	
   			for(int i=0; i < redPieces.Length ; i++)
   			{
   				redPieces[i].GetComponent<SpriteRenderer>().sprite = greenMarker;
   			}

   			for(int i=0; i < bluePieces.Length ; i++)
   			{
   				bluePieces[i].GetComponent<SpriteRenderer>().sprite = redMarker;
   			}

   		}
   		else if(count == 3)
   		{

   		YellowBoardSpriteRenderer.sprite = redSprite;
			GreenBoardSpriteRenderer.sprite = blueSprite;
			RedBoardSpriteRenderer.sprite = yellowSprite;
			BlueBoardSpriteRenderer.sprite = greenSprite;

         GameManager.gm.firstPlayerColor = "red";
         GameManager.gm.secondPlayerColor = "blue";
         GameManager.gm.thirdPlayerColor = "yellow";
         GameManager.gm.fourthPlayerColor = "green";

			winingBoard.transform.localRotation = Quaternion.Euler(0f, 0f, -180f);

			YellowPathSpriteRenderer.sprite = redPathSprite;
			GreenPathSpriteRenderer.sprite = bluePathSprite;
			RedPathSpriteRenderer.sprite = yellowPathSprite;
			BluePathSpriteRenderer.sprite = greenPathSprite;

			yellowSafeHouse.GetComponent<SpriteRenderer>().sprite = redSafeHouseSprite;
			greenSafeHouse.GetComponent<SpriteRenderer>().sprite = blueSafeHouseSprite;
			redSafeHouse.GetComponent<SpriteRenderer>().sprite = yellowSafeHouseSprite;
			blueSafeHouse.GetComponent<SpriteRenderer>().sprite = greenSafeHouseSprite;

			greenTimer.GetComponent<Image>().sprite = redTimerSprite;
			redTimer.GetComponent<Image>().sprite = blueTimerSprite;
			blueTimer.GetComponent<Image>().sprite = yellowTimerSprite;
			yellowTimer.GetComponent<Image>().sprite = greenTimerSprite;

            for(int i=0; i < yellowPieces.Length ; i++)
            {
               yellowPieces[i].GetComponent<SpriteRenderer>().sprite = redMarker;
            }

   			for(int i=0; i < greenPieces.Length ; i++)
   			{

   				greenPieces[i].GetComponent<SpriteRenderer>().sprite = blueMarker;
   			}
   		  	
   			for(int i=0; i < redPieces.Length ; i++)
   			{
   				redPieces[i].GetComponent<SpriteRenderer>().sprite = yellowMarker;
   			}

   			for(int i=0; i < bluePieces.Length ; i++)
   			{
   				bluePieces[i].GetComponent<SpriteRenderer>().sprite = greenMarker;
   			}

   		}
   		else if(count == 4)
   		{
   		YellowBoardSpriteRenderer.sprite = greenSprite;
			GreenBoardSpriteRenderer.sprite = redSprite;
			RedBoardSpriteRenderer.sprite = blueSprite;
			BlueBoardSpriteRenderer.sprite = yellowSprite;

         GameManager.gm.firstPlayerColor = "green";
         GameManager.gm.secondPlayerColor = "red";
         GameManager.gm.thirdPlayerColor = "blue";
         GameManager.gm.fourthPlayerColor = "yellow";

			winingBoard.transform.localRotation = Quaternion.Euler(0f, 0f, -270f);

			YellowPathSpriteRenderer.sprite = greenPathSprite;
			GreenPathSpriteRenderer.sprite = redPathSprite;
			RedPathSpriteRenderer.sprite = bluePathSprite;
			BluePathSpriteRenderer.sprite = yellowPathSprite;

			yellowSafeHouse.GetComponent<SpriteRenderer>().sprite = greenSafeHouseSprite;
			greenSafeHouse.GetComponent<SpriteRenderer>().sprite = redSafeHouseSprite;
			redSafeHouse.GetComponent<SpriteRenderer>().sprite = blueSafeHouseSprite;
			blueSafeHouse.GetComponent<SpriteRenderer>().sprite = yellowSafeHouseSprite;

			greenTimer.GetComponent<Image>().sprite = greenTimerSprite;
			redTimer.GetComponent<Image>().sprite = redTimerSprite;
			blueTimer.GetComponent<Image>().sprite = blueTimerSprite;
			yellowTimer.GetComponent<Image>().sprite = yellowTimerSprite;


            for(int i=0; i < yellowPieces.Length ; i++)
            {
               yellowPieces[i].GetComponent<SpriteRenderer>().sprite = greenMarker;
            }

   			for(int i=0; i < greenPieces.Length ; i++)
   			{

   				greenPieces[i].GetComponent<SpriteRenderer>().sprite = redMarker;
   			}
   		  	
   			for(int i=0; i < redPieces.Length ; i++)
   			{
   				redPieces[i].GetComponent<SpriteRenderer>().sprite = blueMarker;
   			}

   			for(int i=0; i < bluePieces.Length ; i++)
   			{
   				bluePieces[i].GetComponent<SpriteRenderer>().sprite = yellowMarker;
   			}

   		}
   	}

}
