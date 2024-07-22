using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralCameraControls : MonoBehaviour
{
    // Define the camera movement boundaries
    [SerializeField] private float minX = -5f;
    [SerializeField] private float maxX = 0f;
    [SerializeField] private float minY = 2f;
    [SerializeField] private float maxY = 5f;
    [SerializeField] private float minZ = 0f;
    [SerializeField] private float maxZ = 0f;

    [SerializeField] private float cameraSpeed;

    private Vector3 lastMousePosition;

    private MouseManager mouseManager;


    // Start is called before the first frame update
    void Start()
    {
        mouseManager = GameObject.Find("MouseManager").GetComponent<MouseManager>();
    }

    // Update is called once per frame
    void Update()
    {
        // Camera drag logic
        if (Input.GetMouseButtonDown(0))
        {
            lastMousePosition = Input.mousePosition;
        }

        if (Input.GetMouseButton(0) && mouseManager.getDragging())
        {

            if (this.name == "InsideCamera" || this.name == "InsideCamera2")
            {
                MoveZAxis();
            } else
            {
                MoveYAxis();
            }

        }
    }

    void MoveYAxis()
    {
        Vector3 delta = Input.mousePosition - lastMousePosition;

        Vector3 newPosition = Camera.main.transform.position - new Vector3(delta.x * Time.deltaTime, delta.y * Time.deltaTime, 0);

        // Clamp the camera's position to the defined boundaries
        newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);
        newPosition.y = Mathf.Clamp(newPosition.y, minY, maxY);

        Camera.main.transform.position = newPosition;
        lastMousePosition = Input.mousePosition;
    }

    void MoveZAxis()
    {
        Vector3 delta = Input.mousePosition - lastMousePosition;

        Vector3 newPosition = Camera.main.transform.position - new Vector3(delta.x * Time.deltaTime, 0, delta.y * Time.deltaTime);

        // Clamp the camera's position to the defined boundaries
        newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);
        newPosition.z = Mathf.Clamp(newPosition.z, minZ, maxZ);

        Camera.main.transform.position = newPosition;
        lastMousePosition = Input.mousePosition;
    }
}
