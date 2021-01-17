using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    private float spawnX = 0f;
    [SerializeField]
    private GameObject player = null;

    private void Awake()
    {
        GameObject startPoint = GameObject.FindGameObjectWithTag("StartPoint");
        spawnX = startPoint.transform.localPosition.x;
    }

    private void Update()
    {
        float playerDistance = player.transform.localPosition.x - spawnX;
        float followRange = 0 - spawnX;

        if (playerDistance >= followRange)
            transform.localPosition = new Vector3(player.transform.localPosition.x,transform.localPosition.y, transform.localPosition.z);
        else
            transform.localPosition = new Vector3(0, transform.localPosition.y, transform.localPosition.z);
    }
}
