using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoticeBox : MonoBehaviour
{
    public DialogClass diaClass;
    public Image[] noticeImages;
    public int i = 0;

    public void BtnTrigger()
    {
        if (i == 6)
        {
            SetNextButtonX();
            return;
        }
        ClearPrevBtn();
        noticeImages[i + 1].gameObject.SetActive(true);
            noticeImages[i].gameObject.SetActive(false);
            i += 1;
        Debug.Log(i);
         
    }
    public void PrevBtnTrigger()
    {
        if(i != 0)
        {
            ClearNextBtn();
            noticeImages[i - 1].gameObject.SetActive(true);
            noticeImages[i].gameObject.SetActive(false);
            i -= 1;
            Debug.Log(i);
        }else
        {
            SetPrevButtonX();
            return;
        }
       
    }
    public void nextXBtnTrigger()
    {
        for(int i = 0; i<7; i++)
        {
            noticeImages[i].gameObject.SetActive(false);
        }
        gameObject.SetActive(false);
        diaClass.prevBtn.gameObject.SetActive(false);
        diaClass.noticeBox.gameObject.SetActive(false);
        diaClass.nextBtn.gameObject.SetActive(false);
        diaClass.nextXbutton.gameObject.SetActive(false);
        Time.timeScale = 1f;
        return;
    }
    public void SetNextButtonX()
    {
        diaClass.nextXbutton.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }
    public void SetPrevButtonX()
    {
        diaClass.prevXbutton.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }
    public void ClearPrevBtn()
    {
        diaClass.prevXbutton.gameObject.SetActive(false);
        diaClass.prevBtn.gameObject.SetActive(true);

    }
    public void ClearNextBtn()
    {
        diaClass.nextXbutton.gameObject.SetActive(false);
        diaClass.nextBtn.gameObject.SetActive(true);

    }
}

