using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
// Android Merge Ludo and Ludo pro - Fast Ludo
public class SameMarker
{
  private static SameMarker instance;
 public List<Safe> list;
 public List<Player> playerList;
 public List<Player> quitPlayerList;
 public List<Results> resultsList;
 List<MarkerSize> movebaleObjectList;
 List<MarkerSize> onlineMovebaleObjectList;
 public List<JoinPlayerInfo> joinPlayer;

 //better using a List<string>

 private SameMarker() {
    //instanciate MyArray here
    playerList = new List<Player>();
    quitPlayerList = new List<Player>();
    resultsList = new List<Results>();
    movebaleObjectList = new List<MarkerSize>();
    onlineMovebaleObjectList = new List<MarkerSize>();
    joinPlayer = new List<JoinPlayerInfo>();

    list = new List<Safe>();
      list.Add(new Safe(1,1,0,100,0));
      list.Add(new Safe(1,2,0,101,1));
      list.Add(new Safe(1,3,0,102,2));
      list.Add(new Safe(1,4,0,103,3));

      list.Add(new Safe(3,1,0,104,4));
      list.Add(new Safe(3,2,0,105,5));
      list.Add(new Safe(3,3,0,106,6));
      list.Add(new Safe(3,4,0,107,7));

      list.Add(new Safe(2,1,0,108,8));
      list.Add(new Safe(2,2,0,109,9));
      list.Add(new Safe(2,3,0,110,10));
      list.Add(new Safe(2,4,0,111,11));

      list.Add(new Safe(4,1,0,112,12));
      list.Add(new Safe(4,2,0,113,13));
      list.Add(new Safe(4,3,0,114,14));
      list.Add(new Safe(4,4,0,115,15));
 }

 public static SameMarker Instance
 {
    get 
    {
       if (instance == null)
       {
          instance = new SameMarker();
       }
       return instance;
    }
 }

 public static void ClearInstance()
 {
    if(instance != null)
    instance = null;
 }

 // retrieve array fr*om anywhere
  public List<Safe> getArray() {
      return this.list;
  }

  public void updateList(List<Safe> arrayList)
  {
      list.Clear();

      for(int i=0; i< arrayList.Count; i++)
      {
          list.Add(arrayList[i]);
      }

  }

    // unlock the game if online 

    public int getPlayerTotalStepsCount(int index) {
        int totalCount = 0;
        for (int i = 0; i < list.Count; i++)
        {
            if (index == list[i].player_id) {
                totalCount += list[i].count_steps;
                
            }
        }
        //totalCount -= 4;
        return totalCount;
         Debug.Log(">>>>>>>>>>>>>>>>>>>>>>>>>> SameMarker -Player step count online- is called");
    }


    public void updateCountStepsForOnlineGame() {
        if (GameManager.gm.isOnlineGame) {
            for (int i = 0; i < list.Count; i++) {
                //list[i].count_steps = 1;
            }
        }
        // remove code for Ofline game
        for (int i = 0; i < list.Count; i++)
        {
            //list[i].count_steps = 1;
            list[i].isFastLudo = true;
        }
    }


   public void setNewValue(int countStep, int position, int figure_id, int player){
  
      if(player == 1) {
          int index = figure_id -1;
          list[index].count_steps = countStep;
          list[index].position = position;

      }else if(player == 3) {
          int index = 4 + (figure_id-1);
          list[index].count_steps = countStep;
          list[index].position = position;
      }else if(player == 2) {
          int index = 8 + (figure_id-1);
          list[index].count_steps = countStep;
          list[index].position =position;
      }else if(player == 4) {
          int index = 12 + (figure_id-1);
          list[index].count_steps = countStep;
          list[index].position = position;
      }

  }

  public int getCountStep(int pos)
  {
      return list[pos].count_steps;
  }

  public int getMarkerPosition(int pos)
  {
      return list[pos].position;
  }

  public List<Safe> getNewPositionOpponentMarkerList(int position, int player)
  {
     List<Safe> newList = new List<Safe>();

     for(int i=0; i< list.Count; i++)
     {
        if(list[i].position == position && list[i].player_id != player)
        {
          newList.Add(list[i]);
        }
     }

     return newList;

  }

