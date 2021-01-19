using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject menu;
    [SerializeField]
    private GameObject clear;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !menu.activeSelf)
        {
            menu.SetActive(true);
            Time.timeScale = 0;
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && menu.activeSelf) 
        {
            menu.SetActive(false);
            Time.timeScale = 1;
        }
    }
    private void OnClickContinueBtn() 
    {
        menu.SetActive(false);
        Time.timeScale = 1;
    }
    private void OnClickHomeBtn() 
    {
        menu.SetActive(false);
    }

    private void StageClear() 
    {
        clear.SetActive(true);
    }

    private void OnClickNextBtn() 
    {
        clear.SetActive(false);
    }
}
