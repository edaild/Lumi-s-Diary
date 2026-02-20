using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BossUISystem : MonoBehaviour
{
    public TextMeshProUGUI currentBossHealthText;
    public EnemySystem _enemySystem;

    private void Start()
    {
        _enemySystem = Object.FindAnyObjectByType<EnemySystem>();
    }

    private void Update()
    {
        currentBossHealthText.text = $"{_enemySystem._currentHealth}";
    }
}