  public bool isMarkerAvailableToMove(int player, int dice)
  {
      int count = 0;

      if(player == 1) {

      for(int i=0; i< 4; i++)
     {
        int step = list[i].count_steps + dice;
        // Fast Ludo
        if((list[i].count_steps > 0 || list[i].isFastLudo) && step < 58)
        count++;

     }

      }else if(player == 3) {

      for(int i=4; i< 8; i++)
     {
        int step = list[i].count_steps + dice;
        if((list[i].count_steps > 0 || list[i].isFastLudo) && step < 58)
        count++;

     }

      }else if(player == 2) {
      for(int i=8; i< 12; i++)
     {
        int step = list[i].count_steps + dice;
       if((list[i].count_steps > 0 || list[i].isFastLudo) && step < 58)
        count++;

     }
      }else if(player == 4) {
      for(int i = 12; i< 16; i++)
     {
        int step = list[i].count_steps + dice;
        if((list[i].count_steps > 0 || list[i].isFastLudo) && step < 58)
        count++;

     }
  }

  return count>0;
}

public List<Safe> getListOfPlayerPieces(int position)
{
    List<Safe> newList = new List<Safe>();

    for(int i=0; i< list.Count ; i++)
    {
        newList.Add(list[i]);
    }

    return newList;
}

public List<Safe> isSingleAvailableToMove(int player, int dice)
  {
      List<Safe> newList = new List<Safe>();

      if(player == 1) {

      for(int i=0; i< 4; i++)
     {
        int step = list[i].count_steps + dice;
        if((list[i].count_steps > 0 || list[i].isFastLudo) && step < 58)
        newList.Add(list[i]);

     }

      }else if(player == 3) {

      for(int i=4; i< 8; i++)
     {
        int step = list[i].count_steps + dice;
        if((list[i].count_steps > 0 || list[i].isFastLudo) && step < 58)
        newList.Add(list[i]);

     }

      }else if(player == 2) {
      for(int i=8; i< 12; i++)
     {
        int step = list[i].count_steps + dice;
        if((list[i].count_steps > 0 || list[i].isFastLudo) && step < 58)
        newList.Add(list[i]);

     }
      }else if(player == 4) {
      for(int i = 12; i< 16; i++)
     {
        int step = list[i].count_steps + dice;
        if((list[i].count_steps > 0 || list[i].isFastLudo) && step < 58)
        newList.Add(list[i]);

     }
  }

  return newList;
}

public List<Safe> getHomePlayer(int player)
{
    List<Safe> newList = new List<Safe>();

    if(player == 1)
    {
        for(int i = 0; i < 4 ; i++)
        {
            if(list[i].position > 99 && list[i].position < 104)
            {
                newList.Add(list[i]);
            }
        }
    }else if(player == 2)
    {
      for(int i = 8; i < 12 ; i++)
        {
            if(list[i].position > 107 && list[i].position < 112)
            {
                newList.Add(list[i]);
            }
        }

    }else if(player == 3)
    {
        for(int i = 4; i < 8 ; i++)
        {
            if(list[i].position > 103 && list[i].position < 108)
            {
                newList.Add(list[i]);
            }
        }

    }else if(player == 4)
    {
        for(int i = 12; i < 16 ; i++)
        {
            if(list[i].position > 111 && list[i].position < 116)
            {
                newList.Add(list[i]);
            }
        }
    }

    return newList;
}

public bool checkIsItWinner(int player)
{
    List<Safe> newList = new List<Safe>();

    if(player == 1)
    {
        for(int i = 0; i < 4 ; i++)
        {
            if(list[i].count_steps == 57)
            {
                newList.Add(list[i]);
            }
        }
    }else if(player == 2)
    {
      for(int i = 8; i < 12 ; i++)
        {
            if(list[i].count_steps == 57)
            {
                newList.Add(list[i]);
            }
        }

    }if(player == 3)
    {
        for(int i = 4; i < 8 ; i++)
        {
            if(list[i].count_steps == 57)
            {
                newList.Add(list[i]);
            }
        }

    }if(player == 4)
    {
        for(int i = 12; i < 16 ; i++)
        {
            if(list[i].count_steps == 57)
            {
                newList.Add(list[i]);
            }
        }
    }

    return newList.Count == 4;
}

public int getMarkerIndex(int player_id, int figure_id)
{
    int index = 0;

    for(int i = 0; i < 16 ; i++)
    {
        if(list[i].player_id == player_id && list[i].figure_id == figure_id)
        {
            index = list[i].index;
        }
    }

    return index;
}



