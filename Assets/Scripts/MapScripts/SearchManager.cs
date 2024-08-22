using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SearchManager : MonoBehaviour
{

    [SerializeField] private GameObject pact1;
    [SerializeField] private GameObject searchOfficePanel;
    [SerializeField] private ScrollRect searchScrollView;
    [SerializeField] private GameObject officeSearchListPanelPrefab;

    [SerializeField] private GameObject searchBTN;
    [SerializeField] private GameObject helpBTN;
    [SerializeField] private GameObject goBackZoomBTN;
    [SerializeField] private GameObject currentLocationTMP;


    private RectTransform searchScrollContentRectTransform;


    private List<GameObject> searchList = new List<GameObject>();

    private CameraManager cameraManager;

    // Start is called before the first frame update
    void Start()
    {
        searchScrollContentRectTransform = searchScrollView.content;
        cameraManager = GameObject.Find("CameraManager").GetComponent<CameraManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void onSearchClick()
    {
        searchList.Clear();
        ClearScrollView();

        if (cameraManager.isInsideActive())
        {
            cameraManager.SetInsideCameraToInactivePosition();
        }


        searchOfficePanel.SetActive(true);
        PopulateScrollWithOffices(pact1, searchScrollContentRectTransform);

        SortSearchedOfficeList(searchScrollContentRectTransform);
    }

    private void PopulateScrollWithOffices(GameObject building, RectTransform rectTransform)
    {
        GameObject buildingInside = GameObject.Find(building.name + "Inside");

        List<string> imagesPath = new List<string>();
        List<string> titles = new List<string>();
        List<string> objectNames = new List<string>();
        List<string> locations = new List<string>();

        foreach (Transform child in buildingInside.transform)
        {
            if (child.GetComponent<ClickOnObject>() != null)
            {
                InformationSerialize buildingObject = child.GetComponent<ClickOnObject>().getBuilding();
                imagesPath.Add(buildingObject.iconPath);
                titles.Add(buildingObject.title);
                objectNames.Add(buildingObject.gameObjectName);
                locations.Add(building.name + ".0 (Piso 0)");
            }
        }

        if (building == pact1)
        {
            GameObject buildingInside2 = GameObject.Find(building.name + "Inside2");
            foreach (Transform child in buildingInside2.transform)
            {
                if (child.GetComponent<ClickOnObject>() != null)
                {
                    InformationSerialize buildingObject = child.GetComponent<ClickOnObject>().getBuilding();
                    imagesPath.Add(buildingObject.iconPath);
                    titles.Add(buildingObject.title);
                    objectNames.Add(buildingObject.gameObjectName);
                    locations.Add(building.name + ".0 (Piso 1)");
                }
            }
        }

        for (int i = 0; i < imagesPath.Count; i++)
        {
            GameObject newOfficeSearchGO = Instantiate(officeSearchListPanelPrefab, rectTransform);

            newOfficeSearchGO.transform.Find("LogoImage")
                .GetComponent<Image>().sprite = GetOfficeIconFromPath(imagesPath[i]);

            // Create a local copy of the loop variable to capture the correct value in the closure
            string objectName = objectNames[i];
            newOfficeSearchGO.transform.Find("LogoImage").GetComponent<Button>()
                .onClick.AddListener(delegate { onLogoClick(objectName); });


            newOfficeSearchGO.transform.Find("OfficeNameTMP")
                .GetComponent<TextMeshProUGUI>().text = titles[i];
            newOfficeSearchGO.transform.Find("OfficeNameTMP")
                .GetComponent<TextMeshProUGUI>().name = objectNames[i];
            newOfficeSearchGO.transform.Find("LocationNameTMP")
                .GetComponent<TextMeshProUGUI>().text = locations[i];
        }
    }



    private void SortSearchedOfficeList(RectTransform rectTransform)
    {
        searchList.Clear(); // Clear the list to avoid duplications if the method is called multiple times

        // Add all child GameObjects to the list
        foreach (Transform child in rectTransform)
        {
            searchList.Add(child.gameObject);
        }

        // Sort the list by the text of the TextMeshProUGUI component
        List<GameObject> sortedList = searchList
            .OrderBy(x => x.GetComponentInChildren<TextMeshProUGUI>().text)
            .ToList();

        // Reassign sibling indices to sort them in the hierarchy
        for (int i = 0; i < sortedList.Count; i++)
        {
            sortedList[i].transform.SetSiblingIndex(i);
        }
        // Update searchList to maintain the sorted order
        searchList = sortedList;
    }

    private void ClearScrollView()
    {
        foreach (Transform child in searchScrollContentRectTransform)
        {
            Destroy(child.gameObject);
        }
    }

    private Sprite GetOfficeIconFromPath(string currentOfficePath)
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

    public void onLogoClick(string name)
    {
        GameObject foundOffice = GameObject.Find(name);

        cameraManager.SetCurrentLocation(foundOffice.transform.parent.gameObject);


        if (foundOffice != null)
        {
            cameraManager.SetInsideCameraMain();
            Vector3 officePosition = foundOffice.transform.position;

            Vector3 newPosition = cameraManager.getInsideCameraGO().transform.position;
            newPosition.x = officePosition.x;
            newPosition.z = officePosition.z;
            cameraManager.SetInsideCameraPosition(newPosition);
            searchOfficePanel.SetActive(false);

            searchBTN.SetActive(true);
            helpBTN.SetActive(true);
            goBackZoomBTN.SetActive(true);
        }
    }

    public void onExitClick()
    {
        if (cameraManager.isInsideActive())
        {
            Vector3 location = cameraManager.GetCurrentLocation().transform.position;
            Vector3 newPosition = cameraManager.getInsideCameraGO().transform.position;
            newPosition.x = location.x;
            newPosition.z = location.z;
            cameraManager.SetInsideCameraPosition(newPosition);
            goBackZoomBTN.SetActive(true);
            currentLocationTMP.SetActive(true);
        }
    }
}
