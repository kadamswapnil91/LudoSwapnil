using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class SplashScreen : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Image SplashBackgroundImage;
    [SerializeField] Sprite lightBackground, darkBackground;
    //To merge 
    [SerializeField] Sprite darkLudoSplash, lightLudoSplash;

    public string packageName;


    void Start()
    {
        defineSplash();
        StartCoroutine(closeSplash());
    }

   void defineSplash()
    {
        packageName = Application.identifier;
        if (PlayerPrefs.GetInt("themeStatus") == 0)
        {
            if (packageName == "com.sklash.ludo1")
            {
                 SplashBackgroundImage.sprite = darkLudoSplash;
            }
            else 
            {
                SplashBackgroundImage.sprite = darkBackground;
            }
            
        }
        else
        {
           if (packageName == "com.sklash.ludo1")
           {
                 SplashBackgroundImage.sprite = lightLudoSplash;
           }
           else
           {
                 SplashBackgroundImage.sprite = lightBackground;
           }
        }   
    }

   
    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator closeSplash()
    {
        yield return new WaitForSeconds(4f);
        SceneManager.LoadScene(1);
    }

    
}

