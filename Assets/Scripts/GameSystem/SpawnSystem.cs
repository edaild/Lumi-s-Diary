using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnSystem : MonoBehaviour
{
    public static SpawnSystem instance;
    public GameObject playerPrefab;
    public List<Transform> spawnPont = new List<Transform>();
    public QuestSystem questSystem;
    public bool isPlayer;

    private bool isNullObjectScene;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            gameObject.SetActive(false);
    }

    private void Start()
    {
        //PlayerSapwn();

        if(questSystem == null)
            questSystem = GetComponent<QuestSystem>();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += IsNotInGameScene;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= IsNotInGameScene;
    }

    void IsNotInGameScene(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "LumiHouseScene" || scene.name == "MaigicurlHotal" || scene.name == "LobbyScene")
            Destroy(gameObject);
      
    }

    //void PlayerSapwn()
    //{
    //    string isSpawnPoint = "";

    //    if(questSystem.playerquerstID <= 10005 || questSystem.playerLevel <= 2 || SceneManager.GetActiveScene().name == "Snowvillage" || !isPlayer)
    //    {
    //        isSpawnPoint = "눈의 마을 스폰 포인트";
    //        Debug.Log("눈의 마을 스폰");
    //    }

    //    if (questSystem.playerquerstID >= 10005 || questSystem.playerLevel >= 2)
    //    {
    //        isSpawnPoint = "아이시클 시티";
    //        Debug.Log("아이시클 시티 스폰 포인트");
    //    }

    //    if (string.IsNullOrEmpty(isSpawnPoint)) return;

    //    Transform targetSpawnPoint = spawnPont.Find(s => s.name == isSpawnPoint);

    //    if(targetSpawnPoint != null)
    //    {
    //        Instantiate(playerPrefab,targetSpawnPoint.transform.position, Quaternion.identity);
    //        Debug.Log($"{targetSpawnPoint} 으로 플레이어 생성");
    //        isPlayer = true;
    //    } 

    //}
}
