using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : Singleton<CameraMove>
{
    private float spawnX = 0f;
    [SerializeField]
    private GameObject player = null;
    private bool cameraSoftMove = false;

    private void Awake()
    {
        GameObject startPoint = GameObject.FindGameObjectWithTag("StartPoint");
        spawnX = startPoint.transform.localPosition.x;
    }
    private void Update()
    {
        float playerDistance = player.transform.localPosition.x - spawnX;
        float followRange = 0 - spawnX;
        float cameraToPlayerDistance = Mathf.Abs(transform.localPosition.x - player.transform.localPosition.x);

        if (cameraToPlayerDistance > 1)
            cameraSoftMove = true;
        else if(cameraToPlayerDistance <= 0.1f)
            cameraSoftMove = false;

            if (PlayerController.Instance.state == PlayerController.PlayerState.Dead)
            return;

        if (playerDistance >= followRange)
        {
            Vector3 playerX = new Vector3(player.transform.localPosition.x, transform.localPosition.y, transform.localPosition.z);

            if (!cameraSoftMove)
                transform.localPosition = playerX;
            else
                transform.localPosition = Vector3.Lerp(transform.localPosition, playerX, 0.01f);
        }
        else
        {
            Vector3 ZeroX = new Vector3(0, transform.localPosition.y, transform.localPosition.z);

            if (!cameraSoftMove)
                transform.localPosition = ZeroX;
            else
                transform.localPosition = Vector3.Lerp(transform.localPosition, ZeroX, 0.01f);
        }

        if(player.GetComponent<PlayerController>().sleeping != false)
        {
            GetComponent<Camera>().cullingMask = 55;
        }
        else
        {
            GetComponent<Camera>().cullingMask = 63;
        }
    }
}
