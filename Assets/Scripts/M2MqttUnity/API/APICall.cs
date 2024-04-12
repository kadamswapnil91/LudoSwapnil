using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;
using UnityEngine.UI;
using System.Text;
using TMPro;
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.SceneManagement;


public class APICall : MonoBehaviour
{
    //   //statging
    public readonly string baseURL = "http://164.52.193.101:8080/";
    public readonly string baseURL1 = "http://164.52.193.101/api/v1/";
    public readonly string baseURL2 = "http://164.52.193.101/";

    //Broker Address = 164.52.192.171
    //Mqtt Password = WJ+#5b5T6Z9VZwYF

    //Production
    //public readonly string baseURL = "http://164.52.194.237:8080/";
    //public readonly string baseURL1 = "http://164.52.194.237/api/v1/";
    //public readonly string baseURL2 = "https://sklash.io/";

    //Broker Address = 164.52.194.237
    //Mqtt Password = P^#S!j7NpPA3x2@D


    [SerializeField] TextMeshProUGUI resMessage;
    [SerializeField] TextMeshProUGUI signUpResMessage;
    [SerializeField] TextMeshProUGUI loginResMessage;
    string token = "";
    ActionHandler actionHandler;

    Login login;
    SignUp signUp;
    WaitingForOpponent waitingForOpponent;
    ExitManager exitManager;
    ErrorDialogScript errorDialogScript;

    [SerializeField] GameObject LoadingPanel;
    [SerializeField] GameObject LoadingPanel1;
    [SerializeField] GameObject ErrorDialog;

    public M2MqttUnity.Examples.M2MqttUnityTest m2MqttUnityTest;
    M2MqttUnity.M2MqttUnityClient m2MqttUnityClient;


    [SerializeField] TextMeshProUGUI message;

    SampleWebView sampleWebView;



    void Start()
    {
        
    }

    private void Awake()
    {
       login = FindObjectOfType<Login>();
       signUp = FindObjectOfType<SignUp>();
       waitingForOpponent = FindObjectOfType<WaitingForOpponent>();
       exitManager = FindObjectOfType<ExitManager>();
       actionHandler = FindObjectOfType<ActionHandler>();
       m2MqttUnityTest = FindObjectOfType<M2MqttUnity.Examples.M2MqttUnityTest>();
       errorDialogScript = FindObjectOfType<ErrorDialogScript>();
       sampleWebView = FindObjectOfType<SampleWebView>();
       m2MqttUnityClient = FindObjectOfType<M2MqttUnity.M2MqttUnityClient>();

    }

    public void fire()
    {
        StartCoroutine(JoinMatch());
    }

    IEnumerator JoinMatch()
    {
        bool isLogin = false;
        string path1 = Application.persistentDataPath + "/loginUserPath.fun";

        if (File.Exists(path1))
        {
            using (Stream stream = File.Open(path1, FileMode.Open))
            {
                var bformatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

                LoginUser loginUser = (LoginUser)bformatter.Deserialize(stream);

                if (loginUser != null)
                {

                    token = loginUser.token;
                    isLogin = true;
                }
            }
        }

        if(isLogin)
        {
            string url = baseURL + "gc_rails/api/v1/matches/" + Credentials.id + "/join";

            MatchJoinRequest matchJoinRequest = new MatchJoinRequest();

            matchJoinRequest.email = Credentials.email;
            matchJoinRequest.gc_contest_id = Credentials.contestId;
            matchJoinRequest.gc_user_id = Credentials.userId;

            string dataJsonString = JsonUtility.ToJson(matchJoinRequest);

            UnityWebRequest request = new UnityWebRequest(url, "POST");
            byte[] bodyRaw = Encoding.UTF8.GetBytes(dataJsonString);
            request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", "Token " + token);
            yield return request.SendWebRequest();


            if (request.error != null)
            {
                string errorText = "";
                Debug.Log("Erro: " + request.error);

                if (request.responseCode.ToString() == "0")
                {
                    errorText = "check internet connection....";
                }
                else
                    errorText = request.error.ToString();

                if (!ErrorDialog.activeInHierarchy)
                    ErrorDialog.SetActive(true);

                errorDialogScript.displayRetryDialog("JoinMatch " + errorText, true, "JoinMatch", null);

                yield break;
            }

            Debug.Log("All OK");
            Debug.Log("Status Code: " + request.responseCode);
        }

        

    }

