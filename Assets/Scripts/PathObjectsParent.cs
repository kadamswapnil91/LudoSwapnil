using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathObjectsParent : MonoBehaviour
{
   public PathPoint[] commonPathPoints;
   public PathPoint[] greenPathPoints;
   public PathPoint[] redPathPoints;
   public PathPoint[] homePathPoints;
   public PathPoint[] bluePathPoints;
   public PathPoint[] yellowPathPoints;
   public PlayerPiece[] Pieces;
   public GameObject[] diceHolder;
   public GameObject[] greenPlayerPieces;
   public GameObject[] redPlayerPieces;
   public GameObject[] bluePlayerPieces;
   public GameObject[] yellowPlayerPieces;

   public YellowPlayerPieces[] yellowPieces;
   public GreenPlayerPieces[] greenPieces;
   public RedPlayerPieces[] redPieces;
   public BluePlayerPieces[] bluePieces;

    [Header("Scales And Position Difference")]
   public float[] scales;
   public float[] positionDifference;
}
