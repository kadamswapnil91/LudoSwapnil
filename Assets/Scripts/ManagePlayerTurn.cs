using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ManagePlayerTurn
{
  private static ManagePlayerTurn instance;
 List<GameObject> playerObjectList;
 List<GameObject> playerSpriteObjectList;
 //better using a List<string>

 private ManagePlayerTurn() {
    //instanciate MyArray here
    playerObjectList = new List<GameObject>();

     	playerObjectList.Add(GameObject.Find("LudoHomes/GreenHome/GreenPlayerPieces1"));
        playerObjectList.Add(GameObject.Find("LudoHomes/GreenHome/GreenPlayerPieces2"));
        playerObjectList.Add(GameObject.Find("LudoHomes/GreenHome/GreenPlayerPieces3"));
        playerObjectList.Add(GameObject.Find("LudoHomes/GreenHome/GreenPlayerPieces4"));

        //Blue Player Object
        playerObjectList.Add(GameObject.Find("LudoHomes/BlueHome/BluePlayerPieces1"));
        playerObjectList.Add(GameObject.Find("LudoHomes/BlueHome/BluePlayerPieces2"));
        playerObjectList.Add(GameObject.Find("LudoHomes/BlueHome/BluePlayerPieces3"));
        playerObjectList.Add(GameObject.Find("LudoHomes/BlueHome/BluePlayerPieces4"));

        //Red Player Object
        playerObjectList.Add(GameObject.Find("LudoHomes/RedHome/RedPlayerPieces1"));
        playerObjectList.Add(GameObject.Find("LudoHomes/RedHome/RedPlayerPieces2"));
        playerObjectList.Add(GameObject.Find("LudoHomes/RedHome/RedPlayerPieces3"));
        playerObjectList.Add(GameObject.Find("LudoHomes/RedHome/RedPlayerPieces4"));

        //Yellow Player Object
        playerObjectList.Add(GameObject.Find("LudoHomes/YellowHome/YellowPlayerPieces1"));
        playerObjectList.Add(GameObject.Find("LudoHomes/YellowHome/YellowPlayerPieces2"));
        playerObjectList.Add(GameObject.Find("LudoHomes/YellowHome/YellowPlayerPieces3"));
        playerObjectList.Add(GameObject.Find("LudoHomes/YellowHome/YellowPlayerPieces4"));


    playerSpriteObjectList = new List<GameObject>();
    	playerSpriteObjectList.Add(GameObject.Find("LudoHomes/GreenHome/GreenPlayerPieces1/PlayerSprite"));
        playerSpriteObjectList.Add(GameObject.Find("LudoHomes/GreenHome/GreenPlayerPieces2/PlayerSprite"));
        playerSpriteObjectList.Add(GameObject.Find("LudoHomes/GreenHome/GreenPlayerPieces3/PlayerSprite"));
        playerSpriteObjectList.Add(GameObject.Find("LudoHomes/GreenHome/GreenPlayerPieces4/PlayerSprite"));

        //Blue Player Sprite Object 
        playerSpriteObjectList.Add(GameObject.Find("LudoHomes/BlueHome/BluePlayerPieces1/PlayerSprite"));
        playerSpriteObjectList.Add(GameObject.Find("LudoHomes/BlueHome/BluePlayerPieces2/PlayerSprite"));
        playerSpriteObjectList.Add(GameObject.Find("LudoHomes/BlueHome/BluePlayerPieces3/PlayerSprite"));
        playerSpriteObjectList.Add(GameObject.Find("LudoHomes/BlueHome/BluePlayerPieces4/PlayerSprite"));

        //Red Player Sprite Object 
        playerSpriteObjectList.Add(GameObject.Find("LudoHomes/RedHome/RedPlayerPieces1/PlayerSprite"));
        playerSpriteObjectList.Add(GameObject.Find("LudoHomes/RedHome/RedPlayerPieces2/PlayerSprite"));
        playerSpriteObjectList.Add(GameObject.Find("LudoHomes/RedHome/RedPlayerPieces3/PlayerSprite"));
        playerSpriteObjectList.Add(GameObject.Find("LudoHomes/RedHome/RedPlayerPieces4/PlayerSprite"));

        //Yellow Player Sprite Object 
        playerSpriteObjectList.Add(GameObject.Find("LudoHomes/YellowHome/YellowPlayerPieces1/PlayerSprite"));
        playerSpriteObjectList.Add(GameObject.Find("LudoHomes/YellowHome/YellowPlayerPieces2/PlayerSprite"));
        playerSpriteObjectList.Add(GameObject.Find("LudoHomes/YellowHome/YellowPlayerPieces3/PlayerSprite"));
        playerSpriteObjectList.Add(GameObject.Find("LudoHomes/YellowHome/YellowPlayerPieces4/PlayerSprite"));
 }

 public static ManagePlayerTurn Instance
 {
    get 
    {
       if (instance == null)
       {
          instance = new ManagePlayerTurn();
       }
       return instance;
    }
 }

 public static void ClearInstance()
 {
    instance = null;
 }

 // retrieve array fr*om anywhere
  public List<GameObject> getPlayerObjectList() {
      return this.playerObjectList;
  }

  public List<GameObject> getPlayerSpriteObjectList() {
      return this.playerSpriteObjectList;
  }
       
}