    public void callAddWinner()
    {
        StartCoroutine(AddWinner());
    }


    IEnumerator AddWinner()
    {
        string url = baseURL + "gc_rails/api/v1/matches/" + Credentials.id;

        Match match = new Match();
        /*int[] matchId = new int[SameMarker.Instance.getResultArray().Count];


         for(int i=0; i< matchId.Length; i++)
         {
             matchId[i] = SameMarker.Instance.getResultArray()[i].user_id;
         }*/

        int[] matchId = new int[1];
        matchId[0] = SameMarker.Instance.getResultArray()[0].user_id;


        match.winner_ids = matchId;


        WinnerRequest winnerRequest = new WinnerRequest();
        winnerRequest.match = match;

        string dataJsonString = JsonUtility.ToJson(winnerRequest);

        DownloadHandlerBuffer dH = new DownloadHandlerBuffer();

        UnityWebRequest request = new UnityWebRequest(url, "PUT");
         byte[] bodyRaw = Encoding.UTF8.GetBytes(dataJsonString);
         request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
         request.downloadHandler = dH;
         request.SetRequestHeader("Content-Type", "application/json");
         request.SetRequestHeader("Authorization", "Token " + token);
         yield return request.SendWebRequest();


         if (request.error != null)
         {
             Debug.Log("Erro: " + request.error);
             yield break;
         }

         Debug.Log("All OK");
         Debug.Log("Status Code: " + request.responseCode);

        resMessage.text = "Status Code: " + request.downloadHandler.text.ToString();
    }


    public void callSignUp(string name, string email, string pass)
    {
        StartCoroutine(SignUp(name, email, pass));
    }

    IEnumerator SignUp(string name, string email, string pass)
    {

        string url = baseURL + "/gc_rails/api/users/signup";

        User user = new User();
        user.first_name = name;
        user.email = email;
        user.password = pass;

        SignUpRequest signUpRequest = new SignUpRequest();
        signUpRequest.user = user;

        string dataJsonString = JsonUtility.ToJson(signUpRequest);

        if (!LoadingPanel.activeInHierarchy)
            LoadingPanel.SetActive(true);

        signUpResMessage.text = "Loading....";

        DownloadHandlerBuffer dH = new DownloadHandlerBuffer();

        UnityWebRequest request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(dataJsonString);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = dH;
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();


        if (request.error != null)
        {
            string errorText = "";
            Debug.Log("Erro: " + request.error);
            if (request.responseCode.ToString() == "422")
            {
                errorText = "Email has already been taken";
            }
            else if (request.responseCode.ToString() == "0")
            {
                errorText = "check internet connection....";
            }
            else
                errorText = request.error.ToString();

            if (!ErrorDialog.activeInHierarchy)
                ErrorDialog.SetActive(true);

            if (LoadingPanel.activeInHierarchy)
                LoadingPanel.SetActive(false);

            errorDialogScript.displyDialog(errorText, false);

            yield break;
        }

        Debug.Log("All OK");
        Debug.Log("Status Code: " + request.responseCode);

        if (LoadingPanel.activeInHierarchy)
            LoadingPanel.SetActive(false);

        signUpResMessage.text = request.responseCode.ToString();

        //if (request.responseCode.ToString() == "200")
        //{
        //    signUpResMessage.text = "Sign Up Successfully";
        //    signUp.openLoginScreen();
        //}

        JSONNode jsonData = JSON.Parse(request.downloadHandler.text);

        string userInfo = jsonData["user"]["token"].ToString();

        LoginUser loginUser = new LoginUser(
            jsonData["user"]["id"],
            jsonData["user"]["email"],
            jsonData["user"]["first_name"],
            jsonData["user"]["last_name"],
            jsonData["user"]["username"],
            jsonData["user"]["token"],
            jsonData["user"]["created_at"],
            jsonData["user"]["updated_at"],
            true,
            false
            );

        string path = Application.persistentDataPath + "/loginUserPath.fun";
        FileStream fs = new FileStream(path, FileMode.Create);
        BinaryFormatter bf = new BinaryFormatter();

        bf.Serialize(fs, loginUser);
        fs.Close();
        // loginResMessage.text = userInfo;

        if (Credentials.id != null)
        {
#if UNITY_ANDROID
            Credentials.isBranchIOhasData = true;
            SceneManager.LoadScene(1);
#endif

#if UNITY_IOS
                if (!waitingForOpponent.getPanelObject().activeInHierarchy)
                    waitingForOpponent.getPanelObject().SetActive(true);


                waitingForOpponent.connectMQ();
#endif

        }
        else
        {
            signUp.openLoginScreen();
        }
            

    }


