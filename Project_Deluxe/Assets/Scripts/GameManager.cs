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
    private GameObject realPlayer = null;

    [Header("스탬프 스프라이트")]
    [SerializeField]
    private Sprite[] stampSprites = null;
    [SerializeField]
    private Sprite[] emptyStampSprites = null;

    [Header("타이머")]
    [SerializeField]
    private int gameTimerDefault = 180;
    private int realGameTimer = 180;

    private float gameTimerAddTime = 180;
    private float gameTimerNeedTime = 1f; // 시간이 1 없어질때까지 걸리는 시간

    GameObject startPoint;
    GameObject invisibleBlockParent;
    GameObject invisibleBlockDownParent;
    ScoreManager scoreManager;

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
        realPlayer = GameObject.FindGameObjectWithTag("Player");
        realGameTimer = gameTimerDefault;
        private_currentStage = CurrentStage;
        startPoint = GameObject.FindGameObjectWithTag("StartPoint");
        invisibleBlockParent = GameObject.FindGameObjectWithTag("InvisibleBlocks");
        invisibleBlockDownParent = GameObject.FindGameObjectWithTag("InvisibleBlocksDown");
        scoreManager = FindObjectOfType<ScoreManager>();
        UIManager.Instance.TimerTimeOutput();
    }

    private void Start()
    {
        player.transform.localPosition = startPoint.transform.localPosition;

        if(invisibleBlockParent != null)
        for (int i = 0; i < invisibleBlockParent.transform.childCount; i++) // 투명블럭 투명화
        {
            invisibleBlockParent.transform.GetChild(i).GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
        }

        if (invisibleBlockDownParent != null)
            for (int i = 0; i < invisibleBlockDownParent.transform.childCount; i++) // 투명블럭 투명화
        {
            invisibleBlockDownParent.transform.GetChild(i).GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
        }

        if (!isDebugMode)
            DisabledDebug();

        scoreManager.ScoreValueSet(ScoreManager.ScoreType.STAMPTEMP, ScoreManager.SetType.SET, 0);
        scoreManager.ScoreValueSet(ScoreManager.ScoreType.FEED, ScoreManager.SetType.SET, 0);
        scoreManager.ScoreValueSet(ScoreManager.ScoreType.ABILITYUSECOUNT, ScoreManager.SetType.SET, 0);

        UIManager.Instance.FutureCountOutput();
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

    public void PlayerDeadState()
    {
        scoreManager.ScoreValueSet(ScoreManager.ScoreType.LIFE, ScoreManager.SetType.REMOVE, 1);
        scoreManager.ScoreValueSet(ScoreManager.ScoreType.FEED, ScoreManager.SetType.SET, 0);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void Update()
    {
        if(realGameTimer <= -1)
        {
            if (PlayerController.Instance.state != PlayerController.PlayerState.Dead)
            {
                player.GetComponent<HitDeath>().HitDeathPlay();
            }
            return;
        }

        float timer = Time.timeSinceLevelLoad;
        if (PlayerController.Instance.sleeping)
            gameTimerNeedTime = 0.5f;
        else
            gameTimerNeedTime = 1f;
        if(PlayerController.Instance.controlEnabled || realPlayer.GetComponent<Animator>().GetInteger("PlayerAnimation") == 3)
        {
            if(timer >= gameTimerAddTime)
            {
                realGameTimer--;
                if (realGameTimer > -1)
                {
                    UIManager.Instance.TimerTimeOutput();
                    if (realGameTimer <= 10)
                    {
                        AudioManager.Instance.SFX_ClockTic.Play();
                        GlitchEffect.Instance.colorIntensity = 0.306f;
                        GlitchEffect.Instance.intensity = 0.194f;
                    }
                }
                gameTimerAddTime = timer + gameTimerNeedTime;
            }
        }
        else
        {
            gameTimerAddTime = timer + gameTimerNeedTime;
        }
    }

    public int GetRealTimer() => realGameTimer;

    public Sprite GetStampSprite(bool isEmpty, int number)
    {
        if (isEmpty)
        {
            if (number == 0)
                return emptyStampSprites[0];
            else if (number == 1)
                return emptyStampSprites[1];
            else
                return emptyStampSprites[2];
        }
        else
        {
            if (number == 0)
                return stampSprites[0];
            else if (number == 1)
                return stampSprites[1];
            else
                return stampSprites[2];
        }
    }
}
