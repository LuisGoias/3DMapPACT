using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MouseManager : MonoBehaviour
{
    public bool isDragging;
    public bool isClicked;
    private float clickDuration = 0f;
    private float clickThreshold = 0.2f;
    private bool isMouseHeld = false;

    [SerializeField] private Slider slider;
    [SerializeField] private ScrollRect scrollView; // Reference to the ScrollRect component of the ScrollView

    private void Start()
    {
        slider.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
        scrollView.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
    }

    private void Update()
    {
        // Check if the pointer is over any UI element
        if (IsPointerOverUIElement())
        {
            isDragging = false;
            return; // Exit the update to ensure other logic is not executed
        }

        if (Input.GetMouseButtonDown(0))
        {
            isMouseHeld = true;
            clickDuration = 0f; // Reset click duration when the button is first pressed
        }

        if (isMouseHeld)
        {
            clickDuration += Time.deltaTime;

            if (clickDuration >= clickThreshold)
            {
                isDragging = true;
                isClicked = false;
            }
            else
            {
                isDragging = false;
                isClicked = true;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (isMouseHeld && clickDuration < clickThreshold)
            {
                isClicked = true;
                isDragging = false;
            }
            else
            {
                isClicked = false;
            }

            isMouseHeld = false;
            clickDuration = 0f; // Reset click duration after processing
        }
    }

    private void ValueChangeCheck()
    {
        // This method is called when the slider or scrollview value is changed
        if (isDragging)
        {
            isDragging = false;
        }
    }

    // Method to check if the pointer is over any UI element
    private bool IsPointerOverUIElement()
    {
        // Check if the pointer is over any UI element
        return EventSystem.current.IsPointerOverGameObject();
    }

    public bool getDragging()
    {
        return isDragging;
    }

    public void setDragging(bool value)
    {
        isDragging = value;
    }

    public bool getClicked()
    {
        return isClicked;
    }

    public void setClicked(bool value)
    {
        isClicked = value;
    }
}
