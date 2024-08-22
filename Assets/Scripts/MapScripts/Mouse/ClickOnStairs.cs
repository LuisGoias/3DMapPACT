using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickOnStairs : MonoBehaviour
{
    [SerializeField] private Camera insideCamera;
    [SerializeField] private Camera insideCamera2;

    Ray ray;
    RaycastHit hit;

    private CameraManager cameraManager;
    private HelperManager helperManager;

    // Start is called before the first frame update
    void Start()
    {
        cameraManager = GameObject.Find("CameraManager").GetComponent<CameraManager>();
        helperManager = GameObject.Find("HelperManager").GetComponent <HelperManager>();
    }

    // Update is called once per frame
    void Update()
    {
        ray = insideCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            if (Input.GetMouseButtonDown(0) &&
                hit.collider.transform.gameObject.name == "Stairs")
            {
                if (helperManager.isHelperTalking())
                {
                    helperManager.HideWhenObjClicked();
                }
                TeleportCameraToStairs();
            }
        }
    }

    void TeleportCameraToStairs()
    {


        GameObject insideBuilding = GameObject.Find("PACT1Inside2");

        if (insideBuilding != null)
        {

            // Move the insideCamera to the same X position as the clicked building
            Vector3 newPosition = insideCamera2.transform.position;
            newPosition.x = insideBuilding.transform.position.x;
            newPosition.z = insideBuilding.transform.position.z;
            insideCamera2.transform.position = newPosition;

            cameraManager.SetInside2CameraMain();
            
        }
    }
}
