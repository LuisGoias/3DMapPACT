using System.Collections;
using System.Collections.Generic;
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


    private RectTransform searchScrollContentRectTransform;


    private List<GameObject> searchList = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        searchScrollContentRectTransform = searchScrollView.content;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void PopulateScrollWithOffices(GameObject building, RectTransform rectTransform)
    {
        GameObject buildingInside = GameObject.Find(building.name + "Inside");
        List<Sprite> images = new List<Sprite>();
        List<string> titles = new List<string>();

        foreach (Transform child in buildingInside.transform)
        {
            if (child.GetComponent<ClickOnObject>() != null)
            {
                InformationObject buildingObject =
                    child.GetComponent<ClickOnObject>().getBuilding();
                images.Add(buildingObject.icon);
                titles.Add(buildingObject.title);
            }
        }

        for (int i = 0; i < images.Count; i++)
        {
            GameObject newOfficeSearchGO = Instantiate(officeSearchListPanelPrefab, rectTransform);
            newOfficeSearchGO.transform.Find("LogoImage").GetComponent<Image>().sprite = images[i];
            newOfficeSearchGO.GetComponentInChildren<TextMeshProUGUI>().text = titles[i];
        }
    }

    public void onSearchClick()
    {
        searchOfficePanel.SetActive(true);
        PopulateScrollWithOffices(pact1, searchScrollContentRectTransform);
        
        PopulateListWithOffices(searchScrollContentRectTransform);
    }

    private void PopulateListWithOffices(RectTransform rectTransform)
    {
        foreach(Transform child in rectTransform)
        {
            searchList.Add(child.gameObject);
        }
        SortSearchedOfficeList(rectTransform);
    }

    private void SortSearchedOfficeList(RectTransform rectTransform)
    {
        // Sort the searchList based on the text of the TextMeshProUGUI component
        List<GameObject> sortedList = searchList.OrderBy
            (x => x.gameObject.GetComponentInChildren<TextMeshProUGUI>().text).ToList();

        // Clear the existing children of the scroll content
        foreach (Transform child in rectTransform)
        {
            Destroy(child.gameObject);
        }

        // Add the sorted GameObjects back to the scroll content
        for(int i = 0;  i < sortedList.Count; i++)
        {
            GameObject gameObject = Instantiate(sortedList[i]);
            gameObject.transform.Find("LogoImage").GetComponent<Image>().enabled = true;
            gameObject.GetComponentInChildren<TextMeshProUGUI>().enabled = true;
            gameObject.transform.SetParent(rectTransform);
        }

        // Update searchList to maintain the sorted order
        searchList = sortedList;
    }

}
