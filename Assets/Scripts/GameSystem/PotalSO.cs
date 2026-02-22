using UnityEngine;

[CreateAssetMenu(fileName = "NewPortalData", menuName = "LumiProject/PortalData")]
public class PortalSO : ScriptableObject
{
    public string portalName;
    public string targetSceneName;
    public Vector3 spawnPosition;
}