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

        if (scene.name == "LumiHouseScene" || scene.name == "MaigicurlHotel" || scene.name == "LobbyScene" || scene.name == "Communitycenter" || scene.name == "EndingScene" || scene.name == "IcIcleCityLibrary")
        {
            isPlayer = false;
            return;
        }

        spawnPont.Clear();
        GameObject[] points = GameObject.FindGameObjectsWithTag("SpawnPoint");

        foreach (var p in points)
            spawnPont.Add(p.transform);

        PlayerSapwn();
    }

    void PlayerSapwn()
    {
        if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            isPlayer = true;
            return;
        }

        string currentScene = SceneManager.GetActiveScene().name;
        string targetPointName = "";

        if (currentScene == "IcIcleCity")
            targetPointName = "아이시클 시티 스폰 포인트";

        else if (currentScene == "Snowvillage")
            targetPointName = "눈의 마을 스폰 포인트";

        else if (currentScene == "IcIcleCityQuoin")
            targetPointName = "아이시클 시티 외각  스폰 포인트";

        if (string.IsNullOrEmpty(targetPointName)) return;

        Transform targetSpawnPoint = spawnPont.Find(s => s.name == targetPointName);

        if (targetSpawnPoint != null)
        {
            Instantiate(playerPrefab, targetSpawnPoint.position, Quaternion.identity);
            isPlayer = true;
        }
        else
            Debug.LogError($"{targetPointName} 존재 X");
    }
}
