using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEditor;

public class StorySystem : MonoBehaviour
{
    public StoryDataSO StoryDataSO;
    public QuestSystem questSystem;

    public int current_StoryID;
    public int current_StoryCount;
    public bool iscurrent_StoryImage;

    public string current_ImageCommand;
    public string current_ImageName;
    public GameObject storyImageGameObejct;
    public RawImage storyImageTextor;

    public bool isFinishStory;

    [Header("스토리 UI")]
    public GameObject StoryUI;
    public TextMeshProUGUI StoryDialogue;

    private void Start()
    {
        questSystem = GetComponent<QuestSystem>();
    }

    public void QuestStory(int StoryID)
    {
        if (StoryID == 0 && questSystem.finishQuest == false)
        {
            Debug.Log("전투 및 이동 퀘스트 진행중");
            return;
        }
        else
        {
            StoryData story = StoryDataSO.storys.Find(s => s.Story_ID == StoryID);

            if (story != null)
            {
                StoryUI.gameObject.SetActive(true);
                StoryDialogue.text = $"{story.Speaker} : {story.Dialogue}";
                current_StoryID = StoryID;
                iscurrent_StoryImage = story.Is_Image;
                current_ImageCommand = story.Image_Command;
                current_ImageName = story.TargetImageName;
                ShowImage();
            }

            int currnetstoryIdCount = StoryDataSO.storys.FindIndex(sid => sid.Story_ID == current_StoryID);
            Debug.Log($"현재 스토리 아이디: {current_StoryID} 스토리 아이디의 인덱스: {currnetstoryIdCount}");
            current_StoryCount = currnetstoryIdCount;
        }
    }

    public void Update()
    {
        if (Input.GetKeyUp(KeyCode.M))
        {
            NextStory();
        }
    }

    void NextStory()
    {
        Debug.Log($"이전 스토리 아이디: {current_StoryID}");

        current_StoryCount += 1;

        StoryData nextStory = StoryDataSO.storys[current_StoryCount];
        StoryDialogue.text = $"{nextStory.Speaker} : {nextStory.Dialogue}";
        current_StoryID = nextStory.Story_ID;
        iscurrent_StoryImage = nextStory.Is_Image;
        current_ImageCommand = nextStory.Image_Command;
        current_ImageName = nextStory.TargetImageName;
        ShowImage();
        Debug.Log($"현재 스토리 아이디: {current_StoryID}, 엔드 포인트 여부: {nextStory.EndPoint}");


        if (nextStory.EndPoint == true)
        {
            StoryUI.gameObject.SetActive(false);
            Debug.Log($"현재 퀘스트 스토리 종료");
            isFinishStory = true;
            return;
        }
    }

    void ShowImage()
    {
        if (iscurrent_StoryImage)
        {
            Texture2D texture = Resources.Load<Texture2D>(current_ImageName);

            if (texture != null)
            {
                RawImage rawImage = storyImageTextor.GetComponent<RawImage>();
                if (rawImage != null)
                {
                    rawImage.texture = texture;
                    storyImageGameObejct.SetActive(true);
                }
            }
        }
        else
        {
            storyImageGameObejct.SetActive(false);
        }
    }
}