using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable()]
public class Results
{
public Results(int player_id, string name, int user_id) {
	this.player_id = player_id;
    this.name = name;
    this.user_id = user_id;
}

 public string name { get; set; }
 public int player_id { get; set; }
 public int user_id { get; set; }
}
