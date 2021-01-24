using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllAudioManager : Singleton<AllAudioManager>
{
    private AllAudioManager[] allAudioManagers;

    public AudioSource uiClick = null;
    public AudioSource gameStart = null;

    private void Awake()
    {
        allAudioManagers = FindObjectsOfType<AllAudioManager>();

        if (allAudioManagers.Length >= 2)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }
}
