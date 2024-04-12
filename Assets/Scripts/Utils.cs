using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Globalization;

public class Utils
{
   public static double getTime(long timeInt)
    {
        ulong u = (ulong)timeInt;
        double t = u;
        return t / 1000.0d;
    }


    public static double getRemainingTime(string time)
    {
        double seconds = 0.0;
        try
        {
            System.DateTime startTime = System.DateTime.ParseExact(time, "MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
            System.TimeSpan ts = System.DateTime.UtcNow - startTime;
            seconds = Mathf.RoundToInt((float)ts.TotalSeconds);

        }
        catch (Exception e)
        {
            try
            {
                System.DateTime startTime = System.DateTime.ParseExact(time, "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                System.TimeSpan ts = System.DateTime.UtcNow - startTime;
                seconds = Mathf.RoundToInt((float)ts.TotalSeconds);

            }
            catch (Exception error)
            {
                Debug.Log(error.Message);
            }

        }

        return seconds;
    }


    public static double getRemainigSec(string time1, string time2)
    {
         double seconds = 0.0;
        System.DateTime dateTime1 = DateTime.MinValue;
        System.DateTime dateTime2 = DateTime.MinValue;

        //   time1 = TimeZoneInfo.ConvertTime(DateTime.Parse(time1), TimeZoneInfo.FindSystemTimeZoneById("India Standard Time")).ToString();
        //   time2 = TimeZoneInfo.ConvertTime(DateTime.Parse(time2), TimeZoneInfo.FindSystemTimeZoneById("India Standard Time")).ToString();

        try
        {
            dateTime1 = System.DateTime.ParseExact(time1, "MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture);

        }
        catch(Exception e)
        {
            try
            {
                dateTime1 = System.DateTime.ParseExact(time1, "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture);
            }
            catch (Exception error)
            {
                Debug.Log("Time error : "+e.Message+ " : "+ time1);
            }
            
        }

        try
        {
            dateTime2 = System.DateTime.ParseExact(time2, "MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
        }
        catch (Exception e)
        {
            try
            {
                dateTime2 = System.DateTime.ParseExact(time2, "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture);
            }
            catch (Exception error)
            {
                Debug.Log("Time error : " + e.Message + " : " + time2);
            }

        }

        seconds = (dateTime1 - dateTime2).TotalSeconds;

        return seconds;
    }

    public static System.DateTime getParseTime(string time)
    {
        System.DateTime dateTime = DateTime.MinValue;

        try
        {
            dateTime = System.DateTime.ParseExact(time, "MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
        }
        catch (Exception e)
        {
            try
            {
                dateTime = System.DateTime.ParseExact(time, "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture);
            }
            catch (Exception error)
            {
                Debug.Log(e.Message + " : " + time);
            }

        }

        return dateTime;

    }
}
