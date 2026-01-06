using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameMusicSystem : MonoBehaviour
{
    public AudioSource audioSource;
    public List<AudioClip> audioClips = new List<AudioClip>();

    private void Update()
    {
        ChackMusic();
    }

    private void ChackMusic() 
    {
        string targetMusicName = "";


        string sceneName = SceneManager.GetActiveScene().name;
        if (sceneName == "SnowVillage" || sceneName == "SnowVillageRoad3" || sceneName == "SnowVillageRoad")
        {
            targetMusicName = "눈의 마을";
        }

        if (string.IsNullOrEmpty(targetMusicName)) return;

        if (audioSource.clip == null || audioSource.clip.name != targetMusicName)
        {
            AudioClip clipToPlay = audioClips.Find(m => m.name == targetMusicName);

            if (clipToPlay != null)
            {
                audioSource.Stop();
                audioSource.clip = clipToPlay;
                audioSource.Play();
                Debug.Log($"{targetMusicName} 재생 시작");
            }
            else
            {
                Debug.LogWarning($"리스트에 미존재 '{targetMusicName}'");
            }
        }

    }
}
