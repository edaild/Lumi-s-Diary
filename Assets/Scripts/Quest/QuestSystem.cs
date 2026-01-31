using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

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
    public bool finishQuest;

    public string currnetQuestType;
    public string FinishchackScene;
    public int playerEnmeyDieCount;

    [Header("치트키")]
    public Button questSkipButton;
    public Button fastStoryButton;


    public GameObject QuestCanavarse;

    [Header("플레이어 정보")]
    public int playerLevel = 1;
    public int playerExperience = 0;

    [Header("퀘스트 UI")]
    public TextMeshProUGUI questText;

    private int currentQuestIndex = 0;

    public QuestAndStoryDatabase _questAndStoryDatabase;

    // 추후 저장 시스탬 구연후 사용
    private int lastQuestIndex;

    public int currentQuestAndSotorys;
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

    private void OnEnable()
    {
        SceneManager.sceneLoaded += IsNotGameScene;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= IsNotGameScene;
    }

    private void IsNotGameScene(Scene scene, LoadSceneMode mode)
    {
       if(scene.name == "LobbyScene"|| scene.name == "RewordScene" || scene.name == "MathScene")
       {
            QuestCanavarse.gameObject.SetActive(false);
       }
        else
        {
            QuestCanavarse.gameObject.SetActive(true);
        }
    }


    private void Start()
    {
        storySystem = GetComponent<StorySystem>();

        questSkipButton.onClick.AddListener(SuccessQuest);
        fastStoryButton.onClick.AddListener(FastStory);

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
        if (!finishQuest)
        {
            SuccessChack();

            if (Input.GetKeyDown(KeyCode.N))
            {
                SuccessQuest();
                Debug.Log("치트 사용으로 현재 퀘스트 완료 처리");
            }
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
                finishQuest = true;
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
            finishQuest = false;
        }
        else
        {
            Debug.Log("챕터 완료");
            finishQuest = true;
            storySystem.StoryUI.gameObject.SetActive(false);
            ChangeQuest();
        }
    }

    void ChangeQuest()
    {
        currentQuestAndSotorys += 1;

        int nextIndex = currentQuestAndSotorys;
        if (_questAndStoryDatabase != null && nextIndex < _questAndStoryDatabase.questDataSOs.Count)
        {
          
            QuestDataSO nextQuest = _questAndStoryDatabase.questDataSOs[currentQuestAndSotorys];
            if (nextQuest != null)
            {
                questData = nextQuest;
                currentQuestIndex = 0;
                Debug.Log($"{nextQuest.name} 으로 쳅터 변경 완료");
                finishQuest = false;

                StoryDataSO nextStory = _questAndStoryDatabase.storyDataSOs[currentQuestAndSotorys];
                if (nextStory != null)
                {
                    storySystem.StoryDataSO = nextStory;
                    storySystem.current_StoryCount = 0;
                    Debug.Log($"{nextStory.name}으로 스토리 변경 완료");
                    storySystem.isFinishStory = false;
                }

                DubbingDatabase nextDubbing = _questAndStoryDatabase.DubbingDatabases[currentQuestAndSotorys];
                if (nextDubbing != null)
                {
                    storySystem.DubbingDatabase = nextDubbing;
                    Debug.Log($"{nextDubbing.name}으로 스토리 더빙 변경 완료");
                }
            }
        }
        else
        {
            finishQuest = true;
        }
    }

    void FastStory()
    {
        if (!storySystem.isNotStoryTimedelay)
        {
            storySystem.isNotStoryTimedelay = true;
            Debug.Log("스토리 넘기기 대기시간 해제");
        }
        else
        {
            storySystem.isNotStoryTimedelay = false;
            Debug.Log("스토리 넘기기 대기시간 적용");
        }
    }
}

