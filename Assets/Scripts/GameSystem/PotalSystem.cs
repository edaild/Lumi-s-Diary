using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PortalSystem : MonoBehaviour
{
    public PortalSO portalData;
    public TextMeshProUGUI portalName;


    private void Start()
    {
        portalName.text = portalData.portalName;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            Debug.Log($"현제 충돌한 포탈  이름 :{portalData.portalName}");
    }
}
