using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class OfficeListingManager : MonoBehaviour
{
    [SerializeField] private GameObject pact1;
    [SerializeField] private GameObject pact3;

    [SerializeField] private ScrollRect pact1ScrollView;
    [SerializeField] private ScrollRect pact3ScrollView;

    [SerializeField] private GameObject officeImagePrefab;


    private RectTransform pact1ContentRectTransform;
    private RectTransform pact3ContentRectTransform;
    // Start is called before the first frame update
    void Start()
    {
        pact1ContentRectTransform = pact1ScrollView.content;
        pact1ContentRectTransform = pact3ScrollView.content;
        PopulateScrollWithOffices(pact1, pact1ContentRectTransform);
        PopulateScrollWithOffices(pact3, pact3ContentRectTransform);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void PopulateScrollWithOffices(GameObject building, RectTransform rectTransform)
    {        
        GameObject buildingInside = GameObject.Find(building.name + "Inside");
        List<string> imagePaths = new List<string>();
        
        if(building == pact1)
        {
            foreach(Transform child in buildingInside.transform)
            {
                if(child.GetComponent<ClickOnObject>() != null)
                {
                    InformationSerialize buildingObject = 
                        child.GetComponent<ClickOnObject>().getBuilding();
                    imagePaths.Add(buildingObject.iconPath);
                }
            }

            GameObject buildingInside2 = GameObject.Find(building.name + "Inside2");

            foreach (Transform child in buildingInside2.transform)
            {
                if (child.GetComponent<ClickOnObject>() != null)
                {
                    InformationSerialize buildingObject =
                        child.GetComponent<ClickOnObject>().getBuilding();
                    imagePaths.Add(buildingObject.iconPath);
                }
            }
        }




        for (int i = 0;  i < imagePaths.Count; i++)
        {
            GameObject newOfficeGO = Instantiate(officeImagePrefab, rectTransform);
            string currentPath = imagePaths[i];
            newOfficeGO.GetComponent<Image>().sprite = GetOfficeIconFromPath(currentPath);
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
}
