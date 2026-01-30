using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class CharacterMoveSystem : MonoBehaviour
{
    public VariableJoystick joy;

    public float playerSpeed = 5f;
    public float playerJump = 8f;
    public Rigidbody2D rb;
    public Collider2D cd;
    public Animator animator;
    public Button JumpButton;
  
    public PortalSystem portalSystem;
    public GameObject currentPortal;
    public FadeManager fadeManager;

    public bool isHorizontalInput_xManus;
    public bool isNotInGameScene;

    private bool isJump;
    private bool isGround;
    private bool islocalGround;
    private bool isPotalTime;
    private bool isFade;
    
    private void Start()
    {
        JumpButton.onClick.AddListener(() => { if (joy.Vertical < -0.7f) DownJump(); else Jump(); });
        transform.localScale = new Vector3(1.5f, 1.5f, 0);

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
        if (Input.GetKeyDown(KeyCode.Space) && (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow) || joy.Vertical < -0.7f))
            DownJump();
        else if(Input.GetKeyDown(KeyCode.Space))
            Jump();


        if ((currentPortal != null && joy.Vertical > 0.7f || (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && !isPotalTime))
        {
            MovePartal();
        }
    }

    private void FixedUpdate()
    {
        if(joy.Vertical > 0.7f || joy.Vertical < -0.7f)
            Debug.Log("상하 움직임은 지원하지 않습니다.");
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
        animator.SetBool("IsJump", true);
        yield return new WaitForSeconds(0.8f);
        animator.SetBool("IsJump", false);
        cd.enabled = true;
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

        yield return new WaitForSeconds(1f);
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
        {
            transform.position = new Vector3(0, transform.position.y, 0);
        }
          
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        currentPortal = null;
    }
}