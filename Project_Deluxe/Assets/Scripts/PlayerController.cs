using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Singleton<PlayerController>
{
    public AudioSource jumpAudio;
    public AudioSource jumpWingAudio;
    public AudioSource coinGetAudio;
    public AudioSource badCoinGetAudio;
    public AudioSource ouchAudio;
    public AudioSource headBlockAudio;

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

    public bool sleeping { get; private set; }  = false; // 미래예지중
    [SerializeField]
    private float sleepingDurationDefault = 0; // 미래예지중
    private float sleepingDuration = 0; // 미래예지중
    public bool awake = false;
    [SerializeField]
    private GameObject sleepingPlayer = null;
    [SerializeField]
    private GameObject sandClockEffect = null;
    private GroundCheck groundCheck = null;

    private SpriteRenderer spriteRenderer;
    private Animator animator;
    Rigidbody2D rb;

    public float clockDuration = 15;

    /// <summary>
    /// 스프라이트 상의 플레이어 (애니메이션용)
    /// </summary>
    private GameObject realPlayer = null;
    [SerializeField]
    private GameObject futureEffect = null;
    void Awake()
    {
        realPlayer = GameObject.FindGameObjectWithTag("Player");
        groundCheck = FindObjectOfType<GroundCheck>();
        animator = realPlayer.GetComponent<Animator>();
        spriteRenderer = realPlayer.GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // ----------------------확인차 만들어 놓음---------------
        if (Input.GetKeyDown(KeyCode.Q) && state == PlayerState.Grounded && !ExitPoint.Instance.isPlayerOn)
        {
            if (!sleeping)
            {
                Debug.Log("---------미래-------");

                futureEffect.GetComponent<Animator>().Play("FutureEffect_Blue");
                GlitchEffect.Instance.colorIntensity = 0.306f;
                GlitchEffect.Instance.intensity = 0.194f;
                realPlayer.GetComponent<SpriteRenderer>().color = new Color(0, 1, 1, 1);
                sleepingPlayer.SetActive(true);
                sleepingPlayer.transform.localPosition = transform.localPosition;
                sleepingPlayer.GetComponent<SpriteRenderer>().flipX = spriteRenderer.flipX;
                sleepingPlayer.transform.GetChild(0).gameObject.GetComponent<Animator>().Play("SleepSpeechBubble_Sleeping");
                AudioManager.Instance.BGM_FutureRandomPlay();
                AudioManager.Instance.SFX_FutureEnter.Play();
                AudioManager.Instance.BGM_Prototype.volume = 0f;

                sleeping = true;
                sleepingDuration = sleepingDurationDefault;
                sleepingDuration += Time.time;
            }
        }

        if (sleeping)
        {
            float sleepTime = Time.time;

            if (animator.GetInteger("PlayerAnimation") != 4 && animator.GetInteger("PlayerAnimation") != 5)
                clockDuration = sleepingDuration - sleepTime;

            if (sleepTime >= sleepingDuration || PlayerStopEvent.Instance.isFutureDead)
            {
                if (animator.GetInteger("PlayerAnimation") != 4 && animator.GetInteger("PlayerAnimation") != 5)
                {
                    sleeping = false;
                    awake = true;
                    Invoke("ResetTrap", 0.1f);

                    futureEffect.GetComponent<Animator>().Play("FutureEffect_Yellow");
                    GlitchEffect.Instance.colorIntensity = 0f;
                    GlitchEffect.Instance.intensity = 0f;
                    realPlayer.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
                    transform.localPosition = sleepingPlayer.transform.localPosition;
                    spriteRenderer.flipX = sleepingPlayer.GetComponent<SpriteRenderer>().flipX;
                    animator.SetInteger("PlayerAnimation", 0);
                    state = PlayerState.Grounded;
                    controlEnabled = true;
                    animator.Play("Player_Idle");
                    groundCheck.GetComponent<GroundCheck>().enabled = true;
                    GetComponent<Rigidbody2D>().simulated = true;
                    PlayerStopEvent.Instance.isFutureDead = false;

                    sleepingPlayer.transform.GetChild(0).gameObject.GetComponent<Animator>().Play("SleepSpeechBubble_Awake");
                    sleepingPlayer.transform.GetChild(0).transform.SetParent(gameObject.transform);

                    AudioManager.Instance.BGM_FutureBGMStop();
                    AudioManager.Instance.BGM_Prototype.volume = 1f;

                    sleepingPlayer.SetActive(false);

                    Debug.Log("-------현재--------");
                }
            }
        }

        //=========================모래시계이펙트========================
        {
            if (!sleeping)
            {
                sandClockEffect.SetActive(true);
                if (state == PlayerState.Grounded && !ExitPoint.Instance.isPlayerOn)
                    sandClockEffect.GetComponent<Animator>().Play("SandClock_Able");
                else
                    sandClockEffect.GetComponent<Animator>().Play("SandClock_Unable");
            }
            else
                sandClockEffect.SetActive(false);
        }

        //=========================컨트롤================================
        {
            if (controlEnabled)
            {
                if (state == PlayerState.Grounded && Input.GetButtonDown("Jump"))
                {
                    jump = true;
                    jumpAudio.Play();
                }

                if (!Input.GetButton("Jump") || jumpTimer >= jumpTimeLimit)
                {
                    jump = false;
                    jumpTimer = 0f;
                    return;
                }

                if (jump)
                {
                    rb.velocity = Vector2.zero;
                    rb.AddForce(Vector2.up * jumpSpeed * ((jumpTimer * 1.3f) + 1f), ForceMode2D.Impulse); //위방향으로 올라가게함
                    jumpTimer += Time.deltaTime;
                }
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
                    transform.Translate(Vector3.left * moveSpeed * Time.fixedDeltaTime);
                else if (state == PlayerState.Jumping)
                    transform.Translate(Vector3.left * moveSpeed / 1.5f * Time.fixedDeltaTime);
                spriteRenderer.flipX = true;
            }

            if (Input.GetKey(KeyCode.RightArrow))    //오른쪽화살표 입력시 실행함
            {
                if (state == PlayerState.Grounded && animator.GetInteger("PlayerAnimation") == 1)
                    transform.Translate(Vector3.right * moveSpeed * Time.fixedDeltaTime);
                else if (state == PlayerState.Jumping)
                    transform.Translate(Vector3.right * moveSpeed / 1.5f * Time.fixedDeltaTime);
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

            jumpWingAudio.Stop();
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

    private void ResetTrap()
    {
        awake = false;
    }

    public void GameReset()
    {
        awake = true;
        Invoke("ResetTrap", 0.1f);
    }
}