using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class JavaBridge : AndroidJavaProxy
{
	public Action<System.Object[]> GotJavaCallback;

	public Action<int[]> GotJavaCallbackMarkerList;

	public Action<int> GotJavaCallbackGetUserTurn;

	public Action<int> GotJavaCallbackGetUserCountStep;

	public Action<string[]> GotJavaCallback1;

	public Action<bool> GotJavaCallbackGetUserStatus;

	AndroidJavaObject javaObject;

	
	public JavaBridge() : base("ludogame.definelabs.com.ludogame.JavaCallback")
	{
		// We create an instance of the JavaClass in the constructor
		// and pass the reference of this class to the JavaClass
		javaObject = new AndroidJavaObject("ludogame.definelabs.com.ludogame.JavaClass",this);		
	}
	
	
	// Call the method in the plugin to invoke the callback
	public void CallJavaMethod()
	{
		javaObject.Call("JavaMethodName");
		Debug.Log("Successfully List Call: ");
	}

	// This method will be invoked from the plugin
	public void OnJavaCallbackGetList(System.Object[] toArray)
	{
		// Pass the result to the C# event that we register to in the UI class
		Debug.Log("callback gave List: "+ toArray.Length);
		if (GotJavaCallback != null) 
		GotJavaCallback(toArray);
	}

	// Call the method in the plugin to invoke the callback
	public void showPlayerName()
	{
		javaObject.Call("ShowPlayerName");
	}

	// This method will be invoked from the plugin
	public void OnJavaCallback1(string[] index)
	{
		// Pass the result to the C# event that we register to in the UI class
		if (GotJavaCallback != null) 
		GotJavaCallback1(index);
		
	}

	public void updateModelClass(int playerId, int figureId, int countStep, int position)
	{
		javaObject.Call("UpdateModelClass",new object[]{playerId, figureId, countStep, position});
	}

	public void changePlayerTurn(int diceResult, int player)
	{
		javaObject.Call("ChangePlayerTurn",new object[]{diceResult, player});
	}

	public void OnJavaCallbackGetUserTurn(int result)
	{
			if(GotJavaCallbackGetUserTurn != null)
			GotJavaCallbackGetUserTurn(result);
	}

	public void checkMarkerIsAvailableToMove(int diceResult, int player)
	{
		javaObject.Call("CheckMarkerIsAvailableToMove",new object[]{diceResult, player});
	}

	public void OnJavaCallbackGetUserStatus(bool result)
	{
			if(GotJavaCallbackGetUserStatus != null)
			GotJavaCallbackGetUserStatus(result);
	}

	public void CallJavaMethodGetMarkerList()
	{
		javaObject.Call("JavaMethodGetMarkerList");
		Debug.Log("Successfully Marker List Call: ");
	}

	// This method will be invoked from the plugin
	public void OnJavaCallbackMarkerList(int[] toArray)
	{
		// Pass the result to the C# event that we register to in the UI class
		Debug.Log("Happy Message "+ toArray.Length);
		GotJavaCallbackMarkerList(toArray);
	}

	public void CallJavaMethodGetCountStep(int pos)
	{
		javaObject.Call("GetCountStep",new object[]{pos});
	}

	// This method will be invoked from the plugin
	public void OnJavaCallbackGetCountStep(int countStep)
	{
		GotJavaCallbackGetUserCountStep(countStep);
	}
}
