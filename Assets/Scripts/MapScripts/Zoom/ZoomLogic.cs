using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ZoomLogic : MonoBehaviour
{
    [SerializeField] private GameObject outsideCameraGameObject;
    [SerializeField] private GameObject insideCameraGameObject;

    private Slider slider;
    private Camera outsideCamera;
    private Camera insideCamera;

    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponent<Slider>();
        outsideCamera = outsideCameraGameObject.GetComponent<Camera>();
        insideCamera = insideCameraGameObject.GetComponent<Camera>();

        slider.onValueChanged.AddListener(OnSliderValueChanged);

        // Initialize the slider values
        if (outsideCameraGameObject.activeSelf)
        {
            slider.minValue = 3;
            slider.maxValue = 5;
            slider.value = outsideCamera.orthographicSize;
        }
        else if (insideCameraGameObject.activeSelf)
        {
            slider.minValue = 50; 
            slider.maxValue = 100; 
            slider.value = insideCamera.orthographicSize;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (outsideCameraGameObject.activeSelf)
        {
            // Make sure the slider's range matches the outside camera settings
            slider.minValue = 3;
            slider.maxValue = 5;
        }
        else if (insideCameraGameObject.activeSelf)
        {
            // Make sure the slider's range matches the inside camera settings
            slider.minValue = 50;
            slider.maxValue = 100;
        }
    }

    private void OnSliderValueChanged(float value)
    {
        if (outsideCameraGameObject.activeSelf)
        {
            outsideCamera.orthographicSize = value;
        }
        else if (insideCameraGameObject.activeSelf)
        {
            insideCamera.orthographicSize = value;
        }
    }
}
