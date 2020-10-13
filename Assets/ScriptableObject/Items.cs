using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Items", menuName = "Items", order = 51)]
public class Items : ScriptableObject
{

    public enum TypeOfItems
    {
        Construction,
        Craft,
        Matiere,
        Interagir,
        None
    }

    public enum TypeOfMetiers
    {
        Mineur,
        Forgeron,
        Bucheron,
        None
    }

    public string id;
    public string itemsName;
    public string itemDescription;
    public string urlImg;
    public TypeOfMetiers itemMetier;
    public List<TypeOfItems> typeOfItems;
    public int levelRequiered;
    public int timeRequiered;
    public int costEnergy;
    public int costGold;
    public int costGems;
    public int sellGold;
    public int buyGold;
    public bool isStackable;
    public string idItemNecessary;


    public static TypeOfItems SetTypeOfItems(string name)
    {
        if (name.Equals("Construction"))
            return TypeOfItems.Construction;
        if (name.Equals("Craft"))
            return TypeOfItems.Craft;
        if (name.Equals("Matiere"))
            return TypeOfItems.Matiere;

        return TypeOfItems.None;
    }

    public static TypeOfMetiers SetTypeOfMetier(string name)
    {
        if (name.Equals("Mineur"))
            return TypeOfMetiers.Mineur;
        if (name.Equals("Forgeron"))
            return TypeOfMetiers.Forgeron;
        if (name.Equals("Bucheron"))
            return TypeOfMetiers.Bucheron;

        return TypeOfMetiers.None;
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
