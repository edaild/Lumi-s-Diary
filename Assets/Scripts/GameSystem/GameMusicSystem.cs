using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameMusicSystem : MonoBehaviour
{
    public AudioSource audioSource;
    public List<AudioClip> audioClips = new List<AudioClip>();

    public string currentMusic;

    private void Update()
    {
        ChackMusic();
    }

    private void ChackMusic() 
    {
        string targetMusicName = "";

        string sceneName = SceneManager.GetActiveScene().name;
        Debug.Log($"현재씬: {sceneName}");

        if (sceneName == "Snowvillage" || sceneName == "SnowVillageRoad3" || sceneName == "SnowVillageRoad" || sceneName == "MathScene" || sceneName == "Communitycenter")
        {
            targetMusicName = "눈의 마을";
        }
        else if(sceneName == "IcIcleCity")
        {
            targetMusicName = "아이시클 시티";
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