    public void callLoginWithUserCredentials(string email, string pass)
    {
        StartCoroutine(LoginWithUserCredentials(email, pass));
    }

    IEnumerator LoginWithUserCredentials(string email, string pass)
    {
        string url = baseURL + "/gc_rails/api/users/login";

        User user = new User();
        user.email = email;
        user.password = pass;

        SignUpRequest signUpRequest = new SignUpRequest();
        signUpRequest.user = user;

        string dataJsonString = JsonUtility.ToJson(signUpRequest);

        if (!LoadingPanel.activeInHierarchy)
            LoadingPanel.SetActive(true);

        loginResMessage.text = "Loading....";

        DownloadHandlerBuffer dH = new DownloadHandlerBuffer();

        UnityWebRequest request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(dataJsonString);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = dH;
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();


        if (request.error != null)
        {
            string errorText = "";
            Debug.Log("Erro: " + request.error);
            // JSONNode errorInfo = JSON.Parse(request.downloadHandler.text);
            if (request.responseCode.ToString() == "422")
            {
                errorText = "email or password is invalid";
            }
            else if (request.responseCode.ToString() == "0")
            {
                errorText = "check internet connection....";
            }
            else
                errorText = request.error.ToString();

            loginResMessage.text = errorText;

            if (!ErrorDialog.activeInHierarchy)
                ErrorDialog.SetActive(true);

            if (LoadingPanel.activeInHierarchy)
                LoadingPanel.SetActive(false);

            errorDialogScript.displyDialog(errorText, false);

            yield break;
        }

        Debug.Log("All OK");
        Debug.Log("Status Code: " + request.responseCode);
        if (LoadingPanel.activeInHierarchy)
            LoadingPanel.SetActive(false);


            JSONNode jsonData = JSON.Parse(request.downloadHandler.text);

            string userInfo = jsonData["user"]["token"].ToString();

            LoginUser loginUser = new LoginUser(
                jsonData["user"]["id"],
                jsonData["user"]["email"],
                jsonData["user"]["first_name"],
                jsonData["user"]["last_name"],
                jsonData["user"]["username"],
                jsonData["user"]["token"],
                jsonData["user"]["created_at"],
                jsonData["user"]["updated_at"],
                true,
                false
                );

            string path = Application.persistentDataPath + "/loginUserPath.fun";
            FileStream fs = new FileStream(path, FileMode.Create);
            BinaryFormatter bf = new BinaryFormatter();

            bf.Serialize(fs, loginUser);
            fs.Close();
            // loginResMessage.text = userInfo;

            loginResMessage.text = "Login Successfully";

            if (Credentials.id != null)
            {
#if UNITY_ANDROID
            Credentials.isBranchIOhasData = true;
            SceneManager.LoadScene(1);
#endif

#if UNITY_IOS
                if (!waitingForOpponent.getPanelObject().activeInHierarchy)
                    waitingForOpponent.getPanelObject().SetActive(true);

                waitingForOpponent.connectMQ();
#endif

            }
            else
                login.loginSuccessfully();

    }

    public void callLogin(string loginGcToken)
    {
        StartCoroutine(Login(loginGcToken));
    }

