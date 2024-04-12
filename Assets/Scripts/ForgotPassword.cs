using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text.RegularExpressions;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.SceneManagement;


public class ForgotPassword : MonoBehaviour
{
    [SerializeField] GameObject ForgotPasswordPanel;
    [SerializeField] Button continueButton, backButton;
    [SerializeField]
    TMP_InputField inputFieldEmail;
    [SerializeField] Image emailBorder;
    [SerializeField] TextMeshProUGUI placeHolderEmail;

    APICall aPICall;

    public const string MatchEmailPattern = "^[a-zA-Z0-9+_.-]+@[a-zA-Z0-9.-]+$";

    void Awake()
    {
        aPICall = FindObjectOfType<APICall>();
    }

    private void Start()
    {
        continueButton.onClick.AddListener(forgotPass);
        backButton.onClick.AddListener(back);

    }

    public static bool validateEmail(string email)
    {
        if (email != null)
            return Regex.IsMatch(email, MatchEmailPattern);
        else
            return false;
    }

    public void forgotPass()
    {
        string email;

        email = inputFieldEmail.text;

        bool isValidEmail = false;

        if (!validateEmail(email))
        {
            emailBorder.color = new Color32(255, 0, 0, 255);
            placeHolderEmail.color = Color.red;
            placeHolderEmail.text = "Enter Valid Email";
            isValidEmail = false;
        }
        else
        {
            emailBorder.color = new Color32(255, 255, 255, 255);
            isValidEmail = true;
            placeHolderEmail.color = Color.white;
        }

        if (isValidEmail)
        {
            aPICall.forgotPassword(email);
            inputFieldEmail.text = "";
        }
    }

    public void clearEmailField()
    {
        inputFieldEmail.text = "";
    }

    private void back()
    {
        clearEmailField();

#if UNITY_IOS
        PlayerPrefs.SetInt("QuitRestartGame", 1);
#endif

        SceneManager.LoadScene(1);
    }
}
