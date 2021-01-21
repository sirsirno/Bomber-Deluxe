using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : Singleton<PlayerController>
{
    [Header("플레이어 효과음")]
    public AudioSource jumpAudio;
    public AudioSource jumpWingAudio;
    public AudioSource coinGetAudio;
    public AudioSource badCoinGetAudio;
    public AudioSource ouchAudio;
    public AudioSource headBlockAudio;
    public AudioSource landAudio;

    [Header("컨트롤 관련")]
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
    [Header("능력 관련")]
    [SerializeField]
    private float sleepingDurationDefault = 0; // 미래예지중
    private float sleepingDuration = 0; // 미래예지중
    public bool awake = false;
    private bool futureAbillityAbled = false;
    [SerializeField]
    private float futureAbillityCoolDown = 5f;
    private float futureAbillityCoolDownRemaining = 5f;
    [SerializeField]
    private GameObject sleepingPlayer = null;
    [SerializeField]
    private GameObject sandClockEffect = null;
    private bool sandClockAbled = true;
    private GroundCheck groundCheck = null;

    private SpriteRenderer spriteRenderer;
    private ScoreManager scoreManager;
    private Animator animator;
    Rigidbody2D rb;

    public float clockDuration = 15;

    /// <summary>
    /// 스프라이트 상의 플레이어 (애니메이션용)
    /// </summary>
    private GameObject realPlayer = null;
    [SerializeField]
    private GameObject futureEffect = null;

    [Header("먹이 효과")]
    [SerializeField]
    private GameObject feedTxtEffect = null;
    [SerializeField]
    private Text feedTxtEffectText = null;
    [SerializeField]
    private float feedTxtEffectDuration = 1f;
    void Awake()
    {
        realPlayer = GameObject.FindGameObjectWithTag("Player");
        groundCheck = FindObjectOfType<GroundCheck>();
        animator = realPlayer.GetComponent<Animator>();
        spriteRenderer = realPlayer.GetComponent<SpriteRenderer>();
        scoreManager = FindObjectOfType<ScoreManager>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // ----------------------확인차 만들어 놓음---------------
        if (Input.GetKeyDown(KeyCode.Q) && state == PlayerState.Grounded && !ExitPoint.Instance.isPlayerOn)
        {
            if (!sleeping && controlEnabled && futureAbillityAbled)
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
                AudioManager.Instance.BGM_World.volume = 0f;

                sleeping = true;
                sleepingDuration = sleepingDurationDefault;
                sleepingDuration += Time.time;

                futureAbillityAbled = false;
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
                    sleepingPlayer.SetActive(false);

                    AudioManager.Instance.BGM_FutureBGMStop();
                    AudioManager.Instance.SFX_PresentEnter.Play();
                    AudioManager.Instance.BGM_World.volume = 1f;

                    futureAbillityCoolDownRemaining = Time.time + futureAbillityCoolDown;
                    Debug.Log("-------현재--------");
                }
            }
        }
        else // (!sleeping)
        {
            float timer = Time.time;
            if (timer >= futureAbillityCoolDownRemaining)
                futureAbillityAbled = true;
        }

        //=========================모래시계이펙트========================
        {
            if (!sleeping && futureAbillityAbled && sandClockAbled)
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
        //=========================컨트롤(위에다가 코드 쓰세요 쓸꺼면)================================
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

    public void ShowFeedRemaining()
    {
        feedTxtEffectText.text = string.Format("{0} / 8", scoreManager.ScoreValueGet(ScoreManager.ScoreType.FEED));
        feedTxtEffect.SetActive(true);
        sandClockAbled = false;
        Invoke("HideFeedRemaining", feedTxtEffectDuration);
    }

    private void HideFeedRemaining()
    {
        feedTxtEffect.SetActive(false);
        sandClockAbled = true;
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