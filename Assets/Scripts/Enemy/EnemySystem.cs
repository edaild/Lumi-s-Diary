using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing.Design;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

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
    public CharacterHealthSystem _characterHealthSystem;
    public QuestSystem _questSystem;
    public StorySystem _storySystem;
    public GameObject player;
    public bool isPlayerAttack;
    public bool isDistance;
    public bool isPattern;
    public bool isStopPattern;

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
        _characterHealthSystem = UnityEngine.Object.FindAnyObjectByType<CharacterHealthSystem>();
        _currentHealth = _EnemyHealth;

        player = _characterSkillSystem.gameObject;
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

        if (other.gameObject.CompareTag("Navi"))
        {
            isFollowing = false;
            isPlayerAttack = false;
            Debug.Log("Enemy 이동 제한 구간 진입");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other != null && other.gameObject.CompareTag("Player"))
        {
            if (stopCoroutine != null) StopCoroutine(stopCoroutine);
            if (gameObject.activeInHierarchy)
            {
                stopCoroutine = StartCoroutine(StopFollowingAfterDelay());
            }
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

        if (!_storySystem || !_storySystem.isStoryEndPoint || isBackPoition || isStopPattern || _characterSkillSystem.isCrystarGarden) return;

        float Distance = 1f;

        float currentDistance = Vector2.Distance(targetPlayer.position, transform.position);

        if (currentDistance > Distance)
        {
            isDistance = true;
            Vector2 targetDirection = (targetPlayer.position - transform.position).normalized;
            float enemyMoveX = targetDirection.x * _EnemySpeed * Time.deltaTime;
            transform.position = new Vector3(transform.position.x + enemyMoveX, transform.position.y, 0);
            EnemyAnimator.SetBool("isMove", true);

            if (targetDirection.x > 0) Flip(true);
            else Flip(false);
        }
        else
        {
            isDistance = false;
            EnemyAnimator.SetBool("isMove", false);
            AttackPlayer();
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

        Debug.Log("플레이어 공격");
        isPlayerAttackTime = true;
        EnemyAnimator.SetBool("isAttack", true);
        StartCoroutine(AttackPlayerTime());
    }
    IEnumerator AttackPlayerTime()
    {
        Debug.Log("플레이어 공격 코루틴 정상 작동 확인");
        yield return new WaitForSeconds(0.5f);
        PlayerAttack();
        isPlayerAttack = true;
        yield return new WaitForSeconds(0.5f);
        isPlayerAttack = false;
        EnemyAnimator.SetBool("isAttack", false);
        yield return new WaitForSeconds(0.5f);
        isPlayerAttackTime = false;
    }

    void PlayerAttack()
    {
        if (isPlayerAttack) return;
        if (_questSystem.playerLevel < 2 && _questSystem.playerPreQuestID <= _characterHealthSystem.Quest1_lastQuest)
        {
            Debug.Log("현재 플레이어 체력 감소 전투 시스템 미오픈 (레벨 2 이상 필요)");
        }
        else if (_characterHealthSystem.current_Character_Health > 0)
        {
            _characterHealthSystem.current_Character_Health -= _EnemyDamage;

            Debug.Log($"적에게 {_EnemyDamage}만큼 데미지를 받음. 남은 체력: {_characterHealthSystem.current_Character_Health}");

            if (_characterHealthSystem.current_Character_Health <= 0)
            {
                _characterHealthSystem.current_Character_Health = 0;
                _characterHealthSystem.Die();
            }
        }
    }
}
