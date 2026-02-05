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

    public Transform startPoition;
    private GameObject player;
    private bool isPlayer;
    private bool isPlayerAttack;
    private bool isBackPoition;
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

        startPoition.position = transform.position;
    }

    private void Update()
    {
        if (isFollowing && player != null)
        {
            FollowPlayer();
        }

        if(isBackPoition && player != null)
        {
            BackPoition();
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

        if (other.gameObject.CompareTag("Navi"))
        {
            isFollowing = false;
            isBackPoition = true;
            Debug.Log("Enemy 이동 제한 구간 진입");
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

        if (!_storySystem || !_storySystem.isStoryEndPoint || isPlayer || isBackPoition) return;
        float Distance = 2f;

        float currentDistance = Vector2.Distance(targetPlayer.position, transform.position);

        if (currentDistance > Distance)
        {
            Vector2 targetDirection = (targetPlayer.position - transform.position).normalized;
            float enemyMoveX = targetDirection.x * _EnemySpeed * Time.deltaTime;
            transform.position = new Vector3(transform.position.x + enemyMoveX, transform.position.y, 0);
        }
    }

    void BackPoition()
    {
        Vector2 targetBackDirection = (startPoition.position - transform.position).normalized;
        float enemyMoveX = targetBackDirection.x * _EnemySpeed * Time.deltaTime;
        transform.position = new Vector3(transform.position.x + enemyMoveX, transform.position.y, 0);

        if (transform.position == startPoition.position)
            isBackPoition = false;
    }
}
