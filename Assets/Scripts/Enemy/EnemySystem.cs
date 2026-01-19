using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing.Design;
using UnityEngine;

public class EnemySystem : MonoBehaviour
{
    public EnmeySO enmeySO;

    public int _EnemyID;
    public string _EnemyName;
    public int _EnemyLevel;
    public int _EnemyHealth;
    public int _EnemyDamage;
    public float _EnemySpeed;
    public int _GiftCoin;

    private void Start()
    {
        EnemyData Enemy = enmeySO.Enemys.Find(e => e.EnemyID == _EnemyID);

        _EnemyID = Enemy.EnemyID;
        _EnemyName = Enemy.EnemyName;
        _EnemyLevel = Enemy.EnemyLevel;
        _EnemyHealth = Enemy.EnmeyHP;
        _EnemyDamage = Enemy.EnemyDamage;
        _EnemySpeed = Enemy.EnemySpeed;
        _GiftCoin = Enemy.GiftCoin;


        Debug.Log($"현재 몬스터 아이디: {_EnemyID}, 이름: {_EnemyName}");
    }
   
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {

            collision.gameObject.TryGetComponent<SkillBall>(out SkillBall ball);

            int Health = _EnemyHealth;
            Debug.Log(Health);


            Health -= ball.BallDamage;
            Debug.Log(Health);
            Destroy(collision.gameObject);

            if (Health <= 0)
                DIe();
            else
                return;
            
        }
    }


    void DIe()
    {
        Destroy(gameObject);
        Debug.Log($"{_EnemyName} 처치 완료. 루나 {_GiftCoin} 만큼 증가");
    }
}