 // retrieve array fr*om anywhere
  public List<Player> getPlayerArray() {
      return this.playerList;
  }

  public void addPlayer(Player player)
  {
      playerList.Add(player);
  }

  public void clearPlayerList()
  {
      playerList.Clear();
  }

  public string getPlayerName(int id)
  {
      string playerName = "";
      for(int i = 0 ; i < playerList.Count ; i++)
      {
          if(playerList[i].player_id == id)
          playerName = playerList[i].player_name;
      }

      return playerName;
  }

    public string getPlayerUserName(int id)
    {
        string playerName = "";
        for (int i = 0; i < playerList.Count; i++)
        {
            if (playerList[i].player_id == id)
                playerName = playerList[i].username;
        }

        return playerName;
    }

    public int getPlayerUserId(int id)
    {
        int playerUserId = 0;
        for (int i = 0; i < playerList.Count; i++)
        {
            if (playerList[i].player_id == id)
                playerUserId = playerList[i].user_id;
        }

        return playerUserId;
    }

    public bool isPlayerQuit(int Player)
  {
     return playerList[Player - 1].isQuit;
  }

  public int getQuitPlayerCount()
  {
      int count = 0;

      for(int i = 0 ; i < playerList.Count ; i++)
      {
          if(playerList[i].isQuit)
          count++;
      }

      return count;
  }



  public List<Player> getRemainingPlayerArray()
  {
      List<Player> newList = new List<Player>();

      for(int i = 0; i < playerList.Count ; i++)
      {
          if(!playerList[i].isQuit && !playerList[i].isWinner)
          newList.Add(playerList[i]);
      }

      return newList;
  }

  public List<Player> getQuitPlayerArray()
  {
      List<Player> newList = new List<Player>();

      for(int i = 0; i < playerList.Count ; i++)
      {
          if(playerList[i].isQuit)
          newList.Add(playerList[i]);
      }

      return newList;
  }

  // retrieve results array fr*om anywhere
  public List<Results> getResultArray() {
      return this.resultsList;
  }

  public void addResult(Results results)
  {
        bool res = resultsList.Any(c => c.player_id == results.player_id);

        if(!res)
        resultsList.Add(results);
  }

  public void clearResultList()
  {
      resultsList.Clear();
  }

  public void addMovableList(MarkerSize markerSize)
  {
      this.movebaleObjectList.Add(markerSize);
  }

  public void clearMovableList()
  {
      movebaleObjectList.Clear();
  }

  public List<MarkerSize> getMovableArray() {
      return this.movebaleObjectList;
  }


    public void addOnlineMovableList(MarkerSize markerSize)
    {
        this.onlineMovebaleObjectList.Add(markerSize);
    }

    public void clearOnlineMovableList()
    {
        this.onlineMovebaleObjectList.Clear();
    }

    public List<MarkerSize> getOnlineMovableArray()
    {
        return this.onlineMovebaleObjectList;
    }


    // retrieve results array fr*om anywhere
    public List<JoinPlayerInfo> getJointPlayerArray() {
      return this.joinPlayer;
  }

  public void addJoinPlayerList(List<JoinPlayerInfo> arrayList)
  {
      joinPlayer.Clear();

      for(int i=0; i< arrayList.Count; i++)
      {
          joinPlayer.Add(arrayList[i]);
      }

  }


    public void addQuitPlayer(Player quitPlayer)
    {
        bool res = quitPlayerList.Any(c => c.player_id == quitPlayer.player_id);

        if(!res)
        this.quitPlayerList.Add(quitPlayer);
    }

    public void clearQuitPlayerList()
    {
        quitPlayerList.Clear();
    }

    public List<Player> getQuitPlayerList()
    {
        return this.quitPlayerList;
    }


}
