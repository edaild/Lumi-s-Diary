using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StorySystem : MonoBehaviour
{
    public StoryDataSO StoryDataSO;
    public QuestSystem questSystem;
    public int cunnet_StoryID;

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
           
        }
    }
}
