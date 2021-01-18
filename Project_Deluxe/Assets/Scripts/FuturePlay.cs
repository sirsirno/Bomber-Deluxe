using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuturePlay : MonoBehaviour
{
    [SerializeField]
    private bool isFuturePlay = false;

    [SerializeField]
    private int playScene = 0;

    private float playTime = 0f;
    private float playDelay = 0.03f;
    private int playframe = 0;

    private GameManager gameManager = null;
    private RecordManager rm = null;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        rm = gameManager.GetComponent<RecordManager>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (!isFuturePlay)
            {
                isFuturePlay = true;
                Debug.Log("미래예지중");
            }
            else
            {
                isFuturePlay = false;
                playTime = 0f;
                playDelay = 0.03f;
                playframe = 0;
            }
        }

        if (isFuturePlay)
        {
            playTime = Time.time;
            playTime = (float)Math.Round(playTime * 100) / 100;
            if (playTime >= playDelay)
            {
                Vector2 futurePlayerXY = new Vector2(rm.RecordNumber_XY[playScene - 1].XY[playframe].x, rm.RecordNumber_XY[playScene - 1].XY[playframe].y);
                Sprite futurePlayerSprite = rm.RecordNumber_Sprite[playScene - 1].Sprite[playframe];
                bool futurePlayerSpriteFlipX = rm.RecordNumber_SpriteFlipX[playScene - 1].SpriteFlipX[playframe];

                transform.localPosition = futurePlayerXY;
                GetComponent<SpriteRenderer>().sprite = futurePlayerSprite;
                GetComponent<SpriteRenderer>().flipX = futurePlayerSpriteFlipX;

                playDelay = playTime + 0.03f;
                playTime = 0f;
                playframe++;
            }
        }
    }
}
