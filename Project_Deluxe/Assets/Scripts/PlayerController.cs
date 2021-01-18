using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Singleton<PlayerController>
{
    public AudioClip jumpAudio;
    public AudioClip respawnAudio;
    public AudioClip ouchAudio;

    /// <summary>
    /// 플레이어 이동속도
    /// </summary>
    public float moveSpeed = 5f;

    /// <summary>
    /// 플레이어 점프속도
    /// </summary>
    public float jumpSpeed = 5f;
    public float jumpTimeLimit = 0.5f;
    private float jumpTimer = 0;
    private bool jump = false;

    public PlayerState state = PlayerState.Grounded;
    public bool controlEnabled = true;
    public bool sleeping = false; // 미래예지중
    public bool awake = false;

    private SpriteRenderer spriteRenderer;
    private Animator animator;
    Rigidbody2D rb;

    /// <summary>
    /// 스프라이트 상의 플레이어 (애니메이션용)
    /// </summary>
    private GameObject realPlayer = null;
    void Awake()
    {
        realPlayer = GameObject.FindGameObjectWithTag("Player");

        animator = realPlayer.GetComponent<Animator>();
        spriteRenderer = realPlayer.GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // ----------------------확인차 만들어 놓음---------------
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("---------미래-------");
            sleeping = true;
            Invoke("Wakeup", 5);
        }
        if (controlEnabled)
        {
            if (state == PlayerState.Grounded && Input.GetButtonDown("Jump"))
            {
                jump = true;
            }

            if (!Input.GetButton("Jump") || jumpTimer >= jumpTimeLimit)
            {
                jump = false;
                jumpTimer = 0f;
                return;
            }

            if(jump)
            {
                rb.velocity = Vector2.zero;
                rb.AddForce(Vector2.up * jumpSpeed * ((jumpTimer * 1.3f) + 1f), ForceMode2D.Impulse); //위방향으로 올라가게함
                jumpTimer += Time.deltaTime;
            }
        }
    }

    void FixedUpdate()
    {
        if (controlEnabled)
        {
            if (Input.GetKey(KeyCode.LeftArrow))    //왼쪽화살표 입력시 실행함
            {
                if (state == PlayerState.Grounded && animator.GetInteger("PlayerAnimation") == 1)
                    transform.Translate(Vector3.left * moveSpeed);
                else if (state == PlayerState.Jumping)
                    transform.Translate(Vector3.left * moveSpeed / 1.5f);
                spriteRenderer.flipX = true;
            }

            if (Input.GetKey(KeyCode.RightArrow))    //오른쪽화살표 입력시 실행함
            {
                if (state == PlayerState.Grounded && animator.GetInteger("PlayerAnimation") == 1)
                    transform.Translate(Vector3.right * moveSpeed);
                else if (state == PlayerState.Jumping)
                    transform.Translate(Vector3.right * moveSpeed / 1.5f);
                spriteRenderer.flipX = false;
            }

            AnimationUpdate();
        }
    }

    private void AnimationUpdate()
    {
        // PlayerAnimation
        // 0 -> Idle
        // 1 -> Walk
        // 2 -> Jumping
        // 3 -> After Jump Wait
        // 4 -> Death
        // 5 -> FallenDeath

        if (state == PlayerState.Grounded)
        {
            if (Input.GetKey(KeyCode.LeftArrow) && animator.GetInteger("PlayerAnimation") == 0)
            {
                animator.Play("Player_Walk");
                animator.SetInteger("PlayerAnimation", 1);
            }

            if (Input.GetKey(KeyCode.RightArrow) && animator.GetInteger("PlayerAnimation") == 0)
            {
                animator.Play("Player_Walk");
                animator.SetInteger("PlayerAnimation", 1);
            }

            if (!Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow))
            {
                if (animator.GetInteger("PlayerAnimation") == 0 || animator.GetInteger("PlayerAnimation") == 1)
                {
                    animator.Play("Player_Idle");
                    animator.SetInteger("PlayerAnimation", 0);
                }
            }

            if (animator.GetInteger("PlayerAnimation") == 2)
            {

            }
        }

        if (state == PlayerState.Jumping)
        {
            if (animator.GetInteger("PlayerAnimation") == 0 || animator.GetInteger("PlayerAnimation") == 1)
            {
                animator.Play("Player_InputJump");
                animator.SetInteger("PlayerAnimation", 2);
            }
        }
    }

    public enum PlayerState
    {
        Grounded,
        Jumping,
        Dead,
        Stop
    }

    private void Wakeup()
    {
        if (sleeping != true)
            return;
        sleeping = false;
        awake = true;
        Invoke("ResetTrap", 0.01f);
    }
    private void ResetTrap()
    {
        awake = false;
    }
}