using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class characterConteroller : MonoBehaviour
{
    public VariableJoystick joy;

    public float playerSpeed = 5f;
    public float playerJump = 8f;
    public Rigidbody2D rb;
    public Collider2D cd;
    public Animator animator;
    public Button JumpButton;

    public GameObject testPanel;
    public Button testUI;

    private bool isJump;
    private bool isGround;
    private bool islocalGround;
    private bool istextUI;
    private void Start()
    {
        JumpButton.onClick.AddListener(() => { if (joy.Vertical < -0.7f) DownJump(); else Jump();});
        transform.localScale = new Vector3(1.5f, 1.5f, 0);
        testUI.onClick.AddListener(TestUI);
    }

    private void Update()
    {
        // PC 테스트용
        if (Input.GetKeyDown(KeyCode.Space) && (Input.GetKey(KeyCode.DownArrow) || joy.Vertical < -0.7f))
            DownJump();
        else if(Input.GetKeyDown(KeyCode.Space))
            Jump();
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

    void TestUI()
    {
        if (istextUI)
        {
            testPanel.gameObject.SetActive(false);
            istextUI = false;
        }
        else
        {
            testPanel.gameObject.SetActive(true);
            istextUI = true;
        }
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
}
