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
        float followRangeY = 1.1f - spawnY;
        float cameraToPlayerDistanceX = Mathf.Abs(transform.localPosition.x - player.transform.localPosition.x);
        float cameraToPlayerDistanceY = Mathf.Abs(transform.localPosition.y - player.transform.localPosition.y);

        //Debug.Log("playerDistance X :" + playerDistanceX + "\nplayerDistance Y :" + playerDistanceY + "\nfollowRange X :" + followRangeX + "\nfollowRange Y :" + followRangeY + "\ncameraToPlayerDistance X :" + cameraToPlayerDistanceX + "\ncameraToPlayerDistance Y :" + cameraToPlayerDistanceY);


        if (cameraToPlayerDistanceX > 1)
            cameraSoftMove = true;
        else if (cameraToPlayerDistanceX <= 0.1f)
            cameraSoftMove = false;

        if (PlayerController.Instance.state == PlayerController.PlayerState.Dead)
            return;

        if (playerDistanceX >= followRangeX)
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

        if (playerDistanceY >= followRangeY)
        {
            Vector3 playerY = new Vector3(transform.localPosition.x, player.transform.localPosition.y, transform.localPosition.z);

            if (!cameraSoftMove)
                transform.localPosition = playerY;
            else
                transform.localPosition = Vector3.Lerp(transform.localPosition, playerY, 0.01f);
        }
        else
        {
            Vector3 ZeroY = new Vector3(transform.localPosition.x, 1.1f, transform.localPosition.z);

            if (!cameraSoftMove)
                transform.localPosition = ZeroY;
            else
                transform.localPosition = Vector3.Lerp(transform.localPosition, ZeroY, 0.01f);
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
