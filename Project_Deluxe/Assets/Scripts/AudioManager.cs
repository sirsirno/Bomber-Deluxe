using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    [Header("BGM")]
    public AudioSource BGM_Prototype = null;
    public AudioSource BGM_Future = null;
    public AudioSource BGM_Future2 = null;
    [Header("SFX")]
    public AudioSource SFX_ClockTic = null;
    public AudioSource SFX_ClockTok = null;
    public AudioSource SFX_FutureEnter = null;
    public void BGM_FutureRandomPlay()
    {
        int randomRange = Random.Range(0, 4);

        if (randomRange == 0)
        {
            BGM_Future.Play();
            BGM_Future.time = 0;
        }
        else if (randomRange == 1)
        {
            BGM_Future.Play();
            BGM_Future.time = 22;
        }
        else if (randomRange == 2)
        {
            BGM_Future2.Play();
            BGM_Future2.time = 0;
        }
        else
        {
            BGM_Future2.Play();
            BGM_Future2.time = 22;
        }
    }

    public void BGM_FutureBGMStop()
    {
        BGM_Future.Stop();
        BGM_Future2.Stop();
    }

    public void SFX_ClockTicPlay()
    {
        if (!SFX_ClockTic.isPlaying)
            SFX_ClockTic.Play();
    }

    public void SFX_ClockTocPlay()
    {
        if (!SFX_ClockTok.isPlaying)
            SFX_ClockTok.Play();
    }
}
