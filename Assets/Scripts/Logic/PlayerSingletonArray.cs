using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerSingletonArray
{
  private static PlayerSingletonArray instance;
 public List<Player> list;
 //better using a List<string>

 private PlayerSingletonArray() {
    
 }

 public static PlayerSingletonArray Instance
 {
    get 
    {
       if (instance == null)
       {
          instance = new PlayerSingletonArray();
       }
       return instance;
    }
 }

 public static void ClearInstance()
 {
    instance = null;
 }

 // retrieve array fr*om anywhere
  public List<Player> getArray() {
      return this.list;
  }

  public void addPlayer(Player player)
  {
      list.Add(player);
  }

  public void clearList()
  {
      list.Clear();
  }
       
}
