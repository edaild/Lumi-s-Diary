using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterHealthSystem : MonoBehaviour
{
    public int character_Health = 1000;
    public int character_stemina = 1000;

    public int current_Character_Health;
    public int current_Character_stemina;
    [Header("체력과 슬라이드 UI")]
    public GameObject HeaderUI;
    public Slider character_health_slider;
    public Slider character_stemina_slider;

    public AudioClip DieAudio;

    public QuestSystem _questSystem;
    public CharacterMoveSystem _characterMoveSystem;
    public CharacterSkillSystem _characterSkillSystem;
    public BossSceneSystem _bossSceneSystem;


    [Tooltip("2장 바이탈 벤드 수령  퀘스트")] public int Quest1_lastQuest = 20004;

    private void Start()
    {
        _questSystem = UnityEngine.Object.FindObjectOfType<QuestSystem>();
        _characterMoveSystem = UnityEngine.Object.FindAnyObjectByType<CharacterMoveSystem>();
        _characterSkillSystem = UnityEngine.Object.FindAnyObjectByType<CharacterSkillSystem>();

        current_Character_Health = character_Health;
        current_Character_stemina = character_stemina;
    }

    private void Update()
    {
        ShowHeader();
    }
    void ShowHeader()
    {
        if(_questSystem.playerLevel >= 2 ||_questSystem.playerPreQuestID >= Quest1_lastQuest)
            HeaderUI.gameObject.SetActive(true);
        else
            HeaderUI.gameObject.SetActive(false);
    }
    void Die()
    {
        if (_questSystem.playerLevel >= 2 || !_characterMoveSystem || !_characterSkillSystem ||current_Character_Health >= 0) return;
        _characterMoveSystem.animator.SetBool("isDie", true);
        _characterSkillSystem.audioSource.clip = DieAudio;

        _characterSkillSystem.audioSource.Play();
        StartCoroutine(DiePlayer());
    }

    IEnumerator DiePlayer()
    {
        yield return new WaitForSeconds(1f);
        _characterMoveSystem.animator.SetBool("isDie", false);
        _characterSkillSystem.audioSource.Stop();
        _characterSkillSystem.audioSource.clip = null;
        _characterMoveSystem.fadeManager.StartFadeOut(1.5f);

         yield return new WaitForSeconds(3f);
        if (!_bossSceneSystem.isBossScene)
        {
            SceneManager.LoadScene("MaigicurlHotel");
            current_Character_Health = character_Health;
            current_Character_stemina = character_stemina;
        }
        else
        {
            Debug.Log("보스 재시작 클리어 전까지 못나감");
            _characterMoveSystem.fadeManager.StartFadeIn(1.5f);
            current_Character_Health = character_Health;
            current_Character_stemina = character_stemina;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy")/* && other.gameObject.CompareTag("EnmeyAttackCollider")*/)
        {
            other.gameObject.TryGetComponent<EnemySystem>(out EnemySystem enemy);
            MinusHeath(enemy);
        }
    }

    void MinusHeath(EnemySystem Enemy)
    {
        if (_questSystem.playerLevel < 2)
        {
            Debug.Log("현재 플레이어 체력 감소 전투 시스템 미오픈 (레벨 2 이상 필요)");
        }
        else if (current_Character_Health > 0)
        {
            current_Character_Health -= Enemy._EnemyDamage;

            Debug.Log($"적에게 {Enemy._EnemyDamage}만큼 데미지를 받음. 남은 체력: {current_Character_Health}");

            if (current_Character_Health <= 0)
            {
                current_Character_Health = 0;
                Die();
            }
        }
    }
}
