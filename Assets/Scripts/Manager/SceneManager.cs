using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static SceneManager instance;

    public static SceneManager Instance
    {

        get
        {
            if (instance == null)
            {
                instance = new GameObject("SceneManager").AddComponent<SceneManager>();
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

    private SceneType typeOfScene = SceneType.NONE;
    public enum SceneType
    {
        MENU,
        ROOM,
        NONE
    }


    public void LoadingScene(SceneType type)
    {
        typeOfScene = type;
        if(typeOfScene == SceneType.MENU)
        {
            UIManager.Instance.loginCanvas.SetActive(false);
            UIManager.Instance.menuCanvas.SetActive(true);
        }
    }



}
