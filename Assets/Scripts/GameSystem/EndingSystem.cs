using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
public class EndingSystem : MonoBehaviour
{
    public VideoPlayer endingVideio;

    private void Start()
    {
        endingVideio.time = 0;
        endingVideio.Play();
        StartCoroutine(EndingVidioTime());
    }

    IEnumerator EndingVidioTime()
    {
        yield return new WaitForSeconds(192.01f);
        endingVideio.Stop();
        SceneManager.LoadScene("LobbyScene");
    }
}
