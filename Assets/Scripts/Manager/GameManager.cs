using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update

    public static GameManager instance;

    public static GameManager Instance
    {

        get
        {
            if (instance == null)
            {
                instance = new GameObject("GameManager").AddComponent<GameManager>();
                DontDestroyOnLoad(instance);
            }
            return instance;
        }
        private set { }
    }

    public AccountData localAccountData;
   // public List<Items> itemsList;
    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        Debug.Log("Coucou");
    }

    // Update is called once per frame
    void Update()
    {

    }
}
