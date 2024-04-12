using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;
using TMPro;

public class GameSettings : MonoBehaviour
{
    [SerializeField] GameObject SoundButton;

    [Header("Sound On and Off Sprite Images")]
    public Sprite OffSprite;
 	public Sprite OnSprite;

 	Button soundOnandOffButton;

 	void Start()
    {
    	soundOnandOffButton = SoundButton.GetComponent<Button>();
    	soundOnandOffButton.onClick.AddListener(soundController);

    	hideExitButton();

    	if(PlayerPrefs.GetInt("soundStatus") == 1)
    	soundOnandOffButton.image.sprite = OnSprite;
    	else
    	soundOnandOffButton.image.sprite = OffSprite;
    }

	void soundController()
	{
		if (soundOnandOffButton.image.sprite == OnSprite)
		{
		   SoundManager.PlaySound("click");
           soundOnandOffButton.image.sprite = OffSprite;
           PlayerPrefs.SetInt("soundStatus",0);
		}
	    else 
	    {
	       soundOnandOffButton.image.sprite = OnSprite;
	       PlayerPrefs.SetInt("soundStatus",1);
	       SoundManager.PlaySound("click");
	    }
	}


	void objectSetActivie(GameObject gameObject, bool status)
	{
		if(gameObject)
		{
			gameObject.SetActive(status);
		}
	}

	public void showExitButton()
	{
		objectSetActivie(SoundButton,true);
	}

	public void hideExitButton()
	{
		objectSetActivie(SoundButton,false);
	}

	public void setSoundImage()
    {
		if (PlayerPrefs.GetInt("soundStatus") == 1)
			soundOnandOffButton.image.sprite = OnSprite;
		else
			soundOnandOffButton.image.sprite = OffSprite;
	}
}
