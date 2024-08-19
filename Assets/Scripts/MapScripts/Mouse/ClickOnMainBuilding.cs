using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ClickOnMainBuilding : MonoBehaviour
{
    // Zoom variables
    private float zoom;
    private float zoomMultiplier = 4f;
    private float minZoom = 2f;
    private float maxZoom = 8f;
    private float velocity = 0f;
    private float smoothTime = 0.25f;
    private bool isZooming = false;

    private GameObject clickedBuilding;
    private float cameraOffSet = 2f;

    Ray ray;
    RaycastHit hit;

    [SerializeField] private GameObject insideCamera;
    [SerializeField] private GameObject goBackButton;
    [SerializeField] private GameObject locationTMP;

    [SerializeField] private List<Material> materialOutlinePicked;
    [SerializeField] private List<Material> materialsOutlineDefault;

    [SerializeField] private GameObject enterBuildingBTN;


    private GameObject previousClickedObject;

    private MouseManager mouseManager;
    private CameraManager cameraManager;
    private HelperManager helperManager;

    // Start is called before the first frame update
    void Start()
    {
        zoom = GetComponent<Camera>().orthographicSize;
        mouseManager = GameObject.Find("MouseManager").GetComponent<MouseManager>();
        cameraManager = GameObject.Find("CameraManager").GetComponent <CameraManager>();
        helperManager = GameObject.Find("HelperManager").GetComponent<HelperManager>();
    }

    // Update is called once per frame
    void Update()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            clickedBuilding = hit.collider.gameObject;
            if (Input.GetMouseButtonDown(0) && !isZooming)
            {
                StartCoroutine(WaitOnClick(1f));
            }
        }
    }

    IEnumerator WaitOnClick(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (!mouseManager.getDragging() && mouseManager.getClicked())
        {
            if (helperManager.isHelperTalking())
            {
                helperManager.HideWhenObjClicked();
            }
            AddOutLine();
            //StartCoroutine(ZoomInBuildingCoroutine(2f)); // Zoom in for 2 seconds
        }
    }

    private void AddOutLine()
    {
        int lastChildIndex = clickedBuilding.transform.childCount - 1;
        Transform outlineParent = clickedBuilding.transform.GetChild(lastChildIndex);
        for (int i = 0; i < outlineParent.transform.childCount; i++)
        {
            if(outlineParent.transform.GetChild(i).GetComponent<MeshRenderer>() != null)
            {
                Transform currentChild = outlineParent.transform.GetChild(i);
                currentChild.GetComponent<MeshRenderer>().SetMaterials(materialOutlinePicked);
            }
        }

        if(previousClickedObject != null)
        {
            for (int i = 0; i < previousClickedObject.transform.childCount; i++)
            {
                if (previousClickedObject.transform.GetChild(i).GetComponent<MeshRenderer>() != null)
                {
                    Transform currentChild = previousClickedObject.transform.GetChild(i);
                    currentChild.GetComponent<MeshRenderer>().SetMaterials(materialsOutlineDefault);
                }
            }
        }
        previousClickedObject = outlineParent.gameObject;
        enterBuildingBTN.SetActive(true);
    }


    public void OnEnterClick()
    {
        enterBuildingBTN.SetActive(false);
        TeleportCameraToBuilding();
    }


    /*IEnumerator ZoomInBuildingCoroutine(float duration)
    {
        isZooming = true;
        float targetZoom = Mathf.Clamp(zoom - zoomMultiplier, minZoom, maxZoom);
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            GetComponent<Camera>().orthographicSize = Mathf.SmoothDamp(GetComponent<Camera>().orthographicSize, targetZoom, ref velocity, smoothTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the final zoom level is set correctly after the coroutine ends
        GetComponent<Camera>().orthographicSize = targetZoom;
        isZooming = false;
        TeleportCameraToBuilding();
    }*/

    void TeleportCameraToBuilding()
    {


        GameObject insideBuilding = GameObject.Find(clickedBuilding.name + "Inside");

        if (insideBuilding != null)
        {
            cameraManager.SetCurrentLocation(insideBuilding);
            Vector3 newPosition = insideCamera.transform.position;
            newPosition.x = insideBuilding.transform.position.x - cameraOffSet;
            cameraManager.SetInsideCameraPosition(newPosition);

            cameraManager.SetInsideCameraMain();

            if(insideBuilding.name == "PACT1Inside")
            {
                locationTMP.GetComponent<TextMeshProUGUI>().text = clickedBuilding.name + ".0 (Piso 0)";
            } else
            {
                locationTMP.GetComponent<TextMeshProUGUI>().text = clickedBuilding.name + ".0";
            }

            goBackButton.SetActive(true);
            locationTMP.SetActive(true);

            //outsideCameraGameObject.GetComponent<Camera>().orthographicSize = 5;
        }
    }
}
