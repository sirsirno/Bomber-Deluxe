using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UI : MonoBehaviour
{
    [SerializeField]
    private Text startTxt = null;
    [SerializeField]
    private GameObject startPanel = null;
    [SerializeField]
    private GameObject mainCamera = null;
    [SerializeField]
    private GameObject backgroundCamera = null;
    [SerializeField]
    private GameObject TitleObjects = null;

    private ScoreManager scoreManager = null;
    [SerializeField]
    private float titleClickWaitTime = 1f;
    [SerializeField]
    private GameObject toWorldSelectBtn = null;
    [SerializeField]
    private GameObject creditBtn = null;
    [SerializeField]
    private GameObject creditContent = null;
    [SerializeField]
    private GameObject creditScroll = null;
    // 함수용 변수

    [SerializeField]
    private float cameraMoveSpeed=1f;
    [SerializeField]
    private float cameraMoveDistance = 800f;

    [SerializeField]
    private GameObject soundBtn = null;
    private bool isMute = false;

    private void Awake()
    {
        Time.timeScale = 1;

        if (PlayerPrefs.GetInt("Mute") == 0)
        {
            isMute = false;
            soundBtn.GetComponent<Image>().color = new Color(1, 1, 1, 1);
            AudioListener.volume = 1;
        }
        else if(PlayerPrefs.GetInt("Mute") == 1)
        {
            isMute = true;
            soundBtn.GetComponent<Image>().color = new Color(0.7843137f , 0.7843137f , 0.7843137f, 0.5019608f);
            AudioListener.volume = 0;
        }
        else
        {
            isMute = false;
            PlayerPrefs.SetInt("Mute", 0);
            soundBtn.GetComponent<Image>().color = new Color(1, 1, 1, 1);
            AudioListener.volume = 1;
        }
    }

    void Start()
    {
        scoreManager = FindObjectOfType<ScoreManager>();
        scoreManager.ScoreValueSet(ScoreManager.ScoreType.LIFE, ScoreManager.SetType.SET, 5);
        startTxt.DOColor(new Color(1f, 1f, 1f, 10f), 0.8f).SetLoops(-1, LoopType.Yoyo);

        if (!scoreManager.isTitleBegin)
        {
            TitleObjects.transform.DOLocalMoveY(30f, 0);
            TitleObjects.transform.DOScale(new Vector3(0.3f, 0.3f, 0.3f), 0);
            SoundBtnOn();
            soundBtn.GetComponent<Image>().color = new Color(1, 1, 1, 1);
            AudioManager.Instance.BGM_Labyrinth.volume = 1;
            AudioManager.Instance.BGM_Labyrinth.Play();
            JsonSave.Instance.SaveGameData();
        }
        else
            AudioManager.Instance.BGM_Title.Play();
    }

    private void Update()
    {
        if (Input.anyKeyDown && scoreManager.isTitleBegin)
        {
            scoreManager.isTitleBegin = false;
            startTxt.gameObject.SetActive(false);
            SoundBtnOn();
            soundBtn.GetComponent<Image>().color = new Color(1, 1, 1, 0);
            if (isMute)
                soundBtn.GetComponent<Image>().DOColor(new Color(0.7843137f, 0.7843137f, 0.7843137f, 0.5019608f), 0.8f);
            else
                soundBtn.GetComponent<Image>().DOColor(new Color(1, 1, 1, 1), 0.8f);
            toWorldSelectBtn.SetActive(true);
            toWorldSelectBtn.GetComponent<RectTransform>().anchoredPosition = new Vector2(100, 70);
            toWorldSelectBtn.GetComponent<RectTransform>().DOAnchorPos(new Vector2(-70, 70), 1);
            creditBtn.SetActive(true);
            creditBtn.GetComponent<RectTransform>().anchoredPosition = new Vector2(-100, 70);
            creditBtn.GetComponent<RectTransform>().DOAnchorPos(new Vector2(70, 70), 1);
        }

        if (AudioManager.Instance.BGM_Title.volume == 0)
            AudioManager.Instance.BGM_Title.Stop();
    }

    public void TitleToLabyrinthBtnClick()
    {
        AllAudioManager.Instance.uiClick.Play();
        toWorldSelectBtn.GetComponent<RectTransform>().DOAnchorPos(new Vector2(100, 70), 1);
        toWorldSelectBtn.GetComponent<Button>().interactable = false;
        creditBtn.GetComponent<RectTransform>().DOAnchorPos(new Vector2(-100, 70), 1);
        creditBtn.GetComponent<Button>().interactable = false;
        soundBtn.GetComponent<Image>().DOColor(new Color(1, 1, 1, 0), 0.8f);
        Invoke("TitleToLabyrinth", titleClickWaitTime);
    }

    private void TitleToLabyrinth()
    {
        soundBtn.SetActive(false);
        startPanel.GetComponent<RectTransform>().DOLocalMoveY(1000, 2);
        backgroundCamera.transform.DOMoveY(-33.9f, 4.5f);
        TitleObjects.transform.DOLocalMoveY(30f, 2);
        TitleObjects.transform.DOScale(new Vector3(0.3f, 0.3f, 0.3f), 1);
        AudioManager.Instance.BGM_Title.DOFade(0, 2);
        AudioManager.Instance.SFX_Fall.Play();
        AudioManager.Instance.SFX_Fall.DOFade(0.8f, 2f).SetLoops(2, LoopType.Yoyo);
        AudioManager.Instance.BGM_Labyrinth.Play();
        AudioManager.Instance.BGM_Labyrinth.DOFade(1, 3).SetDelay(2);
        mainCamera.transform.DOMoveY(-cameraMoveDistance, cameraMoveSpeed).SetEase(Ease.InOutCubic).OnComplete(SoundBtnOn);
    }

    private void SoundBtnOn()
    {
        soundBtn.SetActive(true);
        if (isMute)
        {
            soundBtn.GetComponent<Image>().color = new Color(0.7843137f, 0.7843137f, 0.7843137f, 0.5019608f);
        }
        else
        {
            soundBtn.GetComponent<Image>().color = new Color(1, 1, 1, 1);
        }
    }


    public void SoundOnOffBtn()
    {
        if(isMute)
        {
            isMute = false;
            PlayerPrefs.SetInt("Mute", 0);
            soundBtn.GetComponent<Image>().color = new Color(1, 1, 1, 1);
            AudioListener.volume = 1;
            AllAudioManager.Instance.uiClick.Play();
        }
        else
        {
            isMute = true;
            PlayerPrefs.SetInt("Mute", 1);
            soundBtn.GetComponent<Image>().color = new Color(0.7843137f, 0.7843137f, 0.7843137f, 0.5019608f);
            AudioListener.volume = 0;
        }
    }

    public void CreditClick()
    {
        AllAudioManager.Instance.uiClick.Play();
        creditContent.SetActive(true);
    }

    public void CreditExit()
    {
        AllAudioManager.Instance.uiClick.Play();
        creditContent.SetActive(false);
        creditScroll.GetComponent<RectTransform>().anchoredPosition = new Vector2(-147.26f, 110.5f);
    }
}
