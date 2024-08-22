using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private GameObject outsideCameraGameObject;
    [SerializeField] private GameObject insideCameraGameObject;
    [SerializeField] private GameObject insideCamera2GameObject;
    [SerializeField] private GameObject insideCamera3GameObject;


    private Vector3 insideCameraInactivePosition = new Vector3(50, 10, -2.14f);
    private Vector3 insideCamera3InactivePosition = new Vector3(7030, 10, 272f);

    private GameObject currentLocation;


    public GameObject getOutsideCameraGO () { return outsideCameraGameObject; }
    public GameObject getInsideCameraGO () { return insideCameraGameObject; }
    public GameObject getInside2CameraGO () { return insideCamera2GameObject; }

    public GameObject getInside3CameraGO () { return insideCamera3GameObject; }


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

    public bool isInside3Active()
    {
        return insideCamera3GameObject.activeSelf;
    }


    public void SetOutsideCameraMain()
    {
        if(isInsideActive())
        {
            insideCameraGameObject.SetActive(false);
            insideCameraGameObject.tag = "Untagged";
            SetInsideCameraToInactivePosition();
        } else if (isInside2Active()) {
            insideCamera2GameObject.SetActive(false);
            insideCamera2GameObject.tag = "Untagged";
        }
        else if (isInside3Active())
        {
            insideCamera3GameObject.SetActive(false);
            insideCamera3GameObject.tag = "Untagged";
            SetInsideCamera3ToInactivePosition();
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
        else if (isInside3Active())
        {
            insideCamera3GameObject.SetActive(false);
            insideCamera3GameObject.tag = "Untagged";
            SetInsideCamera3ToInactivePosition();
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
            SetInsideCameraToInactivePosition();
        }
        else if (isInside3Active())
        {
            insideCamera3GameObject.SetActive(false);
            insideCamera3GameObject.tag = "Untagged";
            SetInsideCamera3ToInactivePosition();
        }
        else if (isOutsideActive())
        {
            outsideCameraGameObject.SetActive(false);
            outsideCameraGameObject.tag = "Untagged";
        }

        insideCamera2GameObject.SetActive(true);
        insideCamera2GameObject.tag = "MainCamera";
    }

    public void SetInside3CameraMain()
    {
        if (isInsideActive())
        {
            insideCameraGameObject.SetActive(false);
            insideCameraGameObject.tag = "Untagged";
            SetInsideCameraToInactivePosition();
        }
        else if (isOutsideActive())
        {
            outsideCameraGameObject.SetActive(false);
            outsideCameraGameObject.tag = "Untagged";
        }
        else if (isInside2Active())
        {
            insideCamera2GameObject.SetActive(false);
            insideCamera2GameObject.tag = "Untagged";
        }

        insideCamera3GameObject.SetActive(true);
        insideCamera3GameObject.tag = "MainCamera";
    }


    public void SetInsideCameraPosition(Vector3 newPosition)
    {
        insideCameraGameObject.transform.position = newPosition;
    }

    public void SetInsideCamera3Position(Vector3 newPosition)
    {
        insideCamera3GameObject.transform.position = newPosition;
    }

    public void SetInsideCameraToInactivePosition()
    {
        insideCameraGameObject.transform.position = insideCameraInactivePosition;
    }

    public void SetInsideCamera3ToInactivePosition()
    {
        insideCamera3GameObject.transform.position = insideCamera3InactivePosition;
    }


    public void SetCurrentLocation(GameObject newLocation)
    {
        currentLocation = newLocation;
    }

    public GameObject GetCurrentLocation()
    {
        return currentLocation;
    }

}
