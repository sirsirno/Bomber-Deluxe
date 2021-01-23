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

    // 함수용 변수

    [SerializeField]
    private float cameraMoveSpeed=2f;
    [SerializeField]
    private float invokeTime = 0.5f;
    public GameObject ppVolume;

    private void Awake()
    {
        JsonSave.Instance.LoadGameData();
        Time.timeScale = 1;
    }

    void Start()
    {
        scoreManager = FindObjectOfType<ScoreManager>();
        scoreManager.ScoreValueSet(ScoreManager.ScoreType.LIFE, ScoreManager.SetType.SET, 5);
        startTxt.DOColor(new Color(1f, 1f, 1f, 10f), 0.8f).SetLoops(-1, LoopType.Yoyo);

    }
    private void Update()
    {
        if (Input.anyKeyDown && scoreManager.isTitleBegin) 
        {
            startPanel.GetComponent<RectTransform>().DOLocalMoveY(1000, 2);
            scoreManager.isTitleBegin = false;
            mainCamera.transform.DOMoveY(-320, cameraMoveSpeed);
            Invoke("FastMoveY", invokeTime);
            backgroundCamera.transform.DOMoveY(-33.9f, 2f);
            TitleObjects.transform.DOLocalMoveY(30f, 2);
        }
    }
    private void FastMoveY(){
    mainCamera.transform.DOMoveY(-320, cameraMoveSpeed/2);
    }
}
