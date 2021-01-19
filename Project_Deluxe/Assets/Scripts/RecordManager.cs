using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordManager : MonoBehaviour
{
    [SerializeField]
    private bool isRecordGame = false;
    private bool isRecrding = false;

    private float recordTime = 0f;
    private float recordDelay = 0.03f;

    [Header("")]
    [Header(" 녹화가 끝났다면 컴포넌트 복사후 붙여넣기")]
    [Header(" Sprite의 사이즈와 레코드 넘버를 동일하게 할것")]
    [Header("레코드 넘버는 1부터 시작, Record Number_XY와")]
    [Header("")]
    public int recordNumber = 0;
    public List<RecordXY> RecordNumber_XY = new List<RecordXY>();
    public List<RecordSprite> RecordNumber_Sprite = new List<RecordSprite>();
    public List<RecordSpriteFlipX> RecordNumber_SpriteFlipX = new List<RecordSpriteFlipX>();
    private PlayerController player = null;
    private GameObject realplayer = null;

    
    private void Start()
    {
        player = FindObjectOfType<PlayerController>();
        realplayer = GameObject.FindGameObjectWithTag("Player");
    }
    private void Update()
    {
        if(isRecordGame)
        {
            if (PlayerController.Instance.sleeping)
            {
                if (!isRecrding)
                {
                    isRecrding = true;
                    RecordNumber_XY[recordNumber - 1].XY.Clear();
                    RecordNumber_Sprite[recordNumber - 1].Sprite.Clear();
                    RecordNumber_SpriteFlipX[recordNumber - 1].SpriteFlipX.Clear();
                    Debug.Log("녹화중");
                }
            }
            else
            {
                isRecrding = false;
            }
        }

        if(isRecrding)
        {
            recordTime = Time.time;
            recordTime = (float)Math.Round(recordTime * 100) / 100;
            //Debug.Log("recordTime : " + recordTime + ", recordDelay : " + recordDelay);
            if (recordTime >= recordDelay)
            {
                RecordNumber_XY[recordNumber - 1].XY.Add(new Vector2(player.transform.localPosition.x, player.transform.localPosition.y));
                RecordNumber_Sprite[recordNumber - 1].Sprite.Add(realplayer.GetComponent<SpriteRenderer>().sprite);
                RecordNumber_SpriteFlipX[recordNumber - 1].SpriteFlipX.Add(realplayer.GetComponent<SpriteRenderer>().flipX);

                recordDelay = recordTime + 0.03f;
                recordTime = 0f;
            }
        }
    }

    [Serializable]
    public class RecordXY
    {
        public List<Vector2> XY;
    }

    [Serializable]
    public class RecordSprite
    {
        public List<Sprite> Sprite;
    }

    [Serializable]
    public class RecordSpriteFlipX
    {
        public List<bool> SpriteFlipX;
    }
}
