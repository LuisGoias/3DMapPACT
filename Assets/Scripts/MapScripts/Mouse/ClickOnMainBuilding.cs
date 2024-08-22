using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ClickOnMainBuilding : MonoBehaviour
{
    // Zoom variables
    /*private float zoom;
    private float zoomMultiplier = 4f;
    private float minZoom = 2f;
    private float maxZoom = 8f;
    private float velocity = 0f;
    private float smoothTime = 0.25f;*/
    private bool isZooming = false;

    private GameObject clickedBuilding;
    private float cameraOffSet = 2f;

    Ray ray;
    RaycastHit hit;

    [SerializeField] private GameObject insideCamera;
    [SerializeField] private GameObject insideCamera3;
    [SerializeField] private GameObject goBackButton;
    [SerializeField] private GameObject locationTMP;

    [SerializeField] private List<Material> materialOutlinePicked;
    [SerializeField] private List<Material> materialsOutlineDefault;

    [SerializeField] private GameObject enterBuildingBTN;


    private GameObject previousClickedObject;
    private GameObject previousClickedOutline;

    private MouseManager mouseManager;
    private CameraManager cameraManager;
    private HelperManager helperManager;

    // Start is called before the first frame update
    void Start()
    {
        //zoom = GetComponent<Camera>().orthographicSize;
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

        if (previousClickedObject == null)
        {
            int lastChildIndex = clickedBuilding.transform.childCount - 1;
            Transform outlineParent = clickedBuilding.transform.GetChild(lastChildIndex);
            for (int i = 0; i < outlineParent.transform.childCount; i++)
            {
                if (outlineParent.transform.GetChild(i).GetComponent<MeshRenderer>() != null)
                {
                    Transform currentChild = outlineParent.transform.GetChild(i);
                    currentChild.GetComponent<MeshRenderer>().SetMaterials(materialOutlinePicked);
                }
            }
            enterBuildingBTN.SetActive(true);

            previousClickedOutline = outlineParent.gameObject;
            previousClickedObject = clickedBuilding;
        } else
        {
            if (previousClickedObject.name == clickedBuilding.name)
            {
                for (int i = 0; i < previousClickedOutline.transform.childCount; i++)
                {
                    if (previousClickedOutline.transform.GetChild(i).GetComponent<MeshRenderer>() != null)
                    {
                        Transform currentChild = previousClickedOutline.transform.GetChild(i);
                        currentChild.GetComponent<MeshRenderer>().SetMaterials(materialsOutlineDefault);
                    }
                }
                previousClickedObject = null;
                enterBuildingBTN.SetActive(false);
            }
            else
            {
                for (int i = 0; i < previousClickedOutline.transform.childCount; i++)
                {
                    if (previousClickedOutline.transform.GetChild(i).GetComponent<MeshRenderer>() != null)
                    {
                        Transform currentChild = previousClickedOutline.transform.GetChild(i);
                        currentChild.GetComponent<MeshRenderer>().SetMaterials(materialsOutlineDefault);
                    }
                }
            }
        }

    }




    public void OnEnterClick()
    {
        RemoveOutlineOnEnter();
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


    private void RemoveOutlineOnEnter()
    {
        if (previousClickedObject.name == clickedBuilding.name)
        {
            for (int i = 0; i < previousClickedOutline.transform.childCount; i++)
            {
                if (previousClickedOutline.transform.GetChild(i).GetComponent<MeshRenderer>() != null)
                {
                    Transform currentChild = previousClickedOutline.transform.GetChild(i);
                    currentChild.GetComponent<MeshRenderer>().SetMaterials(materialsOutlineDefault);
                }
            }
            previousClickedObject = null;
            enterBuildingBTN.SetActive(false);
        }
    }

    void TeleportCameraToBuilding()
    {


        GameObject insideBuilding = GameObject.Find(clickedBuilding.name + "Inside");

        if (insideBuilding != null)
        {
            if(clickedBuilding.name == "PACT1")
            {
                cameraManager.SetCurrentLocation(insideBuilding);
                Vector3 newPosition = insideCamera.transform.position;
                newPosition.x = insideBuilding.transform.position.x - cameraOffSet;
                newPosition.z = insideBuilding.transform.position.z;
                cameraManager.SetInsideCameraPosition(newPosition);

                cameraManager.SetInsideCameraMain();

                if(insideBuilding.name == "PACT1Inside")
                {
                    locationTMP.GetComponent<TextMeshProUGUI>().text = clickedBuilding.name + ".0 (Piso 0)";
                } else
                {
                    locationTMP.GetComponent<TextMeshProUGUI>().text = clickedBuilding.name + ".0";
                }
            } else if (clickedBuilding.name == "PACT3")
            {
                cameraManager.SetCurrentLocation(insideBuilding);
                Vector3 newPosition = insideCamera3.transform.position;
                newPosition.x = insideBuilding.transform.position.x - cameraOffSet;
                newPosition.z = insideBuilding.transform.position.z;
                cameraManager.SetInsideCamera3Position(newPosition);

                cameraManager.SetInside3CameraMain();

                if (insideBuilding.name == "PACT1Inside")
                {
                    locationTMP.GetComponent<TextMeshProUGUI>().text = clickedBuilding.name + ".0 (Piso 0)";
                }
                else
                {
                    locationTMP.GetComponent<TextMeshProUGUI>().text = clickedBuilding.name + ".0";
                }
            }


            goBackButton.SetActive(true);
            locationTMP.SetActive(true);

            //outsideCameraGameObject.GetComponent<Camera>().orthographicSize = 5;
        }
    }
}
