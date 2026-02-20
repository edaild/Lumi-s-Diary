using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEditor;
using System;

public class StorySystem : MonoBehaviour
{
    public StoryDataSO StoryDataSO;
    public QuestSystem questSystem;
    public DubbingDatabase DubbingDatabase;
    public GameMusicSystem _gameMusicSystem;

    public int current_StoryID;
    public int current_StoryCount;
    public bool iscurrent_StoryImage;

    public string current_ImageCommand;
    public string current_ImageName;
    public GameObject storyImageGameObejct;
    public RawImage storyImageTextor;

    public bool isStoryEndPoint;
    public bool isFinishStory;
    public string current_TargetAudio;
    public string current_TargetMusic;

    [Header("스토리 UI")]
    public GameObject StoryUI;
    public Button StoryButton;
    public TextMeshProUGUI characterNameText;
    public Image nameBox;
    public TextMeshProUGUI StoryDialogue;

    public AudioSource storyDubbingAudioSource;


    private string characterName;
    private bool isStoryTIme;
    public bool isNotStoryTimedelay;
    
    private void Start()
    {
        questSystem = GetComponent<QuestSystem>();
        _gameMusicSystem = UnityEngine.Object.FindAnyObjectByType<GameMusicSystem>();
        StoryButton.onClick.AddListener(NextStory);
    }

    public void QuestStory(int StoryID)
    {
        Debug.Log("함수 실행");
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
                characterNameText.text = $"{story.Speaker}";
                StoryDialogue.text = $"{story.Dialogue}";
                current_StoryID = StoryID;
                characterName = story.Speaker;
                iscurrent_StoryImage = story.Is_Image;
                current_TargetAudio = story.TargetAudio;
                current_ImageCommand = story.Image_Command;
                current_ImageName = story.TargetImageName;
                current_TargetMusic = story.TargetMusic;
                CurrentStoryAsset();
            }
            else
            {
                Debug.Log("스토리 아이디를 못찾음");
            }

            int currnetstoryIdCount = StoryDataSO.storys.FindIndex(sid => sid.Story_ID == current_StoryID);
            Debug.Log($"현재 스토리 아이디: {current_StoryID} 스토리 아이디의 인덱스: {currnetstoryIdCount}");
            current_StoryCount = currnetstoryIdCount;
            isStoryEndPoint = false;
        }
    }

    void NextStory()
    {   
        if (isStoryEndPoint == true || isStoryTIme || isFinishStory) return;

        Debug.Log($"이전 스토리 아이디: {current_StoryID}");

        current_StoryCount += 1;

        StoryData nextStory = StoryDataSO.storys[current_StoryCount];
        characterNameText.text = $"{nextStory.Speaker}";
        StoryDialogue.text = $"{nextStory.Dialogue}";
        current_StoryID = nextStory.Story_ID;
        characterName = nextStory.Speaker;
        iscurrent_StoryImage = nextStory.Is_Image;
        current_TargetAudio = nextStory.TargetAudio;
        current_ImageCommand = nextStory.Image_Command;
        current_ImageName = nextStory.TargetImageName;
        current_TargetMusic = nextStory.TargetMusic;
        Debug.Log($"현재 스토리 아이디: {current_StoryID}, 엔드 포인트 여부: {nextStory.EndPoint}");
        CurrentStoryAsset();

        if (nextStory.EndPoint == true)
        {
            StoryUI.gameObject.SetActive(false);
            Debug.Log($"현재 퀘스트 스토리 종료");
            isFinishStory = true;
            isStoryEndPoint = true;
            return;
        }
    }

    void CurrentStoryAsset()
    {
        ShowImage();
        PlayDubbing();
        PlayTargetMusic();
        NameBox();
        if (!isNotStoryTimedelay)
        {
            StartCoroutine(NextStoryTime());
        }
    }

    IEnumerator NextStoryTime()
    {
        isStoryTIme = true;
        yield return new WaitForSeconds(1.3f);
        isStoryTIme = false;
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

    void PlayDubbing()
    {
        if (string.IsNullOrEmpty(current_TargetAudio)) return;

        AudioClip DubbingAudio = DubbingDatabase.dubbingaudioClip.Find(Do => Do.name == current_TargetAudio);

        if(DubbingAudio != null)
        {
            storyDubbingAudioSource.clip = DubbingAudio;
            storyDubbingAudioSource.Play();
        }
    }

    void PlayTargetMusic()
    {
        if (!string.IsNullOrEmpty(current_TargetMusic))
        {
            Debug.Log($"{current_TargetMusic} 존재여분 확인");
            AudioClip ChnageMusic = _gameMusicSystem.audioClips.Find(gm => gm.name == current_TargetMusic);

            if (ChnageMusic != null)
            {
                _gameMusicSystem.audioSource.Stop();
                _gameMusicSystem.audioSource.clip = ChnageMusic;
                _gameMusicSystem.audioSource.Play();
            }
            else
            {
                Debug.Log($"{current_TargetMusic}의 이름의 오디오 클립 미존재");
            }
        }
    }

    void NameBox()
    {
        switch (characterName)
        {
            case "루미":
                nameBox.color = new Color32(135, 206, 235, 200);
                break;
            case "라이":
                nameBox.color = new Color32(127, 255, 212, 200);
                break;
            case "아이리스":
                nameBox.color = new Color32(255, 182, 193, 200);
                break;
            case "세리":
                nameBox.color = new Color32(245, 245, 245, 200);
                break;
            default:
                nameBox.color = new Color32(0, 0, 0, 200);
                break;

        }
    }
}