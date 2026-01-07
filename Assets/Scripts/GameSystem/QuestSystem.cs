using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestSystem : MonoBehaviour
{
    public QuestDataSO questData;

    public int playerquerstID;
    public string playerquestName;
    public bool playerquest_Is_success;
    public int playerPreQuestID;


    public int playerLevel = 1;
    public int playerExperience = 0;


    private void Start()
    {
        Debug.Log("¹Ì±¸Çö");
    }
}