    IEnumerator Login(string loginGcToken)
    {
        if (!LoadingPanel1.activeInHierarchy)
            LoadingPanel1.SetActive(true);
        if (loginGcToken == "" || loginGcToken == null)
        {
            PlayerPrefs.SetInt("loginWithSklash", 0);
            PlayerPrefs.SetString("loginWithSklashTimeStamp", null);
            if (LoadingPanel1.activeInHierarchy)
                LoadingPanel1.SetActive(false);
            yield break;
        }
        string url = baseURL + "/gc_rails/api/users/login";

        //User user = new User();
        //user.email = email;
        //user.password = pass;

        //SignUpRequest signUpRequest = new SignUpRequest();
        //signUpRequest.user = user;

       // string dataJsonString = JsonUtility.ToJson(signUpRequest);

        loginResMessage.text = "Loading....";

        DownloadHandlerBuffer dH = new DownloadHandlerBuffer();

        UnityWebRequest request = new UnityWebRequest(url, "POST");
        //byte[] bodyRaw = Encoding.UTF8.GetBytes(dataJsonString);
        //request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = dH;
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", "Token " + loginGcToken);
        yield return request.SendWebRequest();


        if (request.error != null)
        {
            string errorText = "";
            Debug.Log("Erro: " + request.error);
            // JSONNode errorInfo = JSON.Parse(request.downloadHandler.text);
            if (request.responseCode.ToString() == "422")
            {
                errorText = "email or password is invalid";
            }
            else if (request.responseCode.ToString() == "0")
            {
                errorText = "check internet connection....";
            }
            else
                errorText = request.error.ToString();

            loginResMessage.text = errorText;

            if (!ErrorDialog.activeInHierarchy)
                ErrorDialog.SetActive(true);

            if (LoadingPanel1.activeInHierarchy)
                LoadingPanel1.SetActive(false);

            errorDialogScript.displyDialog(errorText, false);

            yield break;
        }

        Debug.Log("All OK");
        Debug.Log("Status Code: " + request.responseCode);


        if (request.responseCode.ToString() == "201")
        {

            JSONNode jsonData = JSON.Parse(request.downloadHandler.text);

            string userInfo = jsonData["user"]["token"].ToString();

            LoginUser loginUser = new LoginUser(
                jsonData["user"]["id"],
                jsonData["user"]["email"],
                jsonData["user"]["first_name"],
                jsonData["user"]["last_name"],
                jsonData["user"]["username"],
                jsonData["user"]["token"],
                jsonData["user"]["created_at"],
                jsonData["user"]["updated_at"],
                true,
                true
                );

            string path = Application.persistentDataPath + "/loginUserPath.fun";
            FileStream fs = new FileStream(path, FileMode.Create);
            BinaryFormatter bf = new BinaryFormatter();

            bf.Serialize(fs, loginUser);
            fs.Close();
            // loginResMessage.text = userInfo;

            loginResMessage.text = "Login Successfully";

            if (Credentials.id != null)
            {
                if (LoadingPanel1.activeInHierarchy)
                    LoadingPanel1.SetActive(false);
#if UNITY_ANDROID
                SceneManager.LoadScene(1);
#endif

#if UNITY_IOS
                if (!waitingForOpponent.getPanelObject().activeInHierarchy)
                    waitingForOpponent.getPanelObject().SetActive(true);

                waitingForOpponent.connectMQ();
#endif

            }
            else
            {
                if (LoadingPanel1.activeInHierarchy)
                    LoadingPanel1.SetActive(false);
                login.loginSuccessfully();
            }
                

        }

    }

    public void startMatch()
    {
        StartCoroutine(startMatchApi());
    }

    IEnumerator startMatchApi()
    {
        string url = baseURL + "gc_rails/api/v1/matches/"+Credentials.id+"/start";


        StartMatchRequest startMatchRequest = new StartMatchRequest();

        if(Credentials.contestId != null)
        startMatchRequest.contest_id = int.Parse(Credentials.contestId);

        string dataJsonString = JsonUtility.ToJson(startMatchRequest);

        DownloadHandlerBuffer dH = new DownloadHandlerBuffer();

        UnityWebRequest request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(dataJsonString);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = dH;
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", "Token " + token);
        yield return request.SendWebRequest();


        if (request.error != null)
        {
            string errorText = "";
            if (request.responseCode.ToString() == "0")
            {
                errorText = "check internet connection....";
            }
            else
            {
                errorText = request.error.ToString();
            }

            if (!ErrorDialog.activeInHierarchy)
                ErrorDialog.SetActive(true);

            errorDialogScript.displyDialog(errorText, false);


            yield break;
        }


        if (request.responseCode.ToString() == "200")
        {
            Debug.Log("All OK");
            Debug.Log("Status Code: " + request.responseCode);

            if(!Credentials.isAdminContest)
            waitingForOpponent.sucessfullyMatchStart();
        }
    }

    public void callMatchStatus()
    {
        StartCoroutine(getMatchStatus());
    }

