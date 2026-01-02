using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;
using static UnityEngine.InputManagerEntry;
public class characterConteroller : MonoBehaviour
{
    public VariableJoystick joy;

    public float playerSpeed = 5f;
    public float playerJump = 8f;
    public Rigidbody2D rb;
    public Collider2D cd;
    public Animator animator;
    public Button JumpButton;
   
    public GameObject ultimateVidio;
    public Button ultimateButtonUI;
    public VideoPlayer videoPlayer;
    public VideoClip ultimateVidioClip;

    public PortalSystem portalSystem;
    public GameObject currentPortal;

    private bool isultimateVidio;
    private bool isJump;
    private bool isGround;
    private bool islocalGround;



    private void Start()
    {
        JumpButton.onClick.AddListener(() => { if (joy.Vertical < -0.7f) DownJump(); else Jump(); }); 
        transform.localScale = new Vector3(1.5f, 1.5f, 0);
        ultimateButtonUI.onClick.AddListener(Ultimate);

        if(!portalSystem)
            portalSystem = FindAnyObjectByType<PortalSystem>();
    }

    private void Update()
    {
        // PC 테스트용
        if (Input.GetKeyDown(KeyCode.Space) && (Input.GetKey(KeyCode.DownArrow) || joy.Vertical < -0.7f))
            DownJump();
        else if(Input.GetKeyDown(KeyCode.Space))
            Jump();

        if (currentPortal != null && joy.Vertical > 0.7f || Input.GetKeyDown(KeyCode.UpArrow))
            MovePartal();
    }

    private void FixedUpdate()
    {
        if(joy.Vertical > 0.7f || joy.Vertical < -0.7f || isultimateVidio)
        {
            Debug.Log("상하 움직임은 지원하지 않습니다.");
        }
        else
          charcterMove();

    }

    void charcterMove()
    {
        float HorizontalInput = (Input.GetAxisRaw("Horizontal") != 0) ? Input.GetAxisRaw("Horizontal") : joy.Horizontal;
        if (HorizontalInput < 0) Flip(true);
        else  Flip(false);

        Vector3 Derection = new Vector3(HorizontalInput, 0, 0).normalized;
        Vector3 move = Derection * playerSpeed * Time.deltaTime;
        transform.position += move;

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

   void Ultimate()
   {
        if (ultimateVidio == null) return;
        ultimateVidio.gameObject.SetActive(true);

        if (videoPlayer == null) return;
        videoPlayer.clip = ultimateVidioClip;
        videoPlayer.time = 0;
        videoPlayer.Play();
       
       
        StartCoroutine(ultimateVidioTime());
   }

    IEnumerator ultimateVidioTime()
    {
        yield return new WaitForSeconds(8f);
        videoPlayer.Stop();
        ultimateVidio.gameObject.SetActive(false);
        isultimateVidio = false;
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
            SceneManager.LoadScene(portal.portalData.targetSceneName);

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent<PortalSystem>(out PortalSystem portal))
        {
            Debug.Log($"이동 목적지: {portal.portalData.portalName}");
            currentPortal = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        currentPortal = null;
    }

}
