using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DataAccount", menuName = "Account", order = 51)]
public class AccountData : ScriptableObject
{
    public string idPlayfab;
    public string username;
    public string mail;

    public int gold;

    public int energy;

    public int gems;

    public int level;


    public void SetUsername(string _username)
    {
        username = _username;
    }

    public string GetUsername()
    {
        return username;
    }


    public void SetID(string _ID)
    {
        idPlayfab = _ID;
    }

    public string GetID()
    {
        return idPlayfab;
    }


    public void SetMail(string _mail)
    {
        mail = _mail;
    }

    public string GetMail()
    {
        return mail;
    }


}
