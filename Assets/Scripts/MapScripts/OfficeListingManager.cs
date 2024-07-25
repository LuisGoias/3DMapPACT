using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OfficeListingManager : MonoBehaviour
{
    [SerializeField] private GameObject pact1;
    [SerializeField] private ScrollRect pact1ScrollView;
    [SerializeField] private GameObject officeImagePrefab;


    private RectTransform pact1ContentRectTransform;
    // Start is called before the first frame update
    void Start()
    {
        pact1ContentRectTransform = pact1ScrollView.content;
        PopulateScrollWithOffices(pact1, pact1ContentRectTransform); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void PopulateScrollWithOffices(GameObject building, RectTransform rectTransform)
    {
        GameObject buildingInside = GameObject.Find(building.name + "Inside");
        List<Sprite> images = new List<Sprite>();

        foreach(Transform child in buildingInside.transform)
        {
            if(child.GetComponent<ClickOnObject>() != null)
            {
                InformationObject buildingObject = 
                    child.GetComponent<ClickOnObject>().getBuilding();
                images.Add(buildingObject.icon);
            }
        }

        for (int i = 0;  i < images.Count; i++)
        {
            GameObject newOfficeGO = Instantiate(officeImagePrefab, rectTransform);
            newOfficeGO.GetComponent<Image>().sprite = images[i];
        }
    }
}
