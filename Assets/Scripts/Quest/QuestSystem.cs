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
    public DubbingDatabase dubbingDatabase;

    public int playerquerstID;
    public string playerquestName;
    public bool playerquest_Is_success;
    public int playerStory_Id;
    public int playerEnemyTargetCount;
    public int playerPreQuestID;
    public int giftExperience;
    public bool finishQuest;

    public string currnetQuestType;
    public string FinishchackScene;
    public int playerEnmeyDieCount;

    [Header("플레이어 정보")]
    public int playerLevel = 1;
    public int playerExperience = 0;

    [Header("퀘스트 UI")]
    public TextMeshProUGUI questText;

    [Header("퀘스트와 스토리 교체 관련 리스트")]
    public List<QuestDataSO> QuestDataSOs = new List<QuestDataSO>();
    public List<StoryDataSO> storyDataSOs = new List<StoryDataSO>();

    private int currentQuestIndex = 0;
    private int currentQuestSOIndex = 0;
    private int currentStoryIndex = 0;

    // 추후 저장시스탬 구연후 사용
    private int lastQuestIndex;


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
            currnetQuestType = firstQuest.QuestType;
            FinishchackScene = firstQuest.FinishchackScene;


        }
        else
        {
            // 저장 기록이 있는 플레이
            QuestData quest = questData.quests.Find(q => q.QuestID == playerquerstID);
            if (quest != null)
            {
                currentQuestIndex = lastQuestIndex;
                playerquestName = quest.QuestName;
                playerStory_Id = quest.Story_ID;
                playerEnemyTargetCount = quest.TargetCount;
                playerPreQuestID = quest.PreQuestID;
                giftExperience = quest.RewardExperience;
                currnetQuestType = quest.QuestType;
                FinishchackScene = quest.FinishchackScene;
            }
        }
        Debug.Log($"현재 플레이어 퀘스트 ID: {playerquerstID}, 이름: {playerquestName}, 진행될 스토리 ID: {playerStory_Id}, 처치할 몬스터 수: {playerEnemyTargetCount}");
        storySystem.QuestStory(playerStory_Id);
        questText.text = playerquestName;
    }


    private void Update()
    {
        SuccessChack();

        if (Input.GetKeyDown(KeyCode.N))
        {
            SuccessQuest();
            Debug.Log("치트 사용으로 현재 퀘스트 완료 처리");
        }
    }

    void SuccessChack()
    {
        switch (currnetQuestType)
        {
            case "Story":
                if(storySystem.isFinishStory == true)
                {
                    SuccessQuest();
                    storySystem.isFinishStory = false;
                }
                break;

            case "Move":
                if (SceneManager.GetActiveScene().name == FinishchackScene)
                {
                    SuccessQuest();
                }
                break;

            case "Battle":
                if(playerEnmeyDieCount == playerEnemyTargetCount)
                {
                    SuccessQuest();
                    playerEnmeyDieCount = 0;
                }
                break;

            case "Finish":
                SuccessQuest();
                Debug.Log($"{playerquestName} 종료");
                break;
        }
    }

    void SuccessQuest()
    {
        playerPreQuestID = playerquerstID;
        playerExperience += giftExperience;
        Debug.Log($"완료된 퀘스트 ID: {playerquerstID}, 이름: {playerquestName}, 스토리 ID: {playerStory_Id},  처리된 몬스터 수: {playerEnemyTargetCount}");
        Debug.Log($" 경험치 {giftExperience} 만큼 증가");

        currentQuestIndex += 1;

        if(currentQuestIndex < questData.quests.Count)
        {
            QuestData nextQuest = questData.quests[currentQuestIndex];

            playerquerstID = nextQuest.QuestID;
            playerquestName = nextQuest.QuestName;
            playerStory_Id = nextQuest.Story_ID;
            playerEnemyTargetCount = nextQuest.TargetCount;
            giftExperience = nextQuest.RewardExperience;
            currnetQuestType = nextQuest.QuestType;
            FinishchackScene = nextQuest.FinishchackScene;
            questText.text = playerquestName;
            playerEnmeyDieCount = 0;
            Debug.Log($"현재 플레이어 퀘스트 ID: {playerquerstID}, 이름: {playerquestName}, 진행될 스토리 ID: {playerStory_Id}, 처치할 몬스터 수: {playerEnemyTargetCount}");
            storySystem.QuestStory(playerStory_Id);
            playerquest_Is_success = false;
   
        }
        else
        {
            Debug.Log("퀘스트 완료");
            finishQuest = true;
            storySystem.StoryUI.gameObject.SetActive(false);
        }
    }
}

