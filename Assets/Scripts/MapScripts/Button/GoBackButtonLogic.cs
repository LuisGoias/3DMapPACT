using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoBackButtonLogic : MonoBehaviour
{


    private CameraManager cameraManager;


    private void Start()
    {
        cameraManager = GameObject.Find("CameraManager").GetComponent<CameraManager>();
    }


    public void onClickBack()
    {
        cameraManager.SetOutsideCameraMain();

        gameObject.SetActive(false);
    }
}
