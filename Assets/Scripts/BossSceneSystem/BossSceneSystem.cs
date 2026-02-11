using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSceneSystem : MonoBehaviour
{
    public GameObject eixtScenePotal;
    public QuestSystem _questSystem;
    public bool isBossScene;

    private bool isClear;

    private void Start()
    {
        _questSystem =  Object.FindAnyObjectByType<QuestSystem>();
        eixtScenePotal.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (eixtScenePotal == null && !isClear && _questSystem == null && _questSystem.playerEnmeyDieCount != _questSystem.playerEnemyTargetCount) return;
        gameObject.SetActive(true);
    }
}
