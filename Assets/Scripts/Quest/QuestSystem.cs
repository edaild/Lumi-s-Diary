using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuestSystem : MonoBehaviour
{
    public static QuestSystem instance;
    public QuestDataSO questData;

    public int playerquerstID;
    public string playerquestName;
    public bool playerquest_Is_success;
    public int playerPreQuestID;


    public int playerLevel = 1;
    public int playerExperience = 0;

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
        
        if (playerPreQuestID == 0 && questData.quests.Count > 0)
        {
            // 신규 플레이
            QuestData firstQuest = questData.quests[0];
            playerquerstID = firstQuest.QuestID;
            playerquestName = firstQuest.QuestName;
        }
        else
        {
            // 저장 기록이 있는 플레이
            QuestData quest = questData.quests.Find(q => q.QuestID == playerquerstID);
            if (quest != null)
            {
               // cuttentQuestIndex = lastQuestIndex;
                playerquestName = quest.QuestName;
            }
        }
        Debug.Log($"현재 플레이어 퀘스트 ID: {playerquerstID}, 이름: {playerquestName}");
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
            SuccessQuest();
        
    }
    void SuccessQuest()
    {
        Debug.Log($"완료한 퀘스트 ID: {playerquerstID}, 이름: {playerquestName}");

        cuttentQuestIndex += 1;

        if(cuttentQuestIndex < questData.quests.Count)
        {
            QuestData nextQuest = questData.quests[cuttentQuestIndex];

            playerquerstID = nextQuest.QuestID;
            playerquestName = nextQuest.QuestName;

            Debug.Log($"현재 플레이어 퀘스트 ID: {playerquerstID}, 이름: {playerquestName}");
        }
        else
        {
            Debug.Log("퀘스트 완료");
            // 다음 퀘스트SO로 연결
        }
    }
}

