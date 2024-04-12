using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Credentials 
{
   private static Credentials instance;

   public static string channel;
   public static string id;
   public static string old_id;
   public static string email;
   public static string action;
   public static string contestId;
   public static string userId;
	public static string iosSklashToken;
	public static bool isSplashScreen;
	public static bool isBranchIOhasData;
	public static bool isAdminContest;
	public static string contestName;
	public static string userName;
	public static string contestSize;

	public static int adminContestStartTime;

	private Credentials() 
   {
    
   }

    public static Credentials Instance
	 {
	    get 
	    {
	       if (instance == null)
	       {
	          instance = new Credentials();
	       }
	       return instance;
	    }
	 }



}
