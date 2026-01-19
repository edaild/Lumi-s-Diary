using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "EnemyDatabase", menuName = "LumiProject/EnemyDatabase")]
public class EnmeySO : ScriptableObject
{
    public List<EnemyData> Enemys = new List<EnemyData>();
}
