using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "QuestDatabase", menuName = "LumiProject/QuestDatabase")]
public class QuestDataSO : ScriptableObject
{
    public List<QuestData> quests = new List<QuestData>();
}
