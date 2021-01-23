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

    // 함수용 변수

    [SerializeField]
    private float cameraMoveSpeed=1f;
    [SerializeField]
    private float cameraMoveDistance = 800f;

    private void Awake()
    {
        JsonSave.Instance.LoadGameData();
        Screen.SetResolution(800, 480, true);
        Time.timeScale = 1;
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
            AudioManager.Instance.BGM_Labyrinth.volume = 1;
            AudioManager.Instance.BGM_Labyrinth.Play();
        }
        else
            AudioManager.Instance.BGM_Title.Play();
    }

    private void Update()
    {
        if (Input.anyKeyDown && scoreManager.isTitleBegin) 
        {
            scoreManager.isTitleBegin = false;
            Invoke("TitleToLabyrinth", titleClickWaitTime);
        }

        if (AudioManager.Instance.BGM_Title.volume == 0)
            AudioManager.Instance.BGM_Title.Stop();
    }

    private void TitleToLabyrinth()
    {
        
        startPanel.GetComponent<RectTransform>().DOLocalMoveY(1000, 2);
        backgroundCamera.transform.DOMoveY(-33.9f, 4.5f);
        TitleObjects.transform.DOLocalMoveY(30f, 2);
        TitleObjects.transform.DOScale(new Vector3(0.3f, 0.3f, 0.3f), 1);
        AudioManager.Instance.BGM_Title.DOFade(0, 2);
        AudioManager.Instance.BGM_Labyrinth.Play();
        AudioManager.Instance.BGM_Labyrinth.DOFade(1, 6);
        mainCamera.transform.DOMoveY(-cameraMoveDistance, cameraMoveSpeed).SetEase(Ease.InOutCubic);
    }
}
