using System.Collections;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

public class PlayfabInterface : MonoBehaviour
{
    private static PlayfabInterface m_instance = null;
    public System.Action OnConnexion;

    private string temp_mail;
    private string temp_username;
    private string idPlayfab;




    public static PlayfabInterface instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = new PlayfabInterface();
            }

            return m_instance;
        }
        private set { }
    }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }

        InitPlayfab("C08BA");
        OnConnexion += UpdateAccountData;


    }

    private void InitPlayfab(string id)
    {
        if (string.IsNullOrEmpty(PlayFabSettings.TitleId))
        {
            PlayFabSettings.TitleId = id;

        }

    }


    #region FunctionConnex Utilisable
    internal void LoginUnity(string _username, string _password)
    {
        var loginRequest = new LoginWithPlayFabRequest();
        loginRequest.Username = _username;
        loginRequest.Password = _password;
        PlayFabClientAPI.LoginWithPlayFab(loginRequest, OnLoginSuccess, OnLoginFailure);
    }
    internal void LoginGoogle()
    {
    }

    internal void LoginFacebook()
    {
    }

    internal void RegisterUnity(string _username, string _password, string _mail)
    {
        var registerRequest = new RegisterPlayFabUserRequest();
        registerRequest.Email = _mail;
        registerRequest.Password = _password;
        registerRequest.Username = _username;
        PlayFabClientAPI.RegisterPlayFabUser(registerRequest, OnRegisterSuccess, OnRegisterFailure);
    }

    internal void UpdateUsername(string _username)
    {
        var updateRequest = new UpdateUserTitleDisplayNameRequest();
        updateRequest.DisplayName = _username;
        GameManager.Instance.localAccountData.SetUsername(_username);
        PlayFabClientAPI.UpdateUserTitleDisplayName(updateRequest, OnUpdateNameSuccess, OnUpdateNameFailure);
    }
    internal void UpdateContactEmail(string _mail)
    {
        var updateRequest = new AddOrUpdateContactEmailRequest();
        updateRequest.EmailAddress = _mail;
        GameManager.Instance.localAccountData.SetMail(_mail);
        PlayFabClientAPI.AddOrUpdateContactEmail(updateRequest, OnUpdateMailSuccess, OnUpdateEmailFailure);
    }

    internal void UpdatePlayerProfile(string _playfabID)
    {
        var updateRequest = new GetPlayerProfileRequest();
        updateRequest.PlayFabId = _playfabID;
        PlayFabClientAPI.GetPlayerProfile(updateRequest, OnPlayerProfileSuccess, OnPlayerProfileFailure);

    }
    internal void UpdateAccountData()
    {
        UpdateUsername("ErwangeGerot");
        UpdateContactEmail("syhfrappez@gmail.com");

    }


    #endregion
    // Start is called before the first frame update


    #region PrivateFunction

    #endregion

    #region CallBackPlayfab


    void OnRegisterSuccess(RegisterPlayFabUserResult result)
    {
        GameManager.Instance.localAccountData.SetUsername(result.Username);
        GameManager.Instance.localAccountData.SetID(result.PlayFabId);
        

 
        var updateRequest = new UpdateUserTitleDisplayNameRequest();
        updateRequest.DisplayName = GameManager.Instance.localAccountData.GetUsername();
        PlayFabClientAPI.UpdateUserTitleDisplayName(updateRequest, OnUpdateNameSuccess, OnUpdateNameFailure);
     

       // UpdateAccountData();

    }

    void OnRegisterFailure(PlayFabError error)
    {
        Debug.LogError("Erreur d'Enregistement");
        Debug.Log(error.HttpCode);
        Debug.Log(error.GenerateErrorReport());

    }

    void OnLoginSuccess(LoginResult result)
    {


    }

    void OnLoginFailure(PlayFabError error)
    {
        Debug.LogError("Erreur de login");
        Debug.Log(error.HttpCode);
        Debug.Log(error.GenerateErrorReport());

    }


    void OnUpdateMailSuccess(AddOrUpdateContactEmailResult result)
    {

        Debug.LogError("MAIL GOOD");

    }

    void OnUpdateEmailFailure(PlayFabError error)
    {
        Debug.LogError("Erreur d'update Email");
        Debug.Log(error.HttpCode);
        Debug.Log(error.GenerateErrorReport());

    }


    void OnUpdateNameSuccess(UpdateUserTitleDisplayNameResult result)
    {

        SceneManager.Instance.LoadingScene(SceneManager.SceneType.MENU);
        Debug.LogError("Name GOOD");

    }

    void OnUpdateNameFailure(PlayFabError error)
    {
        Debug.LogError("Erreur d'update Name");
        Debug.Log(error.HttpCode);
        Debug.Log(error.GenerateErrorReport());

    }

    void OnPlayerProfileSuccess(GetPlayerProfileResult result)
    {
        result.PlayerProfile.DisplayName = GameManager.Instance.localAccountData.GetUsername();
    }

    void OnPlayerProfileFailure(PlayFabError error)
    {
        Debug.LogError("Erreur d'update profile");
        Debug.Log(error.HttpCode);
        Debug.Log(error.GenerateErrorReport());

    }


    #endregion

}
