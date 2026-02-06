using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using JetBrains.Annotations;
using TMPro;

public class HallMathSystem : MonoBehaviour
{
    public int QuestanswerCount;
    public int QuestNumber01;
    public int QuestNumber02;
    public int QuestNumber03;
    public int answerCount;
    public TextMeshProUGUI QuestText;

    [Header("정답 버튼")]
    public Button answerButton01;
    public Button answerButton02;
    public Button answerButton03;

    public TextMeshProUGUI answerButton01text;
    public TextMeshProUGUI answerButton02text;
    public TextMeshProUGUI answerButton03text;

    public List<GameObject> Buttonlist = new List<GameObject>();
    private int answerButton01Count;
    private int answerButton02Count;
    private int answerButton03Count;

    private void Start()
    {
        SetupQuest();

        answerButton01.onClick.AddListener(AnswerButton01);
        answerButton02.onClick.AddListener(AnswerButton02);
        answerButton03.onClick.AddListener(AnswerButton03);

        RandomButton();
    }

    void SetupQuest()
    {
        QuestNumber01 = Random.Range(0, 10);
        QuestNumber02 = Random.Range(0, 10);
        QuestNumber03 = Random.Range(0, 10);

        if(QuestanswerCount < 3)
        {
            answerCount = QuestNumber01 + QuestNumber02;
            QuestText.text = $"{QuestNumber01} + {QuestNumber02} = ?";
        }
        else if (QuestanswerCount == 3)
        {
            answerCount = (QuestNumber01 + QuestNumber02) & QuestNumber03;
            QuestText.text = $"x: {QuestNumber01}, y: {QuestNumber02}, z: {QuestNumber03} \n \n \n (x + y) % z = ?";
        }
        else if (QuestanswerCount == 4)
        {
            answerCount = (QuestNumber01 + QuestNumber02) * QuestNumber03;
            QuestText.text = $"x: {QuestNumber01}, y: {QuestNumber02}, z: {QuestNumber03} \n \n \n (x + y) * z = ?";
        }
        else if (QuestanswerCount == 5)
        {
            int x = (QuestNumber01 + QuestNumber02) & QuestNumber03;
            int y = (QuestNumber01 + QuestNumber02) * QuestNumber03;
            answerCount = x + y % 2;
            QuestText.text = $"a: {QuestNumber01}, b: {QuestNumber02} c: {QuestNumber03}  \n \n \n ((a + b) % c) + ((a + b) * c) % 2 = ?";
        }

        Debug.Log($"문재 정답: {answerCount}");
    }

    private void Update()
    {
        MiniGameManager();
    }

    void RandomButton()
    {
        int randomIndex = Random.Range(0, Buttonlist.Count);

        GameObject selectButton = Buttonlist[randomIndex];
        Debug.Log(selectButton.name);

        answerButton01Count = (selectButton == Buttonlist[0]) ? answerCount : GetWrongAnswer();
        answerButton02Count = (selectButton == Buttonlist[1]) ? answerCount : GetWrongAnswer();
        answerButton03Count = (selectButton == Buttonlist[2]) ? answerCount : GetWrongAnswer();

        answerButton01text.text = $"{answerButton01Count}";
        answerButton02text.text = $"{answerButton02Count}";
        answerButton03text.text = $"{answerButton03Count}";
    }

    int GetWrongAnswer()
    {
        int wrong;
        do
        {
            wrong = Random.Range(0, 30);
        }
        while (wrong == answerCount);
        return wrong;
    }

    void MiniGameManager()
    {
        if (QuestanswerCount >= 6)
        {
            Debug.Log("미니 게임 종료");
            SceneManager.LoadScene("MaigicurlHotel");
        }
    }

    void NextQuest()
    {
        SetupQuest();
        RandomButton();
    }

    void AnswerButton01()
    {
        if (answerButton01Count == answerCount)
        {
            Debug.Log("정답");
            QuestanswerCount++;
            NextQuest();
        }
        else
            Debug.Log("오답");

    }
    void AnswerButton02()
    {
        if (answerButton02Count == answerCount)
        {
            Debug.Log("정답");
            QuestanswerCount++;
            NextQuest();
        }
        else
            Debug.Log("오답");
    }
    void AnswerButton03()
    {
        if (answerButton03Count == answerCount)
        {
            Debug.Log("정답");
            QuestanswerCount++;
            NextQuest();
        }
        else
            Debug.Log("오답");
    }
}

   
   
