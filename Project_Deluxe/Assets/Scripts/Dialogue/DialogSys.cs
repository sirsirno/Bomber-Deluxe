using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*enum Expression {
    Normal = 0,
    Smile,
    OpenMouse,
    SharkTeeth,
    LiddedEyeNormal,
    LiddedEyeSmile,
    LiddedEyeOpenMouse,
    LiddedEyeSharkTeeth
};*/
public class DialogSys : MonoBehaviour
{
    [SerializeField]
    private Text textCharName;
    [SerializeField]
    private Text textSentence;
    [SerializeField]
    private Image[] charImage;
    [SerializeField]
    private Animator anim;
    [SerializeField]
    private Animator charAnim;
    [SerializeField]
    private float iETextSpeed = 0.05f;
    [SerializeField]
    DialogClass infor;

    Queue<string> sentences = new Queue<string>();

    private void Awake()
    {
        string bString = PlayerPrefs.GetString("TutoOnes", "true");
        infor.isTutoOnes = System.Convert.ToBoolean(bString);
        
        Time.timeScale = 1f;
        if (infor.isTutoOnes == true)
        {
            Time.timeScale = 0f;
            infor.dialogue.gameObject.SetActive(true);
        }

    }

    public void Begin(DialogClass info)
    {
        anim.SetBool("isOpen", true);
        
        sentences.Clear(); // 큐 초기화

        textCharName.text = info.name;

        foreach (var sentence in info.sentences)
        {
            sentences.Enqueue(sentence);
        }

        Next();
    }

    public void Next()
    {
        for(int i =0; i <8; i++)
        {
            charImage[i].gameObject.SetActive(false);
        }

        if (sentences.Count == 2 || sentences.Count == 4)
            LiddedEyeSharkTeeth();
        else if (sentences.Count == 1)
            Smile();
        else if (sentences.Count == 6)
            ;
        else if(sentences.Count == 5)
        {
            charAnim.SetBool("isOpen_Char", true);
            EyeNormal();
        }
        else EyeNormal();

        if (sentences.Count == 0)
        {
            EndQueue();
            return;
        }
        textSentence.text = string.Empty;
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentences.Dequeue()));
        //textSentence.text = sentences.Dequeue();
    }
    IEnumerator TypeSentence(string sentence)
    {
        foreach (var letter in sentence)
        {
            textSentence.text += letter;
            yield return new WaitForSecondsRealtime(iETextSpeed);
            
        }
       
    }
    public void EndQueue()
    {
        anim.SetBool("isOpen", false);
        charAnim.SetBool("isOpen_Char", false);
        textSentence.text = string.Empty;
        infor.background.gameObject.SetActive(false);
        infor.noticeBox.gameObject.SetActive(true);
    }
    public void LiddedEyeNormal()
    {
        charImage[5].gameObject.SetActive(true);
        return;
    }
    public void LiddedEyeSmile()
    {
        charImage[4].gameObject.SetActive(true);
        return;
    }
    public void LiddedEyeSharkTeeth()
    {
        charImage[7].gameObject.SetActive(true);
        return;
    }
    public void LiddedEyeOpenMouse()
    {
        charImage[6].gameObject.SetActive(true);
        return;
    }
    public void Smile()
    {
        charImage[1].gameObject.SetActive(true);
        return;
    }
    public void SharkTeeth()
    {
        charImage[3].gameObject.SetActive(true);
        return;
    }
    public void EyeNormal()
    {
        charImage[0].gameObject.SetActive(true);
        return;
    }
    public void OpenMouse()
    {
        charImage[2].gameObject.SetActive(true);
        return;
    }

    [ContextMenu("테스트")]
    private void Test()
    {
        PlayerPrefs.SetString("TutoOnes", "true");
    }
}
