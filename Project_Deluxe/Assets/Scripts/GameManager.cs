using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    [SerializeField]
    private bool isDebugMode = false;
    [SerializeField]
    private GameObject player = null;

    public int coin { get; private set; } = 0;
    
    public int life { get; private set; } = 5;
    public int star { get; private set; } = 0;

    GameObject startPoint;
    GameObject invisibleBlockParent;
    GameObject invisibleBlockDownParent;

    public enum WorldType
    {
        SKY,
        FOREST,
        TEMPLE,
        MECHA_TEMPLE,
        DUNGEON
    }

    public WorldType worldType = WorldType.SKY;
    [Header("")]
    [Header("0 : 프로토타입, 1~5는 하늘 이런식")]
    [Header("현재 스테이지 번호 입력")]
    [Header("")]
    public int CurrentStage = 0;
    private int private_currentStage = 0;
    public int GetCurrentStage() => private_currentStage;
    private void Awake()
    {
        private_currentStage = CurrentStage;
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


    public void PlayerDeadState()
    {
        life--;
        if (life == -1)
        {
            GameObject.Find("UIManager").GetComponent<UIManager>().StageFail();
            //GameOverState();
            return;
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void GameOverState()
    {
        
    }
}
