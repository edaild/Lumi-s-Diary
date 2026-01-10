using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
//using UnityEngine.Windows;

[CreateAssetMenu()]

public class LumiPortalSystem : MonoBehaviour
{
    [Header("루미집 포탈 관리")]
    public Transform Portar01;
    public Transform Portar02;
    public Transform Portar03;
    public VariableJoystick jay;
    public FadeManager FadeManager;
    public GameObject currentPortal;
    
    public bool isPortalMove;

    void Update()
    {
        if (!isPortalMove && (currentPortal != null && jay.Vertical > 0.7f || (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))))
        {
            Teleport();
            isPortalMove = true;
        }
 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 루미의 집
        if ((collision.CompareTag("housePotar01") || collision.CompareTag("housePotar02") || collision.CompareTag("housePotar03")) && SceneManager.GetActiveScene().name == "LumiHouseScene")
        {
            Debug.Log("현재씬 : 루미의 집");
            currentPortal = collision.gameObject;
   
            StartCoroutine(PortalMoveRunTime());
        }
    }

    IEnumerator PortalMoveRunTime()
    {
        yield return new WaitForSeconds(1.5f);
        isPortalMove = false;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        currentPortal = null;
    }

    void Teleport()
    {
        if(SceneManager.GetActiveScene().name == "LumiHouseScene")
        {
            if (currentPortal.CompareTag("housePotar01"))
                transform.position = Portar02.transform.position;
            else if (currentPortal.CompareTag("housePotar02"))
                transform.position = Portar01.transform.position;
            else if (currentPortal.CompareTag("housePotar03"))
            {
                FadeManager.StartFadeOut(1.5f);
                SceneManager.LoadScene("SnowVillage");
            }
                
        }
       
    }
}


