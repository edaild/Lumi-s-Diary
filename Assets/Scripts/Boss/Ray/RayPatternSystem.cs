using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayPatternSystem : MonoBehaviour
{
    public EnemySystem _enemySystem;
    public int patternCount;

    [Header("패턴 진행시 오브젝트")]
    public GameObject rotateLightningObject;

    private bool isPatternTime;

    private void Start()
    {
        _enemySystem = Object.FindAnyObjectByType<EnemySystem>();
    }

    private void Update()
    {
        if (!_enemySystem.isDistance || _enemySystem.isPattern || _enemySystem == null || isPatternTime) return;
        RandomPattern();
    }

    private void RandomPattern()
    {
        patternCount = Random.Range(0, 4);
        Debug.Log($"현제 패턴 카운트 {patternCount}");

        switch(patternCount)
        {
            case 0:
                Debug.Log("돌진 공격");
                RatageLightning();
                break;
            case 1:
                Debug.Log("번개비");
                break;
            case 2:
                Debug.Log("회전 광성");
                RatageLightning();
                break;
            case 3:
                Debug.Log("즉사키");
                RatageLightning();
                break;
        }
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
        rotateLightningObject.gameObject.SetActive(true);

        float timer = 0f;
        while (timer < 10f)
        {

            rotateLightningObject.transform.Rotate(0, 0, rotateSpeed * Time.deltaTime);
            timer += Time.deltaTime;
            yield return null; 
        }
        Debug.Log("방향 전환");


        timer = 0f;
        while (timer < 10f)
        {
            rotateLightningObject.transform.Rotate(0, 0, -rotateSpeed * Time.deltaTime);
            timer += Time.deltaTime;
            yield return null;
        }

        rotateLightningObject.gameObject.SetActive(false);
        Debug.Log("회전 광선 패턴 종료 및 대기");

        yield return new WaitForSeconds(1f);

        _enemySystem.isPattern = false;
        isPatternTime = true;
        yield return new WaitForSeconds(30f);
        isPatternTime = false;
        Debug.Log("패턴 종료");
    }
}
