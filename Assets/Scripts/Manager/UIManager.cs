using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public static UIManager Instance
    {

        get
        {
            if (instance == null)
            {
                instance = new GameObject("UIManager").AddComponent<UIManager>();
                DontDestroyOnLoad(instance);
            }
            return instance;
        }
        private set { }
    }

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
    }
    // Start is called before the first frame update

    [Header("PANNEL GAME")]
    public GameObject loginCanvas;
    public GameObject menuCanvas;
    public GameObject customCanvas;

    [Header("MESSAGE GAME")]
    public Text textConnexion;

    [Header("DATA GAME")]

    public Text levelText;
    public Text nameText;
    public Text goldText;
    public Text gemsText;
    public Text energyText;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    public void ActualizeData()
    {
        levelText.text ="Level "+GameManager.Instance.localAccountData.level.ToString();
        nameText.text = GameManager.Instance.localAccountData.GetUsername();
        goldText.text = GameManager.Instance.localAccountData.gold.ToString();
        gemsText.text = GameManager.Instance.localAccountData.gems.ToString();
        energyText.text = GameManager.Instance.localAccountData.energy.ToString();
        
    }


    public void ChangeStateMessageConnexion(int state)
    {
        UIManager.Instance.textConnexion.gameObject.SetActive(true);
        if (state == 1)
        {
            UIManager.Instance.textConnexion.text = " Connexion en cours !";
        }
        else if (state == 2)
        {
            UIManager.Instance.textConnexion.text = " Connexion réussie !";
        }
        else if (state == 3)
        {
            UIManager.Instance.textConnexion.text = " Connexion refusée !";
        }
    }
}
