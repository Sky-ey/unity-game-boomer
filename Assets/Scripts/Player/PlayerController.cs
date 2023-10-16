using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;

    public float Speed;
    public float JumpForce;
    public float DoubleJumpForce;

    [Header("StateCheck")]

    public bool canDoubleJump;
    public bool jumpPressed;
    public bool isGround;
    public Transform groundCheck;
    public float checkRadius;
    public LayerMask groundLayer;
    public bool inAir;

    [Header("GetFXObjects")]

    public GameObject jumpFX;
    public GameObject fallFX;

    [Header("Attack")]

    public GameObject BoomPerf;
    public int AttackCount,NowCount;
    public float AttackRate;
    public float LastRecovery;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

    }

    private void FixedUpdate()
    {
        Movement();
        Jump();
        StateCheck();
        FallFXCheck();
    }

    void Update()
    {
        CheckInput();
        StateUpdate();
        AttackRecovery();
    }

    //输入检测
    void CheckInput()
    {
        if (Input.GetButtonDown("Jump"))
        {
            jumpPressed = true;
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            Attack();
        }
    }

    //移动相关
    void Movement()
    {
        float horizentalInput = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(horizentalInput * Speed, rb.velocity.y);
        if (horizentalInput != 0)
        {
            transform.localScale = new Vector3(horizentalInput, 1, 1);
        }
    }
    void Jump()
    {
        if (jumpPressed)
        {
            if (isGround)
            {
                JumpFX();
                rb.velocity = new Vector2(rb.velocity.x, JumpForce);
                jumpPressed = false;
            }
            else
            {
                if (canDoubleJump)
                {
                    rb.velocity = new Vector2(rb.velocity.x, DoubleJumpForce);
                    jumpPressed = false;
                    canDoubleJump = false;
                }
            }
        }
    }
    void StateCheck()
    {
        isGround = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);
    }

    void StateUpdate()
    {
        if (isGround)
        {
            canDoubleJump = true;
        }
        else
        {
            inAir = true;
        }
    }

    //特效相关
    void FallFXCheck()
    {
        if (isGround && inAir)
        {
            FallFX();
        }
    }
    void FallFX()
    {
        fallFX.SetActive(true);
        fallFX.transform.position = transform.position + new Vector3(0, -0.91f, 0);
        inAir = false;
    }

    void JumpFX()
    {
        jumpFX.SetActive(true);
        jumpFX.transform.position = transform.position + new Vector3(0, -0.45f, 0);
    }

    public void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position,checkRadius);
    }

    //攻击相关
    void AttackRecovery()
    {
        if(NowCount < AttackCount)
        {
            if(Time.time >= LastRecovery + AttackRate)
            {
                NowCount++;
                LastRecovery = Time.time;
            }
        }
    }

    void Attack()
    {
        if (NowCount > 0)
        {
            Instantiate(BoomPerf,transform.position,BoomPerf.transform.rotation);
            NowCount--;
        }
    }
}
