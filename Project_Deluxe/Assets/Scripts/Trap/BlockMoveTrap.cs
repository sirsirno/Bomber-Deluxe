using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class BlockMoveTrap : MonoBehaviour
{
    private GameObject moveTraps = null;
    private GameObject player = null;

    private Vector3 defaultposition;

    [Header("좌표가 상대좌표임")]
    [SerializeField]
    private Vector2 ToXY = new Vector2( 0, 0 );
    [SerializeField]
    private float duration = 1f;

    [Header("예) 1로 설정 -> 한번 들어가면 안발동되고 두번 들어가야 발동")]
    [Header("감지를 몇번 무시할건지 설정")]
    public int waitCount = 0;

    private int defaultWaitCount = 0;
    private bool isTrigger = false;
    private bool respawn = false;


    private void Awake()
    {
        defaultWaitCount = waitCount;
        GetComponent<BoxCollider2D>().size = GetComponent<SpriteRenderer>().size;
        player = GameObject.FindGameObjectWithTag("PlayerController");
        moveTraps = transform.parent.gameObject;
        defaultposition = moveTraps.transform.localPosition;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "PlayerController")
        {
            if (waitCount <= 0)
            {
                isTrigger = true;
                if (player.GetComponent<PlayerController>().sleeping != false)
                {
                    respawn = true;
                }
            }
            else
            {
                waitCount--;
            }
        }
    }

    void Update()
    {
        if (isTrigger)
        {
            isTrigger = false;
            GetComponent<BoxCollider2D>().enabled = false;
            moveTraps.transform.DOMove(new Vector2(moveTraps.transform.position.x + ToXY.x, moveTraps.transform.position.y + ToXY.y) , duration).SetEase(Ease.Linear);
        }

        if (player.GetComponent<PlayerController>().awake && respawn) // 함정 리셋
        {
            GetComponent<BoxCollider2D>().enabled = true;
            waitCount = defaultWaitCount;
            moveTraps.transform.position = defaultposition;
            respawn = false;
        }
    }
}
