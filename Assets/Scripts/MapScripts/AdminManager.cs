using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.IO;
using SFB;
using System;

public class AdminManager : MonoBehaviour
{
    [SerializeField] private GameObject officeListPanel;
    [SerializeField] private ScrollRect officeListScroll;

    [SerializeField] private GameObject officeListedPrefab;

    [SerializeField] private GameObject officeInfoPanel;
    [SerializeField] private GameObject officeBanner;
    [SerializeField] private GameObject officeIcon;
    [SerializeField] private GameObject officeInfoTitle;
    [SerializeField] private GameObject officeInfoDescription;

    private List<InformationObject> informationObjects = new List<InformationObject>();

    private string path;
    private Image image;


    private bool adminBannerPicked = false;

    private bool adminTitleChanged = false;

    private InformationObject buildingToChange;


    private RectTransform officeContentRectTransform;

    // Start is called before the first frame update
    void Start()
    {
        officeContentRectTransform = officeListScroll.content;

    }

    // Update is called once per frame
    void Update()
    {


    }

    #region List Office
    public void ClickListOfficeChange()
    {
        string folderPath = "Assets/Objects/Locations"; // Replace with your folder path
        informationObjects = LoadAllScriptableObjects(folderPath);

        foreach (var obj in informationObjects)
        {
            Debug.Log(obj.name);
        }

        PopulateScrollWithOffices(officeContentRectTransform);
    }

    private static List<InformationObject> LoadAllScriptableObjects(string path)
    {
        // Get all asset paths from the folder
        string[] assetPaths = AssetDatabase.FindAssets("t:" + typeof(InformationObject).Name, new[] { path });
        List<InformationObject> scriptableObjects = new List<InformationObject>();

        // Load each asset and add to the list
        foreach (string assetPath in assetPaths)
        {
            string fullPath = AssetDatabase.GUIDToAssetPath(assetPath);
            InformationObject obj = AssetDatabase.LoadAssetAtPath<InformationObject>(fullPath);
            if (obj != null)
            {
                scriptableObjects.Add(obj);
            }
        }

        return scriptableObjects;
    }


    private void PopulateScrollWithOffices(RectTransform rectTransform)
    {
        for (int i = 0; i < informationObjects.Count; i++)
        {
            GameObject newOfficeSearchGO = Instantiate(officeListedPrefab, rectTransform);

            InformationObject currentObject = informationObjects[i];

            newOfficeSearchGO.transform.Find("OfficeImage")
                .GetComponent<Image>().sprite = currentObject.icon;

            newOfficeSearchGO.transform.Find("OfficeTitle")
                .GetComponent<TextMeshProUGUI>().text = currentObject.title;

            newOfficeSearchGO.transform.Find("OfficeImage")
                .GetComponent<Button>().onClick
                .AddListener(delegate { OfficeImageClick(currentObject); });
        }
    }

    private void OfficeImageClick(InformationObject informationObject)
    {
        buildingToChange = informationObject;
        officeListPanel.SetActive(false);
        officeInfoPanel.SetActive(true);

        officeBanner.GetComponent<Image>().sprite = buildingToChange.banner;
        officeIcon.GetComponent<Image>().sprite = buildingToChange.icon;
        officeInfoTitle.GetComponent<TMP_InputField>().text = buildingToChange.title;
        officeInfoDescription.GetComponent<TMP_InputField>().text = buildingToChange.description;
    }


    #endregion

    #region Change Office Details
    private void SetAdminBannerPicked(GameObject gameObject)
    {
        if (gameObject.name == "InfromationBannerADM")
        {
            adminBannerPicked = true;
        }
        else
        {
            adminBannerPicked = false;
        }
    }

    private void SetAdminTitleChanged(GameObject gameObject)
    {
        if(gameObject.name == "InformationTitleFieldTMP")
        {
            adminTitleChanged = true;
        } else
        {
            adminTitleChanged = false;
        }
    }


    public void OpenExplorer(GameObject gameObject)
    {
        SetAdminBannerPicked(gameObject);
        //TODO
        // Open file with filter
        var extensions = new[] {
            new ExtensionFilter("Image Files", "png", "jpg", "jpeg" )
        };
        var paths = StandaloneFileBrowser.OpenFilePanel("Open File", "", extensions, true);
        
        path = "";
        foreach (var p in paths)
        {
            path += p + "\n";
        }

        GetImage();
    }


    void GetImage()
    {
        if (path != null)
        {
            UpdateImage();
        }
    }

    void UpdateImage()
    {
        StartCoroutine(DownloadImage());
    }

    IEnumerator DownloadImage()
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture("file:///" + path);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError ||
            request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError(request.error);
        }
        else
        {
            Texture2D texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
            
            Sprite newSprite = Sprite.Create(texture, 
                new Rect(0.0f, 0.0f, texture.width, texture.height),
                new Vector2(0.5f, 0.5f), 100.0f);

            if (adminBannerPicked)
            {
                image = officeBanner.GetComponentInChildren<Image>();
                image.sprite = newSprite;
                buildingToChange.SetBanner(newSprite);
            }
            else
            {
                image = officeIcon.GetComponentInChildren<Image>();
                image.sprite = newSprite;
                buildingToChange.SetIcon(newSprite);
            }
        }
    }


    public void SetText(GameObject gameObject)
    {
        SetAdminTitleChanged(gameObject);

        if (adminTitleChanged)
        {
            buildingToChange.title = gameObject.GetComponent<TMP_InputField>().text;
        } else
        {
            buildingToChange.description = gameObject.GetComponent<TMP_InputField>().text;
        }
    }
    #endregion
}
