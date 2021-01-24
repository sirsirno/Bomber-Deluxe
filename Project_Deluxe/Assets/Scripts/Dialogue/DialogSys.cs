using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


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
    private float iETextSpeed = 0.05f;

    Queue<string> sentences = new Queue<string>();

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
        if(sentences.Count == 0)
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
            yield return new WaitForSeconds(iETextSpeed);
        }
    }
    public void EndQueue()
    {
        anim.SetBool("isOpen", false);
        textSentence.text = string.Empty;
    }
   
}
