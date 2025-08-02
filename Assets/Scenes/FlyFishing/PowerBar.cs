using System;
using System.Numerics;
using UnityEngine;
using UnityEngine.InputSystem;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class PowerBar : MonoBehaviour
{
    // Object Links
    public GameObject bar;
    public GameObject arrow;
    public GameObject lure;
    public GameObject mainCamera;

    // Public Variables
    public float maxForce = 10f;
    public float cameraPadding = 3f;
    public float minimumY = -6.5f;
    public float maximumX = 5f;

    private float movementRange = 192F;
    public float arrowSpeed = 5F;
    private bool stopped;
    private InputAction stopAction;
    private Camera orthoCam;
    private float screenAspect;

    // Bar Segments
    private float redRange = 32F;
    private float orangeRange = 32F;
    private float yellowRange = 16F;
    private float lightGreenRange = 12F;
    private float greenRange = 4F;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        float xScale = bar.transform.localScale.x;
        movementRange = xScale * movementRange / 100 / 2;
        greenRange = xScale * greenRange / 100;
        lightGreenRange = (xScale * lightGreenRange / 100) + greenRange;
        yellowRange = (xScale * yellowRange / 100) + lightGreenRange;
        orangeRange = (xScale * orangeRange / 100) + yellowRange;
        redRange = (xScale * redRange / 100) + orangeRange;
        stopAction = InputSystem.actions.FindAction("General Action");
        stopped = false;
        screenAspect = (float)Screen.width / (float)Screen.height;
        orthoCam = mainCamera.GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (stopped)
        {
            scaleCamera();
            return;
        }
        else if (stopAction.WasPerformedThisFrame())
        {
            Debug.Log("Stop detected");
            stopArrow();
            return;
        }

        arrow.transform.Translate(new Vector3(arrowSpeed * Time.deltaTime, 0));

        // Adjust Arrow in case it runs out of bounds
        if (arrow.transform.localPosition.x > movementRange)
        {
            arrowSpeed *= -1;
            arrow.transform.localPosition = new Vector3(movementRange, arrow.transform.localPosition.y, 0);
        }
        if (arrow.transform.localPosition.x < -movementRange)
        {
            arrowSpeed *= -1;
            arrow.transform.localPosition = new Vector3(-movementRange, arrow.transform.localPosition.y, 0);
        }
    }

    void stopArrow()
    {
        float forcePercentage = 0f;
        float arrowPosition = Math.Abs(arrow.transform.localPosition.x);
        stopped = true;
        if (arrowPosition < greenRange)
        {
            Debug.Log("Green Zone");
            forcePercentage = 1;
        }
        else if (arrowPosition < lightGreenRange)
        {
            Debug.Log("Light Green Zone");
            forcePercentage = .8f;
        }
        else if (arrowPosition < yellowRange)
        {
            Debug.Log("Yellow Zone");
            forcePercentage = .6f;
        }
        else if (arrowPosition < orangeRange)
        {
            Debug.Log("Orange Zone");
            forcePercentage = .4f;
        }
        else if (arrowPosition <= redRange)
        {
            Debug.Log("Red Zone");
            forcePercentage = .2f;
        }

        // Shoot the fishing lure
        lure.GetComponent<Rigidbody2D>().simulated = true;
        Vector2 force = maxForce * forcePercentage * new Vector2(-1, 1).normalized;
        lure.GetComponent<Rigidbody2D>().AddForce(force, ForceMode2D.Impulse);
    }

    void scaleCamera()
    {
        float camHalfHeight = orthoCam.orthographicSize;
        float camHalfWidth = screenAspect * camHalfHeight;

        if (Math.Abs(lure.transform.position.x) > camHalfWidth + mainCamera.transform.position.x - cameraPadding)
        {
            // Camera Calculations
            float newHalfWidth = (maximumX - lure.transform.position.x + cameraPadding) / 2;
            float newHalfHeight = newHalfWidth / screenAspect;

            // Camera Adjustments
            mainCamera.transform.position = new Vector3(maximumX-newHalfWidth, newHalfHeight + minimumY, mainCamera.transform.position.z);
            orthoCam.orthographicSize = newHalfHeight;
        }
    }
}
