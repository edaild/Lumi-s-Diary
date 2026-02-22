using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class CharacterHealthSystem : MonoBehaviour
{
    public int character_Health = 10000;
    public int current_Character_Health;

    [Header("체력과 슬라이드 UI")]
    public GameObject HeaderUI;
    public TextMeshProUGUI character_health_Text;

    public AudioClip DieAudio;
    public QuestSystem _questSystem;
    public CharacterMoveSystem _characterMoveSystem;
    public CharacterSkillSystem _characterSkillSystem;
    public bool isDeath;
    private bool isHealth;
    private bool isInShield;


    [Tooltip("2장 바이탈 벤드 수령  퀘스트")] public int Quest1_lastQuest = 20004;

    private void Start()
    {
        _questSystem = UnityEngine.Object.FindObjectOfType<QuestSystem>();
        _characterMoveSystem = UnityEngine.Object.FindAnyObjectByType<CharacterMoveSystem>();
        _characterSkillSystem = UnityEngine.Object.FindAnyObjectByType<CharacterSkillSystem>();

        current_Character_Health = character_Health;
    }

    private void Update()
    {
        ShowHeader();
        if(isHealth) character_health_Text.text = $"{current_Character_Health}";

        // 보스전 한정
        if (isDeath && !isInShield) 
        {
            Debug.Log($"보호막 밖에서 즉사! 남은 체력: {current_Character_Health}");
            isDeath = false;
            current_Character_Health = 0;
            Die();
        }
          
    }
    void ShowHeader()
    {
        if(_questSystem.playerLevel >= 2 && _questSystem.playerPreQuestID >= Quest1_lastQuest)
        {
            HeaderUI.gameObject.SetActive(true);
            isHealth = true;
        }
        else
        {
            HeaderUI.gameObject.SetActive(false);
            isHealth = false;
        }

    }
    public void Die()
    {
        Debug.Log("Die 호출 확인");
        if (current_Character_Health > 0) return;
        current_Character_Health = 0;
        //_characterMoveSystem.animator.SetBool("isDie", true);
        //_characterSkillSystem.audioSource.clip = DieAudio;
        //_characterSkillSystem.audioSource.Play();
        StartCoroutine(DiePlayer());
    }

    IEnumerator DiePlayer()
    {
        yield return new WaitForSeconds(1f);
        //_characterMoveSystem.animator.SetBool("isDie", false);
        //_characterSkillSystem.audioSource.Stop();
        //_characterSkillSystem.audioSource.clip = null;
        _characterMoveSystem.fadeManager.StartFadeOut(1.5f);

        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("MaigicurlHotel");
        current_Character_Health = character_Health;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_questSystem.playerLevel < 2 && _questSystem.playerPreQuestID < Quest1_lastQuest) return;

        if (collision.TryGetComponent<BossPatternSystem>(out BossPatternSystem Pattern))
        {
            current_Character_Health -= Pattern.bossPatternSO.BossPatternDamege;
            Debug.Log($"보스에게 {Pattern.bossPatternSO.BossPatternDamege} 만큼에 공격을 받음 남은 체력: {current_Character_Health}");
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Shield")) isInShield = true;
        else isInShield = false;

    }
}
