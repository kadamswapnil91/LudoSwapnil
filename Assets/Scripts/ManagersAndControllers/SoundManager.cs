using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
	public static AudioClip diceSoundClip,moveSoundClip,winSoundClip,killSoundClip,reachedGoalSoundClip,clickSoundClip,popupSoundClip,lessTimeSoundClip;
	static AudioSource audioSrc;
    // Start is called before the first frame update
    void Start()
    {
        clickSoundClip = Resources.Load<AudioClip>("click");
        diceSoundClip = Resources.Load<AudioClip> ("diceSound");
        moveSoundClip = Resources.Load<AudioClip> ("move");
        winSoundClip = Resources.Load<AudioClip> ("win");
        killSoundClip = Resources.Load<AudioClip> ("kill");
        reachedGoalSoundClip = Resources.Load<AudioClip> ("reachedGoal");
        popupSoundClip = Resources.Load<AudioClip> ("popup");
        lessTimeSoundClip = Resources.Load<AudioClip> ("lessTime1");
        audioSrc = GetComponent<AudioSource> ();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void PlaySound(string clip)
    {
        try { 
        if(PlayerPrefs.GetInt("soundStatus") == null || PlayerPrefs.GetInt("soundStatus") == 1)
        {
        	switch (clip)
        	{
        		case "rollDice" :
        			audioSrc.PlayOneShot(diceSoundClip);
        			break;

                case "move" :
                    audioSrc.PlayOneShot(moveSoundClip);
                    break;

                case "win" :
                    audioSrc.PlayOneShot(winSoundClip);
                    break;

                case "kill" :
                    audioSrc.PlayOneShot(killSoundClip);
                    break;
                    
                case "reachedGoal" :
                    audioSrc.PlayOneShot(reachedGoalSoundClip);
                    break;

                case "click" :
                    audioSrc.PlayOneShot(clickSoundClip);
                    break;

                case "popup" :
                    audioSrc.PlayOneShot(popupSoundClip);
                    break;

                case "lessTime" :
                    audioSrc.PlayOneShot(lessTimeSoundClip);
                    break;
        	}
        }

        }
        catch(Exception e)
        {
            Debug.Log(e.Message);
        }
    }

}
