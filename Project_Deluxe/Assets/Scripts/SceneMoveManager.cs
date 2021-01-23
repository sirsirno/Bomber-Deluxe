using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class SceneMoveManager : MonoBehaviour
{
    private ScoreManager scoreManager = null;

    [SerializeField]
    private GameObject startPanel = null;
    [SerializeField]
    private GameObject mainCamera = null;
    [SerializeField]
    private GameObject backgroundCamera = null;
    [SerializeField]
    private GameObject stageCanvas = null;
    [SerializeField]
    private GameObject[] worldStages = null;
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
    private Sprite[] EmptyStamps = null;
    [SerializeField]
    private Sprite[] Stamps = null;


    private int currentShowStage = 0;
    private void Start()
    {
        scoreManager = FindObjectOfType<ScoreManager>();
        TitleMove(scoreManager.WorldTypeGet);
    }

    public void TitleMove(int worldType)
    {
        if (scoreManager.WorldTypeGet == -1)
            return;

        scoreManager.WorldTypeSet(-1);
        startPanel.SetActive(false);
        mainCamera.transform.localPosition = new Vector3(-1.08f, -320, -37);
        backgroundCamera.transform.localPosition = new Vector3(30.5f, -33.9f, -37);
        stageCanvas.SetActive(true);

        worldStages[worldType].SetActive(true);
    }

    public void WorldButtonClick(int worldType)
    {
        stageCanvas.SetActive(true);
        worldStages[0].SetActive(false);
        worldStages[1].SetActive(false);
        worldStages[2].SetActive(false);
        worldStages[3].SetActive(false);
        worldStages[4].SetActive(false);

        worldStages[worldType].SetActive(true);
    }

    public void WorldExit()
    {
        worldStages[0].SetActive(false);
        worldStages[1].SetActive(false);
        worldStages[2].SetActive(false);
        worldStages[3].SetActive(false);
        worldStages[4].SetActive(false);
        stageCanvas.SetActive(false);
    }

    public void TitleReset()
    {
        scoreManager.isTitleBegin = true;
        SceneManager.LoadScene("Title");
    }

    public void StartStage()
    {
        SceneManager.LoadScene(currentShowStage + 1);
    }

    public void StageButtonClick(int stageNumber)
    {
        if (!stageInfo.activeSelf)
            stageInfo.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 517, 0);
        stageInfo.SetActive(true);
        currentShowStage = stageNumber;
        stageInfoStar.GetComponent<ShowStar>().StageNumberSet(stageNumber);
        stageInfoStar.GetComponent<ShowStar>().ShowStars();
        stageInfoStamp.GetComponent<ShowStamp>().StageNumberSet(stageNumber);
        stageInfoStamp.GetComponent<ShowStamp>().ShowStamps();

        // ∏  ≈ÿΩ∫∆Æ
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
        stageInfo.GetComponent<RectTransform>().DOLocalMoveY(181, 1f);
    }

    public void StageInfoClose()
    {
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
}
