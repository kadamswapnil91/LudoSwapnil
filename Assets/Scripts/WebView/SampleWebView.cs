/*
 * Copyright (C) 2012 GREE, Inc.
 * 
 * This software is provided 'as-is', without any express or implied
 * warranty.  In no event will the authors be held liable for any damages
 * arising from the use of this software.
 * 
 * Permission is granted to anyone to use this software for any purpose,
 * including commercial applications, and to alter it and redistribute it
 * freely, subject to the following restrictions:
 * 
 * 1. The origin of this software must not be misrepresented; you must not
 *    claim that you wrote the original software. If you use this software
 *    in a product, an acknowledgment in the product documentation would be
 *    appreciated but is not required.
 * 2. Altered source versions must be plainly marked as such, and must not be
 *    misrepresented as being the original software.
 * 3. This notice may not be removed or altered from any source distribution.
 */

using System.Collections;
using UnityEngine;
#if UNITY_2018_4_OR_NEWER
using UnityEngine.Networking;
#endif
using UnityEngine.UI;

public class SampleWebView : MonoBehaviour
{
   // public Text status;
    WebViewObject webViewObject;
    [SerializeField] GameObject navDrawer;
    MenuHandler menuHandler;
    bool isAvailable;


    public void OpenWebPage(string Url)
    {
        StartCoroutine(WebPage(Url));
    }

    private void Awake()
    {
        menuHandler = FindObjectOfType<MenuHandler>();
    }

