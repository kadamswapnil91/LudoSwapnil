using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;



public class ErrorDialogScript : MonoBehaviour
{
    [SerializeField] Button button;
    [SerializeField] TextMeshProUGUI errorMessage,buttonText;
    [SerializeField] GameObject ErrorDialog,ForgotPasswordPanel;
    bool isRetry;
    string api = "";
    APICall aPICall;
    M2MqttUnity.M2MqttUnityClient m2MqttUnityClient;
    GameMoveForContestReq gameMoveForContestReq = null;



    private void Start()
    {
        button.onClick.AddListener(closeDialog);
    }

    private void Awake()
    {
        aPICall = FindObjectOfType<APICall>();
        m2MqttUnityClient = FindObjectOfType<M2MqttUnity.M2MqttUnityClient>();

    }

    public void displyDialog(string errorMsg, bool isRetry)
    {
        if (!ErrorDialog.activeInHierarchy)
            ErrorDialog.SetActive(true);

        this.isRetry = isRetry;

        errorMessage.text = errorMsg;

        buttonText.text = "OK";
    }

    public void displyForgotPassDialog(string errorMsg, bool isRetry)
    {
        if (!ErrorDialog.activeInHierarchy)
            ErrorDialog.SetActive(true);

        this.isRetry = isRetry;

        api = "ForgotPassword";

        errorMessage.text = errorMsg;

        buttonText.text = "OK";
    }

    public void closeDialog()
    {
        if(isRetry)
        {
            switch(api)
            {
                case "JoinMatch":
                    if (GameManager.gm.mqttIsConnected)
                        aPICall.fire();
                    else
                        m2MqttUnityClient.Connect();
                break;

                case "storeMovesOnServer":
                    if(gameMoveForContestReq != null)
                    {
                        aPICall.storeMovesOnServer(gameMoveForContestReq);
                    }
                    gameMoveForContestReq = null;
                    break;

                case "getMovesOnServer":
                    aPICall.getMovesOnServer();
                    break;

                case "getMatchStatus":
                    aPICall.callMatchStatus();
                    break;

                case "ForgotPassword":
                    ForgotPasswordPanel.SetActive(false);
                    break;

            }
        }
        if (ErrorDialog.activeInHierarchy)
            ErrorDialog.SetActive(false);
    }

    public void displayRetryDialog(string errorMsg, bool isRetry, string apiName, GameMoveForContestReq gameMoveForContestReq)
    {
        if (!ErrorDialog.activeInHierarchy)
            ErrorDialog.SetActive(true);

        if (gameMoveForContestReq != null)
            this.gameMoveForContestReq = gameMoveForContestReq;

        this.isRetry = isRetry;
        api = apiName;

        errorMessage.text = errorMsg;

        buttonText.text = "Retry";

    }
}