    IEnumerator getMatchStatus()
    {
        string url = baseURL + "gc_rails/api/v1/matches/" + Credentials.id + "/get_status";

        StartMatchRequest startMatchRequest = new StartMatchRequest();

        if (Credentials.contestId != null)
            startMatchRequest.contest_id = int.Parse(Credentials.contestId);

        string dataJsonString = JsonUtility.ToJson(startMatchRequest);

        DownloadHandlerBuffer dH = new DownloadHandlerBuffer();

        UnityWebRequest request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(dataJsonString);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = dH;
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", "Token " + GameManager.gm.loginUsertoken);
        yield return request.SendWebRequest();


        if (request.error != null)
        {
            string errorText = "";
            if (request.responseCode.ToString() == "0")
            {
                errorText = "check internet connection....";
            }
            else
            {
                errorText = request.error.ToString();
            }


            GameManager.gm.flow = request.responseCode.ToString();

            errorDialogScript.displayRetryDialog("getMatchStatus "+errorText, true, "getMatchStatus", null);

            yield break;
        }

       
            Debug.Log("All OK");
            Debug.Log("Status Code: " + request.responseCode);

            // waitingForOpponent.sucessfullyMatchStart();

            JSONNode jsonData = JSON.Parse(request.downloadHandler.text);

            string userInfo = jsonData["contest"]["status"].ToString();
            userInfo = userInfo.Replace("\"", "");
            GameManager.gm.flow1 = userInfo;

            if(userInfo == "declared")
            {
                if (waitingForOpponent.getPanelObject().activeInHierarchy)
                {
                    AndroidJavaClass UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                    AndroidJavaObject currentActivity = UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

                    AndroidJavaObject intent = currentActivity.Call<AndroidJavaObject>("getIntent");

                    intent.Call<AndroidJavaObject>("putExtra", "GCPlayer", null);
                    intent.Call<AndroidJavaObject>("putExtra", "tokenID", null);
                    intent.Call<AndroidJavaObject>("putExtra", "MQChannel", null);
                    intent.Call<AndroidJavaObject>("putExtra", "contest_id_gc", null);
                    intent.Call<AndroidJavaObject>("putExtra", "user_id_gc", null);

                    if(GameManager.gm.mqttIsConnected)
                        m2MqttUnityTest.Disconnect();

                    SceneManager.LoadScene(1);
                }
                else
                    exitManager.sendDataToGcApp();
            }
            
    }


    public void storeMovesOnServer(GameMoveForContestReq gameMoveForContestReq)
    {
        StartCoroutine(storeMoves(gameMoveForContestReq));
    }


    IEnumerator storeMoves(GameMoveForContestReq gameMoveForContestReq)
    {
        // string url = baseURL1 + "user/contests/" + Credentials.contestId + "/create_game_moves";
        string url = baseURL + "gc_rails/api/v1/matches/" + Credentials.id + "/game_moves";
        string dataJsonString = JsonUtility.ToJson(gameMoveForContestReq);

        DownloadHandlerBuffer dH = new DownloadHandlerBuffer();

        UnityWebRequest request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(dataJsonString);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = dH;
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", "Token " + token);
        yield return request.SendWebRequest();


        if (request.error != null)
        {
            string errorText = "";
            if (request.responseCode.ToString() == "0")
            {
                errorText = "check internet connection....";
            }
            else
            {
                // loginResMessage.text = request.error.ToString();
                errorText = request.error.ToString();
            }

            errorDialogScript.displayRetryDialog("storeMovesOnServer "+errorText,true, "storeMovesOnServer", gameMoveForContestReq);
            yield break;
        }

        if (request.responseCode.ToString() == "200")
        {
            Debug.Log("All OK");
            Debug.Log("Status Code: " + request.responseCode);
        }
    }


    public void getMovesOnServer()
    {
        StartCoroutine(sendOfflineMessages());

        StartCoroutine(getMoves());
    }


