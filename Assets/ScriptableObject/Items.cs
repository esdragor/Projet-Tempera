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
    public TypeOfItems typeOfItems;
    public int levelRequiered;
    public int timeRequiered;
    public int costEnergy;
    public int costGold;
    public int costGems;
    public int sellGold;
    public int buyGold;


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

    /* public static int SetTypeOfItems(string name)
    {
        if (name.Equals("Construction"))
            return TypeOfItems.Construction;
        if (name.Equals("Craft"))
            return TypeOfItems.Craft;
        if (name.Equals("Matiere"))
            return TypeOfItems.Matiere;

        return TypeOfItems.None;
    }*/
}
