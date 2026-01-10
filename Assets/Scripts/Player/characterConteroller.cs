using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class characterConteroller : MonoBehaviour
{
    public VariableJoystick joy;

    public float playerSpeed = 5f;
    public float playerJump = 8f;
    public Rigidbody2D rb;
    public Collider2D cd;
    public Animator animator;

    [Header("공격 오브젝트")]
    public GameObject iceBall;
    public GameObject snow;
    public GameObject crystalGardenPenel;

    public Transform shootPosition;
    public enum AttackType{ Nomal, Skill}


    [Header("스킬 버튼")]
    public Button AttackButton;
    public Button SkillButton;
    public Button TeleportButton;
    public Button JumpButton;
    public Button ultimateButtonUI;

    public GameObject ultimateVidio;

    public VideoPlayer videoPlayer;
    public VideoClip ultimateVidioClip;

    public AudioSource audioSource;

    [Header("공격 오디오")]
    public AudioClip nomalAtttackVoice;
    public AudioClip skillAttackVoice;
    public AudioClip skillCrystarGardenVoice;


    public PortalSystem portalSystem;
    public GameObject currentPortal;
    public FadeManager fadeManager;

    private bool isultimateVidio;
    private bool isJump;
    private bool isGround;
    private bool islocalGround;
    private bool isNomalAttack;
    private bool isSkillAttack;
    private bool isTeleport;
    private bool isHorizontalInput_xManus;
    private bool isNotInGameScene;
    private bool isPotalTime;
    private bool isFade;

    private void Start()
    {
        JumpButton.onClick.AddListener(() => { if (joy.Vertical < -0.7f) DownJump(); else Jump(); });
        transform.localScale = new Vector3(1.5f, 1.5f, 0);

        if (!isNotInGameScene)
        {
            AttackButton.onClick.AddListener(NomalAttack);
            SkillButton.onClick.AddListener(SkillAttack);
            TeleportButton.onClick.AddListener(Teleport);
            ultimateButtonUI.onClick.AddListener(CrystarGarden);
        }
        if (!portalSystem)
            portalSystem = FindAnyObjectByType<PortalSystem>();

        string sceneName = SceneManager.GetActiveScene().name;

        if (sceneName != "LumiHouseScene" && sceneName != "MaigicurlHotel")
        {
            isFade = true;
            fadeManager.StartFadeIn(1.5f);
        }
        else
        {
            isFade = false;
            Debug.Log("현재는 루미집 또는 매직컬 센터");
        }

    }

    private void Update()
    {
        // PC 테스트용
        if (Input.GetKeyDown(KeyCode.Space) && (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow) || joy.Vertical < -0.7f))
            DownJump();
        else if(Input.GetKeyDown(KeyCode.Space))
            Jump();

        else if (Input.GetMouseButtonDown(1))
        {
            NomalAttack();
        }

        else if (Input.GetKeyDown(KeyCode.Q))
        {
            CrystarGarden();
        }

        else if(Input.GetKeyDown(KeyCode.E))
        {
            SkillAttack();
        }

        else if (Input.GetKeyDown(KeyCode.R))
        {
            Teleport();
        }

        if ((currentPortal != null && joy.Vertical > 0.7f || (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && !isPotalTime))
        {
            MovePartal();
        }
            

    }

    private void FixedUpdate()
    {
        if(joy.Vertical > 0.7f || joy.Vertical < -0.7f)
        {
            Debug.Log("상하 움직임은 지원하지 않습니다.");
        }
        else
          charcterMove();

    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += IsNotInGameScene;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= IsNotInGameScene;
    }

    void IsNotInGameScene(Scene scene, LoadSceneMode mode)
    {
        string sceneName = SceneManager.GetActiveScene().name;
        if (sceneName == "LumiHouseScene" || sceneName == "MaigicurlHotel")
        {
            isNotInGameScene = true;
            isFade = false;
        }
       
        else
        {
            isNotInGameScene = false;
            isFade = true;
        }
            
    }

    

    void charcterMove()
    {
        float HorizontalInput = (Input.GetAxisRaw("Horizontal") != 0) ? Input.GetAxisRaw("Horizontal") : joy.Horizontal;

        Vector3 Derection = new Vector3(HorizontalInput, 0, 0).normalized;
        Vector3 move = Derection * playerSpeed * Time.deltaTime;
        transform.position += move;

        if(HorizontalInput < -0.1f)
        {
            isHorizontalInput_xManus = true;
            Flip(true);
        }
         
        else if(HorizontalInput > 0.1f)
        {
            isHorizontalInput_xManus = false;
            Flip(false);
        }
  
        if (Derection != Vector3.zero) 
            animator.SetBool("IsWalk", true);
        else
            animator.SetBool("IsWalk", false);
    }

    void Flip(bool shouldFlip)
    {
        Vector3 scale = transform.localScale;

        if (shouldFlip)
            scale.x = -Mathf.Abs(scale.x);
        else
            scale.x = Mathf.Abs(scale.x);

        transform.localScale = scale;
    }

    void Jump()
    {
        if (!isJump && (isGround || islocalGround))
        {
            rb.velocity = Vector2.up * playerJump;
            isJump = true;
            isGround = false;
            StartCoroutine(JumpTime());
        }     
    }

    void DownJump()
    {
        if (!isJump && isGround || !islocalGround)
        {
            isJump = true;
            isGround = false;
            StartCoroutine(JumpTime());
        }
    }

    IEnumerator JumpTime()
    {
        cd.enabled = false;
        yield return new WaitForSeconds(0.8f);
        cd.enabled = true;
    }


    void NomalAttack()
    {
        if (!audioSource || !nomalAtttackVoice || isNomalAttack) return;

        isNomalAttack = true;
        audioSource.clip = nomalAtttackVoice;

        audioSource.time = 0;
        audioSource.Play();

        animator.SetBool("IsAttack", true);
        StartCoroutine(AttackTime());
    }

    IEnumerator AttackTime()
    {
        yield return new WaitForSeconds(1f);
        Shoot(AttackType.Nomal);
        Debug.Log("애니매이션 종료");
        animator.SetBool("IsAttack", false);
        yield return new WaitForSeconds(1.0001f);
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

        animator.SetBool("IsAttack", true);
        StartCoroutine(SkillAttackTime());
    }

    IEnumerator SkillAttackTime()
    {
        yield return new WaitForSeconds(1f);
        Shoot(AttackType.Skill);
        Debug.Log("애니매이션 종료");
        animator.SetBool("IsAttack", false);

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
        int offset = (isHorizontalInput_xManus == false) ? 5 : -5;
        transform.position = new Vector3(transform.position.x + offset, transform.position.y, transform.position.z);
    }


    void CrystarGarden()
   {
        if (!audioSource || !skillCrystarGardenVoice|| isultimateVidio|| !ultimateVidio || !videoPlayer) return;

        ultimateVidio.gameObject.SetActive(true);

        videoPlayer.clip = ultimateVidioClip;
        videoPlayer.time = 0;
        videoPlayer.Play();
        isultimateVidio = true;

        audioSource.clip = skillCrystarGardenVoice;
        audioSource.time = 0;
 

        StartCoroutine(CrystarGardenTime());
   }

    IEnumerator CrystarGardenTime()
    {
        ultimateVidio.gameObject.SetActive(true);
        videoPlayer.Play();

        yield return new WaitForSeconds(1f);
        audioSource.Play(); 

        yield return new WaitForSeconds(6f);
        audioSource.Stop();

        yield return new WaitForSeconds(1f);


        videoPlayer.Stop();
        ultimateVidio.gameObject.SetActive(false);

        crystalGardenPenel.gameObject.SetActive(true);


        yield return new WaitForSeconds(30f);
        crystalGardenPenel.gameObject.SetActive(false);
        Debug.Log("크리스탈 가든 효과 종료");

        Debug.Log("크리스탈 가든 대기 쿨타임 : 30초");
        yield return new WaitForSeconds(30f);

        isultimateVidio = false;
        Debug.Log("크리스탈 가든 재사용 가능");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("ground"))
        {
            isGround = true;
            isJump = false;
        }
        if (collision.gameObject.CompareTag("localground"))
        {
            islocalGround = true;
            isJump = false;
        }
    }

    void MovePartal()
    {
        if (currentPortal)
        {
            currentPortal.TryGetComponent<PortalSystem>(out PortalSystem portal);
            string sceneName = SceneManager.GetActiveScene().name;
            if (sceneName != "LumiHouseScene" && sceneName != "MaigicurlHotel")
            {
                StartCoroutine(Fade(portal));
            }
            else
            {
                SceneManager.LoadScene(portal.portalData.targetSceneName);
            }
        }
    }

    IEnumerator Fade(PortalSystem portal)
    {
        fadeManager.StartFadeOut(0.5f);
        yield return new WaitForSeconds(1f);

        string sceneName = SceneManager.GetActiveScene().name;

        SceneManager.LoadScene(portal.portalData.targetSceneName);

        transform.position = new Vector3(portal.portalData.spawnPosition.x, transform.position.y, transform.position.z);

        yield return new WaitForSeconds(1.2f);
         fadeManager.StartFadeIn(0.5f);

        StartCoroutine(PartalTime());
    }

    IEnumerator PartalTime()
    {
        yield return new WaitForSeconds(1.5f);
        isPotalTime = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent<PortalSystem>(out PortalSystem portal))
        {
            Debug.Log($"이동 목적지: {portal.portalData.portalName}");
            currentPortal = collision.gameObject;
        }
        
        // 1차 경계선
        if (collision.gameObject.CompareTag("Localwall"))
            cd.enabled = true;

        // 2차 경계선
        if (collision.gameObject.CompareTag("Localwall2"))
            transform.position = new Vector3(0, transform.position.y, 0);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        currentPortal = null;
    }
}