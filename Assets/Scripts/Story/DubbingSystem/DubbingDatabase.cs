using System.Collections;
using System.Collections.Generic;
using System.Security.Policy;
using UnityEngine;

[CreateAssetMenu(fileName = "DubbingData", menuName = "LumiProject/DubbingData")]
public class DubbingDatabase : ScriptableObject
{
    public List<AudioClip> dubbingaudioClip = new List<AudioClip>();
}
