using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private DialogClass info;

    

    public void Trigger()
    {
        if(gameObject.tag == "NoBtn")
        {
            info.noticeTuto.gameObject.SetActive(false);
            gameObject.SetActive(false);
            info.background.gameObject.SetActive(false);
            info.yesButton.gameObject.SetActive(false);
            Time.timeScale = 1f;
            return;
        }
       var system = FindObjectOfType<DialogSys>();
        system.Begin(info);
        
        info.noticeTuto.gameObject.SetActive(false);
        info.noButton.gameObject.SetActive(false);
        gameObject.SetActive(false);
        
    }
}
