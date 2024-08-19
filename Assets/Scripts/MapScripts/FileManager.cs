//using Palmmedia.ReportGenerator.Core.Common;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FileManager : MonoBehaviour
{
    private string officeInfoFilePath;
    private string helperFilePath;

    [SerializeField] private List<GameObject> buildings;

    private List<InformationSerialize> offices;

    private HelperSerialize helper;

    private void Awake()
    {
        officeInfoFilePath = Application.persistentDataPath + "/informationData.json";
        offices = LoadInformationFile();

        helperFilePath = Application.persistentDataPath + "/helperData.json";
        helper = LoadHelperFile();

        //Debug.Log(officeInfoFilePath);
        Debug.Log(helperFilePath);
        if (SceneManager.GetActiveScene().name == "MapScene")
        {
            if (offices == null || offices.Count == 0)
            {
                Debug.Log("Saved");
                SaveNewInfo();
            }
            else
            {
                Debug.Log("Loaded");
                LoadInfo();
            }

            if(helper.questions == null && helper.answers == null)
            {
                SaveHelper();
            } else
            {
                LoadHelper();
            }
            
        }
    }

    private void Start()
    {
        // Start is kept for any future use
    }

    #region Save/Load JSON
    public void SaveInformationFile(List<InformationSerialize> infoList)
    {
        string json = JsonUtility.ToJson(new SerializationWrapper<InformationSerialize>(infoList));
        File.WriteAllText(officeInfoFilePath, json);
    }

    public List<InformationSerialize> LoadInformationFile()
    {
        if (File.Exists(officeInfoFilePath))
        {
            string json = File.ReadAllText(officeInfoFilePath);
            return JsonUtility.FromJson<SerializationWrapper<InformationSerialize>>(json).data;
        }
        return new List<InformationSerialize>();
    }


    public void SaveHelperFile(HelperSerialize helperToSave)
    {
        string json = JsonUtility.ToJson(helperToSave);
        File.WriteAllText(helperFilePath, json);
    }

    public HelperSerialize LoadHelperFile()
    {
        if (File.Exists(helperFilePath))
        {
            string json = File.ReadAllText(helperFilePath);
            return JsonUtility.FromJson<HelperSerialize>(json);
        }
        return new HelperSerialize();
    }

    #endregion

    #region Info Save and Load functions
    private void SaveNewInfo()
    {
        offices = new List<InformationSerialize>();
        for (int i = 0; i < buildings.Count; i++)
        {
            GameObject buildingInside = GameObject.Find(buildings[i].name + "Inside");

            if (buildingInside != null)
            {
                foreach (Transform child in buildingInside.transform)
                {
                    if (child.GetComponent<ClickOnObject>() != null)
                    {
                        InformationSerialize buildingObject = child.GetComponent<ClickOnObject>().getBuilding();
                        offices.Add(buildingObject);
                    }
                }
            }
            else
            {
                Debug.LogWarning($"BuildingInside not found for {buildings[i].name}");
            }
        }
        SaveInformationFile(offices);
    }

    private void LoadInfo()
    {
        for (int i = 0; i < buildings.Count; i++)
        {
            GameObject buildingInside = GameObject.Find(buildings[i].name + "Inside");

            if (buildingInside != null)
            {
                foreach (Transform child in buildingInside.transform)
                {
                    if (child.GetComponent<ClickOnObject>() != null)
                    {
                        InformationSerialize buildingInList = offices.Find(x => x.gameObjectName == child.gameObject.name);

                        if (buildingInList != null)
                        {
                            child.GetComponent<ClickOnObject>().setBuilding(buildingInList);
                        }
                        else
                        {
                            Debug.LogWarning($"No saved data found for {child.gameObject.name}");
                        }
                    }
                }
            }
        }
    }

    #endregion

    #region Helper Save and Load functions
    private void SaveHelper()
    {
        helper = new HelperSerialize();

        helper = GameObject.Find("HelperManager")
            .GetComponent<HelperManager>().GetHelperSerialize();

        SaveHelperFile(helper);
    }


    private void LoadHelper()
    {
        GameObject.Find("HelperManager")
            .GetComponent<HelperManager>().SetHelperSerialize(helper);
    }

    #endregion

    [System.Serializable]
    private class SerializationWrapper<T>
    {
        public List<T> data;
        public SerializationWrapper(List<T> data)
        {
            this.data = data;
        }
    }
}
