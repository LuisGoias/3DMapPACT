using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FileManager : MonoBehaviour
{
    private string filePath;

    [SerializeField] private List<GameObject> buildings;

    private List<InformationSerialize> offices;

    private void Awake()
    {
        filePath = Application.persistentDataPath + "/informationData.json";
        offices = LoadInformation();

        Debug.Log(filePath);
        if (SceneManager.GetActiveScene().name == "MapScene")
        {
            if (offices == null || offices.Count == 0)
            {
                Debug.Log("Saved");
                SaveNewFile();
            }
            else
            {
                Debug.Log("Loaded");
                LoadExistingFile();
            }
        }
    }

    private void Start()
    {
        // Start is kept for any future use
    }

    public void SaveInformation(List<InformationSerialize> infoList)
    {
        string json = JsonUtility.ToJson(new SerializationWrapper<InformationSerialize>(infoList));
        File.WriteAllText(filePath, json);
    }

    public List<InformationSerialize> LoadInformation()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            return JsonUtility.FromJson<SerializationWrapper<InformationSerialize>>(json).data;
        }
        return new List<InformationSerialize>();
    }

    private void SaveNewFile()
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
        SaveInformation(offices);
    }

    private void LoadExistingFile()
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
