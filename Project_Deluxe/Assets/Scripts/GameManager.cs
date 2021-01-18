using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : Singleton<GameManager>
{
    [SerializeField]
    private bool isDebugMode = false;
    [SerializeField]
    private GameObject player = null;

    public int coin { get; private set; } = 0;

    GameObject startPoint;
    GameObject invisibleBlockParent;
    GameObject invisibleBlockDownParent;

    private void Awake()
    {
        startPoint = GameObject.FindGameObjectWithTag("StartPoint");
        invisibleBlockParent = GameObject.FindGameObjectWithTag("InvisibleBlocks");
        invisibleBlockDownParent = GameObject.FindGameObjectWithTag("InvisibleBlocksDown");
    }

    private void Start()
    {
        player.transform.localPosition = startPoint.transform.localPosition;

        for (int i = 0; i < invisibleBlockParent.transform.childCount; i++) // 투명블럭 투명화
        {
            invisibleBlockParent.transform.GetChild(i).GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
        }

        for (int i = 0; i < invisibleBlockDownParent.transform.childCount; i++) // 투명블럭 투명화
        {
            invisibleBlockDownParent.transform.GetChild(i).GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
        }

        if (!isDebugMode)
            DisabledDebug();
    }

    private void DisabledDebug()
    {
        GameObject[] greenDebugBlocks = GameObject.FindGameObjectsWithTag("JumpTrigger");

        startPoint.SetActive(false); // 스타트 포인트 안보이게

        for (int i = 0; i < greenDebugBlocks.Length; i++) // 함정 표시 디버그 해제
        {
            if (greenDebugBlocks[i].GetComponent<Tilemap>() != null)
            {
                greenDebugBlocks[i].GetComponent<Tilemap>().color = Color.white;
            }
        }

        GameObject[] trapRanges = GameObject.FindGameObjectsWithTag("TrapRange");
        for (int i = 0; i < trapRanges.Length; i++) // 함정 감지 범위 디버그 해제
        {
            trapRanges[i].GetComponent<SpriteRenderer>().enabled = false;
        }
    }


    //==============코인 엑세스 함수================
    public enum SETTYPE
    {
        SET = 1,
        ADD,
        REMOVE
    }

    public void AccessSetCoin(SETTYPE settype, int value)
    {
        if (settype == SETTYPE.SET)
            coin = value;
        else if (settype == SETTYPE.ADD)
            coin += value;
        else if (settype == SETTYPE.REMOVE)
            coin -= value;
    }
}
