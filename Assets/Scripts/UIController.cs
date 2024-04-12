using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIController : MonoBehaviour
{
	UnityEngine.UI.Text text;
	UnityEngine.UI.Text playerName1;
	UnityEngine.UI.Text playerName2;
	JavaBridge javaBridge;
	public static int steps = 0;

	void Start ()
	{
		// Create a new JavaBridge and register to callback event
		javaBridge = new JavaBridge();
		javaBridge.GotJavaCallback += onJavaCallback;
		javaBridge.GotJavaCallback1 += onJavaCallback1;

		// Setup UI
		text = GetComponentInChildren<UnityEngine.UI.Text>();
		playerName1 = GameObject.Find("Canvas/PlayerName1").GetComponent<UnityEngine.UI.Text>();
		playerName2 = GameObject.Find("Canvas/PlayerName2").GetComponent<UnityEngine.UI.Text>();
		var button = GetComponentInChildren<UnityEngine.UI.Button>();
		button.onClick.AddListener(onUiButtonClicked);
		//onUiButtonClicked()
	}
	
	void onUiButtonClicked()
	{
		//javaBridge.CallJavaMethod();
		javaBridge.showPlayerName();
	}

	void onJavaCallback(System.Object[] result)
	{
		print("callback Size : "+result.Length);

		List<Safe> safe = new List<Safe>();

		//for(int i=0; i< result.Length; i++)
		//{
		//	safe.Add(result[i]);
		//}

		//SameMarker.Instance.updateList(safe);

		for(int i=0; i< result.Length; i++)
		{
			print("Print Object");
			print(result[i]);
		}
		

		print("SameMarker Size : "+SameMarker.Instance.getArray().Count);
	}

	void onJavaCallback1(string[] result)
	{
		playerName1.text = result[0];
		playerName2.text = result[1];
		//Debug.Log("Player Name is " + result);
	}

}
