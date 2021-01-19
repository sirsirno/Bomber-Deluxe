using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : Singleton<CameraMove>
{
    private float spawnX = 0f;
    private float spawnY = 0f;
    [SerializeField]
    private GameObject player = null;
    private bool cameraSoftMove = false;

    private void Awake()
    {
        GameObject startPoint = GameObject.FindGameObjectWithTag("StartPoint");
        spawnX = startPoint.transform.localPosition.x;
        spawnY = startPoint.transform.localPosition.y;
    }
    private void Update()
    {
        float playerDistanceX = player.transform.localPosition.x - spawnX;
        float playerDistanceY = player.transform.localPosition.y - spawnY;

        float followRangeX = 0 - spawnX;
        float followRangeY = 0 - spawnY;
        float cameraToPlayerDistanceX = Mathf.Abs(transform.localPosition.x - player.transform.localPosition.x);
        float cameraToPlayerDistanceY = Mathf.Abs(transform.localPosition.y - player.transform.localPosition.y);

        Debug.Log("playerDistance X :" + playerDistanceX + "\nplayerDistance Y :" + playerDistanceY + "\nfollowRange X :" + followRangeX + "\nfollowRange Y :" + followRangeY + "\ncameraToPlayerDistance X :" + cameraToPlayerDistanceX + "\ncameraToPlayerDistance Y :" + cameraToPlayerDistanceY);


        if (cameraToPlayerDistanceX > 1)
            cameraSoftMove = true;
        else if (cameraToPlayerDistanceX <= 0.1f)
            cameraSoftMove = false;

        if (PlayerController.Instance.state == PlayerController.PlayerState.Dead)
            return;

        if (playerDistanceX >= followRangeX || playerDistanceY >= followRangeY)
        {
            Vector3 playerXY = new Vector3(player.transform.localPosition.x, player.transform.localPosition.y, transform.localPosition.z);

            if (!cameraSoftMove)
                transform.localPosition = playerXY;
            else
                transform.localPosition = Vector3.Lerp(transform.localPosition, playerXY, 0.01f);
        }
        else
        {
            Vector3 ZeroXY = new Vector3(0, 0, transform.localPosition.z);

            if (!cameraSoftMove)
                transform.localPosition = ZeroXY;
            else
                transform.localPosition = Vector3.Lerp(transform.localPosition, ZeroXY, 0.01f);
        }

        if (player.GetComponent<PlayerController>().sleeping != false)
        {
            GetComponent<Camera>().cullingMask = 55;
        }
        else
        {
            GetComponent<Camera>().cullingMask = 63;
        }
    }
}
