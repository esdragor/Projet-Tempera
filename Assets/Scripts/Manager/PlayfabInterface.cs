using System;
using System.Collections;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using PlayFab.CloudScriptModels;
using UnityEngine;

public class PlayfabInterface : MonoBehaviour
{
    private static PlayfabInterface m_instance;
    public System.Action OnConnexion;

    private string temp_mail;
    private string temp_username;
    private string idPlayfab;

    private bool pendingRequest = false;




    public static PlayfabInterface instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = new GameObject("PlayfabInterface").AddComponent<PlayfabInterface>();
            }

            return m_instance;

        }
        private set { }
    }

    private void Awake()
    {
        if (m_instance == null)
        {
            m_instance = this;
            DontDestroyOnLoad(gameObject);
        }


        InitPlayfab("5357B");
        StartCoroutine("WaitEndOfRequest");
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
        temp_mail = _mail;
        registerRequest.Password = _password;
        registerRequest.Username = _username;
        PlayFabClientAPI.RegisterPlayFabUser(registerRequest, OnRegisterSuccess, OnRegisterFailure);
    }

    internal void UpdateUsername(string _username)
    {
        pendingRequest = true;
        PlayFabClientAPI.UpdateUserTitleDisplayName(new UpdateUserTitleDisplayNameRequest
        {
            DisplayName = _username
        },
     result2 => { pendingRequest = false; },
      error => { Debug.LogError(error.GenerateErrorReport()); });
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

    internal void GetCurrencies()
    {
        pendingRequest = true;
        PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(),
                  result =>
                  {
                      foreach (KeyValuePair<string, int> langage in result.VirtualCurrency)
                      {
                          SetCurrencies(langage.Key, langage.Value);
                      }
                      pendingRequest = false;
                  },
                   error => { Debug.LogError(error.GenerateErrorReport()); });
    }

    internal void GetItemsCalatogs()
    {
        pendingRequest = true;
        var updateRequest = new GetCatalogItemsRequest();
        updateRequest.CatalogVersion = "Items";
        //  PlayFabClientAPI.GetCatalogItems(new GetCatalogItemsRequest(), ItemsList, OnRegisterFailure);
    }


    //AZUR FUNCTION LATER
    public void SetStatisticsPlayer()
    {
        pendingRequest = true;
        PlayFabClientAPI.UpdatePlayerStatistics(new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate> {
        new StatisticUpdate { StatisticName = "PlayerLevel", Value = 1 }
         }
        },
        result2 =>
        {
            pendingRequest = false;
            Debug.Log("User statistics updated");
        },
        error => { Debug.LogError(error.GenerateErrorReport()); });

    }

    public void GetStatisticsPlayer()
    {
        pendingRequest = true;
        PlayFabClientAPI.GetPlayerStatistics(new GetPlayerStatisticsRequest(), OnGetStatisticsPlayer, error => Debug.LogError(error.GenerateErrorReport()));
    }

    //CHECK LE CLOUD et return la bonne valeur
    void OnGetStatisticsPlayer(GetPlayerStatisticsResult result)
    {
        Debug.Log("Received the following Statistics:");
        foreach (var eachStat in result.Statistics)
        {
            //Debug.Log("Statistic (" + eachStat.StatisticName + "): " + eachStat.Value);
            switch (eachStat.StatisticName)
            {
                case "PlayerLevel":
                    Debug.LogError("here" + eachStat.Value);
                    GameManager.Instance.localAccountData.level = eachStat.Value;
                    break;
            }
        }

        pendingRequest = false;

    }

    #endregion
    // Start is called before the first frame update


    #region PrivateFunction

    #endregion

    #region CallBackPlayfab


    void OnRegisterSuccess(RegisterPlayFabUserResult result)
    {
        GameManager.Instance.localAccountData.SetUsername(temp_mail);
        GameManager.Instance.localAccountData.SetUsername(result.Username);
        GameManager.Instance.localAccountData.SetID(result.PlayFabId);
        SetStatisticsPlayer();
        UpdatePlayerData();
    }
    void OnRegisterFailure(PlayFabError error)
    {
        Debug.LogError("Erreur d'Enregistement");
        Debug.Log(error.HttpCode);
        Debug.Log(error.GenerateErrorReport());

    }

    void OnLoginSuccess(LoginResult login)
    {
        Debug.LogError("test");
        GameManager.Instance.localAccountData.idPlayfab = login.PlayFabId;
        PlayFabClientAPI.GetPlayerProfile(new GetPlayerProfileRequest()
        {
            PlayFabId = login.PlayFabId,
            ProfileConstraints = new PlayerProfileViewConstraints()
            {
                ShowDisplayName = true
            }
        },
        result => SetData(result),
        error => Debug.LogError(error.GenerateErrorReport()));

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

    private void UpdatePlayerData()
    {
        UpdateUsername(GameManager.Instance.localAccountData.GetUsername());
        GetCurrencies();
        GetStatisticsPlayer();
        UIManager.Instance.ActualizeData();
        SceneManager.Instance.LoadingScene(SceneManager.SceneType.MENU);

    }
    IEnumerator WaitEndOfRequest()
    {
        while (pendingRequest)
        {
            yield return new WaitForSeconds(0.2f);
        }
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

    private void SetCurrencies(string key, int value)
    {
        if (key.Equals("OR"))
            GameManager.Instance.localAccountData.gold = value;
        if (key.Equals("GE"))
            GameManager.Instance.localAccountData.gems = value;
        if (key.Equals("EN"))
            GameManager.Instance.localAccountData.energy = value;
    }


    private void SetData(GetPlayerProfileResult result)
    {
        GameManager.instance.localAccountData.SetUsername(result.PlayerProfile.DisplayName);
        UpdatePlayerData();
    }
    private void SetLevelAzur()
    {
        PlayFabCloudScriptAPI.ExecuteFunction(new ExecuteFunctionRequest()
        {
            Entity = new PlayFab.CloudScriptModels.EntityKey()
            {
                Id = PlayFabSettings.staticPlayer.EntityId, //Get this from when you logged in,
                Type = PlayFabSettings.staticPlayer.EntityType, //Get this from when you logged in
            },
            FunctionName = "SetDataRegister", //This should be the name of your Azure Function that you created.
                                              //FunctionParameter = new Dictionary<string, object>() { { "inputValue", "Test" } }, //This is the data that you would want to pass into your function.
            GeneratePlayStreamEvent = false //Set this to true if you would like this call to show up in PlayStream
        }, (ExecuteFunctionResult result2) =>
        {
            Debug.Log("SET LEVEL TO " + result2.FunctionResult.ToString());
        }, (PlayFabError error) =>
        {
            Debug.Log($"Opps Something went wrong: {error.GenerateErrorReport()}");
        });
    }
    #endregion

}
