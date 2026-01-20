using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

[Serializable]
public class QuestData
{
    public int QuestID;
    public string QuestName;
    public string QuestType;
    public int QuestLevel;
    public bool Is_success;
    public string QuestDescription;
    public int Story_ID;
    public int TargetCount;
    public int RewardExperience;
    public int PreQuestID;
    public string FinishchackScene;
}
