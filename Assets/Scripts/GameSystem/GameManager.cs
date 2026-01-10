using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public GameMusicSystem gameMusicSystem;

    public static GameManager Instance;

    private bool isInstance;
    private void Awake()
    {

        if (Instance == null || !isInstance)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else 
            Destroy(Instance);  
    }

    private void Start()
    {
        if(!gameMusicSystem)
            gameMusicSystem = GetComponent<GameMusicSystem>();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (Instance != null)
        {
            isInstance = true;
        }

        if (scene.name == "LumiHouseScene" || scene.name == "LobbyScene" || scene.name == "MaigicurlHotel")
            Destroy(gameObject);

        Debug.Log($"현재 재생 음악: {gameMusicSystem.currentMusic}");


    }
}
