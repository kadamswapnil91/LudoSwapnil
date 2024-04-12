using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TestingDiceNumber : MonoBehaviour
{
    [SerializeField] InputField requiredNumber;
    [SerializeField] Button Ok;
    // Start is called before the first frame update
    void Start()
    {
        GameManager.gm.requirdDiceNumber = 1;
        Ok.onClick.AddListener(getDiceNumber);
    }

    void getDiceNumber()
    {
        if(requiredNumber.text != "" && int.Parse(requiredNumber.text)>0 && int.Parse(requiredNumber.text)<7 )
        GameManager.gm.requirdDiceNumber = int.Parse(requiredNumber.text);
    }

}
