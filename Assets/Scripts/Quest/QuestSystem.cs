using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuestSystem : MonoBehaviour
{
    public static QuestSystem instance;
    public QuestDataSO questData;

    public int playerquerstID;
    public string playerquestName;
    public bool playerquest_Is_success;
    public int playerPreQuestID;


    public int playerLevel = 1;
    public int playerExperience = 0;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += IsNotUseGameObjerct;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= IsNotUseGameObjerct;
    }

    void IsNotUseGameObjerct(Scene scene, LoadSceneMode mode)
    {
        if(scene.name == "LumiHouseScene" || scene.name == "LobbyScene")
        Destroy(gameObject);
    }

    private void Start()
    {
        Debug.Log("¹Ì±¸Çö");
    }
}
