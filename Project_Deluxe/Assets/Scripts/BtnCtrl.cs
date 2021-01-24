using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BtnCtrl : MonoBehaviour
{
    [SerializeField]
    private GameObject mySpriteGameObject = null;

    [SerializeField]
    private Sprite[] mySprites = null;

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
    private bool isJumpEnter = false;
    private bool isEffectOn = false;

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
                PlayerController.Instance.isPressedController[4] = true;
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

        if (isJumpEnter && Input.GetMouseButton(0))
        {
            if (controllType == ControllType.JUMP)
            {
                PlayerController.Instance.isPressedController[3] = true;
                isJumpEnter = false;
            }
        }

        if(isEffectOn && Input.GetMouseButton(0))
        {
            mySpriteGameObject.GetComponent<Image>().sprite = mySprites[1];
        }
        else
        {
            mySpriteGameObject.GetComponent<Image>().sprite = mySprites[0];
        }
    }

    public void OnPointerEnter()
    {
        isEnter = true;
        isJumpEnter = true;
        isEffectOn = true;
    }

    public void OnPointerExit()
    {
        isEnter = false;
        isJumpEnter = false;
        isEffectOn = false;
    }
}