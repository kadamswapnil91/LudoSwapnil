using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[System.Serializable]
public class JoinPlayerInfo
{
	public JoinPlayerInfo(int id, int match_id, int user_id, string email, string first_name, string last_name, string username) {
        this.id = id;
        this.match_id = match_id;
        this.user_id = user_id;
        this.email = email;
        this.first_name = first_name;
        this.last_name = last_name;
        this.username = username;
    }

   public int id { get; set; }
   public int match_id { get; set; }
   public int user_id { get; set; }
   public string email { get; set; }
   public string first_name { get; set; }
   public string last_name { get; set; }
   public string username { get; set; }

}