    IEnumerator WebPage(string Url)
    {
        isAvailable = true;
        navDrawer.SetActive(false);
        webViewObject = (new GameObject("WebViewObject")).AddComponent<WebViewObject>();
        webViewObject.Init(
            cb: (msg) =>
            {
                Debug.Log(string.Format("CallFromJS[{0}]", msg));
              //  status.text = msg;
              //  status.GetComponent<Animation>().Play();
            },
            err: (msg) =>
            {
                Debug.Log(string.Format("CallOnError[{0}]", msg));
              //  status.text = msg;
              //  status.GetComponent<Animation>().Play();
            },
            started: (msg) =>
            {
                Debug.Log(string.Format("CallOnStarted[{0}]", msg));
            },
            hooked: (msg) =>
            {
                Debug.Log(string.Format("CallOnHooked[{0}]", msg));
            },
            ld: (msg) =>
            {
                Debug.Log(string.Format("CallOnLoaded[{0}]", msg));
#if UNITY_EDITOR_OSX || (!UNITY_ANDROID && !UNITY_WEBPLAYER && !UNITY_WEBGL)
                // NOTE: depending on the situation, you might prefer
                // the 'iframe' approach.
                // cf. https://github.com/gree/unity-webview/issues/189
#if true
                webViewObject.EvaluateJS(@"
                  if (window && window.webkit && window.webkit.messageHandlers && window.webkit.messageHandlers.unityControl) {
                    window.Unity = {
                      call: function(msg) {
                        window.webkit.messageHandlers.unityControl.postMessage(msg);
                      }
                    }
                  } else {
                    window.Unity = {
                      call: function(msg) {
                        window.location = 'unity:' + msg;
                      }
                    }
                  }
                ");
#else
                webViewObject.EvaluateJS(@"
                  if (window && window.webkit && window.webkit.messageHandlers && window.webkit.messageHandlers.unityControl) {
                    window.Unity = {
                      call: function(msg) {
                        window.webkit.messageHandlers.unityControl.postMessage(msg);
                      }
                    }
                  } else {
                    window.Unity = {
                      call: function(msg) {
                        var iframe = document.createElement('IFRAME');
                        iframe.setAttribute('src', 'unity:' + msg);
                        document.documentElement.appendChild(iframe);
                        iframe.parentNode.removeChild(iframe);
                        iframe = null;
                      }
                    }
                  }
                ");
#endif
#elif UNITY_WEBPLAYER || UNITY_WEBGL
                webViewObject.EvaluateJS(
                    "window.Unity = {" +
                    "   call:function(msg) {" +
                    "       parent.unityWebView.sendMessage('WebViewObject', msg)" +
                    "   }" +
                    "};");
#endif
                webViewObject.EvaluateJS(@"Unity.call('ua=' + navigator.userAgent)");
            },
            //ua: "custom user agent string",
#if UNITY_EDITOR
            separated: false,
#endif
            enableWKWebView: true,
            wkContentMode: 0);  // 0: recommended, 1: mobile, 2: desktop
#if UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX
        webViewObject.bitmapRefreshCycle = 1;
#endif
        // cf. https://github.com/gree/unity-webview/pull/512
        // Added alertDialogEnabled flag to enable/disable alert/confirm/prompt dialogs. by KojiNakamaru · Pull Request #512 · gree/unity-webview
        //webViewObject.SetAlertDialogEnabled(false);

        // cf. https://github.com/gree/unity-webview/pull/550
        // introduced SetURLPattern(..., hookPattern). by KojiNakamaru · Pull Request #550 · gree/unity-webview
        //webViewObject.SetURLPattern("", "^https://.*youtube.com", "^https://.*google.com");

        // cf. https://github.com/gree/unity-webview/pull/570
        // Add BASIC authentication feature (Android and iOS with WKWebView only) by takeh1k0 · Pull Request #570 · gree/unity-webview
        //webViewObject.SetBasicAuthInfo("id", "password");
        int width = Screen.width;
        int height = Screen.height;

        if(width <= 720)
        {
            webViewObject.SetMargins(5, 110, 5, 5);
        }
        else
        webViewObject.SetMargins(5, 150, 5, 5);
        webViewObject.SetVisibility(true);

#if !UNITY_WEBPLAYER && !UNITY_WEBGL
        if (Url.StartsWith("http")) {
            webViewObject.LoadURL(Url.Replace(" ", "%20"));
        } else {
            var exts = new string[]{
                ".jpg",
                ".js",
                ".html"  // should be last
            };
            foreach (var ext in exts) {
                var url = Url.Replace(".html", ext);
                var src = System.IO.Path.Combine(Application.streamingAssetsPath, url);
                var dst = System.IO.Path.Combine(Application.persistentDataPath, url);
                byte[] result = null;
                if (src.Contains("://")) {  // for Android
#if UNITY_2018_4_OR_NEWER
                    // NOTE: a more complete code that utilizes UnityWebRequest can be found in https://github.com/gree/unity-webview/commit/2a07e82f760a8495aa3a77a23453f384869caba7#diff-4379160fa4c2a287f414c07eb10ee36d
                    var unityWebRequest = UnityWebRequest.Get(src);
                    yield return unityWebRequest.SendWebRequest();
                    result = unityWebRequest.downloadHandler.data;
#else
                    var www = new WWW(src);
                    yield return www;
                    result = www.bytes;
#endif
                } else {
                    result = System.IO.File.ReadAllBytes(src);
                }
                System.IO.File.WriteAllBytes(dst, result);
                if (ext == ".html") {
                    webViewObject.LoadURL("file://" + dst.Replace(" ", "%20"));
                    break;
                }
            }
        }
#else
        if (Url.StartsWith("http")) {
            webViewObject.LoadURL(Url.Replace(" ", "%20"));
        } else {
            webViewObject.LoadURL("StreamingAssets/" + Url.Replace(" ", "%20"));
        }
#endif
        yield break;
    }

    void OnGUI()
    {

        if (isAvailable)
        {

            int widthHeight = 0;
            int buttonWidth = 0;
            int fontSize = 0;

            if (Screen.width <= 720)
            {
                widthHeight = 50;
                buttonWidth = 150;
                fontSize = 30;
            }
            else
            {
                widthHeight = 80;
                buttonWidth = 200;
                fontSize = 50;
            }


            // Create style for a button
            GUIStyle myButtonStyle = new GUIStyle(GUI.skin.button);
            myButtonStyle.fontSize = fontSize;
            // Load and set Font
            Font myFont = (Font)Resources.Load("Fonts/comic", typeof(Font));
            myButtonStyle.font = myFont;

            GUI.enabled = webViewObject.CanGoBack();
            if (GUI.Button(new Rect(60, 50, widthHeight, widthHeight), "<", myButtonStyle))
            {
                webViewObject.GoBack();
            }
            GUI.enabled = true;

            GUI.enabled = webViewObject.CanGoForward();
            if (GUI.Button(new Rect(160, 50, widthHeight, widthHeight), ">", myButtonStyle))
            {
                webViewObject.GoForward();
            }
            GUI.enabled = true;

            if (GUI.Button(new Rect(260, 50, buttonWidth, widthHeight), "Refresh", myButtonStyle))
            {
                webViewObject.Reload();
            }

            //  GUI.TextField(new Rect(300, 10, 200, 80), "" + webViewObject.Progress());

            if (Screen.width <= 720)
            {
                if (GUI.Button(new Rect(500, 50, buttonWidth, widthHeight), "Close", myButtonStyle))
                {
                    var g = GameObject.Find("WebViewObject");
                    if (g != null)
                    {
                        Destroy(g);
                    }
                    navDrawer.SetActive(true);
                    isAvailable = false;
                    GameManager.gm.isWebPageOpen = false;
                    menuHandler.resetSelectedButton();
                }
                GUI.enabled = true;
            }
            else
            {
                if (GUI.Button(new Rect(800, 50, buttonWidth, widthHeight), "Close", myButtonStyle))
                {
                    var g = GameObject.Find("WebViewObject");
                    if (g != null)
                    {
                        Destroy(g);
                    }
                    navDrawer.SetActive(true);
                    isAvailable = false;
                    GameManager.gm.isWebPageOpen = false;
                    menuHandler.resetSelectedButton();
                }
                GUI.enabled = true;
            }

           

        }
    }

    public void closeGUI()
    {
        var g = GameObject.Find("WebViewObject");
        if (g != null)
        {
            Destroy(g);
        }
        navDrawer.SetActive(true);
        isAvailable = false;
        GameManager.gm.isWebPageOpen = false;
    }

}
