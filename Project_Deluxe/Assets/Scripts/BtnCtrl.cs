using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BtnCtrl : MonoBehaviour
{
    public enum ControllType
    {
        LEFT,
        RIGHT,
        JUMP,
        FUTURE
    }

    [SerializeField]
    private ControllType controllType = ControllType.LEFT;

    private bool isEnter = false;

    private void Update()
    {
        if (isEnter && Input.GetMouseButton(0))
        {
            if (controllType == ControllType.LEFT)
                PlayerController.Instance.isPressedController[0] = true;
            else if (controllType == ControllType.RIGHT)
                PlayerController.Instance.isPressedController[1] = true;
            else if (controllType == ControllType.JUMP)
                PlayerController.Instance.isPressedController[2] = true;
            else
            {
                PlayerController.Instance.isPressedController[3] = true;
                isEnter = false;
            }
        }
        else
        {
            if (controllType == ControllType.LEFT)
                PlayerController.Instance.isPressedController[0] = false;
            else if (controllType == ControllType.RIGHT)
                PlayerController.Instance.isPressedController[1] = false;
            else if (controllType == ControllType.JUMP)
                PlayerController.Instance.isPressedController[2] = false;
        }
    }

    public void OnPointerEnter()
    {
        isEnter = true;
    }

    public void OnPointerExit()
    {
        isEnter = false;
    }
}