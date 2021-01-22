using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;


public class UI : MonoBehaviour
{
    [SerializeField]
    private Text startTxt = null;

    private void Awake()
    {
        JsonSave.Instance.LoadGameData();
        Time.timeScale = 1;
    }

    void Start()
    {
        startTxt.DOColor(new Color(1f, 1f, 1f, 10f), 0.8f).SetLoops(-1, LoopType.Yoyo);

    }
    private void Update()
    {
        if (Input.anyKeyDown) 
        {
            SceneManager.LoadScene(1);
        }
    }
}
