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

    [SerializeField] private GameObject helperValuesPanelPrefab;
    [SerializeField] private GameObject helperAddMoreValuesBTN;
    [SerializeField] private ScrollRect helperValuesScroll;



    [SerializeField] private GameObject materialListPanel;
    [SerializeField] private ScrollRect materialListScrollView;
    [SerializeField] private GameObject materialPanelPrefab;
    [SerializeField] private GameObject materialCube;

    [SerializeField] private GameObject colorPickerPanel;

    private List<InformationSerialize> informationOffices = new List<InformationSerialize>();
    private HelperSerialize helper;
    private List<Material> materials = new List<Material>();

    private string path;
    private Image image;


    private bool adminBannerPicked = false;

    private bool adminTitleChanged = false;

    private InformationSerialize buildingToChange;


    private RectTransform officeContentRectTransform;
    private RectTransform materiaContentRectTransform;
    private RectTransform helperContentRectTransform;


    private ColorPickerManager colorPickerManager;

    private FileManager fileManager;

    // Start is called before the first frame update
    void Start()
    {
        officeContentRectTransform = officeListScroll.content;
        materiaContentRectTransform = materialListScrollView.content;
        helperContentRectTransform = helperValuesScroll.content;

        colorPickerManager = GameObject.Find("ColorPickerManager").GetComponent<ColorPickerManager>();
        fileManager = GameObject.Find("FileManager").GetComponent<FileManager>();
    }

    // Update is called once per frame
    void Update()
    {


    }

    #region List Office
    public void ClickListOfficeChange()
    {
        //string folderPath = "Assets/Objects/Locations"; // Replace with your folder path
        informationOffices = fileManager.LoadInformationFile();
        PopulateScrollWithOffices(officeContentRectTransform);
    }

    /* private static List<InformationObject> LoadAllScriptableObjects(string path)
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
     }*/


    private void PopulateScrollWithOffices(RectTransform rectTransform)
    {
        for (int i = 0; i < informationOffices.Count; i++)
        {
            GameObject newOfficeSearchGO = Instantiate(officeListedPrefab, rectTransform);

            InformationSerialize currentObject = informationOffices[i];

            newOfficeSearchGO.transform.Find("OfficeImage")
                .GetComponent<Image>().sprite = currentObject.icon;

            newOfficeSearchGO.transform.Find("OfficeTitle")
                .GetComponent<TextMeshProUGUI>().text = currentObject.title;

            newOfficeSearchGO.transform.Find("OfficeImage")
                .GetComponent<Button>().onClick
                .AddListener(delegate { OfficeImageClick(currentObject); });
        }
    }

    private void OfficeImageClick(InformationSerialize informationObject)
    {
        buildingToChange = informationObject;
        officeListPanel.SetActive(false);
        officeInfoPanel.SetActive(true);

        officeBanner.GetComponent<Image>().sprite = buildingToChange.banner;
        officeIcon.GetComponent<Image>().sprite = buildingToChange.icon;
        officeInfoTitle.GetComponent<TMP_InputField>().text = buildingToChange.title;
        officeInfoDescription.GetComponent<TMP_InputField>().text = buildingToChange.description;
    }


    public void OnOfficeInfoClickBack()
    {
        for (int i = 0; i < informationOffices.Count; i++)
        {
            if (informationOffices[i].gameObjectName == buildingToChange.gameObjectName)
            {
                informationOffices[i] = buildingToChange;

            }
        }
        fileManager.SaveInformationFile(informationOffices);

        for (int i = 0; i < informationOffices.Count; i++)
        {
            Debug.Log(i + ":");
            Debug.Log(informationOffices[i].gameObjectName);
            Debug.Log(informationOffices[i].title);
            Debug.Log(informationOffices[i].description);
        }
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
        if (gameObject.name == "InformationTitleFieldTMP")
        {
            adminTitleChanged = true;
        }
        else
        {
            adminTitleChanged = false;
        }
    }


    public void OpenExplorer(GameObject gameObject)
    {
        SetAdminBannerPicked(gameObject);

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

            /*Sprite newSprite = Sprite.Create(texture,
                new Rect(0.0f, 0.0f, texture.width, texture.height),
                new Vector2(0.5f, 0.5f), 100.0f);*/

            path = path.Trim();

            var fileData = File.ReadAllBytes(path);


            var testTexture = new Texture2D(2, 2, TextureFormat.RGBA32, false);
            testTexture.LoadImage(fileData);



            Sprite newSprite = Sprite.Create(testTexture,
                new Rect(0.0f, 0.0f, testTexture.width, testTexture.height),
                new Vector2(0.5f, 0.5f), 100.0f);

            if (adminBannerPicked)
            {
                image = officeBanner.GetComponentInChildren<Image>();
                image.sprite = newSprite;
                if (image.sprite != null)
                {
                    Debug.Log("yay");
                    Debug.Log(image.sprite);
                    buildingToChange.SetBanner(image.sprite);
                }
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
        }
        else
        {
            buildingToChange.description = gameObject.GetComponent<TMP_InputField>().text;
        }
    }
    #endregion

    #region List Material

    public void ClickListMaterial()
    {
        RectTransform rectTransform = materiaContentRectTransform;

        List<GameObject> materialsToRemove = new List<GameObject>();
        //TODO Listar materials 
        if (rectTransform.childCount > 0 )
        {
            for(int i = 0; i < rectTransform.childCount; i++)
            {
                if(rectTransform.GetChild(i).name == "MaterialListed(Clone)")
                {
                    materialsToRemove.Add(rectTransform.GetChild(i).gameObject);
                }
            }

            for (int i = 0; i < materialsToRemove.Count; i++)
            {
                DestroyImmediate(materialsToRemove[i]);
            }
        }

        materiaContentRectTransform = rectTransform;

        string folderPath = "Assets/Materials"; // Replace with your folder path
        materials = LoadAllMaterials(folderPath);


        PopulateScrollWithMaterials(materiaContentRectTransform);
    }

    private static List<Material> LoadAllMaterials(string path)
    {
        // Get all asset paths from the folder
        string[] assetPaths = AssetDatabase.FindAssets("t:" + typeof(Material).Name, new[] { path });
        List<Material> materialsFound = new List<Material>();

        // Load each asset and add to the list
        foreach (string assetPath in assetPaths)
        {
            string fullPath = AssetDatabase.GUIDToAssetPath(assetPath);
            Material obj = AssetDatabase.LoadAssetAtPath<Material>(fullPath);
            if (obj != null)
            {
                materialsFound.Add(obj);
            }
        }

        return materialsFound;
    }

    private void PopulateScrollWithMaterials(RectTransform rectTransform)
    {
        for (int i = 0; i < materials.Count; i++)
        {
            GameObject newMaterialGO = Instantiate(materialPanelPrefab, rectTransform);

            Material currentMaterial = materials[i];


            newMaterialGO.transform.Find("ColorImage")
                .GetComponent<Image>().color = materials[i].color;

            newMaterialGO.transform.Find("ColorImage")
                .GetComponent<Button>().onClick
                .AddListener(delegate { OnMaterialClick(currentMaterial); });

            newMaterialGO.transform.Find("MaterialName")
                .GetComponent<TextMeshProUGUI>().text = materials[i].name;
        }
    }

    private void OnMaterialClick(Material materialChosen)
    {
        materialCube.GetComponent<MeshRenderer>().material = materialChosen;
        materialListPanel.SetActive(false);
        colorPickerPanel.SetActive(true);
        materialCube.SetActive(true);
        colorPickerManager.SetChoseMaterial(materialChosen);
    }

    #endregion

    #region HelperChange
    public void ClickHelperChange()
    {
        helper = fileManager.LoadHelperFile();
        PopulateScrollWithHelperValues(helperContentRectTransform);
    }

    private void PopulateScrollWithHelperValues(RectTransform rectTransform)
    {
        for (int i = 0; i < helper.questions.Count; i++)
        {
            GameObject newHelperValuesGO = Instantiate(helperValuesPanelPrefab, rectTransform);

            int currentIndex = i+1;
            string helperQuestion = helper.questions[i];
            string helperAnwsers = helper.answers[i];

            newHelperValuesGO.transform.Find("QuestionNumber")
                .GetComponent<TextMeshProUGUI>().text = "Pergunta #" + currentIndex;

            newHelperValuesGO.transform.Find("QuestionInputField")
                .GetComponent<TMP_InputField>().text = helperQuestion;

            newHelperValuesGO.transform.Find("AnwserNumber")
                .GetComponent<TextMeshProUGUI>().text = "Resposta #" + currentIndex;

            newHelperValuesGO.transform.Find("AnwserInputField")
                .GetComponent<TMP_InputField>().text = helperAnwsers;

            newHelperValuesGO.transform.Find("RemoveValueBTN")
                .GetComponent<Button>().onClick
                .AddListener(delegate { RemoveValueFromHelper(newHelperValuesGO); });
        }
        helperAddMoreValuesBTN.transform.SetAsLastSibling();
    }


    public void OnHelperBackClick()
    {
        RectTransform recTransform = helperContentRectTransform;

        List<GameObject> helperValuePanels = new List<GameObject>();

        for (int i = 0; i < recTransform.childCount; i++)
        {
            if(recTransform.GetChild(i).name == "HelperValuesPanel(Clone)")
            {
                helperValuePanels.Add(recTransform.GetChild(i).gameObject);
            }
        }

        List<string> savedQuestions = new List<string>();
        List<string> savedAnwsers = new List<string>();

        for (int i = 0; i < helperValuePanels.Count; i++)
        {
            string foundQuestion = helperValuePanels[i]
                .transform.Find("QuestionInputField")
                .GetComponent<TMP_InputField>().text;

            string foundAnwser = helperValuePanels[i]
                .transform.Find("AnwserInputField")
                .GetComponent<TMP_InputField>().text;

            savedQuestions.Add(foundQuestion);
            savedAnwsers.Add(foundAnwser);
        }

        helper.setQuestions(savedQuestions);
        helper.setAnswers(savedAnwsers);

        fileManager.SaveHelperFile(helper);


        for (int i = 0; i < helperValuePanels.Count; i++)
        {
            Destroy(helperValuePanels[i]);
        }
    }

    public void AddNewValueToHelper()
    {
        RectTransform rectTransform = helperContentRectTransform;


        List<GameObject> helperValuePanels = new List<GameObject>();

        for (int i = 0; i < rectTransform.childCount; i++)
        {
            if (rectTransform.GetChild(i).name == "HelperValuesPanel(Clone)")
            {
                helperValuePanels.Add(rectTransform.GetChild(i).gameObject);
            }
        }

        GameObject newValueGO = Instantiate(helperValuesPanelPrefab, rectTransform);

        newValueGO.transform.Find("QuestionNumber")
            .GetComponent<TextMeshProUGUI>().text = "Pergunta #" + (helperValuePanels.Count + 1);

        newValueGO.transform.Find("QuestionInputField")
            .GetComponent<TMP_InputField>().text = "";

        newValueGO.transform.Find("AnwserNumber")
            .GetComponent<TextMeshProUGUI>().text = "Resposta #" + (helperValuePanels.Count + 1);

        newValueGO.transform.Find("AnwserInputField")
            .GetComponent<TMP_InputField>().text = "";

        newValueGO.transform.Find("RemoveValueBTN")
            .GetComponent<Button>().onClick
            .AddListener(delegate { RemoveValueFromHelper(newValueGO); });

        helperAddMoreValuesBTN.transform.SetAsLastSibling();

        helperContentRectTransform = rectTransform;

    }


    public void RemoveValueFromHelper(GameObject valueToRemove)
    {
        DestroyImmediate(valueToRemove);

        RectTransform rectTransform = helperContentRectTransform;

        List<GameObject> helperValuePanels = new List<GameObject>();

        for (int i = 0; i < rectTransform.childCount; i++)
        {
            if (rectTransform.GetChild(i).name == "HelperValuesPanel(Clone)")
            {
                helperValuePanels.Add(rectTransform.GetChild(i).gameObject);
            }
        }

        Debug.Log(helperValuePanels.Count);

        for (int i = 0; i < helperValuePanels.Count; i++)
        {
            int currentIndex = i + 1;
            helperValuePanels[i].transform.Find("QuestionNumber")
                .GetComponent<TextMeshProUGUI>().text = "Pergunta #" + currentIndex;

            helperValuePanels[i].transform.Find("AnwserNumber")
                .GetComponent<TextMeshProUGUI>().text = "Resposta #" + currentIndex;
        }

        helperContentRectTransform = rectTransform;

    }
    #endregion
}
