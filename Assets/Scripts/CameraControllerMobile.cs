using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControllerMobile : MonoBehaviour
{
    [Header("Mobile Input Panning")]
    private Vector3 touchStart;
    public Camera cam;
    public float groundZ = 0;
    [Header("Mobile Input Pinch To Zoom")]
    public float previousDistance;
    public float zoomSpeed = 1.0f;


    void Start()
    {
        
    }

    void Update()
    {

        //Mobile Input Panning
        if(Input.GetMouseButtonDown(0))
        {
            touchStart = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        if(Input.GetMouseButton(0))
        {
            Vector3 direction = touchStart - Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Camera.main.transform.position += direction;
        }

        //Mobile Input Pinch Zoom
        if (Input.touchCount == 2 && (Input.GetTouch(0).phase == TouchPhase.Began || Input.GetTouch(1).phase == TouchPhase.Began))
        {
            previousDistance = Vector2.Distance(Input.GetTouch(0).position, Input.GetTouch(1).position);
        }
        else if (Input.touchCount == 2 && (Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetTouch(1).phase == TouchPhase.Moved))
        {
            float distance;
            Vector2 touch1 = Input.GetTouch(0).position;
            Vector2 touch2 = Input.GetTouch(1).position;

            distance = Vector2.Distance(touch1, touch2);

            float pinchAmount = (previousDistance - distance) * zoomSpeed * Time.deltaTime;
            Camera.main.transform.Translate(0, 0, pinchAmount);
            previousDistance = distance;
        }

    }

    private Vector3 GetWorldPosition(float z)
    {
        Ray mousePos = cam.ScreenPointToRay(Input.mousePosition);
        Plane ground = new Plane(Vector3.forward, new Vector3(0, 0, z));
        float distance;
        ground.Raycast(mousePos, out distance);
        return mousePos.GetPoint(distance);
    }
}
