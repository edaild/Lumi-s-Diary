using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorySystem : MonoBehaviour
{
    public StoryDataSO StoryDataSO;
    public QuestSystem questSystem;
    public int PlayerStoryCount;
    

    private void Start()
    {
       questSystem = GetComponent<QuestSystem>();

      
    }


    void QuestStory()
    {

    }
}
