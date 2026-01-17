using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Newtonsoft.Json.Bson;

public class StorySystem : MonoBehaviour
{
    public StoryDataSO StoryDataSO;
    public QuestSystem questSystem;
    public int cunnet_StoryID;
    public int cunnet_StoryCount;

    [Header("스토리 UI")]
    public GameObject StoryUI;
    public TextMeshProUGUI StoryDialogue;

    private void Start()
    {
       questSystem = GetComponent<QuestSystem>();
    }



    public void QuestStory(int StoryID)
    {
        if(StoryID == 0)
        {
            Debug.Log("전투 및 이동 퀘스트 진행중");
            return;
        }
        else
        {
            StoryData story = StoryDataSO.storys.Find(s => s.Story_ID == StoryID);

            if(story != null)
            {
                StoryUI.gameObject.SetActive(true);
                StoryDialogue.text = $"{story.Speaker} : {story.Dialogue}";
                cunnet_StoryID = StoryID;
         
            }

            int currnetstoryIdCount = StoryDataSO.storys.FindIndex(sid => sid.Story_ID == cunnet_StoryID);
            Debug.Log($"현재 스토리 아이디: {cunnet_StoryID} 스토리 아이디의 인덱스: {currnetstoryIdCount}");
            cunnet_StoryCount = currnetstoryIdCount;
        }
    }

    public void Update()
    {
        if(Input.GetKeyUp(KeyCode.M))
        {
            NextStory();
        }
    }

    public void NextStory()
    {
        Debug.Log($"이전 스토리 아이디: {cunnet_StoryID}");

        cunnet_StoryCount += 1;

        StoryData nextStory = StoryDataSO.storys[cunnet_StoryCount];
        StoryDialogue.text = $"{nextStory.Speaker} : {nextStory.Dialogue}";
        cunnet_StoryID = nextStory.Story_ID;

        Debug.Log($"현재 스토리 아이디: {cunnet_StoryID}");

        if(nextStory.EndPoint == true)
        {
            StoryUI.gameObject.SetActive(false);
            Debug.Log($"현재 퀘스트 스토리 종료");

        }
    }
}
