using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SleepBubbleEvent : MonoBehaviour
{
    [SerializeField]
    private GameObject SleepingPlayer = null;

    private void Return()
    {
        transform.SetParent(SleepingPlayer.transform);
        transform.localPosition = new Vector2(1.51f, 1.19f);
    }
}
