using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class CharacterHealthSystem : MonoBehaviour
{
    public int character_Health = 1000;
    public int current_Character_Health;

    [Header("체력과 슬라이드 UI")]
    public GameObject HeaderUI;
    public TextMeshProUGUI character_health_Text;

    public AudioClip DieAudio;
    public QuestSystem _questSystem;
    public CharacterMoveSystem _characterMoveSystem;
    public CharacterSkillSystem _characterSkillSystem;

    private bool isHealth;

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
        if (_questSystem.playerLevel >= 2 && current_Character_Health > 0) return;
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
}
