using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPan : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private float maxZoom, minZoom, baseZoom, zoomAmp, moveBox;
    private Vector3 startPos;
    private float currZoom;
    private Vector3 orbitCenter, moveStart;
    private bool isMove;
    private float fingerDist;

    void Start()
    {
        currZoom = baseZoom;
        orbitCenter = Vector3.zero;
        fingerDist = Screen.currentResolution.height / 15f;
    }

    // Update is called once per frame
    void Update()
    {
        if (!StageManagerBehaviour.isPaused)
        {
            if (Input.touchCount == 0)
            {
                isMove = false;
            }
            if (Input.GetAxis("Mouse ScrollWheel") != 0)
            {
                Zoom(Input.GetAxis("Mouse ScrollWheel"));
            }
            if (Input.touchCount == 2)
            {
                Touch touchZero = Input.GetTouch(0);
                Touch touchOne = Input.GetTouch(1);

                if (Mathf.Abs(touchZero.position.y - touchOne.position.y) <= fingerDist)
                {
                    moveStart = GetWorldPoint(touchZero.position - touchZero.deltaPosition);

                    if (touchZero.phase == TouchPhase.Moved)
                    {
                        isMove = true;
                        CamMove(touchZero.position);
                    }
                }
                else if (!isMove)
                {
                    Vector2 touchZeroDelta = touchZero.position - touchZero.deltaPosition;
                    Vector2 touchOneDelta = touchOne.position - touchOne.deltaPosition;

                    float deltaMag = (touchZeroDelta - touchOneDelta).magnitude;
                    float currMag = (touchZero.position - touchOne.position).magnitude;

                    float diff = currMag - deltaMag;

                    Zoom(diff * zoomAmp * Time.deltaTime);
                }                
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
                if (Input.GetMouseButtonDown(1))
                {
                    moveStart = GetWorldPoint(Input.mousePosition);
                }
                if (Input.GetMouseButton(1))
                {
                    CamMove(Input.mousePosition);
                }
            }
        }
    }

    private void CamPan()
    {
        if (Input.GetAxis("Mouse Y") != 0 || Input.GetAxis("Mouse X") != 0)
        {
            Vector3 direction = startPos - cam.ScreenToViewportPoint(Input.mousePosition);

            cam.transform.position = orbitCenter;
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

    private void CamMove(Vector3 pos)
    {
        if (Input.GetAxis("Mouse Y") != 0 || Input.GetAxis("Mouse X") != 0)
        {
            Vector3 diff = (moveStart - GetWorldPoint(pos));
            Vector3 move = diff * Time.deltaTime * 100;
            if (BoundaryCheck(orbitCenter + move))
            {
                orbitCenter += move;
                cam.transform.position += move;
                moveStart = GetWorldPoint(pos);
            }
        }   
    }

    private Vector3 GetWorldPoint(Vector3 pos)
    {
        Ray mousePos = cam.ScreenPointToRay(pos);
        Plane plane = new Plane(Vector3.Normalize(cam.transform.position), orbitCenter);
        float distance;
        plane.Raycast(mousePos, out distance);
        return mousePos.GetPoint(distance);
    }

    private bool BoundaryCheck(Vector3 pos)
    {
        return Mathf.Abs(pos.x) <= moveBox && Mathf.Abs(pos.y) <= moveBox && Mathf.Abs(pos.z) <= moveBox;
    }
}
