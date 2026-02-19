using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameMusicSystem : MonoBehaviour
{
    public AudioSource audioSource;
    public List<AudioClip> audioClips = new List<AudioClip>();

    public string currentMusic;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += IsNotInGameScene;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= IsNotInGameScene;
    }

    private void IsNotInGameScene(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "LumiHouseScene" || scene.name == "Communitycenter" || scene.name == "LobbyScene" || scene.name == "MathScene"|| scene.name == "EndingScene")
        {
            audioSource.Stop();
            audioSource.clip = null;
            Debug.Log("현재씬 음악 시스탬 필요 여부 X");
        }
        else
        {
            ChackMusic();
        }
 
    }

    public void ChackMusic() 
    {
        string targetMusicName = "";

        string sceneName = SceneManager.GetActiveScene().name;
        Debug.Log($"현재씬: {sceneName}");

        if (sceneName == "Snowvillage")
        {
            targetMusicName = "눈의 마을";
        }
        else if(sceneName == "MaigicurlHotel")
        {
            targetMusicName = "매직컬센터";
        }
        else if(sceneName == "Melodya")
        {
            targetMusicName = "멜로디아";
        }
        else if (sceneName == "SnowVillageRoad" || sceneName == "SnowVillageRoad3")
        {
            targetMusicName = "눈의 마을 길";
        }
        else if (sceneName == "IcIcleCity")
        {
            targetMusicName = "아이시클 시티";
        }
        else if (sceneName == "RayBossScene")
        {
            targetMusicName = "레이 보스전";
        }
        else if(sceneName == "IcIcleCityPark1" || sceneName == "IcIcleCityParkSquare" || sceneName == "IcIcleCityPark2")
        {
            targetMusicName = "아이시클 시티 공원";
        }
        else
        {
            Debug.Log($"GameMusicSystem : {sceneName}씬이 존재하지 않습니다.");
        }

        if (string.IsNullOrEmpty(targetMusicName)) return;

        if (audioSource.clip == null || audioSource.clip.name != targetMusicName)
        {
            AudioClip clipToPlay = audioClips.Find(m => m.name == targetMusicName);

            if (clipToPlay != null)
            {
                audioSource.Stop();
                audioSource.clip = clipToPlay;
                audioSource.time = 0;

                audioSource.Play();
                Debug.Log($"{targetMusicName} 재생 시작");
                currentMusic = audioSource.clip.name;
            }
            else
            {
                Debug.LogWarning($"리스트에 미존재 '{targetMusicName}'");
            }
        }

    }
}
