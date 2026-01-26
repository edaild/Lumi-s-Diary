using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ReworImageSystem : MonoBehaviour
{
    public GameObject ImageType01;
    public GameObject ImageType02;
    public GameObject ImageType03;
    public FadeManager fadeManager;
    public int isImage;

    private void Start()
    {
        isImage = Random.Range(0, 3);
       
        if(isImage == 0)
            ImageType01.gameObject.SetActive(true);
        else if(isImage == 1)
            ImageType02.gameObject.SetActive(true);
        else if(isImage ==2)
            ImageType03.gameObject.SetActive(true);

            StartCoroutine(IngameIn());
        
    }

   IEnumerator IngameIn()
    {
        yield return new WaitForSeconds(5f);
        fadeManager.StartFadeOut(1.5f);
        SceneManager.LoadScene("LumiHouseScene");
    }
}
