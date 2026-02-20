using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayPatternSystem : MonoBehaviour
{
    public EnemySystem _enemySystem;
    public int patternCount;

    [Header("패턴 진행시 오브젝트")]
    public GameObject rightrotateLightningObject;
    public GameObject leftrotateLightningObject;

    public List<GameObject> lightningRainObejct = new List<GameObject>();

    private bool isPatternTime;

    private void Start()
    {
        _enemySystem = UnityEngine.Object.FindAnyObjectByType<EnemySystem>();
    }

    private void Update()
    {
        if (!_enemySystem.isDistance || _enemySystem.isPattern || _enemySystem == null || isPatternTime) return;
        RandomPattern();
    }

    private void RandomPattern()
    {
        patternCount = UnityEngine.Random.Range(0, 5);
        Debug.Log($"현제 패턴 카운트 {patternCount}");

        switch(patternCount)
        {
            case 0:
                Debug.Log("텔레포트");
                Teleport();
                break;
            case 1:
                Debug.Log("번개비1");
                lightningRain01();
                break;
            case 2:
                Debug.Log("번개비2");
                lightningRain02();
                break;

            case 3:
                Debug.Log("회전 광성");
                RatageLightning();
                break;
            case 4:
                Debug.Log("즉사키");
                Teleport();
                break;
        }
    }


    private void Teleport()
    {
        _enemySystem.isPattern = true;
        Debug.Log("텔레포트 패턴 진행");

        _enemySystem.EnemyAnimator.SetBool("isTelePort", true);
        Transform tartargetPlayere = _enemySystem.player.gameObject.transform;
        StartCoroutine(IsTeleport(tartargetPlayere));
    }

    IEnumerator IsTeleport(Transform tartargetPlayere)
    {
        yield return new WaitForSeconds(0.5f);
        _enemySystem.gameObject.transform.position = tartargetPlayere.position;
        yield return new WaitForSeconds(1f);
        _enemySystem.EnemyAnimator.SetBool("isTelePort", false);
        StartCoroutine(IsPattern());
    }


    void lightningRain01()
    {
        _enemySystem.isPattern = true;
        Debug.Log("번개비1 패턴 진행");

        int luckyNumber = UnityEngine.Random.Range(0, 2);

        for (int i = 0; i < lightningRainObejct.Count; i++)
        {
            GameObject light = lightningRainObejct[i];

            if (light != null)
            {
                if (i % 2 == luckyNumber)
                    light.SetActive(true);
                else
                    light.SetActive(false);
                
                StartCoroutine(lightningRainTime(light));
            }
        }
    }

    void lightningRain02()
    {
        _enemySystem.isPattern = true;
        Debug.Log("번개비2 패턴 진행");
        StartCoroutine(PlaylightningRain02());
    }

    IEnumerator PlaylightningRain02()
    {
        Debug.Log("1번부터 8번까지 패턴 진행");
        for (int i = 0; i < lightningRainObejct.Count; i++)
        {
           
            GameObject light = lightningRainObejct[i];
            if (light != null)
            {
                light.SetActive(true);
                StartCoroutine(lightningRainTime(light));
                yield return new WaitForSeconds(0.2f);
            }
        }
     
        yield return new WaitForSeconds(1f);
        Debug.Log(" 8번부터 1번까지 패턴 진행");
        for (int i = 0; i > lightningRainObejct.Count; i--)
        {
 
            GameObject light = lightningRainObejct[i];
            if (light != null)
            {
                light.SetActive(true);
                StartCoroutine(lightningRainTime(light));
                yield return new WaitForSeconds(0.2f);
            }
        }
    }

    IEnumerator lightningRainTime(GameObject light)
    {
        yield return new WaitForSeconds(1f);
        light.gameObject.SetActive(false);
        StartCoroutine(IsPattern());
    }

    private void RatageLightning()
    {
        _enemySystem.isPattern = true;
        Debug.Log("회전 광선 패턴 진행");
        StartCoroutine(RatageLightningTime());
    }

    IEnumerator RatageLightningTime()
    {
        yield return new WaitForSeconds(1f);
        float rotateSpeed = 10f;
        rightrotateLightningObject.gameObject.SetActive(true);
        leftrotateLightningObject.gameObject.SetActive(true);

        float timer = 0f;
        while (timer < 10f)
        {
            rightrotateLightningObject.transform.Rotate(0, 0, rotateSpeed * Time.deltaTime);
            leftrotateLightningObject.transform.Rotate(0, 0, -rotateSpeed * Time.deltaTime);
            timer += Time.deltaTime;
            yield return null; 
        }
        Debug.Log("방향 전환");


        timer = 0f;
        while (timer < 10f)
        {
            rightrotateLightningObject.transform.Rotate(0, 0, -rotateSpeed * Time.deltaTime);
            leftrotateLightningObject.transform.Rotate(0, 0, rotateSpeed * Time.deltaTime);
            timer += Time.deltaTime;
            yield return null;
        }

        rightrotateLightningObject.gameObject.SetActive(false);
        leftrotateLightningObject.gameObject.SetActive(false);
        Debug.Log("회전 광선 패턴 종료 및 대기");

        yield return new WaitForSeconds(1f);
        StartCoroutine(IsPattern());
    }


    

    IEnumerator IsPattern()
    {
       _enemySystem.isPattern = false;
       isPatternTime = true;
       yield return new WaitForSeconds(10f);
       isPatternTime = false;
       Debug.Log("패턴 종료");
    }
}
