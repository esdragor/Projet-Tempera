using System;
using System.Collections;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using PlayFab.CloudScriptModels;
using UnityEngine;

public class PlayfabInterface : MonoBehaviour
{
    private static PlayfabInterface m_instance = null;
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

        var updateRequest = new GetCatalogItemsRequest();
        updateRequest.CatalogVersion = "Items";
        PlayFabClientAPI.GetCatalogItems(new GetCatalogItemsRequest(), ItemsList, OnRegisterFailure);



    }

    private void ItemsList(GetCatalogItemsResult result)
    {
        for (int i = 0; i < result.Catalog.Count; i++)
        {
            Items temp = new Items();
            temp.id = result.Catalog[i].ItemId;
            temp.itemsName = result.Catalog[i].DisplayName;
            for (int j = 0; j < result.Catalog[i].Tags.Count; j++)
            {
                temp.typeOfItems[j] = Items.SetTypeOfItems(result.Catalog[i].Tags[j]);
            }
            temp.urlImg = result.Catalog[i].ItemImageUrl;
            temp.itemMetier = Items.SetTypeOfMetier(result.Catalog[i].ItemClass);
            temp.isStackable = result.Catalog[i].IsStackable;
            temp.itemDescription = result.Catalog[i].Description;
            foreach (KeyValuePair<string, uint> currency in result.Catalog[i].VirtualCurrencyPrices)
            {
                if (currency.Key.Equals("EN"))
                    temp.costEnergy = (int)currency.Value;

                if (currency.Key.Equals("OR"))
                    temp.costGold = (int)currency.Value;

                if (currency.Key.Equals("GE"))
                    temp.costGems = (int)currency.Value;
            }
            string[] customDataTab = Items.ParseCustomData(result.Catalog[i].CustomData);
            for (int j = 0; j < customDataTab.Length; j++)
                SetCustomData(temp, customDataTab[i], customDataTab[i + 1]);
            // temp.levelRequiered = result.Catalog[i].CustomData

        }

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
        SetLevelAzur();
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

    private void SetCustomData(Items item, string key, string value)
    {
        if (key.Equals("Level"))
            item.levelRequiered = Int32.Parse(value);
        if (key.Equals("Vente"))
            item.sellGold = Int32.Parse(value);
        if (key.Equals("Achat"))
            item.buyGold = Int32.Parse(value);
        if (key.Equals("Besoin"))
            item.idItemNecessary = value;
    }

    private void SetData(GetPlayerProfileResult result)
    {
        GameManager.instance.localAccountData.SetUsername(result.PlayerProfile.DisplayName);
        GetCurrencies();


        /* PlayFabCloudScriptAPI.ExecuteFunction(new ExecuteFunctionRequest()
         {
             Entity = new PlayFab.CloudScriptModels.EntityKey()
             {
                 Id = PlayFabSettings.staticPlayer.EntityId, //Get this from when you logged in,
                 Type = PlayFabSettings.staticPlayer.EntityType, //Get this from when you logged in
             },
             FunctionName = "FirstMessage", //This should be the name of your Azure Function that you created.
             FunctionParameter = new Dictionary<string, object>() { { "inputValue", "Test" } }, //This is the data that you would want to pass into your function.
             GeneratePlayStreamEvent = false //Set this to true if you would like this call to show up in PlayStream
         }, (ExecuteFunctionResult result2) =>
         {
             if (result2.FunctionResultTooLarge ?? false)
             {
                 Debug.Log("This can happen if you exceed the limit that can be returned from an Azure Function, See PlayFab Limits Page for details.");
                 return;
             }
             Debug.Log($"The {result2.FunctionName} function took {result2.ExecutionTimeMilliseconds} to complete");
             Debug.Log($"Result: {result2.FunctionResult.ToString()}");
         }, (PlayFabError error) =>
         {
             Debug.Log($"Opps Something went wrong: {error.GenerateErrorReport()}");
         });*/
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
            GameManager.Instance.localAccountData.level = Int32.Parse(result2.FunctionResult.ToString());
            UIManager.Instance.ActualizeData();
            SceneManager.Instance.LoadingScene(SceneManager.SceneType.MENU);



        }, (PlayFabError error) =>
        {
            Debug.Log($"Opps Something went wrong: {error.GenerateErrorReport()}");
        });
    }
    #endregion

}
