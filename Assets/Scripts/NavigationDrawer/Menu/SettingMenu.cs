using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NavigationDrawer.UI;


public class SettingMenu : MonoBehaviour
{

	[SerializeField] public GameObject SettingPanel;
   	[SerializeField] public Button CloseButton;
    MenuHandler menuHandler;

    void Start()
    {
        CloseButton.onClick.AddListener(() => objectSetActivie(SettingPanel, false));
    }

    private void Awake()
    {
        menuHandler = FindObjectOfType<MenuHandler>();
    }



    void objectSetActivie(GameObject gameObject, bool status)
	{
		if(gameObject)
		{
			gameObject.SetActive(status);
		}
        menuHandler.resetSelectedButton();
	}

    public GameObject getSettingPanelObject()
    {
        return SettingPanel;
    }

}
