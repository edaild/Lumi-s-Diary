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
    private bool isPlayerAttackTime;
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
        AttackCollider.gameObject.SetActive(false);
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
            AttackPlayer();
        }

        if (other.gameObject.CompareTag("Navi"))
        {
            isFollowing = false;
            isPlayerAttack = false;
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

        if (AttackCollider.gameObject.CompareTag("Player"))
        {
            isPlayerAttack = false;
        }
    }

    IEnumerator StopFollowingAfterDelay()
    {
        yield return new WaitForSeconds(8f);
        EnemyAnimator.SetBool("isMove", false);
        isFollowing = false;
        stopCoroutine = null;
        Debug.Log("추적 중지");
    }

    void FollowPlayer()
    {
        Transform targetPlayer = player.gameObject.transform;

        if (!_storySystem || !_storySystem.isStoryEndPoint || isPlayer || isBackPoition) return;

        float Distance = 0.5f;

        float currentDistance = Vector2.Distance(targetPlayer.position, transform.position);

        if (currentDistance > Distance)
        {
            Vector2 targetDirection = (targetPlayer.position - transform.position).normalized;
            float enemyMoveX = targetDirection.x * _EnemySpeed * Time.deltaTime;
            transform.position = new Vector3(transform.position.x + enemyMoveX, transform.position.y, 0);
            EnemyAnimator.SetBool("isMove", true);
            if (targetDirection.x > 0) Flip(true);
            else Flip(false);
        }
    }

    void Flip(bool shouldFlip)
    {
        Vector3 scale = transform.localScale;

        if (shouldFlip)
            scale.x = -Mathf.Abs(scale.x);
        else
            scale.x = Mathf.Abs(scale.x);

        transform.localScale = scale;
    }

    void AttackPlayer()
    {
        if (isPlayerAttackTime) return;

        isPlayerAttackTime = true;
        StartCoroutine(AttackPlayerTime());
    }
    IEnumerator AttackPlayerTime()
    {
        yield return new WaitForSeconds(0.5f);
        AttackCollider.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        AttackCollider.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        isPlayerAttackTime = false;
    }
}
