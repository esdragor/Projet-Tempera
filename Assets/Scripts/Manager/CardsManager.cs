using System.Collections;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using PlayFab.CloudScriptModels;
using UnityEngine;

public class CardsManager : MonoBehaviour
{

    private static CardsManager m_instance;

    public static CardsManager instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = new GameObject("CardsManager").AddComponent<CardsManager>();
            }

            return m_instance;

        }
        private set { }
    }

    public List<Cards> cardsList = new List<Cards>();

    private void Awake()
    {
        if (m_instance == null)
        {
            m_instance = this;
            DontDestroyOnLoad(gameObject);
        }

    }
    // Start is called before the first frame update


    public void GenerateCards(CatalogItem cardsData)
    {
        Cards cards = new Cards();
        cards.id = cardsData.ItemId;
        cards.itemsName = cardsData.DisplayName;
        cards.itemDescription = cardsData.Description;
        cardsList.Add(cards);
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
