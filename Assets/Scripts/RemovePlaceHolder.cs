using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RemovePlaceHolder : InputField
{
    public Text placeholderText;
    public InputField inputField;
    public string placeholder;
 
    public override void OnSelect(BaseEventData data) {
        placeholderText.text = "";
	}

	 public override void OnDeselect(BaseEventData data) {
	 	if(inputField.text == "")
       	placeholderText.text = placeholder;
	}

}
