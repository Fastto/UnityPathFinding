using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    [SerializeField] private GameObject _object; //An object camera will follow
    [SerializeField] private Vector3 _distanceFromObject; // Camera's distance from the object

    [SerializeField] private Dropdown CameraMode;
    [SerializeField] private SceneController sceneController;

    private Camera camera;

    private bool isTracking;

    private void Start()
    {
        camera = GetComponent<Camera>();
    }

    private void LateUpdate() //Works after all update functions called
    {
        if (isTracking)
        {
            Vector3 positionToGo =
                _object.transform.position - _object.transform.forward * 5; //Target position of the camera
            Vector3 smoothPosition = Vector3.Lerp(a: transform.position, b: positionToGo, t: 0.05F);
            transform.position = smoothPosition + new Vector3(0, .1f, 0);
            transform.LookAt(_object.transform.position); //Camera will look(returns) to the object
        }
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.O) && !camera.orthographic)
        {
            camera.fieldOfView -= 1;
        }

        if (Input.GetKey(KeyCode.P) && !camera.orthographic)
        {
            camera.fieldOfView += 1;
        }

        if (Input.GetKey(KeyCode.O) && camera.orthographic)
        {
            camera.orthographicSize -= 1;
        }

        if (Input.GetKey(KeyCode.P) && camera.orthographic)
        {
            camera.orthographicSize += 1;
        }

        if (Input.GetKey(KeyCode.I))
        {
            camera.transform.position += new Vector3(0, 1, 0);
        }

        if (Input.GetKey(KeyCode.K))
        {
            camera.transform.position -= new Vector3(0, 1, 0);
        }

        if (Input.GetKey(KeyCode.Y))
        {
            camera.transform.position += new Vector3(1, 0, 0);
        }

        if (Input.GetKey(KeyCode.U))
        {
            camera.transform.position -= new Vector3(1, 0, 0);
        }

        if (Input.GetKey(KeyCode.H))
        {
            camera.transform.position += new Vector3(0, 0, 1);
        }

        if (Input.GetKey(KeyCode.J))
        {
            camera.transform.position -= new Vector3(0, 0, 1);
        }
        
        if (Input.GetKey(KeyCode.N))
        {
            camera.transform.position = new Vector3(sceneController.WorldWidth / 2, 5, sceneController.WorldHeight / 2);
            transform.LookAt(new Vector3(sceneController.WorldWidth / 2, 0, sceneController.WorldHeight / 2));
        }

        if (Input.GetKey(KeyCode.M))
        {
            camera.transform.position = new Vector3(-3, 5, -3);
            transform.LookAt(new Vector3(sceneController.WorldWidth / 2, 0, sceneController.WorldHeight / 2));
        }

        if (Input.GetMouseButtonDown(0) && Input.GetKey(KeyCode.B))
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, 50, 1 << 7))
            {
                transform.LookAt(hit.transform.position);
            }
        }
    }

    public void OnCameraModeChanged()
    {
        switch (CameraMode.value)
        {
            case 1:
                camera.orthographic = true;
                isTracking = false;
                break;
            case 2:
                camera.orthographic = false;
                isTracking = true;
                break;
            case 0:
            default:
                camera.orthographic = false;
                isTracking = false;
                break;
        }
    }
}