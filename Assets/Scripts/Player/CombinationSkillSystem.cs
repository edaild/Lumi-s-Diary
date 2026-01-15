using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class CombinationSkillSystem : MonoBehaviour
{
    public List<VideoClip> videoClips = new List<VideoClip>();
    public VideoPlayer videoPlayer;
    public Button combinationAttackButton;

    public string testcurrentcombinationAttackName = "";

    private void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        combinationAttackButton.onClick.AddListener(CombinationAttack);
    }

    void CombinationAttack()
    {
        if (videoPlayer == null || videoClips == null) return;

        string currentcombinationAttackName = testcurrentcombinationAttackName;

        VideoClip currentcombinationAttack = videoClips.Find(sv => sv.name == currentcombinationAttackName);

        if (currentcombinationAttack != null)
        {
            videoPlayer.clip = currentcombinationAttack;
            videoPlayer.Play();
            Debug.Log("협동 공격 비디오 제생");
            CombinationAttackCullVideoTime();
        }
    

    }

    IEnumerator CombinationAttackCullVideoTime()
    {
        yield return new WaitForSeconds(8f);
        videoPlayer.Stop();
        videoPlayer.clip = null;
        Debug.Log("협동 공격 비디오 종료및 60초 쿨타임");

        StartCoroutine(CombinationAttackCullTime());
    }


    IEnumerator CombinationAttackCullTime()
    {
        yield return new WaitForSeconds(60f);
        Debug.Log("협동 스킬 쿨 타임 종료");
    }
}
