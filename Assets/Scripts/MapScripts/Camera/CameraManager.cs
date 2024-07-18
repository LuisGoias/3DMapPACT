using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private GameObject outsideCameraGameObject;
    [SerializeField] private GameObject insideCameraGameObject;
    [SerializeField] private GameObject insideCamera2GameObject;

    private List<GameObject> activeCameras;

    private Vector3 insideCameraInactivePosition = new Vector3(50, 10, -2.14f);


    public bool isOutsideActive()
    {
        return outsideCameraGameObject.activeSelf;
    }

    public bool isInsideActive()
    {
        return insideCameraGameObject.activeSelf;
    }

    public bool isInside2Active()
    {
        return insideCamera2GameObject.activeSelf;
    }


    public void SetOutsideCameraMain()
    {
        if(isInsideActive())
        {
            insideCameraGameObject.SetActive(false);
            insideCameraGameObject.tag = "Untagged";
            insideCameraGameObject.transform.position = insideCameraInactivePosition;
        } else if (isInside2Active()) {
            insideCamera2GameObject.SetActive(false);
            insideCamera2GameObject.tag = "Untagged";
        }

        outsideCameraGameObject.SetActive(true);
        outsideCameraGameObject.tag = "MainCamera";
    }

    public void SetInsideCameraMain()
    {
        if (isOutsideActive())
        {
            outsideCameraGameObject.SetActive(false);
            outsideCameraGameObject.tag = "Untagged";
        }
        else if (isInside2Active())
        {
            insideCamera2GameObject.SetActive(false);
            insideCamera2GameObject.tag = "Untagged";
        }

        insideCameraGameObject.SetActive(true);
        insideCameraGameObject.tag = "MainCamera";
    }

    public void SetInside2CameraMain()
    {
        if (isInsideActive())
        {
            insideCameraGameObject.SetActive(false);
            insideCameraGameObject.tag = "Untagged";
            insideCameraGameObject.transform.position = insideCameraInactivePosition;
        }
        else if (isOutsideActive())
        {
            outsideCameraGameObject.SetActive(false);
            outsideCameraGameObject.tag = "Untagged";
        }

        insideCamera2GameObject.SetActive(true);
        insideCamera2GameObject.tag = "MainCamera";
    }


}
