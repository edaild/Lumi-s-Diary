using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public bool isInGame;

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
        isInGame = true;

        if (Instance != null)
            isInstance = true;

        if (scene.name == "LumiHouseScene" || scene.name == "LobbyScene" || scene.name == "MaigicurlHotel" || scene.name == "Communitycenter")
        {
            isInGame = false;
            Destroy(gameObject);
        }
           
    }
}
