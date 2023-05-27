using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPan : MonoBehaviour
{
    [SerializeField] private Camera cam;
    private Vector3 startPos;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startPos = cam.ScreenToViewportPoint(Input.mousePosition);
        }
        if (Input.GetMouseButton(0) && Time.timeScale > 0f)
        {
            CamPan();
        }
    }

    private void CamPan()
    {
        if (Input.GetAxis("Mouse Y") != 0 || Input.GetAxis("Mouse X") != 0)
        {
            Vector3 direction = startPos - cam.ScreenToViewportPoint(Input.mousePosition);

            cam.transform.position = new Vector3();
            Debug.Log(cam.transform.rotation.eulerAngles.x);
            float nextX = (cam.transform.rotation * Quaternion.Euler(new Vector3(direction.y * 180, 0, 0))).eulerAngles.x;
            if (nextX < 80 || nextX > 280)
            {
                cam.transform.Rotate(new Vector3(1, 0, 0), direction.y * 180);
            }
            cam.transform.Rotate(new Vector3(0, 1, 0), -direction.x * 180, Space.World);
            cam.transform.Translate(0, 0, -15);

            startPos = cam.ScreenToViewportPoint(Input.mousePosition);

        }
    }
}
