using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private bool isDebugMode = false;
    [SerializeField]
    private GameObject player = null;

    GameObject startPoint;
    GameObject trapParent;
    GameObject invisibleBlockParent;
    GameObject invisibleBlockDownParent;

    private void Awake()
    {
        startPoint = GameObject.FindGameObjectWithTag("StartPoint");
        trapParent = GameObject.FindGameObjectWithTag("Trap");
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
        startPoint.SetActive(false); // 스타트 포인트 안보이게

        for (int i = 0; i < trapParent.transform.childCount; i++) // 함정 표시 디버그 해제
        {
            if (trapParent.transform.GetChild(i).GetComponent<Tilemap>() != null)
            {
                trapParent.transform.GetChild(i).GetComponent<Tilemap>().color = Color.white;
            }
        }

        GameObject[] trapRanges = GameObject.FindGameObjectsWithTag("TrapRange");
        for (int i = 0; i < trapRanges.Length; i++) // 함정 감지 범위 디버그 해제
        {
            trapRanges[i].GetComponent<SpriteRenderer>().enabled = false;
        }
    }
}
