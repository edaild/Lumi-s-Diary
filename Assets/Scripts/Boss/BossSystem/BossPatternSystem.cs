using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPatternSystem : MonoBehaviour
{
    public BossPatternSO bossPatternSO;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log($"현재 보스 이름: {bossPatternSO.BossName}, 현재 보스 패턴: {bossPatternSO.BossPatternName}, 데미지: {bossPatternSO.BossPatternDamege}");
        }
    }
}
