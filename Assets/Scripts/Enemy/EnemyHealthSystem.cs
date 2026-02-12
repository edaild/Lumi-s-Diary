using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthSystem : MonoBehaviour
{
    public EnemySystem _enemySystem;
    public QuestSystem _questSystem;

    void Start()
    {
        if( _enemySystem == null )
        _enemySystem = GetComponent<EnemySystem>();
        _questSystem = Object.FindAnyObjectByType<QuestSystem>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {

            collision.gameObject.TryGetComponent<SkillBall>(out SkillBall ball);

            Debug.Log(_enemySystem._currentHealth);
            if (!_enemySystem._characterSkillSystem.isCrystarGarden)
                _enemySystem._currentHealth -= ball.BallDamage;
            else
            {
                int isCrystarGardDamage = ball.BallDamage += 100;
                _enemySystem._currentHealth -= isCrystarGardDamage;
                Debug.Log("크리스탈 가든 버프 적용");
            }
            Debug.Log($"몬스터 체력: {_enemySystem._currentHealth}");
            Destroy(collision.gameObject);

            if (_enemySystem._currentHealth <= 0)
                DIe();
            else
                return;
        }
    }

    void DIe()
    {
        
        Destroy(_enemySystem.gameObject);
        Debug.Log($"{_enemySystem._EnemyName} 처치 완료. 루나 {_enemySystem._GiftCoin} 만큼 증가");

        if(_questSystem.playerTargetName == _enemySystem._EnemyName)
            _enemySystem._questSystem.playerEnmeyDieCount++;
    }
}
