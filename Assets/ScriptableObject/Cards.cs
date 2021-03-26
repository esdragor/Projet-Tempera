using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Cards", menuName = "Cards", order = 51)]
public class Cards : ScriptableObject
{

    public enum TypeOfElements
    {
        FIRE,
        WATER,
        EARTH,
        AIR,
        NONE
    }

    public string id;
    public string itemsName;
    public string itemDescription;
    public string urlImg;
    public string rarity;
    public TypeOfElements firstElement;
    public TypeOfElements secondElement;
    public string level;
    public List<int> deckPowerLevel;
    public List<int> attackLevel;
    public List<int> defenseLevel;
    public List<int> manaCostLevel;
    public List<int> rechargeLevel;

    public static TypeOfElements SetTypeOfElements(string name)
    {
        if (name.Equals("Air"))
            return TypeOfElements.AIR;
        if (name.Equals("Fire"))
            return TypeOfElements.FIRE;
        if (name.Equals("Water"))
            return TypeOfElements.WATER;
        if (name.Equals("Earth"))
            return TypeOfElements.EARTH;

        return TypeOfElements.NONE;
    }

    public static string[] ParseCustomData(string data)
    {
        string[] separator = { ",", ":" };
        int count = CountStringOccurrences(data, ",");
        string[] result = data.Split(separator, count, System.StringSplitOptions.RemoveEmptyEntries);
        for (int i = 0; i < result.Length; i++)
        {
            result[i] = result[i].Replace("{", "");
            result[i] = result[i].Replace("\"", "");
            result[i] = result[i].Replace("}", "");
        }

        return result;
    }

    private static int CountStringOccurrences(string text, string pattern)
    {
        // Loop through all instances of the string 'text'.
        int count = 2;
        int i = 0;
        while ((i = text.IndexOf(pattern, i)) != -1)
        {
            i += pattern.Length;
            count++;
        }
        return count * 2;
    }



}
