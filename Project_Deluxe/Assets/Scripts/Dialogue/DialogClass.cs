using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[System.Serializable]
public class DialogClass 
{
    // Start is called before the first frame update
    public string name;
    public List<string> sentences;
    public Text noticeTuto;
    public Image background;
    public Button yesButton;
    public Button noButton;

    // ���� ��Ƽ�� �ڽ� ���� 
    public Image noticeBox;
    public Button nextXbutton;
    public Button prevXbutton;
    public Button prevBtn;
    public Button nextBtn;
}
