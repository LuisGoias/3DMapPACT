using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ZoomButtonLogic : MonoBehaviour
{

    [SerializeField] private Slider slider;

    private CameraManager cameraManager;

    private void Start()
    {
        cameraManager = GameObject.Find("CameraManager").GetComponent<CameraManager>();
    }

    public void OnMinusClick()
    {
        if (cameraManager.isOutsideActive())
        {
            slider.value += 0.5f;
        } else if (cameraManager.isInsideActive()) 
        {
            slider.value += 5;
        }

    }

    public void OnPlusClick()
    {
        if (cameraManager.isOutsideActive())
        {
            slider.value -= 0.5f;
        }
        else if (cameraManager.isInsideActive())
        {
            slider.value -= 5;
        }
    }
}
