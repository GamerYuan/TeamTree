using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPan : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private float maxZoom, minZoom, baseZoom, zoomAmp;
    private Vector3 startPos;
    private float currZoom;

    void Start()
    {
        currZoom = baseZoom;
    }

    // Update is called once per frame
    void Update()
    {
        if (!StageManagerBehaviour.isPaused)
        {
            if (Input.GetAxis("Mouse ScrollWheel") != 0)
            {
                Zoom(Input.GetAxis("Mouse ScrollWheel"));
            }
            if (Input.touchCount == 2)
            {
                Touch touchZero = Input.GetTouch(0);
                Touch touchOne = Input.GetTouch(1);

                Vector2 touchZeroDelta = touchZero.position - touchZero.deltaPosition;
                Vector2 touchOneDelta = touchOne.position - touchOne.deltaPosition;

                float deltaMag = (touchZeroDelta - touchOneDelta).magnitude;
                float currMag = (touchZero.position - touchOne.position).magnitude;

                float diff = currMag - deltaMag;

                Zoom(diff * zoomAmp);
            }
            else
            {
                if (Input.GetMouseButtonDown(0))
                {
                    startPos = cam.ScreenToViewportPoint(Input.mousePosition);
                }
                if (Input.GetMouseButton(0))
                {
                    CamPan();
                }
            }
        }
    }

    private void CamPan()
    {
        if (Input.GetAxis("Mouse Y") != 0 || Input.GetAxis("Mouse X") != 0)
        {
            Vector3 direction = startPos - cam.ScreenToViewportPoint(Input.mousePosition);

            cam.transform.position = new Vector3();
            if ((cam.transform.rotation * Quaternion.Euler(new Vector3(direction.y * 180, 0, 0))).eulerAngles.x < 80)
            {
                cam.transform.Rotate(new Vector3(1, 0, 0), direction.y * 180);
            }
            cam.transform.Rotate(new Vector3(0, 1, 0), -direction.x * 180, Space.World);
            cam.transform.Translate(0, 0, currZoom);

            startPos = cam.ScreenToViewportPoint(Input.mousePosition);

        }
    }

    private void Zoom(float increment)
    {
        if (currZoom + increment < minZoom && currZoom + increment > maxZoom)
        {
            cam.transform.Translate(0, 0, increment);
            currZoom += increment;
        }
    }
}
