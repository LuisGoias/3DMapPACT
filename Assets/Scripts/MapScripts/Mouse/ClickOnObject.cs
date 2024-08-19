using System.Collections;
using System.Collections.Generic;
using System.IO;
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
    [SerializeField] private GameObject searchButton;
    [SerializeField] private GameObject currentLocationTMP;

    [SerializeField] private List<GameObject> buildingCubes;


    public InformationSerialize building;


    Ray ray;
    RaycastHit hit;

    private bool clickedOnObject = false;

    private Vector3 originalPosition;
    private Quaternion originalRotation;



    private HelperManager helperManager;


    void Start()
    {
        // Store the original position and rotation of the camera
        originalPosition = insideCamera.transform.position;
        originalRotation = insideCamera.transform.rotation;

        informationImage.GetComponentInChildren<Image>().sprite = GetOfficeImageFromPath(building.iconPath);

        for (int i = 0; i < buildingCubes.Count; i++)
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
                else if (hit.collider.transform.parent.gameObject.name == building.gameObjectName)
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
        searchButton.SetActive(false);
        currentLocationTMP.SetActive(false);


        clickedOnObject = true;
        informationPanel.gameObject.SetActive(true);
        informationScroll.gameObject.SetActive(true);

        informationBanner.GetComponentInChildren<Image>().sprite = 
            GetOfficeImageFromPath(building.bannerPath);
        informationTitle.GetComponentInChildren<TextMeshProUGUI>().text = building.title;
        informationDescription.GetComponentInChildren<TextMeshProUGUI>().text = building.description;

    }
    private Sprite GetOfficeImageFromPath(string currentOfficePath)
    {
        if (string.IsNullOrEmpty(currentOfficePath)) return null;

        string temporaryPath = currentOfficePath.Trim();

        var fileData = File.ReadAllBytes(temporaryPath);


        var textureImage = new Texture2D(2, 2, TextureFormat.RGBA32, false);
        textureImage.LoadImage(fileData);



        Sprite spriteTranslated = Sprite.Create(textureImage,
            new Rect(0.0f, 0.0f, textureImage.width, textureImage.height),
            new Vector2(0.5f, 0.5f), 100.0f);

        return spriteTranslated;
    }

    public InformationSerialize getBuilding() { return building; }

    public void setBuilding(InformationSerialize newBuilding) { building = newBuilding; }
}
