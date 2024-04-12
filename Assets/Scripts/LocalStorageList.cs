using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class LocalStorageList : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI localStorageMessages;
    [SerializeField] Button closeButton, displayButton;
    [SerializeField] GameObject ListPanel;

    // Start is called before the first frame update
    void Start()
    {
        displayButton.onClick.AddListener(displayList);
        closeButton.onClick.AddListener(closeList);
    }

    public void displayList()
    {
        ListPanel.SetActive(true);
        localStorageMessages.text = getList();
    }


    private string getList()
    {
        string result = "";

        List<MqMessage> list = MqttMessageArray.Instance.getMessageList();

        for(int i= list.Count - 1; i >= 0 ; i--)
        {
            result += list[i].message.ToString()+ "  " +i.ToString() + '\n'+ '\n'+ '\n';
        }

        return result;
        
    }

    private void closeList()
    {
        ListPanel.SetActive(false);
    }

   
}
