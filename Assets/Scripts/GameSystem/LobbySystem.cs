using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using JetBrains.Annotations;

public class LobbySystem: MonoBehaviour
{
    public Button GameStartButton;
    public GameObject Logo;
    public FadeManager fade;
    public VideoPlayer videoPlayer;
    public VideoClip Lobbyclip;
    public VideoClip OPclip;

    public AudioSource LobbyMusic;


    private void Start()
    {
        if(!fade)
            fade = GetComponent<FadeManager>();

        GameStartButton.onClick.AddListener(GameStart);
    }

    void GameStart()
    {
        if (!videoPlayer) return;

        if (Logo || GameStartButton)
        {
            Logo.gameObject.SetActive(false);
            GameStartButton.gameObject.SetActive(false);
        }

        if (!OPclip) return;

       videoPlayer.clip = OPclip;

       videoPlayer.Play();
       StartCoroutine(SceneManagerments());
    
    
    }

    IEnumerator SceneManagerments()
    {
        yield return new WaitForSeconds(8f);
        videoPlayer.Stop();
        fade.StartFadeOut(1.5f);
        yield return new WaitForSeconds(8.5f);
        SceneManager.LoadScene("LumiHouseScene");
    }
}