    IEnumerator getMoves()
    {
        // string url = baseURL1 + "user/contests/" + Credentials.contestId + "/game_moves"+ "?move_id="+ MqttMessageArray.Instance.getMessageList().Count.ToString();
        string url = baseURL + "gc_rails/api/v1/matches/" + Credentials.id + "/game_moves" + "?move_id=" + (MqttMessageArray.Instance.getMessageList().Count).ToString();
        //string dataJsonString = JsonUtility.ToJson(gameMoveForContestReq);

        DownloadHandlerBuffer dH = new DownloadHandlerBuffer();

        UnityWebRequest request = new UnityWebRequest(url, "GET");
       // byte[] bodyRaw = Encoding.UTF8.GetBytes(dataJsonString);
       // request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = dH;
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", "Token " + token);
        yield return request.SendWebRequest();


        if (request.error != null)
        {
            string errorText = "";
            if (request.responseCode.ToString() == "0")
            {
                errorText = "check internet connection....";
            }
            else
            {
                // loginResMessage.text = request.error.ToString();
                errorText = request.error.ToString();
            }

            if (LoadingPanel)
                LoadingPanel.SetActive(false);

            if (!ErrorDialog.activeInHierarchy)
                ErrorDialog.SetActive(true);

            errorDialogScript.displyDialog(errorText, false);

            GameManager.gm.isBackground = false;

            yield break;
        }

        if (request.responseCode.ToString() == "200")
        {
            Debug.Log("All OK");
            Debug.Log("Status Code: " + request.responseCode);

            GameManager.gm.isBackground = false;

            GameManager.gm.isAPIcallProcessing = true;

            JSONNode jsonData = JSON.Parse(request.downloadHandler.text);

            JSONArray arr = jsonData.AsArray;

            string size = arr.Count.ToString();

            StartCoroutine(getMessages(arr));

            print(size);

        }
    }

    IEnumerator getMessages(JSONArray arr)
    {

        for (int i = 0; i < arr.Count; i++)
        {
            string msg = arr[i]["info"];

            GameManager.gm.flow = msg;

            decodeJson(msg);

            yield return new WaitForSeconds(3f);

        }

        StartCoroutine(callMqttArray());


         /* if(GameManager.gm.isAPIcallProcessing)
          {
              for(int i=0; i< MqttMessageArray.Instance.getList().Count; i++)
              {
                  decodeJson(MqttMessageArray.Instance.getList()[i].message);

                  yield return new WaitForSeconds(1.8f);
              }

              MqttMessageArray.Instance.clearList();

              GameManager.gm.isAPIcallProcessing = false;

          }

          if (LoadingPanel)
              LoadingPanel.SetActive(false);*/

    }

    IEnumerator sendOfflineMessages()
    {
        if (LoadingPanel)
            LoadingPanel.SetActive(true);


        if (MqttMessageArray.Instance.getOfflinePublishList().Count > 0)
        {
            for (int i = 0; i < MqttMessageArray.Instance.getOfflinePublishList().Count; i++)
            {
                Credentials.action = MqttMessageArray.Instance.getOfflinePublishList()[i];
              //  MqttMessageArray.Instance.addMessages(new MqMessage(Credentials.action));
                m2MqttUnityTest.TestPublish();

                yield return new WaitForSeconds(2f);
            }

            MqttMessageArray.Instance.clearOfflineList();
        }

        yield return new WaitForEndOfFrame();
    }

    IEnumerator callMqttArray()
    {
        if (GameManager.gm.isAPIcallProcessing)
        {

            if(MqttMessageArray.Instance.getList().Count > 0)
            {
                decodeJson(MqttMessageArray.Instance.getList()[0].message);

                MqttMessageArray.Instance.getList().RemoveAt(0);

                yield return new WaitForSeconds(3f);
            }
                
        }

        if(MqttMessageArray.Instance.getList().Count > 0)
        {
            StartCoroutine(callMqttArray());
        }
        else
        {
            GameManager.gm.isAPIcallProcessing = false;

            if (LoadingPanel)
                LoadingPanel.SetActive(false);
        }
           
    }


