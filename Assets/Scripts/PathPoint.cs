using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathPoint : MonoBehaviour
{
	   public List<PlayerPiece> PlayerPieces = new List<PlayerPiece>();
	   public PathObjectsParent pathParent;
	   public PathPoint previousPathPoint;
 	   public PathPoint currentPathPoint;

	   private void Start()
       {
      		pathParent = FindObjectOfType<PathObjectsParent>();
       }


	   public void AddPlayerPiece(PlayerPiece playerPiece)
	   {
	   		if(!PlayerPieces.Contains(playerPiece))
	   		{
	   			PlayerPieces.Add(playerPiece);
	   			RescaleAndRepositionAllPlayerPieces();
	   		}
	   }

	   public void RemovePlayerPiece(PlayerPiece playerPiece)
	   {
	   		if(PlayerPieces.Contains(playerPiece))
	   		{
	   			PlayerPieces.Remove(playerPiece);

	   			RescaleAndRepositionAllPlayerPieces();
	   		}
	   		
	   }

	   public void RescaleAndRepositionAllPlayerPieces()
	   {
	   		if(PlayerPieces.Count > 0)
	   		{
	   			for(int i=0; i < PlayerPieces.Count ; i++)
	   			{
	   				PlayerPieces[i].transform.localScale = new Vector3(pathParent.scales[PlayerPieces.Count-1],pathParent.scales[PlayerPieces.Count-1],1f);
	   				/*if(i%2 == 0)
	   					PlayerPieces[i].transform.position = new Vector3(transform.position.x+(i*pathParent.positionDifference[PlayerPieces.Count-1]),transform.position.y+(i*pathParent.positionDifference[PlayerPieces.Count-1]),0f);
	   				else
	   					PlayerPieces[i].transform.position = new Vector3(transform.position.x - (i*pathParent.positionDifference[PlayerPieces.Count-1]),transform.position.y- (i*pathParent.positionDifference[PlayerPieces.Count-1]),0f);*/
	   			}

	   			switch(PlayerPieces.Count - 1)
	   			{
	   				case 0: PlayerPieces[0].transform.position = new Vector3(transform.position.x,transform.position.y,0f);
	   						break;

	   				case 1:
							PlayerPieces[0].transform.position = new Vector3(transform.position.x + (0.1f), transform.position.y, 0f);
							PlayerPieces[1].transform.position = new Vector3(transform.position.x - (0.1f), transform.position.y, 0f);
							break;

                case 2: PlayerPieces[0].transform.position = new Vector3(transform.position.x+(0.1f),transform.position.y-(0.1f),0f);
	   						PlayerPieces[1].transform.position = new Vector3(transform.position.x-(0.1f),transform.position.y+(0.1f),0f);
	   						PlayerPieces[2].transform.position = new Vector3(transform.position.x+(0.1f),transform.position.y+(0.1f),0f);
	   						break;

	   				case 3: PlayerPieces[0].transform.position = new Vector3(transform.position.x+(0.1f),transform.position.y-(0.1f),0f);
	   						PlayerPieces[1].transform.position = new Vector3(transform.position.x-(0.1f),transform.position.y+(0.1f),0f);
	   						PlayerPieces[2].transform.position = new Vector3(transform.position.x+(0.1f),transform.position.y+(0.1f),0f);
	   						PlayerPieces[3].transform.position = new Vector3(transform.position.x-(0.1f),transform.position.y-(0.1f),0f);
	   						break;	

	   				/*case 4:	PlayerPieces[0].transform.position = new Vector3(transform.position.x+(0.1f),transform.position.y-(0.1f),0f);
	   						PlayerPieces[1].transform.position = new Vector3(transform.position.x-(0.1f),transform.position.y-(0.1f),0f);
	   						PlayerPieces[2].transform.position = new Vector3(transform.position.x-(0.1f),transform.position.y+(0.0f),0f);
	   						PlayerPieces[3].transform.position = new Vector3(transform.position.x+(0.1f),transform.position.y+(0.0f),0f);
	   						PlayerPieces[4].transform.position = new Vector3(transform.position.x-(0.1f),transform.position.y-(0.1f),0f);
	   						PlayerPieces[5].transform.position = new Vector3(transform.position.x-(0.1f),transform.position.y-(0.1f),0f);
	   						break;

	   				case 5:	PlayerPieces[0].transform.position = new Vector3(transform.position.x+(0.1f),transform.position.y-(0.1f),0f);
	   						PlayerPieces[1].transform.position = new Vector3(transform.position.x-(0.1f),transform.position.y-(0.1f),0f);
	   						PlayerPieces[2].transform.position = new Vector3(transform.position.x-(0.1f),transform.position.y+(0.0f),0f);
	   						PlayerPieces[3].transform.position = new Vector3(transform.position.x+(0.1f),transform.position.y+(0.0f),0f);
	   						PlayerPieces[4].transform.position = new Vector3(transform.position.x-(0.1f),transform.position.y-(0.1f),0f);
	   						PlayerPieces[5].transform.position = new Vector3(transform.position.x-(0.1f),transform.position.y-(0.1f),0f);
	   						break;*/

	   			}

	   			if(PlayerPieces.Count>4)
	   			{
	   				int plsCount = PlayerPieces.Count;
	   				bool isOdd = (plsCount % 2) == 0 ? false : true;
	   				int extent = plsCount/2;
	   				int counter = 0;

	   				if(isOdd)
	   				{
	   					for(int i = -extent; i <= extent; i++)
	   					{
	   							PlayerPieces[counter].transform.position = new Vector3(transform.position.x + (i * pathParent.positionDifference[plsCount - 1]),transform.position.y,0f);
	   							counter++;
	   					}
	   				}else
	   					{
	   					for(int i = -extent; i < extent; i++)
	   					{
	   						PlayerPieces[counter].transform.position = new Vector3(transform.position.x + (i * pathParent.positionDifference[plsCount - 1]),transform.position.y,0f);
	   						counter++;
	   					}
	   				}
	   			}

	   		}
	   }
}
