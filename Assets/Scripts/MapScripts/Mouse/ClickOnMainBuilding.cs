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
    [SerializeField] private GameObject adminToggle;
    [SerializeField] private GameObject locationTMP;


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
            StartCoroutine(ZoomInBuildingCoroutine(2f)); // Zoom in for 2 seconds
        }
    }

    IEnumerator ZoomInBuildingCoroutine(float duration)
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
    }

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
            adminToggle.SetActive(true);
            locationTMP.SetActive(true);

            //outsideCameraGameObject.GetComponent<Camera>().orthographicSize = 5;
        }
    }
}
