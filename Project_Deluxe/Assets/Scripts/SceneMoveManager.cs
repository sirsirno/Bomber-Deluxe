using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class SceneMoveManager : MonoBehaviour
{


    public GameObject StageBlur;
    private ScoreManager scoreManager = null;
    [SerializeField]
    private float invokeDelay = 1f;

    [SerializeField]
    private GameObject startPanel = null;
    [SerializeField]
    private GameObject mainCamera = null;
    [SerializeField]
    private GameObject backgroundCamera = null;
    [SerializeField]
    private GameObject[] stageBackgrounds = null;
    [SerializeField]
    private GameObject stageCanvas = null;
    [SerializeField]
    private GameObject[] worldStages = null;
    [SerializeField]
    private Text[] worldStagePercentTexts = null;
    [SerializeField]
    private GameObject worldExit = null;
    [SerializeField]
    private Sprite[] starSprites = null;
    [SerializeField]
    private GameObject stageInfo = null;
    [SerializeField]
    private Text stageInfoText = null;
    [SerializeField]
    private Text stageInfoPercentText = null;
    [SerializeField]
    private GameObject stageInfoStar = null;
    [SerializeField]
    private GameObject stageInfoStamp = null;
    [SerializeField]
    private Sprite[] stageButton = null;
    [SerializeField]
    private Sprite[] EmptyStamps = null;
    [SerializeField]
    private Sprite[] Stamps = null;
    [SerializeField]
    private RectTransform[] Stages = null;
    [SerializeField]
    private GameObject block = null;
    private int worldType_ = 0;
    private int currentShowStage = 0;

    [Header("스크롤 표시 화살표")]
    [SerializeField]
    private GameObject[] scrollShowArrows = null;
    [SerializeField]
    private GameObject scroll = null;

    private void Start()
    {
        scoreManager = FindObjectOfType<ScoreManager>();
        TitleMove(scoreManager.WorldTypeGet);
        scrollShowArrows[0].GetComponent<RectTransform>().DOAnchorPosY(30, 0.5f).SetLoops(-1, LoopType.Yoyo);
        scrollShowArrows[1].GetComponent<RectTransform>().DOAnchorPosY(-30, 0.5f).SetLoops(-1, LoopType.Yoyo);
    }

    public void TitleMove(int worldType)
    {
        if (scoreManager.WorldTypeGet == -1)
            return;

        scoreManager.WorldTypeSet(-1);
        startPanel.SetActive(false);
        mainCamera.transform.localPosition = new Vector3(-1.08f, -800, -37);
        backgroundCamera.transform.localPosition = new Vector3(30.5f, -33.9f, -37);
        stageCanvas.SetActive(true);
        worldStages[0].SetActive(false);
        worldStages[1].SetActive(false);
        worldStages[2].SetActive(false);
        worldStages[3].SetActive(false);
        worldStages[4].SetActive(false);

        worldStages[worldType].SetActive(true);
        stageBackgrounds[worldType].SetActive(true);
        worldExit.SetActive(true);
        worldType_ = worldType;
        //Debug.Log(Stages[worldType].position.x + " " + Stages[worldType].position.y + " " + worldType) ;
        Vector3 stagePosition = new Vector3(Stages[worldType].position.x, Stages[worldType].position.y + 18.4213f, -37);
        mainCamera.transform.DOMove(stagePosition, 0);
        mainCamera.GetComponent<Camera>().DOOrthoSize(1, 0);

        {
            int[] stars = new int[5];
            int[] stamps = new int[5];

            for (int i = 0; i < 5; i++)
            {
                stars[i] = JsonSave.Instance.gameData.StageGetValueSave(GameData.StageValueType.STAR, worldType * 5 + i + 1);
                stamps[i] = JsonSave.Instance.gameData.StageGetValueSave(GameData.StageValueType.STAMP, worldType * 5 + i + 1, true);
            }

            int percentCount = 0;
            for (int i = 0; i < 5; i++)
            {
                percentCount += stars[i];
                percentCount += stamps[i];
            }

            float percent = 3.3f * percentCount;
            if (percent >= 99)
                percent = 100;
            worldStagePercentTexts[worldType].text = string.Format("{0}%", percent);
        }
    }

    public void WorldButtonClick(int worldType)
    {
        AllAudioManager.Instance.uiClick.Play();
        worldType_ = worldType;
        stageCanvas.SetActive(true);
        worldStages[0].SetActive(false);
        worldStages[1].SetActive(false);
        worldStages[2].SetActive(false);
        worldStages[3].SetActive(false);
        worldStages[4].SetActive(false);
        block.SetActive(true);
        Vector3 stagePosition = new Vector3(Stages[worldType].position.x, Stages[worldType].position.y, -37);
        SetOnStageBlur();
        //블러 해제
        Invoke("SetDownStageBlur", invokeDelay);
        mainCamera.transform.DOMove(stagePosition, 0.5f).SetEase(Ease.InCubic);
        mainCamera.GetComponent<Camera>().DOOrthoSize(1, 0.5f).SetEase(Ease.InCubic).OnComplete(ShowStage);

        {
            int[] stars = new int[5];
            int[] stamps = new int[5];

            for (int i = 0; i < 5; i++)
            {
                stars[i] = JsonSave.Instance.gameData.StageGetValueSave(GameData.StageValueType.STAR, worldType * 5 + i + 1);
                stamps[i] = JsonSave.Instance.gameData.StageGetValueSave(GameData.StageValueType.STAMP, worldType * 5 + i + 1, true);
            }

            int percentCount = 0;
            for (int i = 0; i < 5; i++)
            {
                percentCount += stars[i];
                percentCount += stamps[i];
            }

            float percent = 3.3f * percentCount;
            if (percent >= 99)
                percent = 100;
            worldStagePercentTexts[worldType].text = string.Format("{0}%", percent);
        }
    }
    private void ShowStage()
    {
        block.SetActive(false);
        stageBackgrounds[worldType_].SetActive(true);
        worldStages[worldType_].SetActive(true);
        worldExit.SetActive(true);
    }
    private void StopBolck()
    {
        block.SetActive(false);
    }
    public void WorldExit()
    {
        AllAudioManager.Instance.uiClick.Play();
        worldStages[worldType_].SetActive(false);
        stageBackgrounds[worldType_].SetActive(false);
        worldExit.SetActive(false);
        stageCanvas.SetActive(false);

        block.SetActive(true);
        mainCamera.transform.DOMove(new Vector3(-1f ,-800f, -37), 0.5f).SetEase(Ease.InCubic);
        mainCamera.GetComponent<Camera>().DOOrthoSize(5, 0.5f).SetEase(Ease.InCubic).OnComplete(StopBolck);
        stageInfo.SetActive(false);
        currentShowStage = 0;
    }

    public void TitleReset()
    {
        scoreManager.isTitleBegin = true;
        SceneManager.LoadScene("Title");
    }

    public void StartStage()
    {
        AllAudioManager.Instance.gameStart.Play();
        SceneManager.LoadScene(currentShowStage + 1);
    }

    public void StageButtonClick(int stageNumber)
    {
        AllAudioManager.Instance.uiClick.Play();
        if (!stageInfo.activeSelf)
            stageInfo.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 517, 0);
        stageInfo.SetActive(true);
        currentShowStage = stageNumber;
        stageInfoStar.GetComponent<ShowStar>().StageNumberSet(stageNumber);
        stageInfoStar.GetComponent<ShowStar>().ShowStars();
        stageInfoStamp.GetComponent<ShowStamp>().StageNumberSet(stageNumber);
        stageInfoStamp.GetComponent<ShowStamp>().ShowStamps();

        // 맵 이름
        {
            stageInfoText.text = "";
            stageInfoText.fontSize = 47;

            if (stageNumber <= 5)
                stageInfoText.text += "SKY";
            else if (stageNumber <= 10)
                stageInfoText.text += "FOREST";
            else if (stageNumber <= 15)
                stageInfoText.text += "TEMPLE";
            else if (stageNumber <= 20)
            {
                stageInfoText.text += "MECHA TEMPLE";
                stageInfoText.fontSize = 36;
            }
            else if (stageNumber <= 25)
                stageInfoText.text += "DUNGEON";

            stageInfoText.text += " - ";

            if (stageNumber % 5 == 1)
                stageInfoText.text += "1";
            else if (stageNumber % 5 == 2)
                stageInfoText.text += "2";
            else if (stageNumber % 5 == 3)
                stageInfoText.text += "3";
            else if (stageNumber % 5 == 4)
                stageInfoText.text += "4";
            else if (stageNumber % 5 == 0)
                stageInfoText.text += "5";
        }

        int starCount = JsonSave.Instance.gameData.StageGetValueSave(GameData.StageValueType.STAR, stageNumber);
        int stampCount = JsonSave.Instance.gameData.StageGetValueSave(GameData.StageValueType.STAMP, stageNumber, true);
        int percent = 17 * (starCount + stampCount);
        if (percent >= 100)
            percent = 100;
        stageInfoPercentText.text = string.Format("{0}%", percent);
        stageInfo.GetComponent<RectTransform>().DOLocalMoveY(181, 1f);
    }

    public void StageInfoClose()
    {
        AllAudioManager.Instance.uiClick.Play();
        stageInfo.SetActive(false);
        currentShowStage = 0;
    }
    public Sprite GetStarSprites(bool isEmpty)
    {
        if (isEmpty)
                return starSprites[0];
        else
                return starSprites[1];
    }

    public Sprite GetStampSprites(bool isEmpty, int spriteNumber)
    {
        if (isEmpty)
            return EmptyStamps[spriteNumber];
        else
            return Stamps[spriteNumber];
    }

    public Sprite GetStageButtonSprites(int spriteNumber)
    {
        return stageButton[spriteNumber];
    }

    private void Update()
    {
        if (scroll.GetComponent<RectTransform>().anchoredPosition.y >= 40)
            scrollShowArrows[1].SetActive(true);
        else
            scrollShowArrows[1].SetActive(false);

        if (scroll.GetComponent<RectTransform>().anchoredPosition.y <= 260)
            scrollShowArrows[0].SetActive(true);
        else
            scrollShowArrows[0].SetActive(false);
    }
    private void SetOnStageBlur(){
        StageBlur.gameObject.SetActive(true);
        Debug.Log("스테이지 블러 활성화");
    }
    private void SetDownStageBlur(){
        StageBlur.gameObject.SetActive(false);
        Debug.Log("스테이지 블러 비활성화");
    }
}
