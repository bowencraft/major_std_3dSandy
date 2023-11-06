using System;
using System.Collections;
using System.Collections.Generic;
using Hypertonic.GridPlacement;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public float distance = 5.0f;
    public float xSpeed = 120.0f;
    public float ySpeed = 120.0f;
    public float moveSpeed = 5.0f;
    //public float dragSpeed = 5.0f;

    public float yMinLimit = 30f;
    public float yMaxLimit = 60f;
    public float snapSpeed = 5f;
    public float snapThreshold = 1f;

    public float snapAngle = 30f;

    private float x = 0.0f;
    private float y = 0.0f;
    private bool dragging = false;

    //private Vector3 dragOrigin;

    void Start()
    {
        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;

        if (GetComponent<Rigidbody>())
        {
            GetComponent<Rigidbody>().freezeRotation = true;
        }
    }

    void Update()
    {
        float scrollWheel = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scrollWheel) > 0.01f)
        {
            Camera.main.orthographicSize -= scrollWheel * 2.0f;
            Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, 1.0f, 50.0f);
            distance -= scrollWheel * 2.0f;
        }


        if (Input.GetMouseButtonDown(0)
            //&& !FindObjectOfType<HexGrid>().isHovering
            && !GridManagerAccessor.GridManager.IsPlacingGridObject
            )
        {
            dragging = true;
        }

        if (Input.GetMouseButtonUp(0))
        {
            dragging = false;
        }

        if (dragging)
        {
            x += Input.GetAxis("Mouse X") * xSpeed * Time.deltaTime;
            y -= Input.GetAxis("Mouse Y") * ySpeed * Time.deltaTime;

            y = ClampAngle(y, yMinLimit, yMaxLimit);

            Quaternion rotation = Quaternion.Euler(y, x, 0);
            Vector3 position = rotation * new Vector3(0.0f, 0.0f, -distance) + target.position;

            transform.rotation = rotation;
            transform.position = position;
        }
        else
        {
            float targetAngle = Mathf.Round(x / snapAngle) * snapAngle;
            x = Mathf.Lerp(x, targetAngle, snapSpeed * Time.deltaTime);

            if (Mathf.Abs(x - targetAngle) < snapThreshold)
            {
                x = targetAngle;
            }

            Quaternion rotation = Quaternion.Euler(y, x, 0);
            Vector3 position = rotation * new Vector3(0.0f, 0.0f, -distance) + target.position;



            transform.rotation = rotation;
            transform.position = position;
        }


        // Camera movement with W, A, S, D, Q, E keys
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        float elevation = 0.0f;

        if (Input.GetKey(KeyCode.Q))
        {
            elevation = -1.0f;
        }
        if (Input.GetKey(KeyCode.E))
        {
            elevation = 1.0f;
        }

        //if (Input.GetMouseButtonDown(1))
        //{
        //    dragOrigin = Input.mousePosition;
        //}
        //else if (Input.GetMouseButton(1))
        //{
        //    Vector3 diff = (dragOrigin - Input.mousePosition) * dragSpeed;
        //    diff.z = 0;
        //    dragOrigin = Input.mousePosition;
        //    target.position += diff * Time.deltaTime;
        //}

        Camera.main.orthographicSize -= elevation * moveSpeed * Time.deltaTime;
        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, 1.0f, 50.0f);

        Vector3 moveDirection = new Vector3(horizontal, vertical, 0);

        //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //RaycastHit hit;
        //Physics.Raycast(ray, out hit);

        target.position += transform.rotation * moveDirection * moveSpeed * Time.deltaTime;
    }

    float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360f)
            angle += 360f;
        if (angle > 360f)
            angle -= 360f;
        return Mathf.Clamp(angle, min, max);
    }
}