using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BossPattern", menuName = "LumiProject/BosssPattern")]
public class BossPatternSO : ScriptableObject
{
    public string BossName;
    public string BossPatternName;
    public int BossPatternDamege;
}

