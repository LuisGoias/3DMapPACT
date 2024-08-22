using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ZoomLogic : MonoBehaviour
{
    [SerializeField] private GameObject outsideCameraGameObject;
    [SerializeField] private GameObject insideCameraGameObject;
    [SerializeField] private GameObject insideCamera2GameObject;
    [SerializeField] private GameObject insideCamera3GameObject;

    private Slider slider;
    private Camera outsideCamera;
    private Camera insideCamera;
    private Camera insideCamera2;
    private Camera insideCamera3;

    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponent<Slider>();
        outsideCamera = outsideCameraGameObject.GetComponent<Camera>();
        insideCamera = insideCameraGameObject.GetComponent<Camera>();
        insideCamera2 = insideCamera2GameObject.GetComponent<Camera>();
        insideCamera3 = insideCamera3GameObject.GetComponent<Camera>();

        slider.onValueChanged.AddListener(OnSliderValueChanged);

        // Initialize the slider values
        if (outsideCameraGameObject.activeSelf)
        {
            slider.minValue = 30;
            slider.maxValue = 84;
            slider.value = slider.maxValue;
        }
        else if (insideCameraGameObject.activeSelf)
        {
            slider.minValue = 50; 
            slider.maxValue = 150; 
            slider.value = insideCamera.orthographicSize;
        } else if (insideCamera2GameObject.activeSelf)
        {
            slider.minValue = 50;
            slider.maxValue = 150;
            slider.value = insideCamera2.orthographicSize;
        }
        else if (insideCamera3GameObject.activeSelf)
        {
            slider.minValue = 1500;
            slider.maxValue = 2000;
            slider.value = insideCamera3.orthographicSize;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (outsideCameraGameObject.activeSelf)
        {
            // Make sure the slider's range matches the outside camera settings
            slider.minValue = 30;
            slider.maxValue = 84;
        }
        else if (insideCameraGameObject.activeSelf)
        {
            // Make sure the slider's range matches the inside camera settings
            slider.minValue = 50;
            slider.maxValue = 150;
        }
        else if (insideCamera2GameObject.activeSelf)
        {
            // Make sure the slider's range matches the inside camera settings
            slider.minValue = 50;
            slider.maxValue = 150;
        }
        else if (insideCamera3GameObject.activeSelf)
        {
            // Make sure the slider's range matches the inside camera settings
            slider.minValue = 1500;
            slider.maxValue = 2000;
        }
    }

    private void OnSliderValueChanged(float value)
    {
        if (outsideCameraGameObject.activeSelf)
        {
            outsideCamera.fieldOfView = value;
        }
        else if (insideCameraGameObject.activeSelf)
        {
            insideCamera.orthographicSize = value;
        }
        else if (insideCamera2GameObject.activeSelf)
        {
            insideCamera2.orthographicSize = value;
        }
        else if (insideCamera3GameObject.activeSelf)
        {
            insideCamera3.orthographicSize = value;
        }
    }
}
