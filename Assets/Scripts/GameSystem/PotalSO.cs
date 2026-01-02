using UnityEngine;

[CreateAssetMenu(fileName = "NewPortalData", menuName = "LumiProject/PortalData")]
public class PortalSO : ScriptableObject
{
    [Header("포탈 정보")]
    public int portalID;
    public string portalName;

    [Header("이동 설정")]
    public string targetSceneName;
    public Vector3 spawnPosition;
}