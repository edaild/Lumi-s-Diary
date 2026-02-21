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

        // 눈의 마을 지역----------------------------------------------------------------
        if (sceneName == "Snowvillage") targetMusicName = "눈의 마을";

        else if (sceneName == "SnowVillageRoad" || sceneName == "SnowVillageRoad3" || sceneName == "SnowVillageRoad2" || sceneName == "SnowVillageRoad1") targetMusicName = "눈의 마을 길";

        // 아이시클 시티 지역------------------------------------------------------------
        else if (sceneName == "IcIcleCity" || sceneName == "IcIcleCityQuoin") targetMusicName = "아이시클 시티";

        else if (sceneName == "IcIcleCityPark1" || sceneName == "IcIcleCityParkSquare" || sceneName == "IcIcleCityPark2") targetMusicName = "아이시클 시티 공원";

        else if (sceneName == "IcIcleCityLibrary") targetMusicName = "아이시클 시티 도서관";

        else if (sceneName == "IcIcleStadiumSquare") targetMusicName = "아이시클 스타디움 광장";

        else if (sceneName == "IcIcleStadium") targetMusicName = "아이시클 스타디움";

        else if (sceneName == "MaigicurlHotel") targetMusicName = "매직컬센터";

        // 멜로디아 지역------------------------------------------------------------------
        else if (sceneName == "Melodya") targetMusicName = "멜로디아";

      // 보스전---------------------------------------------------------------------------
        else if (sceneName == "RayBossScene") targetMusicName = "레이 보스전";

        else Debug.Log($"GameMusicSystem : {sceneName}씬 미존재.");

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
