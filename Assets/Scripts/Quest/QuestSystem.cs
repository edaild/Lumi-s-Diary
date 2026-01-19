using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class QuestSystem : MonoBehaviour
{
    public static QuestSystem instance;
    public QuestDataSO questData;
    public StorySystem storySystem;

    public int playerquerstID;
    public string playerquestName;
    public bool playerquest_Is_success;
    public int playerStory_Id;
    public int playerEnemyTargetCount;
    public int playerPreQuestID;
    public int giftExperience;

    [Header("플레이어 정보")]
    public int playerLevel = 1;
    public int playerExperience = 0;

    [Header("퀘스트 UI")]
    public TextMeshProUGUI questText;

    private int cuttentQuestIndex = 0;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    private void Start()
    {
        storySystem = GetComponent<StorySystem>();


        if (playerPreQuestID == 0 && questData.quests.Count > 0)
        {
            // 신규 플레이
            QuestData firstQuest = questData.quests[0];
            playerquerstID = firstQuest.QuestID;
            playerquestName = firstQuest.QuestName;
            playerStory_Id = firstQuest.Story_ID;
            playerEnemyTargetCount = firstQuest.TargetCount;
           
        }
        else
        {
            // 저장 기록이 있는 플레이
            QuestData quest = questData.quests.Find(q => q.QuestID == playerquerstID);
            if (quest != null)
            {
               // cuttentQuestIndex = lastQuestIndex;

                playerquestName = quest.QuestName;
                playerStory_Id = quest.Story_ID;
                playerEnemyTargetCount = quest.TargetCount;
                playerPreQuestID = quest.PreQuestID;
                giftExperience = quest.RewardExperience;
            }
        }
        Debug.Log($"현재 플레이어 퀘스트 ID: {playerquerstID}, 이름: {playerquestName}, 진행될 스토리 ID: {playerStory_Id}, 처치할 몬스터 수: {playerEnemyTargetCount}");
        storySystem.QuestStory(playerStory_Id);
        questText.text = playerquestName;
    }


    private void Update()
    {
        CurrentQuest();
    }

    void CurrentQuest()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            SuccessQuest();
            return;
        }
        else
        {
            Debug.Log("퀘스트 미완료");
            return;
        }
    }
    void SuccessQuest()
    {
        playerPreQuestID = playerquerstID;
        playerExperience += giftExperience;
        Debug.Log($"완료된 퀘스트 ID: {playerquerstID}, 이름: {playerquestName}, 스토리 ID: {playerStory_Id},  처리된 몬스터 수: {playerEnemyTargetCount}");
        Debug.Log($" 경험치 {giftExperience} 만큼 증가");

        cuttentQuestIndex += 1;

        if(cuttentQuestIndex < questData.quests.Count)
        {
            QuestData nextQuest = questData.quests[cuttentQuestIndex];

            playerquerstID = nextQuest.QuestID;
            playerquestName = nextQuest.QuestName;
            playerStory_Id = nextQuest.Story_ID;
            playerEnemyTargetCount = nextQuest.TargetCount;
            giftExperience = nextQuest.RewardExperience;

            questText.text = playerquestName;
            Debug.Log($"현재 플레이어 퀘스트 ID: {playerquerstID}, 이름: {playerquestName}, 진행될 스토리 ID: {playerStory_Id}, 처치할 몬스터 수: {playerEnemyTargetCount}");
            storySystem.QuestStory(playerStory_Id);
            playerquest_Is_success = false;
      
        }
        else
        {
            Debug.Log("퀘스트 완료");
            storySystem.StoryUI.gameObject.SetActive(false);
            // 다음 퀘스트SO로 연결
        }
    }
}

