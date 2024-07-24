using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class ClickOnObject : MonoBehaviour
{

    [SerializeField] private Camera insideCamera;

    [SerializeField] private GameObject informationPanel;
    [SerializeField] private GameObject informationTitle;
    [SerializeField] private GameObject informationDescription;
    [SerializeField] private GameObject informationBanner;
    [SerializeField] private GameObject informationImage;
    [SerializeField] private GameObject informationScroll;

    [SerializeField] private GameObject goBackButton;
    [SerializeField] private GameObject helpButton;


    [SerializeField] private List<GameObject> buildingCubes;


    Ray ray;
    RaycastHit hit;

    private bool clickedOnObject = false;

    private Vector3 originalPosition;
    private Quaternion originalRotation;

    public InformationObject building;


    private HelperManager helperManager;


    void Start()
    {
        // Store the original position and rotation of the camera
        originalPosition = insideCamera.transform.position;
        originalRotation = insideCamera.transform.rotation;

        informationImage.GetComponentInChildren<Image>().sprite = building.icon;

        for(int i = 0; i < buildingCubes.Count; i++)
        {
            buildingCubes[i].GetComponentInChildren<MeshRenderer>().material = building.typeOfBuilding;
        }
        
        helperManager = GameObject.Find("HelperManager").GetComponent<HelperManager>();

    }

    void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return; // If the pointer is over a UI element, do nothing
        }

        ray = insideCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            if (Input.GetMouseButtonDown(0) && !clickedOnObject)
            {
                if (hit.collider.gameObject == null)
                {
                    insideCamera.transform.position = originalPosition;
                    insideCamera.transform.rotation = originalRotation;
                }
                else if (hit.collider.transform.parent.gameObject.name == building.name)
                {
                    if (helperManager.isHelperTalking())
                    {
                        helperManager.HideWhenObjClicked();
                    }

                        ShowInformationOfObject();
                }
            }
        }

        if(!informationPanel.activeSelf)
        {
            clickedOnObject = false;
        }
    }

    /*
    void LookAtObject(GameObject target)
    {
        insideCamera.transform.LookAt(target.transform);
        ShowInformationOfObject();
    }*/

    void ShowInformationOfObject()
    {
        goBackButton.SetActive(false);
        helpButton.SetActive(false);
        clickedOnObject = true;
        informationPanel.gameObject.SetActive(true);
        informationScroll.gameObject.SetActive(true);

        informationBanner.GetComponentInChildren<Image>().sprite = building.banner;
        informationTitle.GetComponentInChildren<TextMeshProUGUI>().text = building.title;
        informationDescription.GetComponentInChildren<TextMeshProUGUI>().text = building.description;

    }

    /*void ShowInformationObjectAdmin()
    {
        goBackButton.SetActive(false);
        helpButton.SetActive(false);
        clickedOnObject = true;

        adminPanel.SetActive(true);

        adminBanner.GetComponentInChildren<Image>().sprite = building.banner;
        adminIcon.GetComponentInChildren<Image>().sprite = building.icon;
        adminTitle.GetComponentInChildren<TMP_InputField>().text = building.title;
        adminDescription.GetComponentInChildren<TMP_InputField>().text = building.description;

        adminManager.building = getBuilding();
    }*/

    public InformationObject getBuilding() { return building; }
}
