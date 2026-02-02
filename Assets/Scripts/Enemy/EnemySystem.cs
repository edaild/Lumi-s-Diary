using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing.Design;
using UnityEngine;
using TMPro;

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

    public int _currentHealth;
    public TextMeshProUGUI EnemyName;
    public Animator EnemyAnimator;

    public CharacterSkillSystem _characterSkillSystem;
    public QuestSystem _questSystem;
    public StorySystem _storySystem;

    public GameObject AttackCollider;

    private GameObject player;
    private bool isPlayer;
    private bool isPlayerAttack;

    private bool isFollowing = false;
    private Coroutine stopCoroutine;

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

        _characterSkillSystem = UnityEngine.Object.FindAnyObjectByType<CharacterSkillSystem>();
        _questSystem = UnityEngine.Object.FindAnyObjectByType<QuestSystem>();
        _storySystem = UnityEngine.Object.FindAnyObjectByType<StorySystem>();
        _currentHealth = _EnemyHealth;

        player = _characterSkillSystem.gameObject;
        EnemyName.text = _EnemyName;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {

            collision.gameObject.TryGetComponent<SkillBall>(out SkillBall ball);

            Debug.Log(_currentHealth);
            if (!_characterSkillSystem.isCrystarGarden)
                _currentHealth -= ball.BallDamage;
            else
            {
                int isCrystarGardDamage = ball.BallDamage += 100;
                _currentHealth -= isCrystarGardDamage;
                Debug.Log("크리스탈 가든 버프 적용");
            }
            Debug.Log(_currentHealth);
            Destroy(collision.gameObject);

            if (_currentHealth <= 0)
                DIe();
            else
                return;
        }
    }

    void DIe()
    {
        _questSystem.playerEnmeyDieCount++;
        Destroy(gameObject);
        Debug.Log($"{_EnemyName} 처치 완료. 루나 {_GiftCoin} 만큼 증가");
    }

    private void Update()
    {
        if (isFollowing && player != null)
        {
            FollowPlayer();
        }
    }


    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (stopCoroutine != null)
            {
                StopCoroutine(stopCoroutine);
                stopCoroutine = null;
            }
            isFollowing = true;
        }
            
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") || player != null)
        {
            if (stopCoroutine != null) StopCoroutine(stopCoroutine);
            stopCoroutine = StartCoroutine(StopFollowingAfterDelay());
        }
    }

    IEnumerator StopFollowingAfterDelay()
    {
        yield return new WaitForSeconds(8f);

        isFollowing = false;
        stopCoroutine = null;
        Debug.Log("추적 중지");
    }

    void FollowPlayer()
    {
        Transform targetPlayer = player.gameObject.transform;

        if (!_storySystem || !_storySystem.isStoryEndPoint || isPlayer) return;
        float Distance = 2f;

        float currentDistance = Vector2.Distance(targetPlayer.position, transform.position);

        //if (currentDistance > Distance)
        //{
            Vector2 targetDirection = (targetPlayer.position - transform.position).normalized;
            float enemyMoveX = targetDirection.x * _EnemySpeed * Time.deltaTime;
            transform.position = new Vector3(transform.position.x + enemyMoveX, transform.position.y, 0);
        //}
    }
}
