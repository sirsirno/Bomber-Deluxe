using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitPoint : Singleton<ExitPoint>
{
    public bool isPlayerOn { get; private set; } = false;
    private float clearNeedTimer = 0f;
    private float clearNeedDuration = 3f;
    [SerializeField]
    private float clearNeedDurationDefault = 3f;

    private GameObject realPlayer = null;
    private SpriteRenderer spriteRenderer = null;
    public AudioSource exitSFX = null;

    private void Start()
    {
        realPlayer = GameObject.FindGameObjectWithTag("Player");
        spriteRenderer = realPlayer.GetComponent<SpriteRenderer>();
        clearNeedDuration = clearNeedDurationDefault;
    }




    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlayerController" && !PlayerController.Instance.sleeping)
        {
            isPlayerOn = true;
            clearNeedDuration = clearNeedDurationDefault;
            clearNeedDuration += Time.time;
            exitSFX.Play();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlayerController")
        {
            isPlayerOn = false;
            clearNeedTimer = 0f;
            clearNeedDuration = clearNeedDurationDefault;
            exitSFX.Stop();
        }
    }

    private void Update()
    {
        if (isPlayerOn)
        {
            clearNeedTimer = Time.time;
            if (clearNeedTimer >= clearNeedDuration)
            {
                GameObject.Find("UIManager").GetComponent<UIManager>().StageClear();
                isPlayerOn = false;
                exitSFX.Stop();
            }
        }

        float clearTimeRemain = clearNeedDuration - clearNeedTimer;
        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, clearTimeRemain / clearNeedDurationDefault);
        exitSFX.pitch = clearTimeRemain / clearNeedDurationDefault;
    }
}
