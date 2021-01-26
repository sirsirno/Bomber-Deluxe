using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeMoveTrap : MonoBehaviour
{
    private GameObject moveTraps = null;
    private GameObject player = null;

    private Vector3 defaultposition;

    public enum MoveType
    {
        LEFT,
        RIGHT,
        UP,
        DOWN
    }

    [SerializeField]
    private float moveSpeed = 1f;
    public MoveType moveType = MoveType.LEFT;
    [Header("��) 1�� ���� -> �ѹ� ���� �ȹߵ��ǰ� �ι� ���� �ߵ�")]
    [Header("������ ��� �����Ұ��� ����")]
    public int waitCount = 0;
    [Header("�ߵ��Ҷ� �����̸� ��")]
    [SerializeField]
    private float delay = 0f;

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
                Invoke("TrapTrigger", delay);
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
            if (moveType == MoveType.LEFT)
                moveTraps.transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
            else if (moveType == MoveType.RIGHT)
                moveTraps.transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
            else if (moveType == MoveType.UP)
                moveTraps.transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);
            else
                moveTraps.transform.Translate(Vector3.down * moveSpeed * Time.deltaTime);
        }

        if (player.GetComponent<PlayerController>().awake && respawn) // ���� ����
        {
            waitCount = defaultWaitCount;
            isTrigger = false;
            moveTraps.transform.position = defaultposition;
            respawn = false;
        }
    }

    private void TrapTrigger()
    {
        isTrigger = true;
        if (player.GetComponent<PlayerController>().sleeping != false)
        {
            respawn = true;
        }
    }
}