    public void decodeJson(string msg)
    {
        Debug.Log(msg);

        try
        {
            JSONNode pokeInfo = JSON.Parse(msg);

            JSONNode jsonInfo = pokeInfo["playEvent"];

            JSONNode info = pokeInfo["playEvent"]["playerPieceEvent"];

            if (jsonInfo != null)
            {
                string action = pokeInfo["playEvent"]["action"];

                /*if (action == "actionMoveGreenPiece" || action == "actionMoveRedPiece" || action == "actionMoveBluePiece" || action == "actionMoveYellowPiece")
                  {
                      GameManager.gm.numberOfStepsToMove = info["dice_value"];
                      GameManager.gm.playerTurn = info["playerId"];
                      GameManager.gm.isReadyToMove = true;
                  }*/

                if (info["player_chance"] != null)
                GameManager.gm.playerChance = info["player_chance"];

                if (info["sixCount"] != null)
                    GameManager.gm.sixCount = info["sixCount"];

                if(jsonInfo["action"] != "actionStart")
                {
                    if (SameMarker.Instance.getPlayerArray()[getIndex()].player_name == Credentials.email)
                    {
                        if (action == "actionAcknowledgement")
                        {
                            return;
                        }
                    }
                }

                actionHandler.action(jsonInfo["action"], pokeInfo);
                GameManager.gm.flow2 = GameManager.gm.flow2.ToString() + "\n" + jsonInfo["action"].ToString();
            }
            
        }
        catch (Exception e)
        {
            Debug.LogException(e, this);

        }
    }


    int getIndex()
    {
        int index = GameManager.gm.playerTurn;
        if (SameMarker.Instance.getPlayerArray().Count == 2)
        {
            if (index == 3)
                index = 1;
            else
                index = index - 1;
        }
        else
        {
            index = index - 1;
        }

        return index;
    }


    public void quitGameAPI()
    {
        StartCoroutine(quitGame());
    }

    IEnumerator quitGame()
    {
        string url = baseURL + "/gc_rails/api/v1/matches/"+Credentials.id+"/quite";

        QuitPlayerRequest quitPlayerRequest = new QuitPlayerRequest();

        if (Credentials.email != null)
            quitPlayerRequest.email = Credentials.email;

        string dataJsonString = JsonUtility.ToJson(quitPlayerRequest);

        DownloadHandlerBuffer dH = new DownloadHandlerBuffer();

        UnityWebRequest request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(dataJsonString);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = dH;
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", "Token " + token);
        yield return request.SendWebRequest();

        if (!LoadingPanel.activeInHierarchy)
            LoadingPanel.SetActive(true);

        if (request.error != null)
        {
            string errorText = "";
            if (request.responseCode.ToString() == "0")
            {
                errorText = "check internet connection....";
            }
            else if (request.responseCode.ToString() == "401")
            {
                Application.Quit();
            }
            else
            {
                errorText = request.error.ToString();
            }

            if(!ErrorDialog.activeInHierarchy)
            ErrorDialog.SetActive(true);

            errorDialogScript.displyDialog(errorText, false);

            if (LoadingPanel)
                LoadingPanel.SetActive(false);

            yield break;
        }


        if (LoadingPanel.activeInHierarchy)
            LoadingPanel.SetActive(false);

        clearIntent();
        clearCredentials();
        SceneManager.LoadScene(1);

       // Application.Quit();

    }



    public void checkConnection(string menuFrom)
    {
        StartCoroutine(checkInternetStatus(menuFrom));
    }

    IEnumerator checkInternetStatus(string menuFrom)
    {
        
        string url = "http://sklash.io/";

        UnityWebRequest request = new UnityWebRequest(url, "GET");
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();

        if (!LoadingPanel.activeInHierarchy)
            LoadingPanel.SetActive(true);

        if (request.error != null)
        {
            string errorText = "";
            Debug.Log("Erro: " + request.error);

            if (request.responseCode.ToString() == "0")
            {
                errorText = "check internet connection....";
            }
            else
                errorText = request.error.ToString();

            errorDialogScript.displyDialog(errorText, false);

            if (LoadingPanel.activeInHierarchy)
                LoadingPanel.SetActive(false);

            yield break;
        }

        Debug.Log("All OK");
        Debug.Log("Status Code: " + request.responseCode);

        GameManager.gm.isWebPageOpen = true;

        if(menuFrom == "about_us")
        {
            sampleWebView.OpenWebPage(baseURL2+"ludos");
        }
        else if (menuFrom == "home")
        {
            sampleWebView.OpenWebPage(baseURL2);
        }
        else
        {
            sampleWebView.OpenWebPage(baseURL2+"home/faq");
        }

        yield return new WaitForSeconds(3f);

        if (LoadingPanel.activeInHierarchy)
            LoadingPanel.SetActive(false);

    }



