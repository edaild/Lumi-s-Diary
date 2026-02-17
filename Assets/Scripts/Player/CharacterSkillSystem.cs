using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class CharacterSkillSystem : MonoBehaviour
{
    [Header("공격 오브젝트")]
    public GameObject iceBall;
    public GameObject snow;
    public GameObject crystalGardenPenel;

    public Transform shootPosition;
    public enum AttackType { Nomal, Skill }

    [Header("스킬 버튼")]
    public Button AttackButton;
    public Button SkillButton;
    public Button TeleportButton;
    public Button ultimateButtonUI;

    public GameObject ultimateVidio;

    public VideoPlayer videoPlayer;
    public VideoClip ultimateVidioClip;

    public AudioSource audioSource;

    [Header("공격 오디오")]
    public AudioClip nomalAtttackVoice;
    public AudioClip skillAttackVoice;
    public AudioClip skillCrystarGardenVoice;

    public CharacterMoveSystem _characterMoveSystem;

    public bool isCrystarGarden;

    private bool isultimateVidio;
    private bool isNomalAttack;
    private bool isSkillAttack;
    private bool isTeleport;


    private void Start()
    {
        _characterMoveSystem = UnityEngine.Object.FindAnyObjectByType<CharacterMoveSystem>();

        if (!_characterMoveSystem.isNotInGameScene)
        {
            AttackButton.onClick.AddListener(NomalAttack);
            SkillButton.onClick.AddListener(SkillAttack);
            TeleportButton.onClick.AddListener(Teleport);
            ultimateButtonUI.onClick.AddListener(CrystarGarden);
        }
    }
    private void Update()
    {
        //if (Input.GetMouseButtonDown(1))
        //{
        //    NomalAttack();
        //}

        //else if (Input.GetKeyDown(KeyCode.Q))
        //{
        //    CrystarGarden();
        //}

        //else if (Input.GetKeyDown(KeyCode.E))
        //{
        //    SkillAttack();
        //}

        //else if (Input.GetKeyDown(KeyCode.R))
        //{
        //    Teleport();
        //}
    }

    void NomalAttack()
    {
        if (!audioSource || !nomalAtttackVoice || isNomalAttack) return;

        isNomalAttack = true;
        audioSource.clip = nomalAtttackVoice;

        audioSource.time = 0;
        audioSource.Play();

        _characterMoveSystem.animator.SetBool("IsAttack", true);
        StartCoroutine(AttackTime());
    }

    IEnumerator AttackTime()
    {
        yield return new WaitForSeconds(0.6f);
        Debug.Log("애니매이션 종료");
        _characterMoveSystem.animator.SetBool("IsAttack", false);
        Shoot(AttackType.Nomal);
        isNomalAttack = false;
        audioSource.Stop();
        audioSource.clip = null;
    }

    void SkillAttack()
    {
        if (!audioSource || !skillAttackVoice || isSkillAttack) return;

        isSkillAttack = true;
        audioSource.clip = skillAttackVoice;

        audioSource.time = 0;
        audioSource.Play();

        _characterMoveSystem.animator.SetBool("IsSkillAttack", true);
        StartCoroutine(SkillAttackTime());
    }

    IEnumerator SkillAttackTime()
    {
        yield return new WaitForSeconds(1f);
        Shoot(AttackType.Skill);
        Debug.Log("애니매이션 종료");
        _characterMoveSystem.animator.SetBool("IsSkillAttack", false);

        yield return new WaitForSeconds(2f);
        audioSource.Stop();
        audioSource.clip = null;
        Debug.Log("스킬 공격 대기 쿨타임 : 10초");

        yield return new WaitForSeconds(10f);
        isSkillAttack = false;
        Debug.Log("스킬 공격 재사용 가능");
    }

    void Shoot(AttackType type)
    {
        GameObject ballPrefab = (type == AttackType.Nomal) ? iceBall : snow;
        GameObject ball = Instantiate(ballPrefab, shootPosition.position, shootPosition.rotation);

        float direction = transform.localScale.x > 0 ? 1f : -1f;

        Vector3 currentScale = ball.transform.localScale;
        ball.transform.localScale = new Vector3(Mathf.Abs(currentScale.x) * direction, currentScale.y, currentScale.z);

        ball.TryGetComponent<SkillBall>(out SkillBall Boll);

        float rotationDir = (direction > 0) ? -1f : 1f;
        ball.GetComponent<Rigidbody2D>().angularVelocity = Boll.BallRotageSpeed * rotationDir;
        ball.GetComponent<Rigidbody2D>().velocity = new Vector2(direction * Boll.BallSpeed, 0);
    }

    void Teleport()
    {
        if (isTeleport) return;
        int offset = (_characterMoveSystem.isHorizontalInput_xManus == false) ? 5 : -5;
        transform.position = new Vector3(transform.position.x + offset, transform.position.y, transform.position.z);
    }

    void CrystarGarden()
    {
        if (!audioSource || !skillCrystarGardenVoice || isultimateVidio || !ultimateVidio || !videoPlayer) return;

        ultimateVidio.gameObject.SetActive(true);

        videoPlayer.clip = ultimateVidioClip;
        videoPlayer.time = 0;
        videoPlayer.Play();
        isultimateVidio = true;
        _characterMoveSystem.playerSpeed = 8f;
        isCrystarGarden = true;
        StartCoroutine(CrystarGardenTime());
    }

    IEnumerator CrystarGardenTime()
    {
        ultimateVidio.gameObject.SetActive(true);
        videoPlayer.Play();

        yield return new WaitForSeconds(8.1f);
        videoPlayer.Stop();
        ultimateVidio.gameObject.SetActive(false);

        crystalGardenPenel.gameObject.SetActive(true);

        yield return new WaitForSeconds(30f);
        crystalGardenPenel.gameObject.SetActive(false);
        isCrystarGarden = false;
        _characterMoveSystem.playerSpeed = 5f;
        Debug.Log("크리스탈 가든 효과 종료");

        yield return new WaitForSeconds(30f);
        Debug.Log("크리스탈 가든 대기 쿨타임 : 30초");
        isultimateVidio = false;
        Debug.Log("크리스탈 가든 재사용 가능");
    }
}
