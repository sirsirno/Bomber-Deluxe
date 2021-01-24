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
       var system = FindObjectOfType<DialogSys>();
        system.Begin(info);
    }
}
