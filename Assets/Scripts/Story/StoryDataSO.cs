using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StoryDataBase", menuName = "LumiProject/StoryDatabase")]
public class StoryDataSO : ScriptableObject
{
   public List<StoryData> storys = new List<StoryData>();
}
