using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Windows;

public class PotalSystem : MonoBehaviour
{
    [Header("루미집 포탈 관리")]
    public Transform Potar01;
    public Transform Potar02;
    public Transform Potar03;
    public VariableJoystick jay;

    public GameObject currentPortal;

    void Update()
    {
        if (currentPortal != null && jay.Vertical > 0.7f || UnityEngine.Input.GetKeyDown(KeyCode.UpArrow))
            Teleport();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.CompareTag("housePotar01") || collision.CompareTag("housePotar02") || collision.CompareTag("housePotar03")) && SceneManager.GetActiveScene().name == "LumiHouseScene")
        {
            currentPortal = collision.gameObject;
        }
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
                transform.position = Potar02.transform.position;
            else if (currentPortal.CompareTag("housePotar02"))
                transform.position = Potar01.transform.position;
            else if (currentPortal.CompareTag("housePotar03"))
                SceneManager.LoadScene("GameScene");
        }
       
    }
}


