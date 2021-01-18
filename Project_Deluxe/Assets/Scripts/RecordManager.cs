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
    private float recordDelay = 0.1f;

    [Header("")]
    [Header(" ��ȭ�� �����ٸ� ������Ʈ ������ �ٿ��ֱ�")]
    [Header(" Sprite�� ������� ���ڵ� �ѹ��� �����ϰ� �Ұ�")]
    [Header("���ڵ� �ѹ��� 1���� ����, Record Number_XY��")]
    [Header("")]
    public int recordNumber = 0;
    public List<RecordXY> RecordNumber_XY = new List<RecordXY>();
    public List<RecordSprite> RecordNumber_Sprite = new List<RecordSprite>();
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

            if (Input.GetKeyDown(KeyCode.R))
            {
                if (!isRecrding)
                {
                    isRecrding = true;
                    Debug.Log("��ȭ��");
                }
                else
                    isRecrding = false;
            }
        }

        if(isRecrding)
        {
            recordTime = Time.time;
            recordTime = (float)Math.Round(recordTime * 10) / 10;
            Debug.Log("recordTime : " + recordTime + ", recordDelay : " + recordDelay);
            if (recordTime >= recordDelay)
            {
                RecordNumber_XY[recordNumber - 1].XY.Add(new Vector2(player.transform.localPosition.x, player.transform.localPosition.y));
                RecordNumber_Sprite[recordNumber - 1].Sprite.Add(realplayer.GetComponent<SpriteRenderer>().sprite);

                recordDelay = recordTime + 0.1f;
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
}
