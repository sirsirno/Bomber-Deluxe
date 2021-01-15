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

    public PlayerState state = PlayerState.Grounded;
    public bool controlEnabled = true;

    private SpriteRenderer spriteRenderer;
    Rigidbody2D rb;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (state == PlayerState.Grounded && Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse); //위방향으로 올라가게함
            state = PlayerState.Jumping;
        }
    }

    void FixedUpdate()
    {
        if (controlEnabled)
        {

            if (Input.GetKey(KeyCode.LeftArrow))    //왼쪽화살표 입력시 실행함
            {
                Vector3 scale = transform.localScale;
                scale.x = -Mathf.Abs(scale.x);
                transform.localScale = scale;
                transform.Translate(Vector3.left * moveSpeed);
                spriteRenderer.flipX = false;
            }

            if (Input.GetKey(KeyCode.RightArrow) )    //오른쪽화살표 입력시 실행함
            {
                Vector3 scale = transform.localScale;
                scale.x = -Mathf.Abs(scale.x);
                transform.localScale = scale;
                transform.Translate(Vector3.right * moveSpeed);
                spriteRenderer.flipX = true;
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
}