    public void forgotPassword(string email)
    {
        StartCoroutine(doForgotPassword(email));
    }


    IEnumerator doForgotPassword(string email)
    {
        if (!LoadingPanel.activeInHierarchy)
            LoadingPanel.SetActive(true);

        string url = baseURL + "gc_rails/api/users/forgot_password";

        QuitPlayerRequest quitPlayerRequest = new QuitPlayerRequest();

        if (email != null)
            quitPlayerRequest.email = email;

        string dataJsonString = JsonUtility.ToJson(quitPlayerRequest);

        DownloadHandlerBuffer dH = new DownloadHandlerBuffer();

        UnityWebRequest request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(dataJsonString);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = dH;
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();

        if (request.error != null)
        {
            string errorText = "";
            if (request.responseCode.ToString() == "0")
            {
                errorText = "check internet connection....";
            }
            else
            {
                errorText = request.error.ToString();
            }

            if (!ErrorDialog.activeInHierarchy)
                ErrorDialog.SetActive(true);

            errorDialogScript.displyDialog(errorText, false);

            if (LoadingPanel)
                LoadingPanel.SetActive(false);

            yield break;
        }


        if (LoadingPanel.activeInHierarchy)
            LoadingPanel.SetActive(false);

        errorDialogScript.displyForgotPassDialog("We have sent reset link on your mail", false);

    }


    void clearIntent()
    {
        AndroidJavaClass UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject currentActivity = UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

        AndroidJavaObject intent = currentActivity.Call<AndroidJavaObject>("getIntent");

        intent.Call<AndroidJavaObject>("putExtra", "GCPlayer", null);
        intent.Call<AndroidJavaObject>("putExtra", "tokenID", null);
        intent.Call<AndroidJavaObject>("putExtra", "MQChannel", null);
        intent.Call<AndroidJavaObject>("putExtra", "contest_id_gc", null);
        intent.Call<AndroidJavaObject>("putExtra", "user_id_gc", null);
        intent.Call<AndroidJavaObject>("putExtra", "is_admin_contest", false);
    }

    void clearCredentials()
    {
        Credentials.id = null;
        Credentials.channel = null;
        Credentials.id = null;
        Credentials.old_id = null;
        Credentials.email = null;
        Credentials.action = null;
        Credentials.contestId = null;
        Credentials.userId = null;
        GameManager.gm.isOnlineGame = false;
        SeriouslyDeleteAllSaveFiles();
        GameManager.gm.isGameRunning = false;
        SameMarker.ClearInstance();
        MqttMessageArray.ClearInstance();
        GameManager.clearGameManager();
        m2MqttUnityTest.Disconnect();
        Credentials.isAdminContest = false;
        Credentials.contestSize = null;

    }

    public static void SeriouslyDeleteAllSaveFiles()
    {
        string path = Application.persistentDataPath + "/player.fun";
        File.Delete(path);

        string path1 = Application.persistentDataPath + "/playername.fun";
        File.Delete(path1);

        string path2 = Application.persistentDataPath + "/gamemanager.fun";
        File.Delete(path2);

        string path3 = Application.persistentDataPath + "/winnerList.fun";
        File.Delete(path3);

        /*DirectoryInfo directory = new DirectoryInfo(path);
        directory.Delete(true);
        Directory.CreateDirectory(path);*/
    }


    public void checkInternetConnection()
    {
        StartCoroutine(checkInternetStatus());
    }

    IEnumerator checkInternetStatus()
    {
        if (GameManager.gm.isOnlineGame || waitingForOpponent.getPanelObject().activeInHierarchy)
        {
            string url = "https://www.google.com/";

            UnityWebRequest request = new UnityWebRequest(url, "GET");
            request.SetRequestHeader("Content-Type", "application/json");
            yield return request.SendWebRequest();


            if (request.error != null)
            {
                InternetConnectionAvailability.timeRemaining = 3;
                m2MqttUnityClient.Disconnect();
                yield break;
            }


            InternetConnectionAvailability.call = true;
            yield return new WaitForSeconds(1f);

            if(!InternetConnectionAvailability.isFirstTimeCall)
            m2MqttUnityClient.Connect();

            yield return new WaitForSeconds(3f);
            InternetConnectionAvailability.timeRemaining = 3;
            InternetConnectionAvailability.call = false;
        }
    }

}
