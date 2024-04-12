using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable()]
public class LoginUser
{
    public LoginUser(int id, string email, string first_name, string last_name,
        string username, string token, string created_at, string updated_at, bool isLogin, bool isFromLoginWithSklash)
    {
        this.id = id;
        this.email = email;
        this.first_name = first_name;
        this.last_name = last_name;
        this.username = username;
        this.token = token;
        this.created_at = created_at;
        this.updated_at = updated_at;
        this.isLogin = isLogin;
        this.isFromLoginWithSklash = isFromLoginWithSklash;
    }

    public int id { get; set; }
    public string email { get; set; }
    public string first_name { get; set; }
    public string last_name { get; set; }
    public string username { get; set; }
    public string token { get; set; }
    public string created_at { get; set; }
    public string updated_at { get; set; }
    public bool isLogin { get; set; }
    public bool isFromLoginWithSklash { get; set; }
}