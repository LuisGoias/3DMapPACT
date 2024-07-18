using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class AdminManager : MonoBehaviour
{
    [SerializeField] private Toggle adminToggle;
    [SerializeField] private GameObject adminBanner;
    [SerializeField] private GameObject adminIcon;


    private string path;
    private Image image;

    private bool adminEnabled = false;

    private bool adminBannerPicked = false;

    private bool adminTitleChanged = false;

    public InformationObject building;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (adminToggle.isOn)
        {
            adminEnabled = true;
        }
        else
        {
            adminEnabled = false;
        }

    }

    public bool GetAdminStatus()
    {
        return adminEnabled;
    }

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
        path = EditorUtility.OpenFilePanel("Overwrite with png", "", "png");
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
            Sprite newSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

            if (adminBannerPicked)
            {
                image = adminBanner.GetComponentInChildren<Image>();
                image.sprite = newSprite;
                building.banner = image.sprite;
            }
            else
            {
                image = adminIcon.GetComponentInChildren<Image>();
                image.sprite = newSprite;
                building.icon = image.sprite;
            }
        }
    }


    public void SetText(GameObject gameObject)
    {
        SetAdminTitleChanged(gameObject);

        if (adminTitleChanged)
        {
            building.title = gameObject.GetComponent<TMP_InputField>().text;
        } else
        {
            building.description = gameObject.GetComponent<TMP_InputField>().text;
        }
    }
}
