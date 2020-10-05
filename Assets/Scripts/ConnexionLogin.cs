using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConnexionLogin : MonoBehaviour
{

    [Header("Pannel Connex")]
    [SerializeField] GameObject loginPanel;
    [SerializeField] GameObject registerPanel;

    [Header("Login Connex")]
    [SerializeField] InputField usernameLogin;
    [SerializeField] InputField passwordLogin;

    [Header("Register Connex")]
    [SerializeField] InputField usernameRegister;
    [SerializeField] InputField passwordRegister;
    [SerializeField] InputField mailRegister;



    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        CheatLogin();
    }


    private void CheatLogin()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            usernameLogin.text = "admin1";
            passwordLogin.text = "admin1";
            
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            usernameLogin.text = "admin2";
            passwordLogin.text = "admin2";
            
        }
        if (Input.GetKeyDown(KeyCode.F3))
        {
            usernameLogin.text = "admin3";
            passwordLogin.text = "admin3";
      
        }
    }

    public void ButtonLogin()
    {
        PlayfabInterface.instance.LoginUnity(usernameLogin.text, passwordLogin.text);

    }

    public void ButtonRegister()
    {
        PlayfabInterface.instance.RegisterUnity(usernameRegister.text, passwordRegister.text, mailRegister.text);
    }
















}
